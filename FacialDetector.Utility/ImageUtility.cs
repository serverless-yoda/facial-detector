using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace FacialDetector.Utility
{
    public static class ImageUtility
    {
        public static byte[] ImageToBytes(string imagePath)
        {
            var ms = new MemoryStream();
            using var fs = new FileStream(imagePath, FileMode.Open);
            fs.CopyTo(ms);
            return ms.ToArray();
        }

        public static void ByteToImage(byte[] imageBytes,string fileName) {
            using var ms = new MemoryStream(imageBytes);
            Image img = Image.FromStream(ms);
            img.Save($"{fileName}.jpg", ImageFormat.Jpeg);
        }
    }
}
