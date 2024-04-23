using System;
using System.Windows.Input;
using System.Windows.Threading;
using FingerPrintManagerApp.ViewModel.Command;
using FingerPrintManagerApp.ViewModel;
using libzkfpcsharp;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using FingerPrintManagerApp.Model.Employe;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using FingerPrintManagerApp.Model;
using DPFP.Capture;
using CrystalDecisions.CrystalReports.ViewerObjectModel;
using DPFP.Error;
using FingerPrintManagerApp.Model.Helper;
using DPFP;
using System.Globalization;
using CrystalDecisions.Shared;

namespace FingerPrintManagerApp.Modules.Presence.ViewModel
{
    public class PresenceWindowViewModel : ViewModelBase, DPFP.Capture.EventHandler
    {

        DPFP.Capture.Capture _capturer;
        DPFP.Processing.Enrollment _enroller;

        DispatcherTimer timer;
        private SpeechSynthesizer Parleur { get; set; }

        public PresenceWindowViewModel()
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += timer_Tick;

            timer.Start();
            timer.IsEnabled = false;

            Salutation = "Salut,";
            ScanStatus = "";
            Message = "Utilisez le scanneur pour vous identifier !";

            SetupSpeaker();
        }

        bool inCaptureRange = false;

        private void timer_Tick(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now;

            if (!initDeviceSucceeded)
                InitDevice();

            if (DateTime.Now.Hour < 12) {
                if (DateTime.Now.Hour < AppConfig.MorningPresenceEndHour)
                    inCaptureRange = true;
                else if (DateTime.Now.Hour > AppConfig.MorningPresenceEndHour)
                    inCaptureRange = false;
                else
                    inCaptureRange = DateTime.Now.Minute <= AppConfig.MorningPresenceEndMinute;

            }
            else
            {
                if (DateTime.Now.Hour > AppConfig.EveningPresenceStartHour)
                    inCaptureRange = true;
                else if (DateTime.Now.Hour < AppConfig.EveningPresenceStartHour)
                    inCaptureRange = false;
                else
                    inCaptureRange = DateTime.Now.Minute >= AppConfig.EveningPresenceStartMinute;
            }
        }

        void SetupSpeaker()
        {
            Parleur = new SpeechSynthesizer();

            try
            {
                Parleur.SelectVoice("Microsoft Hortense Desktop");
                Parleur.Volume = 100;
            }
            catch (Exception)
            {
            }
        }

        void Speak(string msg)
        {
           
            Thread.Sleep(0);
            Parleur.SpeakAsync(msg);
        }

        #region Printer Fields
        IntPtr mDevHandle = IntPtr.Zero;
        IntPtr mDBHandle = IntPtr.Zero;

        byte[] FPBuffer;
        int RegisterCount = 0;
        const int REGISTER_FINGER_COUNT = 3;

        byte[][] RegTmps = new byte[3][];
        byte[] RegTmp = new byte[2048];
        byte[] CapTmp = new byte[2048];

        int cbCapTmp = 2048;
        int cbRegTmp = 0;

        private bool canPrint = false;

        private int mfpWidth = 0;
        private int mfpHeight = 0;
        private int mfpDpi = 0;

        const int MESSAGE_CAPTURED_OK = 0x0400 + 6;

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
        #endregion

        private bool _deviceSucceeded;
        private bool initDeviceSucceeded;
        private bool _deviceFailed;

        public bool DeviceSucceeded
        {
            get
            {
                return this._deviceSucceeded;
            }
            set
            {
                if (_deviceSucceeded != value)
                {
                    _deviceSucceeded = value;
                    RaisePropertyChanged(() => DeviceSucceeded);
                }
            }
        }
        public bool DeviceFailed
        {
            get
            {
                return this._deviceFailed;
            }
            set
            {
                if (_deviceFailed != value)
                {
                    _deviceFailed = value;
                    RaisePropertyChanged(() => DeviceFailed);
                }
            }
        }

        private string _scanStatus;
        private DateTime _currentTime;
        private string _salutation;
        private string _message;
        private Model.Employe.Employe _employe;
        private Model.Presence.Periode _currentPeriode;

        public string Salutation
        {
            get { return _salutation; }
            set
            {
                if (_salutation != value)
                {
                    _salutation = value;
                    RaisePropertyChanged(() => Salutation);
                }
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    RaisePropertyChanged(() => Message);
                }
            }
        }
        public Model.Employe.Employe Employe
        {
            get { return _employe; }
            set
            {
                if (_employe != value)
                {
                    _employe = value;
                    RaisePropertyChanged(() => Employe);
                }
            }
        }
        public Model.Presence.Periode CurrentPeriode
        {
            get { return _currentPeriode; }
            set
            {
                if (_currentPeriode != value)
                {
                    _currentPeriode = value;
                    RaisePropertyChanged(() => CurrentPeriode);
                }
            }
        }
        private Model.Presence.Presence Presence { get; set; }
        public DateTime CurrentTime
        {
            get { return _currentTime; }
            set
            {
                if (_currentTime != value)
                {
                    _currentTime = value;
                    RaisePropertyChanged(() => CurrentTime);
                }
            }
        }
        public bool DejaPointe { get; set; }

        public string ScanStatus
        {
            get { return _scanStatus; }
            set
            {
                if (_scanStatus != value)
                {
                    _scanStatus = value;
                    RaisePropertyChanged(() => ScanStatus);
                }
            }
        }


        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                    _loadCommand = new AsyncCommand(p => Load());

                return _loadCommand;
            }
        }

        private async Task Load()
        {
            CurrentTime = DateTime.Now;

            timer.Start();
            timer.IsEnabled = true;

            CurrentPeriode = await new Dao.Presence.PeriodeDao().GetAsync(DateTime.Today);

            InitDevice();
        }

        //void InitDevice()
        //{
        //    initDeviceSucceeded = false;
        //    int ret = zkfperrdef.ZKFP_ERR_OK;
        //    if ((ret = zkfp2.Init()) == zkfperrdef.ZKFP_ERR_OK)
        //    {
        //        int nCount = zkfp2.GetDeviceCount();
        //        if (nCount > 0)
        //        {
        //            ret = zkfp.ZKFP_ERR_OK;
        //            if (IntPtr.Zero == (mDevHandle = zkfp2.OpenDevice(0)))
        //            {
        //                Status = "L'ouverture de l'appareil a échoué !";
        //                DeviceFailed = true;
        //                return;
        //            }
        //            if (IntPtr.Zero == (mDBHandle = zkfp2.DBInit()))
        //            {
        //                DeviceFailed = true;
        //                //MessageBox.Show("Init DB fail");
        //                zkfp2.CloseDevice(mDevHandle);
        //                mDevHandle = IntPtr.Zero;
        //                return;
        //            }

        //            RegisterCount = 0;
        //            cbRegTmp = 0;

        //            for (int i = 0; i < 3; i++)
        //                RegTmps[i] = new byte[2048];

        //            byte[] paramValue = new byte[4];
        //            int size = 4;
        //            zkfp2.GetParameters(mDevHandle, 1, paramValue, ref size);
        //            zkfp2.ByteArray2Int(paramValue, ref mfpWidth);

        //            size = 4;
        //            zkfp2.GetParameters(mDevHandle, 2, paramValue, ref size);
        //            zkfp2.ByteArray2Int(paramValue, ref mfpHeight);

        //            FPBuffer = new byte[mfpWidth * mfpHeight];

        //            size = 4;
        //            zkfp2.GetParameters(mDevHandle, 3, paramValue, ref size);
        //            zkfp2.ByteArray2Int(paramValue, ref mfpDpi);

        //            Thread captureThread = new Thread(new ThreadStart(DoCapture));
        //            captureThread.IsBackground = true;
        //            captureThread.Start();
        //            canPrint = true;

        //            initDeviceSucceeded = true;

        //            DeviceSucceeded = true;
        //        }
        //        else
        //        {
        //            zkfp2.Terminate();
        //            Status = "Aucun appareil n'est connecté.";
        //        }
        //    }
        //    else
        //    {
        //        DeviceFailed = true;
        //        Status = "L'initialisation de l'appreil a échoué !";
        //    }
        //}
        void InitDevice()
        {
            DeviceFailed = DeviceSucceeded = false;

            _capturer = new DPFP.Capture.Capture();
            _enroller = new DPFP.Processing.Enrollment();
            _capturer.EventHandler = this;

            if (_capturer == null)
                DeviceFailed = true;
            else
            {
                _capturer.StartCapture();
                canPrint = true;
                initDeviceSucceeded = true;
            }
        }
        private void DoCapture()
        {
            while (canPrint && inCaptureRange)
            {
                try
                {
                    cbCapTmp = 2048;
                    int ret = zkfp2.AcquireFingerprint(mDevHandle, FPBuffer, CapTmp, ref cbCapTmp);

                    if (ret == zkfp.ZKFP_ERR_OK)
                        CallBack(MESSAGE_CAPTURED_OK);

                    Thread.Sleep(200);
                }
                catch (Exception)
                {
                }
            }
        }

        void CallBack(int msg)
        {
            switch (msg)
            {
                case MESSAGE_CAPTURED_OK:
                    {
                        canPrint = false;
                        Status = ScanStatus = string.Empty;

                        var ms = new MemoryStream();
                        Sample.BitmapFormat.GetBitmap(FPBuffer, mfpWidth, mfpHeight, ref ms);
                        
                        //Identify(CapTmp);

                        canPrint = true;

                    }
                    break;

                default:
                    break;
            }
        }



        private async Task Process(DPFP.Sample Sample)
        {
            try
            {
                bool failMsg = true;
                Status = ScanStatus = string.Empty;

                CultureInfo culture = new CultureInfo("fr-FR", true);
                var date = DateTime.Now;
                var jour = date.ToString("dddd", culture);

                var horaire = AppConfig.HoraireTravailSemaines.Find(h => h.Jour == jour);

                //Verifions si aujourd'hui est un jour ouvrable
                if (horaire.EstOuvrable)
                {
                    failMsg = false;
                    var exception = AppConfig.DateExceptions.Find(d => d.ToString() == date.ToString("ddMM"));

                    if (exception == null) //la date d'aujourd'hui n'est pas ferié
                    {
                        failMsg = false;

                        var count = new Dao.Employe.EmployeEmpreinteDao().Count();

                        Model.Employe.Employe employe = null;

                        if (count > 0)
                        {
                            int k = 0;
                            int limit = 5;

                            bool found = false;

                            while (k < count)
                            {
                                var empreintes = await new Dao.Employe.EmployeEmpreinteDao().GetAllAsync(k, limit);
                                foreach (var empreinte in empreintes)
                                {
                                    if (FingerPrintUtil.verifiy(Sample, empreinte.Template))
                                    {
                                        employe = empreinte.Employe;
                                        found = true;
                                        break;
                                    }
                                }

                                if (found)
                                    break;

                                k += limit;
                            }
                        }

                        if (employe != null)
                        {
                            failMsg = false;

                            Presence = await new Dao.Presence.PresenceDao().GetAsync(employe, DateTime.Today);

                            if (DateTime.Now.Hour <= horaire.ClotureHeureMatin)
                            {
                                DejaPointe = Presence != null;

                                if (DejaPointe)
                                    await ErrorMsgVoice("Vous avez déjà pointé pour l'arrivé !", true) ;
                                else
                                {
                                    var presence = new Model.Presence.Presence()
                                    {
                                        Employe = employe,
                                        Periode = CurrentPeriode,
                                        Date = DateTime.Today,
                                        HeureArrivee = DateTime.Now,
                                        Mode = Model.Presence.ModePointage.Empreinte
                                    };

                                    var feed = await new Dao.Presence.PresenceDao().AddAsync(presence);

                                    if (feed > 0)
                                        await Pointe(employe);
                                }
                            }
                            else
                            {
                                if (Presence != null)
                                {
                                    if (DateTime.Now.Hour >= horaire.DebutHeureSoir)
                                    {
                                        DejaPointe = new Dao.Presence.PresenceDao().CheckDepaturePointage(Presence);

                                        if (DejaPointe)
                                            await ErrorMsgVoice("Vous avez déjà pointé pour la sortie!", true) ;
                                        else
                                        {
                                            Presence.HeureDepart = DateTime.Now;

                                            var feed = await new Dao.Presence.PresenceDao().MarkDeparture(Presence);

                                            if (feed > 0)
                                                await Pointe(employe);
                                        }
                                    }
                                    else
                                        Message = $"{employe.Name} il n'est pas encore l'heure de partir";
                                }
                            }
                        }
                        else
                        {
                            await ErrorMsgVoice("Empreinte non reconnue dans le système !", true);
                        }
                    }
                    else
                        Message = $"{exception.Date.ToString("dd-MM-yyyy")} est une date fériée ";

                }
                else
                    Message = $"{horaire.Jour} n'est pas un jour ouvrable";


                if (failMsg)
                    ScanStatus = "Fail";
            }
            catch (Exception)
            {
            }
        }

        async Task ErrorMsgVoice(string msg, bool withVoice = false)
        {
            ScanStatus = "Fail";
            Message = msg;

            if(withVoice)
                Speak(msg);
            await Task.Delay(5000);
            Message = "Utilisez le scanneur pour vous identifier !";
        }

        async Task Pointe(Model.Employe.Employe employe)
        {

            CultureInfo culture = new CultureInfo("fr-FR", true);
            var date = DateTime.Now;
            var jour = date.ToString("dddd", culture);

            var horaire = AppConfig.HoraireTravailSemaines.Find(h => h.Jour == jour);

            Salutation = DateTime.Now.Hour < horaire.ClotureHeureMatin ? "Bienvenu" : "Au revoir";
            var endWord = DateTime.Now.Hour < horaire.ClotureHeureMatin ? "Bon service." : "Bonne soirée.";
            var civility = employe.Sexe == Model.Sex.Femme ? "Madame" : "Monsieur";

            Employe = employe;

            Message = Employe.Name;

            var msg = string.Format("{0} {1} {2}, {3}", Salutation, civility, Employe.Prenom, endWord);

            ScanStatus = "Success";

            Speak(msg);

            await Task.Delay(5000);
            Employe = null;
            Presence = null;
            DejaPointe = false;
            Salutation = "Salut";
            Message = "Utilisez le scanneur pour vous identifier !";
        }

        public ICommand ClosingCommand
        {
            get
            {
                if (_closingCommand == null)
                    _closingCommand = new RelayCommand(p => Close(p));

                return _closingCommand;
            }
        }

        private void Close(object param)
        {
            canPrint = false;
            Thread.Sleep(1000);

            try
            {
                zkfp2.Terminate();
                zkfp2.CloseDevice(mDevHandle);
            }
            catch (Exception)
            {
            }
        }

        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
        {
            Task.Run(async () =>
            {
                await Process(Sample);
            });    
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            initDeviceSucceeded = true;
            DeviceFailed = false;
            DeviceSucceeded = !DeviceFailed;
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            DeviceFailed = true;
            DeviceSucceeded = !DeviceFailed;
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, CaptureFeedback CaptureFeedback)
        {
            throw new NotImplementedException();
        }

        private AsyncCommand _loadCommand;
        private RelayCommand _closingCommand;
    }
}
