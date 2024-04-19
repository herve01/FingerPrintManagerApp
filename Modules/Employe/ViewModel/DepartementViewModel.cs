using FingerPrintManagerApp.Dao.Employe;
using FingerPrintManagerApp.Extension;
using FingerPrintManagerApp.Model;
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

namespace FingerPrintManagerApp.Modules.Employe.ViewModel
{
    public class DepartementViewModel : PageViewModel
    {
        private static object _lock = new object();
        private ObservableCollection<Departement> departements;
        private ObservableCollection<Direction> directions;

        private bool editing = false;
        public ICollectionView DepartementsView { get; private set; }
        public ICollectionView DirectionsView { get; private set; }

        public DepartementViewModel()
        {
            departements = new ObservableCollection<Departement>();
            BindingOperations.EnableCollectionSynchronization(departements, _lock);
            DepartementsView = (CollectionView)CollectionViewSource.GetDefaultView(departements);
            DepartementsView.GroupDescriptions.Add(new PropertyGroupDescription("Direction.Denomination"));
            DepartementsView.SortDescriptions.Add(new SortDescription("Direction.EstGenerale", ListSortDirection.Descending));
            DepartementsView.SortDescriptions.Add(new SortDescription("Direction.Denomination", ListSortDirection.Ascending));
            DepartementsView.SortDescriptions.Add(new SortDescription("Denomination", ListSortDirection.Ascending));
            //Filtering
            DepartementsView.Filter = OnFilterDepartement;

            directions = new ObservableCollection<Direction>();
            DirectionsView = (CollectionView)CollectionViewSource.GetDefaultView(directions);
            DirectionsView.SortDescriptions.Add(new SortDescription("EstGenerale", ListSortDirection.Descending));
            DirectionsView.SortDescriptions.Add(new SortDescription("Denomination", ListSortDirection.Ascending));

            FilterText = string.Empty;

            InitSave();
            MenuInit();
        }

        private bool OnFilterDepartement(object obj)
        {
            var departement = obj as Departement;

            if (departement == null)
                return false;

            var pattern = FilterText.Trim().ToLower().NoAccent();

            return departement.Denomination.ToLower().NoAccent().Contains(pattern)
                || departement.Direction.Denomination.ToLower().NoAccent().Contains(pattern);

        }

        private string _action;
        private string _filterText;
        private int _count;
        private Departement _departement;
        private bool _departementLoading;
        private Direction _visibleDirection;

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
                    DepartementsView.Refresh();
                    RaisePropertyChanged(() => FilterText);
                }
            }
        }
        public int DepartementCount
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
                    RaisePropertyChanged(() => DepartementCount);
                }
            }
        }

        public Departement Departement
        {
            get
            {
                return this._departement;
            }
            set
            {
                if (_departement != value)
                {
                    _departement = value;
                    RaisePropertyChanged(() => Departement);
                }
            }
        }

        public bool DepartementLoading
        {
            get
            {
                return this._departementLoading;
            }
            set
            {
                if (_departementLoading != value)
                {
                    _departementLoading = value;
                    RaisePropertyChanged(() => DepartementLoading);
                }
            }
        }

        #region Commands

        protected override async Task LoadData()
        {
            UpdateTimer.IsEnabled = false;

            await LoadDirectionChanges();
            await LoadDepartementChanges();

            LastDataUpdateTime = DateTime.Now;

            UpdateTimer.IsEnabled = true;
        }

        protected override async Task Load(object param)
        {
            if (!IsInit)
            {
                await LoadDepartements();
                await LoadDirections();
                LastDataUpdateTime = DateTime.Now;
                IsInit = true;
            }

            UpdateTimer.IsEnabled = true;
        }

        async Task LoadDepartements()
        {
            DepartementLoading = true;

            departements.Clear();

            DepartementCount = new DepartementDao().Count();

            await Task.Run(() => new DepartementDao().GetAllAsync(departements));

            DepartementLoading = false;

            DepartementsView.Refresh();
        }

        async Task LoadDirections()
        {
            directions.Clear();

            var list = await Task.Run(() => new DirectionDao().GetAllAsync());

            list.ForEach(d => directions.Add(d));

            DirectionsView.MoveCurrentToFirst();
        }

        async Task LoadDirectionChanges()
        {
            var list = await Task.Run(() => new DirectionDao().GetAllAsync(LastDataUpdateTime.AddSeconds(-5)));

            list.ForEach(d =>
            {
                var _d = directions.ToList().Find(e => e.Equals(d));

                if (_d != null) directions.Remove(_d);

                if (d.Equals(Departement.Direction))
                    Departement.Direction = d;

                directions.Add(d);
            });

            DirectionsView.Refresh();
        }

        async Task LoadDepartementChanges()
        {
            //DepartementCount = new DepartementDao().Count(AppConfig.CurrentUser.Entite);

            //var list = await Task.Run(() => new DepartementDao().GetAllAsync(LastDataUpdateTime.AddSeconds(-5)));

            //list.ForEach(d => {
            //    var _d = departements.ToList().Find(e => e.Equals(d));

            //    if (_d != null) departements.Remove(_d);

            //    departements.Add(d);
            //});

            //DepartementsView.Refresh();
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

            //Departement.Entite = AppConfig.CurrentUser.Entite;

            if (!editing)
            {
                if (departements.Contains(Departement))
                {
                    Status = "Une departement avec la même description existe déjà.";
                    return;
                }

                if (new DepartementDao().Add(Departement) > 0)
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Departement + "",
                            string.Format("Enregistrement de la departement '{0}' (ID : {1}).", Departement.Denomination, Departement.Id)
                        );

                    departements.Add(Departement);
                    DepartementCount++;
                    DepartementsView.Refresh();
                    Status = "Departement enregistrée avec succès !";
                    InitSave();
                }
            }
            else
            {
                var clone = (Departement)Departement.Clone();
                clone.Id = string.Empty;

                departements.Remove(Departement);

                if (!departements.Contains(clone))
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Departement + "",
                            string.Format("Modification de la departement '{0}' (ID : {1}).", Departement.Denomination, Departement.Id)
                        );

                    if (new DepartementDao().Update(Departement) > 0)
                    {
                        Status = "Departement modifiée avec succès !";
                        departements.Add(Departement);
                        DepartementsView.Refresh();
                        InitSave();
                    }
                }
                else
                {
                    Status = "Une departement avec la même description existe déjà !";
                    Departement.CancelEdit();
                    departements.Add(Departement);
                    DepartementsView.Refresh();
                }

            }
        }

        private void InitSave()
        {
            //var direction = Departement?.Direction;

            Departement = new Departement();
            Action = "Enregistrer";
            editing = false;
            Title = "Nouvelle departement";

            //if (direction != null)
            //    DirectionsView.MoveCurrentTo(direction);
            //else
            //    DirectionsView.MoveCurrentToFirst();
        }

        private void MenuInit()
        {
            Name = "Departement";
            OptionItem = new OptionItem()
            {
                Name = "Departement",
                ToolTip = "Departement",
                IconPathData = "M8,24C6.3460002,24 5,25.346001 5,27 5,28.653999 6.3460002,30 8,30 9.6540003,30 11,28.653999 11,27 11,25.346001 9.6540003,24 8,24z M21,18C19.346001,18 18,19.346001 18,21 18,22.653999 19.346001,24 21,24 22.653999,24 24,22.653999 24,21 24,19.346001 22.653999,18 21,18z M27,5C25.346001,5 24,6.3460002 24,8 24,9.6540003 25.346001,11 27,11 28.653999,11 30,9.6540003 30,8 30,6.3460002 28.653999,5 27,5z M8,2C4.6910095,2 2,4.6909943 2,8 2,11.309006 4.6910095,14 8,14 11.30899,14 14,11.309006 14,8 14,4.6909943 11.30899,2 8,2z M8,0C12.066401,0,15.434203,3.0501282,15.935603,6.9825435L15.937605,7 22.100616,7 22.101765,6.9935956C22.568781,4.7172971 24.587626,3 27,3 29.757,3 32,5.243 32,8 32,10.757 29.757,13 27,13 24.587626,13 22.568781,11.282703 22.101765,9.0064049L22.100616,9 15.937605,9 15.935603,9.0174561C15.765205,10.353863,15.263757,11.588371,14.516341,12.635893L14.388103,12.808235 18.345156,16.766047 18.409283,16.724895C19.165298,16.265041 20.05228,16 21,16 23.757,16 26,18.243 26,21 26,23.757 23.757,26 21,26 18.243,26 16,23.757 16,21 16,19.966125 16.315422,19.004532 16.855087,18.206396L16.897621,18.146629 12.995,14.244009 12.974164,14.261286C11.884956,15.128406,10.565331,15.717825,9.1234684,15.921363L8.9759865,15.938684 8.9759865,22.096317 9.0064049,22.101765C11.282703,22.568781 13,24.587626 13,27 13,29.757 10.757,32 8,32 5.243,32 3,29.757 3,27 3,24.67378 4.5968232,22.713486 6.7518983,22.157686L6.975987,22.105839 6.975987,15.934683 6.7833576,15.907648C2.9475975,15.319838 0,11.997465 0,8 0,3.5890045 3.5889893,0 8,0z"
            };
        }

        private bool CanSave()
        {
            return Departement.Error == string.Empty;
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
                Departement.CancelEdit();

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

        private void Edit(object param)
        {
            if (param is Departement)
            {
                Departement = (Departement)param;
                Departement.BeginEdit();
                editing = true;
                Action = "Modifier";
                Title = "Modification de la departement";

                //DirectionsView.MoveCurrentTo(directions.ToList().Find(d => d.Equals(Departement.Direction)));
            }
        }

        private bool CanEdit(object param)
        {
            return !editing;
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
            Status = string.Empty;

            if (param is Departement)
            {
                var departement = (Departement)param;

                var msg = string.Format("Êtes - vous sûr(e) de vouloir supprimer la departement <<{0}>> ?", departement.Denomination);

                if (MyMsgBox.Show(msg, "Humager", MyMsgBoxButton.YesNoCancel, MyMsgBoxIcon.Warning) == DialogueResult.Yes)
                {
                    if (new DepartementDao().Delete(departement) > 0)
                    {
                        Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Departement + "",
                            string.Format("Suppression de la departement '{0}' (ID : {1}).", departement.Denomination, departement.Id)
                        );

                        departements.Remove(departement);
                        DepartementCount--;

                        Status = "Departement supprimée avec succès !";
                    }
                    else
                    {
                        Status = "Suppression de la departement échouée ! Vérifiez que vous êtes bien connecté au serveur de données, " +
                            "sinon cette departement est réliée à d'autres objets et ne peut donc pas être supprimée.";
                    }
                }

            }
        }

        private bool CanDelete(object param)
        {
            return !editing;
        }

        protected override void Scroll(object param)
        {
            //var control = param as System.Windows.Controls.ItemsControl;

            //if (control != null)
            //{
            //    var items = Humager.ViewModel.Helper.View.GetItemsControlVisibleItemsFromTheTop(control, (FrameworkElement)control.Parent);

            //    if (items.Count > 0)
            //        VisibleDirection = ((Departement)items[0]).Direction;
            //    else
            //        VisibleDirection = null;

            //}
        }

        private RelayCommand _saveCommand;
        private RelayCommand _cancelCommand;
        private RelayCommand _editCommand;
        private RelayCommand _deleteCommand;

        #endregion

    }
}
