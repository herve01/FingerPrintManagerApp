using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace FingerPrintManagerApp.Modules.Employe.View
{
    /// <summary>
    /// Logique d'interaction pour AffectationView.xaml
    /// </summary>
    public partial class AffectationView : UserControl
    {
        Storyboard statusSB;
        public AffectationView()
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

        private void BtnPrint_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            popupPrint.IsOpen = !popupPrint.IsOpen;
        }
    }
}
