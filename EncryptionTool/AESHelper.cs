using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Input;

namespace EncryptionTool
{
    public class AESHelper : IEncryptor
    {
        private Aes aesAlgorithm;
        public byte[] IV;

        private byte[] key;
        public byte[] Key
        {
            get => key;
            set { if (key != value) key = value; }
        }
        public AESHelper(string key)
        {
            Key = Convert.FromBase64String(key);
        }

        public bool EncryptString(string msg)
        {
            using(var aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = Key;
                aesAlgorithm.GenerateIV();
                IV = aesAlgorithm.IV;
                ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor();
                byte[] encryptedData;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(msg);
                        }
                        encryptedData = ms.ToArray();
                    }
                }
                return StorageHelper.SaveFile(Convert.ToBase64String(encryptedData));
            }
        }

        public string DecryptString(string encryptedMsg)
        {
            using (var aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = Key;
                aesAlgorithm.IV = IV;
                string message = string.Empty;
                ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();
                byte[] cipher = Convert.FromBase64String(encryptedMsg);
                using (MemoryStream ms = new MemoryStream(cipher))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            message += sr.ReadToEnd();
                        }
                    }
                }
                return message;
            }
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
