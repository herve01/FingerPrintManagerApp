using System;
using System.Windows.Data;

namespace FingerPrintManagerApp.View.Converter
{
    [ValueConversion(typeof(decimal), typeof(decimal))]
    public class DecimalInvertConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal v;

            if (decimal.TryParse(value.ToString(), out v) && v > 0)
            {
                decimal back = Math.Round(1 / v, 5);
                return back - (int)back == 0 ? (int)back : back;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal valeur;
            if (decimal.TryParse(value + "", out valeur))
            {
                if (valeur > 0)
                    return 1 / valeur;
            }

            return 0;
        }


    }
}
