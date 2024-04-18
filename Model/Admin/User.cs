//using FingerPrintManagerApp.Model.Employe;
using FingerPrintManagerApp.Model.Employe;
using System;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Admin
{
    public class User : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private UserType _type;
        private string _nom;
        private string _prenom;
        private string _username;
        private Sex _sex;
        private Entite _entite;
        private string _email;
        private string _passwd;
        private string _telephone;
        private UserState _etat;

        public UserType Type
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
                }

            }
        }
        public string UserName
        {
            get
            {
                return _username;
            }
            set
            {
                if (value != _username)
                {
                    _username = value;
                    RaisePropertyChanged(() => UserName);
                }

            }
        }
        public Sex Sex
        {
            get
            {
                return _sex;
            }
            set
            {
                if (value != _sex)
                {
                    _sex = value;
                    RaisePropertyChanged(() => Sex);
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
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (value != _email)
                {
                    _email = value;
                    RaisePropertyChanged(() => Email);
                }

            }
        }
        public string PassWd
        {
            get
            {
                return _passwd;
            }
            set
            {
                if (value != _passwd)
                {
                    _passwd = value;
                    RaisePropertyChanged(() => PassWd);
                }

            }
        }
        public string Telephone
        {
            get
            {
                return _telephone;
            }
            set
            {
                if (value != _telephone)
                {
                    _telephone = value;
                    RaisePropertyChanged(() => Telephone);
                }

            }
        }
        public UserState Etat
        {
            get
            {
                return _etat;
            }
            set
            {
                if (value != _etat)
                {
                    _etat = value;
                    RaisePropertyChanged(() => Etat);
                }

            }
        }
        
        public string Name
        {
            get
            {
                return Nom + " " + Prenom;
            }
        }

        public override string ToString()
        {
            return Prenom + " " + Nom;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var user = (User)obj;

            return (!string.IsNullOrWhiteSpace(Id) && user.Id == Id) || (!string.IsNullOrWhiteSpace(Email) && Email.ToLower() == user.Email) 
                || (!string.IsNullOrWhiteSpace(UserName) && UserName.ToLower() == user.UserName);
              
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
        public object Clone()
        {
            return MemberwiseClone();
        }

        private User backup;
        public void BeginEdit()
        {
            backup = MemberwiseClone() as User;
        }

        public void EndEdit()
        {

        }

        public void CancelEdit()
        {
            Id = backup.Id;
            Nom = backup.Nom;
            Prenom = backup.Prenom;
            Sex = backup.Sex;
            Type = backup.Type;
            Email = backup.Email;
            UserName = backup.UserName;
            Telephone = backup.Telephone;
            //Entite = backup.Entite;
        }

        public virtual string Error
        {
            get
            {
                if (this["Nom"] != string.Empty)
                    return this["Nom"];
                else if (this["Email"] != string.Empty)
                    return this["Email"];
                else if (this["Prenom"] != string.Empty)
                    return this["Prenom"];
                else if (this["Telephone"] != string.Empty)
                    return this["Telephone"];
                else if (this["UserName"] != string.Empty)
                    return this["UserName"];
                else if (this["Entite"] != string.Empty)
                    return this["Entite"];
                return string.Empty;
            }
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
                            error = "Le nom de l'utilisateur ne peut être vide.";
                        break;


                    case "Prenom":
                        if (string.IsNullOrWhiteSpace(Prenom))
                            error = "Le prenom de l'utilisateur ne peut être vide.";
                        break;


                    case "UserName":
                        if (string.IsNullOrWhiteSpace(UserName))
                            error = "Le username de l'utilisateur ne peut être vide.";
                        break;

                    case "Email":
                        if (string.IsNullOrWhiteSpace(Email))
                            error = "L'email de l'utilisateur ne peut être vide.";
                        else if (!ValueValidator.IsValidEmail(Email))
                            error = "L'adresse email saisie n'est pas valide.";
                        break;

                    case "Telephone":
                        if (string.IsNullOrWhiteSpace(Telephone))
                            error = "Le téléphone de l'utilisateur ne peut être vide.";
                        else if (!ValueValidator.IsValidPhoneNumber(Telephone))
                            error = "Le numéro de téléphone saisi n'est pas valide.";
                        break;

                    //case "Entite":
                    //    if (Entite == null)
                    //        error = "L'entité de cet utilisateur doit être spécifiée.";
                    //    break;
                        
                    default:
                        break;
                }

                return error;
            }
        }
    }

}
