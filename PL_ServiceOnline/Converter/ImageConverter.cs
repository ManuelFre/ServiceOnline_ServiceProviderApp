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
        public static byte[] ImageToByteArray(BitmapImage image)
        {
            using (var ms = new MemoryStream())
            {

                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(ms);
                return ms.ToArray();
            }

        }

        //TODO: byte[] to BitmapImage converter (still needs to be tested)
        public static BitmapImage ByteArrayToImage(byte[] array)
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
