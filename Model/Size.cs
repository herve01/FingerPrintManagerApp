namespace FingerPrintManagerApp.Model
{
    public class Size : ModelBase
    {
        private double _width;
        public double Width
        {
            get
            {
                return this._width;
            }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    RaisePropertyChanged(() => Width);
                }
            }
        }

        private double _height;
        public double Height
        {
            get
            {
                return this._height;
            }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    RaisePropertyChanged(() => Height);
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0}x{1}", Width, Height);
        }
    }
}
