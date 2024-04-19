using FingerPrintManagerApp.Dao.Presence;
using FingerPrintManagerApp.Dialog.Facade;
using FingerPrintManagerApp.Dialog.Service;
using FingerPrintManagerApp.Extension;
using FingerPrintManagerApp.Model.Admin.Util;
using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.ViewModel;
using FingerPrintManagerApp.ViewModel.Command;
using FingerPrintManagerApp.ViewModel.Contract;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using FingerPrintManagerApp.Modules.Employe.ViewModel;
using System.Windows;
using System.Globalization;

namespace FingerPrintManagerApp.Modules.Presence.ViewModel
{
    public class PresenceEmployeViewModel : PageViewModel, IChooserViewModel
    {
        private static object _lock = new object();
        private ObservableCollection<Model.Presence.Presence> presences;

        private bool editing = false;
        public ICollectionView PresencesView { get; private set; }

        IDialogFacade facade;
        IReportViewer reportViewer;

        public PresenceEmployeViewModel(IDialogFacade facade, IReportViewer reportViewer)
        {
            this.facade = facade;
            this.reportViewer = reportViewer;

            presences = new ObservableCollection<Model.Presence.Presence>();
            BindingOperations.EnableCollectionSynchronization(presences, _lock);

            PresencesView = (CollectionView)CollectionViewSource.GetDefaultView(presences);
            PresencesView.GroupDescriptions.Add(new PropertyGroupDescription("Employe.LastAffectation.Entite"));
            PresencesView.SortDescriptions.Add(new SortDescription("Employe.LastAffectation.Entite.EstPrincipale", ListSortDirection.Descending));
            PresencesView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
            //Filtering
            PresencesView.Filter = OnFilterPresence;

            FilterText = string.Empty;

            InitSave();
            MenuInit();
        }

        private bool OnFilterPresence(object obj)
        {
            var presence = obj as Model.Presence.Presence;

            if (presence == null)
                return false;

            var pattern = FilterText.Trim().ToLower().NoAccent();

           return presence.Employe.Name.ToLower().NoAccent().Contains(pattern)
                && presence.Employe.CurrentAffectation.Entite.Id == AppConfig.EntiteId
                ;

        }

        private string _action;
        private string _filterText;
        private int _count;
        private Model.Employe.Employe _employe;
        private Model.Presence.Presence _presence;
        private Model.Presence.Periode _currentPeriode;
        private bool _presenceLoading;
        private DateTime _selectedPresenceDate;

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
                    PresencesView.Refresh();
                    RaisePropertyChanged(() => FilterText);
                }
            }
        }
        public int PresenceCount
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
                    RaisePropertyChanged(() => PresenceCount);
                }
            }
        }

        public Model.Presence.Presence Presence
        {
            get
            {
                return this._presence;
            }
            set
            {
                if (_presence != value)
                {
                    _presence = value;
                    RaisePropertyChanged(() => Presence);
                }
            }
        }

        public bool PresenceLoading
        {
            get
            {
                return this._presenceLoading;
            }
            set
            {
                if (_presenceLoading != value)
                {
                    _presenceLoading = value;
                    RaisePropertyChanged(() => PresenceLoading);
                }
            }
        }

        public DateTime SelectedPresenceDate
        {
            get
            {
                return this._selectedPresenceDate;
            }
            set
            {
                if (_selectedPresenceDate != value)
                {
                    _selectedPresenceDate = value;
                    RaisePropertyChanged(() => SelectedPresenceDate);


                    if (SelectedPresenceDate.Date != DateTime.Now.Date)
                        LoadPresences();
                }
            }
        }

        #region Commands

        protected override async Task LoadData()
        {
            UpdateTimer.IsEnabled = false;

            await LoadPresenceChanges();

            LastDataUpdateTime = DateTime.Now;

            UpdateTimer.IsEnabled = true;
        }

        protected override async Task Load(object param)
        {
            if (!IsInit)
            {
                _currentPeriode = await new PeriodeDao().GetAsync(DateTime.Today);

                await LoadPresences();

                LastDataUpdateTime = DateTime.Now;
                IsInit = true;
            }

            UpdateTimer.IsEnabled = true;
        }

        async Task LoadPresences()
        {
            PresenceLoading = true;

            presences.Clear();

            PresenceCount = new PresenceDao().Count(SelectedPresenceDate);

            await Task.Run(() => new PresenceDao().GetAllAsync(presences, SelectedPresenceDate));

            PresenceLoading = false;

            PresencesView.Refresh();
        }

        async Task LoadPresenceChanges()
        {
            PresenceCount = new PresenceDao().Count(DateTime.Now);

            var list = await Task.Run(() => new PresenceDao().GetAllAsync(LastDataUpdateTime.AddSeconds(-5)));

            list.ForEach(d =>
            {
                var _d = presences.ToList().Find(e => e.Equals(d));

                if (_d != null) presences.Remove(_d);

                presences.Add(d);
            });

            PresencesView.Refresh();
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
                if (presences.Contains(Presence))
                {
                    Status = "Une présence avec la même description existe déjà.";
                    return;
                }

                PresenceLoading = true;

                //if (Presence.Employe.FonctionsInterim != null)
                //    Presence.Employe.FonctionsInterim.ForEach(f => f.DateFin = Presence.Date);

                Presence.Mode = Model.Presence.ModePointage.Utilisateur;
                Presence.Date = SelectedPresenceDate;

                Presence.HeureArrivee = SelectedPresenceDate;
                Presence.Periode = _currentPeriode;

                if (await new PresenceDao().AddAsync(Presence) > 0)
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Presence + "",
                            string.Format("Enregistrement de la présence de '{0}' (ID : {1}).", Presence.Employe.Name, Presence.Id)
                        );

                    presences.Add(Presence);
                    PresenceCount++;
                    PresencesView.Refresh();
                    Status = "Présence enregistrée avec succès !";
                    InitSave();
                }
                else
                    Status = "Enregistrement échoué. Vérifiez votre connexion au réseau puis réessayez !";

                PresenceLoading = false;
            }
            else
            {
                var clone = (Model.Presence.Presence)Presence.Clone();
                clone.Id = string.Empty;

                presences.Remove(Presence);

                if (!presences.Contains(clone))
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Presence + "",
                            string.Format("Modification de la presence de '{0}' (ID : {1}).", Presence.Employe.Name, Presence.Id)
                        );
                    Presence.HeureDepart = SelectedPresenceDate;

                    if (new PresenceDao().Update(Presence, toEdit) > 0)
                    {
                        Status = "Presence modifiée avec succès !";
                        presences.Add(Presence);
                        PresencesView.Refresh();
                        InitSave();
                    }
                }
                else
                {
                    Status = "Une presence avec la même description existe déjà !";
                    Presence.CancelEdit();
                    presences.Add(Presence);
                    PresencesView.Refresh();
                }
            }
        }

        private void InitSave()
        {
            Presence = new Model.Presence.Presence();

            _selectedPresenceDate = DateTime.Now;

            Action = "Enregistrer";
            editing = false;
            Title = "Nouvelle présence";
        }

        private void MenuInit()
        {
            Name = "Présence";

            OptionItem = new OptionItem()
            {
                Name = "Présence",
                ToolTip = "Présence",
                IconPathData = "M7.9915932,6.8739929C9.0265773,6.8739929,9.8655808,7.7139893,9.8655808,8.7489929L9.8655808,14.98999 9.8895677,14.925995C10.152562,14.289993 10.841545,13.835999 11.65052,13.835999 12.685504,13.835999 13.525484,14.579987 13.525484,15.497986L13.525484,16.14299 13.549502,16.084C13.812496,15.501999 14.501479,15.085999 15.310455,15.085999 16.345437,15.085999 17.184442,15.766998 17.184442,16.606995L17.184442,21.03299 17.401418,18.790985C17.487415,17.90799 18.272403,17.260986 19.155381,17.347 20.038362,17.431 20.685352,18.217987 20.599355,19.100998L19.766362,27.69899 19.766362,28.384995C19.766362,30.381989,18.147405,32,16.151441,32L9.724562,32C8.040603,32,6.6256216,30.847992,6.2236212,29.288986L6.1926468,29.153 1.2367164,20.863998C0.86172376,20.237 1.1977159,19.347 1.9877092,18.875 2.776696,18.403992 3.719671,18.528992 4.0946639,19.154999L6.1166289,22.537994 6.1166289,8.7489929C6.1166289,7.7139893,6.956609,6.8739929,7.9915932,6.8739929z M14.894477,2.7559967C15.209474,2.7559967 15.525446,2.8759918 15.766439,3.1169891 16.248454,3.598999 16.248454,4.3789978 15.766439,4.8609924L13.697479,6.9299927C13.215493,7.4109955 12.435509,7.4109955 11.953523,6.9299927 11.472545,6.447998 11.472545,5.6679993 11.953523,5.1859894L14.022484,3.1169891C14.263477,2.8759918,14.578473,2.7559967,14.894477,2.7559967z M1.2327186,2.7559967C1.5487224,2.7559967,1.864726,2.8759918,2.1057189,3.1169891L4.1746796,5.1859894C4.655658,5.6679993 4.655658,6.447998 4.1746796,6.9299927 3.6926942,7.4109955 2.9117031,7.4109955 2.4297178,6.9299927L0.36075655,4.8609924C-0.12025218,4.3789978 -0.12025218,3.598999 0.36075655,3.1169891 0.60174946,2.8759918 0.91772244,2.7559967 1.2327186,2.7559967z M8.0915969,0C8.7725842,0,9.3245757,0.55198669,9.3245757,1.2329865L9.3245757,4.1589966C9.3245757,4.8409882 8.7725842,5.3919983 8.0915969,5.3919983 7.4106105,5.3919983 6.858619,4.8409882 6.858619,4.1589966L6.858619,1.2329865C6.858619,0.55198669,7.4106105,0,8.0915969,0z"
            };
        }

        private bool CanSave()
        {
            return Presence.Error == string.Empty || Presence.Employe != null || SelectedPresenceDate != DateTime.Now;

               // || SelectedPresenceDate.Hour < 9 && SelectedPresenceDate.Hour > 17;
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
                Presence.CancelEdit();

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

        Model.Presence.Presence toEdit;

        private void Edit(object param)
        {
            if (param is Model.Presence.Presence)
            {
                Presence = (Model.Presence.Presence)param;
                toEdit = Presence.Clone() as Model.Presence.Presence;

                Presence.BeginEdit();
                editing = true;
                Action = "Modifier";
                Title = "Modification de la presence";

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

            if (param is Model.Presence.Presence)
            {
                var presence = (Model.Presence.Presence)param;

                var msg = string.Format("Êtes - vous sûr(e) de vouloir supprimer la presence de <<{0}>> ?", presence.Employe.Name);

                if (MyMsgBox.Show(msg, "Humager", MyMsgBoxButton.YesNoCancel, MyMsgBoxIcon.Warning) == DialogueResult.Yes)
                {
                    if (new PresenceDao().Delete(presence) > 0)
                    {
                        Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Presence + "",
                            string.Format("Suppression de presence de '{0}' (ID : {1}).", presence.Employe.Name, presence.Id)
                        );

                        presences.Remove(presence);
                        PresenceCount--;

                        Status = "Presence supprimée avec succès !";
                    }
                    else
                    {
                        Status = "Suppression de la presence échouée ! Vérifiez que vous êtes bien connecté au serveur de données, " +
                            "sinon cette presence est réliée à d'autres objets et ne peut donc pas être supprimée.";
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

                //employe.CurrentFonctionNomination = new Dao.Employe.EmployeFonctionDao().GetCurrent(employe);

                Presence.Employe = employe;

                return;
            }
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
            vm.Title = "Sélection de l'employé actifs";

            facade.ShowDialog(vm, param as Window);
        }

        private bool CustomEmployeFilter(object obj)
        {
            var employe = obj as Model.Employe.Employe;

            if (employe == null)
                return false;

            if (employe.Equals(Presence.Employe) )
                return false;

            return true;
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
            //var presence = values[0] as Presence;
            //var win = values[1] as Window;

            //if (presence.Details.Count == 0)
            //    presence.Details = await Task.Run(() => new PresenceEmployeDao().GetAllAsync(presence));

            //if (presence.Document.Pages.Count == 0)
            //    await LoadDoc(presence);

            //var vm = new PresenceDetailsViewModel(filePathProvider, presence);
            //vm.Title = "Détails du presence";

            //facade.ShowDialog(vm, win);
        }

        private AsyncCommand _saveCommand;
        private AsyncCommand _viewDetailsCommand;
        private RelayCommand _cancelCommand;
        private RelayCommand _editCommand;
        private RelayCommand _deleteCommand;
        private RelayCommand _selectEmployeCommand;
        private RelayCommand _markArrivalCommand;
        private RelayCommand _markDepartureCommand;

        #endregion

        #region REPORTS

        public IAsyncCommand PrintRegistreJournalierCommand
        {
            get
            {
                if (_printRegistreJournalierCommand == null)
                    _printRegistreJournalierCommand = new AsyncCommand(p => PrintRegistreJournalier(), p => CanPrint());

                return _printRegistreJournalierCommand;
            }
        }
        private bool CanPrint()
        {
            return !editing;
        }

        private async Task PrintRegistreJournalier()
        {

            PresenceLoading = true;

            var entite = AppConfig.CurrentUser.Entite;

            var report = new Reporting.Report.RegistrePresenceJournaliereReport();
            var list = await Task.Run(() => new PresenceDao().GetRegistrePresenceJournaliereReportAsync(entite, SelectedPresenceDate));
            list.TableName = "RegistreJournalier";

            var info = AppConfig.GetReportInfo();
            info.Rows[0]["report_header"] = AppConfig.ReportHeader;

            var dataset = new System.Data.DataSet();

            dataset.Tables.Add(info);
            if (list.Columns.Count > 0)
                dataset.Tables.Add(list);

            var title = string.Format("Registre journalier de présence du {0:dd/MM/yyyy}{1}", SelectedPresenceDate, entite.EstPrincipale ? "" : " - " + entite.ToString());

            report.SetDataSource(dataset);
            report.SetParameterValue("title", title);
            report.SetParameterValue("est_siege", entite.EstPrincipale);

            PresenceLoading = false;

            reportViewer.ShowViewer(report);


        }


        public IAsyncCommand PrintAgentMensCommand
        {
            get
            {
                if (_printAgentMensCommand == null)
                    _printAgentMensCommand = new AsyncCommand(p => PrintPresenceMensAgentListe(), p => CanPrint());

                return _printAgentMensCommand;
            }
        }
        private bool CanMensPrint()
        {
            return !editing && presences.Count > 0;
        }

        private async Task PrintPresenceMensAgentListe()
        {

            PresenceLoading = true;

            var entite = AppConfig.CurrentUser.Entite;
            var mois = SelectedPresenceDate.Month;
            var annnee = SelectedPresenceDate.Year;
            CultureInfo cultureInfo = new CultureInfo("fr-FR");

            var report = new Reporting.Report.RegistrePresenceMensuelReport();
            var list = await Task.Run(() => new PresenceDao().GetEmployePresenceReportAsync(entite, mois, annnee));
            list.TableName = "RegistreMensuel";

            var info = AppConfig.GetReportInfo();
            info.Rows[0]["report_header"] = AppConfig.ReportHeader;

            var dataset = new System.Data.DataSet();

            dataset.Tables.Add(info);
            if (list.Columns.Count > 0)
                dataset.Tables.Add(list);

            var title = string.Format("Registe Mensuel de mois de {0}  {1} {2}", (cultureInfo.DateTimeFormat.GetMonthName(mois)).ToUpper(), annnee, entite.EstPrincipale ? "" : " - " + entite.ToString());

            report.SetDataSource(dataset);
            report.SetParameterValue("title", title);
            report.SetParameterValue("est_siege", entite.EstPrincipale);

            PresenceLoading = false;

            reportViewer.ShowViewer(report);


        }

        #endregion

        private AsyncCommand _printRegistreJournalierCommand;
        private AsyncCommand _printAgentMensCommand;

    }
}
