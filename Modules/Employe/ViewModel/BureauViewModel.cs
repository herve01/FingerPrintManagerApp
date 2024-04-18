using FingerPrintManagerApp.Dao.Employe;
using FingerPrintManagerApp.Extension;
using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Admin.Util;
using FingerPrintManagerApp.Model.Employe;
using FingerPrintManagerApp.ViewModel;
using FingerPrintManagerApp.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace FingerPrintManagerApp.Modules.Employe.ViewModel
{
    public class BureauViewModel : PageViewModel
    {
        private static object _lock = new object();
        private ObservableCollection<Bureau> bureaux;
        private ObservableCollection<Departement> divisions;

        private bool editing = false;
        public ICollectionView BureauxView { get; private set; }
        public ICollectionView DirectionsView { get; private set; }
        public ICollectionView DivisionsView { get; private set; }

        public BureauViewModel()
        {
            bureaux = new ObservableCollection<Bureau>();
            BindingOperations.EnableCollectionSynchronization(bureaux, _lock);

            BureauxView = (CollectionView)CollectionViewSource.GetDefaultView(bureaux);
            BureauxView.GroupDescriptions.Add(new PropertyGroupDescription("Direction.Denomination"));
            BureauxView.SortDescriptions.Add(new SortDescription("Direction.EstGenerale", ListSortDirection.Descending));
            BureauxView.SortDescriptions.Add(new SortDescription("Direction.Denomination", ListSortDirection.Ascending));
            BureauxView.SortDescriptions.Add(new SortDescription("Division.Denomination", ListSortDirection.Ascending));
            BureauxView.SortDescriptions.Add(new SortDescription("Denomination", ListSortDirection.Ascending));
            //Filtering
            BureauxView.Filter = OnFilterBureau;

 

            divisions = new ObservableCollection<Departement>();
            DivisionsView = (CollectionView)CollectionViewSource.GetDefaultView(divisions);
            DivisionsView.SortDescriptions.Add(new SortDescription("Denomination", ListSortDirection.Ascending));

            FilterText = string.Empty;

            InitSave();
            MenuInit();
        }

        private bool OnFilterBureau(object obj)
        {
            var bureau = obj as Bureau;

            if (bureau == null)
                return false;

            var pattern = FilterText.Trim().ToLower().NoAccent();

            return bureau.Denomination.ToLower().NoAccent().Contains(pattern) ||
                (bureau.Division != null && bureau.Division.Denomination.ToLower().NoAccent().Contains(pattern));

        }

        private string _action;
        private string _filterText;
        private int _count;
        private int _multiplicite;
        private Bureau _bureau;
        private bool _bureauLoading;
        private bool _isSiege;


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
                    BureauxView.Refresh();
                    RaisePropertyChanged(() => FilterText);
                }
            }
        }
        public int BureauCount
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
                    RaisePropertyChanged(() => BureauCount);
                }
            }
        }

        public int Multiplicite
        {
            get
            {
                return this._multiplicite;
            }
            set
            {
                if (_multiplicite != value && value >= 1)
                {
                    _multiplicite = value;
                    RaisePropertyChanged(() => Multiplicite);
                }
            }
        }

        public Bureau Bureau
        {
            get
            {
                return this._bureau;
            }
            set
            {
                if (_bureau != value)
                {
                    _bureau = value;
                    RaisePropertyChanged(() => Bureau);
                }
            }
        }

        public bool BureauLoading
        {
            get
            {
                return this._bureauLoading;
            }
            set
            {
                if (_bureauLoading != value)
                {
                    _bureauLoading = value;
                    RaisePropertyChanged(() => BureauLoading);
                }
            }
        }

        public bool IsSiege
        {
            get
            {
                return this._isSiege;
            }
            set
            {
                if (_isSiege != value)
                {
                    _isSiege = value;
                    RaisePropertyChanged(() => IsSiege);
                }
            }
        }

        #region Commands

        protected override async Task LoadData()
        {
            if (!IsSiege)
                return;

            UpdateTimer.IsEnabled = false;

            await LoadDirectionChanges();
            await LoadDivisionChanges();
            await LoadBureauChanges();

            LastDataUpdateTime = DateTime.Now;

            UpdateTimer.IsEnabled = true;
        }

        protected override async Task Load(object param)
        {
            //IsSiege = AppConfig.CurrentUser.Entite.EstPrincipale;

            //BureauxView.GroupDescriptions.Clear();

            //if (IsSiege)
            //    BureauxView.GroupDescriptions.Add(new PropertyGroupDescription("Direction.Denomination"));
            //else
            //    BureauxView.GroupDescriptions.Add(new PropertyGroupDescription("Division.Entite"));

            if (!IsInit)
            {
                await LoadBureaux();

                //if (IsSiege)
                //    await LoadDirections();
                //else
                //    LoadDivisionProvinciale();

                LastDataUpdateTime = DateTime.Now;

                IsInit = true;
            }

            UpdateTimer.IsEnabled = true;
        }

        async Task LoadBureaux()
        {
            //BureauLoading = true;

            //bureaux.Clear();

            //BureauCount = new BureauDao().Count(AppConfig.CurrentUser.Entite);

            //await Task.Run(() => new BureauDao().GetAllAsync(AppConfig.CurrentUser.Entite, bureaux));

            //BureauLoading = false;

            //BureauxView.Refresh();
        }



        void LoadDivisionProvinciale()
        {
            //divisions.Clear();
            //divisions.Add(AppConfig.CurrentUser.Entite.Division);
            //DivisionsView.MoveCurrentToFirst();
        }

        void LoadDivisions()
        {
            //divisions.Clear();

            //if (SelectedDirection == null)
            //    return;

            //SelectedDirection.Divisions.ForEach(d => divisions.Add(d));

            //DivisionsView.MoveCurrentToFirst();
        }

        async Task LoadDirectionChanges()
        {
            //var list = await Task.Run(() => new DirectionDao().GetAllAsync(LastDataUpdateTime.AddSeconds(-5)));

            //list.ForEach(d => {
            //    var _d = directions.ToList().Find(e => e.Equals(d));

            //    if (_d != null) directions.Remove(_d);

            //    if (d.Equals(Bureau.Direction))
            //        Bureau.Direction = d;

            //    directions.Add(d);
            //});

            //DirectionsView.Refresh();
        }

        async Task LoadDivisionChanges()
        {
            var list = await Task.Run(() => new DivisionDao().GetAllAsync(LastDataUpdateTime.AddSeconds(-5)));

            //list.ForEach(d => {
            //    var direction = directions.ToList().Find(dir => dir.Equals(d.Direction));

            //    var _d = direction.Divisions.ToList().Find(e => e.Equals(d));

            //    if (_d != null) { direction.Divisions.Remove(_d); if (direction.Equals(SelectedDirection)) divisions.Remove(_d); }

            //    if (d.Equals(Bureau.Division))
            //        Bureau.Division = d;

            //    direction.Divisions.Add(d);

            //    if (direction.Equals(SelectedDirection))
            //        divisions.Add(d);
            //});

            DivisionsView.Refresh();
        }

        async Task LoadBureauChanges()
        {
            //BureauCount = new BureauDao().Count(AppConfig.CurrentUser.Entite);

            //var list = await Task.Run(() => new BureauDao().GetAllAsync(AppConfig.CurrentUser.Entite, LastDataUpdateTime.AddSeconds(-5)));

            //list.ForEach(d =>
            //{
            //    var _d = bureaux.ToList().Find(e => e.Equals(d));

            //    if (_d != null) bureaux.Remove(_d);

            //    bureaux.Add(d);
            //});

            //BureauxView.Refresh();
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new AsyncCommand(p => Save(), p => CanSave());

                return _saveCommand;
            }
        }

        private async Task Save()
        {
            Status = string.Empty;

            if (Bureau.NombreChefs > 1)
            {
                var msg = string.Format("Êtes-vous sûr(e) que ce bureau devra avoir {0} chefs ?", Bureau.NombreChefs);

                if (MyMsgBox.Show(msg, "Humager", MyMsgBoxButton.YesNoCancel, MyMsgBoxIcon.Warning) != DialogueResult.Yes)
                    return;
            }

            if (!editing)
            {
                if (Multiplicite > 1)
                {
                    var msg = string.Format("Êtes-vous sûr(e) de vouloir créer {0} copies de ce bureau ?", Multiplicite);

                    if (MyMsgBox.Show(msg, "Humager", MyMsgBoxButton.YesNoCancel, MyMsgBoxIcon.Warning) != DialogueResult.Yes)
                        return;
                }

                BureauLoading = true;

                if (bureaux.Contains(Bureau))
                {
                    Status = "Cette division contient déjà un bureau d'une même dénomination !";
                    return;
                }

                var list = new List<Bureau>();

                if (Multiplicite == 1)
                    list.Add(Bureau);
                else
                {
                    for (int i = 0; i < Multiplicite; i++)
                    {
                        list.Add(new Bureau()
                        {
                            Denomination = string.Format("{0} {1}", Bureau.Denomination, i + 1),
                            Division = Bureau.Division,
                            Mission = Bureau.Mission,
                            NombreChefs = Bureau.NombreChefs
                        });
                    }
                }

                if (await new BureauDao().AddAsync(list) > 0)
                {
                    foreach (var bureau in list)
                    {
                        Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Bureau + "",
                            string.Format("Enregistrement du bureau '{0}' (ID : {1}).", bureau.Denomination, bureau.Id)
                        );

                        bureaux.Add(bureau);
                    }
                    
                    
                    BureauCount += list.Count;
                    BureauxView.Refresh();
                    Status = string.Format("Bureau{0} enregistré{1} avec succès !", Multiplicite > 1 ? "x" : "", Multiplicite > 1 ? "s" : "");
                    InitSave();
                }
            }
            else
            {
                var clone = (Bureau)Bureau.Clone();
                clone.Id = string.Empty;

                bureaux.Remove(Bureau);

                if (!bureaux.Contains(clone))
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Bureau + "",
                            string.Format("Modification du bureau '{0}' (ID : {1}).", Bureau.Denomination, Bureau.Id)
                        );

                    if (new BureauDao().Update(Bureau) > 0)
                    {
                        Status = "Bureau modifié avec succès !";
                        bureaux.Add(Bureau);
                        BureauxView.Refresh();
                        InitSave();
                    }
                }
                else
                {
                    Status = "Un bureau avec la même description existe déjà !";
                    Bureau.CancelEdit();
                    bureaux.Add(Bureau);
                    BureauxView.Refresh();
                }

            }

            BureauLoading = false;
        }

        private void InitSave()
        {
            Multiplicite = 1;


            var division = Bureau?.Division;

            Bureau = new Bureau();
            Action = "Enregistrer";
            editing = false;
            Title = "Nouveau bureau";

            //if (direction != null)
            //    DirectionsView.MoveCurrentTo(direction);
            //else
            //    DirectionsView.MoveCurrentToFirst();

            //if (division != null)
            //    DivisionsView.MoveCurrentTo(division);
            //else
            //    DivisionsView.MoveCurrentToFirst();
        }

        private void MenuInit()
        {
            Name = "Bureau";
            OptionItem = new OptionItem()
            {
                Name = "Bureau",
                ToolTip = "Bureau",
                IconPathData = "M1.9999998,26.988997L1.9999998,30.000014 29,30.000014 29,26.988997z M24.844001,20.512998C25.62,20.512998 26.25,20.932 26.25,21.449001 26.25,21.966003 25.62,22.385004 24.844001,22.385004 24.067003,22.385004 23.437004,21.966003 23.437004,21.449001 23.437004,20.932 24.067003,20.512998 24.844001,20.512998z M20.094001,20.512998C20.87,20.512998 21.5,20.932 21.5,21.449001 21.5,21.966003 20.87,22.385004 20.094001,22.385004 19.317003,22.385004 18.687004,21.966003 18.687004,21.449001 18.687004,20.932 19.317003,20.512998 20.094001,20.512998z M15.594001,20.512998C16.37,20.512998 17,20.932 17,21.449001 17,21.966003 16.37,22.385004 15.594001,22.385004 14.817002,22.385004 14.187003,21.966003 14.187003,21.449001 14.187003,20.932 14.817002,20.512998 15.594001,20.512998z M10.844001,20.512998C11.62,20.512998 12.25,20.932 12.25,21.449001 12.25,21.966003 11.62,22.385004 10.844001,22.385004 10.067002,22.385004 9.4370027,21.966003 9.4370027,21.449001 9.4370027,20.932 10.067002,20.512998 10.844001,20.512998z M6.0940014,20.512998C6.8700004,20.512998 7.4999996,20.932 7.4999996,21.449001 7.4999996,21.966003 6.8700004,22.385004 6.0940014,22.385004 5.3170024,22.385004 4.6870032,21.966003 4.6870032,21.449001 4.6870032,20.932 5.3170024,20.512998 6.0940014,20.512998z M5.5059963,17.971012L2.3906494,24.988997 28.665628,24.988997 25.573997,17.971012z M5.9999996,12.960004L5.9999996,15.971013 25,15.971013 25,12.960004z M23,8.9519992L23,10.960004 25,10.960004 25,8.9519992z M9.98414,8.9519992L9.9977556,10.960004 21,10.960004 21,8.9519992z M5.9999996,8.9519992L5.9999996,10.960004 7.9999996,10.960004 7.9999996,8.9519992z M21,1.9359999L9.9369999,2 9.9705778,6.9519997 21,6.9519997z M9.9369999,0L21.062999,0C22.131,0,23,0.86800003,23,1.9359999L23,6.9519997 25,6.9519997 25,6.6130037C25,6.0600042 25.448,5.6130037 26,5.6130037 26.552,5.6130037 27,6.0600042 27,6.6130037L27,6.9519997 28.062,6.9519997 28.062,6.6130075C28.062,6.0610085 28.51,5.6130085 29.062,5.6130085 29.614,5.6130085 30.062,6.0610085 30.062,6.6130075L30.062,11.291001C30.062,11.844 29.614,12.291 29.062,12.291 28.51,12.291 28.062,11.844 28.062,11.291001L28.062,8.9519992 27,8.9519992 27,16.2479 31,25.32602 31,32.000013 0,32.000013 0,25.445008 3.9999996,16.43503 3.9999996,8.9519992 2.9369996,8.9519992 2.9369996,11.300004C2.9369996,11.853004 2.4889997,12.300004 1.9369996,12.300004 1.3849996,12.300004 0.93699956,11.853004 0.93699974,11.300004L0.93699974,6.6320028C0.93699956,6.0790033 1.3849996,5.6320028 1.9369996,5.6320028 2.4889997,5.6320028 2.9369996,6.0790033 2.9369996,6.6320028L2.9369996,6.9519997 3.9999996,6.9519997 3.9999996,6.6320066C3.9999996,6.0800066 4.4479995,5.6320066 4.9999996,5.6320066 5.5519996,5.6320066 5.9999996,6.0800066 5.9999996,6.6320066L5.9999996,6.9519997 7.9999996,6.9519997 7.9999996,1.9359999C7.9999996,0.86800003,8.869,0,9.9369999,0z"
            };
        }

        private bool CanSave()
        {
            return IsSiege && Bureau.Error == string.Empty;
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
                Bureau.CancelEdit();

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
            if (param is Bureau)
            {
                Bureau = (Bureau)param;
                Bureau.BeginEdit();
                editing = true;
                Action = "Modifier";
                Title = "Modification du bureau";

                //DirectionsView.MoveCurrentTo(directions.ToList().Find(d => d.Equals(Bureau.Direction)));
                //DivisionsView.MoveCurrentTo(divisions.ToList().Find(d => d.Equals(Bureau.Division)));
            }
        }

        private bool CanEdit(object param)
        {
            var bureau = param as Bureau;

            if (bureau == null)
                return false;

            return IsSiege && bureau.Division != null && !editing;
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

            if (param is Bureau)
            {
                var bureau = (Bureau)param;

                var msg = string.Format("Êtes - vous sûr(e) de vouloir supprimer le bureau <<{0}>> ?", bureau.Denomination);

                if (MyMsgBox.Show(msg, "Humager", MyMsgBoxButton.YesNoCancel, MyMsgBoxIcon.Warning) == DialogueResult.Yes)
                {
                    if (new BureauDao().Delete(bureau) > 0)
                    {
                        Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Bureau + "",
                            string.Format("Suppression du bureau '{0}' (ID : {1}).", bureau.Denomination, bureau.Id)
                        );

                        bureaux.Remove(bureau);
                        BureauCount--;

                        Status = "Bureau supprimé avec succès !";
                    }
                    else
                    {
                        Status = "Suppression du bureau échouée ! Vérifiez que vous êtes bien connecté au serveur de données, " +
                            "sinon ce bureau est rélié à d'autres objets et ne peut donc pas être supprimé.";
                    }
                }

            }
        }

        private bool CanDelete(object param)
        {
            var bureau = param as Bureau;

            if (bureau == null)
                return false;

            return IsSiege && bureau.Division != null && !editing;
        }

        protected override void Scroll(object param)
        {
            var control = param as System.Windows.Controls.ItemsControl;

            if (control != null)
            {
                var items = FingerPrintManagerApp.ViewModel.Helper.View.GetItemsControlVisibleItemsFromTheTop(control, (FrameworkElement)control.Parent);

                //if (items.Count > 0)
                //    VisibleDirection = ((Bureau)items[0]).Direction;
                //else
                //    VisibleDirection = null;

            }
        }

        private AsyncCommand _saveCommand;
        private RelayCommand _cancelCommand;
        private RelayCommand _editCommand;
        private RelayCommand _deleteCommand;

        #endregion

    }
}
