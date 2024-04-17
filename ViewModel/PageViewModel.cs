
using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.ViewModel.Command;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace FingerPrintManagerApp.ViewModel
{
    public class PageViewModel : ViewModelBase
    {
        public PageViewModel()
        {
            UpdateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(4) };
            UpdateTimer.Tick += UpdateTimerTick;

            UpdateTimer.Start();
            UpdateTimer.IsEnabled = false;
            
        }

        private async void UpdateTimerTick(object sender, EventArgs e)
        {
            try
            {
                await LoadData();
            }
            catch (Exception)
            {
            }
            
        }

        protected string _help;
        protected string _name;
        protected bool _isInit;
        protected bool _isVisible;
        private bool _accessible;
        protected OptionItem _selectedItem;

        public OptionItem OptionItem { get; set; }

        public virtual OptionItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if(_selectedItem != value)
                {
                    _selectedItem = value;
                    RaisePropertyChanged(() => SelectedItem);
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }
        public string Help
        {
            get { return _help; }
            set
            {
                if (_help != value)
                {
                    _help = value;
                    RaisePropertyChanged(() => Help);
                }
            }
        }
        public virtual bool IsInit
        {
            get { return _isInit; }
            set
            {
                if (_isInit != value)
                {
                    _isInit = value;
                    RaisePropertyChanged(() => IsInit);
                }
            }
        }
        public virtual bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    RaisePropertyChanged(() => IsVisible);
                }
            }
        }
        public bool IsAccessible
        {
            get { return _accessible; }
            set
            {
                if (_accessible != value)
                {
                    _accessible = value;
                    RaisePropertyChanged(() => IsAccessible);
                }
            }
        }

        public DateTime LastDataUpdateTime { get; set; }

        public ObservableCollection<OptionItem> SegmentedOptions { get; set; }

        protected DispatcherTimer UpdateTimer;

        public virtual void RefreshAccess(bool isOkay)
        {

        }

        protected PageViewModel _parent;
        public PageViewModel Parent
        {
            get { return _parent; }
            set
            {
                if (_parent != value)
                {
                    _parent = value;
                    RaisePropertyChanged(() => Parent);
                }
            }
        }

        public async Task NavigateTo(Type pageType, object param = null)
        {
            if (this is ControllerViewModel)
            {
                var ctrl = (ControllerViewModel)this;

                var page = ctrl.GetPage(pageType);

                if (page != null)
                {
                    ((ControllerViewModel)this).CurrentPageViewModel = page;
                    if (param != null)
                        await page.Activate(param);
                }
                    
            }

            await Parent?.NavigateTo(GetType());
        }

        public virtual async Task Activate(object param = null)
        {
            Parent?.NavigateTo(GetType());
            //await Parent?.Activate(param);
        }

        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                    _loadCommand = new AsyncCommand(p => Load(p), p => CanLoad(p));

                return _loadCommand;
            }
        }

        /// <summary>
        /// Call after a view is loaded
        /// </summary>
        protected virtual async Task Load(object param)
        {
            IsVisible = true;
            UpdateTimer.IsEnabled = true;
        }

        protected virtual bool CanLoad(object param)
        {
            return true;
        }

        /// <summary>
        /// Call after a view is unloaded
        /// </summary>
        public virtual void Unload(object param = null)
        {
            IsVisible = false;
            UpdateTimer.IsEnabled = false;
        }

        /// <summary>
        /// Refresh data in views after a timer tick to reflete the system state
        /// </summary>
        protected virtual async Task LoadData()
        {
        }

        /// <summary>
        /// Invokable by Views which have a ListView
        /// </summary>
        public ICommand ScrollingCommand
        {
            get
            {
                if (_scrollingCommand == null)
                    _scrollingCommand = new RelayCommand(p => Scroll(p));

                return _scrollingCommand;
            }
        }

        protected virtual void Scroll(object param)
        {
            
        }

        protected AsyncCommand _loadCommand;
        protected RelayCommand _scrollingCommand;
    }
}
