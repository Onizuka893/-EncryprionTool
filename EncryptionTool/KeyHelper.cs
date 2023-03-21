using System.IO;
using System.Security.Cryptography;
using System.Text;

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
    }
}
