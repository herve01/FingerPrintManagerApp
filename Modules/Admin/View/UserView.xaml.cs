using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace FingerPrintManagerApp.Modules.Admin.View
{
    /// <summary>
    /// Logique d'interaction pour UserView.xaml
    /// </summary>
    public partial class UserView : UserControl
    {
        Storyboard statusSB;
        public UserView()
        {
            InitializeComponent();

            statusSB = TryFindResource("fadeInStatusSB") as Storyboard;
        }

        private void This_Loaded(object sender, RoutedEventArgs e)
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

        private void Password_Changed(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(((PasswordBox)sender).Password) && this.DataContext != null)
                ((ViewModel.UserViewModel)this.DataContext).Password = ((PasswordBox)sender).Password;
        }

        private void ConfirmedPassword_Changed(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(((PasswordBox)sender).Password) && this.DataContext != null)
                ((ViewModel.UserViewModel)this.DataContext).ConfirmedPassword = ((PasswordBox)sender).Password;
        }

        private void TxtPwd_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPwd.Text))
                pbPwd.Password = string.Empty;
        }

        private void TxtConfPwd_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConfPwd.Text))
                pbConfPwd.Password = string.Empty;
        }
    }
}
