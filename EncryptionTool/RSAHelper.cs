using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;

namespace EncryptionTool
{
    public class RSAHelper : IEncryptor
    {

        private RSACryptoServiceProvider rsa;

        public RSAHelper(string publicKeyFilePath, string privateKeyFilePath)
        {
            rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(File.ReadAllText(publicKeyFilePath));
            rsa.FromXmlString(File.ReadAllText(privateKeyFilePath));
        }

        public bool EncryptString(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherTextBytes = rsa.Encrypt(plainTextBytes, false);
            string cipherTextBase64 = Convert.ToBase64String(cipherTextBytes);
            return StorageHelper.SaveFile(cipherTextBase64);
        }

        public string DecryptString(string cipherText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            byte[] plainTextBytes = rsa.Decrypt(cipherTextBytes, false);
            string plainText = Encoding.UTF8.GetString(plainTextBytes);
            return plainText;
        }

        public string DecryptImage(byte[] data)
        {
            throw new NotImplementedException();
        }

        public string EncryptImage(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
