using System.Security.Cryptography;
using System.Text;

namespace API.Controllers.Code
{
    public class SecurityController
    {
        private readonly byte[] Key = Encoding.UTF8.GetBytes("32-byte-long-secret-key-here-12!");

        public SecurityController()
        {

        }

        public string Decrypt(string base64CipherText, string base64IV)
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
    }
}
