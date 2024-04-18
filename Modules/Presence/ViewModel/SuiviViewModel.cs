using ARG.Controls;
using FingerPrintManagerApp.Dao.Employe;
using FingerPrintManagerApp.Extension;
using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Employe;
using FingerPrintManagerApp.Model.Presence;
using FingerPrintManagerApp.ViewModel;
using FingerPrintManagerApp.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FingerPrintManagerApp.Modules.Presence.ViewModel
{
    public class SuiviViewModel : PageViewModel
    {
        private static object _lock = new object();

        ObservableCollection<Model.Employe.Employe> employes;
        List<Model.Presence.Presence> details;

        public ICollectionView EmployesView { get; private set; }

        public SuiviViewModel()
        {
            employes = new ObservableCollection<Model.Employe.Employe>();
            BindingOperations.EnableCollectionSynchronization(employes, _lock);

            EmployesView = (CollectionView)CollectionViewSource.GetDefaultView(employes);
            EmployesView.SortDescriptions.Add(new SortDescription("Eleve.Nom", ListSortDirection.Ascending));
            EmployesView.SortDescriptions.Add(new SortDescription("Eleve.PostNom", ListSortDirection.Ascending));
            EmployesView.SortDescriptions.Add(new SortDescription("Eleve.Prenom", ListSortDirection.Ascending));
            EmployesView.Filter += OnEmployeFilter;

            Appointments = new ObservableCollection<Appointment>();

            details = new List<Model.Presence.Presence>();

            EmployeFilterText = string.Empty;

            MenuInit();
        }

        private bool OnEmployeFilter(object obj)
        {
            var employe = obj as Model.Employe.Employe;

            if (employe == null)
                return false;

            var motif = EmployeFilterText.Trim().ToLower().NoAccent();

            return (employe.Name.ToLower().NoAccent().Contains(motif) ||
                    employe.Matricule.ToString().ToLower().NoAccent().Contains(motif) ||
                    //employe.CurrentGrade.Id.ToLower().NoAccent().Contains(motif) ||
                    employe.Sexe.ToString().ToLower().NoAccent().Contains(motif) ||
                    (!string.IsNullOrWhiteSpace(employe.CurrentAffectation.Id) && employe.CurrentAffectation.Unite.ToString().ToLower().NoAccent().Contains(motif))
                   );
        }

        private Model.Employe.Employe _selectedEmploye;
        private string _employeFilterText;
        ObservableCollection<Appointment> appointments;

        public ObservableCollection<Appointment> Appointments
        {
            get
            {
                return this.appointments;
            }
            set
            {
                if (appointments != value)
                {
                    appointments = value;
                    RaisePropertyChanged(() => Appointments);
                }
            }
        }
        public string EmployeFilterText
        {
            get
            {
                return this._employeFilterText;
            }
            set
            {
                if (_employeFilterText != value)
                {
                    _employeFilterText = value;
                    RaisePropertyChanged(() => EmployeFilterText);

                    EmployesView.Refresh();
                }
            }
        }
        public Model.Employe.Employe SelectedEmploye
        {
            get { return _selectedEmploye; }
            set
            {
                if (_selectedEmploye != value)
                {
                    _selectedEmploye = value;
                    RaisePropertyChanged(() => SelectedEmploye);
                    LoadDetails();
                }
            }
        }
        
        private DateTime _displayDate;
        private DateTime _endDate;
        private string _selectedMonth;
        private int _yearPresences;
        private int _monthPresences;
        private bool _employeLoading;

        public DateTime DisplayDate
        {
            get { return _displayDate; }
            set
            {
                if (_displayDate != value)
                {
                    _displayDate = value;
                    RaisePropertyChanged(() => DisplayDate);
                    SelectedMonth = MonthOfDay(DisplayDate);
                }
            }
        }
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    RaisePropertyChanged(() => EndDate);
                }
            }
        }

        string MonthOfDay(DateTime day)
        {
            //return string.Format("{0} {1}", GetMonthName(day.Month), day.Year);

            return string.Empty;
        }

        public string SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                if (_selectedMonth != value)
                {
                    _selectedMonth = value;
                    RaisePropertyChanged(() => SelectedMonth);
                    MonthPresences = details.Count;
                }
            }
        }
        public int YearPresences
        {
            get { return _yearPresences; }
            set
            {
                if (_yearPresences != value)
                {
                    _yearPresences = value;
                    RaisePropertyChanged(() => YearPresences);
                }
            }
        }
        public int MonthPresences
        {
            get { return _monthPresences; }
            set
            {
                if (_monthPresences != value)
                {
                    _monthPresences = value;
                    RaisePropertyChanged(() => MonthPresences);
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
        
        protected override async Task Load(object param = null)
        {
            if (!IsInit)
            {
                DisplayDate = DateTime.Today;

                await LoadEmployes();
                IsInit = true;
            }
        }

        private async Task LoadEmployes()
        {
            EmployeLoading = true;

            employes.Clear();

            await Task.Run(() => new EmployeDao().GetAllActifsAsync(AppConfig.CurrentUser.Entite, employes));

            EmployeLoading = false;

            EmployesView.Refresh();
        }

        async Task LoadDetails()
        {
            Appointments = new ObservableCollection<Appointment>();
            details.Clear();
            YearPresences = MonthPresences = 0;

            if (SelectedEmploye == null)
                return;

            //details = await new Dao.Presence.AppelDetailDao().GetDetailsByEmployeAsync(SelectedEmploye);

            //YearPresences = details.FindAll(d => d.Etat == AppelEtat.Présent).Count;
            //MonthPresences = details.FindAll(d => d.Etat == AppelEtat.Présent && d.Jour.Month == DisplayDate.Month).Count;

            var appointments = new ObservableCollection<Appointment>();

            foreach (var detail in details)
            {
                //var subject = string.Format("Etat : {0},\nConduite : {1},\nApplication : {2},\nObservation: {3}", detail.Etat, detail.Conduite,
                //    detail.Application, detail.Observation);

                //appointments.Add(new Appointment()
                //{
                //    Day = detail.Jour,
                //    Subject = subject
                //});
            }

            Appointments = appointments;
        }

        private void MenuInit()
        {
            Name = "Suivi";

            OptionItem = new OptionItem()
            {
                Name = "Suivi",
                ToolTip = "Suivi",
                IconPathData = "M5.3000031,24.599984L5.3000031,31.699994 0,31.699994 0,30.00001z M9.6000061,19.4L11.700012,22.599984 11.700012,31.8 6.9000092,31.8 6.9000092,22.599984z M18.600006,18.799994L18.600006,31.699994 13.800003,31.699994 13.800003,23.599984z M25.5,11.899999L25.5,31.699994 20.700012,31.699994 20.700012,17.199988z M32,0L27.800003,10.599979 25.900009,8.5000027 13.800003,21.19999 9.8000031,17.199988 0,27.299998 0,22.500008 9.8000031,12.399999 13.700012,16.4 23.300003,6.2999901 21.200012,4.2999896z"
            };

            var help = string.Empty;
            App.Tips.TryGetValue(Name, out help);
            Help = help;
        }

        private AsyncCommand _refreshEmployeCommand;
        #endregion
    }
}
