using API.Controllers.Database.Administration.Table.Password;
using API.Controllers.Database.Administration.Table.User;
using API.Controllers.Database.Mapping.Table;
using API.Entity.Code;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Controllers.Code
{
    public class ProcessBrokerLoginController : WebsiteRequestController
    {
        public ProcessBrokerLoginController()
        {

        }

        internal IActionResult ProcessBrokerLogin(JObject json)
        {
            var securityController = new SecurityController();
            var userAttributeController = new UserAttributeController();
            var userDetailController = new UserDetailController();

            var processBrokerLoginEntity = JsonConvert.DeserializeObject<ProcessBrokerLoginEntity>(json.ToString());

            var decryptedUsername = securityController.Decrypt(processBrokerLoginEntity.Username, processBrokerLoginEntity.iv_username);

            /*
             * Check username
             * If username is valid, check if account is locked
             * Check password
             * Check combination
             * Check user is broker or internal
             * Store json
             * Return result
             */

            long userId = 0;
            var emailAddressUserAttributeEntity = userAttributeController.GetActiveEntityByDescription("Email Address");
            var emailAddressUserDetailEntity = userDetailController.GetActiveEntityByAttributeIdAndDescription(emailAddressUserAttributeEntity.Id, decryptedUsername);

            if (emailAddressUserDetailEntity != null)
            {
                var isAccountLockedUserAttributeEntity = userAttributeController.GetActiveEntityByDescription("Is Account Locked?");
                var isAccountLockedUserDetailEntity = userDetailController.GetActiveEntityByIdAndAttributeId(emailAddressUserDetailEntity.UserId, isAccountLockedUserAttributeEntity.Id);

                if (isAccountLockedUserDetailEntity == null)
                {
                    userId = emailAddressUserDetailEntity.UserId;
                }
            }

            if (userId == 0)
            {
                return Ok(new { authenticated = false });
            }

            var administration_Password_To_Administration_UserController = new Administration_Password_To_Administration_UserController();
            var administration_Password_To_Administration_UserEntity = administration_Password_To_Administration_UserController.GetActiveEntityByUserId(userId);

            if (administration_Password_To_Administration_UserEntity == null)
            {
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
                return Ok(new { authenticated = false });
            }

            return Ok(new { authenticated = true });
        }
    }
}
