using FingerPrintManagerApp.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;
using FingerPrintManagerApp.Dialog.Service;
using FingerPrintManagerApp.ViewModel.Command;
using System.Windows;
using System.Windows.Input;
using System.Data.Common;
using System.Threading.Tasks;
using FingerPrintManagerApp.Model.Admin;
using System.Windows.Data;
using FingerPrintManagerApp.Dao;

namespace FingerPrintManagerApp.Modules.Admin.ViewModel
{
    public class ConnectorViewModel : DialogViewModelBase, IDataErrorInfo
    {
        ObservableCollection<string> databases;
        public ICollectionView DatabasesView { get; private set; }

        public ConnectorViewModel()
        {
            databases = new ObservableCollection<string>();
            DatabasesView = (CollectionView)CollectionViewSource.GetDefaultView(databases);

            MenuInit();
        }

        private string _serveur;
        private string _user;
        private string _port;
        private string _password;
        private string _dbName;
        private bool _databaseLoading;
        private bool _isTested;

        public bool DatabaseLoading
        {
            get
            {
                return this._databaseLoading;
            }
            set
            {
                _databaseLoading = value;
                RaisePropertyChanged(() => DatabaseLoading);
            }
        }
        public bool IsTested
        {
            get
            {
                return this._isTested;
            }
            set
            {
                _isTested = value;
                RaisePropertyChanged(() => IsTested);
            }
        }
        public string Serveur
        {
            get
            {
                return _serveur;
            }

            set
            {
                _serveur = value;
                RaisePropertyChanged(() => Serveur);
            }
        }
        public string User
        {
            get
            {
                return _user;
            }

            set
            {
                _user = value;
                RaisePropertyChanged(()=>User);
            }
        }
        public string Port
        {
            get
            {
                return _port;
            }

            set
            {
                _port = value;
                RaisePropertyChanged(() => Port);
            }

        }
        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }
        public string DbName
        {
            get
            {
                return _dbName;
            }

            set
            {
                _dbName = value;
                RaisePropertyChanged(()=>DbName);
            }

        }

        private DbConnection connection;

        protected override async Task Load(object param)
        {
            Status = string.Empty;
            Bind();
            IsTested = false;
            databases.Clear();
        }

        void Bind()
        {
            Serveur = Properties.Settings.Default.local_server;
            User = Properties.Settings.Default.local_user;
            Port = Properties.Settings.Default.local_port;
            DbName = Properties.Settings.Default.local_dbname;
        }

        public ICommand TestCommand
        {
            get
            {
                if (_testCommand == null)
                    _testCommand = new RelayCommand(p => Test(), p => CanTest());

                return _testCommand;
            }
        }

        private void Test()
        {
            if (Dao.Admin.AppDao.TestDatabase(connection, DbName))
            {
                IsTested = true;
                MyMsgBox.Show("Connexion réussie !", "Humager", MyMsgBoxButton.OK, MyMsgBoxIcon.Success);
            }
            else
                MyMsgBox.Show("La connexion a échoué. Vous avez choisi une mauvaise base de données !", "Humager", MyMsgBoxButton.OK, MyMsgBoxIcon.Error);
        }

        private bool CanTest()
        {
            return Error == string.Empty && this["DbName"] == string.Empty &&
                (AppConfig.CurrentUser == null || AppConfig.CurrentUser.Type == UserType.ADMIN);
        }

        public ICommand DropDownCommand
        {
            get
            {
                if (_dropDownCommand == null)
                    _dropDownCommand = new AsyncCommand(DropDown, CanLoadData);

                return _dropDownCommand;
            }
        }

        private async Task DropDown(object p = null)
        {
            if (Error != string.Empty)
            {
                databases.Clear();
                return;
            }

            if (databases.Count == 0)
            {
                var feed = Dao.Admin.AppDao.CreateConnection(Serveur, User, Password, Port);
                if (feed is DbConnection)
                {
                    connection = (DbConnection)feed;
                    await LoadDatabases();
                }
                else
                {
                    MyMsgBox.Show(feed.ToString(), "Humager", MyMsgBoxButton.OK, MyMsgBoxIcon.Error);
                }
            }
        }

        public ICommand RefreshDatabaseCommand
        {
            get
            {
                if (_refreshCommand == null)
                    _refreshCommand = new AsyncCommand(LoadDatabases, CanLoadData);

                return _refreshCommand;
            }
        }

        async Task LoadDatabases(object p = null)
        {
            DatabaseLoading = true;

            databases.Clear();
            var list = await Task.Run(() => Dao.Admin.AppDao.GetDatabases(connection));

            list.ForEach(d => databases.Add(d));

            if (!string.IsNullOrWhiteSpace(DbName))
                DatabasesView.MoveCurrentTo(DbName);
            else
                DatabasesView.MoveCurrentToFirst();

            DatabaseLoading = false;
        }

        private bool CanLoadData(object p = null)
        {
            return Error == string.Empty && !DatabaseLoading &&
                (AppConfig.CurrentUser == null || AppConfig.CurrentUser.Type == UserType.ADMIN);
        }

        public string Error
        {
            get
            {
                if (this["Serveur"] != string.Empty)
                    return this["Serveur"];
                else if (this["User"] != string.Empty)
                    return this["User"];
                else if (this["Password"] != string.Empty)
                    return this["Password"];
                else if (this["Port"] != string.Empty)
                    return this["Port"];

                return string.Empty;
            }
        }

        public string this[string columnName]
        {
            get
            {
                var error = string.Empty;

                switch (columnName)
                {
                    case "Serveur":
                        if (string.IsNullOrWhiteSpace(Serveur))
                            error = "Le nom ou l'adresse IP du serveur doit être spécifié.";
                        break;
                    case "User":
                        if (string.IsNullOrWhiteSpace(User))
                            error = string.Format("Le nom d'utilisateur {0} doit être spécifié.", Properties.Settings.Default.local_db_invariant);
                        break;
                    case "Password":
                        if (string.IsNullOrWhiteSpace(Password))
                            error = string.Format("Le mot de passe d'utilisateur {0} doit être spécifié.", Properties.Settings.Default.local_db_invariant);
                        break;

                    case "Port":
                        if (string.IsNullOrWhiteSpace(Port))
                            error = string.Format("Le port d'écoute {0} doit être spécifié.", Properties.Settings.Default.local_db_invariant);
                        break;

                    case "DbName":
                        if (string.IsNullOrWhiteSpace(DbName))
                            error = "Le nom de la base de données doit être spécifié.";
                        break;
                    default:
                        break;
                }

                return error;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(p => Save(), p => CanSave());

                return _saveCommand;
            }
        }

        private void Save()
        {
            Status = string.Empty;

            Properties.Settings.Default.local_server = DbConfig.ServerName = Serveur;
            Properties.Settings.Default.local_user = DbConfig.DbUser = User;
            Properties.Settings.Default.local_port = DbConfig.DbPort = Port;
            Properties.Settings.Default.local_dbname = DbConfig.DbName = DbName;
            Properties.Settings.Default.local_pwd = DbConfig.DbPassword = Password;

            Properties.Settings.Default.Save();

            Password = string.Empty;

            Status = "Paramètres de base de données configurés avec succès.";
        }

        private bool CanSave()
        {
            return IsTested;
        }

        private void MenuInit()
        {
            Name = "Base de données";
            OptionItem = new OptionItem()
            {
                Name = "Base de données",
                ToolTip = "Base de données",
                IconPathData = "M23.706957,20.757375L23.572074,20.855012C21.309067,22.449249 17.432978,23.459015 12.852985,23.459015 8.2736155,23.459015 4.3977605,22.449249 2.134843,20.855012L2.0010125,20.758133 2.0010125,23.189667 2.0033857,23.251979C2.1454433,25.122744 6.7390391,27.176042 12.852985,27.176042 18.967913,27.176042 23.561526,25.122744 23.703583,23.251979L23.706957,23.163384z M29.001012,17.716992L31.001012,17.716992 31.001012,28.588021 29.001012,28.588021z M23.706957,15.039484L23.572177,15.137046C21.30917,16.731283 17.433079,17.741033 12.853083,17.741033 8.2736975,17.741033 4.3978363,16.731283 2.1349164,15.137046L2.0010125,15.040115 2.0010125,17.472641 2.0033857,17.534953C2.1454433,19.405717 6.7390391,21.459015 12.852985,21.459015 18.967913,21.459015 23.561526,19.405717 23.703583,17.534953L23.706957,17.446358z M23.706957,9.3243585L23.572074,9.4219947C21.309067,11.016233 17.432978,12.025999 12.852985,12.025999 8.2736155,12.025999 4.3977605,11.016233 2.134843,9.4219947L2.0010125,9.3251171 2.0010125,11.752799 2.0034567,11.816972C2.1455134,13.687776 6.7390791,15.741033 12.853083,15.741033 18.967929,15.741033 23.561625,13.687776 23.703686,11.816972L23.706957,11.73108z M12.852985,1.999999C6.7390391,1.999999,2.1454433,4.0532975,2.0033857,5.9240623L2.0010125,5.9863739 2.0010125,6.039638 2.0033857,6.1019816C2.1454433,7.9736695 6.7390391,10.025999 12.852985,10.025999 19.064976,10.025999 23.706971,7.907999 23.706971,6.0129991 23.706971,4.118999 19.064976,1.999999 12.852985,1.999999z M29.001012,1.4110222L31.001012,1.4110222 31.001012,12.282051 29.001012,12.282051z M12.852985,0C20.180976,0 25.706969,2.5849991 25.706969,6.0129991 25.706969,6.0665617 25.70562,6.1199183 25.702935,6.1730628L25.700741,6.2020068 25.706957,6.2020068 25.706957,11.728033 25.706957,11.732571 25.706957,13.588021 28.65651,13.588021 28.664582,13.574823C28.992448,13.092844 29.547608,12.776049 30.176987,12.776049 31.183995,12.776049 32.000001,13.587044 32.000001,14.588037 32.000001,15.588031 31.183995,16.400026 30.176987,16.400026 29.547608,16.400026 28.992448,16.08284 28.664582,15.600812L28.656758,15.58802 25.706957,15.58802 25.706957,17.446016 25.706957,17.446465 25.706957,23.163042 25.706957,23.16349 25.706957,23.263995 25.704425,23.263995 25.702935,23.323106C25.533841,26.671192 20.066476,29.176042 12.852985,29.176042 5.6404778,29.176042 0.17312732,26.671192 0.0040330888,23.323106L0.0029473306,23.280005 0.0010123253,23.280005 0.0010123253,23.203209 0,23.163042 0.0010123253,23.163042 0.0010123253,17.486183 0,17.446016 0.0010123253,17.446016 0.0010123253,11.765371 7.1525574E-05,11.728033 0.0010123253,11.728033 0.0010123253,6.0531659 0,6.0129991 0.0010123253,5.9728322 0.0010123253,5.7960129 0.0083503725,5.7960129 0.016076088,5.6941471C0.34713368,2.4259615,5.7549621,0,12.852985,0z"
            };

            var help = string.Empty;
            App.Tips.TryGetValue(Name, out help);
            Help = help;
        }

        public override void RefreshAccess(bool isOkay)
        {
            IsAccessible = AppConfig.CurrentUser == null || AppConfig.CurrentUser.Type == UserType.ADMIN;
        }

        protected override void Close(object param)
        {
            CloseDialogWithResult(param as Window, DialogResult.Yes);
        }

        private AsyncCommand _dropDownCommand;
        private AsyncCommand _refreshCommand;
        private RelayCommand _saveCommand;
        private RelayCommand _testCommand;
    }
}
