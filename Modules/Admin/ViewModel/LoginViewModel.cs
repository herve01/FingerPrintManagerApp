using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Admin;
using FingerPrintManagerApp.Model.Admin.Util;
using FingerPrintManagerApp.ViewModel;
using FingerPrintManagerApp.ViewModel.Command;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FingerPrintManagerApp.Modules.Admin.ViewModel
{
    public class LoginViewModel : PageViewModel
    {
        public LoginViewModel(bool forEdit)
        {
            ForEditing = forEdit;
            Action = ForEditing ? "Modifier" :  "Se connecter";
            Login = Properties.Settings.Default.last_connected_username;
        }

        private bool _loading;
        public bool Loading
        {
            get
            {
                return this._loading;
            }
            set
            {
                if (_loading != value)
                {
                    _loading = value;
                    RaisePropertyChanged(() => Loading);
                }
            }
        }

        private bool _forEditing;
        public bool ForEditing
        {
            get
            {
                return this._forEditing;
            }
            set
            {
                if (_forEditing != value)
                {
                    _forEditing = value;
                    RaisePropertyChanged(() => ForEditing);
                }
            }
        }

        private string _login;
        public string Login
        {
            get
            {
                return this._login;
            }
            set
            {
                if (_login != value)
                {
                    _login = value;
                    RaisePropertyChanged(() => Login);
                }
            }
        }

        private string _pwd;
        public string Pwd
        {
            get
            {
                return this._pwd;
            }
            set
            {
                if (_pwd != value)
                {
                    _pwd = value;
                    RaisePropertyChanged(() => Pwd);
                }
            }
        }

        private string _newPwd;
        public string NewPwd
        {
            get
            {
                return this._newPwd;
            }
            set
            {
                if (_newPwd != value)
                {
                    _newPwd = value;
                    RaisePropertyChanged(() => NewPwd);
                }
            }
        }

        private string _action;
        public string Action
        {
            get
            {
                return this._action;
            }
            set
            {
                if (_action != value)
                {
                    _action = value;
                    RaisePropertyChanged(() => Action);
                }
            }
        }

        private User _connectedUser;
        public User ConnectedUser
        {
            get
            {
                return this._connectedUser;
            }
            set
            {
                if (_connectedUser != value)
                {
                    _connectedUser = value;
                    RaisePropertyChanged(() => ConnectedUser);
                }
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(p => Close(p));
                }

                return _closeCommand;
            }
        }

        private void Close(object param)
        {
            var win = param as Window;
            if (win != null)
                win.Close();
        }

        public IAsyncCommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new AsyncCommand(LogIn, p => CanLogin(p));
                }
                return _loginCommand;
            }
        }

        private bool CanLogin(object p)
        {
            var gPwd = !string.IsNullOrWhiteSpace(Pwd);
            var gLog = !string.IsNullOrWhiteSpace(Login) && (!Login.Contains("@") || ValueValidator.IsValidEmail(Login));

            if (!ForEditing)
                return gPwd && gLog;
            else
                return gPwd && gLog && Pwd != NewPwd && ValueValidator.IsStrongPassword(NewPwd);
        }

        private async Task LogIn(object param)
        {
            Loading = true;

            if (ForEditing)
            {
                if (!new Dao.Admin.UserDao().CheckPassword(AppConfig.CurrentUser, Pwd))
                {
                    MyMsgBox.Show("L'ancien mot de passe que vous avez tapé est érroné !", "Humager", MyMsgBoxButton.OK, MyMsgBoxIcon.Error);
                    return;
                }

                if (new Dao.Admin.UserDao().SetPasswd(AppConfig.CurrentUser, NewPwd) > 0)
                {
                    Dao.Admin.LogUtil.AddEntry(
                           AppConfig.CurrentUser,
                           DbUtil.Entity.Utilisateur + "",
                           string.Format("Configuration de nouveau mot de passe de '{0}' (ID : {1}).", AppConfig.CurrentUser, AppConfig.CurrentUser.Id)
                     );

                    MyMsgBox.Show("Nouveau mot de passe configuré avec succès !", "Humager", MyMsgBoxButton.OK, MyMsgBoxIcon.Success);

                    Close(param);
                }
                else
                {
                    Dao.Admin.LogUtil.AddEntry(
                           AppConfig.CurrentUser,
                           DbUtil.Entity.Utilisateur + "",
                           string.Format("Tentative de reconfiguration de mot de passe échoué '{0}' (ID : {1}).", AppConfig.CurrentUser, AppConfig.CurrentUser.Id)
                     );

                    MyMsgBox.Show("Modification de mot de passe échouée !", "Humager", MyMsgBoxButton.OK, MyMsgBoxIcon.Error);
                }
            }
            else
            {
                ConnectedUser = await Task.Run(() => new Dao.Admin.UserDao().GetUserAsync(Login, Pwd));

                if (ConnectedUser != null)
                {
                    if (ConnectedUser.Etat == UserState.Fonctionnel)
                    {
                        AppConfig.EntiteId = ConnectedUser.Entite.Id;

                        Dao.Admin.LogUtil.AddEntry(
                           ConnectedUser,
                           DbUtil.Entity.Utilisateur + "",
                           string.Format("Connexion avec succès de '{0}' (ID : {1}).", ConnectedUser, ConnectedUser.Id)
                        );

                        Properties.Settings.Default.last_connected_username = Login;
                        Properties.Settings.Default.Save();

                        ((App)Application.Current).User = ConnectedUser;

                        Close(param);
                    }
                    else
                    {
                        Dao.Admin.LogUtil.AddEntry(
                           ConnectedUser,
                           DbUtil.Entity.Utilisateur + "",
                           string.Format("Connexion avec interdiction d'accès suite à un blocage du compte de '{0}' (ID : {1}).", ConnectedUser, ConnectedUser.Id)
                        );

                        Loading = false;

                        ConnectedUser = null;
                        MyMsgBox.Show("Vous ne pouvez pas accéder à l'application pour l'instant. Votre compte a été bloqué. Veuillez contacter l'administrateur !", "Humager", MyMsgBoxButton.OK, MyMsgBoxIcon.Warning);
                    }
                }
                else
                {
                    Loading = false;
                    MyMsgBox.Show("Nom d'utilisateur ou mot de passe incorrect !", "Humager", MyMsgBoxButton.OK, MyMsgBoxIcon.Error);
                }
            }

            Loading = false;
        }


        private AsyncCommand _loginCommand;
        private RelayCommand _closeCommand;

    }
}
