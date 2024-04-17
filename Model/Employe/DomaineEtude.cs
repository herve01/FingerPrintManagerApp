using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Model.Employe
{
    public class DomaineEtude : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private string _intitule;

        public DomaineEtude()
        {
            Id = string.Empty;
        }

        public string Intitule
        {
            get
            {
                return _intitule;
            }
            set
            {
                if (value != _intitule)
                {
                    _intitule = value;
                    RaisePropertyChanged(() => Intitule);
                }
            }
        }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var domaine = (DomaineEtude)obj;

            return (!string.IsNullOrWhiteSpace(Id) && domaine.Id == Id) || (!string.IsNullOrWhiteSpace(Intitule) && Intitule.ToLower() == domaine.Intitule);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private DomaineEtude backup;
        public void BeginEdit()
        {
            backup = Clone() as DomaineEtude;
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
            Intitule = backup.Intitule;
        }


        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Intitule":
                        if (string.IsNullOrWhiteSpace(Intitule))
                            error = "L'intitulé du domaine ne peut être vide.";
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
                if (this["Intitule"] != string.Empty)
                    return this["Intitule"];
                return string.Empty;
            }
        }
    }
}
