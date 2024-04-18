using System.Windows;
using System.Windows.Controls;

namespace FingerPrintManagerApp.Modules.Admin.View
{
    /// <summary>
    /// Logique d'interaction pour SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }
        
        void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            //txtName.Text = Properties.Settings.Default.organisation_name;
        }

        private void btnInfo_Click_1(object sender, RoutedEventArgs e)
        {
            //popupInfo.IsOpen = !popupInfo.IsOpen;
        }

        private void btnSave_Click_1(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrWhiteSpace(txtName.Text))
            //{
            //    Properties.Settings.Default.organisation_name = txtName.Text.Trim();
            //    Properties.Settings.Default.are_params_configured = true;
            //    Properties.Settings.Default.is_app_configured = Properties.Settings.Default.is_db_configured;

            //    Properties.Settings.Default.Save();

            //    if (App.CurrentUser != null)
            //    {
            //        Dao.LogUtil.AddEntry(
            //                    App.CurrentUser,
            //                    Util.DBUtil.Entity.Application + "",
            //                    string.Format("Paramétrage réussi de l'application sur : PC {0}, IP {1}.", Util.AppUtil.GetClientMachineName(), Util.AppUtil.GetClientIPAddress())
            //                                );
            //    }

            //    MyMsgBox.Show("Paramètres de l'application enregistrés avec succès !", "BoGED", MyMsgBoxButton.OK, MyMsgBoxIcon.Success);
            //}
        }
    }
}
