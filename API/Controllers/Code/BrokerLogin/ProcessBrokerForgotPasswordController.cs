using API.Controllers.Database.Administration.Table.Password;
using API.Controllers.Database.Administration.Table.User;
using API.Controllers.Database.Administration.Table.ValidationCode;
using API.Controllers.Database.Information.Table.Setting;
using API.Controllers.Database.Mapping.Table;
using API.Entity.Code;
using API.Entity.Database.Administration.Table.User;
using API.Entity.Database.Information.Table.Setting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Controllers.Code.BrokerLogin
{
    public class ProcessBrokerForgotPasswordController : WebsiteRequestController
    {
        private readonly SecurityController _securityController = new();
        private readonly SettingAttributeController _settingAttributeController = new();
        private readonly SettingDetailController _settingDetailController = new();

        private readonly SettingAttributeEntity _nameAccountSettingAttributeEntity;
        private readonly SettingAttributeEntity _valueAccountSettingAttributeEntity;

        private readonly SettingDetailEntity _defaultErrorMessageNameSettingDetailEntity;
        private readonly SettingDetailEntity _defaultErrorMessageValueSettingDetailEntity;

        public ProcessBrokerForgotPasswordController()
        {
            _nameAccountSettingAttributeEntity = _settingAttributeController.GetActiveEntityByDescription("Name");
            _valueAccountSettingAttributeEntity = _settingAttributeController.GetActiveEntityByDescription("Value");

            _defaultErrorMessageNameSettingDetailEntity = _settingDetailController.GetActiveEntityByAttributeIdAndDescription(_nameAccountSettingAttributeEntity.Id, "Default Error Message");
            _defaultErrorMessageValueSettingDetailEntity = _settingDetailController.GetActiveEntityByIdAndAttributeId(_defaultErrorMessageNameSettingDetailEntity.SettingId, _valueAccountSettingAttributeEntity.Id);
        }

        internal OkObjectResult ProcessBrokerForgotPassword(JObject json)
        {
            try
            {
                var processBrokerForgotPasswordEntity = JsonConvert.DeserializeObject<ProcessBrokerForgotPasswordEntity>(json.ToString());

                return processBrokerForgotPasswordEntity.Step switch
                {
                    "email" => SendValidationCode(processBrokerForgotPasswordEntity),
                    "code" => ValidateValidationCode(processBrokerForgotPasswordEntity),
                    "reset" => ResetPassword(processBrokerForgotPasswordEntity),
                    _ => Ok(new { valid = false, message = _defaultErrorMessageValueSettingDetailEntity.Description })
                };
            }
            catch (Exception)
            {
                return Ok(new { valid = false, message = _defaultErrorMessageValueSettingDetailEntity.Description });
            }
        }

        private OkObjectResult SendValidationCode(ProcessBrokerForgotPasswordEntity processBrokerForgotPasswordEntity)
        {
            try
            {
                var emailAddressUserDetailEntity = GetEmailAddressUserDetailEntity(processBrokerForgotPasswordEntity);

                var forgotPasswordValidationCodeSentMessageNameSettingDetailEntity = _settingDetailController.GetActiveEntityByAttributeIdAndDescription(_nameAccountSettingAttributeEntity.Id, "Forgot Password Validation Code Sent Message");
                var forgotPasswordValidationCodeSentMessageValueSettingDetailEntity = _settingDetailController.GetActiveEntityByIdAndAttributeId(forgotPasswordValidationCodeSentMessageNameSettingDetailEntity.SettingId, _valueAccountSettingAttributeEntity.Id);

                if (emailAddressUserDetailEntity == null)
                {
                    return Ok(new { valid = true, message = forgotPasswordValidationCodeSentMessageValueSettingDetailEntity.Description });
                }

                var validationCodeId = _securityController.GenerateForgotPasswordValidationCode();

                var administration_User_To_Administration_ValidationCodeController = new Administration_User_To_Administration_ValidationCodeController();
                administration_User_To_Administration_ValidationCodeController.Insert(emailAddressUserDetailEntity.UserId, validationCodeId);

                /*
                 * SEND EMAIL!
                 */

                return Ok(new { valid = true, message = forgotPasswordValidationCodeSentMessageValueSettingDetailEntity.Description });
            }
            catch (Exception)
            {
                return Ok(new { valid = false, message = _defaultErrorMessageValueSettingDetailEntity.Description });
            }
        }

        private OkObjectResult ValidateValidationCode(ProcessBrokerForgotPasswordEntity processBrokerForgotPasswordEntity)
        {
            try
            {
                var emailAddressUserDetailEntity = GetEmailAddressUserDetailEntity(processBrokerForgotPasswordEntity);

                var forgotPasswordInvalidValidationCodeMessageNameSettingDetailEntity = _settingDetailController.GetActiveEntityByAttributeIdAndDescription(_nameAccountSettingAttributeEntity.Id, "Forgot Password Invalid Validation Code Message");
                var forgotPasswordInvalidValidationCodeMessageValueSettingDetailEntity = _settingDetailController.GetActiveEntityByIdAndAttributeId(forgotPasswordInvalidValidationCodeMessageNameSettingDetailEntity.SettingId, _valueAccountSettingAttributeEntity.Id);

                if (emailAddressUserDetailEntity == null)
                {
                    return Ok(new { valid = false, message = forgotPasswordInvalidValidationCodeMessageValueSettingDetailEntity.Description });
                }

                var decryptedValidationCode = _securityController.Decrypt(processBrokerForgotPasswordEntity.ValidationCode, processBrokerForgotPasswordEntity.iv_ValidationCode);

                var validationCodeAttributeController = new ValidationCodeAttributeController();
                var codeValidationCodeAttributeEntity = validationCodeAttributeController.GetActiveEntityByDescription("Code");

                var validationCodeDetailController = new ValidationCodeDetailController();
                var codeValidationCodeDetailEntity = validationCodeDetailController.GetActiveEntityByAttributeIdAndDescription(codeValidationCodeAttributeEntity.Id, decryptedValidationCode);

                if (codeValidationCodeDetailEntity == null)
                {
                    return Ok(new { valid = false, message = forgotPasswordInvalidValidationCodeMessageValueSettingDetailEntity.Description });
                }

                var administration_User_To_Administration_ValidationCodeController = new Administration_User_To_Administration_ValidationCodeController();
                var administration_User_To_Administration_ValidationCodeEntity = administration_User_To_Administration_ValidationCodeController.GetActiveEntityByUserIdAndValidationCodeId(emailAddressUserDetailEntity.UserId, codeValidationCodeDetailEntity.ValidationCodeId);

                if (administration_User_To_Administration_ValidationCodeEntity == null)
                {
                    return Ok(new { valid = false, message = forgotPasswordInvalidValidationCodeMessageValueSettingDetailEntity.Description });
                }

                return Ok(new { valid = true, message = string.Empty });
            }
            catch (Exception)
            {
                return Ok(new { valid = false, message = _defaultErrorMessageValueSettingDetailEntity.Description });
            }
        }

        private OkObjectResult ResetPassword(ProcessBrokerForgotPasswordEntity processBrokerForgotPasswordEntity)
        {
            try
            {
                var emailAddressUserDetailEntity = GetEmailAddressUserDetailEntity(processBrokerForgotPasswordEntity);

                var invalidNewPasswordMessageNameSettingDetailEntity = _settingDetailController.GetActiveEntityByAttributeIdAndDescription(_nameAccountSettingAttributeEntity.Id, "Invalid New Password Message");
                var invalidNewPasswordMessageValueSettingDetailEntity = _settingDetailController.GetActiveEntityByIdAndAttributeId(invalidNewPasswordMessageNameSettingDetailEntity.SettingId, _valueAccountSettingAttributeEntity.Id);

                if (emailAddressUserDetailEntity == null)
                {
                    return Ok(new { valid = false, message = invalidNewPasswordMessageValueSettingDetailEntity.Description });
                }

                var decryptedPassword = _securityController.Decrypt(processBrokerForgotPasswordEntity.Password, processBrokerForgotPasswordEntity.iv_Password);

                var upperLetterList = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                var lowerLetterList = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
                var numberList = Enumerable.Range(0, 10).Select(n => n.ToString()).ToList();
                var symbolList = new List<string> { "!", "%", "&", "*", "@", "?" };

                var upperLetterExists = decryptedPassword.Any(p => upperLetterList.Contains(p.ToString()));
                var lowerLetterExists = decryptedPassword.Any(p => lowerLetterList.Contains(p.ToString()));
                var numberExists = decryptedPassword.Any(p => numberList.Contains(p.ToString()));
                var symbolExists = decryptedPassword.Any(p => symbolList.Contains(p.ToString()));
                var validPassword = upperLetterExists && lowerLetterExists && numberExists && symbolExists;

                if (!validPassword)
                {
                    return Ok(new { valid = false, message = invalidNewPasswordMessageValueSettingDetailEntity.Description });
                }

                var passwordAttributeController = new PasswordAttributeController();
                var base64CipherTextPasswordAttributeEntity = passwordAttributeController.GetActiveEntityByDescription("Base64CipherText");
                var base64IVPasswordAttributeEntity = passwordAttributeController.GetActiveEntityByDescription("Base64IV");

                var passwordDetailController = new PasswordDetailController();
                var base64CipherTextPasswordDetailEntityList = passwordDetailController.GetActiveEntityListByAttributeId(base64CipherTextPasswordAttributeEntity.Id);
                var base64IVPasswordDetailEntityList = passwordDetailController.GetActiveEntityListByAttributeId(base64IVPasswordAttributeEntity.Id);

                var base64CipherTextPasswordDetailDictionary = base64CipherTextPasswordDetailEntityList.ToDictionary
                    (
                        p => p.Description,
                        p => p.PasswordId
                    );
                var base64IVPasswordDetailDictionary = base64IVPasswordDetailEntityList.ToDictionary
                    (
                        p => p.Description,
                        p => p.PasswordId
                    );

                var passwordDictionary = base64CipherTextPasswordDetailDictionary.ToDictionary
                    (
                        d => _securityController.Decrypt(d.Key, base64IVPasswordDetailDictionary.First(p => p.Value == d.Value).Key),
                        d => d.Value
                    );

                var passwordId = passwordDictionary.TryGetValue(decryptedPassword, out long value) ? value : 0;

                if (passwordId == 0)
                {
                    var passwordController = new PasswordController();
                    var passwordEntity = passwordController.InsertNewAndGetEntity();
                    passwordId = passwordEntity.Id;

                    passwordDetailController.Insert(emailAddressUserDetailEntity.UserId, passwordId, base64CipherTextPasswordAttributeEntity.Id, processBrokerForgotPasswordEntity.Password);
                    passwordDetailController.Insert(emailAddressUserDetailEntity.UserId, passwordId, base64IVPasswordAttributeEntity.Id, processBrokerForgotPasswordEntity.iv_Password);
                }

                var administration_Password_To_Administration_UserController = new Administration_Password_To_Administration_UserController();
                var administration_Password_To_Administration_UserEntity = administration_Password_To_Administration_UserController.GetActiveEntityByUserId(emailAddressUserDetailEntity.UserId);

                if(administration_Password_To_Administration_UserEntity != null)
                {
                    administration_Password_To_Administration_UserController.UpdateEffectiveToDateTime(administration_Password_To_Administration_UserEntity.Id, emailAddressUserDetailEntity.UserId);
                }

                administration_Password_To_Administration_UserController.Insert(emailAddressUserDetailEntity.UserId, passwordId, emailAddressUserDetailEntity.UserId);

                return Ok(new { valid = true, message = string.Empty });
            }
            catch (Exception)
            {
                return Ok(new { valid = false, message = _defaultErrorMessageValueSettingDetailEntity.Description });
            }
        }

        private UserDetailEntity GetEmailAddressUserDetailEntity(ProcessBrokerForgotPasswordEntity processBrokerForgotPasswordEntity)
        {
            var decryptedUsername = _securityController.Decrypt(processBrokerForgotPasswordEntity.Username, processBrokerForgotPasswordEntity.iv_username);

            var userAttributeController = new UserAttributeController();
            var emailAddressUserAttributeEntity = userAttributeController.GetActiveEntityByDescription("Email Address");

            var userDetailController = new UserDetailController();
            return userDetailController.GetActiveEntityByAttributeIdAndDescription(emailAddressUserAttributeEntity.Id, decryptedUsername);
        }
    }
}
