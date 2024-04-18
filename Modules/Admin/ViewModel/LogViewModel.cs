using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Admin;
using FingerPrintManagerApp.Model.Admin.Util;
using FingerPrintManagerApp.ViewModel;
using FingerPrintManagerApp.ViewModel.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FingerPrintManagerApp.Modules.Admin.ViewModel
{
    public class LogViewModel : PageViewModel
    {
        private static object _lock = new object();

        private ObservableCollection<Model.Admin.EntryLog> entries { get; set; }
        private ObservableCollection<string> entitiesFilters { get; set; }
        private ObservableCollection<object> usersFilters { get; set; }

        public ICollectionView EntriesView { get; private set; }
        public ICollectionView EntitiesCV { get; private set; }
        public ICollectionView UsersCV { get; private set; }

        public LogViewModel()
        {
            entries = new ObservableCollection<Model.Admin.EntryLog>();
            BindingOperations.EnableCollectionSynchronization(entries, _lock);

            EntriesView = (CollectionView)CollectionViewSource.GetDefaultView(entries);

            EntriesView.Filter += OnEntryFilter;

            entitiesFilters = new ObservableCollection<string>() { "Tous" };
            var entities = Enum.GetValues(typeof(DbUtil.Entity));
            foreach (var ent in entities)
                entitiesFilters.Add(ent + "");
            
            EntitiesCV = (CollectionView)CollectionViewSource.GetDefaultView(entitiesFilters);
            SelectedEntity = "Tous";

            FromDate = DateTime.Today;
            ToDate = DateTime.Today;

            usersFilters = new ObservableCollection<object>() { "Tous" };
            UsersCV = (CollectionView)CollectionViewSource.GetDefaultView(usersFilters);

            FilterText = string.Empty;

            MenuInit();
        }

        private bool OnEntryFilter(object param)
        {
            var entry = param as Model.Admin.EntryLog;
            if (entry == null)
                return false;

            return (SelectedEntity == "Tous" || entry.Entity == SelectedEntity) && (SelectedUser == null || SelectedUser.Equals("Tous") || entry.User.Equals(SelectedUser)) && 
                (entry.IPAddress.StartsWith(FilterText.ToUpper().Trim()) || entry.MachineName.StartsWith(FilterText.ToUpper().Trim()) || 
                entry.Event.ToUpper().Contains(FilterText.ToUpper().Trim()));
        }

        private bool _running;
        public bool Running
        {
            get
            {
                return this._running;
            }
            set
            {
                if (_running != value)
                {
                    _running = value;
                    RaisePropertyChanged(() => Running);
                }
            }
        }

        private bool changesDone = false;

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
                    RaisePropertyChanged(() => FilterText);
                    EntriesView.Refresh();
                }
            }
        }

        private string _selectedEntity;
        public string SelectedEntity
        {
            get
            {
                return this._selectedEntity;
            }
            set
            {
                if (_selectedEntity != value)
                {
                    _selectedEntity = value;
                    RaisePropertyChanged(() => SelectedEntity);

                    EntriesView.Refresh();
                }
            }
        }

        private object _selectedUser;
        public object SelectedUser
        {
            get
            {
                return this._selectedUser;
            }
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    RaisePropertyChanged(() => SelectedUser);
                    EntriesView.Refresh();
                }
            }
        }

        void UpdateTitle()
        {
            if (FromDate == ToDate)
                Title = string.Format("Opérations du {0}", FromDate.ToShortDateString());
            else
            {
                Title = string.Format("Opérations du {0} au {1}", FromDate.ToShortDateString(), ToDate.ToShortDateString());
            }
        }

        private DateTime _fromDate;
        public DateTime FromDate
        {
            get
            {
                return this._fromDate;
            }
            set
            {
                if (_fromDate != value)
                {
                    _fromDate = value;
                    changesDone = true;
                    RaisePropertyChanged(() => FromDate);
                }
            }
        }

        private DateTime _toDate;
        public DateTime ToDate
        {
            get
            {
                return this._toDate;
            }
            set
            {
                if (_toDate != value)
                {
                    _toDate = value;
                    changesDone = true;
                    RaisePropertyChanged(() => ToDate);
                }
            }
        }

        public IAsyncCommand GoCommand
        {
            get
            {
                if (_goCommand == null)
                {
                    _goCommand = new AsyncCommand(LoadEntries, CanLoadEntries);
                }

                return _goCommand;
            }
        }

        int loadCount = 0;
        
        protected override async Task Load(object param)
        {
            if (!IsInit)
            {
                await LoadUsers();
                await LoadEntries(null);
                IsInit = true;
            }

            if (++loadCount == 3)
            {
                await LoadEntries(null);
                loadCount = 0;
            }

        }

        public IAsyncCommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new AsyncCommand(Refresh, CanRefresh);
                }
                return _refreshCommand;
            }
        }

        public IAsyncCommand RefreshUserCommand
        {
            get
            {
                if (_refreshUserCommand == null)
                {
                    _refreshUserCommand = new AsyncCommand(RefreshUser, CanRefresh);
                }
                return _refreshUserCommand;
            }
        }

        private async Task RefreshUser(object param)
        {
            await LoadUsers();
        }

        private async Task LoadUsers()
        {
            Running = true;

            usersFilters.Clear();
            usersFilters.Add("Tous");

            var list = await Task.Run(() => new Dao.Admin.UserDao().GetUsersAsync());
            list.ForEach(u => usersFilters.Add(u));

            SelectedUser = usersFilters[0];

            Running = false;
        }

        private async Task LoadEntries(object p)
        {
            Running = true;

            UpdateTitle();

            entries.Clear();

            await Task.Run(() => new Dao.Admin.EntryLogDao().GetLogEntriesAsync(entries, FromDate, ToDate));

            changesDone = false;

            Running = false;
        }

        private bool CanLoadEntries(object p)
        {
            return changesDone && !Running && FromDate > new DateTime() && ToDate > new DateTime() && FromDate <= ToDate;
        }

        private async Task Refresh(object param)
        {
            await LoadEntries(param);
        }

        private bool CanRefresh(object param)
        {
            return !Running;
        }

        private void MenuInit()
        {
            Name = "Fichier Log";
            OptionItem = new OptionItem()
            {
                Name = "Fichier Log",
                ToolTip = "Fichier Log",
                IconPathData = "M29.200012,18.300007C28.400024,18.300007 27.800018,18.900013 27.800018,19.700016 27.800018,20.500004 28.400024,21.100012 29.200012,21.100012 30,21.100012 30.600006,20.500004 30.600006,19.700016 30.600006,18.900013 30,18.300007 29.200012,18.300007z M28.100006,16.900013C30.300018,16.900013 32,18.60001 32,20.800009 32,23.000006 30.200012,24.700018 28.100006,24.700018 27.600006,24.700018 27.100006,24.600012 26.700012,24.500006L24.600006,26.600012 23.200012,26.600012 23.200012,28.000006 21.800018,28.000006 21.800018,29.400002 19.5,29.400002 19.5,27.000006 24.300018,22.200018C24.200012,21.700018 24.100006,21.300009 24.100006,20.800009 24.200012,18.60001 25.900024,16.900013 28.100006,16.900013z M13.200012,0L13.300018,0C18.400024,-7.3981937E-08 19.300018,3.6000069 19.200012,5.000001 19.100006,6.1000076 18.800018,7.6000076 18.800018,7.6000076 18.800018,7.6000076 19.300018,7.800005 19.300018,8.800005 19.100006,11.300006 18.100006,10.200014 17.900024,11.300006 17.5,13.400012 16.700012,13.500003 16.400024,14.300006 16.300018,14.500003 16.300018,14.700016 16.300018,14.900013 16.300018,17.300007 17.5,18.400013 21.100006,19.700016 21.900024,20.000004 22.600006,20.300007 23.200012,20.500004L23.200012,20.700018C23.200012,21.100012,23.200012,21.400015,23.300018,21.800009L18.600006,26.500006 18.600006,29.20002 0,29.20002 0,23.200018C0,22.300009 1.6000061,20.900015 5.3000183,19.60001 9,18.300007 10.100006,17.200016 10.100006,14.800007 10.100006,14.500003 10.100006,14.300006 10,14.200015 9.7000122,13.500003 8.9000244,13.400012 8.5,11.200015 8.3000183,10.100008 7.3000183,11.200015 7.1000061,8.7000142 7.1000061,7.7000142 7.6000061,7.5000015 7.6000061,7.5000015 7.6000061,7.5000015 7.4000244,6.1000076 7.3000183,5.000001 7.2000122,3.6000069 8.1000061,-7.3981937E-08 13.200012,0z"
            };

            var help = string.Empty;
            App.Tips.TryGetValue(Name, out help);
            Help = help;
        }

        public override void RefreshAccess(bool isOkay)
        {
            IsAccessible = AppConfig.CurrentUser != null && AppConfig.CurrentUser.Type == UserType.ADMIN;
        }

        private AsyncCommand _refreshCommand;
        private AsyncCommand _refreshUserCommand;
        private AsyncCommand _goCommand;

    }
}
