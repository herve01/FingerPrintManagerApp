using System;
using System.Windows;
using System.Windows.Data;

namespace FingerPrintManagerApp.View.Converter
{
    public class AccountNumberLengthToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var lenght = value.ToString().Length;
            return new Thickness((lenght - 2) * 50, 0, 0, 0);
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        
    }
}
