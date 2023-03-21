using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using Path = System.IO.Path;
using System.Threading;
using System.Xml.Linq;

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
            if (txtName.Text == "")
            {
                MessageBox.Show("Please enter a name for the keys!");
            }
            else
            {
                KeyHelper.GenerateAesKey(txtName.Text);
                AESHelper aesHelper = new(KeyHelper.GetAesKey());
                bool hasEncryted = aesHelper.EncryptString("hello");
                if (hasEncryted)
                {
                    MessageBox.Show("Key generated and saved.");
                }
                else
                {
                    MessageBox.Show("Error");
                }
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
                KeyHelper.GenerateRsaKey(txtName.Text);
                RSAHelper rsaHelper = new(KeyHelper.GetRsaPublicKey(txtName.Text), KeyHelper.GetRsaPrivateKey(txtName.Text));
                bool hasEncrypted = rsaHelper.EncryptString("test");
                if(hasEncrypted)
                {
                    MessageBox.Show("Key generated and saved.");
                }
                else
                {
                    MessageBox.Show("Error");
                }

                //var defaultFolder = StorageHelper.SetDefaultFolder();

                //// Generate RSA key pair
                //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                //RSAParameters publicKey = rsa.ExportParameters(false);
                //RSAParameters privateKey = rsa.ExportParameters(true);
                //string publicKeyXml = rsa.ToXmlString(false);
                //string privateKeyXml = rsa.ToXmlString(true);

                //// Save RSA key pair to file
                //string rsaPublicFilePath = Path.Combine(defaultFolder, $"{txtName.Text}_rsaPublic.xml");
                //string rsaPrivateFilePath = Path.Combine(defaultFolder, $"{txtName.Text}_rsaPrivate.xml");
                //File.WriteAllText(rsaPublicFilePath, publicKeyXml);
                //File.WriteAllText(rsaPrivateFilePath, privateKeyXml);

            }
        }
    }
}
