using System.Threading.Tasks;
using System.Windows.Input;

namespace FingerPrintManagerApp.ViewModel.Command
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
        bool CanExecute(object parameter);
    }
}
