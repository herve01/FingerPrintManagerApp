using FingerPrintManagerApp.Dao.Employe;
using FingerPrintManagerApp.Dialog.Facade;
using FingerPrintManagerApp.Dialog.Service;
using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Employe;
using FingerPrintManagerApp.ViewModel;
using FingerPrintManagerApp.ViewModel.Command;
using FingerPrintManagerApp.ViewModel.Contract;
using FingerPrintManagerApp.ViewModel.Extension;
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
    public class EmployeListViewModel : PageViewModel, ICallerViewModel
    {
       
        private static object _lock = new object();

        // Collections
        private ObservableCollection<Model.Employe.Employe> employes;
        private ObservableCollection<Entite> entites;
        
        // Collection Views
        public ICollectionView EmployesView { get; private set; }
        public ICollectionView EntitesView { get; private set; }
        public ICollectionView DirectionsView { get; private set; }
        
        //Facade
        private IDialogFacade facade;
        private IReportViewer reportViewer;
        private IFilePathProvider filePathProvider;
        private IPhotoCapture photoCapture;

        public EmployeListViewModel(IDialogFacade facade,IReportViewer reportViewer, IFilePathProvider filePathProvider, IPhotoCapture photoCapture)
        {
            this.facade = facade;
            this.reportViewer = reportViewer;
            this.filePathProvider = filePathProvider;
            this.photoCapture = photoCapture;

            employes = new ObservableCollection<Model.Employe.Employe>();
            BindingOperations.EnableCollectionSynchronization(employes, _lock);

            EmployesView = (CollectionView)CollectionViewSource.GetDefaultView(employes);
            EmployesView.GroupDescriptions.Add(new PropertyGroupDescription("CurrentAffectation.Entite"));
            EmployesView.SortDescriptions.Add(new SortDescription("EstAffecte", ListSortDirection.Ascending));
            EmployesView.SortDescriptions.Add(new SortDescription("CurrentAffectation.Entite.EstPrincipale", ListSortDirection.Descending));
            EmployesView.SortDescriptions.Add(new SortDescription("CurrentGrade.Niveau", ListSortDirection.Descending));
            EmployesView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            EmployesView.Filter = OnFilterEmploye;

            EmployeFilterText = string.Empty;

            entites = new ObservableCollection<Entite>();
            EntitesView = (CollectionView)CollectionViewSource.GetDefaultView(entites);

   

            MenuInit();
        }

        private void MenuInit()
        {
            Name = "Employé";
            OptionItem = new OptionItem()
            {
                Name = Name,
                ToolTip = "Liste des employés",
                IconPathData = "M15.642869,1.9992427C15.6335,1.9992428 15.624497,1.9994946 15.614976,1.9999981 13.508044,1.9999981 11.853017,2.8380101 10.690054,4.4930072 9.9460364,5.5509896 9.55297,6.883996 9.55297,8.3479843L9.55297,13.094989C9.55297,13.982012 10.384024,15.045976 11.117056,15.985001 11.375966,16.318007 11.628041,16.641006 11.846059,16.95198 12.11498,17.337966 12.325062,17.685988 12.489979,17.98799 13.270007,19.418958 13.233997,22.310006 12.082997,23.644967 11.608998,24.193976 10.992056,24.638007 10.247061,24.965949 9.5500403,25.272956 8.744011,25.443 7.8530202,25.474006 2.8730443,25.641974 2.1790753,26.21198 1.9970683,26.892949L1.9990215,29.983951 13.765979,29.998965 28.99802,29.983951 28.99802,26.888982C28.749973,26.060002,27.556982,25.587958,22.526957,25.454963L22.301983,25.435004C21.666973,25.379951 19.978987,25.235969 18.816999,23.659005 18.104963,22.693978 17.432968,19.798963 18.739973,17.936965L19.415999,16.980971C19.612043,16.70198 19.831038,16.414016 20.056013,16.116958 20.839947,15.081986 21.728985,13.907976 21.728985,12.909991L21.728985,8.487999C21.728985,6.8200006 21.207989,5.266993 20.259991,4.1149864 19.10301,2.712003 17.56102,1.9999981 15.672959,1.9999981 15.661973,1.9994946 15.652238,1.9992428 15.642869,1.9992427z M15.608018,0L15.679063,0C18.150984,0 20.270977,0.98300076 21.804058,2.8439918 23.046,4.3529925 23.728983,6.3579955 23.728983,8.487999L23.728983,12.909991C23.728983,14.579972 22.624979,16.039016 21.649029,17.324965 21.438946,17.602004 21.234966,17.870986 21.052959,18.130994L20.378033,19.086011C19.700056,20.049998 20.066023,21.982982 20.427959,22.471993 21.052959,23.320992 21.977032,23.399972 22.473002,23.441964 22.530008,23.446969 22.58201,23.451974 22.625956,23.456979 27.973974,23.599984 30.450047,24.086005 30.977022,26.557988L31.004,26.686955 30.997042,26.842961 30.998018,31.981996 14.047961,31.999999 13.765979,31.998963 13.405994,31.999999 13.405994,31.998963 0,31.980958 0.023071288,26.548955C0.53601025,24.141974 2.935056,23.638986 7.7840505,23.474984 8.4210129,23.453012 8.9780198,23.338998 9.4420081,23.134957 9.9119788,22.927987 10.291006,22.660958 10.567007,22.338998 11.087027,21.73701 11.184072,19.771008 10.733999,18.945996 10.59899,18.69801 10.426016,18.412976 10.206045,18.096997 10.008048,17.813979 9.776969,17.519973 9.5410071,17.215957 8.6080246,16.022964 7.5529719,14.669999 7.5529719,13.094989L7.5529719,8.3479843C7.5529719,6.469995 8.0720139,4.7399859 9.0540686,3.3430147 10.127066,1.8170148 12.134999,0 15.608018,0z"
            };
        }


        private bool OnFilterEmploye(object obj)
        {
            var employe = obj as Model.Employe.Employe;

            if (employe == null)
                return false;

            var motif = EmployeFilterText.Trim().ToLower().NoAccent();

            var allE = SelectedEntite.Zone == null;
            var allD = true; //SelectedDirection == null || SelectedDirection.Id == "-1";
            var nonA = true; // SelectedDirection != null && SelectedDirection.Id == "-2";

            return (allE || (SelectedEntite.EstPrincipale && string.IsNullOrWhiteSpace(employe.CurrentAffectation.Id)) || SelectedEntite.Equals(employe.CurrentAffectation?.Entite)) &&
                   (allD || (nonA && string.IsNullOrWhiteSpace(employe.CurrentAffectation.Id))) &&
                   (employe.Name.ToLower().NoAccent().Contains(motif) ||
                    employe.Matricule.ToString().ToLower().NoAccent().Contains(motif) ||
                    //employe.CurrentGrade.Id.ToLower().NoAccent().Contains(motif) ||
                    //employe.Etat.Intitule.ToLower().NoAccent().Contains(motif) ||
                    employe.Sexe.ToString().ToLower().NoAccent().Contains(motif) ||
                    (!string.IsNullOrWhiteSpace(employe.CurrentAffectation.Id) && employe.CurrentAffectation.Unite.ToString().ToLower().NoAccent().Contains(motif))
                   );
        }

        private int _count;
        private bool _employeLoading;
        private string _filterText;
        private bool _isSiege;
        private Entite _selectedEntite;

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
        public string EmployeFilterText
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
                    RaisePropertyChanged(() => EmployeFilterText);
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

                    IsSiege = SelectedEntite != null && (SelectedEntite.EstPrincipale || SelectedEntite.Zone == null);

                    //if (SelectedEntite != null && !SelectedEntite.EstPrincipale)
                    //{
                    //    if (directions.Count > 0)
                    //        SelectedDirection = directions[0];
                    //    else
                    //        SelectedDirection = null;
                    //}

                    EmployesView.Refresh();
                }
            }
        }

        public void AddObject(object obj)
        {
            var employe = obj as Model.Employe.Employe;

            if (employe == null)
                return;

            employes.Add(employe);
            EmployeCount++;
            EmployesView.Refresh();
        }

        public void InsertObject(object obj)
        {
            var employe = obj as Model.Employe.Employe;

            if (employe == null)
                return;

            employes.Add(employe);
            EmployesView.Refresh();
        }

        public void RemoveObject(object obj)
        {
            var employe = obj as Model.Employe.Employe;

            if (employe == null)
                return;

            employes.Remove(employe);
        }

        public void EndEditObject(object obj)
        {
            var employe = obj as Model.Employe.Employe;

            if (employe == null)
                return;

            EmployesView.MoveCurrentTo(employe);
        }

        public void DeleteObject(object obj)
        {
            var employe = obj as Model.Employe.Employe;

            if (employe == null)
                return;

            employes.Remove(employe);
            EmployeCount--;
        }

        public bool ContainsObject(object obj)
        {
            var employe = obj as Model.Employe.Employe;

            if (employe == null)
                return false;

            return employes.Contains(employe);
        }

        #region Commands
        protected override async Task LoadData()
        {
            UpdateTimer.IsEnabled = false;

            if (entites.Count == 0)
                await LoadEntites();
            else {
                if (AppConfig.CurrentUser?.Entite?.EstPrincipale == true)
                   await LoadEntiteChanges();
            }

                if (AppConfig.CurrentUser?.Entite?.EstPrincipale == true) {
                //if (directions.Count == 0)
                //    await LoadDirections();
                //else
                //    await LoadDirectionChanges();
            }

            if (employes.Count == 0)
                await LoadEmployes();
            else
                await LoadEmployeChanges();

            LastDataUpdateTime = DateTime.Now;

            UpdateTimer.IsEnabled = true;
        }

        protected override async Task Load(object param = null)
        {
            if (!IsInit)
            {
                IsSiege = AppConfig.CurrentUser.Entite.EstPrincipale;

                await LoadEntites();

                if (IsSiege)
                    await LoadDirections();

                await LoadEmployes();
                
                IsInit = true;
            }
            
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

        async Task LoadEmployeChanges()
        {
            EmployeCount = new EmployeDao().Count(AppConfig.CurrentUser.Entite);

            var list = await Task.Run(() => new EmployeDao().GetAllAsync(AppConfig.CurrentUser.Entite, LastDataUpdateTime.AddSeconds(-5)));

            list.ForEach(d => {
                var _d = employes.ToList().Find(e => e.Equals(d));

                if (_d != null) employes.Remove(_d);

                employes.Add(d);
            });

            if (list.Count > 0)
                EmployesView.Refresh();
        }

        private bool CanRefreshEmploye(object param = null)
        {
            return !EmployeLoading;
        }

        private async Task LoadEntites()
        {
            entites.Clear();

            if (!IsSiege)
                entites.Add(AppConfig.CurrentUser.Entite);
            else
            {
                entites.Add(new Entite() { Id = "-1"});
                var list = await Task.Run(() => new EntiteDao().GetAllAsync());
                list.ForEach(e => entites.Add(e));
            }

            EntitesView.MoveCurrentToFirst();

        }

        async Task LoadEntiteChanges()
        {
            var list = await Task.Run(() => new EntiteDao().GetAllAsync(LastDataUpdateTime.AddSeconds(-5)));

            list.ForEach(d => {
                var _d = entites.ToList().Find(e => e.Equals(d));

                if (_d != null) entites.Remove(_d);

                if (d.Equals(SelectedEntite))
                    SelectedEntite = d;

                entites.Add(d);
            });

            if (list.Count > 0)
                EntitesView.Refresh();
        }

        private async Task LoadDirections()
        {
            //directions.Clear();

            //directions.Add(new DirectionInterne() { Id = "-1", Denomination = "Toutes", Sigle = "Toutes" });
            //directions.Add(new DirectionInterne() { Id = "-2", Denomination = "", Sigle = "Non affectés" });

            //var list = await Task.Run(() => new DirectionDao().GetAllAsync(true));

            //list.ForEach(c => { directions.Add(c); });

            //DirectionsView.MoveCurrentToFirst();

        }

        public ICommand EditEmployeCommand
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
            var values = (object[])param;
            var employe = values[0] as Model.Employe.Employe;
            var win = values[1] as Window;

            //var editVM = new EditEmployeViewModel(facade, this, filePathProvider, photoCapture, employe);
            //facade.ShowDialog(editVM, win);

            var editVM = new FingerPrintViewModel(employe); 
            editVM.Title = "Scannage d'empreintes digitales";

            facade.ShowDialog(editVM, param as Window);

            if (employe.Empreintes.Count > 0)
            {
                //var feed = new EmployeEmpreinteDao().AddAsync(employe.Empreintes);

                //if (feed.Result > 0)
                //    Status = "Alright!";
            }
        }

        private bool CanEdit(object param)
        {
            return true;
        }

        public ICommand ChangePhotoCommand
        {
            get
            {
                if (_changePhotoCommand == null)
                    _changePhotoCommand = new AsyncCommand(p => ChangePhoto(p), p => true);

                return _changePhotoCommand;
            }
        }

        private async Task ChangePhoto(object param)
        {
            var employe = param as Model.Employe.Employe;

            if (employe == null)
                return;

            var photo = photoCapture.GetBytes();

            if (photo != null)
            {
                if (await new EmployeDao().SetPhoto(employe, photo) > 0) {
                    employe.Photo = photo;
                }
            }
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
            if (param is Model.Employe.Employe)
            {
                var employes = (Model.Employe.Employe)param;
                if (new EmployeDao().Delete(employes) > 0)
                {
                    this.employes.Remove(employes);
                    EmployeCount--;
                }
            }
        }

        private bool CanDelete(object param)
        {
            return true;
        }

        public ICommand CreateEmployeCommand
        {
            get
            {
                if (_createEmployeCommand == null)
                    _createEmployeCommand = new RelayCommand(p => CreateEmploye(p), p => true);

                return _createEmployeCommand;
            }
        }

        private void CreateEmploye(object param)
        {
            var editVM = new EditEmployeViewModel(facade, this, filePathProvider, photoCapture);
            editVM.Title = "Nouvel employé";

            facade.ShowDialog(editVM, param as Window);
        }

        public ICommand VisualizeEmployeCommand
        {
            get
            {
                if (_visualizeEmployeCommand == null)
                    _visualizeEmployeCommand = new RelayCommand(p => VisualizeEmploye(p), p => true);

                return _visualizeEmployeCommand;
            }
        }

        private void VisualizeEmploye(object param)
        {
            var values = (object[])param;
            var employe = values[0] as Model.Employe.Employe;
            var win = values[1] as Window;

            if (employe == null)
                return;

            var vm = new EmployeDetailsViewModel(reportViewer, employe);
            vm.Title = "Fiche individuelle de l'employé";

            facade.ShowDialog(vm, win);
        }

        private AsyncCommand _changePhotoCommand;
        private AsyncCommand _refreshEmployeCommand;
        private RelayCommand _editCommand;
        private RelayCommand _deleteCommand;
        private RelayCommand _createEmployeCommand;
        private RelayCommand _visualizeEmployeCommand;

        #endregion

        #region REPORTS
        public IAsyncCommand PrintCarteServiceRectoCommand
        {
            get
            {
                if (_printCarteServiceCommand == null)
                    _printCarteServiceCommand = new AsyncCommand(p => PrintCarteServiceRecto(p));

                return _printCarteServiceCommand;
            }
        }
        private async Task PrintCarteServiceRecto(object param)
        {
            var employe = param as Model.Employe.Employe;

            if (employe == null)
                return;

            EmployeLoading = true;

            var entite = AppConfig.CurrentUser.Entite;

            var report = new Reporting.Report.CarteServiceRecto();
            var data = await Task.Run(() => new EmployeDao().GetCarteServiceReportAsync(employe, AppConfig.SignatureDG));
            data.TableName = "CarteService";

            var info = AppConfig.GetReportInfo();
            info.Rows[0]["report_header"] = AppConfig.ServiceCardHeader;

            var dataset = new System.Data.DataSet();

            dataset.Tables.Add(info);

            if (data.Columns.Count > 0)
                dataset.Tables.Add(data);

            report.SetDataSource(dataset);
            report.SetParameterValue("annee", System.DateTime.Today.Year + "");
            report.SetParameterValue("zone", AppConfig.CurrentUser.Entite.Zone.Nom);

            EmployeLoading = false;

            reportViewer.ShowViewer(report);
        }

        public IAsyncCommand PrintDeclarativeListCommand
        {
            get
            {
                if (_printDeclarativeListCommand == null)
                    _printDeclarativeListCommand = new AsyncCommand(p => PrintDeclarativeList());

                return _printDeclarativeListCommand;
            }
        }

        private async Task PrintDeclarativeList()
        {
            EmployeLoading = true;

            var entite = AppConfig.CurrentUser.Entite;

            var report = new Reporting.Report.ListeDeclarativeReport();
            var list = await Task.Run(() => new EmployeDao().GetListeDeclarativeReportAsync(entite));
            list.TableName = "ListeDeclarative";
           
            var info = AppConfig.GetReportInfo();
            info.Rows[0]["report_header"] = AppConfig.ReportHeader;

            var dataset = new System.Data.DataSet();

            dataset.Tables.Add(info);
            if (list.Columns.Count > 0)
                dataset.Tables.Add(list);

            report.SetDataSource(dataset);
            report.SetParameterValue("title", string.Format("Liste déclarative {0}", entite.EstPrincipale ? "" : " - "  + entite.ToString()).Trim());
            report.SetParameterValue("est_siege", entite.EstPrincipale);

            EmployeLoading = false;

            reportViewer.ShowViewer(report);
        }

        public IAsyncCommand PrintFemmeListCommand
        {
            get
            {
                if (_printFemmeListCommand == null)
                    _printFemmeListCommand = new AsyncCommand(p => PrintFemmeList());

                return _printFemmeListCommand;
            }
        }

        private async Task PrintFemmeList()
        {
            EmployeLoading = true;

            var entite = AppConfig.CurrentUser.Entite;

            var report = new Reporting.Report.ListeFemmesReport();
            var list = await Task.Run(() => new EmployeDao().GetListeFemmeReportAsync(entite));
            list.TableName = "ListeFemmes";

            var info = AppConfig.GetReportInfo();
            info.Rows[0]["report_header"] = AppConfig.ReportHeader;

            var dataset = new System.Data.DataSet();

            dataset.Tables.Add(info);
            if (list.Columns.Count > 0)
                dataset.Tables.Add(list);

            report.SetDataSource(dataset);
            report.SetParameterValue("title", string.Format("Liste des personnels feminins {0}", entite.EstPrincipale ? "" : " - " + entite.ToString()).Trim());
            report.SetParameterValue("est_siege", entite.EstPrincipale);

            EmployeLoading = false;

            reportViewer.ShowViewer(report);
        }

        public IAsyncCommand PrintAgentsAffectesCommand
        {
            get
            {
                if (_printAgentAffecteCommand == null)
                    _printAgentAffecteCommand = new AsyncCommand(p => PrintAgentAffecte());

                return _printAgentAffecteCommand;
            }
        }
        private async Task PrintAgentAffecte()
        {
            EmployeLoading = true;

            var entite = AppConfig.CurrentUser.Entite;

            var report = new Reporting.Report.ListeAgentsAffectesReport();
            var list = await Task.Run(() => new EmployeDao().GetAgentsAffectesReportAsync(entite));
            list.TableName = "ListeAgentsAffectes";

            var info = AppConfig.GetReportInfo();
            info.Rows[0]["report_header"] = AppConfig.ReportLargeHeader;

            var dataset = new System.Data.DataSet();

            dataset.Tables.Add(info);
            if (list.Columns.Count > 0)
                dataset.Tables.Add(list);

            var title = string.Format("Liste des Agents Affectés {0}", entite.EstPrincipale ? "" : " - " + entite.ToString());

            report.SetDataSource(dataset);
            report.SetParameterValue("title", title);

            EmployeLoading = false;

            reportViewer.ShowViewer(report, title);
        }
        public IAsyncCommand PrintAgentDecedeCommand
        {
            get
            {
                if (_printAgentDecedeCommand == null)
                    _printAgentDecedeCommand = new AsyncCommand(p => PrintAgentDecede());

                return _printAgentDecedeCommand;
            }
        }
        private async Task PrintAgentDecede()
        {
            var entite = AppConfig.CurrentUser.Entite;

            var report = new Reporting.Report.ListeAgentDecedeReport();
            var list = await Task.Run(() => new EmployeDao().GetAgentDecedeReportAsync());
            list.TableName = "ListeAgentDecede";

            var info = AppConfig.GetReportInfo();
            info.Rows[0]["report_header"] = AppConfig.ReportHeader;

            var dataset = new System.Data.DataSet();

            dataset.Tables.Add(info);
            if (list.Columns.Count > 0)
                dataset.Tables.Add(list);

            var title = string.Format("Liste des agent decedés {0}", entite.EstPrincipale ? "" : " - " + entite.ToString());

            report.SetDataSource(dataset);
            report.SetParameterValue("title", title);
            report.SetParameterValue("est_siege", entite.EstPrincipale);
            reportViewer.ShowViewer(report);
        }
        public IAsyncCommand PrintAgentMecaniseCommand
        {
            get
            {
                if (_printAgentMecaniseCommand == null)
                    _printAgentMecaniseCommand = new AsyncCommand(p => PrintAgentMecanise());

                return _printAgentMecaniseCommand;
            }
        }
        private async Task PrintAgentMecanise()
        {
            var entite = AppConfig.CurrentUser.Entite;

            var report = new Reporting.Report.ListeAgentMecaniseReport();
            var list = await Task.Run(() => new EmployeDao().GetAgentMecaniseReportAsync(AppConfig.CurrentUser.Entite));
            list.TableName = "ListeAgentMecanise";

            var info = AppConfig.GetReportInfo();
            info.Rows[0]["report_header"] = AppConfig.ReportHeader;

            var dataset = new System.Data.DataSet();

            dataset.Tables.Add(info);
            if (list.Columns.Count > 0)
                dataset.Tables.Add(list);

            var title = string.Format("Liste des Agents Mecanisés {0}", entite.EstPrincipale ? "" : " - " + entite.ToString());

            report.SetDataSource(dataset);
            report.SetParameterValue("title", title);
            report.SetParameterValue("est_siege", entite.EstPrincipale);
            reportViewer.ShowViewer(report);
        }
        public IAsyncCommand PrintGradeEffectifCommand
        {
            get
            {
                if (_printGradeEffectifCommand == null)
                    _printGradeEffectifCommand = new AsyncCommand(p => PrintGradeEffectif());

                return _printGradeEffectifCommand;
            }
        }
        private async Task PrintGradeEffectif()
        {
            EmployeLoading = true;
            var entite = AppConfig.CurrentUser.Entite;

            var report = new Reporting.Report.ListeEffectifReport();
            var list = await Task.Run(() => new EmployeDao().GetGradeEffectifReportAsync(entite));
            list.TableName = "ListeEffectif";

            var info = AppConfig.GetReportInfo();
            info.Rows[0]["report_header"] = AppConfig.ReportHeader;

            var dataset = new System.Data.DataSet();

            dataset.Tables.Add(info);
            if (list.Columns.Count > 0)
                dataset.Tables.Add(list);

            var title = string.Format("Effectifs par Grade {0}", entite.EstPrincipale ? "" : " - " + entite.ToString());

            report.SetDataSource(dataset);
            report.SetParameterValue("title", title);
            report.SetParameterValue("est_siege", entite.EstPrincipale);

            EmployeLoading = false;

            reportViewer.ShowViewer(report);
        }

        public IAsyncCommand PrintAgentEnInstanceAffectationCommand
        {
            get
            {
                if (_printAgentEnInstanceAffectationCommand == null)
                    _printAgentEnInstanceAffectationCommand = new AsyncCommand(p => PrintAgentEnInstanceAffectation());

                return _printAgentEnInstanceAffectationCommand;
            }
        }
        private async Task PrintAgentEnInstanceAffectation()
        {
            EmployeLoading = true;
            var entite = AppConfig.CurrentUser.Entite;

            var report = new Reporting.Report.ListeAgentEnInstanceAffectationReport();
            var list = await Task.Run(() => new EmployeDao().GetAgentEnInstanceAffectationReportAsync(entite));
            list.TableName = "ListeAgentEnInstanceAffectation";

            var info = AppConfig.GetReportInfo();
            info.Rows[0]["report_header"] = AppConfig.ReportHeader;

            var dataset = new System.Data.DataSet();

            dataset.Tables.Add(info);
            if (list.Columns.Count > 0)
                dataset.Tables.Add(list);

            report.SetDataSource(dataset);
            report.SetParameterValue("title", string.Format("Agents en Instance d'Affectation {0}", entite.EstPrincipale ? "" : " - " + entite.ToString()).Trim());
            report.SetParameterValue("est_siege", entite.EstPrincipale);

            EmployeLoading = false;

            reportViewer.ShowViewer(report);
        }

        public IAsyncCommand PrintAgentEnDetachementCommand
        {
            get
            {
                if (_printAgentEnDetachementCommand == null)
                    _printAgentEnDetachementCommand = new AsyncCommand(p => PrintAgentEnDetachement());

                return _printAgentEnDetachementCommand;
            }
        }
        private async Task PrintAgentEnDetachement()
        {
            var entite = AppConfig.CurrentUser.Entite;

            var report = new Reporting.Report.ListeAgentEnDetachementReport();
            var list = await Task.Run(() => new EmployeDao().GetAgentEnDetachementReportAsync());
            list.TableName = "ListeAgentEnDetachement";


            var info = AppConfig.GetReportInfo();
            info.Rows[0]["report_header"] = AppConfig.ReportHeader;

            var dataset = new System.Data.DataSet();

            dataset.Tables.Add(info);
            if (list.Columns.Count > 0)
                dataset.Tables.Add(list);

            var title = string.Format("Liste des agent en detachement {0}", entite.EstPrincipale ? "" : " - " + entite.ToString());

            report.SetDataSource(dataset);
            report.SetParameterValue("title", title);
            report.SetParameterValue("est_siege", entite.EstPrincipale);
            reportViewer.ShowViewer(report);
        }

        public IAsyncCommand PrintAgentEnDisponibiliteCommand
        {
            get
            {
                if (_printAgentEnDisponibiliteCommand == null)
                    _printAgentEnDisponibiliteCommand = new AsyncCommand(p => PrintAgentEnDisponibilite());

                return _printAgentEnDisponibiliteCommand;
            }
        }
        private async Task PrintAgentEnDisponibilite()
        {
            var entite = AppConfig.CurrentUser.Entite;

            var report = new Reporting.Report.ListeAgentEnDisponibiliteReport();
            var list = await Task.Run(() => new EmployeDao().GetAgentEnDisponibiliteReportAsync());
            list.TableName = "ListeAgentEnDisponibilite";

            var info = AppConfig.GetReportInfo();
            info.Rows[0]["report_header"] = AppConfig.ReportHeader;

            var dataset = new System.Data.DataSet();

            dataset.Tables.Add(info);
            if (list.Columns.Count > 0)
                dataset.Tables.Add(list);

            var title = string.Format("Liste des agent en disponibilité {0}", entite.EstPrincipale ? "" : " - " + entite.ToString());

            report.SetDataSource(dataset);
            report.SetParameterValue("title", title);
            report.SetParameterValue("est_siege", entite.EstPrincipale);
            reportViewer.ShowViewer(report);
        }

        public IAsyncCommand PrintPresenceJournaliereCommand
        {
            get
            {
                if (_printPresenceJournaliereCommand == null)
                    _printPresenceJournaliereCommand = new AsyncCommand(p => PrintPresenceJournaliere());

                return _printPresenceJournaliereCommand;
            }
        }
        private async Task PrintPresenceJournaliere()
        {
            var dataset = new System.Data.DataSet();
            //var report = new Reporting.Report.EffectifReport();
            //var list = await Task.Run(() => new EmployeDao().GetPresenceJournaliereReportAsync());

            //var info = AppConfig.GetReportInfo();
            //info.Rows[0]["report_header"] = AppConfig.ReportHeader;


            //dataset.Tables.Add(info);
            //if (list.Columns.Count > 0)
            //    dataset.Tables.Add(list);

            //report.SetDataSource(dataset);

            ////  reportView.PrintSilently(report);
            //reportViewer.ShowViewer(report);
        }
        #endregion

        private AsyncCommand _printCarteServiceCommand;
        private AsyncCommand _printDeclarativeListCommand;
        private AsyncCommand _printFemmeListCommand;
        private AsyncCommand _printAgentAffecteCommand;
        private AsyncCommand _printAgentMecaniseCommand;
        private AsyncCommand _printAgentDecedeCommand;
        private AsyncCommand _printGradeEffectifCommand;
        private AsyncCommand _printAgentEnInstanceAffectationCommand;
        private AsyncCommand _printAgentEnDetachementCommand;
        private AsyncCommand _printAgentEnDisponibiliteCommand;
        private AsyncCommand _printPresenceJournaliereCommand;
    }
}
