using System;
using System.IO;
using System.Reflection.PortableExecutable;
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
                string obfuscatedString = ObfuscationHelper.ObfuscateFileContent(Convert.ToBase64String(encryptedData), Convert.ToBase64String(IV));
                return StorageHelper.SaveFile(obfuscatedString);
            }
        }

        public string DecryptString(string encryptedMsg)
        {
            var msgAndIV = ObfuscationHelper.DeObfuscateFileContent(encryptedMsg);
            using (var aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = Key;
                aesAlgorithm.IV = Convert.FromBase64String(msgAndIV[1]);
                string message = string.Empty;
                ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();
                byte[] cipher = Convert.FromBase64String(msgAndIV[0]);
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
            Dictionary<int, byte> identifiers = new();
            byte[] imageHeader = data[0..11];
            byte[] imageBody = data[12..];

            var indexesList = imageBody.Select((s, i) => new { i, s })
            .Where(t => t.s == ImageMetaData.JPEGIdentifier)
            .Select(t => t.i)
            .ToList();

            var oldList = indexesList.ToList();
            indexesList.ToList().ForEach(indexes => { indexesList.Add(indexes + 1); });
            var newList = indexesList.ToList();

            foreach (var index in newList)
            {
                identifiers.Add(index, imageBody[index]);
            }


            using (Aes aes = Aes.Create())
            {

                aes.Key = Key;
                aes.GenerateIV();

                using (MemoryStream ciphertext = new MemoryStream())
                {
                    // Create a CryptoStream to encrypt the data
                    using (CryptoStream cs = new CryptoStream(ciphertext, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        // Write the plaintext to the CryptoStream
                        cs.Write(imageBody, 0, imageBody.Length);
                    }
                    var chiperArray = ciphertext.ToArray();
                    for(int i = 0; i < chiperArray.Length; i++)
                    {
                        if (chiperArray[i] == ImageMetaData.JPEGIdentifier)
                        {
                            chiperArray[i] = 0x10;
                        }
                    }
                    foreach (var kvp in identifiers)
                    {
                        chiperArray[kvp.Key] = kvp.Value;
                    }
                    var fullImage = new byte[imageHeader.Length + chiperArray.Length];
                    imageHeader.CopyTo(fullImage, 0);
                    chiperArray.CopyTo(fullImage, imageHeader.Length);
                    File.WriteAllBytes("Foo6.jpg", fullImage);
                    //mislukt


                    //string test = Convert.ToBase64String(chiperArray);
                }
            }

            return "johnathan";
        }
    }
}
