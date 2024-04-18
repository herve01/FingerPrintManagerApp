using FingerPrintManagerApp.Dao.Employe;
using FingerPrintManagerApp.Dialog.Service;
using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Employe;
using FingerPrintManagerApp.ViewModel.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FingerPrintManagerApp.Modules.Employe.ViewModel
{
    public class EmployeDetailsViewModel : DialogViewModelBase
    {
        ObservableCollection<EnfantEmploye> enfants;
        // Collection Views
        public ICollectionView EnfantsView { get; private set; }

        IReportViewer reportViewer;

        public EmployeDetailsViewModel(IReportViewer reportView, Model.Employe.Employe employe)
        {
            this.reportViewer = reportView;
            Employe = employe;

            enfants = new ObservableCollection<EnfantEmploye>();
            EnfantsView = (CollectionView)CollectionViewSource.GetDefaultView(enfants);
            EnfantsView.SortDescriptions.Add(new SortDescription("DateNaissance", ListSortDirection.Descending));
        }

        Model.Employe.Employe _employe;
        bool _employeLoading;

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

        #region Commands
        protected override async Task Load(object param)
        {
            EmployeLoading = true;
            await LoadMissedInfo();
            await LoadEnfants();
            EmployeLoading = false;
        }

        async Task LoadEnfants()
        {
            if (Employe.Enfants.Count == 0)
                Employe.Enfants = await Task.Run(() => new EnfantEmployeDao().GetAllAsync(Employe));

            enfants.Clear();

            Employe.Enfants.ForEach(e => enfants.Add(e));

            EnfantsView.Refresh();
        }

        async Task LoadMissedInfo()
        {
            if (Employe.CurrentFonctionNomination.Fonction == null)
                Employe.CurrentFonctionNomination = new EmployeFonctionDao().GetCurrent(Employe);

            if (Employe.CurrentHighEtude.Niveau == null)
                Employe.CurrentHighEtude = new EmployeEtudeDao().Get(Employe);
        }

        protected override void Close(object param)
        {
            this.CloseDialogWithResult(param as Window, DialogResult.No);
        }

        public IAsyncCommand PrintServiceCardRectoCommand
        {
            get
            {
                if (_printCarteServiceCommand == null)
                    _printCarteServiceCommand = new AsyncCommand(p => PrintServiceCardRecto(p), p => Employe.Photo != null);

                return _printCarteServiceCommand;
            }
        }

        private async Task PrintServiceCardRecto(object param)
        {
            EmployeLoading = true;

            var report = new Reporting.Report.CarteServiceRecto();
            var data = await Task.Run(() => new EmployeDao().GetCarteServiceReportAsync(Employe, AppConfig.SignatureDG));
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

        public IAsyncCommand PrintFicheIndividuelleCommand
        {
            get
            {
                if (_printFicheIndividuelleCommand == null)
                    _printFicheIndividuelleCommand = new AsyncCommand(p => PrintFicheIndividuelle(p));

                return _printFicheIndividuelleCommand;
            }
        }

        private async Task PrintFicheIndividuelle(object param)
        {
            EmployeLoading = true;

            var report = new Reporting.Report.FicheIndividuelleReport();
            var avatar = Employe.Sexe == Sex.Homme ? AppConfig.ManAvatar : AppConfig.WomanAvatar;
            var fiche = await Task.Run(() => new EmployeDao().GetFicheIndividuelleReportAsync(Employe, avatar));
            fiche.TableName = "FicheIndividuelle";

            var info = AppConfig.GetReportInfo();
            info.Rows[0]["report_header"] = AppConfig.ReportHeader;

            var enfants = await Task.Run(() => new EnfantEmployeDao().GetEmployeEnfantsReportAsync(Employe));

            if (enfants != null)
                enfants.TableName = "Enfant";

            var dataset = new System.Data.DataSet();

            dataset.Tables.Add(info);
            dataset.Tables.Add(fiche);
            dataset.Tables.Add(enfants);

            //report.Database.Tables[0].SetDataSource(info);
            //report.Database.Tables[1].SetDataSource(fiche);
            //report.Database.Tables[2].SetDataSource(enfants);

            report.SetDataSource(dataset);

            EmployeLoading = false;

            reportViewer.ShowViewer(report);
        }


        private AsyncCommand _printCarteServiceCommand;
        private AsyncCommand _printFicheIndividuelleCommand;
        #endregion
    }
}
