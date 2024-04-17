using FingerPrintManagerApp.ViewModel.Extension;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FingerPrintManagerApp.ViewModel.Command
{
    public class AsyncCommand : IAsyncCommand
    {
        //public event EventHandler CanExecuteChanged;

        private bool _isExecuting;
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;
        private readonly IErrorHandler _errorHandler;

        public AsyncCommand(Func<object, Task> execute, Func<object, bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
        }

        public bool CanExecute(object parameter)
        {
            return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
        }

        public async Task ExecuteAsync(object parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    await _execute(parameter);
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            //RaiseCanExecuteChanged();
        }

        //public void RaiseCanExecuteChanged()
        //{
        //    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        //}

        // Come from RelayCommand
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #region Explicit implementations
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            ExecuteAsync(parameter).FireAndForgetSafeAsync(_errorHandler);
        }
        #endregion
    }
}
