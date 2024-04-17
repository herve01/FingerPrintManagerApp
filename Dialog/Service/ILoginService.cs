
using System.Windows;

namespace FingerPrintManagerApp.Dialog.Service
{
    public interface ILoginService
    {
        void Login(bool forEdit = false, Window owner = null);
        object ConnectedUser { get; }
    }
}
