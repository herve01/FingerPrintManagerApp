using FingerPrintManagerApp.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Presence
{
    public class Periode : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private int _mois;
        private int _annee;

        public Periode()
        {
            Id = string.Empty;
        }

        public int Mois
        {
            get
            {
                return _mois;
            }
            set
            {
                if (value != _mois)
                {
                    _mois = value;
                    RaisePropertyChanged(() => Mois);
                }
            }
        }
        public int Annee
        {
            get
            {
                return _annee;
            }
            set
            {
                if (value != _annee)
                {
                    _annee = value;
                    RaisePropertyChanged(() => Annee);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var month = (Periode)obj;

            return (!string.IsNullOrWhiteSpace(Id) && month.Id == Id) ;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private Periode backup;
        public void BeginEdit()
        {
            backup = Clone() as Periode;
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
            Mois = backup.Mois;
            Annee = backup.Annee;
        }


        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Mois":
                        if (Mois > 0)
                            error = "Le mois doit être strictement positif.";
                        break;

                    //case "Direction":
                    //    if (Direction == null)
                    //        error = "La direction de la month doit être renseignée.";
                    //    break;

                    //case "Annee":
                    //    if (string.IsNullOrWhiteSpace(Annee))
                    //        error = "La mission de la month ne peut être vide.";
                    //    break;

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
                if (this["Mois"] != string.Empty)
                    return this["Mois"];

                //else if (this["Annee"] != string.Empty)
                //    return this["Annee"];
                return string.Empty;
            }
        }

    }
}
