using System;
using System.Windows.Data;

namespace FingerPrintManagerApp.View.Converter
{
    [ValueConversion(typeof(int), typeof(double))]
    public class ValueToAngleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)((int)value * 3.6);
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (int)((double)value / 3.6);
        }

        
    }
}
