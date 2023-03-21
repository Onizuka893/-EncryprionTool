using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Input;

namespace EncryptionTool
{
    public class AESHelper : IEncryptor
    {
        private Aes aesAlgorithm;
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

        public string EncryptString(string msg)
        {
            using(var aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = Key;
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
                return Convert.ToBase64String(encryptedData);
            }
        }

        public string DecryptString(string encryptedMsg)
        {
            string message = string.Empty;
            ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();
            byte[] cipher = Convert.FromBase64String(encryptedMsg);
            using (MemoryStream ms = new MemoryStream(cipher))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        message += $"messagedecrypt : {sr.ReadToEnd()}\n";
                    }
                }
            }
            return message;
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
