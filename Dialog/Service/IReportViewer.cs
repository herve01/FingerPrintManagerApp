
namespace FingerPrintManagerApp.Dialog.Service
{
    public interface IReportViewer
    {
        void PrintSilently(CrystalDecisions.CrystalReports.Engine.ReportClass report, string printerName = "");
        void ShowViewer(CrystalDecisions.CrystalReports.Engine.ReportClass report, string title = "");
    }
}
