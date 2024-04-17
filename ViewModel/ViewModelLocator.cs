//using FingerPrintManagerApp.Modules.Admin.ViewModel;
//using FingerPrintManagerApp.Modules.Presence.ViewModel;
//using Ninject;

//namespace FingerPrintManagerApp.ViewModel
//{
//    public class ViewModelLocator
//    {
//        private MainWindowViewModel mainWindowViewModel = null;
//        private AdminWindowViewModel adminWindowViewModel = null;
//        private PresenceWindowViewModel presenceWindowViewModel = null;

//        public MainWindowViewModel MainWindowViewModel
//        {
//            get { return mainWindowViewModel; }
//            set { mainWindowViewModel = value; }
//        }
//        public AdminWindowViewModel AdminWindowViewModel
//        {
//            get { return adminWindowViewModel; }
//            set { adminWindowViewModel = value; }
//        }
//        public PresenceWindowViewModel PresenceWindowViewModel
//        {
//            get { return presenceWindowViewModel; }
//            set { presenceWindowViewModel = value; }
//        }

//        public ViewModelLocator()
//        {
//            this.mainWindowViewModel = IoC.Container.Instance.Kernel.Get<MainWindowViewModel>();
//            this.adminWindowViewModel = IoC.Container.Instance.Kernel.Get<AdminWindowViewModel>();
//            this.presenceWindowViewModel = IoC.Container.Instance.Kernel.Get<PresenceWindowViewModel>();
//        }
//    }
//}
