using System.IO;
using System.Security.Cryptography;
using System.Windows;
using Path = System.IO.Path;
using System.Threading;
using System.Xml.Linq;
using System.Windows.Media.Animation;
using System.Windows.Controls;

namespace EncryptionTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> listKeysize = new List<string> { "512 bits", "521 bits", "1024 bits", "2048 bits", "4096 bits" };

        public MainWindow()
        {
            InitializeComponent();
            foreach (var keysize in listKeysize)
            {
                cmbSize.Items.Add(keysize);
            }
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
                var selectedKeySize = cmbSize.SelectedItem.ToString();
                var keySize = int.Parse(selectedKeySize.Replace("bits", ""));
                KeyHelper.GenerateRsaKey(txtName.Text, keySize);
                MessageBox.Show($"Key generated and saved.");
                //RSAHelper rsaHelper = new(KeyHelper.GetRsaPublicKey(txtName.Text), KeyHelper.GetRsaPrivateKey(txtName.Text));
                //bool hasEncrypted = rsaHelper.EncryptString("test");
                //if(hasEncrypted)
                //{
                //    MessageBox.Show("Key generated and saved.");
                //}
                //else
                //{
                //    MessageBox.Show("Error");
                //}
            }
        }

        private void btntest_Click(object sender, RoutedEventArgs e)
        {
            
            RSAHelper rsaHelper = new();
            rsaHelper.DecryptString("pRScICU0OmKsRoLf87QZrBF/FgdTs3Ccc+UdgUA59RF0yUgth8gKQ38MWziKc3RUkl9q1Io5ClcJYKfXSsuOgYUzPwMOrliIeIz2NeAw8Xb+8D1OCkOq69FVqEsbDzJKmgl0yk82lON6RRGwQniGF51kLKbf0/UCzQ0QRnKU5jl6NbLrTVBvlyn+NkbAmXbrER3JlHSHbxWNWQy89T9jM1UqKrQVB8OcMOMUBgpUDzPu2/BryndEcXA5TvVAPO6ansNWVH5r4a2H0umTbWg9mVPThZY4jj44jzog8c4HJze/Vl6PeTGhCFcJq4ieG9d49+boOyavSt7paI8c5TYucg==");
        }

        private void PublishKeyPair_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
