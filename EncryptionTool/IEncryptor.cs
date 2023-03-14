using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionTool
{
    public interface IEncryptor
    {
        public void Encrypt(byte[] data);
        public byte[] Decrypt(byte[] data);
    }
}
