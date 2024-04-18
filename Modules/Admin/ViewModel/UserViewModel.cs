using FingerPrintManagerApp.Extension;
using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Admin;
using FingerPrintManagerApp.Model.Admin.Util;
using FingerPrintManagerApp.Model.Employe;
using FingerPrintManagerApp.ViewModel;
using FingerPrintManagerApp.ViewModel.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace FingerPrintManagerApp.Modules.Admin.ViewModel
{
    public class UserViewModel : PageViewModel, IDataErrorInfo
    {
        private static object _lock = new object();
        private ObservableCollection<User> users;
        private ObservableCollection<Entite> entites;

        private Array sexes;
        private Array types;

        private bool editing = false;
        public ICollectionView UsersView { get; private set; }
        public ICollectionView SexesView { get; private set; }
        public ICollectionView TypesView { get; private set; }
        public ICollectionView EntitesView { get; private set; }
        
        public UserViewModel()
        {
            users = new ObservableCollection<User>();
            BindingOperations.EnableCollectionSynchronization(users, _lock);

            UsersView = (CollectionView)CollectionViewSource.GetDefaultView(users);
            // Sorting
            UsersView.SortDescriptions.Add(new SortDescription("Nom", ListSortDirection.Ascending));
            UsersView.SortDescriptions.Add(new SortDescription("Prenom", ListSortDirection.Ascending));

            //Filtering
            UsersView.Filter = OnFilterUser;

            entites = new ObservableCollection<Entite>();
            EntitesView = (CollectionView)CollectionViewSource.GetDefaultView(entites);
            // Sorting
            EntitesView.SortDescriptions.Add(new SortDescription("EstPrincipale", ListSortDirection.Descending));
            EntitesView.SortDescriptions.Add(new SortDescription("Zone.Nom", ListSortDirection.Ascending));

            types = Enum.GetValues(typeof(UserType));
            TypesView = (CollectionView)CollectionViewSource.GetDefaultView(types);

            sexes = Enum.GetValues(typeof(Sex));
            SexesView = (CollectionView)CollectionViewSource.GetDefaultView(sexes);
            
            FilterText = string.Empty;

            InitSave();
            MenuInit();
        }

        private bool OnFilterUser(object obj)
        {
            var user = obj as User;

            if (user != null)
            {
                var motif = FilterText.ToLower().Trim().NoAccent();

                return user.UserName.ToLower() != "sysadmin" && (user.UserName.ToLower().NoAccent().Contains(motif)
                    || user.Nom.ToLower().NoAccent().Contains(motif) || user.Prenom.ToLower().NoAccent().Contains(motif));
            }

            return false;
        }

        private UserType _selectedType;
        public UserType SelectedType
        {
            get
            {
                return this._selectedType;
            }
            set
            {
                if (_selectedType != value)
                {
                    _selectedType = value;
                    RaisePropertyChanged(() => SelectedType);

                    User.Type = SelectedType;

                    IsUser = SelectedType == UserType.USER;
                }
            }
        }
        
        private bool _isUser;
        public bool IsUser
        {
            get
            {
                return this._isUser;
            }
            set
            {
                if (_isUser != value)
                {
                    _isUser = value;
                    RaisePropertyChanged(() => IsUser);
                }
            }
        }

        private bool _withPassword;
        public bool WithPassword
        {
            get
            {
                return this._withPassword;
            }
            set
            {
                if (_withPassword != value)
                {
                    _withPassword = value;
                    RaisePropertyChanged(() => WithPassword);
                }
            }
        }

        private bool _withIdentity;
        public bool WithIdentity
        {
            get
            {
                return this._withIdentity;
            }
            set
            {
                if (_withIdentity != value)
                {
                    _withIdentity = value;
                    RaisePropertyChanged(() => WithIdentity);
                }
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    User.PassWd = value;
                    RaisePropertyChanged(() => Password);
                    RaisePropertyChanged(() => ConfirmedPassword);
                }
            }
        }

        private string _confirmedPassword;
        public string ConfirmedPassword
        {
            get
            {
                return this._confirmedPassword;
            }
            set
            {
                if (_confirmedPassword != value)
                {
                    _confirmedPassword = value;
                    RaisePropertyChanged(() => ConfirmedPassword);
                }
            }
        }

        private int _count;
        public int UserCount
        {
            get
            {
                return this._count;
            }
            set
            {
                if (_count != value)
                {
                    _count = value;
                    RaisePropertyChanged(() => UserCount);
                }
            }
        }

        private bool _userLoading;
        public bool UserLoading
        {
            get
            {
                return this._userLoading;
            }
            set
            {
                if (_userLoading != value)
                {
                    _userLoading = value;
                    RaisePropertyChanged(() => UserLoading);
                }
            }
        }

        private bool _drillingLoading;
        public bool DrillingLoading
        {
            get
            {
                return this._drillingLoading;
            }
            set
            {
                if (_drillingLoading != value)
                {
                    _drillingLoading = value;
                    RaisePropertyChanged(() => DrillingLoading);
                }
            }
        }

        private bool _roleLoading;
        public bool RoleLoading
        {
            get
            {
                return this._roleLoading;
            }
            set
            {
                if (_roleLoading != value)
                {
                    _roleLoading = value;
                    RaisePropertyChanged(() => RoleLoading);
                }
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return this._status;
            }
            set
            {
                _status = value;
                RaisePropertyChanged(() => Status);
            }
        }

        private string _filterText;
        public string FilterText
        {
            get
            {
                return this._filterText;
            }
            set
            {
                if (_filterText != value)
                {
                    _filterText = value;
                    UsersView.Refresh();
                    RaisePropertyChanged(() => FilterText);
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

        private User _user;
        public User User
        {
            get
            {
                return this._user;
            }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    RaisePropertyChanged(() => User);
                }
            }
        }

        #region Commands
        protected override async Task LoadData()
        {
            UpdateTimer.IsEnabled = false;

            if (entites.Count == 0)
                await LoadEntites();
            //else if (AppConfig.CurrentUser.Entite.EstPrincipale)
            //    await LoadEntitesChanges();

            if (users.Count > 0)
                await LoadUserChanges();
            else
                await LoadUsers();

            LastDataUpdateTime = DateTime.Now;

            UpdateTimer.IsEnabled = true;
        }

        protected override async Task Load(object param = null)
        {
            Status = string.Empty;

            EntityRight = AppConfig.CurrentUser?.Type == UserType.ADMIN;

            if (!EntityRight)
                return;

            if (!IsInit)
            {
                await LoadEntites();
                await LoadUsers();
                IsInit = true;
            }
        }

        private async Task LoadUsers(object param = null)
        {
            UserLoading = true;

            users.Clear();

            //UserCount = new Dao.Admin.UserDao().Count(AppConfig.CurrentUser.Entite);

            //await Task.Run(() => new Dao.Admin.UserDao().GetUsersAsync(AppConfig.CurrentUser.Entite, users));

            UserLoading = false;
        }

        async Task LoadUserChanges()
        {
            //UserCount = new Dao.Admin.UserDao().Count(AppConfig.CurrentUser.Entite);

            //var list = await Task.Run(() => new Dao.Admin.UserDao().GetAllAsync(AppConfig.CurrentUser.Entite, LastDataUpdateTime.AddSeconds(-5)));

            //list.ForEach(d => {
            //    var _d = users.ToList().Find(e => e.Equals(d));

            //    if (_d != null) users.Remove(_d);

            //    users.Add(d);
            //});

            //UsersView.Refresh();
        }

        private async Task LoadEntites(object param = null)
        {
            //entites.Clear();

            //if (!AppConfig.CurrentUser.Entite.EstPrincipale)
            //{
            //    entites.Add(AppConfig.CurrentUser.Entite);
            //    EntitesView.MoveCurrentToFirst();

            //    return;
            //}

            //var list = await Task.Run(() => new Dao.Employe.EntiteDao().GetAllAsync());
            //list.ForEach(e => entites.Add(e));

            //EntitesView.MoveCurrentToFirst();
        }

        async Task LoadEntitesChanges()
        {
            var list = await Task.Run(() => new Dao.Employe.EntiteDao().GetAllAsync(LastDataUpdateTime.AddSeconds(-5)));

            list.ForEach(d => {
                var _d = entites.ToList().Find(e => e.Equals(d));

                if (_d != null) entites.Remove(_d);

                entites.Add(d);
            });

            EntitesView.Refresh();
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

            if (!editing)
            {
                if (new Dao.Admin.UserDao().Add(User) > 0)
                {
                    Dao.Admin.LogUtil.AddEntry(
                        AppConfig.CurrentUser,
                        DbUtil.Entity.Utilisateur + "",
                        string.Format("Ajout de l'utilisateur '{0}' (ID : {1}).", User, User.Id)
                        );

                    users.Add(User);
                    UserCount++;
                    UsersView.Refresh();
                    Status = "Utilisateur enregistré avec succès !";
                    InitSave();
                }
                else
                    Status = "Ajout non abouti. Vérifiez votre connexion au réseau puis réessayer !";
            }
            else
            {
                if (WithIdentity)
                {
                    var clone = (User)User.Clone();
                    clone.Id = string.Empty;

                    users.Remove(User);

                    if (!users.Contains(clone))
                    {
                        if (new Dao.Admin.UserDao().Update(User, editable) > 0)
                        {
                            Dao.Admin.LogUtil.AddEntry(
                                AppConfig.CurrentUser,
                                DbUtil.Entity.Utilisateur + "",
                                string.Format("Modification de l'utilisateur '{0}' (ID : {1}).", clone, User.Id)
                            );

                            Status = "Utilisateur modifié avec succès !";
                            users.Add(User);
                            UsersView.Refresh();
                            InitSave();
                        }
                        else
                        {
                            Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Utilisateur + "",
                            string.Format("Tentative de modification sans succès de l'utilisateur '{0}' (ID : {1}).", clone, User.Id)
                            );

                            Status = "Modification non abouti. Vérifiez votre connexion au réseau puis réessayer !";
                        }
                    }
                    else
                    {

                        Status = "Un utilisateur avec la même description existe déjà !";
                        User.CancelEdit();

                        users.Add(User);
                        UsersView.Refresh();
                    }
                }
                else
                {
                    if (new Dao.Admin.UserDao().SetPasswd(User, Password) > 0)
                    {
                        Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Utilisateur + "",
                            string.Format("Réinitialisation du mot de passe de l'utilisateur '{0}' (ID : {1}).", User, User.Id)
                        );
                        
                        Status = "Mot de passe réinitialisé avec succès !";
                        InitSave();
                    }
                    else
                        Status = "Réinitialisation non abouti. Vérifiez votre connexion au réseau puis réessayer !";
                }
            }
        }

        private void InitSave()
        {
            User = new User();
            User.Sex = Sex.Homme;

            TypesView.MoveCurrentToFirst();
            SexesView.MoveCurrentToFirst();

            WithIdentity = WithPassword = true;
            Password = ConfirmedPassword = string.Empty;

            Title = "Nouvel utilisateur";
            Action = "Enregistrer";
            editing = false;
        }

        private void MenuInit()
        {
            Name = "Utilisateur";
            OptionItem = new OptionItem()
            {
                Name = Name,
                ToolTip = Name,
                IconPathData = "M16,2.0050001C12.416,2.0050001 9.5,5.0289993 9.5,8.7459979 9.5,12.462997 12.416,15.487996 16,15.487997 19.584,15.487996 22.5,12.462997 22.5,8.7459979 22.5,5.0289993 19.584,2.0050001 16,2.0050001z M16,0C20.687,0 24.5,3.9239988 24.5,8.7459979 24.5,11.760372 23.010548,14.423184 20.74917,15.996397L20.493732,16.165044 20.752514,16.244553C27.261414,18.335448,32,24.603727,32,31.991016L30,31.999989C30,24.00401 23.719971,17.505989 16,17.505989 8.2800293,17.505989 2,24.00401 2,31.991016L0,31.999989 0,31.991016C0,24.603727,4.7385874,18.335448,11.247486,16.244553L11.506267,16.165044 11.25083,15.996397C8.9894533,14.423184 7.5,11.760372 7.5,8.7459979 7.5,3.9239988 11.313,0 16,0z"
            };

            var help = string.Empty;
            App.Tips.TryGetValue(Name, out help);
            Help = help;
        }

        private bool CanSave()
        {
             return AppConfig.CurrentUser?.Type == UserType.ADMIN && User.Error == string.Empty && (!WithPassword || Error == string.Empty);
        }

        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand(p => Cancel(), p => CanCancel());

                return _cancelCommand;
            }
        }

        private void Cancel()
        {
            if (editing)
                User.CancelEdit();

            InitSave();
        }

        private bool CanCancel()
        {
            return true;
        }

        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new RelayCommand(p => Edit(p), p => CanEdit(p));

                return _editCommand;
            }
        }

        private User editable;
        private void Edit(object param)
        {
            if (param is User)
            {
                var user = (User)param;
                editable = user.Clone() as User;

                SelectedType = user.Type;

                User = user;
                User.BeginEdit();
                
                WithPassword = false;

                editing = true;
                Action = "Modifier";
                Title = "Modification de l'utiliateur";
            }
        }

        private bool CanEdit(object param)
        {
            return AppConfig.CurrentUser?.Type == UserType.ADMIN && !editing;
        }

        public ICommand PasswordReinitCommand
        {
            get
            {
                if (_passwordReinitCommand == null)
                    _passwordReinitCommand = new RelayCommand(p => Reinit(p), p => CanEdit(p));

                return _passwordReinitCommand;
            }
        }

        private void Reinit(object param)
        {
            if (param is User)
            {
                var user = (User)param;

                SelectedType = user.Type;

                User = user;
                User.BeginEdit();

                WithIdentity = false;

                editing = true;
                Action = "Modifier";
            }
        }
        
        public string Error
        {
            get
            {
                if (this["Password"] != string.Empty)
                    return this["Password"];
                else if (this["ConfirmedPassword"] != string.Empty)
                    return this["ConfirmedPassword"];

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
                    case "Password":
                        if (!ValueValidator.IsStrongPassword(Password))
                            error = "Le mot de passe saisi ne vérifie pas les critères définis.";
                        break;

                    case "ConfirmedPassword":
                        if (Password != ConfirmedPassword)
                            error = "Les deux mots de passe ne correspondent pas.";
                        break;
                        
                    default:
                        break;
                }

                return error;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(p => Delete(p), p => CanDelete(p));

                return _deleteCommand;
            }
        }

        private void Delete(object param)
        {
            if (param is User)
            {
                if (MyMsgBox.Show("Etes-vous sûr(e) de vouloir supprimer cet utilisateur ?", "Humager", MyMsgBoxButton.YesNoCancel, MyMsgBoxIcon.Warning) != DialogueResult.Yes)
                    return;

                var user = (User)param;
                if (new Dao.Admin.UserDao().Delete(user) > 0)
                {
                    Dao.Admin.LogUtil.AddEntry(
                        AppConfig.CurrentUser,
                        DbUtil.Entity.Utilisateur + "",
                        string.Format("Suppression de l'utilisateur '{0}' (ID : {1}).", user, user.Id)
                        );

                    users.Remove(user);
                    UserCount--;

                    Status = "Utilisateur supprimé avec succès";
                }
                else
                {
                    Dao.Admin.LogUtil.AddEntry(
                        AppConfig.CurrentUser,
                        DbUtil.Entity.Utilisateur + "",
                        string.Format("Tentative de suppression sans succès de l'utilisateur '{0}' (ID : {1}).", user, user.Id)
                        );

                    Status = "Impossible de supprimer cet utilisateur pour l'instant. Rassurez-vous que vous êtes toujours connecté(e) au réseau ou que cet utilisateur " +
                        "n'est pas associé à un quelconque autre objet.";
                }
            }
        }

        private bool CanDelete(object param)
        {
            return AppConfig.CurrentUser?.Type == UserType.ADMIN && !editing;
        }

        public ICommand LockUnlockCommand
        {
            get
            {
                if (_lockUnlockCommand == null)
                    _lockUnlockCommand = new RelayCommand(p => LockUnlock(p), p => CanLock(p));

                return _lockUnlockCommand;
            }
        }

        private void LockUnlock(object param)
        {
            if (param is User)
            {
                var user = (User)param;

                if (user.Etat == UserState.Fonctionnel)
                {
                    var msg = string.Format("Vous êtes sur le point de bloquer l'utilisateur {0}. En le bloquant, ce dernier n'aura plus accès au système jusqu'à ce qu'il sera débloqué de nouveau.\nSouhaitez-vous continuer ? ", user);
                    if (MyMsgBox.Show(msg, "Humager", MyMsgBoxButton.YesNoCancel, MyMsgBoxIcon.Warning) != DialogueResult.Yes)
                        return;

                    if (new Dao.Admin.UserDao().Lock(user) > 0)
                    {
                        Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Utilisateur + "",
                            string.Format("Blocage de l'utilisateur '{0}' (ID : {1}).", user, user.Id)
                            );
                        
                        Status = "Utilisateur bloqué avec succès";
                    }
                    else
                        Status = "Blocage non abouti. Vérifiez votre connexion au serveur puis réssayer.";
                }
                else
                {
                    var msg = string.Format("Vous êtes sur le point de débloquer l'utilisateur {0}. En le débloquant, ce dernier pourra de nouveau accéder au système.\nSouhaitez-vous continuer ? ", user);
                    if (MyMsgBox.Show(msg, "Humager", MyMsgBoxButton.YesNoCancel, MyMsgBoxIcon.Warning) != DialogueResult.Yes)
                        return;

                    if (new Dao.Admin.UserDao().Unlock(user) > 0)
                    {
                        Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Utilisateur + "",
                            string.Format("Déblocage de l'utilisateur '{0}' (ID : {1}).", user, user.Id)
                            );

                        Status = "Utilisateur débloqué avec succès";
                    }
                    else
                        Status = "Déblocage non abouti. Vérifiez votre connexion au serveur puis réssayer.";
                }
            }
        }

        private bool CanLock(object param)
        {
            var user = param as User;
            return user != AppConfig.CurrentUser && AppConfig.CurrentUser?.Type == UserType.ADMIN && !editing;
        }

        public override void RefreshAccess(bool isOaky)
        {
            IsAccessible = AppConfig.CurrentUser != null && AppConfig.CurrentUser.Type == UserType.ADMIN;
        }

        private RelayCommand _saveCommand;
        private RelayCommand _cancelCommand;
        private RelayCommand _editCommand;
        private RelayCommand _deleteCommand;
        private RelayCommand _lockUnlockCommand;
        private RelayCommand _passwordReinitCommand;

        #endregion
    }
}
