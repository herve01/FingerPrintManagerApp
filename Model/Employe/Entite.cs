using System;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Employe
{
    public class Entite : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {

        private Direction _direction;
        private Zone _zone;
        private Address _address;
        private bool _estPrincipale;
        private EntiteType _type;

        public Entite()
        {
            Address = new Address() { IsRequired = true };
            Id = string.Empty;
            Type = EntiteType.Direction;
        }

        public Direction Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                if (value != _direction)
                {
                    _direction = value;
                    RaisePropertyChanged(() => Direction);
                }
            }
        }

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

        public EntiteType Type
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

        public Address Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (value != _address)
                {
                    _address = value;
                    RaisePropertyChanged(() => Address);
                }
            }
        }

        public bool EstPrincipale
        {
            get
            {
                return _estPrincipale;
            }
            set
            {
                if (value != _estPrincipale)
                {
                    _estPrincipale = value;
                    RaisePropertyChanged(() => EstPrincipale);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var entite = (Entite)obj;

            return (!string.IsNullOrWhiteSpace(Id) && entite.Id == Id);
               
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private Entite backup;
        public void BeginEdit()
        {
            backup = Clone() as Entite;

            backup.Zone = Zone.Clone() as Zone;
        }

        public void EndEdit()
        {
            ;
        }
        public void CancelEdit()
        {
            if (backup == null)
                return;

            Id = backup.Id;
            Direction = backup.Direction;
            Zone = backup.Zone;
            Address = backup.Address;
            EstPrincipale = backup.EstPrincipale;
            Type = backup.Type;
        }

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Direction":
                        if (Direction == null)
                            error = "La direction provinciale de l'entité doit être renseignée.";
                        break;

                    case "Zone":
                        if (Zone == null)
                            error = "La zone de l'entité doit être renseignée.";
                        break;

                    case "Address":
                        if (Address.IsRequired)
                            error = Address.Error;
                        break;

                    default:
                        break;
                }

                return error;
            }
        }

        public virtual string Error
        {
            get
            {
                if (this["Zone"] != string.Empty)
                    return this["Zone"];
                if(this["Address"] != string.Empty)
                    return this["Address"]; 

                return string.Empty;
            }
        }

        public override string ToString()
        {
            return EstPrincipale ? "Siège social" : Zone != null ? "Agence de " + Zone.Nom : "Toutes";
        }

        public string Description
        {
            get
            {
                return ToString();
            }
        }
    }
}
