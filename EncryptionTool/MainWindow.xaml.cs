using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            DoThang();
        }

        public void DoThang()
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                string keybase, ivbase, encryptmsg;
                aesAlgorithm.GenerateKey();
                aesAlgorithm.GenerateIV();
                AESHelper aesHelper = new(aesAlgorithm.Key,aesAlgorithm.IV);
                encryptmsg = aesHelper.EncryptString("testen nigga", out keybase, out ivbase);
                txtTest.Text += $" message : {encryptmsg}\n";
                txtTest.Text += $" key : {keybase}\n";
                txtTest.Text += $" iv : {ivbase}\n";

                aesAlgorithm.Key = Convert.FromBase64String(keybase);
                aesAlgorithm.IV = Convert.FromBase64String(ivbase);
                ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();
                byte[] cipher = Convert.FromBase64String(encryptmsg);
                using (MemoryStream ms = new MemoryStream(cipher))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            txtTest.Text += $"messagedecrypt : { sr.ReadToEnd()}\n";
                        }
                    }
                }


            }
        }
    }
}
