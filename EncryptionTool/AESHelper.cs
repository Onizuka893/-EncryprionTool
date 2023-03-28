using System.IO;
using System.Security.Cryptography;

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

        public void EncryptImage()
        {
            var filePath = StorageHelper.GetImageFile();
            var bitmapImageData = ImageHelper.Convert(filePath);
            byte[] imageHeader = bitmapImageData[..54];
            byte[] encryptedImageData;
            byte[] HiddenIv;

            using (var aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = Key;
                //aesAlgorithm.IV = Convert.FromBase64String("RFOW2huw0cLw0izie7k3/A==");
                aesAlgorithm.GenerateIV();
                HiddenIv = aesAlgorithm.IV;
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
                var tempList = encryptedImageData.ToList();
                foreach (var b in HiddenIv)
                {
                    tempList.Add(b);
                }
                encryptedImageData = tempList.ToArray();
                StorageHelper.SaveImageFromBytes(encryptedImageData, true);
            }
        }

        public void DecryptImage()
        {
            var filePath = StorageHelper.GetImageFile();
            var bitmapImageData = File.ReadAllBytes(filePath);
            byte[] imageHeader = bitmapImageData[..54];
            byte[] fileSizeHeaderInfo = imageHeader[2..6];
            int fileSize = BitConverter.ToInt32(fileSizeHeaderInfo, 0);
            byte[] decryptedImage;

            var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = GetHiddenKey(bitmapImageData);
            bitmapImageData = bitmapImageData.Take(bitmapImageData.Length - 16).ToArray();
            ICryptoTransform decryptor = aes.CreateDecryptor();
            using (var ms = new MemoryStream(bitmapImageData))
            using (var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            {

                var decrypted = new byte[bitmapImageData.Length];
                var bytesRead = cryptoStream.Read(decrypted, 0, bitmapImageData.Length);

                decryptedImage = decrypted.Take(bytesRead).ToArray();
            }

            for (int i = 0; i < imageHeader.Length; i++)
            {
                decryptedImage[i] = imageHeader[i];
            }

            if(decryptedImage.Length != fileSize)
            {
                int difference = fileSize - decryptedImage.Length;
                var list = decryptedImage.ToList();
                for (int i = 0; i < difference; i++)
                {
                    list.Add(ImageMetaData.BMPByte);
                }
                decryptedImage = list.ToArray();
            }
            StorageHelper.SaveImageFromBytes(decryptedImage);
        }

        public byte[] GetHiddenKey(byte[] data)
            => data.TakeLast(16).ToArray();
    }
}
