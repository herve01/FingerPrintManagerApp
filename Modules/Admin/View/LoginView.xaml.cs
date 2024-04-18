using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace FingerPrintManagerApp.Modules.Admin.View
{
    /// <summary>
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        Storyboard closingAmin;
        public LoginView(Window owner)
        {
            InitializeComponent();

            if(owner != null)
                this.Owner = Window.GetWindow(owner);


            closingAmin = (Storyboard)TryFindResource("winCloseAnim");
            closingAmin.Completed += ClosingAmin_Completed;
        }

        private void ClosingAmin_Completed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            closingAmin.Begin(this);
        }

        private void Border_MouseMove_1(object sender, MouseEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text == string.Empty)
                txtLogin.Focus();
            else
                pdPasswd.Focus();
        }

        private void Password_Changed(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                ((ViewModel.LoginViewModel)this.DataContext).Pwd = ((PasswordBox)sender).Password;
        }

        private void Password2_Changed(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                ((ViewModel.LoginViewModel)this.DataContext).NewPwd = ((PasswordBox)sender).Password;
        }

        private void TxtPw_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txtPwdT = txtPwd.Text.Trim();
            var pdPwdT = pdPasswd.Password.Trim();

            if (txtPwdT == string.Empty && txtPwdT != pdPwdT)
                pdPasswd.Password = string.Empty;
        }

        private void TxtNewPw_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txtPwdT = txtNewPwd.Text.Trim();
            var pdPwdT = pdNewPasswd.Password.Trim();

            if (txtPwdT == string.Empty && txtPwdT != pdPwdT)
                pdNewPasswd.Password = string.Empty;
        }

    }
}
