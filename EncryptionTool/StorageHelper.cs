using Microsoft.Win32;
using System.IO;
using System.Windows;

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
                Title = "Open Image File",
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

        public static bool SaveImageFromBytes(byte[] imageData, bool encypted = false)
        {
            SaveFileDialog sfd = new()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = encypted ? "Save encrypted Image File" : "Save Image File",
                CheckPathExists = true,
                DefaultExt = "bmp",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;...",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (sfd.ShowDialog() == true)
            {
                File.WriteAllBytes(sfd.FileName, imageData);
                return true;
            }
            return false;
        }

        public static bool DeleteConvertedImage(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
                return true;
            }
            return false;
        }
    }
}
