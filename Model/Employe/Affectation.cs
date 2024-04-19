using System;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Employe
{
    public class Affectation : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private Employe _employe;
        private Entite _ancienneEntite;
        private Entite _entite;
        private UniteType _niveau;
        private object _unite;
        private DateTime _date;

        public Affectation()
        {
            Id = string.Empty;
        }

        public Employe Employe
        {
            get
            {
                return _employe;
            }
            set
            {
                if (value != _employe)
                {
                    _employe = value;
                    RaisePropertyChanged(() => Employe);
                }
            }
        }
        public Entite AncienneEntite
        {
            get
            {
                return _ancienneEntite;
            }
            set
            {
                if (value != _ancienneEntite)
                {
                    _ancienneEntite = value;
                    RaisePropertyChanged(() => AncienneEntite);
                }
            }
        }
        public Entite Entite
        {
            get
            {
                return _entite;
            }
            set
            {
                if (value != _entite)
                {
                    _entite = value;
                    RaisePropertyChanged(() => Entite);
                }
            }
        }

        public UniteType Niveau
        {
            get
            {
                return _niveau;
            }
            set
            {
                if (value != _niveau)
                {
                    _niveau = value;
                    RaisePropertyChanged(() => Niveau);
                }
            }
        }
        public object Unite
        {
            get
            {
                return _unite;
            }
            set
            {
                if (value != _unite)
                {
                    _unite = value;
                    RaisePropertyChanged(() => Unite);
                }
            }
        }
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (value != _date)
                {
                    _date = value;
                    RaisePropertyChanged(() => Date);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var affectation = (Affectation)obj;

            return (!string.IsNullOrWhiteSpace(Id) && affectation.Id == Id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private Affectation backup;
        public void BeginEdit()
        {
            backup = Clone() as Affectation;
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
            Employe = backup.Employe;
            AncienneEntite = backup.AncienneEntite;
            Entite = backup.Entite;
       
            Niveau = backup.Niveau;
            Unite = backup.Unite;
            Date = backup.Date;
        }


        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Employe":
                        if (Employe == null)
                            error = "L'employé de l'affection doit être renseigné.";
                        break;

                    case "Entite":
                        if (Entite == null)
                            error = "La nouvelle entité de l'affectation doit être renseignée.";
                        break;

                    case "Unite":
                        if (Unite == null)
                            error = "L'unité de l'affectation doit être renseignée.";
                        break;

                    //case "Acte":
                    //    if (Acte == null)
                    //        error = "L'acte d'affectation doit être joint.";
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
                if (this["Employe"] != string.Empty)
                    return this["Employe"];
       
                if (this["Entite"] != string.Empty)
                    return this["Entite"];
              
                if (this["Unite"] != string.Empty)
                    return this["Unite"];

                if (this["Acte"] != string.Empty)
                    return this["Acte"];

                return string.Empty;
            }
        }

        

        public Departement Departement
        {
            get
            {
                if (Unite is Departement)
                    return (Departement)Unite;

                return null;
            }
        }

        public Direction Direction
        {
            get
            {
                if (Unite is Direction)
                    return (Direction)Unite;

                return null;
            }
        }
    }
}
