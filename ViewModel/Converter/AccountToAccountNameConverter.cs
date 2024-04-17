using System;
using System.Windows;
using System.Windows.Data;

namespace FingerPrintManagerApp.ViewModel.Converter
{
    public class AccountToAccountNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var str = value.ToString();
            return str.Split('|')[1].Trim();
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        
    }
}
