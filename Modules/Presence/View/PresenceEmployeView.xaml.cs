using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Animation;


namespace FingerPrintManagerApp.Modules.Presence.View
{
    /// <summary>
    /// Logique d'interaction pour PresenceEmployeView.xaml
    /// </summary>
    public partial class PresenceEmployeView : UserControl
    {
        Storyboard statusSB;
        public PresenceEmployeView()
        {
            InitializeComponent();

            statusSB = TryFindResource("fadeInStatusSB") as Storyboard;
        }

        private void TextBox_GotFocus_1(object sender, System.Windows.RoutedEventArgs e)
        {
            var txt = (TextBox)sender;
            txt.Select(txt.Text.Length, 0);
        }

        private void This_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var dp = DependencyPropertyDescriptor.FromProperty(TextBlock.TextProperty, typeof(TextBlock));
            dp.AddValueChanged(txtStatus, (s, a) =>
            {
                if (((TextBlock)s).Text != string.Empty)
                {
                    statusSB.Begin(this);
                }
            });
        }

        private void btnPrint_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            popupPrint.IsOpen = !popupPrint.IsOpen;
        }
    }
}
