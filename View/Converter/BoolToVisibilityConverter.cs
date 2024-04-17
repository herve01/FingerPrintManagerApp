using System;
using System.Windows.Data;

namespace FingerPrintManagerApp.View.Converter
{
    [ValueConversion(typeof(object), typeof(System.Windows.Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is bool)
            {
                return (bool)value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            
            return null;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        
    }
}
