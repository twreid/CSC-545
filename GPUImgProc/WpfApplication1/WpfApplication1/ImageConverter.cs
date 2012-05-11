using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApplication1
{
    [ValueConversion(typeof(Image), typeof(ImageSource))]
    class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var img = value as Image;
            var bmp = new BitmapImage();

            bmp.BeginInit();
            var mem = new MemoryStream();
            img.Save(mem, ImageFormat.Bmp);
            mem.Seek(0, SeekOrigin.Begin);
            bmp.StreamSource = mem;
            bmp.EndInit();

            return bmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
