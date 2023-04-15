using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace EncryptionTool
{
    public static class KeyHelper
    {
        public static void GenerateAesKey(string keyName)
        {
            var path = StorageHelper.SetDefaultFolder();
            byte[] aesKey;
            // Generate AES key and IV
            using (var aes = Aes.Create())
            {
                aes.GenerateKey();
                aesKey = aes.Key;
            }
            // Save AES key and IV to file
            string aesKeyFilePath = Path.Combine(path, $"{keyName}.txt");
            string aesKeyBase64 = Convert.ToBase64String(aesKey);
            File.WriteAllText(aesKeyFilePath, aesKeyBase64);
        }

        public static string GetAesKey() => StorageHelper.GetFile(true);
        

        public static void GenerateRsaKey(string keyName, int keySize)
        {
            var path = StorageHelper.SetDefaultFolder();
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize);
            string privateKeyXml = rsa.ToXmlString(true);
            string publicKeyXml = rsa.ToXmlString(false);

            // Save RSA key pair to file
            string rsaPrivateFilePath = Path.Combine(path, $"{keyName}_Private.xml");
            string rsaPublicFilePath = Path.Combine(path, $"{keyName}_Public.xml");
            File.WriteAllText(rsaPrivateFilePath, privateKeyXml);
            File.WriteAllText(rsaPublicFilePath, publicKeyXml);

        }

        public static string GetRsaPrivateKey(string keyName)
        {
            var path = StorageHelper.SetDefaultFolder();
            string rsaPrivateFilePath = Path.Combine(path, $"{keyName}_Private.xml");
            return rsaPrivateFilePath;
        }

        public static string GetRsaPublicKey(string keyName)
        {
            var path = StorageHelper.SetDefaultFolder();
            string rsaPublicFilePath = Path.Combine(path, $"{keyName}_Public.xml");
            return rsaPublicFilePath;
        }
    }
}
