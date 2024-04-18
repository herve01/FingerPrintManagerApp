using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.ViewModel;
using Ninject;

namespace FingerPrintManagerApp.Modules.Admin.ViewModel
{
    public class AdminCtrlViewModel : ControllerViewModel
    {
        public AdminCtrlViewModel()
        {
            PresenterViewModel = IoC.Container.Instance.Kernel.Get<AdminPresentationViewModel>();
            InitView();
        }

        public override void InitView()
        {
            base.InitView();
            MenuInit();
            LoadViewModels();
        }

        void LoadViewModels()
        {
            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<ConnectorViewModel>());
           // pageViewModels.Add(IoC.Container.Instance.Kernel.Get<SettingsViewModel>());
            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<UserViewModel>());
            //pageViewModels.Add(IoC.Container.Instance.Kernel.Get<RoleViewModel>());
            //pageViewModels.Add(IoC.Container.Instance.Kernel.Get<ShopViewModel>());
            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<LogViewModel>());

            foreach (var vm in pageViewModels)
                vm.Parent = this;
        }

        public override void RefreshAccess(bool isOkay)
        {
            IsAccessible = AppConfig.CurrentUser != null;

            foreach (var page in pageViewModels)
                page.RefreshAccess(isOkay);
        }

        private void MenuInit()
        {
            Name = "Paramètres & Admin.";
            OptionItem = new OptionItem()
            {
                Name = Name,
                ToolTip = "Administration, Utilisateur, Paramètre, Rôle, Log, Département",
                IconPathData = "M20.693141,13.340046C21.718123,13.340046 22.54811,14.173035 22.54811,15.201023 22.54811,16.229009 21.718123,17.062 20.693141,17.062 20.640141,17.062 20.591144,17.051 20.539144,17.045999L20.539144,13.356046C20.591144,13.351045,20.640141,13.340046,20.693141,13.340046z M8.5700741,11.576941L9.8290606,16.004963 7.4030857,16.004963z M9.9420252,9.2130125L7.3430238,9.3710936 4.213016,20.096069 6.2990775,20.24707 6.9190731,17.874023 10.352059,17.890014 10.936044,20.501098 13.476086,20.601074z M24.725125,4.5749825L27.994102,6.5899804 26.695111,9.4409769C27.073108,9.8429774,27.443105,10.337976,27.735103,10.809976L31.139079,10.491977 31.999072,14.103972 28.937095,15.390971C28.921094,15.884971,28.860095,16.593969,28.762095,17.06197L31.398076,19.103968 29.346091,22.264965 26.462113,21.039965C26.027116,21.466965,25.542119,21.839964,25.022122,22.162964L25.264121,25.364961 21.496148,26.17296 20.544156,23.880962 20.539155,22.845964 20.539155,20.213966C20.590156,20.214966 20.640155,20.221966 20.692155,20.221966 23.502133,20.221966 25.780118,17.936968 25.780118,15.118972 25.780118,12.299974 23.502133,10.015977 20.692155,10.015977 20.640155,10.015977 20.590156,10.021977 20.539155,10.022977L20.539155,6.8429802C20.590156,6.8419803 20.640155,6.8349803 20.692155,6.8349803 21.348149,6.8349803 21.982144,6.91898 22.593141,7.0639799z M18.808,0L18.808,30 0,26.993042 0,3.0570069z"
            };

            RefreshAccess(false);
        }

    }
}
