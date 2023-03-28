using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using Path = System.IO.Path;
using System.Threading;
using System.Xml.Linq;
using System.Windows.Markup;

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
            AESHelper aesHelper = new("mnNJ8hYUnyMRPYjSE7tnRXaQtl0wd0uB28cfFrRKX6E=");
            aesHelper.EncryptImage();
            aesHelper.DecryptImage();
        }

        private void btnAES_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Please enter a name for the keys!");
            }
            else
            {
                KeyHelper.GenerateAesKey(txtName.Text);
                AESHelper aesHelper = new(KeyHelper.GetAesKey());
                //aesHelper.EncryptImage();
                aesHelper.DecryptImage();
                //bool hasEncryted = aesHelper.EncryptString("hello");
                MessageBox.Show($"Key generated and saved.");
                //string message = StorageHelper.GetFile();
                //MessageBox.Show(aesHelper.DecryptString(message));
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
                var defaultFolder = StorageHelper.SetDefaultFolder();

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
    }
}
