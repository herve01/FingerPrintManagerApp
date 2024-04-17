//using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Media.Imaging;

namespace FingerPrintManagerApp.Model
{
    public class AppConfig
    {
        private AppConfig()
        {
            LoadHeader();
            LoadLogo();
        }

        static void LoadHeader()
        {
            ReportHeader = View.Helper.ImageUtil.BitmapImageToByte(new BitmapImage(new Uri("pack://application:,,,/View/Image/enteteA4.jpg")));
            ReportLargeHeader = View.Helper.ImageUtil.BitmapImageToByte(new BitmapImage(new Uri("pack://application:,,,/View/Image/enteteA4_Paysage.jpg")));
        }

        static void LoadLogo()
        {
            FirmLogo = View.Helper.ImageUtil.BitmapImageToByte(new BitmapImage(new Uri("pack://application:,,,/View/Image/Logo.jpg")));
        }

        public static string LocalCountryCode { get; set; }

        public static string EntiteId { get; set; }
        //public static Entite CurrentUserEntite { get; set; }
        //public static Financial.Currency MainCurrency { get; set; }
        public static Admin.User CurrentUser { get; set; }
        public static float SleepDelay = 15;
        public static int RetraiteAgeLimit = 65;
        public static int RetraiteServiceLimit = 35;

        public static float VATRate = 16F;
        public static float CreditDueDelay = 1;
        public static bool SellWithoutStock = false;

        public static string DefaultPrinterName { get; set; }
        public static byte[] DefaultProductImage { get; set; }
        public static byte[] ReportHeader { get; set; }
        public static byte[] FirmLogo { get; set; }
        public static byte[] ReportLargeHeader { get; set; }
        public static byte[] ReportSmallHeader { get; set; }

        private static byte[] _signatureDG;
        public static byte[] SignatureDG
        {
            get
            {
                if (_signatureDG == null)
                    _signatureDG = View.Helper.ImageUtil.BitmapImageToByte(new BitmapImage(new Uri("pack://application:,,,/View/Image/signature.png")));

                return _signatureDG;
            }
        }

        private static byte[] _serviceCardHeader;
        public static byte[] ServiceCardHeader
        {
            get
            {
                if (_serviceCardHeader == null)
                    _serviceCardHeader = View.Helper.ImageUtil.BitmapImageToByte(new BitmapImage(new Uri("pack://application:,,,/View/Image/entete_carte_service.png")));

                return _serviceCardHeader;
            }
        }

        private static byte[] _manAvatar;
        public static byte[] ManAvatar
        {
            get
            {
                if (_manAvatar == null)
                    _manAvatar = View.Helper.ImageUtil.BitmapImageToByte(new BitmapImage(new Uri("pack://application:,,,/View/Image/man_avatar.png")));

                return _manAvatar;
            }
        }

        private static byte[] _womanAvatar;
        public static byte[] WomanAvatar
        {
            get
            {
                if (_womanAvatar == null)
                    _womanAvatar = View.Helper.ImageUtil.BitmapImageToByte(new BitmapImage(new Uri("pack://application:,,,/View/Image/woman_avatar.png")));

                return _womanAvatar;
            }
        }

        public static int MorningPresenceEndHour = 8;
        public static int MorningPresenceEndMinute = 30;
        public static int EveningPresenceStartHour = 15;
        public static int EveningPresenceStartMinute = 0;

        public static DataTable GetReportInfo()
        {
            if (ReportHeader == null || ReportLargeHeader == null)
                LoadHeader();
            if (FirmLogo == null)
                LoadLogo();

            var dic = new Dictionary<string, object>()
            {
                { "entite_id", "000001" },
                { "firm_name", "SENAPI" },
                { "firm_address", "43, Avenue des forces armées, Gombe / Kinshasa" },
                { "report_header", ReportHeader },
                { "firm_logo", FirmLogo }
            };

            //var table = Dao.DbUtil.DicToTable(new List<Dictionary<string, object>>() { dic });
            //table.TableName = "ReportInfo";

            return null;
        }
    }
}
