using FingerPrintManagerApp.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Model
{
    public class Zone : ModelBase, ICloneable
    {
        public Zone()
        {
            Communes = new List<Commune>();
        }
        
        public new int Id { get; set; }
        
        private List<Commune> _communes;
        public List<Commune> Communes
        {
            get
            {
                return _communes;
            }
            set
            {
                if (value != _communes)
                {
                    _communes = value;
                    RaisePropertyChanged(() => Communes);
                    RaisePropertyChanged(() => NbreCommunes);
                    RaisePropertyChanged(() => NbreSecteurs);
                    RaisePropertyChanged(() => NbreChefferies);
                }

            }
        }
        
        private TypeZone _type;
        public TypeZone Type
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
        
        private Province _province;
        public Province Province
        {
            get
            {
                return _province;
            }
            set
            {
                if (value != _province)
                {
                    _province = value;
                    RaisePropertyChanged(() => Province);
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
        
        public int NbreCommunes
        {
            get
            {
                return Communes.Count(c => c.Type == TypeCommune.Commune);
            }
        }

        public int NbreSecteurs
        {
            get
            {
                return Communes.Count(c => c.Type == TypeCommune.Secteur);
            }
        }

        public int NbreChefferies
        {
            get
            {
                return Communes.Count(c => c.Type == TypeCommune.Chefférie);
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

            var zone = (Zone)obj;

            return (zone.Id > 0 && zone.Id == Id) || (Nom.ToLower().NoAccent() == zone.Nom.ToLower().NoAccent() && zone.Type == Type && Province.Equals(zone.Province));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public enum TypeZone
    {
        Ville,
        Territoire
    }
}
