
using FingerPrintManagerApp.ViewModel.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace FingerPrintManagerApp.ViewModel
{
    public abstract class ControllerViewModel : PageViewModel
    {
        public ControllerViewModel()
        {
            pageViewModels = new ObservableCollection<PageViewModel>();
            PagesView = (CollectionView)CollectionViewSource.GetDefaultView(pageViewModels);
        }

        protected ObservableCollection<PageViewModel> pageViewModels;
        public ICollectionView PagesView { get; set; }
        protected RelayCommand changePageCommand;

        protected PageViewModel _currentPageViewModel;
        public virtual PageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    if (_currentPageViewModel != null)
                        _currentPageViewModel.Unload();

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

        public void Reinit(bool auto)
        {
            foreach (var page in pageViewModels)
            {
                if (page is ControllerViewModel)
                    ((ControllerViewModel)page).Reinit(auto);

                page.IsInit = auto;
                page.IsAccessible = false;
            }

            CurrentPageViewModel = PresenterViewModel;
        }

        public PageViewModel GetPage(Type type)
        {
            if (type == this.GetType())
                return this;

            foreach (var page in pageViewModels)
            {
                if (page.GetType() == type)
                    return page;
                else
                {
                    if (page is ControllerViewModel)
                    {
                        var p = ((ControllerViewModel)page).GetPage(type);
                        if (p != null)
                            return p;
                    }
                }

            }


            return null;
        }
    }
}
