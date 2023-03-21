using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionTool
{
    public class RSAHelper : IEncryptor
    {

        private RSA rsaAlgorithm;

        public RSAHelper()
        {

        }
       
        public string EncryptString(string plaintext)
        {
            
        }

        public string DecryptString(string plaintext)
        {

        }

        public string DecryptImage(byte[] data)
        {
            throw new NotImplementedException();
        }

        public string EncryptImage(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
