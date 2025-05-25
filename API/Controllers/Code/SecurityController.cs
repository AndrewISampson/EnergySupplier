using System.Security.Cryptography;
using System.Text;
using API.Controllers.Database.Administration.Table.ValidationCode;

namespace API.Controllers.Code
{
    public class SecurityController
    {
        private readonly byte[] Key = Encoding.UTF8.GetBytes("32-byte-long-secret-key-here-12!");

        public SecurityController()
        {

        }

        internal string Decrypt(string base64CipherText, string base64IV)
        {
            byte[] cipherBytes = Convert.FromBase64String(base64CipherText);
            byte[] iv = Convert.FromBase64String(base64IV);

            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            using var ms = new MemoryStream(cipherBytes);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var reader = new StreamReader(cs);
            return reader.ReadToEnd();
        }

        internal long GenerateForgotPasswordValidationCode(long userId)
        {
            var guid = Guid.NewGuid();
            var baseCode = guid.ToString().Replace("-", string.Empty);
            var timeStamp = DateTime.UtcNow.Ticks;
            var validationCode = $"{baseCode}{timeStamp}";

            var validationCodeController = new ValidationCodeController();
            var validationCodeEntity = validationCodeController.InsertNewAndGetEntity(userId);

            var validationCodeAttributeController = new ValidationCodeAttributeController();
            var codeAccountSettingAttributeEntity = validationCodeAttributeController.GetActiveEntityByDescription("Code");

            var validationCodeDetailController = new ValidationCodeDetailController();
            validationCodeDetailController.Insert(userId, validationCodeEntity.Id, codeAccountSettingAttributeEntity.Id, validationCode);

            return validationCodeEntity.Id;
        }
    }
}
