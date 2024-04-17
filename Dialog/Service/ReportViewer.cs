using FingerPrintManagerApp.ViewModel;
using CrystalDecisions.CrystalReports.Engine;

namespace FingerPrintManagerApp.Dialog.Service
{
    public class ReportViewer : IReportViewer
    {
        public void PrintSilently(ReportClass report, string printerName = "")
        {
            var vm = new ReportViewModel(report, "", printerName);
            vm.PrintSilenty();
        }

        public void ShowViewer(ReportClass report, string title = "")
        {
            var vm = new ReportViewModel(report, title);
            new View.ReportView() { DataContext = vm }.ShowDialog();
        }
    }
}
