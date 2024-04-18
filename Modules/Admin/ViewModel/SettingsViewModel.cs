using FingerPrintManagerApp.Model;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System;
using System.ComponentModel;
using FingerPrintManagerApp.Dialog.Service;
using System.Collections.ObjectModel;
using System.Windows.Data;
using FingerPrintManagerApp.ViewModel;
using FingerPrintManagerApp.ViewModel.Command;
using FingerPrintManagerApp.Dao.Admin;
using FingerPrintManagerApp.Model.Helper;
using FingerPrintManagerApp.Model.Admin;

namespace FingerPrintManagerApp.Modules.Admin.ViewModel
{
    public class SettingsViewModel : PageViewModel, IDataErrorInfo
    {
        IFolderPathProvider folderPathProvider;
        IPrinterProvider printerProvider;
        IFilePathProvider filePathProvider;

        //private ObservableCollection<Shop> shops;

        public ICollectionView ShopsView { get; private set; }

        public SettingsViewModel(IFolderPathProvider folderPathProvider, IPrinterProvider printerProvider, IFilePathProvider filePathProvider)
        {
            this.folderPathProvider = folderPathProvider;
            this.printerProvider = printerProvider;
            this.filePathProvider = filePathProvider;

            //shops = new ObservableCollection<Shop>();
            //ShopsView = (CollectionView)CollectionViewSource.GetDefaultView(shops);
            //ShopsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            
            MenuInit();
        }
        
        #region Local Settings
     
        private string _defaultPrintName;
        public string DefaultPrintName
        {
            get
            {
                return this._defaultPrintName;
            }
            set
            {
                if (_defaultPrintName != value)
                {
                    _defaultPrintName = value;
                    RaisePropertyChanged(() => DefaultPrintName);
                    
                    AppConfig.DefaultPrinterName = value;
                    Properties.Settings.Default.default_printer_name = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private float _sleepDelay;
        public float SleepDelay
        {
            get
            {
                return this._sleepDelay;
            }
            set
            {
                if (_sleepDelay != value)
                {
                    _sleepDelay = value;
                    RaisePropertyChanged(() => SleepDelay);

                    AppConfig.SleepDelay = _sleepDelay;
                    Properties.Settings.Default.sleep_delay = _sleepDelay;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private float _backgroundLogoOpacity;
        public float BackgroundLogoOpacity
        {
            get
            {
                return this._backgroundLogoOpacity;
            }
            set
            {
                if (_backgroundLogoOpacity != value)
                {
                    _backgroundLogoOpacity = value;

                    Properties.Settings.Default.background_image_opacity = _backgroundLogoOpacity;
                    ((App)Application.Current).BackgroundOpacity = _backgroundLogoOpacity;
                    Properties.Settings.Default.Save();

                    RaisePropertyChanged(() => BackgroundLogoOpacity);
                }
            }
        }
        #endregion

        #region Global Settings

        private string _organisationName;
        public string OrganisationName
        {
            get
            {
                return this._organisationName;
            }
            set
            {
                if (_organisationName != value)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        if (new Dao.Admin.SettingDao().SetSetting("organisation_name", value) > 0)
                        {
                            _organisationName = value;
                            //((App)Application.Current).OrganisationName = value;
                            RaisePropertyChanged(() => OrganisationName);
                        }
                    }
                }
            }
        }

        private string _organisationAddress;
        public string OrganisationAddress
        {
            get
            {
                return this._organisationAddress;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (new Dao.Admin.SettingDao().SetSetting("organisation_address", value) > 0)
                    {
                        _organisationAddress = value;
                        //((App)Application.Current).OrganisationAddress = value;
                        RaisePropertyChanged(() => OrganisationAddress);
                    }
                }
            }
        }

        private byte[] _greatLogo;
        public byte[] GreatLogo
        {
            get
            {
                return this._greatLogo;
            }
            set
            {
                if (_greatLogo != value)
                {
                    if (value != null)
                    {
                        if (new Dao.Admin.SettingDao().SetSetting("great_logo", System.Convert.ToBase64String(value)) > 0)
                        {
                            _greatLogo = value;
                            //((App)Application.Current).GreatLogo = value;
                            RaisePropertyChanged(() => GreatLogo);
                        }
                    }
                }
            }
        }

        private byte[] _reportLogo;
        public byte[] ReportLogo
        {
            get
            {
                return this._reportLogo;
            }
            set
            {
                if (_reportLogo != value)
                {
                    if (value != null)
                    {
                        if (new Dao.Admin.SettingDao().SetSetting("report_logo", System.Convert.ToBase64String(value)) > 0)
                        {
                            _reportLogo = value;
                            RaisePropertyChanged(() => ReportLogo);
                        }
                    }
                }
            }
        }
 

        private byte[] _reportHeader;
        public byte[] ReportHeader
        {
            get
            {
                return this._reportHeader;
            }
            set
            {
                if (_reportHeader != value)
                {
                    if (value != null)
                    {
                        if (new SettingDao().SetSetting("report_header", Convert.ToBase64String(value)) > 0)
                        {
                            _reportHeader = value;
                            RaisePropertyChanged(() => ReportHeader);
                        }
                    }
                }
            }
        }

        private byte[] _reportLargeHeader;
        public byte[] ReportLargeHeader
        {
            get
            {
                return this._reportLargeHeader;
            }
            set
            {
                if (_reportLargeHeader != value)
                {
                    if (value != null)
                    {
                        if (new SettingDao().SetSetting("report_large_header", Convert.ToBase64String(value)) > 0)
                        {
                            _reportLargeHeader = value;
                            RaisePropertyChanged(() => ReportLargeHeader);
                        }
                    }
                }
            }
        }

        private byte[] _reportSmallHeader;
        public byte[] ReportSmallHeader
        {
            get
            {
                return this._reportSmallHeader;
            }
            set
            {
                if (_reportSmallHeader != value)
                {
                    if (value != null)
                    {
                        if (new SettingDao().SetSetting("report_small_header", Convert.ToBase64String(value)) > 0)
                        {
                            _reportSmallHeader = value;
                            RaisePropertyChanged(() => ReportSmallHeader);
                        }
                    }
                }
            }
        }

        private string _stringVATRate;
        public string StringVATRate
        {
            get { return _stringVATRate; }
            set
            {
                if (value != _stringVATRate)
                {
                    _stringVATRate = value;
                    float _v;

                    VATRate = float.TryParse(value, out _v) ? _v : 0;

                    RaisePropertyChanged(() => StringVATRate);
                }
            }
        }

        private float _vatRate;
        public float VATRate
        {
            get
            {
                return this._vatRate;
            }
            set
            {
                if (value != _vatRate && _vatRate >= 0 && _vatRate <= 50)
                {
                    if (new SettingDao().SetSetting("vat_rate", value.ToString()) > 0)
                    {
                        _vatRate = value;
                        AppConfig.VATRate = value;
                        RaisePropertyChanged(() => VATRate);
                    }
                }
            }
        }

        private float _creditDueDelay;
        public float CreditDueDelay
        {
            get
            {
                return this._creditDueDelay;
            }
            set
            {
                if (value != _creditDueDelay && _creditDueDelay >= 0)
                {
                    if (new SettingDao().SetSetting("credit_due_delay", value.ToString()) > 0)
                    {
                        _creditDueDelay = value;
                        AppConfig.CreditDueDelay = value;
                        RaisePropertyChanged(() => CreditDueDelay);
                    }
                }
            }
        }

        private bool _sellWithoutStock;
        public bool SellWithoutStock
        {
            get
            {
                return this._sellWithoutStock;
            }
            set
            {
                if (value != _sellWithoutStock && _creditDueDelay >= 0)
                {
                    if (new SettingDao().SetSetting("sell_without_stock", value.ToString()) > 0)
                    {
                        _sellWithoutStock = value;
                        AppConfig.SellWithoutStock = value;
                        RaisePropertyChanged(() => SellWithoutStock);
                    }
                }
            }
        }
        #endregion

        private bool _isAdmin;
        public bool IsAdmin
        {
            get
            {
                return this._isAdmin;
            }
            set
            {
                if (_isAdmin != value)
                {
                    _isAdmin = value;
                    RaisePropertyChanged(() => IsAdmin);
                }
            }
        }

        #region Commands
        public IAsyncCommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new AsyncCommand(Load);
                }
                return _loadCommand;
            }
        }
        
        private async Task Load(object param = null)
        {
            EntityRight = AppConfig.CurrentUser != null;

            if (!EntityRight)
                return;

            IsAdmin = AppConfig.CurrentUser != null && AppConfig.CurrentUser.Type == UserType.ADMIN;
          
            await LoadGlobalData();
            LoadLocalData();
            await LoadShops();
        }

        private void LoadLocalData()
        {
            DefaultPrintName = Properties.Settings.Default.default_printer_name;
            //var g = Properties.Settings.Default.shop_id;

            //if (!string.IsNullOrWhiteSpace(g))
            //    SelectedShop = new Dao.ShopDao().GetShop(g);
        }

        private async Task LoadGlobalData()
        {
            var dao = new SettingDao();
            
            var vatRate = dao.GetSetting("vat_rate");
            if (vatRate != null)
                StringVATRate = vatRate.Value + "";

            var creditDueDelay = dao.GetSetting("credit_due_delay");
            if (creditDueDelay != null)
                CreditDueDelay = Convert.ToSingle(creditDueDelay.Value);

            var sellWithoutStock = dao.GetSetting("sell_without_stock");
            if (sellWithoutStock != null)
                SellWithoutStock = bool.Parse(sellWithoutStock.Value + "");
            
            var reportHeader = await Task.Run(() => dao.GetSettingAsync("report_header"));
            if (reportHeader != null)
                ReportHeader = !string.IsNullOrWhiteSpace(Convert.ToString(reportHeader.Value)) ? Convert.FromBase64String(reportHeader.Value + "") : null;

            var reportLargeHeader = await Task.Run(() => dao.GetSettingAsync("report_large_header"));
            if (reportLargeHeader != null)
                ReportLargeHeader = !string.IsNullOrWhiteSpace(Convert.ToString(reportLargeHeader.Value)) ? Convert.FromBase64String(reportLargeHeader.Value + "") : null;

            var reportSmallHeader = await Task.Run(() => dao.GetSettingAsync("report_small_header"));
            if (reportSmallHeader != null)
                ReportSmallHeader = !string.IsNullOrWhiteSpace(Convert.ToString(reportSmallHeader.Value)) ? Convert.FromBase64String(reportSmallHeader.Value + "") : null;

            var orgName = dao.GetSetting("organisation_name");
            if (orgName != null)
                OrganisationName = orgName.Value + "";

            var orgAddress = dao.GetSetting("organisation_address");
            if (orgAddress != null)
                OrganisationAddress = orgAddress.Value + "";

            var greatLogo = dao.GetSetting("great_logo");
            if (greatLogo != null)
                GreatLogo = !string.IsNullOrWhiteSpace(System.Convert.ToString(greatLogo.Value)) ? System.Convert.FromBase64String(greatLogo.Value + "") : null;

            var reportLogo = dao.GetSetting("report_logo");
            if (reportLogo != null)
                ReportLogo = !string.IsNullOrWhiteSpace(System.Convert.ToString(reportLogo.Value)) ? System.Convert.FromBase64String(reportLogo.Value + "") : null;

        }

        public IAsyncCommand RefreshShopCommand
        {
            get
            {
                if (_refreshShopCommand == null)
                {
                    _refreshShopCommand = new AsyncCommand(LoadShops, p => EntityRight && !ShopLoading);
                }

                return _refreshShopCommand;
            }
        }

        bool ShopLoading;

        async Task LoadShops(object p = null)
        {
            ShopLoading = true;
            //shops.Clear();

            //var list = await Task.Run(() => new Dao.ShopDao().GetShopsAsync());

            //list.ForEach(g => shops.Add(g));

            //if (SelectedShop != null)
            //    ShopsView.MoveCurrentTo(SelectedShop);
            //else
            //    ShopsView.MoveCurrentToFirst();

            ShopLoading = false;
        }
        
        public string Error
        {
            get
            {
                if (this["VATRate"] != string.Empty)
                    return this["VATRate"];

                return string.Empty;
            }
        }

        public string this[string columnName]
        {
            get
            {
                var error = string.Empty;
                switch (columnName)
                {
                    case "VATRate":
                        if (VATRate < 0 || VATRate > 50)
                            error = "Le taux raisonnable de la TVA doit être compris entre 0 et 50%";
                        
                        break;

                    case "StringVATRate":
                        if (string.IsNullOrWhiteSpace(StringVATRate))
                            error = "Le taux de la TVA ne doit pas être vide.";
                        else
                        {
                            float _v;
                            if (!float.TryParse(StringVATRate, out _v))
                                error = string.Format("La chaîne saisie '{0}' ne peut être convertie en numérique.", StringVATRate);
                        }

                        if (error == string.Empty)
                            error = this["VATRate"];
                        break;
                    default:
                        break;
                }

                return error;
            }
        }
        
        public ICommand ChoosePrinterCommand
        {
            get
            {
                if (_choosePrinterCommand == null)
                    _choosePrinterCommand = new RelayCommand(p => ChoosePrinter(p), p => EntityRight);

                return _choosePrinterCommand;
            }
        }

        private void ChoosePrinter(object p)
        {
            var printer = printerProvider.ChoosePrinter();

            if (!string.IsNullOrWhiteSpace(printer))
                DefaultPrintName = printer;
        }

        public ICommand ChooseReportHeaderCommand
        {
            get
            {
                if (_chooseReportHeaderCommand == null)
                    _chooseReportHeaderCommand = new RelayCommand(p => ChooseReportHeader(1), p => EntityRight);

                return _chooseReportHeaderCommand;
            }
        }

        private void ChooseReportHeader(object p)
        {
            var file = filePathProvider.GetLoadPath("Images |*.png; *.jpg; *.jpeg;");

            if (!string.IsNullOrWhiteSpace(file))
            {
                var img = ImageUtil.ImageFileToByte(file);

                switch (Convert.ToInt32(p))
                {
                    case 1:
                        ReportHeader = img;
                        break;
                    case 2:
                        ReportLargeHeader = img;
                        break;
                    case 0:
                        ReportSmallHeader = img;
                        break;
                    case 3:
                        GreatLogo = img;
                        break;
                    case 4:
                        ReportLogo = img;
                        break;
                    default:
                        break;
                }
            }
        }

        public ICommand ChooseReportLargeHeaderCommand
        {
            get
            {
                if (_chooseReportLargeHeaderCommand == null)
                    _chooseReportLargeHeaderCommand = new RelayCommand(p => ChooseReportHeader(2), p => EntityRight);

                return _chooseReportLargeHeaderCommand;
            }
        }

        public ICommand ChooseReportSmallHeaderCommand
        {
            get
            {
                if (_chooseReportSmallHeaderCommand == null)
                    _chooseReportSmallHeaderCommand = new RelayCommand(p => ChooseReportHeader(0), p => EntityRight);

                return _chooseReportSmallHeaderCommand;
            }
        }

        public ICommand ChooseOrganisationGreatLogoCommand
        {
            get
            {
                if (_chooseOrganisationGreatLogoCommand == null)
                    _chooseOrganisationGreatLogoCommand = new RelayCommand(p => ChooseReportHeader(3), p => EntityRight);

                return _chooseOrganisationGreatLogoCommand;
            }
        }

        public ICommand ChooseOrganisationReportLogoCommand
        {
            get
            {
                if (_chooseOrganisationSmallLogoCommand == null)
                    _chooseOrganisationSmallLogoCommand = new RelayCommand(p => ChooseReportHeader(4), p => EntityRight);

                return _chooseOrganisationSmallLogoCommand;
            }
        }

        private void MenuInit()
        {
            Name = "Paramètres";
            OptionItem = new OptionItem()
            {
                Name = Name,
                ToolTip = Name,
                IconPathData = "M15.799988,7.9000244C11.400024,7.9000244 7.7999878,11.5 7.7999878,15.900024 7.7999878,20.300018 11.400024,23.900024 15.799988,23.900024 20.200012,23.900024 23.799988,20.300018 23.799988,15.900024 23.799988,11.400024 20.299988,7.9000244 15.799988,7.9000244z M12.600037,0L15.5,4.1000061 16.900024,4.1000061 17.100037,4.1000061 20.100037,0.20001221 24.600037,2.2000122 23.799988,7.1000061 23.900024,7.2000122C24.200012,7.6000061,24.600037,8,25,8.4000244L25.100037,8.6000061 30.200012,7.9000244 32,12.5 27.700012,15.600006 27.700012,17 31.799988,20.100006 29.799988,24.600006 24.700012,23.700012 24.600037,23.800018 23.900024,24.5 23.700012,24.700012 24.400024,30.100006 19.700012,32 16.700012,27.700012 16.5,27.700012 15.200012,27.700012 14.799988,27.700012 11.5,31.900024 7,29.900024 7.9000244,24.700012 7.7999878,24.600006C7.5,24.400024,7.2999878,24.100006,7.1000366,23.900024L6.9000244,23.600006 1.7999878,24.300018 0,19.700012 4,16.800018 4,16.5 4,14.800018 0,11.700012 2,7.2000122 7,8C7.5,7.5,8,7,8.5,6.6000061L8,1.8000183z"
            };

            var help = string.Empty;
            App.Tips.TryGetValue(Name, out help);
            Help = help;
        }

        public override void RefreshAccess(bool isOkay)
        {
            EntityRight = IsAccessible = AppConfig.CurrentUser != null;
        }

        private AsyncCommand _loadCommand;
        private AsyncCommand _refreshShopCommand;
        private RelayCommand _saveShopCommand;
        private RelayCommand _chooseOrganisationGreatLogoCommand;
        private RelayCommand _chooseOrganisationSmallLogoCommand;
        private RelayCommand _chooseReportHeaderCommand;
        private RelayCommand _chooseReportSmallHeaderCommand;
        private RelayCommand _chooseReportLargeHeaderCommand;
        private RelayCommand _choosePrinterCommand;
        
        #endregion
    }
}
