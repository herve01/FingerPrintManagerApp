using System;
using System.Windows.Data;
using System.Windows.Media;

namespace FingerPrintManagerApp.View.Converter
{
    [ValueConversion(typeof(object), typeof(Geometry))]
    public class PathDataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string data = (string)value;
            try
            {
                var geometry = Geometry.Parse(data);
                return geometry;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }


    }
}
