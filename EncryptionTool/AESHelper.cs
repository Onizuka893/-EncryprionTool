using System;
using System.Drawing;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Controls;
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
            using (var aesAlgorithm = Aes.Create())
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

        public void DecryptImage()
        {
            var bitmapImageData = File.ReadAllBytes(@"C:\Users\Lenovo\Desktop\onizuka_encrypted.bmp");
            byte[] imageHeader = bitmapImageData[..40];
            byte[] decryptedImage;

            var aes = Aes.Create();
            aes.Key = Convert.FromBase64String("mnNJ8hYUnyMRPYjSE7tnRXaQtl0wd0uB28cfFrRKX6E=");
            aes.IV = Convert.FromBase64String("RFOW2huw0cLw0izie7k3/A==");
            ICryptoTransform decryptor = aes.CreateDecryptor();
            using (var ms = new MemoryStream(bitmapImageData))
            using (var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            {

                var dycrypted = new byte[bitmapImageData.Length];
                var bytesRead = cryptoStream.Read(dycrypted, 0, bitmapImageData.Length);

                decryptedImage = dycrypted.Take(bytesRead).ToArray();
            }

            for (int i = 0; i < imageHeader.Length; i++)
            {
                decryptedImage[i] = imageHeader[i];
            }
            File.WriteAllBytes(@"C:\Users\Lenovo\Desktop\onizuka_decrypted.bmp", decryptedImage);
        }

        public void EncryptImage()
        {
            var bitmapImageData = ImageHelper.Convert();
            byte[] imageHeader = bitmapImageData[..40];
            byte[] encryptedImageData;

            using (var aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = Convert.FromBase64String("mnNJ8hYUnyMRPYjSE7tnRXaQtl0wd0uB28cfFrRKX6E=");
                aesAlgorithm.IV = Convert.FromBase64String("RFOW2huw0cLw0izie7k3/A==");
                string message = string.Empty;
                ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor();
                using (MemoryStream mstream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mstream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bitmapImageData, 0, bitmapImageData.Length);
                    }
                    encryptedImageData = mstream.ToArray();
                }
                for (int i = 0; i < imageHeader.Length; i++)
                {
                    encryptedImageData[i] = imageHeader[i];
                }
                File.WriteAllBytes(@"C:\Users\Lenovo\Desktop\onizuka_encrypted.bmp", encryptedImageData);
            }
        }
    }
}
