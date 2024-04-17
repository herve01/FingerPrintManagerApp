using FingerPrintManagerApp.Extension;
using System;
using System.Collections.Generic;

namespace FingerPrintManagerApp.Model
{
    public class Continent : ModelBase, ICloneable
    {
        public new int Id { get; set; }
        private string _nom;
        public List<Pays> Pays { get; set; }
        public Continent()
        {
            Pays = new List<Pays>();
        }
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
            return  Nom;
           
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

            var continent = (Continent)obj;

            return (continent.Id > 0 && continent.Id == Id) || (Nom.ToLower().NoAccent() == continent.Nom.ToLower().NoAccent());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
