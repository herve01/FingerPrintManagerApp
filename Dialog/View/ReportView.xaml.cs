using System.Windows;

namespace FingerPrintManagerApp.Dialog.View
{
    /// <summary>
    /// Logique d'interaction pour ReportViewer.xaml
    /// </summary>
    public partial class ReportView : Window
    {
        public ReportView()
        {
            InitializeComponent();

            DataContext = new ViewModel.ReportViewModel();
        }

        //CrystalDecisions.CrystalReports.Engine.ReportClass report;

        //private string exportPath;
        //private DiskFileDestinationOptions dfdOptions;
        //private ExportOptions exportOptions;

        //private string fileName;
        //private Libs.AppConfig.Report reportType;

        //int nbrePages = 1;
        //int actualPage = 1;

        //public ReportView(CrystalDecisions.CrystalReports.Engine.ReportClass report, Libs.AppConfig.Report reportType, string fileName, string title)
        //    : this()
        //{
        //    this.report = report;
        //    txtTitre.Text = title;
        //    this.reportType = reportType;
        //    this.fileName = fileName;

        //    System.ComponentModel.DependencyPropertyDescriptor p = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(SAPBusinessObjects.WPF.Viewer.ViewerCore.TotalPageNumberProperty, typeof(SAPBusinessObjects.WPF.Viewer.ViewerCore));
        //    p.AddValueChanged(crViewer.ViewerCore, onPageCountChanged);
        //}

        //private void onPageCountChanged(object sender, EventArgs e)
        //{
        //    nbrePages = crViewer.ViewerCore.TotalPageNumber;
        //    txtActualPage.Text = "Page " + (nbrePages == 0 ? 0 : actualPage) + " sur " + nbrePages;

        //    if (nbrePages > 1)
        //    {
        //        btNext.IsEnabled = true;
        //        btLast.IsEnabled = true;
        //    }
        //}

        //private void btnPrintBR_Click(object sender, RoutedEventArgs e)
        //{
        //    crViewer.ViewerCore.PrintReport();
        //}

        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    crViewer.ViewerCore.ReportSource = report;
        //    updateTilte();

        //}

        //void updateTilte()
        //{
        //    txtActualPage.Text = "Page " + actualPage + " sur " + nbrePages;
        //}

        //private void btnWord_Click(object sender, RoutedEventArgs e)
        //{
        //    setupExport(2);

        //    try
        //    {
        //        this.report.Export(exportOptions);
        //        MyMsgBox.Show("Exportation vers WORD effectuée avec succès !", "TPI", MyMsgBoxButton.OK, MyMsgBoxIcon.Success);
        //    }
        //    catch (Exception)
        //    {
        //        MyMsgBox.Show("Exportation WORD word échouée !", "TPI", MyMsgBoxButton.OK, MyMsgBoxIcon.Error);
        //    }
        //}

        //private void btnPDF_Click(object sender, RoutedEventArgs e)
        //{
        //    setupExport(1);

        //    try
        //    {
        //        this.report.Export(exportOptions);
        //        MyMsgBox.Show("Exportation vers PDF effectuée avec succès !", "TPI", MyMsgBoxButton.OK, MyMsgBoxIcon.Success);
        //    }
        //    catch (Exception)
        //    {
        //        MyMsgBox.Show("Exportation vers PDF échouée !", "TPI", MyMsgBoxButton.OK, MyMsgBoxIcon.Error);
        //    }
        //}

        //private void btnExcel_Click(object sender, RoutedEventArgs e)
        //{
        //    setupExport(0);

        //    try
        //    {
        //        this.report.Export(exportOptions);
        //        MyMsgBox.Show("Exportation vers EXCEL effectuée avec succès !", "TPI", MyMsgBoxButton.OK, MyMsgBoxIcon.Success);
        //    }
        //    catch (Exception)
        //    {
        //        MyMsgBox.Show("Exportation vers EXCEL échouée !", "TPI", MyMsgBoxButton.OK, MyMsgBoxIcon.Error);
        //    }
        //}

        //private void setupExport(int exportType)
        //{
        //    dfdOptions = new DiskFileDestinationOptions();
        //    exportOptions = new ExportOptions();
        //    exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //    exportOptions.ExportFormatOptions = null;

        //    string fiche = "";
        //    switch (reportType)
        //    {
        //        case Libs.AppConfig.Report.LISTE_ENTREPRISES_SECTEUR:
        //            fiche = "Listes_Entreprises\\SA";
        //            break;

        //        case Libs.AppConfig.Report.LISTE_ENTREPRISES_ENTITE:
        //            fiche = "Listes_Entreprises\\Entite";
        //            break;

        //        case Libs.AppConfig.Report.LISTE_EXTENSIONS_ENTREPRISE:
        //            fiche = "Listes_Extensions\\Entreprise";
        //            break;

        //        case Libs.AppConfig.Report.LISTE_PRODUITS_ENTREPRISE:
        //            fiche = "Listes_Produits\\Entreprise";
        //            break;

        //        case Libs.AppConfig.Report.JOURNAL_DECLARATION:
        //            fiche = "Journal_Declaration";
        //            break;

        //        case Libs.AppConfig.Report.LISTE_ENTREPRISE_VS_DECLARATION:
        //            fiche = "Entreprises_vs_Declaration";
        //            break;

        //        case Libs.AppConfig.Report.NOTE_PERCEPTION:
        //            fiche = "Notes_Perceptions";
        //            break;

        //        case Libs.AppConfig.Report.EVOLUTION_DECLARATION_ANNUELLE:
        //            fiche = "Evolutions_Declarations_Annuelles\\Ensemble";
        //            break;

        //        case Libs.AppConfig.Report.EVOLUTION_DECLARATION_ANNUELLE_ENTITE:
        //            fiche = "Evolutions_Declarations_Annuelles\\Entites";
        //            break;

        //        case Libs.AppConfig.Report.EVOLUTION_DECLARATION_ANNUELLE_EXTENSION:
        //            fiche = "Evolutions_Declarations_Annuelles\\Entreprises";
        //            break;

        //        case Libs.AppConfig.Report.EVOLUTION_DECLARATION_ANNUELLE_SECTEUR:
        //            fiche = "Evolutions_Declarations_Annuelles\\Secteurs";
        //            break;

        //        default:
        //            fiche = "Reports";
        //            break;
        //    }

        //    switch (exportType)
        //    {
        //        case 0:
        //            exportPath = @"C:\FPI_TPI\TPI_L\Exported\Excel\" + fiche + "\\";

        //            if (!System.IO.Directory.Exists(exportPath))
        //                System.IO.Directory.CreateDirectory(exportPath);

        //            exportPath += fileName + ".xls";
        //            exportOptions.ExportFormatType = ExportFormatType.Excel;
        //            break;

        //        case 1:
        //            exportPath = @"C:\FPI_TPI\TPI_L\Exported\PDF\" + fiche + "\\";

        //            if (!System.IO.Directory.Exists(exportPath))
        //                System.IO.Directory.CreateDirectory(exportPath);

        //            exportPath += fileName + ".pdf";
        //            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            break;


        //        case 2:
        //            exportPath = @"C:\FPI_TPI\TPI_L\Exported\Word\" + fiche + "\\";

        //            if (!System.IO.Directory.Exists(exportPath))
        //            {
        //                System.IO.Directory.CreateDirectory(exportPath);
        //            }
        //            exportPath += fileName + ".doc";
        //            exportOptions.ExportFormatType = ExportFormatType.WordForWindows;
        //            break;
        //        default:
        //            break;
        //    }

        //    dfdOptions.DiskFileName = exportPath;
        //    exportOptions.ExportDestinationOptions = dfdOptions;
        //}

        //private void navigation(object sender, RoutedEventArgs e)
        //{
        //    Button bt = sender as Button;
        //    switch (bt.Name)
        //    {
        //        case "btNext":
        //            if (actualPage < nbrePages)
        //            {
        //                crViewer.ViewerCore.ShowNextPage();
        //                actualPage++;

        //                btFirst.IsEnabled = true;
        //                btPrev.IsEnabled = true;

        //                if (actualPage == nbrePages)
        //                {
        //                    btLast.IsEnabled = false;
        //                    btNext.IsEnabled = false;
        //                }
        //            }
        //            break;

        //        case "btPrev":
        //            if (actualPage > 1)
        //            {
        //                crViewer.ViewerCore.ShowPreviousPage();
        //                actualPage--;

        //                btLast.IsEnabled = true;
        //                btNext.IsEnabled = true;

        //                if (actualPage == 1)
        //                {
        //                    btFirst.IsEnabled = false;
        //                    btPrev.IsEnabled = false;

        //                }
        //            }
        //            break;

        //        case "btFirst":
        //            crViewer.ViewerCore.ShowFirstPage();
        //            actualPage = 1;

        //            btFirst.IsEnabled = false;
        //            btPrev.IsEnabled = false;
        //            btLast.IsEnabled = true;
        //            btNext.IsEnabled = true;

        //            break;

        //        case "btLast":
        //            crViewer.ViewerCore.ShowLastPage();
        //            actualPage = nbrePages;

        //            btFirst.IsEnabled = true;
        //            btPrev.IsEnabled = true;
        //            btLast.IsEnabled = false;
        //            btNext.IsEnabled = false;

        //            break;
        //    }

        //    txtActualPage.Text = "Page " + actualPage + " sur " + nbrePages;
        //}
    }
}
