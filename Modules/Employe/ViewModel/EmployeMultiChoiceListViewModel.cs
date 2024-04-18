using FingerPrintManagerApp.Dao.Employe;
using FingerPrintManagerApp.Dialog.Service;
using FingerPrintManagerApp.Extension;
using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.ViewModel.Command;
using FingerPrintManagerApp.ViewModel.Contract;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace FingerPrintManagerApp.Modules.Employe.ViewModel
{
    public class EmployeMultiChoiceListViewModel : DialogViewModelBase
    {
        private static object _lock = new object();

        // Collections
        private ObservableCollection<Model.Employe.Employe> employes;

        // Collection Views
        public ICollectionView EmployesView { get; private set; }
        
        //Facade
        private IChooserViewModel chooserViewModel;
        private System.Predicate<object> customFilter;

        public EmployeMultiChoiceListViewModel(IChooserViewModel chooserViewModel, System.Predicate<object> filter = null)
        {
            this.chooserViewModel = chooserViewModel;
            this.customFilter = filter;

            employes = new ObservableCollection<Model.Employe.Employe>();
            BindingOperations.EnableCollectionSynchronization(employes, _lock);

            EmployesView = (CollectionView)CollectionViewSource.GetDefaultView(employes);
            EmployesView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            EmployesView.Filter += OnFilterEmploye;

            FilterText = string.Empty;

            SelectAllTag = "Tout sélectionner";

        }

        private bool OnFilterEmploye(object obj)
        {
            if (customFilter != null)
                if (!customFilter(obj))
                    return false;

            var employe = obj as Model.Employe.Employe;

            if (employe == null)
                return false;

            var motif = FilterText.Trim().ToLower().NoAccent();

            return employe.Name.ToLower().NoAccent().Contains(motif) || employe.Sexe.ToString().ToLower().NoAccent().Contains(motif) ||
                (!string.IsNullOrWhiteSpace(employe.Matricule) && employe.Matricule.ToLower().NoAccent().Contains(motif));
                //|| employe.CurrentGrade.Id.ToLower().NoAccent().Contains(motif);
        }

        private int _count;
        public int EmployeCount
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
                    RaisePropertyChanged(() => EmployeCount);
                }
            }
        }

        private int _selectionCount;
        public int SelectionCount
        {
            get
            {
                return this._selectionCount;
            }
            set
            {
                if (_selectionCount != value)
                {
                    _selectionCount = value;
                    RaisePropertyChanged(() => SelectionCount);
                }
            }
        }

        private bool _employeLoading;
        public bool EmployeLoading
        {
            get
            {
                return this._employeLoading;
            }
            set
            {
                if (_employeLoading != value)
                {
                    _employeLoading = value;
                    RaisePropertyChanged(() => EmployeLoading);
                }
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
                    EmployesView.Refresh();
                    RaisePropertyChanged(() => FilterText);
                }
            }
        }

        private string _selectAllTag;
        public string SelectAllTag
        {
            get
            {
                return this._selectAllTag;
            }
            set
            {
                if (_selectAllTag != value)
                {
                    _selectAllTag = value;
                    RaisePropertyChanged(() => SelectAllTag);
                }
            }
        }

        private bool _toSelectAll;
        public bool ToSelectAll
        {
            get
            {
                return this._toSelectAll;
            }
            set
            {
                if (_toSelectAll != value)
                {
                    _toSelectAll = value;
                    RaisePropertyChanged(() => ToSelectAll);
                }
            }
        }

        #region Commands
        protected override async Task Load(object param = null)
        {
            await LoadEmployes();
        }

        public IAsyncCommand RefreshEmployeCommand
        {
            get
            {
                if (_refreshEmployeCommand == null)
                {
                    _refreshEmployeCommand = new AsyncCommand(LoadEmployes, CanRefreshEmploye);
                }
                return _refreshEmployeCommand;
            }
        }

        private async Task LoadEmployes(object param = null)
        {
            EmployeLoading = true;

            EmployeCount = new EmployeDao().Count(AppConfig.CurrentUser.Entite);

            employes.Clear();

            await Task.Run(() => new EmployeDao().GetAllAsync(AppConfig.CurrentUser.Entite, employes));

            EmployeLoading = false;

            EmployesView.Refresh();
        }

        private bool CanRefreshEmploye(object param = null)
        {
            return !EmployeLoading;
        }

        public ICommand ChooseCommand
        {
            get
            {
                if (_chooseCommand == null)
                    _chooseCommand = new RelayCommand(p => Choose(p), p => SelectionCount > 0);

                return _chooseCommand;
            }
        }

        private void Choose(object param)
        {
            var win = param as Window;

            chooserViewModel.Choose(employes.ToList().FindAll(s => s.IsSelected));

            this.CloseDialogWithResult(win, DialogResult.Yes);

        }

        protected override void Close(object param)
        {
            this.CloseDialogWithResult(param as Window, DialogResult.No);
        }

        public ICommand CheckCommand
        {
            get
            {
                if (_checkCommand == null)
                    _checkCommand = new RelayCommand(p => Check(p));

                return _checkCommand;
            }
        }

        private void Check(object param)
        {
            SelectionCount = employes.ToList().FindAll(s => s.IsSelected).Count;
            ToSelectAll = SelectionCount == employes.Count;
            SelectAllTag = "Tout sélectionner";
        }

        public ICommand AllSelectCommand
        {
            get
            {
                if (_allSelectCommand == null)
                    _allSelectCommand = new RelayCommand(p => AllSelect(p), p => true);

                return _allSelectCommand;
            }
        }

        private void AllSelect(object param)
        {
            foreach (var employe in employes)
            {
                if (customFilter != null && !customFilter(employe))
                    continue;

                employe.IsSelected = ToSelectAll;
            }
            
            SelectionCount = employes.ToList().FindAll(s => s.IsSelected).Count;

            SelectAllTag = !ToSelectAll ? "Tout sélectionner" : "Tout désélectionner";
        }

        private AsyncCommand _refreshEmployeCommand;
        private RelayCommand _chooseCommand;
        private RelayCommand _checkCommand;
        private RelayCommand _allSelectCommand;

        #endregion
    }
}
