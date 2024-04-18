using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.ViewModel;
using Ninject;

namespace FingerPrintManagerApp.Modules.Employe.ViewModel
{
    public class EmployeCtrlViewModel : ControllerViewModel
    {
        public EmployeCtrlViewModel()
        {
            PresenterViewModel = IoC.Container.Instance.Kernel.Get<EmployeDefaultViewModel>();
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
            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<EmployeListViewModel>());
            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<FonctionViewModel>());
            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<BureauViewModel>());
            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<DivisionViewModel>());
            //pageViewModels.Add(IoC.Container.Instance.Kernel.Get<DirectionInterneViewModel>());
            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<DomaineEtudeViewModel>());
            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<NiveauEtudeViewModel>());
            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<EntiteViewModel>());

            pageViewModels.Add(IoC.Container.Instance.Kernel.Get<Modules.Presence.ViewModel.PresenceEmployeViewModel>());

            foreach (var vm in pageViewModels)
                vm.Parent = this;
        }

        private void MenuInit()
        {
            Name = "Employé";
            OptionItem = new OptionItem()
            {
                Name = this.Name,
                ToolTip = "Employé, Grade, Fonction, Affectation, Mécanisation",
                IconPathData = "M15.642869,1.9992427C15.6335,1.9992428 15.624497,1.9994946 15.614976,1.9999981 13.508044,1.9999981 11.853017,2.8380101 10.690054,4.4930072 9.9460364,5.5509896 9.55297,6.883996 9.55297,8.3479843L9.55297,13.094989C9.55297,13.982012 10.384024,15.045976 11.117056,15.985001 11.375966,16.318007 11.628041,16.641006 11.846059,16.95198 12.11498,17.337966 12.325062,17.685988 12.489979,17.98799 13.270007,19.418958 13.233997,22.310006 12.082997,23.644967 11.608998,24.193976 10.992056,24.638007 10.247061,24.965949 9.5500403,25.272956 8.744011,25.443 7.8530202,25.474006 2.8730443,25.641974 2.1790753,26.21198 1.9970683,26.892949L1.9990215,29.983951 13.765979,29.998965 28.99802,29.983951 28.99802,26.888982C28.749973,26.060002,27.556982,25.587958,22.526957,25.454963L22.301983,25.435004C21.666973,25.379951 19.978987,25.235969 18.816999,23.659005 18.104963,22.693978 17.432968,19.798963 18.739973,17.936965L19.415999,16.980971C19.612043,16.70198 19.831038,16.414016 20.056013,16.116958 20.839947,15.081986 21.728985,13.907976 21.728985,12.909991L21.728985,8.487999C21.728985,6.8200006 21.207989,5.266993 20.259991,4.1149864 19.10301,2.712003 17.56102,1.9999981 15.672959,1.9999981 15.661973,1.9994946 15.652238,1.9992428 15.642869,1.9992427z M15.608018,0L15.679063,0C18.150984,0 20.270977,0.98300076 21.804058,2.8439918 23.046,4.3529925 23.728983,6.3579955 23.728983,8.487999L23.728983,12.909991C23.728983,14.579972 22.624979,16.039016 21.649029,17.324965 21.438946,17.602004 21.234966,17.870986 21.052959,18.130994L20.378033,19.086011C19.700056,20.049998 20.066023,21.982982 20.427959,22.471993 21.052959,23.320992 21.977032,23.399972 22.473002,23.441964 22.530008,23.446969 22.58201,23.451974 22.625956,23.456979 27.973974,23.599984 30.450047,24.086005 30.977022,26.557988L31.004,26.686955 30.997042,26.842961 30.998018,31.981996 14.047961,31.999999 13.765979,31.998963 13.405994,31.999999 13.405994,31.998963 0,31.980958 0.023071288,26.548955C0.53601025,24.141974 2.935056,23.638986 7.7840505,23.474984 8.4210129,23.453012 8.9780198,23.338998 9.4420081,23.134957 9.9119788,22.927987 10.291006,22.660958 10.567007,22.338998 11.087027,21.73701 11.184072,19.771008 10.733999,18.945996 10.59899,18.69801 10.426016,18.412976 10.206045,18.096997 10.008048,17.813979 9.776969,17.519973 9.5410071,17.215957 8.6080246,16.022964 7.5529719,14.669999 7.5529719,13.094989L7.5529719,8.3479843C7.5529719,6.469995 8.0720139,4.7399859 9.0540686,3.3430147 10.127066,1.8170148 12.134999,0 15.608018,0z"
            };

            RefreshAccess(AppConfig.CurrentUser != null);
        }

        public override void RefreshAccess(bool isOkay)
        {
            IsAccessible = isOkay;

            foreach (var page in pageViewModels)
                page.RefreshAccess(isOkay);
        }
    }
}
