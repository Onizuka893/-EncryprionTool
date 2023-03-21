namespace EncryptionTool
{
    public interface IEncryptor
    {
        public string EncryptImage(byte[] data);
        public string DecryptImage(byte[] data);
    }
}
