using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using Path = System.IO.Path;
using System.Threading;

namespace EncryptionTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAES_Click(object sender, RoutedEventArgs e)
        {
            if(txtName.Text == "")
            {
                MessageBox.Show("Please enter a name for the keys!");
            }
            else
            {
                SetDefaultFolder();

                // Generate AES key and IV
                AesManaged aes = new AesManaged();
                aes.GenerateKey();
                aes.GenerateIV();
                byte[] aesKey = aes.Key;
                byte[] aesIV = aes.IV;

                // Save AES key and IV to file
                string aesKeyFilePath = Path.Combine(defaultFolder, $"{txtName.Text}_aesKey.txt");
                string aesIVFilePath = Path.Combine(defaultFolder, $"{txtName.Text}_aesIV.txt");
                string aesKeyBase64 = Convert.ToBase64String(aesKey);
                string aesIVBase64 = Convert.ToBase64String(aesIV);
                File.WriteAllText(aesKeyFilePath, aesKeyBase64);
                File.WriteAllText(aesIVFilePath, aesIVBase64);

                MessageBox.Show("Keys generated and saved.");
            }
        }
            KeyHelper.GenerateAesKey();
            AESHelper aesHelper = new(KeyHelper.GetAesKey());
            bool hasEncryted = aesHelper.EncryptString("hello");
            if(hasEncryted)
            {
                MessageBox.Show("Key generated and saved.");
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private void btnRSA_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Please enter a name for the keys!");
            }
            else
            {
                SetDefaultFolder();

                // Generate RSA key pair
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                RSAParameters publicKey = rsa.ExportParameters(false);
                RSAParameters privateKey = rsa.ExportParameters(true);
                string publicKeyXml = rsa.ToXmlString(false);
                string privateKeyXml = rsa.ToXmlString(true);

                // Save RSA key pair to file
                string rsaPublicFilePath = Path.Combine(defaultFolder, $"{txtName.Text}_rsaPublic.xml");
                string rsaPrivateFilePath = Path.Combine(defaultFolder, $"{txtName.Text}_rsaPrivate.xml");
                File.WriteAllText(rsaPublicFilePath, publicKeyXml);
                File.WriteAllText(rsaPrivateFilePath, privateKeyXml);

                MessageBox.Show("Keys generated and saved.");
            }
        }

        private void SetDefaultFolder()
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
                defaultFolder = Path.GetDirectoryName(dialog.FileName);
            }
        }
    }
}
