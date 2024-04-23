using FingerPrintManagerApp.Dao.Employe;
using FingerPrintManagerApp.Dialog.Facade;
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
    public class FonctionViewModel : PageViewModel, IDataErrorInfo
    {
        private static object _lock = new object();
        // Collections
        private ObservableCollection<Fonction> fonctions;
        private ObservableCollection<Grade> grades;
        private ObservableCollection<Entite> entites;
        private ObservableCollection<Direction> directions;
        private ObservableCollection<Departement> departements;
        private ObservableCollection<Entite> agences;

        // Collection Views
        public ICollectionView FonctionsView { get; private set; }
        public ICollectionView GradesView { get; private set; }
        public ICollectionView EntitesView { get; private set; }
        public ICollectionView UniteTypesView { get; private set; }
        public ICollectionView DirectionsView { get; private set; }
        public ICollectionView DepartementsView { get; private set; }
        public ICollectionView AgencesView { get; private set; }

        //Array
        public Array uniteTypes { get; set; }

        IDialogFacade facade;

        public FonctionViewModel(IDialogFacade facade)
        {
            this.facade = facade;

            fonctions = new ObservableCollection<Fonction>();
            BindingOperations.EnableCollectionSynchronization(fonctions, _lock);

            FonctionsView = (CollectionView)CollectionViewSource.GetDefaultView(fonctions);
            FonctionsView.SortDescriptions.Add(new SortDescription("Grade.Niveau", ListSortDirection.Descending));
            FonctionsView.Filter = OnFilterFonction;

            grades = new ObservableCollection<Grade>();
            GradesView = (CollectionView)CollectionViewSource.GetDefaultView(grades);
            GradesView.SortDescriptions.Add(new SortDescription("Niveau", ListSortDirection.Ascending));
            GradesView.Filter = OnGradeFilter;

            entites = new ObservableCollection<Entite>();
            EntitesView = (CollectionView)CollectionViewSource.GetDefaultView(entites);

            uniteTypes = Enum.GetValues(typeof(UniteType));
            UniteTypesView = (CollectionView)CollectionViewSource.GetDefaultView(uniteTypes);
            UniteTypesView.Filter = OnTypeFilter;

            directions = new ObservableCollection<Direction>();
            DirectionsView = (CollectionView)CollectionViewSource.GetDefaultView(directions);
            DirectionsView.SortDescriptions.Add(new SortDescription("EstGenerale", ListSortDirection.Descending));
            DirectionsView.SortDescriptions.Add(new SortDescription("Denomination", ListSortDirection.Ascending));

            departements = new ObservableCollection<Departement>();
            DepartementsView = (CollectionView)CollectionViewSource.GetDefaultView(departements);
            DepartementsView.SortDescriptions.Add(new SortDescription("Denomination", ListSortDirection.Ascending));

            agences = new ObservableCollection<Entite>();
            AgencesView = (CollectionView)CollectionViewSource.GetDefaultView(agences);
            AgencesView.SortDescriptions.Add(new SortDescription("Denomination", ListSortDirection.Ascending));

            FilterText = string.Empty;

            InitSave();
            MenuInit();
        }

        private bool OnTypeFilter(object obj)
        {
            var type = (UniteType)obj;

            return IsSiege || type != UniteType.Direction;
        }

        private bool OnGradeFilter(object obj)
        {
            var grade = obj as Grade;

            if (grade == null)
                return false;

            //switch (SelectedUnite)
            //{
            //    case UniteType.Direction:
            //        return SelectedDirection == null ? true : SelectedDirection.EstGenerale ? grade.Id.StartsWith("DG") : grade.Id == "DIR";

            //    case UniteType.Departement:
            //        return grade.Id == "CD";

            //    case UniteType.Agence:
            //        return grade.Id == "CB";

            //    default:
            //        return false;
            //}
            return true;
        }

        private bool OnFilterFonction(object obj)
        {
            var fonction = obj as Fonction;

            if (fonction == null)
                return false;

            var pattern = FilterText.Trim().ToLower().NoAccent();

            return fonction.Intitule.ToLower().NoAccent().Contains(pattern)
                || fonction.Grade.Intitule.ToLower().NoAccent().Contains(pattern);

        }

        private bool editing = false;
        private string _action;
        private string _filterText;
        private int _count;
        private Fonction _fonction;
        private bool _fonctionLoading;
        private UniteType _selectedUnite;
        private bool _isSiege;
        private bool _initState;

        private bool _showDepartement;
        private bool _showEntite;
        private bool _showDirection;

        private Direction _selectedDirection;
        private Departement _selectedDepartement;
        private Entite _selectedAgence;
        private Grade _selectedGrade;
        // SelectedUnite

        public Direction SelectedDirection
        {
            get
            {
                return this._selectedDirection;
            }
            set
            {
                if (_selectedDirection != value)
                {
                    _selectedDirection = value;
                    RaisePropertyChanged(() => SelectedDirection);

                    if (SelectedUnite == UniteType.Direction)
                        Fonction.Unite = SelectedDirection;

                    GradesView.Refresh();

                    if(ShowDepartement)
                         LoadDepartements();

                    EstimateName();
                }
            }
        }
        public Departement SelectedDepartement
        {
            get
            {
                return this._selectedDepartement;
            }
            set
            {
                if (_selectedDepartement != value)
                {
                    _selectedDepartement = value;
                    RaisePropertyChanged(() => SelectedDepartement);

                    if (SelectedUnite == UniteType.Departement)
                        Fonction.Unite = SelectedDepartement;

                    LoadAgences();

                    EstimateName();
                }
            }
        }
        public Entite SelectedAgence
        {
            get
            {
                return this._selectedAgence;
            }
            set
            {
                if (_selectedAgence != value)
                {
                    _selectedAgence = value;
                    RaisePropertyChanged(() => SelectedAgence);

                    if (SelectedUnite == UniteType.Agence)
                        Fonction.Unite = (object)SelectedAgence;

                    EstimateName();
                }
            }
        }
        public Grade SelectedGrade
        {
            get
            {
                return this._selectedGrade;
            }
            set
            {
                if (_selectedGrade != value)
                {
                    _selectedGrade = value;
                    RaisePropertyChanged(() => SelectedGrade);

                    Fonction.Grade = SelectedGrade;

                    EstimateName();
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
        public bool InitState
        {
            get
            {
                return this._initState;
            }
            set
            {
                if (_initState != value)
                {
                    _initState = value;

                    RaisePropertyChanged(() => InitState);
                }
            }
        }
        public bool ShowDepartement
        {
            get
            {
                return this._showDepartement;
            }
            set
            {
                if (_showDepartement != value)
                {
                    _showDepartement = value;
                    RaisePropertyChanged(() => ShowDepartement);
                }
            }
        }
        public bool ShowAgence
        {
            get
            {
                return this._showEntite;
            }
            set
            {
                if (_showEntite != value)
                {
                    _showEntite = value;
                    RaisePropertyChanged(() => ShowAgence);
                }
            }
        }
        public bool ShowDirection
        {
            get
            {
                return this._showDirection;
            }
            set
            {
                if (_showDirection != value)
                {
                    _showDirection = value;
                    RaisePropertyChanged(() => ShowDirection);
                }
            }
        }
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
                    FonctionsView.Refresh();
                    RaisePropertyChanged(() => FilterText);
                }
            }
        }
        public int FonctionCount
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
                    RaisePropertyChanged(() => FonctionCount);
                }
            }
        }
        public Fonction Fonction
        {
            get
            {
                return this._fonction;
            }
            set
            {
                if (_fonction != value)
                {
                    _fonction = value;
                    RaisePropertyChanged(() => Fonction);
                }
            }
        }
        public bool FonctionLoading
        {
            get
            {
                return this._fonctionLoading;
            }
            set
            {
                if (_fonctionLoading != value)
                {
                    _fonctionLoading = value;
                    RaisePropertyChanged(() => FonctionLoading);
                }
            }
        }

        public UniteType SelectedUnite
        {
            get
            {
                return this._selectedUnite;
            }
            set
            {
                if (_selectedUnite != value)
                {
                    _selectedUnite = value;
                    RaisePropertyChanged(() => SelectedUnite);

                    Fonction.Unite = SelectedUnite == UniteType.Direction ? (object)SelectedDirection : SelectedUnite == UniteType.Departement ? (object)SelectedDepartement : (object)SelectedAgence;

                    ShowDirection = ShowDepartement = ShowAgence = false;
              
                    if (SelectedUnite == UniteType.Agence)
                    {
                        ShowAgence = true;
                        LoadAgences();
                    }
                    else
                    {
                        ShowDirection = true;
                        LoadDirections();
                        if(SelectedUnite == UniteType.Departement)
                        {
                            ShowDepartement = true;
                            //LoadDepartements();
                        }
                    }

                    GradesView.Refresh();

                    EstimateName();
                }
            }
        }

        void EstimateName()
        {
            if (Fonction.Unite == null || Fonction.Grade == null)
                Fonction.Intitule = string.Empty;
            else
                Fonction.Intitule = Fonction.EstimatedName;
        }

        #region Commands

        protected override async Task LoadData()
        {
            UpdateTimer.IsEnabled = false;

            //await LoadFonctionChanges();

            LastDataUpdateTime = DateTime.Now;

            UpdateTimer.IsEnabled = true;
        }

        protected override async Task Load(object param)
        { 
            IsSiege = AppConfig.CurrentUser.Entite.EstPrincipale;
            ////SelectedUnite = UniteType.Direction;

            //UniteTypesView.MoveCurrentToFirst();

            FonctionsView.GroupDescriptions.Clear();

            if (IsSiege)
                FonctionsView.GroupDescriptions.Add(new PropertyGroupDescription("Entite.Direction.Description"));
            else
                FonctionsView.GroupDescriptions.Add(new PropertyGroupDescription("Entite"));

            if (!IsInit)
            {
                //if (IsSiege)
                //    await LoadDirections();
                await LoadFonctions();
                await LoadGrades();
              
                LastDataUpdateTime = DateTime.Now;
                IsInit = true;
            }

            UpdateTimer.IsEnabled = true;
        }

        async Task LoadFonctions()
        {
            FonctionLoading = true;

            fonctions.Clear();

            FonctionCount = new FonctionDao().Count(AppConfig.CurrentUser.Entite);

            InitState = FonctionCount == 0;

            if (FonctionCount > 0)
                await Task.Run(() => new FonctionDao().GetAllAsync(AppConfig.CurrentUser.Entite, fonctions));

            FonctionLoading = false;
        }

        async Task LoadGrades()
        {
            grades.Clear();

            var list = await Task.Run(() => new GradeDao().GetAllAsync());

            list.ForEach(g => grades.Add(g));

            GradesView.MoveCurrentToFirst();
        }

        private async Task LoadDirections()
        {
            directions.Clear();

            var list = await Task.Run(() => new DirectionDao().GetAllAsync());

            list.ForEach(c => { directions.Add(c); });

            DirectionsView.MoveCurrentToFirst();
        }

        private void LoadDepartements()
        {
            departements.Clear();

            if (SelectedDirection == null && !ShowDepartement)
                return;

            SelectedDirection?.Departements.ForEach(d => departements.Add(d));

            DepartementsView.MoveCurrentToFirst();
        }

        private async Task LoadAgences()
        {
            agences.Clear();

            if (SelectedDirection == null & !ShowAgence)
                return;

            var list = await Task.Run(() => new EntiteDao().GetAllAgencesAsync());

            list.ForEach(b => agences.Add(b));

            AgencesView.MoveCurrentToFirst();

        }

        async Task LoadFonctionChanges()
        {
            FonctionCount = new FonctionDao().Count(AppConfig.CurrentUser.Entite);
            InitState = FonctionCount == 0;

            if (FonctionCount == 0)
                return;

            var list = await Task.Run(() => new FonctionDao().GetAllAsync(LastDataUpdateTime));

            list.ForEach(d =>
            {
                var _d = fonctions.ToList().Find(e => e.Equals(d));

                if (_d != null) fonctions.Remove(_d);

                if (_d.Equals(Fonction))
                    Fonction = d;

                fonctions.Add(d);
            });

            FonctionsView.Refresh();
        }

        private List<Fonction> GenerateAllEntityFunctions()
        {
            var list = new List<Fonction>();

            if (IsSiege)
            {
                foreach (var direction in directions)
                {
                    if (direction.EstGenerale)
                    {
                        var dgF = new Fonction()
                        {
                            Niveau = UniteType.Direction,
                            Unite = direction,
                            Grade = grades.ToList().Find(g => g.Id == "DG"),
                            Entite = AppConfig.CurrentUser.Entite
                        };

                        dgF.Intitule = dgF.EstimatedName;

                        list.Add(dgF);

                        var dgaF = new Fonction()
                        {
                            Niveau = UniteType.Direction,
                            Unite = direction,
                            Grade = grades.ToList().Find(g => g.Id == "DGA"),
                            Entite = AppConfig.CurrentUser.Entite
                        };

                        dgaF.Intitule = dgaF.EstimatedName;

                        list.Add(dgaF);
                    }
                    else
                    {
                        var fonc = new Fonction()
                        {
                            Niveau = UniteType.Direction,
                            Unite = direction,
                            Grade = grades.ToList().Find(g => g.Id == "DIR"),
                            Entite = AppConfig.CurrentUser.Entite
                        };

                        fonc.Intitule = fonc.EstimatedName;

                        list.Add(fonc);
                    }

                    //if (direction.Secretariat != null)
                    //{
                    //    var fonc = new Fonction()
                    //    {
                    //        Niveau = UniteType.Agence,
                    //        Unite = direction.Secretariat,
                    //        Grade = grades.ToList().Find(g => g.Id == "CB"),
                    //        Entite = AppConfig.CurrentUser.Entite
                    //    };

                    //    fonc.Intitule = fonc.EstimatedName;

                    //    list.Add(fonc);
                    //}

                    //foreach (var departement in direction.Departements)
                    //{
                    //    var fonc = new Fonction()
                    //    {
                    //        Niveau = UniteType.Departement,
                    //        Unite = departement,
                    //        Grade = grades.ToList().Find(g => g.Id == "CD"),
                    //        Entite = AppConfig.CurrentUser.Entite
                    //    };

                    //    fonc.Intitule = fonc.EstimatedName;

                    //    list.Add(fonc);

                    //    foreach (var bureau in departement.Agences)
                    //    {
                    //        for (int i = 0; i < bureau.NombreChefs; i++)
                    //        {
                    //            fonc = new Fonction()
                    //            {
                    //                Niveau = UniteType.Agence,
                    //                Unite = bureau,
                    //                Grade = grades.ToList().Find(g => g.Id == "CB"),
                    //                Entite = AppConfig.CurrentUser.Entite
                    //            };

                    //            fonc.Intitule = Fonction.EstimateName(fonc, i + 1);

                    //            list.Add(fonc);
                    //        }

                    //    }
                    //}
                }
            }
            else
            {
                //var departement = AppConfig.CurrentUser.Entite.Departement;

                //var fonc = new Fonction()
                //{
                //    Niveau = UniteType.Departement,
                //    Unite = departement,
                //    Grade = grades.ToList().Find(g => g.Id == "CD"),
                //    Entite = AppConfig.CurrentUser.Entite
                //};

                //fonc.Intitule = fonc.EstimatedName;

                //list.Add(fonc);

                //foreach (var bureau in departement.Agences)
                //{
                //    for (int i = 0; i < bureau.NombreChefs; i++)
                //    {
                //        fonc = new Fonction()
                //        {
                //            Niveau = UniteType.Agence,
                //            Unite = bureau,
                //            Grade = grades.ToList().Find(g => g.Id == "CB"),
                //            Entite = AppConfig.CurrentUser.Entite
                //        };

                //        fonc.Intitule = Fonction.EstimateName(fonc, i + 1);

                //        list.Add(fonc);
                //    }
                    
                //}
            }

            return list;
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
            if (!editing)
            {
                Fonction.Niveau = SelectedUnite;
                Fonction.Entite = AppConfig.CurrentUser.Entite;

                if (new FonctionDao().Add(Fonction) > 0)
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Fonction + "",
                            string.Format("Enregistrement de la fonction '{0}' (ID : {1}).", Fonction.Intitule, Fonction.Id)
                        );

                    fonctions.Add(Fonction);
                    FonctionCount++;
                    FonctionsView.Refresh();
                    Status = "Fonction enregistrée avec succès !";
                    InitSave();
                }
            }
            else
            {
                var clone = (Fonction)Fonction.Clone();
                clone.Id = string.Empty;

                fonctions.Remove(Fonction);

                if (!fonctions.Contains(clone))
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Fonction + "",
                            string.Format("Modification de la fonction '{0}' (ID : {1}).", Fonction.Intitule, Fonction.Id)
                        );

                    if (new FonctionDao().Update(Fonction, clone) > 0)
                    {
                        Status = "Fonction modifiée avec succès !";
                        fonctions.Add(Fonction);
                        FonctionsView.Refresh();
                        InitSave();
                    }
                }
                else
                {
                    Status = "Une fonction avec la même description existe déjà !";
                    Fonction.CancelEdit();
                    fonctions.Add(Fonction);
                    FonctionsView.Refresh();
                }

            }
        }

        private void InitSave()
        {
            var grade = Fonction?.Grade;

            Fonction = new Fonction();
            Action = "Enregistrer";
            editing = false;
            Title = "Nouvelle fonction";

            if (grade != null)
                GradesView.MoveCurrentTo(grade);
            else
                GradesView.MoveCurrentToFirst();
        }

        private void MenuInit()
        {
            Name = "Fonction";
            OptionItem = new OptionItem()
            {
                Name = "Fonction",
                ToolTip = "Fonction",
                IconPathData = "M11.600006,11.299994C10.900024,11.299994 7.8000183,12.600012 8.1000061,12.9 8.5,13.299995 9.6000061,14.100013 10,14.100013 10.400024,14.100013 11.600006,11.799994 11.700012,11.299994z M6.9000244,5.8999968C6.4000244,5.8999968 5.9000244,5.8999968 5.8000183,6.0000029 5.5,6.2000151 4.8000183,9.10001 5.2000122,8.7999917 5.6000061,8.5000039 7.9000244,6.2999907 8.1000061,6.100009 8.2000122,6.0000029 7.6000061,5.8999968 6.9000244,5.8999968z M7.0312629,2.7609373C7.4437637,2.7624982 7.8750153,2.7749951 8.3000183,2.7999893 10,2.799989 11.800018,2.8999951 12.100006,3.0000015 12.700012,3.2999893 15.300018,7.5000034 15.800018,8.2000161 16.300018,8.7999917 15.700012,9.6000109 15.700012,9.6000109L12,16.20002 19.900024,22.600018 20,22.600018C20.200012,22.600018 20.300018,22.700024 20.300018,22.700024 21.300018,21.700022 22.200012,21.400003 22.900024,21.400003 24.400024,21.400003 25.300018,22.799999 25.300018,22.799999 25.300018,22.799999 28,22.400005 28.300018,25.700024 28.300018,25.700024 29.400024,25.799999 29.400024,26.700026 29.400024,26.700026 31.600006,26.300001 32,28.300001L12.300018,28.300001C12.300018,26.500013 15.400024,26.60002 15.400024,26.60002 15.5,24.700024 17.200012,24.900005 17.200012,24.900005 17.200012,23.600018 17.800018,23.000012 18.400024,22.700024L18.5,22.700024 11.5,17.000008 11.300018,17.299995C11.300018,17.299995 11.800018,17.900002 12.400024,18.400003 13,18.900003 12.400024,19.900003 12.400024,19.900003L8.4000244,27.60002C8.4000244,27.60002 7.7000122,28.400007 6.5,27.800001 5.4000244,27.300001 5.7000122,26.100018 5.7000122,26.100018 5.7000122,26.100018 9.1000061,19.700022 9,19.299997 8.8000183,19.200022 5.3000183,15.000007 5.3000183,15.000007 5.3000183,15.000007 5.2000122,19.600016 5.2000122,20.100016 5.1000061,20.600016 2.8000183,27.900007 2.8000183,27.900007 2.8000183,27.900007 1.8000183,28.500013 1,28.200026 0.1000061,27.800001 0,26.900007 0,26.900007 0,26.900007 1.8000183,20.600016 1.8000183,20.100016 1.9000244,19.600016 1.7000122,12.700018 1.8000183,12.4 1.9000244,12.000006 3.1000061,10.899999 3.1000061,10.899999L2.3000183,10.299993 2.3000183,9.8999987 2.3000183,9.7999926 0.80001831,8.5000039 1.4000244,7.7000156 2.5,8.60001 2.5,8.5000039C2.8000183,6.7000151 3.4000244,3.7000139 4.5,3.0000015 4.7250137,2.8499921 5.7937603,2.7562535 7.0312629,2.7609373z M17.400024,0C19,-6.0725142E-08 20.400024,1.2999883 20.400024,3.0000015 20.400024,4.6000081 19.100006,6.0000029 17.400024,6.0000029 15.800018,6.0000029 14.400024,4.7000142 14.400024,3.0000015 14.400024,1.2999883 15.800018,-6.0725142E-08 17.400024,0z"
            };
        }

        private bool CanSave()
        {
            return Fonction.Error == string.Empty;
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
                Fonction.CancelEdit();

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
            if (param is Fonction)
            {
                Fonction = (Fonction)param;
                Fonction.BeginEdit();
                editing = true;
                Action = "Modifier";
                Title = "Modification de la fonction";

                GradesView.MoveCurrentTo(grades.ToList().Find(d => d.Equals(Fonction.Intitule)));
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

        public string Error
        {
            get
            {
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
                    case "SelectedGrade":
                        error = Fonction["Grade"];
                        break;
                    default:
                        break;
                }

                return error;
            }
        }

        private void Delete(object param)
        {
            Status = string.Empty;

            if (param is Fonction)
            {
                var fonction = (Fonction)param;

                var msg = string.Format("Êtes - vous sûr(e) de vouloir supprimer la fonction <<{0}>> ?", fonction.Intitule);

                if (MyMsgBox.Show(msg, "Humager", MyMsgBoxButton.YesNoCancel, MyMsgBoxIcon.Warning) == DialogueResult.Yes)
                {
                    if (new FonctionDao().Delete(fonction) > 0)
                    {
                        Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Fonction + "",
                            string.Format("Suppression de la fonction '{0}' (ID : {1}).", fonction.Intitule, fonction.Id)
                        );

                        fonctions.Remove(fonction);
                        FonctionCount--;

                        Status = "Fonction supprimée avec succès !";
                    }
                    else
                    {
                        Status = "Suppression de la fonction échouée ! Vérifiez que vous êtes bien connecté au serveur de données, " +
                            "sinon cette fonction est réliée à d'autres objets et ne peut donc pas être supprimée.";
                    }
                }

            }
        }

        private bool CanDelete(object param)
        {
            return !editing;
        }

        public ICommand AddAllCommand
        {
            get
            {
                if (_addAllCommand == null)
                    _addAllCommand = new RelayCommand(p => AddAll(p), p => CanAddAll(p));

                return _addAllCommand;
            }
        }

        private void AddAll(object param)
        {
            var fonctions = GenerateAllEntityFunctions();

            var vm = new EntiteAddAllFonctionsViewModel(AppConfig.CurrentUser.Entite, fonctions);
            var feed = facade.ShowDialog(vm, param as Window);

            if (feed == Dialog.Service.DialogResult.Yes)
            {
                FonctionCount = fonctions.Count;

                fonctions.ForEach(f => this.fonctions.Add(f));

                FonctionsView.Refresh();
            }
            
        }

        private bool CanAddAll(object p)
        {
            return fonctions.Count == 0;
        }

        private RelayCommand _saveCommand;
        private RelayCommand _cancelCommand;
        private RelayCommand _editCommand;
        private RelayCommand _deleteCommand;
        private RelayCommand _addAllCommand;

        #endregion

    }
}
