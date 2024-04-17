using FingerPrintManagerApp.Model.Admin;
using System;
using System.Windows.Data;

namespace FingerPrintManagerApp.View.Converter
{
    [ValueConversion(typeof(object), typeof(System.Windows.Visibility))]
    public class UserStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            var etat = (UserState)value;

            switch (etat)
            {
                case UserState.Fonctionnel:
                    return System.Windows.Visibility.Visible;
                case UserState.Bloqué:
                    return System.Windows.Visibility.Collapsed;
                default:
                    return null;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        
    }
}
