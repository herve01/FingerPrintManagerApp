using FingerPrintManagerApp.Extension;
using System;

namespace FingerPrintManagerApp.Model
{
    public class Pays : ModelBase, ICloneable
    {
        
        private Continent _continent;
        private string _frenchName;
        private string _englishName;
        private float _surface;
        private string _code2;
        private string _svgMag;
        private string _svgMagViewPort;


        public Pays()
        {
            Id = string.Empty;
        }


        public Continent Continent
        {
            get
            {
                return _continent;
            }
            set
            {
                if (value != _continent)
                {
                    _continent = value;
                    RaisePropertyChanged(() => Continent);
                }

            }
        }
        public string FrenchName
        {
            get
            {
                return _frenchName;
            }
            set
            {
                if (value != _frenchName)
                {
                    _frenchName = value;
                    RaisePropertyChanged(() => FrenchName);
                }

            }
        }   
        public string EnglishName
        {
            get
            {
                return _englishName;
            }
            set
            {
                if (value != _englishName)
                {
                    _englishName = value;
                    RaisePropertyChanged(() => EnglishName);
                }

            }
        }
        public float Surface
        {
            get
            {
                return _surface;
            }
            set
            {
                if (value != _surface)
                {
                    _surface = value;
                    RaisePropertyChanged(() => Surface);
                }

            }
        }
        public string Code2
        {
            get
            {
                return _code2;
            }
            set
            {
                if (value != _code2)
                {
                    _code2 = value;
                    RaisePropertyChanged(() => Code2);
                }

            }
        }
        public string SvgMag
        {
            get
            {
                return _svgMag;
            }
            set
            {
                if (value != _svgMag)
                {
                    _svgMag = value;
                    RaisePropertyChanged(() => SvgMag);
                }

            }
        }
        public string SvgMagViewPort
        {
            get
            {
                return _svgMagViewPort;
            }
            set
            {
                if (value != _svgMagViewPort)
                {
                    _svgMagViewPort = value;
                    RaisePropertyChanged(() => SvgMagViewPort);
                }

            }
        }

        public override string ToString()
        {
            return Continent + " de " + FrenchName;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var pays = (Pays)obj;

            return (pays.Id != string.Empty && pays.Id == Id) || (FrenchName.ToLower().NoAccent() == pays.FrenchName.ToLower().NoAccent() && Continent == pays.Continent);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
