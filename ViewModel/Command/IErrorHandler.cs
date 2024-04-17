using System;

namespace FingerPrintManagerApp.ViewModel.Command
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
