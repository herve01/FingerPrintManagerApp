using System;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Employe
{
    public class EmployeEtude : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private int _annee;
        private Employe _employe;
        private NiveauEtude _niveau;
        private DomaineEtude _domaine;

        public EmployeEtude()
        {
            Id = string.Empty;
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
        public NiveauEtude Niveau
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
                    RaisePropertyChanged(() => Domaine);

                    if (Niveau == null || !Niveau.ADomaine)
                        Domaine = null;
                }
            }
        }
        public DomaineEtude Domaine
        {
            get
            {
                return _domaine;
            }
            set
            {
                if (value != _domaine)
                {
                    _domaine = value;
                    RaisePropertyChanged(() => Domaine);
                }
            }
        }
      
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var niveau = (EmployeEtude)obj;

            return (!string.IsNullOrWhiteSpace(Id) && niveau.Id == Id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private EmployeEtude backup;
        public void BeginEdit()
        {
            backup = Clone() as EmployeEtude;
        }

        public void EndEdit()
        {
        }

        public void CancelEdit()
        {
            if (backup == null)
                return;

            Id = backup.Id;
            Employe = backup.Employe;
            Niveau = backup.Niveau;
            Domaine = backup.Domaine;
            Annee = backup.Annee;
        }


        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {

                    case "Domaine":
                        if (Domaine == null && Niveau != null && Niveau.ADomaine)
                            error = "Le domaine d'études doit être renseigné.";
                            break;

                    case "Niveau":
                        if (Niveau == null)
                            error = "Le niveau d'études de l'employé doit être renseigné.";
                        break;

                    case "Annee":
                        if (Annee <= 0)
                            error = "L'année de l'obtention est incorrecte.";
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
                if (this["Domaine"] != string.Empty)
                    return this["Domaine"];
                else if (this["Niveau"] != string.Empty)
                    return this["Niveau"];
                else if (this["Annee"] != string.Empty)
                    return this["Annee"];
               
                return string.Empty;
            }
        }
    }
}
