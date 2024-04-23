using FingerPrintManagerApp.Dao;
using FingerPrintManagerApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FingerPrintManagerApp
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application, INotifyPropertyChanged
    {
        public static Dictionary<string, string> Tips;
        public static string HelpPath;

        public App()
        {
            DbConfig.DbInvariant = FingerPrintManagerApp.Properties.Settings.Default.local_db_invariant;
            DbConfig.DbName = FingerPrintManagerApp.Properties.Settings.Default.local_dbname;
            DbConfig.DbUser = FingerPrintManagerApp.Properties.Settings.Default.local_user;
            DbConfig.DbPassword = FingerPrintManagerApp.Properties.Settings.Default.local_pwd;
            DbConfig.ServerName = FingerPrintManagerApp.Properties.Settings.Default.local_server;
            DbConfig.DbPort = FingerPrintManagerApp.Properties.Settings.Default.local_port;
            AppConfig.LocalCountryCode = FingerPrintManagerApp.Properties.Settings.Default.local_country_code;
            //SetupReportInfo();

            LoadTips(HelpPath);

            InitConfData();
        }


        void InitConfData()
        {
            if (ConnectionHelper.GetConnection() != null)
            {
                //AppConfig.MainCurrency = new Dao.Financial.CurrencyDao().GetMainCurrency();

                AppConfig.HoraireTravailSemaines = new Dao.Presence.HoraireTravailSemaineDao().GetAll();
                AppConfig.DateExceptions = new Dao.Presence.DateExceptionDao().GetAll();

            }
        }

        private void LoadTips(string fileName)
        {
            Tips = new Dictionary<string, string>();

            //try
            //{
            //    XElement.Load(fileName).Elements("entity").ToList().ForEach(e => Tips.Add(e.Element("id").Value, e.Element("help").ToString()
            //        .Replace("<help>", "")
            //        .Replace("</help>", "")
            //        .Replace("\r\n", "")
            //        .Replace("\t", "")
            //        .Replace("  ", " ")
            //        .Replace("  ", " ")));
            //}
            //catch (Exception)
            //{
            //}
        }

        private Model.Admin.User _user;
        public Model.Admin.User User
        {
            get
            {
                return _user;
            }
            set
            {
                if (value != _user)
                {
                    _user = value;
                    AppConfig.CurrentUser = _user;
                    NotifyPropertyChanged();
                }
            }
        }

        private float _backgroundOpacity;
        public float BackgroundOpacity
        {
            get
            {
                return _backgroundOpacity;
            }
            set
            {
                if (_backgroundOpacity != value)
                {
                    _backgroundOpacity = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static DataTable ReportInfo { get; set; }

        void SetupReportInfo()
        {
            var header = View.Helper.ImageUtil.BitmapImageToByte(new BitmapImage(new Uri("pack://application:,,,/View/Image/arg_header.png")));

            var dao = new Dao.Admin.SettingDao();
            var org = dao.GetSetting("organisation_name")?.Value;
            var address = dao.GetSetting("organisation_address")?.Value;
            var portraitHeader = dao.GetSetting("report_header")?.Value;
            var landscapeHeader = dao.GetSetting("report_large_header")?.Value;
            var invoiceHeader = dao.GetSetting("report_small_header")?.Value;

            var dic = new Dictionary<string, object>()
            {
                { "warehouse_id", "00001" },
                { "firm_name", org == null ? "" : org + "" },
                { "firm_address", address == null ? "" : address + "" },
                { "report_header", portraitHeader == null ? null : (byte[])header }
            };

            ReportInfo = DbUtil.DicToTable(new List<Dictionary<string, object>>() { dic });
            ReportInfo.TableName = "ReportInfo";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception.StackTrace.Contains("<GetTooltipData>"))
            {
                e.Handled = true;
                return;
            }
        }
    }
}
