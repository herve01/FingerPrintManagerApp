
using System.Windows;

namespace FingerPrintManagerApp.Dialog.Service
{
    public static class DialogService
    {
        public static DialogResult OpenDialog(DialogViewModelBase vm, Window owner)
        {
            DialogWindow win = new DialogWindow();
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
    }
}
