using System;
using System.Windows.Data;

namespace FingerPrintManagerApp.View.Converter
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var param = parameter.ToString().Split(',');
            var p1 = double.Parse(param[0].Replace('.', ','));
            var p2 = param.Length == 2 ? double.Parse(param[1]) : 0;

            return (double.Parse(value.ToString()) - p2) * p1 / 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }


    }
}
