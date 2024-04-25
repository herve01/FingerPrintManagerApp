using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.ViewModel;
using Ninject;

namespace FingerPrintManagerApp.Modules.Presence.ViewModel
{
    public class PresenceCtrlViewModel : ControllerViewModel
    {
        public PresenceCtrlViewModel()
        {
            PresenterViewModel = IoC.Container.Instance.Kernel.Get<PresenceDefaultViewModel>();
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
            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<PresenceEmployeViewModel>());
            //pageViewModels.Add(IoC.Container.Instance.Kernel.Get<SuiviViewModel>());

            foreach (var vm in pageViewModels)
                vm.Parent = this;
        }

        private void MenuInit()
        {
            Name = "Présence";
            OptionItem = new OptionItem()
            {
                Name = this.Name,
                ToolTip = "Présence, suivi, période",
                IconPathData = "M7.9915932,6.8739929C9.0265773,6.8739929,9.8655808,7.7139893,9.8655808,8.7489929L9.8655808,14.98999 9.8895677,14.925995C10.152562,14.289993 10.841545,13.835999 11.65052,13.835999 12.685504,13.835999 13.525484,14.579987 13.525484,15.497986L13.525484,16.14299 13.549502,16.084C13.812496,15.501999 14.501479,15.085999 15.310455,15.085999 16.345437,15.085999 17.184442,15.766998 17.184442,16.606995L17.184442,21.03299 17.401418,18.790985C17.487415,17.90799 18.272403,17.260986 19.155381,17.347 20.038362,17.431 20.685352,18.217987 20.599355,19.100998L19.766362,27.69899 19.766362,28.384995C19.766362,30.381989,18.147405,32,16.151441,32L9.724562,32C8.040603,32,6.6256216,30.847992,6.2236212,29.288986L6.1926468,29.153 1.2367164,20.863998C0.86172376,20.237 1.1977159,19.347 1.9877092,18.875 2.776696,18.403992 3.719671,18.528992 4.0946639,19.154999L6.1166289,22.537994 6.1166289,8.7489929C6.1166289,7.7139893,6.956609,6.8739929,7.9915932,6.8739929z M14.894477,2.7559967C15.209474,2.7559967 15.525446,2.8759918 15.766439,3.1169891 16.248454,3.598999 16.248454,4.3789978 15.766439,4.8609924L13.697479,6.9299927C13.215493,7.4109955 12.435509,7.4109955 11.953523,6.9299927 11.472545,6.447998 11.472545,5.6679993 11.953523,5.1859894L14.022484,3.1169891C14.263477,2.8759918,14.578473,2.7559967,14.894477,2.7559967z M1.2327186,2.7559967C1.5487224,2.7559967,1.864726,2.8759918,2.1057189,3.1169891L4.1746796,5.1859894C4.655658,5.6679993 4.655658,6.447998 4.1746796,6.9299927 3.6926942,7.4109955 2.9117031,7.4109955 2.4297178,6.9299927L0.36075655,4.8609924C-0.12025218,4.3789978 -0.12025218,3.598999 0.36075655,3.1169891 0.60174946,2.8759918 0.91772244,2.7559967 1.2327186,2.7559967z M8.0915969,0C8.7725842,0,9.3245757,0.55198669,9.3245757,1.2329865L9.3245757,4.1589966C9.3245757,4.8409882 8.7725842,5.3919983 8.0915969,5.3919983 7.4106105,5.3919983 6.858619,4.8409882 6.858619,4.1589966L6.858619,1.2329865C6.858619,0.55198669,7.4106105,0,8.0915969,0z"
            };

            RefreshAccess(AppConfig.CurrentUser != null);
        }

        public override void RefreshAccess(bool isOkay)
        {
            IsAccessible = isOkay;

            foreach (var page in pageViewModels)
                page.RefreshAccess(isOkay);
        }

        public override PageViewModel CurrentPageViewModel
        {
            get => base.CurrentPageViewModel;
            set
            {
                base.CurrentPageViewModel = value;
            }
        }
    }
}
