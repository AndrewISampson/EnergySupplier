using API.Controllers.Database.Administration.Table.Login;
using API.Controllers.Database.Administration.Table.Password;
using API.Controllers.Database.Administration.Table.User;
using API.Controllers.Database.Mapping.Table;
using API.Entity.Code;
using API.Entity.Database.Administration.Table.Login;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Controllers.Code
{
    public class ProcessBrokerLoginController : WebsiteRequestController
    {
        private readonly LoginDetailController _loginDetailController = new();

        public ProcessBrokerLoginController()
        {

        }

        internal IActionResult ProcessBrokerLogin(JObject json)
        {
            var processBrokerLoginEntity = JsonConvert.DeserializeObject<ProcessBrokerLoginEntity>(json.ToString());

            var loginController = new LoginController();
            var loginEntity = loginController.InsertNewAndGetEntity();

            var loginAttributeController = new LoginAttributeController();
            var loginAttributeDescriptionToIdDictionary = loginAttributeController.GetActiveEntityList().ToDictionary
                (
                    l => l.Description,
                    l => l.Id
                );

            var loginDetailEntityList = new List<LoginDetailEntity>
            {
                new() { LoginAttributeId = loginAttributeDescriptionToIdDictionary["Username"], Description = processBrokerLoginEntity.Username },
                new() { LoginAttributeId = loginAttributeDescriptionToIdDictionary["Password"], Description = processBrokerLoginEntity.Password },
                new() { LoginAttributeId = loginAttributeDescriptionToIdDictionary["IV Username"], Description = processBrokerLoginEntity.iv_username },
                new() { LoginAttributeId = loginAttributeDescriptionToIdDictionary["IV Password"], Description = processBrokerLoginEntity.iv_password },
                new() { LoginAttributeId = loginAttributeDescriptionToIdDictionary["Browser"], Description = processBrokerLoginEntity.client_metadata.browser },
                new() { LoginAttributeId = loginAttributeDescriptionToIdDictionary["IP Address"], Description = processBrokerLoginEntity.client_metadata.ip_address },
                new() { LoginAttributeId = loginAttributeDescriptionToIdDictionary["Accept Language"], Description = processBrokerLoginEntity.client_metadata.accept_language },
                new() { LoginAttributeId = loginAttributeDescriptionToIdDictionary["Referer"], Description = processBrokerLoginEntity.client_metadata.referer },
                new() { LoginAttributeId = loginAttributeDescriptionToIdDictionary["Host"], Description = processBrokerLoginEntity.client_metadata.host },
                new() { LoginAttributeId = loginAttributeDescriptionToIdDictionary["Request Method"], Description = processBrokerLoginEntity.client_metadata.request_method },
                new() { LoginAttributeId = loginAttributeDescriptionToIdDictionary["Query String"], Description = processBrokerLoginEntity.client_metadata.query_string },
                new() { LoginAttributeId = loginAttributeDescriptionToIdDictionary["Session Key"], Description = processBrokerLoginEntity.client_metadata.session_key.ToString() },
                new() { LoginAttributeId = loginAttributeDescriptionToIdDictionary["CSRF Token"], Description = processBrokerLoginEntity.client_metadata.cookies.csrftoken },
            };
            loginDetailEntityList.ForEach(l => l.LoginId = loginEntity.Id);

            _loginDetailController.BulkInsert(loginDetailEntityList);

            var securityController = new SecurityController();
            var decryptedUsername = securityController.Decrypt(processBrokerLoginEntity.Username, processBrokerLoginEntity.iv_username);

            var userAttributeController = new UserAttributeController();
            var emailAddressUserAttributeEntity = userAttributeController.GetActiveEntityByDescription("Email Address");

            var userDetailController = new UserDetailController();
            var emailAddressUserDetailEntity = userDetailController.GetActiveEntityByAttributeIdAndDescription(emailAddressUserAttributeEntity.Id, decryptedUsername);

            if (emailAddressUserDetailEntity == null)
            {
                LogLoginFailureAndCheckAccountLock(loginEntity.Id, loginAttributeDescriptionToIdDictionary, "Email Address not found", 0);
                return Ok(new { authenticated = false });
            }

            var isAccountLockedUserAttributeEntity = userAttributeController.GetActiveEntityByDescription("Is Account Locked?");
            var isAccountLockedUserDetailEntity = userDetailController.GetActiveEntityByIdAndAttributeId(emailAddressUserDetailEntity.UserId, isAccountLockedUserAttributeEntity.Id);

            if (isAccountLockedUserDetailEntity != null)
            {
                LogLoginFailureAndCheckAccountLock(loginEntity.Id, loginAttributeDescriptionToIdDictionary, "Account is locked", emailAddressUserDetailEntity.UserId);
                return Ok(new { authenticated = false });
            }

            var roleUserAttributeEntity = userAttributeController.GetActiveEntityByDescription("Role");
            var roleUserDetailEntityList = userDetailController.GetActiveEntityListByIdAndAttributeId(emailAddressUserDetailEntity.UserId, roleUserAttributeEntity.Id);

            if (!roleUserDetailEntityList.Select(u => u.Description).Contains("Broker")
                && !roleUserDetailEntityList.Select(u => u.Description).Contains("Internal"))
            {
                LogLoginFailureAndCheckAccountLock(loginEntity.Id, loginAttributeDescriptionToIdDictionary, "Account is neither broker nor internal", emailAddressUserDetailEntity.UserId);
                return Ok(new { authenticated = false });
            }

            var administration_Password_To_Administration_UserController = new Administration_Password_To_Administration_UserController();
            var administration_Password_To_Administration_UserEntity = administration_Password_To_Administration_UserController.GetActiveEntityByUserId(emailAddressUserDetailEntity.UserId);

            if (administration_Password_To_Administration_UserEntity == null)
            {
                LogLoginFailureAndCheckAccountLock(loginEntity.Id, loginAttributeDescriptionToIdDictionary, "Account has no password", emailAddressUserDetailEntity.UserId);
                return Ok(new { authenticated = false });
            }

            var passwordId = administration_Password_To_Administration_UserEntity.PasswordId;

            var passwordAttributeController = new PasswordAttributeController();
            var base64CipherTextPasswordAttributeEntity = passwordAttributeController.GetActiveEntityByDescription("Base64CipherText");
            var base64IVPasswordAttributeEntity = passwordAttributeController.GetActiveEntityByDescription("Base64IV");

            var passwordDetailController = new PasswordDetailController();
            var base64CipherTextPasswordDetailEntity = passwordDetailController.GetActiveEntityByIdAndAttributeId(passwordId, base64CipherTextPasswordAttributeEntity.Id);
            var base64IVPasswordDetailEntity = passwordDetailController.GetActiveEntityByIdAndAttributeId(passwordId, base64IVPasswordAttributeEntity.Id);

            var decryptedExistingPassword = securityController.Decrypt(base64CipherTextPasswordDetailEntity.Description, base64IVPasswordDetailEntity.Description);
            var decryptedPassword = securityController.Decrypt(processBrokerLoginEntity.Password, processBrokerLoginEntity.iv_password);

            if (decryptedExistingPassword != decryptedPassword)
            {
                LogLoginFailureAndCheckAccountLock(loginEntity.Id, loginAttributeDescriptionToIdDictionary, "Incorrect password", emailAddressUserDetailEntity.UserId);
                return Ok(new { authenticated = false });
            }

            var administration_Login_To_Administration_UserController = new Administration_Login_To_Administration_UserController();
            administration_Login_To_Administration_UserController.Insert(loginEntity.Id, emailAddressUserDetailEntity.UserId);
            _loginDetailController.Insert(loginEntity.Id, loginAttributeDescriptionToIdDictionary["Login Result"], "Succcess");
            return Ok(new { authenticated = true });
        }

        private void LogLoginFailureAndCheckAccountLock(long loginId, Dictionary<string, long> loginAttributeDescriptionToIdDictionary, string failureReason, long userId)
        {
            _loginDetailController.Insert(loginId, loginAttributeDescriptionToIdDictionary["Login Result"], "Failed");
            _loginDetailController.Insert(loginId, loginAttributeDescriptionToIdDictionary["Login Failure Reason"], failureReason);

            if (userId != 0)
            {
                var administration_Login_To_Administration_UserController = new Administration_Login_To_Administration_UserController();
                administration_Login_To_Administration_UserController.Insert(loginId, userId);

                var loginIdList = administration_Login_To_Administration_UserController.GetActiveEntityListByUserId(userId)
                    .OrderByDescending(l => l.Id)
                    .Select(l => l.LoginId)
                    .Take(5)
                    .ToList();

                if (loginIdList.Count < 5)
                {
                    return;
                }

                var loginResultList = loginIdList.Select(l => _loginDetailController.GetActiveEntityByIdAndAttributeId(l, loginAttributeDescriptionToIdDictionary["Login Result"]))
                    .Select(l => l.Description)
                    .Distinct()
                    .ToList();

                if (!loginResultList.Contains("Success"))
                {
                    var userAttributeController = new UserAttributeController();
                    var isAccountLockedUserAttributeEntity = userAttributeController.GetActiveEntityByDescription("Is Account Locked?");

                    var userDetailController = new UserDetailController();
                    userDetailController.Insert(userId, isAccountLockedUserAttributeEntity.Id, "1");
                }
            }
        }
    }
}