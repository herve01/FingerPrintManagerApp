using DPFP.Capture;
using DPFP.Error;
using FingerPrintManagerApp.Dialog.Service;
using FingerPrintManagerApp.Model.Employe;
using FingerPrintManagerApp.Model.Helper;
using FingerPrintManagerApp.ViewModel.Command;
using libzkfpcsharp;
using Sample;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace FingerPrintManagerApp.Modules.Employe.ViewModel
{
    public class FingerPrintViewModel : DialogViewModelBase, DPFP.Capture.EventHandler
    {
        DPFP.Capture.Capture _capturer;
        DPFP.Processing.Enrollment _enroller;

        private object _lock = new object();

        private ObservableCollection<EmployeEmpreinte> empreintes;
        private ObservableCollection<string> doigts;

        public ICollectionView EmpreintesView { get; private set; }
        public ICollectionView DoigtsView { get; private set; }

        SynchronizationContext ViewContext;

        public FingerPrintViewModel(Model.Employe.Employe employe)
        {
            ViewContext = SynchronizationContext.Current;

            Employe = employe;

            empreintes = new ObservableCollection<EmployeEmpreinte>();
            BindingOperations.EnableCollectionSynchronization(empreintes, _lock);

            EmpreintesView = (CollectionView)CollectionViewSource.GetDefaultView(empreintes);
            
            doigts = new ObservableCollection<string>()
            {
                "Pouce droit",
                "Pouce gauche",
                "Index droit",
                "Index gauche",
                "Majeur droit",
                "Majeur gauche",
                "Annulaire droit",
                "Annulaire gauche",
                "Auriculaire droit",
                "Auriculaire gauche"
            };
            DoigtsView = (CollectionView)CollectionViewSource.GetDefaultView(doigts);
            DoigtsView.Filter = OnDoigtFilter;

            CurrentFingerIndex = employe.Empreintes.Count;

            Doigt = doigts[CurrentFingerIndex];

         


        }

        private bool OnDoigtFilter(object obj)
        {
            var doigt = obj as string;

            if (doigt == null)
                return false;

            var find = Employe.Empreintes.Find(e => e.Doigt == doigt);

            return find == null;
        }

        #region Printer Fields
        IntPtr mDevHandle = IntPtr.Zero;
        IntPtr mDBHandle = IntPtr.Zero;

        byte[] FPBuffer;
        int RegisterCount = 0;
        const int REGISTER_FINGER_COUNT = 4;

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

        private Model.Employe.Employe _employe;
        private string _action;
        private bool _deviceSucceeded;
        private bool initDeviceSucceeded;
        private bool _deviceFailed;
        private bool _allScanned;
        private bool _nextFinger;
        private int _count;
        private byte[] _printFingerImage;
        private int CurrentFingerIndex;
        private string _doigt;

        public string Action
        {
            get
            {
                return this._action;
            }
            set
            {
                if (_action != value)
                {
                    _action = value;
                    RaisePropertyChanged(() => Action);
                }
            }
        }
        public string Doigt
        {
            get
            {
                return this._doigt;
            }
            set
            {
                if (_doigt != value)
                {
                    _doigt = value;
                    RaisePropertyChanged(() => Doigt);

                    if (!string.IsNullOrWhiteSpace(Doigt))
                    {
                        ScanCount = REGISTER_FINGER_COUNT;
                        RegisterCount = 0;
                        PrintFingerImage = null;
                    }
                }
            }
        }

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

        public bool AllScanned
        {
            get
            {
                return this._allScanned;
            }
            set
            {
                if (_allScanned != value)
                {
                    _allScanned = value;
                    RaisePropertyChanged(() => AllScanned);
                }
            }
        }

        public int ScanCount
        {
            get
            {
                return this._count;
            }
            set
            {
                if (_count != value)
                {
                    _count = value;
                    RaisePropertyChanged(() => ScanCount);
                }
            }
        }
        public byte[] PrintFingerImage
        {
            get
            {
                return this._printFingerImage;
            }
            set
            {
                if (_printFingerImage != value)
                {
                    _printFingerImage = value;
                    RaisePropertyChanged(() => PrintFingerImage);
                }
            }
        }
        public bool NextFinger
        {
            get
            {
                return this._nextFinger;
            }
            set
            {
                if (_nextFinger != value)
                {
                    _nextFinger = value;
                    RaisePropertyChanged(() => NextFinger);

                    if (NextFinger)
                    {
                        CurrentFingerIndex++;
                        if (CurrentFingerIndex < doigts.Count)
                            Doigt = doigts[CurrentFingerIndex];

                        //DoigtsView.Refresh();
                    }
                }
            }
        }
        public Model.Employe.Employe Employe
        {
            get
            {
                return this._employe;
            }
            set
            {
                if (_employe != value)
                {
                    _employe = value;
                    RaisePropertyChanged(() => Employe);
                }
            }
        }

        #region Commands
        
        protected override async Task Load(object param)
        {
            empreintes.Clear();

            Employe.Empreintes.ForEach(e => empreintes.Add(e));

            DeviceSucceeded = DeviceFailed = AllScanned = false;
            InitDevice();
        }


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
                ScanCount = REGISTER_FINGER_COUNT;
                initDeviceSucceeded = true;

                AllScanned = Employe.Empreintes.Count == doigts.Count;

                if (!AllScanned)
                    DeviceSucceeded = true;
            }
        }

        byte[] saveFinger;
        void Process(DPFP.Sample Sample)
        {
            try
            {
                var bmp = FingerPrintUtil.ConvertSampleToBitmap(Sample);
                PrintFingerImage = ImageUtil.BitmapToByte(bmp);

                foreach (var empreinte in Employe.Empreintes) //ici nous verifions si le doigt inserer existe déjà
                {
                    if(FingerPrintUtil.verifiy(Sample, empreinte.Template))
                    {
                        Status = string.Format("Ce doigt << {0} >> est déjà enregistré !", empreinte.Doigt);
                        return;
                    }
                }


                //if (RegisterCount > 0 && !FingerPrintUtil.verifiy(Sample, saveFinger)) //Ici on s'assure que la personne introduit le même doigt
                //{
                //    Status = string.Format("Veuillez appuyer 4 fois le même doit << {0} >> pour l'enrôlement. Recommencons !", Doigt);
                //    RegisterCount = 0;
                //    ScanCount = REGISTER_FINGER_COUNT;
                //    return;
                //}

                RegisterCount++;

                var features = FingerPrintUtil.ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Enrollment);

                if (features != null)
                {
                    _enroller.AddFeatures(features);
                    ScanCount--;
                }

                if (RegisterCount >= REGISTER_FINGER_COUNT)
                {
                    RegisterCount = 0;
                    Status = string.Empty;
                    NextFinger = false;

                    if (_enroller.TemplateStatus == DPFP.Processing.Enrollment.Status.Ready)
                    {
                        try
                        {
                            MemoryStream s = new MemoryStream();
                            _enroller.Template.Serialize(s);

                            var empreinte = new EmployeEmpreinte()
                            {
                                Employe = Employe,
                                Image = PrintFingerImage,
                                Template = s.ToArray(),
                                Size = features.Size,
                                Finger = Dao.Employe.Util.ToFingers(Doigt)
                            };

                            Employe.Empreintes.Add(empreinte);
                            empreintes.Add(empreinte);

                            ViewContext.Send(x => {
                                DoigtsView.Refresh();
                            }, null);

                            AllScanned = empreintes.Count == doigts.Count;
                            DeviceSucceeded = canPrint = !AllScanned;

                            NextFinger = true;

                            ScanCount = REGISTER_FINGER_COUNT;
                        }
                        catch (SDKException)
                        {
                            //MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(p => Save(), p => CanSave());

                return _saveCommand;
            }
        }

        private void Save()
        {
        }

        private bool CanSave()
        {
            return true;
        }

        protected override void Close(object param)
        {
            canPrint = false;
            Thread.Sleep(1000);

            try
            {
                _capturer.StopCapture();
            }
            catch (Exception)
            {
            }
            
            CloseDialogWithResult(param as Window, Dialog.Service.DialogResult.No);
        }

        public ICommand TryAgainCommand
        {
            get
            {
                if (_tryAgainCommand == null)
                    _tryAgainCommand = new RelayCommand(p => InitDevice());

                return _tryAgainCommand;
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                if (_removeCommand == null)
                    _removeCommand = new RelayCommand(p => Remove(p));

                return _removeCommand;
            }
        }

        private void Remove(object p)
        {
            var empreinte = p as EmployeEmpreinte;

            if (empreinte == null)
                return;

            empreintes.Remove(empreinte);
            Employe.Empreintes.Remove(empreinte);

            DoigtsView.Refresh();

            DoigtsView.MoveCurrentToFirst();

            if (initDeviceSucceeded)
            {
                AllScanned = false;
                DeviceSucceeded = canPrint = true;
            }
        }

        private async Task<EmployeEmpreinte> Identify(byte[] template)
        {
            var count = new Dao.Employe.EmployeEmpreinteDao().Count();

            EmployeEmpreinte empreinte = null;

            if (count > 0)
            {
                int k = 0;
                int limit = 5;

                bool found = false;

                while (k < count)
                {
                    var empreintes = await new Dao.Employe.EmployeEmpreinteDao().GetAllAsync(k, limit);
                    foreach (var print in empreintes)
                    {
                        var score = zkfp2.DBMatch(mDBHandle, template, print.Template);
                        if (score >= 90)
                        {
                            empreinte = print;
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        break;

                    k += limit;
                }

                    
            }

            return empreinte;
        }

        public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
        {
            Process(Sample);
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            //throw new NotImplementedException();
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            //throw new NotImplementedException();
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
            if (CaptureFeedback == DPFP.Capture.CaptureFeedback.Good) ;
            //("The quality of the fingerprint sample is good.");
            else;
            //("The quality of the fingerprint sample is poor.");
        }

        private RelayCommand _saveCommand;
        private RelayCommand _tryAgainCommand;
        private RelayCommand _removeCommand;

        #endregion

    }
}
