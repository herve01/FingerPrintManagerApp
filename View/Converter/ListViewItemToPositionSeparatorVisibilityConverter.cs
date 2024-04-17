using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FingerPrintManagerApp.View.Converter
{
    public class ListViewItemToPositionSeparatorVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var item = value as ListViewItem;
            var listView = ItemsControl.ItemsControlFromItemContainer(item);
            var index = listView.ItemContainerGenerator.IndexFromContainer(item);

            return index == listView.Items.Count - 1 ? Visibility.Collapsed : Visibility.Visible;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
