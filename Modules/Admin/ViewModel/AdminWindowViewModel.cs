using FingerPrintManagerApp.ViewModel;
using Ninject;

namespace FingerPrintManagerApp.Modules.Admin.ViewModel
{
    public class AdminWindowViewModel : ControllerViewModel
    {
        public AdminWindowViewModel()
        {
            LoadViewModels();

            CurrentPageViewModel = pageViewModels[0];
        }

        void LoadViewModels()
        {
            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<AdminCtrlViewModel>());
        }

        public override void RefreshAccess(bool isOkay)
        {
            foreach (var page in pageViewModels)
                page.RefreshAccess(isOkay);
        }

    }
}
