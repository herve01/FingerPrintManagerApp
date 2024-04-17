using System;
using System.Windows.Data;

namespace FingerPrintManagerApp.View.Converter
{
    [ValueConversion(typeof(object), typeof(string))]
    public class ToUpperCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            return value.ToString().ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }


    }
}
