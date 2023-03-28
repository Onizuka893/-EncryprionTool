using Microsoft.Win32;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace EncryptionTool
{
    public static class StorageHelper
    {
        public static string SetDefaultFolder()
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Multiselect = false;
            dialog.Title = "Select Default Folder";
            dialog.ValidateNames = false;
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.FileName = "Folder Selection.";
            if (dialog.ShowDialog() == true)
            {
                return Path.GetDirectoryName(dialog.FileName);
            }
            return null;
        }

        public static bool SaveFile(string fileContent)
        {
            SaveFileDialog sfd = new()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "Save encryped file",
                CheckPathExists = true,
                DefaultExt = "txt",
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (sfd.ShowDialog() == true)
            {
                File.WriteAllText(sfd.FileName, fileContent);
                return true;
            }
            return false;
        }

        public static string GetFile(bool isKey = false)
        {
            string fileContent = string.Empty;
            Stream myStream;
            OpenFileDialog theDialog = new()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = isKey ? "Select Key File" : "Open Encryped Text file",
                CheckPathExists = true,
                DefaultExt = isKey ? "txt" : "jpg",
                Filter = isKey ? "Text files (*.txt)|*.txt|All files (*.*)|*.*" : "Files|*.jpg;*.jpeg;*.png;",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (theDialog.ShowDialog() == true)
            {
                try
                {
                    if ((myStream = theDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            using(StreamReader sr = new(myStream))
                            {
                                fileContent += sr.ReadToEnd();
                            }
                        }
                    }
                    return fileContent;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            return null;
        }

        public static byte[] ReadImageFile()
        {
            byte[] imageData = null;
            OpenFileDialog theDialog = new()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "Open Encryped Text file",
                CheckPathExists = true,
                DefaultExt = "jpg",
                Filter = "Files|*.jpg;*.jpeg;*.png;",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (theDialog.ShowDialog() == true)
            {
                imageData = File.ReadAllBytes(theDialog.FileName);
            }
            return imageData;
        }

        public static string GetImageFile()
        {
            OpenFileDialog theDialog = new()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "Open Encryped Text file",
                CheckPathExists = true,
                DefaultExt = "jpg",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;*.bmp;...",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (theDialog.ShowDialog() == true)
            {
                return theDialog.FileName;
            }
            return null;
        }

        public static void EncryptImageBody(byte[] body, byte[] header)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(StorageHelper.GetFile(true));

                // Generate a random initialization vector (IV)
                aes.GenerateIV();

                // Create a memory stream to hold the ciphertext
                using (MemoryStream ciphertext = new MemoryStream())
                {
                    // Create a CryptoStream to encrypt the data
                    using (CryptoStream cs = new CryptoStream(ciphertext, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        // Write the plaintext to the CryptoStream
                        cs.Write(body, 0, body.Length);
                    }

                    string test = Convert.ToBase64String(ciphertext.ToArray());
                    DecryptImage(test, header, Convert.ToBase64String(aes.IV));
                }
            }
        }

        public static void DecryptImage(string body, byte[] header, string iv)
        {
            using (var aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = Convert.FromBase64String(StorageHelper.GetFile(true));
                aesAlgorithm.IV = Convert.FromBase64String(iv);
                string message = string.Empty;
                ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();
                byte[] cipher = Convert.FromBase64String(body);
                byte[] imagebody = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
                var fullImage = new byte[header.Length + cipher.Length];
                header.CopyTo(fullImage, 0);
                cipher.CopyTo(fullImage, header.Length);

                File.WriteAllBytes("Foo2.txt", fullImage);

                //using (MemoryStream ms = new MemoryStream(cipher))
                //{
                //    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                //    {
                //        using (StreamReader sr = new StreamReader(cs))
                //        {
                //            message = sr.ReadToEnd();
                //        }
                //        var bytes = Convert.FromBase64String(message);
                //    }
                //}
            }
        }
    }
}
