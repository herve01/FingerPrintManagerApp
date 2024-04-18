using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Employe
{
    public class DirectionProvinciale : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private Province _province;
        private bool _estGenerale;

        public DirectionProvinciale()
        {
            Id = string.Empty;
            Entites = new List<Entite>();
        }

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

        public bool EstGenerale
        {
            get
            {
                return _estGenerale;
            }
            set
            {
                if (value != _estGenerale)
                {
                    _estGenerale = value;
                    RaisePropertyChanged(() => EstGenerale);
                }
            }
        }

        public List<Entite> Entites { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var direction = (DirectionProvinciale)obj;

            return (!string.IsNullOrWhiteSpace(Id) && direction.Id == Id);
               
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private DirectionProvinciale backup;
        public void BeginEdit()
        {
            backup = Clone() as DirectionProvinciale;
            backup.Province = Province.Clone() as Province;
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
            Province = backup.Province;
            EstGenerale = backup.EstGenerale;
        }


        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Province":
                        if (Province == null)
                            error = "La province de la direction provinciale doit être renseignée.";
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
                if (this["Province"] != string.Empty)
                    return this["Province"];

                return string.Empty;
            }
        }
    }
}
