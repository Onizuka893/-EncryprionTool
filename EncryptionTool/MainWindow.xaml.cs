using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using Path = System.IO.Path;

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
            string defaultFolder = StorageHelper.SetDefaultFolder();
            // Generate RSA key pair
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            RSAParameters publicKey = rsa.ExportParameters(false);
            RSAParameters privateKey = rsa.ExportParameters(true);
            string publicKeyXml = rsa.ToXmlString(false);
            string privateKeyXml = rsa.ToXmlString(true);

            // Save RSA key pair to file
            string rsaPublicFilePath = Path.Combine(defaultFolder, "rsaPublic.xml");
            string rsaPrivateFilePath = Path.Combine(defaultFolder, "rsaPrivate.xml");
            File.WriteAllText(rsaPublicFilePath, publicKeyXml);
            File.WriteAllText(rsaPrivateFilePath, privateKeyXml);

            MessageBox.Show("Keys generated and saved.");
        }

    }
}
