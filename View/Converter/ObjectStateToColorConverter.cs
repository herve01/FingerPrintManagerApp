using ARG.Controls;
using FingerPrintManagerApp.Extension;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace ARG.Converters
{
    [ValueConversion(typeof(object), typeof(Brush))]
    public class ObjectStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return (bool)value ? new SolidColorBrush(Color.FromArgb(50, 3, 220, 102)) : new SolidColorBrush(Color.FromArgb(50, 220, 0, 0));
            }

            if (value is Appointment)
            {
                var appointement = (Appointment)value;
                if (appointement.Subject.ToLower().NoAccent().Contains("present"))
                    return new SolidColorBrush(Color.FromArgb(255, 3, 220, 102));
                else if (appointement.Subject.ToLower().NoAccent().Contains("absent"))
                    return new SolidColorBrush(Color.FromArgb(255, 220, 0, 0));
                else if (appointement.Subject.ToLower().NoAccent().Contains("ferie"))
                    return new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
            }

            if (value is DateTime)
            {
                var date = (DateTime)value;

                return date.DayOfWeek == DayOfWeek.Sunday ? new SolidColorBrush(Color.FromArgb(99, 0, 0, 0)) : new SolidColorBrush(Color.FromArgb(240, 0, 0, 0)); ;
            }

            return new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        
    }
}
