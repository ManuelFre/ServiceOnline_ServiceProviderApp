using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PL_ServiceOnline.Converter
{
    internal static class ImageConverter
    {
        internal static byte[] ImageToByteArray(BitmapImage image)
        {
            using (var ms = new MemoryStream()) //Image resourcen sind sehr groß und damit der Stream sofort wieder disposed wird diesen in ein using statement
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(ms);
                return ms.ToArray();
            }

        }        
        internal static BitmapImage ByteArrayToImage(byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
    }
}
