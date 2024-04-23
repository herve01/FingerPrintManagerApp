using FingerPrintManagerApp.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Presence
{
    public class HoraireTravailSemaine : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private string _jour;
        private DateTime? _heureDebut;
        private DateTime? _heureFin;
        private bool _estOuvrable;
        private int _numero;

        public HoraireTravailSemaine()
        {
            Id = string.Empty;
        }

        public string Jour
        {
            get
            {
                return _jour;
            }
            set
            {
                if (value != _jour)
                {
                    _jour = value;
                    RaisePropertyChanged(() => Jour);
                }
            }
        }
        public DateTime? HeureDebut
        {
            get
            {
                return _heureDebut;
            }
            set
            {
                if (value != _heureDebut)
                {
                    _heureDebut = value;
                    RaisePropertyChanged(() => HeureDebut);
                }
            }
        }
        public DateTime? HeureFin
        {
            get
            {
                return _heureFin;
            }
            set
            {
                if (value != _heureFin)
                {
                    _heureFin = value;
                    RaisePropertyChanged(() => HeureFin);
                }
            }
        }
        public bool EstOuvrable
        {
            get
            {
                return _estOuvrable;
            }
            set
            {
                if (value != _estOuvrable)
                {
                    _estOuvrable = value;
                    RaisePropertyChanged(() => EstOuvrable);
                }
            }
        }
        public int Numero
        {
            get
            {
                return _numero;
            }
            set
            {
                if (value != _numero)
                {
                    _numero = value;
                    RaisePropertyChanged(() => Numero);
                }
            }
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var horaire = (HoraireTravailSemaine)obj;

            return (!string.IsNullOrWhiteSpace(Id) && horaire.Id == Id) ;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private HoraireTravailSemaine backup;
        public void BeginEdit()
        {
            backup = Clone() as HoraireTravailSemaine;
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
            Jour = backup.Jour;
            HeureDebut = backup.HeureDebut;
            HeureFin = backup.HeureFin;
            EstOuvrable = backup.EstOuvrable;
            Numero = backup.Numero;
        }


        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Jour":
                        if (string.IsNullOrWhiteSpace(Jour))
                            error = "La designtion ne peut être vide.";
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
                if (this["Jour"] != string.Empty)
                    return this["Jour"];

                //else if (this["Annee"] != string.Empty)
                //    return this["Annee"];
                return string.Empty;
            }
        }

    }
}
