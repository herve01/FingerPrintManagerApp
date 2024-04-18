using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Employe
{
    public class Direction : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private string _denomination;
        private string _sigle;
        private string _mission;
        private bool _estGenerale;
        //private Bureau _secretariat;
        
        //public List<Division> Divisions { get; set; }
        //public List<Fonction> Fonctions { get; set; }

        public Direction()
        {
            Id = string.Empty;
            //Divisions = new List<Division>();
            //Fonctions = new List<Fonction>();
        }

        public string Denomination
        {
            get
            {
                return _denomination;
            }
            set
            {
                if (value != _denomination)
                {
                    _denomination = value;
                    RaisePropertyChanged(() => Denomination);
                }
            }
        }
        public string Sigle
        {
            get
            {
                return _sigle;
            }
            set
            {
                if (value != _sigle)
                {
                    _sigle = value;
                    RaisePropertyChanged(() => Sigle);
                }
            }
        }
        public string Mission
        {
            get
            {
                return _mission;
            }
            set
            {
                if (value != _mission)
                {
                    _mission = value;
                    RaisePropertyChanged(() => Mission);
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var direction = (Direction)obj;

            return (!string.IsNullOrWhiteSpace(Id) && direction.Id == Id) || (!string.IsNullOrWhiteSpace(Denomination) && Denomination.ToLower() == direction.Denomination)
                || (!string.IsNullOrWhiteSpace(Sigle) && Sigle.ToLower() == direction.Sigle);

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private Direction backup;
        public void BeginEdit()
        {
            backup = Clone() as Direction;
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
            Denomination = backup.Denomination;
            Sigle = backup.Sigle;
            Mission = backup.Mission;
        }


        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Denomination":
                        if (string.IsNullOrWhiteSpace(Denomination))
                            error = "La dénomination de la direction ne peut être vide.";
                        break;

                    case "Sigle":
                        if (string.IsNullOrWhiteSpace(Sigle))
                            error = "Le sigle de la direction ne peut être vide.";
                        break;

                    //case "Mission":
                    //    if (string.IsNullOrWhiteSpace(Mission))
                    //        error = "La mission de la direction interne  ne peut être vide.";
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
                if (this["Denomination"] != string.Empty)
                    return this["Denomination"];
                if (this["Sigle"] != string.Empty)
                    return this["Sigle"];
                return string.Empty;
            }
        }

        public string Description
        {
            get
            {
                return Denomination;
            }
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
