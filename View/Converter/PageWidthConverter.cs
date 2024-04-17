using System;
using System.Globalization;
using System.Windows.Data;

namespace FingerPrintManagerApp.View.Converter
{
    public class PageWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return 0;

            var item = values[0] as Model.Page;
            //item.ComputeSize();

            var actualWidth = System.Convert.ToDouble(values[1]);

            var span = System.Convert.ToDouble(parameter);

            if (item != null)
                return (item.Size.Width / item.Document.MaxPageSize.Width) * actualWidth - span;

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

    }
}
