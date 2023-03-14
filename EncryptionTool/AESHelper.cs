using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionTool
{
    public class AESHelper : IEncryptor
    {
        private byte[] key;
        public byte[] Key
        {
            get => key;
            set { if (key != value) key = value; }
        }
        private byte[] iv;
        public byte[] Iv
        {
            get => iv;
            set { if (iv != value) iv = value; }
        }
        public AESHelper(byte[] key, byte[] iv)
        {
            Key = key;
            Iv = iv;
        }
        public void Encrypt(byte[] data)
        {
            Aes aesAlgorithm = Aes.Create();
            aesAlgorithm.Key = Key;
            aesAlgorithm.IV = Iv;
            ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor();
        }

        public string EncryptString(string msg, out string keyBase64, out string vectorBase64)
        {
            Aes aesAlgorithm = Aes.Create();
            aesAlgorithm.Key = Key;
            aesAlgorithm.IV = Iv;
            keyBase64 = Convert.ToBase64String(aesAlgorithm.Key);
            vectorBase64 = Convert.ToBase64String(aesAlgorithm.IV);
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

        public byte[] Decrypt(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
