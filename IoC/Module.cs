using FingerPrintManagerApp.Dialog.Facade;
using FingerPrintManagerApp.Dialog.Service;
using FingerPrintManagerApp.Modules.Admin.ViewModel;

//using FingerPrintManagerApp.Modules.Admin.ViewModel;
//using FingerPrintManagerApp.Modules.Subscriber.ViewModel;
//using FingerPrintManagerApp.Modules.Employe.ViewModel;
using FingerPrintManagerApp.ViewModel;
using Ninject.Modules;
//using FingerPrintManagerApp.Modules.Carriere.ViewModel;
//using FingerPrintManagerApp.Modules.Presence.ViewModel;

namespace FingerPrintManagerApp.IoC
{
    public class Module : NinjectModule
    {
        public override void Load()
        {
            Bind<IDialogFacade>().To<DialogFacade>().InSingletonScope();
            Bind<IFilePathProvider>().To<FilePathProvider>().InSingletonScope();
            Bind<IFolderPathProvider>().To<FolderPathProvider>().InSingletonScope();
            Bind<IReportViewer>().To<ReportViewer>().InSingletonScope();
            Bind<IPrinterProvider>().To<PrinterProvider>().InSingletonScope();
            Bind<IImageViewer>().To<ImageViewer>().InSingletonScope();
            Bind<ILoginService>().To<LoginService>().InSingletonScope();
            Bind<IPhotoCapture>().To<PhotoCapture>().InSingletonScope();

            #region Global
            Bind<MainWindowViewModel>().ToSelf();
            Bind<MainWindowPresenterViewModel>().ToSelf();
            #endregion

            //#region Subscriber
            //Bind<SubscriptionCtrlViewModel>().ToSelf();
            //Bind<BankViewModel>().ToSelf();
            //Bind<CurrencyViewModel>().ToSelf();
            //Bind<BankAccountViewModel>().ToSelf();
            //Bind<ExchangeRateViewModel>().ToSelf();
            //Bind<MoneyConversionViewModel>().ToSelf();
            //Bind<ExchangeViewModel>().ToSelf();
            //Bind<SubscriptionDefaultViewModel>().ToSelf();
            //Bind<SubscriptionViewModel>().ToSelf();
            //Bind<SubscriberListViewModel>().ToSelf();
            //Bind<PricingViewModel>().ToSelf();
            //Bind<BonusViewModel>().ToSelf();
            //#endregion

            //#region Employe

            //Bind<EmployeCtrlViewModel>().ToSelf();
            //Bind<EmployeDefaultViewModel>().ToSelf();
            //Bind<EmployeListViewModel>().ToSelf();
            //Bind<DomaineEtudeViewModel>().ToSelf();
            //Bind<NiveauEtudeViewModel>().ToSelf();
            //Bind<DivisionViewModel>().ToSelf();
            //Bind<FonctionViewModel>().ToSelf();
            //Bind<DirectionInterneViewModel>().ToSelf();
            //Bind<BureauViewModel>().ToSelf();
            //Bind<EntiteViewModel>().ToSelf();
            //#endregion

            //#region Carrière
            //Bind<CarriereCtrlViewModel>().ToSelf();
            //Bind<CarriereDefaultViewModel>().ToSelf();
            //Bind<MecanisationViewModel>().ToSelf();
            //Bind<RecensementViewModel>().ToSelf();
            //Bind<RepriseViewModel>().ToSelf();
            //Bind<AlitementViewModel>().ToSelf();
            //Bind<DisponibiliteViewModel>().ToSelf();
            //Bind<SuspensionViewModel>().ToSelf();
            //Bind<DecesViewModel>().ToSelf();
            //Bind<RetraiteViewModel>().ToSelf();
            //Bind<FormationViewModel>().ToSelf();
            //Bind<DetachementViewModel>().ToSelf();
            //Bind<AffectationViewModel>().ToSelf();
            //Bind<PromotionViewModel>().ToSelf();
            //Bind<NominationViewModel>().ToSelf();
            //#endregion

            //#region Admin
            Bind<AdminWindowViewModel>().ToSelf();
            Bind<AdminCtrlViewModel>().ToSelf();
            Bind<AdminPresentationViewModel>().ToSelf();
            Bind<LogViewModel>().ToSelf();
            Bind<UserViewModel>().ToSelf();
            Bind<ConnectorViewModel>().ToSelf();
            //#endregion

            //#region Présence
            //Bind<PresenceCtrlViewModel>().ToSelf();
            //Bind<PresenceDefaultViewModel>().ToSelf();
            //Bind<PresenceWindowViewModel>().ToSelf();
            //Bind<PresenceEmployeViewModel>().ToSelf();
            //#endregion
        }
    }
}
