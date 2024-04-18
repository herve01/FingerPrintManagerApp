using FingerPrintManagerApp.Dao.Employe;
using FingerPrintManagerApp.Dialog.Service;
using FingerPrintManagerApp.Extension;
using FingerPrintManagerApp.ViewModel.Command;
using FingerPrintManagerApp.ViewModel.Contract;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace FingerPrintManagerApp.Modules.Employe.ViewModel
{
    public class EmployeChoiceListViewModel : DialogViewModelBase
    {
        private static object _lock = new object();

        // Collections
        private ObservableCollection<Model.Employe.Employe> employes;

        // Collection Views
        public ICollectionView EmployesView { get; private set; }

        //View Models
        private IChooserViewModel chooserViewModel;
        private System.Predicate<object> customFilter;

        public EmployeChoiceListViewModel(IChooserViewModel chooserViewModel, System.Predicate<object> filter = null)
        {
            this.chooserViewModel = chooserViewModel;
            this.customFilter = filter;

            employes = new ObservableCollection<Model.Employe.Employe>();
            BindingOperations.EnableCollectionSynchronization(employes, _lock);

            EmployesView = (CollectionView)CollectionViewSource.GetDefaultView(employes);
            EmployesView.SortDescriptions.Add(new SortDescription("Nom", ListSortDirection.Ascending));

            //Filtering
            EmployesView.Filter = OnFilterEmploye;

            FilterText = string.Empty;

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

            EmployeCount = new EmployeDao().Count();

            employes.Clear();

            await Task.Run(() => new EmployeDao().GetAllAsync(employes));

            EmployeLoading = false;

            EmployesView.Refresh();
        }

        private bool CanRefreshEmploye(object param = null)
        {
            return !EmployeLoading;
        }

        public ICommand ChooseEmployeCommand
        {
            get
            {
                if (_chooseEmployeCommand == null)
                    _chooseEmployeCommand = new RelayCommand(p => Choose(p), p => true);

                return _chooseEmployeCommand;
            }
        }

        private void Choose(object param)
        {
            var values = (object[])param;
            var employe = values[0] as Model.Employe.Employe;
            var win = values[1] as Window;

            chooserViewModel.Choose(employe);

            this.CloseDialogWithResult(win, DialogResult.Yes);

        }

        protected override void Close(object param)
        {
            this.CloseDialogWithResult(param as Window, DialogResult.No);
        }

        private AsyncCommand _refreshEmployeCommand;
        private RelayCommand _chooseEmployeCommand;

        #endregion

    }
}
