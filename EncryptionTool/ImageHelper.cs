using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace EncryptionTool
{
    static public class ImageHelper
    {
        static public byte[] Convert(string filePath)
        {
            var fileName = filePath;
            var fileExtension = fileName.Split('.').Last();
            var newPath = fileName.Replace(fileExtension, "bmp");
            Image imgInFile = Image.FromFile(fileName);
            imgInFile.Save(newPath, ImageFormat.Bmp);
            var bytes = File.ReadAllBytes(newPath);
            StorageHelper.DeleteConvertedImage(newPath);
            return bytes;
        }
    }
}
