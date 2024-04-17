using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FingerPrintManagerApp.View.Converter
{
    [ValueConversion(typeof(object), typeof(ImageSource))]
    public class ByteArrayToImageThumbnailConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var height = 100;
            var width = 100;

            if (parameter != null)
            {
                var param = parameter.ToString().Split(',');
                int.TryParse(param[0], out width);
                int.TryParse(param[1], out height);
            }
            

            var byteArrayImage = (byte[])value;

            if (byteArrayImage != null && byteArrayImage.Length > 0)
            {
                byteArrayImage = Helper.ImageUtil.ComputeThumbnail(byteArrayImage, width, height);

                if (byteArrayImage != null)
                {
                    var ms = new MemoryStream(byteArrayImage);
                    var bitmapImg = new BitmapImage();
                    bitmapImg.BeginInit();
                    bitmapImg.StreamSource = ms;
                    bitmapImg.EndInit();

                    return bitmapImg;
                }
                
            }

            //var bit = new BitmapImage(new Uri("pack://application:,,,/View/Image/product_thumb.png"));
            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var imgSource = value as BitmapSource;

            //if (imgSource != null)
            //{
            //    Byte[] byteArray = Libs.Functions.bitmapToByte(Libs.Functions.BitmapImage2Bitmap(imgSource));

            //    return byteArray;
            //}
            return null;
        }

        #endregion
    }
}
