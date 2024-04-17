using System.Windows;
using System.Windows.Controls;

namespace FingerPrintManagerApp.View.Control
{
    public class CloseableTabItem : TabItem
    {
        static CloseableTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseableTabItem), new FrameworkPropertyMetadata(typeof(CloseableTabItem)));
        }
        public static readonly RoutedEvent CloseTabEvent = EventManager.RegisterRoutedEvent("CloseTab", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CloseableTabItem));

        public event RoutedEventHandler CloseTab
        {
            add { AddHandler(CloseTabEvent, value);}
            remove { RemoveHandler(CloseTabEvent, value);}
        }

        public override void OnApplyTemplate()
        {
 	         base.OnApplyTemplate();
                    Button closeButton = base.GetTemplateChild("PART_Close") as Button;
                    if (closeButton != null)
	        {
		         closeButton.Click += new RoutedEventHandler(closeButton_Click);
	        }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
 	        this.RaiseEvent(new RoutedEventArgs(CloseTabEvent, this));
        }
    }
}
