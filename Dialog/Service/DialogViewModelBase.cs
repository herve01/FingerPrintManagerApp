using FingerPrintManagerApp.ViewModel.Command;
using System.Windows;
using System.Windows.Input;

namespace FingerPrintManagerApp.Dialog.Service
{
    public abstract class DialogViewModelBase : ViewModel.PageViewModel
    {
        public DialogResult Result
        {
            get;
            private set;
        }

        public string Message
        {
            get;
            private set;
        }

        public DialogViewModelBase(string message = "")
        {
            this.Message = message;
        }

        public void CloseDialogWithResult(Window dialog, DialogResult result)
        {
            this.Result = result;
            if (dialog != null)
                dialog.DialogResult = true;
        }

        protected RelayCommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand(p => Close(p), p => CanCancel(p));

                return _closeCommand;
            }
        }

        protected virtual void Close(object param)
        {
            UpdateTimer.IsEnabled = false;
        }

        protected virtual bool CanCancel(object param)
        {
            return true;
        }

    }
}
