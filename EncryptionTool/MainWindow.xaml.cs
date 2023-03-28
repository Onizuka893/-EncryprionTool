using System.IO;
using System.Security.Cryptography;
using System.Windows;
using Path = System.IO.Path;
using System.Threading;
using System.Xml.Linq;
using System.Windows.Media.Animation;

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
                MessageBox.Show($"Key generated and saved.");
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

        private void btntest_Click(object sender, RoutedEventArgs e)
        {
            
            RSAHelper rsaHelper = new();
            rsaHelper.DecryptString("pRScICU0OmKsRoLf87QZrBF/FgdTs3Ccc+UdgUA59RF0yUgth8gKQ38MWziKc3RUkl9q1Io5ClcJYKfXSsuOgYUzPwMOrliIeIz2NeAw8Xb+8D1OCkOq69FVqEsbDzJKmgl0yk82lON6RRGwQniGF51kLKbf0/UCzQ0QRnKU5jl6NbLrTVBvlyn+NkbAmXbrER3JlHSHbxWNWQy89T9jM1UqKrQVB8OcMOMUBgpUDzPu2/BryndEcXA5TvVAPO6ansNWVH5r4a2H0umTbWg9mVPThZY4jj44jzog8c4HJze/Vl6PeTGhCFcJq4ieG9d49+boOyavSt7paI8c5TYucg==");
        }

        public void Demo()
        {
            AESHelper aesHelper = new("mnNJ8hYUnyMRPYjSE7tnRXaQtl0wd0uB28cfFrRKX6E=");
            aesHelper.EncryptImage();
            aesHelper.DecryptImage();
        }
    }
}
