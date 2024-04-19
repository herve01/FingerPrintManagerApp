using FingerPrintManagerApp.Dialog.Facade;
using FingerPrintManagerApp.Dialog.Service;
using FingerPrintManagerApp.Modules.Admin.ViewModel;
using FingerPrintManagerApp.Modules.Employe.ViewModel;
using FingerPrintManagerApp.Modules.Presence.ViewModel;
using FingerPrintManagerApp.ViewModel;
using FingerPrintManagerApp.Modules.Employe.View;
using Ninject.Modules;

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


            //#region Employe

            Bind<EmployeCtrlViewModel>().ToSelf();
            Bind<EmployeDefaultViewModel>().ToSelf();
            Bind<EmployeListViewModel>().ToSelf();
            Bind<AffectationViewModel>().ToSelf();
            Bind<DepartementViewModel>().ToSelf();
            Bind<DirectionViewModel>().ToSelf();
            Bind<EntiteViewModel>().ToSelf();
            Bind<FonctionViewModel>().ToSelf();
            Bind<NiveauEtudeViewModel>().ToSelf();
            Bind<DomaineEtudeViewModel>().ToSelf();
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
            Bind<PresenceCtrlViewModel>().ToSelf();
            Bind<PresenceDefaultViewModel>().ToSelf();
            Bind<PresenceWindowViewModel>().ToSelf();
            Bind<PresenceEmployeViewModel>().ToSelf();
            //#endregion
        }
    }
}
