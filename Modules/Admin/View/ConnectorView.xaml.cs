using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace FingerPrintManagerApp.Modules.Admin.View
{
    /// <summary>
    /// Logique d'interaction pour ConnectorView.xaml
    /// </summary>
    public partial class ConnectorView : UserControl
    {
        Storyboard statusSB;
        public ConnectorView()
        {
            InitializeComponent();

            statusSB = TryFindResource("fadeInStatusSB") as Storyboard;
        }

        private void This_Loaded(object sender, RoutedEventArgs e)
        {
            txtPwd.Password = string.Empty;

            var dp = DependencyPropertyDescriptor.FromProperty(TextBlock.TextProperty, typeof(TextBlock));
            dp.AddValueChanged(txtStatus, (s, a) =>
            {
                if (((TextBlock)s).Text != string.Empty)
                {
                    statusSB.Begin(this);
                }
            });
        }

        private void Password_Changed(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                ((ViewModel.ConnectorViewModel)this.DataContext).Password = ((PasswordBox)sender).Password;
        }

    }
}
