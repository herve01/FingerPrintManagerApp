
using FingerPrintManagerApp.Model.Helper;

namespace FingerPrintManagerApp.Model
{
    public class Page : ModelBase
    {
        public Page()
        {
        }

        private int _numero;
        public int Number
        {
            get
            {
                return this._numero;
            }
            set
            {
                if (_numero != value)
                {
                    _numero = value;
                    RaisePropertyChanged(() => Number);
                }
            }
        }
        
        private byte[] _file;
        public byte[] File
        {
            get
            {
                return this._file;
            }
            set
            {
                if (_file != value)
                {
                    _file = value;
                    RaisePropertyChanged(() => File);

                }
            }
        }

        private Document _document;
        public Document Document
        {
            get
            {
                return this._document;
            }
            set
            {
                if (_document != value)
                {
                    _document = value;
                    RaisePropertyChanged(() => Document);
                }
            }
        }

        public Size Size { get; set; }

        bool sizeCompute;
        public void ComputeSize()
        {
            if (sizeCompute)
                return;

            Size.Height = Size.Width = 0;

            if (File == null)
                return;

            var img = ImageUtil.ByteToBitmap(File);

            Size.Height = img.Height;
            Size.Width = img.Width;

            sizeCompute = true;
        }

    }
}
