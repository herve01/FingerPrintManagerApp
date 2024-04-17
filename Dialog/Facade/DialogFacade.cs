using System.Windows;
using FingerPrintManagerApp.Dialog.Service;

namespace FingerPrintManagerApp.Dialog.Facade
{
    public class DialogFacade : IDialogFacade
    {
        public DialogResult ShowDialog(string message, Window owner)
        {
            return ShowDialog(new View.DialogYesNoViewModel(message), owner);
        }

        public static DialogResult ShowDialog(DialogViewModelBase vm, Window owner)
        {
            var win = new View.DialogWindow();
            win.DataContext = vm;

            if (owner != null)
            {
                win.Owner = owner;
                win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                win.ShowInTaskbar = false;
            }
            else
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            win.ShowDialog();

            return (win.DataContext as DialogViewModelBase).Result;
        }

        public DialogResult ShowDialog(object context, Window owner)
        {
            if (context is DialogViewModelBase)
                return ShowDialog((DialogViewModelBase)context, owner);

            return DialogResult.No;
        }
    }
}
