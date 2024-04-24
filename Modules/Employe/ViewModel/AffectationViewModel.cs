using FingerPrintManagerApp.Dialog.Facade;
using FingerPrintManagerApp.Dialog.Service;
using FingerPrintManagerApp.Extension;
using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Employe;
using FingerPrintManagerApp.Model.Helper;
using FingerPrintManagerApp.ViewModel;
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
using FingerPrintManagerApp.Dao.Employe;
using DbUtil = FingerPrintManagerApp.Model.Admin.Util.DbUtil;
using System.Collections.Generic;
using FingerPrintManagerApp.Modules.Employe.ViewModel;

namespace FingerPrintManagerApp.Modules.Employe.ViewModel
{
    public class AffectationViewModel : PageViewModel, IChooserViewModel
    {
        private static object _lock = new object();
        private ObservableCollection<Affectation> affectations;
        private ObservableCollection<Direction> directions;
        private ObservableCollection<Departement> departements;
        private ObservableCollection<Entite> entites;
        private Array uniteTypes { get; set; }

        private bool editing = false;
        public ICollectionView AffectationsView { get; private set; }
        public ICollectionView UniteTypesView { get; private set; }
        public ICollectionView DirectionsView { get; private set; }
        public ICollectionView DepartementsView { get; private set; }
        public ICollectionView BureauxView { get; private set; }
        public ICollectionView EntitesView { get; private set; }

        IDialogFacade facade;
        IFilePathProvider filePathProvider;
        IReportViewer reportViewer;

        public AffectationViewModel(IDialogFacade facade, IReportViewer reportViewer, IFilePathProvider filePathProvider)
        {
            this.facade = facade;
            this.reportViewer = reportViewer;
            this.filePathProvider = filePathProvider;

            affectations = new ObservableCollection<Affectation>();
            BindingOperations.EnableCollectionSynchronization(affectations, _lock);

            AffectationsView = (CollectionView)CollectionViewSource.GetDefaultView(affectations);
            AffectationsView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
            AffectationsView.SortDescriptions.Add(new SortDescription("Employe.Name", ListSortDirection.Ascending));
            //Filtering
            AffectationsView.Filter = OnFilterAffectation;

            uniteTypes = Enum.GetValues(typeof(UniteType));
            UniteTypesView = (CollectionView)CollectionViewSource.GetDefaultView(uniteTypes);
            UniteTypesView.Filter = OnTypeFilter;

            entites = new ObservableCollection<Entite>();
            EntitesView = (CollectionView)CollectionViewSource.GetDefaultView(entites);
            EntitesView.Filter = OnEntityFilter;

            directions = new ObservableCollection<Direction>();
            DirectionsView = (CollectionView)CollectionViewSource.GetDefaultView(directions);
            DirectionsView.SortDescriptions.Add(new SortDescription("EstGenerale", ListSortDirection.Descending));
            DirectionsView.SortDescriptions.Add(new SortDescription("Denomination", ListSortDirection.Ascending));

            departements = new ObservableCollection<Departement>();
            DepartementsView = (CollectionView)CollectionViewSource.GetDefaultView(departements);
            DepartementsView.SortDescriptions.Add(new SortDescription("Denomination", ListSortDirection.Ascending));


            FilterText = string.Empty;

            //busyFonctions = new List<EmployeFonction>();

            InitSave();
            MenuInit();
        }

        private bool OnEntityFilter(object obj)
        {
            var entity = obj as Entite;

            if (entity == null)
                return false;

            if (Affectation.Employe == null)
                return true;

            if (Affectation.Employe.CurrentGrade.Grade.Niveau >= Grade.DIR_NIVEAU)
                return entity.EstPrincipale;

            return true;
        }

        private bool OnTypeFilter(object obj)
        {
            var type = (UniteType)obj;

            var niveau = Affectation?.Employe?.CurrentGrade?.Grade.Niveau;

            if (niveau >= Grade.DIR_NIVEAU)
            {
                if (type != UniteType.Direction)
                    return false;
            }
            //else if (niveau >= Grade.CD_NIVEAU)
            //{
            //    if (type == UniteType.Bureau)
            //        return false;
            //}

            return IsSiege || type != UniteType.Direction;
        }

        private bool OnFilterAffectation(object obj)
        {
            var affectation = obj as Affectation;

            if (affectation == null)
                return false;

            var pattern = FilterText.Trim().ToLower().NoAccent();

            return affectation.Employe.Name.ToLower().NoAccent().Contains(pattern);

        }

        private string _action;
        private string _filterText;
        private int _count;
        private Affectation _affectation;
        private bool _affectationLoading;
        private bool _isSiege;
        private Direction _selectedDirection;
        private Departement _selectedDepartement;
        private Entite _selectedEntite;
        private UniteType _selectedUnite;
        private bool _showDepartement;
        private bool _showBureau;

        //private List<EmployeFonction> busyFonctions;

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
                    AffectationsView.Refresh();
                    RaisePropertyChanged(() => FilterText);
                }
            }
        }
        public int AffectationCount
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
                    RaisePropertyChanged(() => AffectationCount);
                }
            }
        }
        public Affectation Affectation
        {
            get
            {
                return this._affectation;
            }
            set
            {
                if (_affectation != value)
                {
                    _affectation = value;
                    RaisePropertyChanged(() => Affectation);
                }
            }
        }
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
                    {
                        Affectation.Unite = SelectedDirection;
                       
                    }

                    LoadDepartements();
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
                    {
                        Affectation.Unite = SelectedDepartement;
                        //LoadFonctions();
                    }

                    //LoadBureaux();
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
                            LoadDepartements();
                        else
                            LoadDepartementProvinciale();
                    }

                    Affectation.Entite = SelectedEntite;
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
                    Affectation.Niveau = SelectedUnite;

                    //Affectation.Unite = SelectedUnite == UniteType.Direction ? (object)SelectedDirection : SelectedUnite == UniteType.Departement ? (object)SelectedDepartement : (object)SelectedBureau;

                    ShowDepartement = ShowBureau = false;

                    if (SelectedUnite == UniteType.Departement)
                        ShowDepartement = true;
                    //else if (SelectedUnite == UniteType.Direction)
                    //    ShowBureau = ShowDepartement = true;
                }
            }
        }
        public bool AffectationLoading
        {
            get
            {
                return this._affectationLoading;
            }
            set
            {
                if (_affectationLoading != value)
                {
                    _affectationLoading = value;
                    RaisePropertyChanged(() => AffectationLoading);
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

        #region Commands

        protected override async Task LoadData()
        {
            UpdateTimer.IsEnabled = false;

            await LoadAffectationChanges();

            LastDataUpdateTime = DateTime.Now;

            UpdateTimer.IsEnabled = true;
        }

        protected override async Task Load(object param)
        {
            IsSiege = AppConfig.CurrentUser.Entite.EstPrincipale;

            if (!IsInit)
            {
                if (IsSiege)
                    await LoadDirections();
                else
                    await LoadDepartementProvinciale();

                await LoadEntites();
                await LoadAffectations();

                LastDataUpdateTime = DateTime.Now;
                IsInit = true;
            }

            UpdateTimer.IsEnabled = true;
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

        async Task LoadAffectations()
        {
            AffectationLoading = true;

            affectations.Clear();

            AffectationCount = new AffectationDao().Count(AppConfig.CurrentUser.Entite);

            await Task.Run(() => new AffectationDao().GetAllAsync(AppConfig.CurrentUser.Entite, affectations));

            AffectationLoading = false;

            AffectationsView.Refresh();
        }

        private async Task LoadDirections()
        {
            directions.Clear();

            var list = await Task.Run(() => new DirectionDao().GetAllAsync(true));

            list.ForEach(c => { directions.Add(c); });

            DirectionsView.MoveCurrentToFirst();
        }

        private void LoadDepartements()
        {
            //departements.Clear();

            //if (SelectedDirection == null)
            //    return;

            //SelectedDirection.Departements.ForEach(d => departements.Add(d));

            //DepartementsView.MoveCurrentToFirst();
        }

        async Task LoadDepartementProvinciale()
        {
            //departements.Clear();

            //var departement = SelectedEntite.Departement;

            //if (departement.Bureaux.Count == 0)
            //    departement.Bureaux = await Task.Run(() => new BureauDao().GetAllAsync(departement, true));

            //departements.Add(departement);
            //DepartementsView.MoveCurrentToFirst();
        }

        async Task LoadAffectationChanges()
        {
            //AffectationCount = new AffectationDao().Count();

            //var list = await Task.Run(() => new AffectationDao().GetAllAsync(LastDataUpdateTime.AddSeconds(-5)));

            //list.ForEach(d => {
            //    var _d = affectations.ToList().Find(e => e.Equals(d));

            //    if (_d != null) affectations.Remove(_d);

            //    affectations.Add(d);
            //});

            //AffectationsView.Refresh();
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

            if (!editing)
            {
                Affectation.AncienneEntite = Affectation.Employe.LastAffectation?.Entite;

                if (affectations.Contains(Affectation))
                {
                    Status = "Une affectation avec la même description existe déjà.";
                    return;
                }

                AffectationLoading = true;

                //if (Affectation.Employe.FonctionsInterim != null)
                //    Affectation.Employe.FonctionsInterim.ForEach(f => f.DateFin = Affectation.Date);

                if (await new AffectationDao().AddAsync(Affectation) > 0)
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Affectation + "",
                            string.Format("Enregistrement de l'affectation de '{0}' (ID : {1}).", Affectation.Employe.Name, Affectation.Id)
                        );

                    affectations.Add(Affectation);
                    AffectationCount++;
                    AffectationsView.Refresh();
                    Status = "Affectation enregistrée avec succès !";
                    InitSave();
                }
                else
                    Status = "Enregistrement échoué. Vérifiez votre connexion au réseau puis réessayez !";

                AffectationLoading = false;
            }
            else
            {
                var clone = (Affectation)Affectation.Clone();
                clone.Id = string.Empty;

                affectations.Remove(Affectation);

                if (!affectations.Contains(clone))
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Affectation + "",
                            string.Format("Modification de l'affectation de '{0}' (ID : {1}).", Affectation.Employe.Name, Affectation.Id)
                        );

                    if (new AffectationDao().Update(Affectation, toEdit) > 0)
                    {
                        Status = "Affectation modifiée avec succès !";
                        affectations.Add(Affectation);
                        AffectationsView.Refresh();
                        InitSave();
                    }
                }
                else
                {
                    Status = "Une affectation avec la même description existe déjà !";
                    Affectation.CancelEdit();
                    affectations.Add(Affectation);
                    AffectationsView.Refresh();
                }

            }
        }

        private void InitSave()
        {
            Affectation = new Affectation();
            Affectation.Date = DateTime.Today;

            Action = "Enregistrer";
            editing = false;
            Title = "Nouvelle affectation";

        }

        private void MenuInit()
        {
            Name = "Affectation";

            OptionItem = new OptionItem()
            {
                Name = "Affectation",
                ToolTip = "Affectation",
                IconPathData = "M9.251038,12.712006C9.6920418,12.712006,10.134045,12.881012,10.472044,13.218994L10.961045,13.705994 9.2520374,15.416016 3.9420187,20.723999 11.25905,28.039001 11.275049,28.057007 16.585067,22.746002 16.568069,22.731018 18.276077,21.022003 18.781076,21.527008C19.45708,22.201996,19.455081,23.294006,18.782075,23.968018L11.256044,31.494019C10.582045,32.167999,9.4910368,32.170013,8.81504,31.492004L0.50600031,23.184998C-0.16900183,22.509003,-0.16799856,21.416992,0.50500086,20.744019L8.0300325,13.21701C8.3680319,12.880005,8.8090347,12.712006,9.251038,12.712006z M18.465072,10.32901L21.667089,13.532013 13.379054,21.821991 10.176045,18.618011z M21.963088,0C22.405092,0,22.847095,0.16799927,23.184093,0.50601208L31.494125,8.8150024C32.16913,9.4909973,32.16813,10.583008,31.494125,11.257996L23.968101,18.78299C23.294095,19.458008,22.203094,19.458008,21.52709,18.78299L21.022083,18.278015 22.730089,16.569 22.747095,16.585999 28.055116,11.276001 28.039117,11.26001 20.724084,3.9450073 15.415065,9.2539978 13.707057,10.963013 13.218057,10.472992C12.543051,9.7980042,12.544051,8.7059937,13.218057,8.0320129L20.743081,0.50601208C21.080089,0.16900635,21.521085,0,21.963088,0z"

            };
        }

        private bool CanSave()
        {
            //if (Affectation.Error != string.Empty)
            //    return false;

            //if (busyFonctions.Count == 0)
            //    return true;

            //var busyCount = busyFonctions.Count(f => f.Type == FonctionEmployeType.Officiel);
            //var uniteFonctCount = Affectation.Niveau == UniteType.Bureau ? ((Bureau)Affectation.Unite).NombreChefs : 1;

            //return busyCount < uniteFonctCount;

            return true;
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
                Affectation.CancelEdit();

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

        Affectation toEdit;

        private void Edit(object param)
        {
            if (param is Affectation)
            {
                Affectation = (Affectation)param;
                toEdit = Affectation.Clone() as Affectation;

                Affectation.BeginEdit();
                editing = true;
                Action = "Modifier";
                Title = "Modification de l'affectation";

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

            if (param is Affectation)
            {
                var affectation = (Affectation)param;

                var msg = string.Format("Êtes - vous sûr(e) de vouloir supprimer la affectation de <<{0}>> ?", affectation.Employe.Name);

                if (MyMsgBox.Show(msg, "FingerPrintManagerApp", MyMsgBoxButton.YesNoCancel, MyMsgBoxIcon.Warning) == DialogueResult.Yes)
                {
                    if (new AffectationDao().Delete(affectation) > 0)
                    {
                        Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Affectation + "",
                            string.Format("Suppression de affectation de '{0}' (ID : {1}).", affectation.Employe.Name, affectation.Id)
                        );

                        affectations.Remove(affectation);
                        AffectationCount--;

                        Status = "Affectation supprimée avec succès !";
                    }
                    else
                    {
                        Status = "Suppression de l'affectation échouée ! Vérifiez que vous êtes bien connecté au serveur de données, " +
                            "sinon cette affectation est réliée à d'autres objets et ne peut donc pas être supprimée.";
                    }
                }

            }
        }

        private bool CanDelete(object param)
        {
            return !editing;
        }

        public void Choose(object obj)
        {
            if (obj is Model.Employe.Employe)
            {
                var employe = (Model.Employe.Employe)obj;

                employe.CurrentFonctionNomination = new EmployeFonctionDao().GetCurrent(employe);

                Affectation.Employe = employe;

                //LoadFonctions();

                EntitesView.Refresh();
                UniteTypesView.Refresh();

                return;
            }

        }

        public ICommand ImportDocumentCommand
        {
            get
            {
                if (_importDocumentCommand == null)
                    _importDocumentCommand = new RelayCommand(p => ImportDocument(p));

                return _importDocumentCommand;
            }
        }

        private void ImportDocument(object param)
        {
            //var vm = new ActeChoiceListViewModel(this, facade, filePathProvider, ActeFilter);
            //vm.Title = "Lettre d'affectation";

            //facade.ShowDialog(vm, param as Window);
        }

        private bool ActeFilter(object obj)
        {
            //var acte = obj as ActeNomination;

            //return acte != null && acte.Type == ActeType.Affectation;

            return true;
        }

        public ICommand SelectEmployeCommand
        {
            get
            {
                if (_selectEmployeCommand == null)
                    _selectEmployeCommand = new RelayCommand(p => SelectEmploye(p));

                return _selectEmployeCommand;
            }
        }

        private void SelectEmploye(object param)
        {
            var vm = new EmployeChoiceListViewModel(this, CustomEmployeFilter);
            vm.Title = "Sélection de l'employé affecté";

            facade.ShowDialog(vm, param as Window);
        }

        private bool CustomEmployeFilter(object obj)
        {
            var employe = obj as Model.Employe.Employe;

            if (employe == null)
                return false;

            if (employe.Equals(Affectation.Employe))
                return false;

            return true;
        }

        public ICommand RemoveDocumentCommand
        {
            get
            {
                if (_removeDocumentCommand == null)
                    _removeDocumentCommand = new RelayCommand(p => RemoveDocument(), p => true);

                return _removeDocumentCommand;
            }
        }

        private void RemoveDocument()
        {
            //Affectation.Acte = null;
        }

        async Task LoadDoc(Affectation affectation)
        {
            AffectationLoading = true;

            //if (affectation.Acte.File == null)
            //    affectation.Acte.File = await Task.Run(() => new ActeNominationDao().GetFileAsync(affectation.Acte));

            //if (affectation.Acte.File != null)
            //    affectation.Acte.Document = await Task.Run(() => ImageUtil.LoadDocument(affectation.Acte.File));

            //AffectationLoading = false;
        }

        public ICommand ViewDetailsCommand
        {
            get
            {
                if (_viewDetailsCommand == null)
                    _viewDetailsCommand = new AsyncCommand(p => ViewDetails(p));

                return _viewDetailsCommand;
            }
        }

        private async Task ViewDetails(object param)
        {
            //var values = (object[])param;
            //var affectation = values[0] as Affectation;
            //var win = values[1] as Window;

            //if (affectation.Details.Count == 0)
            //    affectation.Details = await Task.Run(() => new AffectationEmployeDao().GetAllAsync(affectation));

            //if (affectation.Document.Pages.Count == 0)
            //    await LoadDoc(affectation);

            //var vm = new AffectationDetailsViewModel(filePathProvider, affectation);
            //vm.Title = "Détails du affectation";

            //facade.ShowDialog(vm, win);
        }

        private AsyncCommand _saveCommand;
        private AsyncCommand _viewDetailsCommand;
        private RelayCommand _cancelCommand;
        private RelayCommand _editCommand;
        private RelayCommand _deleteCommand;
        private RelayCommand _importDocumentCommand;
        private RelayCommand _selectEmployeCommand;
        private RelayCommand _removeDocumentCommand;

        #endregion

        #region REPORTS

        public IAsyncCommand PrintAgentsAffectesCommand
        {
            get
            {
                if (_printAgentsAffectesCommand == null)
                    _printAgentsAffectesCommand = new AsyncCommand(p => PrintAgentAffecte());

                return _printAgentsAffectesCommand;
            }
        }

        public IAsyncCommand PrintAgentsNonAffectesCommand
        {
            get
            {
                if (_printAgentsNonAffectesCommand == null)
                    _printAgentsNonAffectesCommand = new AsyncCommand(p => PrintAgentNonAffecte());

                return _printAgentsNonAffectesCommand;
            }
        }

        private async Task PrintAgentAffecte()
        {
            Status = string.Empty;

            AffectationLoading = true;

            var entite = AppConfig.CurrentUser.Entite;

            var report = new Employe.Reporting.Report.ListeAgentsAffectesReport();
            var list = await Task.Run(() => new EmployeDao().GetAgentsAffectesReportAsync(entite));
            list.TableName = "ListeAgentsAffectes";

            if (list.Rows.Count == 0)
            {
                Status = "Aucun agent affecté trouvé !";
                AffectationLoading = false;

                return;
            }

            var info = AppConfig.GetReportInfo();
            info.Rows[0]["report_header"] = AppConfig.ReportLargeHeader;

            var dataset = new System.Data.DataSet();

            dataset.Tables.Add(info);
            if (list.Columns.Count > 0)
                dataset.Tables.Add(list);

            var title = string.Format("Agents Affectés{0}", entite.EstPrincipale ? "" : " - " + entite.ToString());

            report.SetDataSource(dataset);
            report.SetParameterValue("title", title);

            AffectationLoading = false;

            reportViewer.ShowViewer(report, title);
        }
        private async Task PrintAgentNonAffecte()
        {
            Status = string.Empty;

            AffectationLoading = true;

            var entite = AppConfig.CurrentUser.Entite;

            var report = new Employe.Reporting.Report.ListeAgentEnInstanceAffectationReport();
            var list = await Task.Run(() => new EmployeDao().GetAgentEnInstanceAffectationReportAsync(entite));
            list.TableName = "ListeAgentEnInstanceAffectation";

            if (list.Rows.Count == 0)
            {
                Status = "Aucun agent non affecté trouvé !";
                AffectationLoading = false;

                return;
            }

            var info = AppConfig.GetReportInfo();
            info.Rows[0]["report_header"] = AppConfig.ReportLargeHeader;

            var dataset = new System.Data.DataSet();

            dataset.Tables.Add(info);
            if (list.Columns.Count > 0)
                dataset.Tables.Add(list);

            var title = string.Format("Agents en instance d'affectation{0}", entite.EstPrincipale ? "" : " - " + entite.ToString());

            report.SetDataSource(dataset);
            report.SetParameterValue("title", title);

            AffectationLoading = false;

            reportViewer.ShowViewer(report, title);
        }
        #endregion

        private AsyncCommand _printAgentsAffectesCommand;
        private AsyncCommand _printAgentsNonAffectesCommand;
    }
}
