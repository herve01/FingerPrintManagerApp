using FingerPrintManagerApp.Dialog.Service;
using FingerPrintManagerApp.ViewModel.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace FingerPrintManagerApp.ViewModel
{
    public abstract class ControllerDialogViewModel : DialogViewModelBase
    {
        public ControllerDialogViewModel()
        {
            pageViewModels = new ObservableCollection<PageViewModel>();
            PagesView = (ICollectionView)CollectionViewSource.GetDefaultView(pageViewModels);
        }

        protected ObservableCollection<PageViewModel> pageViewModels;
        public ICollectionView PagesView { get; set; }
        protected RelayCommand changePageCommand;

        protected PageViewModel _currentPageViewModel;
        public PageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    RaisePropertyChanged(() => CurrentPageViewModel);
                }
            }
        }

        public virtual void InitView()
        {
            CurrentPageViewModel = PresenterViewModel;
        }

        protected PageViewModel PresenterViewModel = null;

        public ICommand ChangePageCommand
        {
            get
            {
                if (changePageCommand == null)
                {
                    changePageCommand = new RelayCommand(p => ChangeViewModel(p), p => p is PageViewModel);
                }

                return changePageCommand;
            }
        }

        protected void ChangeViewModel(object param)
        {
            var viewModel = param as PageViewModel;
            if (viewModel != null)
            {
                if (!pageViewModels.Contains(viewModel))
                    pageViewModels.Add(viewModel);

                CurrentPageViewModel = viewModel;
                PagesView.MoveCurrentTo(viewModel);
            }

        }
    }
}
