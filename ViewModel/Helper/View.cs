using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FingerPrintManagerApp.ViewModel.Helper
{
    public class View
    {
        public static UIElement GetItemContainerFromItemsControl(ItemsControl itemsControl)
        {
            UIElement container = null;
            if (itemsControl != null && itemsControl.Items.Count > 0)
            {
                container = itemsControl.ItemContainerGenerator.ContainerFromIndex(0) as UIElement;
            }
            else
            {
                container = itemsControl;
            }
            return container;
        }

        public static bool IsUserVisibleFromTheMiddle(FrameworkElement element, FrameworkElement container)
        {
            if (!element.IsVisible)
                return false;

            Rect bounds = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));

            var rect = new Rect(container.ActualWidth / 2, container.ActualHeight / 2, container.ActualWidth, container.ActualHeight);
            return rect.IntersectsWith(bounds);
        }

        public static bool IsUserVisibleInFromTheTop(FrameworkElement element, FrameworkElement container)
        {
            if (!element.IsVisible)
                return false;

            Rect bounds = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));

            var rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
            return rect.IntersectsWith(bounds);
        }

        public static List<object> GetItemsControlVisibleItemsInFromMiddle(ItemsControl control, FrameworkElement parent)
        {

            var items = new List<object>();
            foreach (var item in control.Items)
            {
                if (IsUserVisibleFromTheMiddle((FrameworkElement)control.ItemContainerGenerator.ContainerFromItem(item), control))
                    items.Add(item);
                else if (items.Any())
                    break;
            }

            return items;
        }

        public static List<object> GetItemsControlVisibleItemsFromTheTop(ItemsControl control, FrameworkElement parent)
        {

            var items = new List<object>();
            foreach (var item in control.Items)
            {
                if (IsUserVisibleInFromTheTop((FrameworkElement)control.ItemContainerGenerator.ContainerFromItem(item), control))
                    items.Add(item);
                else if (items.Any())
                    break;
            }

            return items;
        }
    }
}
