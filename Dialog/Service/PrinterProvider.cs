using System.Windows.Controls;

namespace FingerPrintManagerApp.Dialog.Service
{
    public class PrinterProvider : IPrinterProvider
    {
        public string ChoosePrinter()
        {
            var print = new PrintDialog();

            return print.ShowDialog() == true ? print.PrintQueue.Name : string.Empty;
        }
    }
}
