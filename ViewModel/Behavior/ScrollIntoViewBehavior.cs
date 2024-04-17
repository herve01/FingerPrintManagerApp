using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace FingerPrintManagerApp.ViewModel.Behavior
{
    public class ScrollIntoLastViewBehavior : Behavior<ListView>
    {
        protected override void OnAttached()
        {
            ListView lv = AssociatedObject;
            ((INotifyCollectionChanged)lv.Items).CollectionChanged += ScrollIntoViewBehavior_CollectionChanged;
        }

        protected override void OnDetaching()
        {
            ListView lv = AssociatedObject;
            ((INotifyCollectionChanged)lv.Items).CollectionChanged -= ScrollIntoViewBehavior_CollectionChanged;
        }

        private void ScrollIntoViewBehavior_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ListView lv = AssociatedObject;
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                lv.ScrollIntoView(lv.Items[lv.Items.Count - 1]);
            }
        }
    }

    public static class ScrollToSelectedBehavior
    {
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.RegisterAttached(
            "SelectedValue",
            typeof(object),
            typeof(ScrollToSelectedBehavior),
            new PropertyMetadata(null, OnSelectedValueChange));

        public static void SetSelectedValue(DependencyObject source, object value)
        {
            source.SetValue(SelectedValueProperty, value);
        }

        public static object GetSelectedValue(DependencyObject source)
        {
            return (object)source.GetValue(SelectedValueProperty);
        }

        private static void OnSelectedValueChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var lv = d as ListView;
            lv.ScrollIntoView(e.NewValue);
        }
    }
}
