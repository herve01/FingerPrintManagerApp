using FingerPrintManagerApp.Extension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FingerPrintManagerApp.Model
{
    public class Province : ModelBase, ICloneable
    {
        public Province()
        {
            Zones = new List<Zone>();
            Communes = new List<Commune>();
        }
        
        public new int Id { get; set; }

        private List<Zone> _zones;
        public List<Zone> Zones
        {
            get { return _zones; }
            set
            {
                if (_zones != value)
                {
                    _zones = value;
                    RaisePropertyChanged(() => Zones);
                    RaisePropertyChanged(() => NbreVilles);
                    RaisePropertyChanged(() => NbreTerritoires);
                }
            }
        }

        private List<Commune> _communes;
        public List<Commune> Communes
        {
            get { return _communes; }
            set
            {
                if (_communes != value)
                {
                    _communes = value;
                    RaisePropertyChanged(() => Communes);
                    RaisePropertyChanged(() => NbreCommunes);
                    RaisePropertyChanged(() => NbreSecteurs);
                    RaisePropertyChanged(() => NbreChefferies);
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

        private double _superficie;
        public double Superficie
        {
            get
            {
                return _superficie;
            }
            set
            {
                if (value != _superficie)
                {
                    _superficie = value;
                    RaisePropertyChanged(() => Superficie);
                }

            }
        }

        private string _geoTrace;
        public string GeoTrace
        {
            get
            {
                return _geoTrace;
            }
            set
            {
                if (value != _geoTrace)
                {
                    _geoTrace = value;
                    RaisePropertyChanged(() => GeoTrace);
                }

            }
        }
        
        public int NbreVilles
        {
            get
            {
                return Zones.Count(z => z.Type == TypeZone.Ville);
            }
        }

        public int NbreTerritoires
        {
            get
            {
                return Zones.Count(z => z.Type == TypeZone.Territoire);
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
            return "Province de " + Nom;
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

            var prov = (Province)obj;

            return (prov.Id > 0 && prov.Id == Id) || (Nom.ToLower().NoAccent() == prov.Nom.ToLower().NoAccent());
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
