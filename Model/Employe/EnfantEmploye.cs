using System;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Employe
{
    public class EnfantEmploye : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private string _nom;
        private string _postNom;
        private string _prenom;
        private Sex _sexe;
        private DateTime _dateNaissance;
        private Employe _employe;

        public EnfantEmploye()
        {
            Id = string.Empty;
        }

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
                    RaisePropertyChanged(() => Name);
                }
            }
        }
        public string PostNom
        {
            get
            {
                return _postNom;
            }
            set
            {
                if (value != _postNom)
                {
                    _postNom = value;
                    RaisePropertyChanged(() => PostNom);
                    RaisePropertyChanged(() => Name);
                }
            }
        }
        public string Prenom
        {
            get
            {
                return _prenom;
            }
            set
            {
                if (value != _prenom)
                {
                    _prenom = value;
                    RaisePropertyChanged(() => Prenom);
                    RaisePropertyChanged(() => Name);
                }
            }
        }
        public Sex Sexe
        {
            get
            {
                return _sexe;
            }
            set
            {
                if (value != _sexe)
                {
                    _sexe = value;
                    RaisePropertyChanged(() => Sexe);
                }
            }
        }
        public DateTime DateNaissance
        {
            get
            {
                return _dateNaissance;
            }
            set
            {
                if (value != _dateNaissance)
                {
                    _dateNaissance = value;
                    RaisePropertyChanged(() => DateNaissance);
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
  
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var enfant = (EnfantEmploye)obj;

            return (!string.IsNullOrWhiteSpace(Id) && enfant.Id == Id) || (!string.IsNullOrWhiteSpace(Nom) && Nom.ToLower() == enfant.Nom);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private EnfantEmploye backup;
        public void BeginEdit()
        {
            backup = Clone() as EnfantEmploye;
            backup.Employe = Employe.Clone() as Employe;
        }

        public void EndEdit()
        {
        }
        public void CancelEdit()
        {
            if (backup == null)
                return;

            Id = backup.Id;
            Nom = backup.Nom;
            PostNom = backup.PostNom;
            Prenom = backup.Prenom;
            Sexe = backup.Sexe;
            DateNaissance = backup.DateNaissance;
            Employe = backup.Employe;    
        }

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Nom":
                        if (string.IsNullOrWhiteSpace(Nom))
                            error = "Le nom de l'enfant ne peut être vide.";
                        break;

                    case "PostNom":
                        if (string.IsNullOrWhiteSpace(PostNom))
                            error = "Le postnom de l'enfant ne peut être vide.";
                        break;

                    case "Prenom":
                        if (string.IsNullOrWhiteSpace(Prenom))
                            error = "Le prénom de l'enfant ne peut être vide.";
                        break;

                    case "Employe":
                        if (Employe == null)
                            error = "Le parent de l'enfant doit être renseigné.";
                        break;

                    case "DateNaissance":
                        if (DateNaissance == null)
                            error = "La date de naissance de l'enfant doit être renseignée.";
                        else
                            if (DateNaissance < DateTime.Now)
                            error = "";
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
                if (this["Nom"] != string.Empty)
                    return this["Nom"];
                else if (this["PostNom"] != string.Empty)
                    return this["PostNom"]; 
                else if (this["Prenom"] != string.Empty)
                    return this["Prenom"]; 
                else if (this["Sexe"] != string.Empty)
                    return this["Sexe"]; 
                else if (this["DateNaissance"] != string.Empty)
                    return this["DateNaissance"];
                else if (this["Employe"] != string.Empty)
                    return this["Employe"];
            ;
                return string.Empty;
            }
        }

        public string Name
        {
            get
            {
                return Prenom + " " + Nom + " " + PostNom;
            }
        }
    }
}
