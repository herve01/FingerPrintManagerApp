using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace FingerPrintManagerApp.Modules.Employe.View
{
    /// <summary>
    /// Logique d'interaction pour FingerPrintView.xaml
    /// </summary>
    public partial class FingerPrintView : UserControl
    {
        Storyboard statusSB;
        public FingerPrintView()
        {
            InitializeComponent();

            statusSB = TryFindResource("fadeInStatusSB") as Storyboard;
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
    }
}
