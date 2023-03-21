using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
            string aesKeyFilePath = Path.Combine(path, keyName);
            string aesKeyBase64 = Convert.ToBase64String(aesKey);
            File.WriteAllText(aesKeyFilePath, aesKeyBase64);
        }

        public static string GetAesKey()
        {
            var path = StorageHelper.SetDefaultFolder();
            string aesKeyFilePath = Path.Combine(path, "aesKey.txt");
            return File.ReadAllText(aesKeyFilePath, Encoding.UTF8);
        }
    }
}
