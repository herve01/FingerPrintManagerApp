using FingerPrintManagerApp.Dialog.Service;
using System.Windows;

namespace FingerPrintManagerApp.Dialog.Facade
{
    public interface IDialogFacade
    {
        DialogResult ShowDialog(string message, Window owner);
        DialogResult ShowDialog(object context, Window owner);
    }
}
