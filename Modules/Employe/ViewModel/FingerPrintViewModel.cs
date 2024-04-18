using FingerPrintManagerApp.Dialog.Service;
using FingerPrintManagerApp.Model.Employe;
using FingerPrintManagerApp.Model.Helper;
using FingerPrintManagerApp.ViewModel.Command;
using libzkfpcsharp;
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
using System.Windows.Input;

namespace FingerPrintManagerApp.Modules.Employe.ViewModel
{
    public class FingerPrintViewModel : DialogViewModelBase
    {
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

            int ret = zkfperrdef.ZKFP_ERR_OK;
            if ((ret = zkfp2.Init()) == zkfperrdef.ZKFP_ERR_OK)
            {
                int nCount = zkfp2.GetDeviceCount();
                if (nCount > 0)
                {
                    ret = zkfp.ZKFP_ERR_OK;
                    if (IntPtr.Zero == (mDevHandle = zkfp2.OpenDevice(0)))
                    {
                        //Status = "L'ouverture de l'appareil a échoué !";
                        DeviceFailed = true;
                        return;
                    }
                    if (IntPtr.Zero == (mDBHandle = zkfp2.DBInit()))
                    {
                        DeviceFailed = true;
                        //MessageBox.Show("Init DB fail");
                        zkfp2.CloseDevice(mDevHandle);
                        mDevHandle = IntPtr.Zero;
                        return;
                    }

                    RegisterCount = 0;
                    cbRegTmp = 0;

                    for (int i = 0; i < 3; i++)
                        RegTmps[i] = new byte[2048];

                    byte[] paramValue = new byte[4];
                    int size = 4;
                    zkfp2.GetParameters(mDevHandle, 1, paramValue, ref size);
                    zkfp2.ByteArray2Int(paramValue, ref mfpWidth);

                    size = 4;
                    zkfp2.GetParameters(mDevHandle, 2, paramValue, ref size);
                    zkfp2.ByteArray2Int(paramValue, ref mfpHeight);

                    FPBuffer = new byte[mfpWidth * mfpHeight];

                    size = 4;
                    zkfp2.GetParameters(mDevHandle, 3, paramValue, ref size);
                    zkfp2.ByteArray2Int(paramValue, ref mfpDpi);

                    Thread captureThread = new Thread(new ThreadStart(DoCapture));
                    captureThread.IsBackground = true;
                    captureThread.Start();
                    canPrint = true;
                    ScanCount = REGISTER_FINGER_COUNT;

                    initDeviceSucceeded = true;

                    AllScanned = Employe.Empreintes.Count == doigts.Count;

                    if (!AllScanned)
                        DeviceSucceeded = true;
                }
                else
                {
                    zkfp2.Terminate();
                    //Status = "Aucun appareil n'est connecté.";
                }
            }
            else
            {
                DeviceFailed = true;
                //Status = "L'initialisation de l'appreil a échoué !";
            }
        }

        private void DoCapture()
        {
            while (canPrint)
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
                        Status = string.Empty;

                        var ms = new MemoryStream();
                        Sample.BitmapFormat.GetBitmap(FPBuffer, mfpWidth, mfpHeight, ref ms);
                        var bmp = new Bitmap(ms);
                        PrintFingerImage = ImageUtil.BitmapToByte(bmp);

                        foreach (var empreinte in Employe.Empreintes)
                        {
                            var score = zkfp2.DBMatch(mDBHandle, CapTmp, empreinte.Template);
                            if (score >= 90)
                            {
                                Status = string.Format("Ce doigt << {0} >> est déjà enregistré !", empreinte.Doigt);
                                return;
                            }
                        }

                        var found = Identify(CapTmp).Result;

                        if (found != null)
                        {
                            Status = string.Format("Cette empreinte est déjà enregistrée. Elle identifie : << {0} >>", found.Employe.Name);
                            return;
                        }

                        if (RegisterCount > 0 && zkfp2.DBMatch(mDBHandle, CapTmp, RegTmps[RegisterCount - 1]) <= 0)
                        {
                            Status = string.Format("Veuillez appuyer 3 fois le même doit << {0} >> pour l'enrôlement. Recommencons !", Doigt);
                            RegisterCount = 0;
                            ScanCount = REGISTER_FINGER_COUNT;
                            return;
                        } 

                        Array.Copy(CapTmp, RegTmps[RegisterCount], cbCapTmp);
                        RegisterCount++;

                        int ret = zkfp.ZKFP_ERR_OK;

                        if (RegisterCount >= REGISTER_FINGER_COUNT)
                        {
                            RegisterCount = 0;
                            Status = string.Empty;
                            NextFinger = false;

                            if (zkfp.ZKFP_ERR_OK == (ret = zkfp2.DBMerge(mDBHandle, RegTmps[0], RegTmps[1], RegTmps[2], RegTmp, ref cbRegTmp)))
                            {
                                //Status = "Enrôlement effectué avec succès !";

                                var empreinte = new EmployeEmpreinte()
                                {
                                    Employe = Employe,
                                    Image = PrintFingerImage,
                                    Template = RegTmp,
                                    Size = RegTmp.Length,
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
                            else
                            {
                                Status = "Enrôlement échoué !";
                            }

                            return;
                        }
                        else
                            ScanCount--;

                    }
                    break;

                default:
                    break;
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
                zkfp2.Terminate();
                zkfp2.CloseDevice(mDevHandle);
            }
            catch (Exception)
            {
            }
            
            CloseDialogWithResult(param as Window, DialogResult.No);
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

        private RelayCommand _saveCommand;
        private RelayCommand _tryAgainCommand;
        private RelayCommand _removeCommand;

        #endregion

    }
}
