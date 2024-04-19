using FingerPrintManagerApp.Dao.Employe;
using FingerPrintManagerApp.Dialog.Facade;
using FingerPrintManagerApp.Dialog.Service;
using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Admin.Util;
using FingerPrintManagerApp.Model.Employe;
using FingerPrintManagerApp.ViewModel.Command;
using FingerPrintManagerApp.ViewModel.Contract;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace FingerPrintManagerApp.Modules.Employe.ViewModel 
{
    public class EditEmployeViewModel : DialogViewModelBase, IDataErrorInfo, IChooserViewModel
    {
        // Collections
        private ObservableCollection<Province> provincesOrigine;
        private ObservableCollection<Province> provinces;
        private ObservableCollection<Zone> zones;
        private ObservableCollection<Commune> communes;

        private ObservableCollection<Grade> initGrades;
        private ObservableCollection<Grade> currentGrades;
        private ObservableCollection<Grade> currentStatutaireGrades;

        private ObservableCollection<Departement> departements;

        private ObservableCollection<Entite> entites;
        private ObservableCollection<int> annees;
        private ObservableCollection<EmployeEmpreinte> empreintes;

        // Collection Views
        public ICollectionView ProvincesOrigineView { get; private set; }
        public ICollectionView ProvincesView { get; private set; }
        public ICollectionView ZonesView { get; private set; }
        public ICollectionView CommunesView { get; private set; }
        public ICollectionView SexesView { get; private set; }
        public ICollectionView EtatsCivilsView { get; private set; }
        public ICollectionView EnfantsView { get; private set; }
        public ICollectionView InitGradesView { get; private set; }
        public ICollectionView CurrentGradesView { get; private set; }
        public ICollectionView CurrentStatutaireGradesView { get; private set; }
        public ICollectionView CurrentFonctionsView { get; private set; }
        public ICollectionView NiveauxView { get; private set; }
        public ICollectionView UniteTypesView { get; private set; }
        public ICollectionView DomainesView { get; private set; }
        public ICollectionView AnneesView { get; private set; }
        public ICollectionView UnitesView { get; private set; }
        public ICollectionView DirectionsView { get; private set; }
        public ICollectionView DivisionsView { get; private set; }
        public ICollectionView BureauxView { get; private set; }
        public ICollectionView EtatsView { get; private set; }
        public ICollectionView EntitesView { get; private set; }
        public ICollectionView EmpreintesView { get; private set; }

        // View Models
        private IDialogFacade facade;
        private ICallerViewModel callerViewModel;
        private IFilePathProvider filePathProvider;
        private IPhotoCapture photoCapture;
        
        public Array sexes { get; set; }
        public Array etatsCivils { get; set; }
        public Array uniteTypes { get; set; }

        public EditEmployeViewModel(IDialogFacade facade, ICallerViewModel callerViewModel, IFilePathProvider filePathProvider, IPhotoCapture photoCapture)
        {
            this.callerViewModel = callerViewModel;
            this.filePathProvider = filePathProvider;
            this.photoCapture = photoCapture;
            this.facade = facade;

            provinces = new ObservableCollection<Province>();
            ProvincesView = (CollectionView)CollectionViewSource.GetDefaultView(provinces);
            ProvincesView.SortDescriptions.Add(new SortDescription("Nom", ListSortDirection.Ascending));

            provincesOrigine = new ObservableCollection<Province>();
            ProvincesOrigineView = (CollectionView)CollectionViewSource.GetDefaultView(provincesOrigine);
            ProvincesOrigineView.SortDescriptions.Add(new SortDescription("Nom", ListSortDirection.Ascending));

            zones = new ObservableCollection<Zone>();
            ZonesView = (CollectionView)CollectionViewSource.GetDefaultView(zones);
            ZonesView.SortDescriptions.Add(new SortDescription("Nom", ListSortDirection.Ascending));

            communes = new ObservableCollection<Commune>();
            CommunesView = (CollectionView)CollectionViewSource.GetDefaultView(communes);
            CommunesView.SortDescriptions.Add(new SortDescription("Nom", ListSortDirection.Ascending));

   

            empreintes = new ObservableCollection<EmployeEmpreinte>();
            EmpreintesView = (CollectionView)CollectionViewSource.GetDefaultView(empreintes);

            sexes = Enum.GetValues(typeof(Sex));
            SexesView = (CollectionView)CollectionViewSource.GetDefaultView(sexes);

            uniteTypes = Enum.GetValues(typeof(UniteType));
            UniteTypesView = (CollectionView)CollectionViewSource.GetDefaultView(uniteTypes);
            UniteTypesView.Filter = OnTypeFilter;

            etatsCivils = Enum.GetValues(typeof(EtatCivil));
            EtatsCivilsView = (CollectionView)CollectionViewSource.GetDefaultView(etatsCivils);


            entites = new ObservableCollection<Entite>();
            EntitesView = (CollectionView)CollectionViewSource.GetDefaultView(entites);

            initGrades = new ObservableCollection<Grade>();
            InitGradesView = (CollectionView)CollectionViewSource.GetDefaultView(initGrades);
            InitGradesView.SortDescriptions.Add(new SortDescription("Niveau", ListSortDirection.Ascending));
            InitGradesView.Filter += OnInitGradeFilter;

            currentStatutaireGrades = new ObservableCollection<Grade>();
            CurrentStatutaireGradesView = (CollectionView)CollectionViewSource.GetDefaultView(currentStatutaireGrades);
            CurrentStatutaireGradesView.SortDescriptions.Add(new SortDescription("Niveau", ListSortDirection.Ascending));

            currentGrades = new ObservableCollection<Grade>();
            CurrentGradesView = (CollectionView)CollectionViewSource.GetDefaultView(currentGrades);
            CurrentGradesView.SortDescriptions.Add(new SortDescription("Niveau", ListSortDirection.Ascending));
            CurrentGradesView.Filter += OnCurrentGradeFilter;

            
            departements = new ObservableCollection<Departement>();
            DivisionsView = (CollectionView)CollectionViewSource.GetDefaultView(departements);
            DivisionsView.SortDescriptions.Add(new SortDescription("Denomination", ListSortDirection.Ascending));

            
            NombreEnfantString = "0";

            annees = new ObservableCollection<int>();
            AnneesView = (CollectionView)CollectionViewSource.GetDefaultView(annees);

            int k = 0;
            int year = DateTime.Today.Year;

            while (k < 60)
            {
                annees.Add(year - k);
                k++;
            }

            AnneesView.MoveCurrentToFirst();

            InitSave();

            IsSiege = AppConfig.CurrentUser.Entite.EstPrincipale;

            InitStepHeaders();
            
        }



        private bool OnTypeFilter(object obj)
        {
            var type = (UniteType)obj;

            return IsSiege || type != UniteType.Direction;
        }

        private bool OnInitGradeFilter(object obj)
        {
            var grade = obj as Grade;

            if (grade == null)
                return false;

            return grade.Niveau < 10;
        }

        private bool OnCurrentGradeFilter(object obj)
        {
            var grade = obj as Grade;

            if (grade == null)
                return false;

            if (InitGrade == null || InitGrade.Id == null)
                return false;

            return grade.Niveau >= InitGrade.Niveau && grade.Niveau < 10;
        }

        Model.Employe.Employe toEditEmploye;

        public EditEmployeViewModel(IDialogFacade facade, ICallerViewModel callerViewModel, IFilePathProvider filePathProvider, IPhotoCapture photoCapture, Model.Employe.Employe toEditEmploye)
            : this(facade, callerViewModel, filePathProvider, photoCapture)
        {
            this.toEditEmploye = toEditEmploye;
            Action = "Modifier";
            Title = "Modification de l'employé";
            ForAdding = false;
        }

        Grade _initGrade;
        Grade _currentGrade;
        Grade _currentStatutaireGrade;


        DateTime _dateEngagement;
        DateTime _dateCurrentGrade;
        DateTime _dateCurrentStatutaireGrade;
        DateTime _dateCurrentFonction;

        private bool _step1;
        private bool _step2;
        private bool _step3;
        private bool _step4;
        private bool _adding;
        private bool _employeLoading;
        private bool _engagementNotInit;
        private string _matricule;
        private string _nombreEnfantString;
        private int _nbreEnfant;
        private Province _selectedProvince;
        private Zone _selectedZone;
        private Entite _selectedEntite;
        private bool _isSiege;
        private bool _estAffecte;


        void InitStepHeaders()
        {
            SegmentedOptions = new ObservableCollection<OptionItem>();
            SegmentedOptions.Add(new OptionItem()
            {
                Id = "1",
                Name = "Profil",
                ToolTip = "Infos personnelles"
            });

            SegmentedOptions.Add(new OptionItem()
            {
                Id = "2",
                Name = "Profession",
                ToolTip = "Infos professionnelles"
            });

            SegmentedOptions.Add(new OptionItem()
            {
                Id = "3",
                Name = "Famille",
                ToolTip = "Situation familiale"
            });

            SegmentedOptions.Add(new OptionItem()
            {
                Id = "4",
                Name = "Soumission",
                ToolTip = "Revoir et valider"
            });

            SelectedItem = SegmentedOptions[0];
            Step1 = true;
        }

        public bool Step1
        {
            get
            {
                return this._step1;
            }
            set
            {
                if (_step1 != value)
                {
                    _step1 = value;
                    RaisePropertyChanged(() => Step1);
                    RaisePropertyChanged(() => NotStep1);
                }
            }
        }
        public bool NotStep1
        {
            get
            {
                return !Step1;
            }
        }
        public bool Step2
        {
            get
            {
                return this._step2;
            }
            set
            {
                if (_step2 != value)
                {
                    _step2 = value;
                    RaisePropertyChanged(() => Step2);
                }
            }
        }
        public bool Step3
        {
            get
            {
                return this._step3;
            }
            set
            {
                if (_step3 != value)
                {
                    _step3 = value;
                    RaisePropertyChanged(() => Step3);
                }
            }
        }
        public bool Step4
        {
            get
            {
                return this._step4;
            }
            set
            {
                if (_step4 != value)
                {
                    _step4 = value;
                    RaisePropertyChanged(() => Step4);
                }
            }
        }
        public bool NoEmpreinte
        {
            get
            {
                return Employe.Empreintes.Count == 0;
            }
        }
        public bool EngagementNotInit
        {
            get
            {
                return this._engagementNotInit;
            }
            set
            {
                if (_engagementNotInit != value)
                {
                    _engagementNotInit = value;
                    RaisePropertyChanged(() => EngagementNotInit);

                    if (value)
                        DateCurrentGrade = DateEngagement;
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

                    UniteTypesView.Refresh();

                    SelectedUnite = (UniteType)uniteTypes.GetValue(2);
                }
            }
        }
        public bool EstAffecte
        {
            get
            {
                return this._estAffecte;
            }
            set
            {
                if (_estAffecte != value)
                {
                    _estAffecte = value;

                    RaisePropertyChanged(() => EstAffecte);

                    Employe.EstAffecte = EstAffecte;

                    //if (EstAffecte && CurrentGrade != null && CurrentGrade.Niveau >= 7)
                    //    Employe.CurrentFonctionNomination.IsRequired = true;
                }
            }
        }


        public Grade InitGrade
        {
            get
            {
                return this._initGrade;
            }
            set
            {
                if (_initGrade != value)
                {
                    
                    RaisePropertyChanged(() => InitGrade);

                    if (CurrentStatutaireGrade == null || CurrentStatutaireGrade.Id == null ||
                        CurrentStatutaireGrade.Equals(_initGrade))
                        CurrentStatutaireGrade = value;

                    _initGrade = value;

                    Employe.CurrentGrade.Grade = _initGrade;
                    CurrentGradesView.Refresh();
                }
            }
        }

        public Grade CurrentStatutaireGrade
        {
            get
            {
                return this._currentStatutaireGrade;
            }
            set
            {
                if (_currentStatutaireGrade != value)
                {
                    if (CurrentGrade == null || CurrentGrade.Id == null ||
                        CurrentGrade.Equals(_currentStatutaireGrade))
                        CurrentGrade = value;

                    _currentStatutaireGrade = value;

                    RaisePropertyChanged(() => CurrentStatutaireGrade);

                    //Employe.CurrentStatutaireGradeNomination.Grade = _currentStatutaireGrade;
                }
            }
        }

        public Grade CurrentGrade
        {
            get
            {
                return this._currentGrade;
            }
            set
            {
                if (_currentGrade != value)
                {
                    _currentGrade = value;
                    RaisePropertyChanged(() => CurrentGrade);

                    //Employe.CurrentGradeNomination.Grade = _currentGrade;

                    //if (EstAffecte && CurrentGrade != null && CurrentGrade.Niveau >= 7)
                    //    Employe.CurrentFonctionNomination.IsRequired = true;
                }
            }
        }

        public DateTime DateEngagement
        {
            get
            {
                return this._dateEngagement;
            }
            set
            {
                if (_dateEngagement != value)
                {
                    RaisePropertyChanged(() => DateEngagement);

                    if (DateCurrentGrade == _dateEngagement)
                        DateCurrentGrade = value;

                    _dateEngagement = value;

                    //Employe.GradeEngagement.Date = _dateEngagement;
                }
            }
        }

        public DateTime DateCurrentGrade
        {
            get
            {
                return this._dateCurrentGrade;
            }
            set
            {
                if (_dateCurrentGrade != value)
                {
                    if (DateCurrentFonction == _dateCurrentGrade)
                        DateCurrentFonction = value;

                    _dateCurrentGrade = value;

                    //Employe.CurrentGradeNomination.Date = _dateCurrentGrade;

                    RaisePropertyChanged(() => DateCurrentGrade);
                }
            }
        }

        public DateTime DateCurrentStatutaireGrade
        {
            get
            {
                return this._dateCurrentStatutaireGrade;
            }
            set
            {
                if (_dateCurrentStatutaireGrade != value)
                {
                    if (DateCurrentGrade == _dateCurrentStatutaireGrade)
                        DateCurrentGrade = value;

                    _dateCurrentStatutaireGrade = value;

                    //Employe.CurrentStatutaireGradeNomination.Date = _dateCurrentStatutaireGrade;

                    RaisePropertyChanged(() => DateCurrentStatutaireGrade);
                }
            }
        }
        public DateTime DateCurrentFonction
        {
            get
            {
                return this._dateCurrentFonction;
            }
            set
            {
                if (_dateCurrentFonction != value)
                {
                    _dateCurrentFonction = value;

                    //Employe.CurrentFonctionNomination.Date = _dateCurrentFonction;

                    RaisePropertyChanged(() => DateCurrentFonction);
                }
            }
        }

        public bool ForAdding
        {
            get
            {
                return this._adding;
            }
            set
            {
                if (_adding != value)
                {
                    _adding = value;
                    RaisePropertyChanged(() => ForAdding);
                }
            }
        }
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
        public string Matricule
        {
            get
            {
                return this._matricule;
            }
            set
            {
                if (_matricule != value)
                {
                    _matricule = value;
                    RaisePropertyChanged(() => Matricule);
                    Employe.Matricule = Matricule;
                }
            }
        }
        public string NombreEnfantString
        {
            get
            {
                return this._nombreEnfantString;
            }
            set
            {
                if (_nombreEnfantString != value)
                {
                    _nombreEnfantString = value.Trim();
                    RaisePropertyChanged(() => NombreEnfantString);

                    int nbre = 0;
                    int.TryParse(NombreEnfantString.Trim(), out nbre);
                    NombreEnfant = nbre;
                }
            }
        }
        public int NombreEnfant
        {
            get
            {
                return this._nbreEnfant;
            }
            set
            {
                if (_nbreEnfant != value)
                {
                    _nbreEnfant = value;
                    RaisePropertyChanged(() => NombreEnfant);
                }
            }
        }
        public Province SelectedProvince
        {
            get
            {
                return this._selectedProvince;
            }
            set
            {
                if (_selectedProvince != value)
                {
                    _selectedProvince = value;
                    RaisePropertyChanged(() => SelectedProvince);
                    
                }
            }
        }
        public Zone SelectedZone
        {
            get
            {
                return this._selectedZone;
            }
            set
            {
                if (_selectedZone != value)
                {
                    _selectedZone = value;
                    RaisePropertyChanged(() => SelectedZone);
                   
                }
            }
        }

        public Entite SelectedEntite
        {
            get
            {
                return this._selectedEntite;
            }
            set
            {
                if (_selectedEntite != value)
                {
                    _selectedEntite = value;
                    RaisePropertyChanged(() => SelectedEntite);

                    IsSiege = SelectedEntite.EstPrincipale;

                    if (IsInit)
                    {
                        if (IsSiege)
                            LoadDivisions();
                        else
                            LoadDivisionProvinciale();
                    }

                    Employe.CurrentAffectation.Entite = SelectedEntite;
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

        private Model.Employe.Employe _employe;
        public Model.Employe.Employe Employe
        {
            get
            {
                return this._employe;
            }
            set
            {
                if (_employe != value)
                {
                    _employe = value;
                    RaisePropertyChanged(() => Employe);
                }
            }
        }

        private Departement _selectedDivision;
        public Departement SelectedDivision
        {
            get
            {
                return this._selectedDivision;
            }
            set
            {
                if (_selectedDivision != value)
                {
                    _selectedDivision = value;
                    RaisePropertyChanged(() => SelectedDivision);

                    //if (SelectedUnite == UniteType.Division)
                    //{
                    //    Employe.CurrentAffectation.Unite = SelectedDivision;
                    //    //LoadFonctions(SelectedDivision);
                    //}

                    //LoadBureaux();
                }
            }
        }

   

        private bool _showDivision;
        public bool ShowDivision
        {
            get
            {
                return this._showDivision;
            }
            set
            {
                if (_showDivision != value)
                {
                    _showDivision = value;
                    RaisePropertyChanged(() => ShowDivision);

                }
            }
        }

        private bool _showBureau;
        public bool ShowBureau
        {
            get
            {
                return this._showBureau;
            }
            set
            {
                if (_showBureau != value)
                {
                    _showBureau = value;
                    RaisePropertyChanged(() => ShowBureau);

                }
            }
        }

        private UniteType _selectedUnite;
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
                    Employe.CurrentAffectation.Niveau = SelectedUnite;

                    //Employe.CurrentAffectation.Unite = SelectedUnite == UniteType.Direction ? (object)SelectedDirection : SelectedUnite == UniteType.Division ? (object)SelectedDivision : (object)SelectedBureau;

                    //LoadFonctions(Employe.CurrentAffectation.Unite);

                    ShowDivision = ShowBureau = false;

                    //if (SelectedUnite == UniteType.Division)
                    //    ShowDivision = true;
                    //else if (SelectedUnite == UniteType.Bureau)
                    //    ShowBureau = ShowDivision = true;
                }
            }
        }

        #region Validation

        private bool Step1IsValid()
        {
            return Employe["Nom"] == string.Empty &&
                Employe["PostNom"] == string.Empty &&
                Employe["Prenom"] == string.Empty &&
                Employe["LieuNaissance"] == string.Empty &&
                Employe["DateNaissance"] == string.Empty &&
                Employe["ProvinceOrigine"] == string.Empty &&
                this["NombreEnfant"] == string.Empty &&
                //Employe.CurrentHighEtude.Error == string.Empty &&
                Employe["Telephone"] == string.Empty &&
                Employe["Email"] == string.Empty &&
                Employe["PersonneContact"] == string.Empty &&
                Employe["QualiteContact"] == string.Empty;
                //Employe.Address.Error == string.Empty;

        }

        private bool Step2IsValid()
        {
            return Employe["Etat"] == string.Empty &&
                this["Matricule"] == string.Empty &&
                Employe.CurrentGrade.Error == string.Empty &&
                (!Employe.EstAffecte || (Employe.EstAffecte && Employe.CurrentAffectation.Error == string.Empty));
                //&&
              
                //(!Employe.CurrentFonctionNomination.IsRequired || (Employe.CurrentFonctionNomination.IsRequired && Employe.CurrentFonctionNomination.Error == string.Empty));

        }

        private bool Step3IsValid()
        {
            return Employe["Conjoint"] == string.Empty &&
                Employe["TelephoneConjoint"] == string.Empty &&
                Employe["Enfants"] == string.Empty;
        }

        #endregion

        #region Commands
        protected override async Task Load(object param = null)
        {
            Status = string.Empty;

            SelectedEntite = AppConfig.CurrentUser.Entite;

            if (!ForAdding)
            {
                Employe = toEditEmploye;
                Employe.BeginEdit();

                LoadEmpreintes();
            }

            if (IsSiege) ;
            //await LoadDirections();
            else
                await LoadDivisionProvinciale();

            await LoadGrades();
            await LoadEntites();

            IsInit = true;
        }

        private void LoadEmpreintes()
        {
            empreintes.Clear();
            Employe.Empreintes.ForEach(e => empreintes.Add(e));

            RaisePropertyChanged(() => NoEmpreinte);
        }

        private async Task LoadGrades()
        {
            initGrades.Clear();
            currentGrades.Clear();
            currentStatutaireGrades.Clear();

            var list = await Task.Run(() => new GradeDao().GetAllAsync());

            list.ForEach(g => { initGrades.Add(g); currentGrades.Add(g); currentStatutaireGrades.Add(g); });

            //if (initGrades.Count > 0 && SelectedNiveau != null)
            //    InitGradesView.MoveCurrentTo(initGrades.ToList().Find(n => n.Equals(SelectedNiveau.GradeRecrutement)));
            //else 
            //    InitGradesView.MoveCurrentToFirst();
        }



        private async Task LoadEntites()
        {
            entites.Clear();

            if (!IsSiege)
                entites.Add(AppConfig.CurrentUser.Entite);
            else
            {
                var list = await Task.Run(() => new EntiteDao().GetAllAsync());
                list.ForEach(e => entites.Add(e));
            }

            EntitesView.MoveCurrentToFirst();

        }

      

        private void LoadDivisions()
        {
            departements.Clear();

            //if (SelectedDirection == null)
            //    return;

            //SelectedDirection.Divisions.ForEach(d => departements.Add(d));

            //if (!ForAdding)
            //{
            //    var uniteType = Employe.CurrentAffectation.Niveau;

            //    var departement = uniteType == UniteType.Direction ? SelectedDirection.Divisions.First() :
            //        uniteType == UniteType.Division ? (Division)Employe.CurrentAffectation.Unite : ((Bureau)Employe.CurrentAffectation.Unite).Division;

            //    DivisionsView.MoveCurrentTo(SelectedDirection.Divisions.Find(c => c.Equals(departement)));
            //}
            //else
            //    DivisionsView.MoveCurrentToFirst();
        }

        async Task LoadDivisionProvinciale()
        {
            //departements.Clear();

            //var departement = SelectedEntite.Division;

            //if (departement.Bureaux.Count == 0)
            //    departement.Bureaux = await Task.Run(() => new BureauDao().GetAllAsync(departement, true));

            //departements.Add(departement);
            //DivisionsView.MoveCurrentToFirst();
        }



   

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new AsyncCommand(p => Save(p), p => CanSave());

                return _saveCommand;
            }
        }

        private async Task Save(object param)
        {
            Status = string.Empty;

            //if (Employe.CurrentFonctionNomination.IsRequired)
            //    if (Employe.CurrentFonctionNomination.GradeAssocie.Equals(Employe.GradeEngagement))
            //        Employe.CurrentFonctionNomination.GradeAssocie.EstInitial = true;

            //if (Employe.EstAffecte)
            //    Employe.CurrentAffectation.Date = Employe.CurrentGradeNomination.Date;
            //else
            //    Employe.CurrentAffectation.Unite = null;

            if (ForAdding)
            {
                EmployeLoading = true;

                if (await new EmployeDao().AddAsync(Employe) > 0)
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Employé + "",
                            string.Format("Enregistrement de l'employé '{0}' (ID : {1}).", Employe.Name, Employe.Id)
                        );

                    Status = "Employé enregistré avec succès !";

                    callerViewModel.AddObject(Employe);

                    await Task.Delay(3000);

                    CloseDialogWithResult(param as Window, DialogResult.Yes);
                    
                }
                else
                {
                    Status = "Enregistrement échoué. Vérifiez votre connexion au serveur puis réessayer !";
                }

                EmployeLoading = false;
            }

            
        }

        private bool CanSave()
        {
            return Step4;
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

        public string Error
        {
            get
            {
                if (this["Matricule"] != string.Empty)
                    return this["Matricule"];

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
                    case "Matricule":
                        error = Employe["Matricule"];

                        if (error == "")
                        {
                            var count = ForAdding ? new EmployeDao().Count(Matricule) : new EmployeDao().Count(Matricule, Employe);

                            if (count > 0)
                                error = "Ce matricule est déjà utilisé par un autre employé";
                        }
                        break;

                    case "NombreEnfant":
                        if (NombreEnfant < 0)
                            error = "Le nombre d'enfant ne doit pas être négatif.";
                        break;

                    case "NombreEnfantString":
                        int nbre;
                        if (!int.TryParse(NombreEnfantString.Trim(), out nbre))
                            error = "La chaîne saisie n'est pas un numérique";
                        else
                            error = this["NombreEnfant"];
                        break;

                    //case "SelectedNiveau":
                    //    if (SelectedNiveau == null)
                    //        error = Employe.CurrentHighEtude["Niveau"];
                    //    break;

                    //case "DateCurrentFonction":
                    //    error = Employe.CurrentFonctionNomination["Date"];
                    //    break;

                    //case "DateCurrentGrade":
                    //    error = Employe.CurrentGradeNomination["Date"];
                    //    break;

                    //case "DateEngagement":
                    //    error = Employe.GradeEngagement["Date"];
                    //    break;
                    default:
                        break;
                }

                return error;
            }
        }

        private void Cancel()
        {
            if (!ForAdding)
                Employe.CancelEdit();

            InitSave();
        }

        private bool CanCancel()
        {
            return true;
        }

        protected override void Close(object param)
        {
            if (!ForAdding)
                Employe.CancelEdit();

            this.CloseDialogWithResult(param as Window, DialogResult.No);
        }

        public ICommand NextCommand
        {
            get
            {
                if (_nextCommand == null)
                    _nextCommand = new AsyncCommand(p => Next(p), p => CanNext());

                return _nextCommand;
            }
        }

        private async Task Next(object p)
        {
            if (Step1)
            {
                Step1 = false;
                Step2 = true;
                SelectedItem = SegmentedOptions[1];
            }
            else if (Step2)
            {
                Step2 = false;
                Step3 = true;
                SelectedItem = SegmentedOptions[2];

                GenerateChildren();
            }
            else if (Step3)
            {
                Step3 = false;
                Step4 = true;
                Action = "Enregistrer";
                SelectedItem = SegmentedOptions[3];
            }
            else if (Step4)
            {
                await Save(p);
            }
        }

        private bool CanNext()
        {
            if (Step1)
                return Step1IsValid();
            else if (Step2)
                return Step2IsValid();
            else if (Step3)
                return Step3IsValid();

            return true;
        }

        public ICommand PreviousCommand
        {
            get
            {
                if (_previousCommand == null)
                    _previousCommand = new RelayCommand(p => Previous(p), p => true);

                return _previousCommand;
            }
        }

        private void Previous(object p)
        {
            if (Step2)
            {
                Step2 = false;
                Step1 = true;
                SelectedItem = SegmentedOptions[0];
            }
            else if (Step3)
            {
                Step3 = false;
                Step2 = true;
                SelectedItem = SegmentedOptions[1];
            }
            else if (Step4)
            {
                Step4 = false;
                Step3 = true;
                Action = "Suivant";
                SelectedItem = SegmentedOptions[2];
            }
        }

        void GenerateChildren()
        {
            //if (NombreEnfant == enfants.Count)
            //    return;

            //if (NombreEnfant > enfants.Count)
            //{
            //    var diff = NombreEnfant - enfants.Count;

            //    for (int i = 0; i < diff; i++)
            //    {
            //        var enfant = new EnfantEmploye()
            //        {
            //            Employe = Employe,
            //            DateNaissance = DateTime.Parse("01/01/2005")
            //        };

            //        Employe.Enfants.Add(enfant);
            //        enfants.Add(new EnfantAddAdapter(enfant));
            //    }

            //}
            //else
            //{
            //    for (int i = NombreEnfant; i < enfants.Count; i++)
            //    {
            //        Employe.Enfants.RemoveAt(i);
            //        enfants.RemoveAt(i);
            //        i--;
            //    }
            //}

            //RaisePropertyChanged("Employe");
        }

        private void InitSave()
        {
            Employe = new Model.Employe.Employe();

            Employe.DateNaissance = DateTime.Today.AddYears(-21);

            //Employe.EstRecense = Employe.EstMecanisePrime = Employe.EstMecaniseSalaire = true;
            ForAdding = true;
            Status = string.Empty;
            Action = "Suivant";

            DateEngagement = DateCurrentGrade = DateCurrentStatutaireGrade = DateTime.Today;
            EngagementNotInit = true;
        }

        public ICommand TakePhotoCommand
        {
            get
            {
                if (_takePhotoCommand == null)
                    _takePhotoCommand = new RelayCommand(p => TakePhoto(p), p => true);

                return _takePhotoCommand;
            }
        }

        private void TakePhoto(object p)
        {
            var photo = photoCapture.GetBytes();

            if (photo != null)
                Employe.Photo = photo;
        }

        public ICommand ScanFingerPrintCommand
        {
            get
            {
                if (_scanFingerPrintCommand == null)
                    _scanFingerPrintCommand = new RelayCommand(p => ScanFingerPrint(p), p => true);

                return _scanFingerPrintCommand;
            }
        }

        private void ScanFingerPrint(object p)
        {
            var vm = new FingerPrintViewModel(Employe);
            vm.Title = "Scannage d'empreintes";

            facade.ShowDialog(vm, p as Window);

            LoadEmpreintes();
        }

        public ICommand ChooseActeCommand
        {
            get
            {
                if (_chooseActeCommand == null)
                    _chooseActeCommand = new RelayCommand(p => ChooseActe(p), p => true);

                return _chooseActeCommand;
            }
        }

        string ACTE_PARAM = string.Empty;

        private void ChooseActe(object p)
        {
            ACTE_PARAM = p + "";

            Predicate<object> callback = null;

            if (ACTE_PARAM == "ENGAGEMENT")
                callback = EngagementActeFilter;
            else if (ACTE_PARAM == "GRADE")
                callback = GradeActeFilter;
            else if (ACTE_PARAM == "FONCTION")
                callback = FonctionActeFilter;
            else if (ACTE_PARAM == "AFFECTATION")
                callback = GradeStatutaireActeFilter;

            //var vm = new ActeChoiceListViewModel(this, facade, filePathProvider, callback);
            //vm.Title = "Choix d'un acte";

            //facade.ShowDialog(vm, null);

        }

        private bool EngagementActeFilter(object obj)
        {
            return true;
            //var acte = obj as ActeNomination;

            //return acte != null && acte.Type != ActeType.Notification && acte.Type != ActeType.Affectation;
        }

        private bool GradeActeFilter(object obj)
        {
            return true;
            //var acte = obj as ActeNomination;

            //return acte != null && acte.Type != ActeType.Affectation;
        }

        private bool GradeStatutaireActeFilter(object obj)
        {
            return true;
            //var acte = obj as ActeNomination;

            //return acte != null && (acte.Type == ActeType.Arrêté || acte.Type == ActeType.Décret || acte.Type == ActeType.Ordonnance);
        }

        private bool FonctionActeFilter(object obj)
        {
            return true;
            //var acte = obj as ActeNomination;

            //return acte != null && (acte.Type == ActeType.Notification || acte.Type == ActeType.Ordonnance);
        }

        public void Choose(object obj)
        {
            //var acte = obj as ActeNomination;

            //if (acte == null)
            //    return;

            //switch (ACTE_PARAM)
            //{
            //    case "ENGAGEMENT":
                    
            //        if (Employe.CurrentGradeNomination.Acte == null || Employe.CurrentGradeNomination.Acte.Equals(Employe.GradeEngagement.Acte))
            //            Employe.CurrentGradeNomination.Acte = acte;

            //        Employe.GradeEngagement.Acte = acte;

            //        EngagementNotInit = true;

            //        break;

            //    case "GRADE":
            //        Employe.CurrentGradeNomination.Acte = acte;
            //        break;

            //    case "AFFECTATION":
            //        Employe.CurrentAffectation.Acte = acte;
            //        break;

            //    case "FONCTION":
            //        Employe.CurrentFonctionNomination.Acte = acte;
            //        break;
            //    default:
            //        break;
            //}
        }

        public ICommand RemoveEmpreinteCommand
        {
            get
            {
                if (_removeEmpreinteCommand == null)
                    _removeEmpreinteCommand = new RelayCommand(p => RemoveEmpreinte(p));

                return _removeEmpreinteCommand;
            }
        }

        private void RemoveEmpreinte(object p)
        {
            var empreinte = p as EmployeEmpreinte;

            if (empreinte == null)
                return;

            empreintes.Remove(empreinte);
            Employe.Empreintes.Remove(empreinte);

            RaisePropertyChanged(() => NoEmpreinte);

        }

        private AsyncCommand _saveCommand;
        private RelayCommand _cancelCommand;
        private AsyncCommand _nextCommand;
        private RelayCommand _previousCommand;
        private RelayCommand _takePhotoCommand;
        private RelayCommand _scanFingerPrintCommand;
        private RelayCommand _chooseActeCommand;
        private RelayCommand _removeEmpreinteCommand;

        #endregion
    }
}
