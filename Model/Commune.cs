using FingerPrintManagerApp.Extension;
using System;

namespace FingerPrintManagerApp.Model
{
    public class Commune : ModelBase, ICloneable
    {
        public new int Id { get; set; }
        
        private TypeCommune _type;
        public TypeCommune Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    RaisePropertyChanged(() => Type);
                }

            }
        }
        
        private Zone _zone;
        public Zone Zone
        {
            get
            {
                return _zone;
            }
            set
            {
                if (value != _zone)
                {
                    _zone = value;
                    RaisePropertyChanged(() => Zone);
                }

            }
        }

        private string _nom;
        public string Nom
        {
            get
            {
                return _nom;
            }
            set
            {
                if (value != _nom)
                {
                    _nom = value;
                    RaisePropertyChanged(() => Nom);
                }

            }
        }
        
        public override string ToString()
        {
            return Type + " de " + Nom;
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

            var commune = (Commune)obj;

            return (commune.Id > 0 && commune.Id == Id) || (Nom.ToLower().NoAccent() == commune.Nom.ToLower().NoAccent() && Type == commune.Type && Zone.Equals(commune.Zone));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public enum TypeCommune
    {
        Commune,
        Secteur,
        Chefférie
    }
}
