using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace FingerPrintManagerApp.View.Converter
{
    public class ListViewItemToPositionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var item = value as ListViewItem;
                var listView = ItemsControl.ItemsControlFromItemContainer(item);
                var index = listView.ItemContainerGenerator.IndexFromContainer(item);

                return index + 1;
            }
            catch (Exception)
            {
                return 0;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
