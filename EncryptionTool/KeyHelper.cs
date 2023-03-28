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

        public static string GetAesKey()
         => StorageHelper.GetFile(true);
        {
            var path = StorageHelper.SetDefaultFolder();
            string aesKeyFilePath = Path.Combine(path, "aesKey.txt");
            return File.ReadAllText(aesKeyFilePath, Encoding.UTF8);
        }

        public static void GenerateRsaKey(string keyName)
        {
            var path = StorageHelper.SetDefaultFolder();
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
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

            //// Read the private key from its file
            //string privateKeyXml = File.ReadAllText(rsaPrivateFilePath);

            //// Create an instance of the RSA algorithm and import the private key
            //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //rsa.FromXmlString(privateKeyXml);

            //return rsa;
        }

        public static string GetRsaPublicKey(string keyName)
        {
            var path = StorageHelper.SetDefaultFolder();
            string rsaPublicFilePath = Path.Combine(path, $"{keyName}_Public.xml");
            return rsaPublicFilePath;

            //// Read the public key from its file
            //string publicKeyXml = File.ReadAllText(rsaPublicFilePath);

            //// Create an instance of the RSA algorithm and import the public key
            //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //rsa.FromXmlString(publicKeyXml);

            //return rsa;
        }
    }
}
