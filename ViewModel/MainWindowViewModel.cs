//using System;
//using System.Windows;
//using System.Windows.Input;
//using System.Windows.Threading;
//using FingerPrintManagerApp.Dialog.Facade;
//using FingerPrintManagerApp.Dialog.Service;
//using FingerPrintManagerApp.View;
//using FingerPrintManagerApp.ViewModel.Command;
//using FingerPrintManagerApp.ViewModel.Extension;
//using Ninject;
//using System.Collections.Generic;
//using FingerPrintManagerApp.Modules.Admin.ViewModel;
//using FingerPrintManagerApp.Modules.Employe.ViewModel;
//using FingerPrintManagerApp.Model;
//using FingerPrintManagerApp.Model.Util;
//using FingerPrintManagerApp.Dao;
//using FingerPrintManagerApp.Model.Admin;
//using FingerPrintManagerApp.Modules.Carriere.ViewModel;
//using FingerPrintManagerApp.Modules.Presence.ViewModel;

//namespace FingerPrintManagerApp.ViewModel
//{
//    public class MainWindowViewModel : ControllerViewModel
//    {
//        ILoginService loginService;
//        IDialogFacade facade;

//        DispatcherTimer timer; // Login Timer
//        DispatcherTimer timer2; // Sleep Timer
//        User autoLogoutUser = null;

//        bool autoLogout = false; 

//        public MainWindowViewModel(ILoginService loginService, IDialogFacade facade)
//        {
            
//            this.loginService = loginService;
//            this.facade = facade;

//            LoadViewModels();

//            var presenter = IoC.Container.Instance.Kernel.Get<MainWindowPresenterViewModel>();
//            presenter.MainController = this;

//            PresenterViewModel = presenter;
//            CurrentPageViewModel = PresenterViewModel;

//            PagesView.Filter = OnPageFilter;

//            FilterText = string.Empty;
            
//            timer2 = new DispatcherTimer { Interval = TimeSpan.FromMinutes(AppConfig.SleepDelay) };
//            timer2.Tick += timer2_Tick;

//            timer2.Start();
//            timer2.IsEnabled = false;

//            InputManager.Current.PostProcessInput += Current_PostProcessInput;
            
//        }

//        private void timer2_Tick(object sender, EventArgs e)
//        {
//            timer2.Stop();
//            autoLogoutUser = AppConfig.CurrentUser;
//            autoLogout = true;

//            //((App)Application.Current).Status = "Délai d'inactivité dépassé. Veuillez vous reconnecter.";

//            AppUtil.CloseAllDialogs();

//            Logout();

//            timer2.IsEnabled = false;

//            Login();
//        }

//        private void Current_PostProcessInput(object sender, ProcessInputEventArgs e)
//        {
//            if (timer2.IsEnabled && (e.StagingItem.Input is MouseButtonEventArgs || e.StagingItem.Input is KeyEventArgs || e.StagingItem.Input is MouseEventArgs))
//                timer2.Interval = TimeSpan.FromMinutes(AppConfig.SleepDelay);
//        }

//        int ticks = 0;
//        private void timer_Tick(object sender, EventArgs e)
//        {
//            timer.Stop();
//            timer.IsEnabled = false;
//            timer = null;

//            if (ConnectionHelper.GetConnection() == null)
//            {
//                var connector = new ConnectorViewModel();
//                connector.Title = "Configuration des paramètres de connexion";

//                facade.ShowDialog(connector, null);

//                if (ConnectionHelper.GetConnection() != null)
//                    Login();
//            }
//            else
//            {
//                Login();
//            }
//        }
        
//        public override PageViewModel CurrentPageViewModel
//        {
//            get => base.CurrentPageViewModel;
//            set
//            {
//                base.CurrentPageViewModel = value;
//                NotPresenter = CurrentPageViewModel != PresenterViewModel;

//                FilterText = string.Empty;
//            }
//        }

//        private bool _notPresenter;
//        public bool NotPresenter
//        {
//            get { return _notPresenter; }
//            set
//            {
//                if (_notPresenter != value)
//                {
//                    _notPresenter = value;
//                    RaisePropertyChanged(() => NotPresenter);
//                }
//            }
//        }

//        private string _filterText;
//        public string FilterText
//        {
//            get { return _filterText; }
//            set
//            {
//                if(_filterText != value)
//                {
//                    _filterText = value;
//                    RaisePropertyChanged(() => FilterText);
//                    PagesView.Refresh();
//                }
//            }
//        }

//        private bool OnPageFilter(object obj)
//        {
//            var page = obj as PageViewModel;
//            if (page == null)
//                return false;

//            var motif = (FilterText != null ? FilterText : string.Empty).NoAccent().ToLower();

//            return page.OptionItem.Name.NoAccent().ToLower().Contains(motif) || page.OptionItem.ToolTip.NoAccent().ToLower().Contains(motif);
//        }

//        static List<PageViewModel> pages = new List<PageViewModel>();

//        void LoadViewModels()
//        {
//            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<PresenceCtrlViewModel>());
//            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<CarriereCtrlViewModel>());
//            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<EmployeCtrlViewModel>());
//            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<AdminCtrlViewModel>());

//            foreach (var ctrl in pageViewModels)
//            {
//                ctrl.Parent = this;
//                pages.Add(ctrl);
//                ctrl.RefreshAccess(AppConfig.CurrentUser != null);
//                //((ControllerViewModel)ctrl).InitView();
//            }

//        }

//        public ICommand LoadCommand
//        {
//            get
//            {
//                if (_loadCommand == null)
//                    _loadCommand = new RelayCommand(p => Load());

//                return _loadCommand;
//            }
//        }

//        private void Load()
//        {
//            if (!IsInit)
//            {
//                timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
//                timer.Tick += timer_Tick;

//                timer.Start();
//                timer.IsEnabled = true;

//                IsInit = true;
//            }
//        }
        
//        public ICommand LoginCommand
//        {
//            get
//            {
//                if (_loginCommand == null)
//                    _loginCommand = new RelayCommand(p => Login(), p => CanLogin());

//                return _loginCommand;
//            }
//        }

//        private void Login()
//        {
//            if (ConnectionHelper.GetConnection() != null)
//            {
//                loginService.Login();

//                if (loginService.ConnectedUser != null)
//                {
//                    var user = (User)loginService.ConnectedUser;
//                    ((App)Application.Current).User = user;
//                    AppConfig.CurrentUserEntite = user.Entite;

//                    if (autoLogout && !user.Equals(autoLogoutUser))
//                        Reinit(false);


//                    timer2.IsEnabled = true;
//                    timer2.Start();

//                    autoLogout = false;
//                }

//                RefreshAccess(AppConfig.CurrentUser != null);
//            }
//            else
//            {
//                var connector = new ConnectorViewModel();
//                connector.Title = "Configuration des paramètres de connexion";

//                facade.ShowDialog(connector, null);

//                if (ConnectionHelper.GetConnection() != null)
//                    Login();
//            }
//        }

//        private bool CanLogin()
//        {
//            return AppConfig.CurrentUser == null;
//        }

//        public ICommand LogoutCommand
//        {
//            get
//            {
//                if (_logoutCommand == null)
//                    _logoutCommand = new RelayCommand(p => Logout(), p => CanLogout());

//                return _logoutCommand;
//            }
//        }

//        private void Logout()
//        {
//            ((App)Application.Current).User = null;
//            Reinit(autoLogout);
//        }

//        private bool CanLogout()
//        {
//            return AppConfig.CurrentUser != null;
//        }

//        public ICommand EditAccountCommand
//        {
//            get
//            {
//                if (_editAccountCommand == null)
//                    _editAccountCommand = new RelayCommand(p => EditAccount(p), p => CanEdit());

//                return _editAccountCommand;
//            }
//        }

//        private void EditAccount(object p)
//        {
//            loginService.Login(true, p as Window);
//        }

//        private bool CanEdit()
//        {
//            return AppConfig.CurrentUser != null;
//        }

//        public ICommand AboutCommand
//        {
//            get
//            {
//                if (_aboutCommand == null)
//                    _aboutCommand = new RelayCommand(p => About());

//                return _aboutCommand;
//            }
//        }

//        private void About()
//        {
//            new About().ShowDialog();
//        }

//        public override void RefreshAccess(bool isOkay)
//        {
//            foreach (var ctrl in pageViewModels)
//                ctrl.RefreshAccess(isOkay);
//        }

//        public ICommand ClosingCommand
//        {
//            get
//            {
//                if (_closingCommand == null)
//                    _closingCommand = new RelayCommand(p => Close(p));

//                return _closingCommand;
//            }
//        }

//        private void Close(object param)
//        {
//            var e = param as System.ComponentModel.CancelEventArgs;

//            if (e != null)
//            {
//                if (MyMsgBox.Show("Voulez-vous vraiment abondonner la session et fermer l'application ?", "Humager", MyMsgBoxButton.YesNoCancel, MyMsgBoxIcon.Warning) == DialogueResult.Yes)
//                {
//                    if (AppConfig.CurrentUser != null)
//                    {
//                        Dao.Admin.LogUtil.AddEntry(
//                            AppConfig.CurrentUser,
//                             Model.Admin.Util.DbUtil.Entity.Utilisateur + "",
//                            string.Format("Déconnexion de l'utilisateur '{0}' (ID : {1}).", AppConfig.CurrentUser, AppConfig.CurrentUser.Id)
//                        );
//                    }

//                    Environment.Exit(0);
//                }
//                else
//                    e.Cancel = true;
//            }
//        }

//        public static PageViewModel Get(Type type)
//        {
//            foreach (var vm in pages)
//            {
//                var page = ((ControllerViewModel)vm).GetPage(type);

//                if (page != null)
//                    return page;
//            }

//            return null;
//        }

//        private RelayCommand _loadCommand;
//        private RelayCommand _loginCommand;
//        private RelayCommand _logoutCommand;
//        private RelayCommand _editAccountCommand;
//        private RelayCommand _aboutCommand;
//        private RelayCommand _closingCommand;
//    }
//}
