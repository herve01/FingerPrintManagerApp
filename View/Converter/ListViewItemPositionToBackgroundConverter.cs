using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace FingerPrintManagerApp.View.Converter
{
    public class ListViewItemPositionToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var item = value as ListViewItem;
            var listView = ItemsControl.ItemsControlFromItemContainer(item);
            var index = listView.ItemContainerGenerator.IndexFromContainer(item);

            if (index % 2 == 0)
                return Brushes.Transparent;
            else
                return new SolidColorBrush(Color.FromArgb(255, 245, 245, 245));
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
