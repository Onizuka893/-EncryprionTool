using Microsoft.Win32;
using System.IO;

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
    }
}
