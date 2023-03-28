using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace EncryptionTool
{
    static public class ImageHelper
    {
        static public byte[] Convert()
        {
            var fileName = StorageHelper.GetImageFile();
            var newPath = fileName.Replace("jpg", "bmp");
            Image imgInFile = Image.FromFile(fileName);
            imgInFile.Save(newPath, ImageFormat.Bmp);
            var bytes = File.ReadAllBytes(newPath);
            return bytes;
        }
    }
}
