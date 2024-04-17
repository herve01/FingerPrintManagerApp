using CrystalDecisions.Shared;

namespace FingerPrintManagerApp.ViewModel
{
    public class ReportViewModel : ViewModelBase
    {
        private string printerName;

        public ReportViewModel()
        {
        }

        public ReportViewModel(CrystalDecisions.CrystalReports.Engine.ReportClass report, string title, string printerName = "")
        {
            this._report = report;
            this._title = title;
            this.printerName = printerName;
        }

        CrystalDecisions.CrystalReports.Engine.ReportClass _report;
        public CrystalDecisions.CrystalReports.Engine.ReportClass Report
        {
            get
            {
                return this._report;
            }
            set
            {
                if (_report != value)
                {
                    _report = value;
                    RaisePropertyChanged(() => Report);
                }
            }
        }

        private string exportPath;
        private DiskFileDestinationOptions dfdOptions;
        private ExportOptions exportOptions;

        private string fileName;

        private int _pageCount;
        public int PageCount
        {
            get
            {
                return this._pageCount;
            }
            set
            {
                if (_pageCount != value)
                {
                    _pageCount = value;
                    RaisePropertyChanged(() => PageCount);
                }
            }
        }

        private int _currentPage;
        public int CurrentPage
        {
            get
            {
                return this._currentPage;
            }
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    RaisePropertyChanged(() => CurrentPage);
                }
            }
        }

        private string _title;
        public string ReportTitle
        {
            get
            {
                return this._title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChanged(() => ReportTitle);
                }
            }
        }

        public void PrintSilenty()
        {
            if (printerName != string.Empty)
                _report.PrintOptions.PrinterName = printerName;

            try
            {
                _report.PrintToPrinter(1, false, 0, 0);
            }
            catch (System.Exception)
            {
            }

        }
    }
}


