using FingerPrintManagerApp.Extension;
using System;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Employe
{
    public class Fonction : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private Grade _grade;
        private string _intitule;
        private UniteType _niveau;
        private object _unite;
        private Entite _entite;
        private string _description;

        public Fonction()
        {
            Id = string.Empty;
        }

        public Grade Grade
        {
            get
            {
                return _grade;
            }
            set
            {
                if (value != _grade)
                {
                    _grade = value;
                    RaisePropertyChanged(() => Grade);
                }
            }
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
                    RaisePropertyChanged(() => Direction);
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
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    RaisePropertyChanged(() => Description);
                }
            }
        }

        public Direction Direction
        {
            get
            {
                if (Unite is Direction)
                    return (Direction)Unite;
                //else if (Unite is Division)
                //    return ((Division)Unite).Direction;
                //else if (Unite is Bureau)
                //    return ((Bureau)Unite).Direction ?? ((Bureau)Unite).Division.Direction;

                return null;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var fonction = (Fonction)obj;

            return (!string.IsNullOrWhiteSpace(Id) && fonction.Id == Id) || (!string.IsNullOrWhiteSpace(Intitule) && Intitule.ToLower() == fonction.Intitule);
        }

        public override string ToString()
        {
            return Niveau == UniteType.Direction ? ((Direction)Unite).Denomination :
                        Niveau == UniteType.Departement ? ((Departement)Unite).Denomination : ((Entite)Unite).Type.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private Fonction backup;
        public void BeginEdit()
        {
            backup = Clone() as Fonction;
            backup.Grade = Grade.Clone() as Grade;
            backup.Entite = Entite.Clone() as Entite;
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
            Grade = backup.Grade;
            Intitule = backup.Intitule;
            Niveau = backup.Niveau;
            Unite = backup.Unite;
            Entite = backup.Entite;
            Description = backup.Description;
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
                            error = "L'intitulé de la fonction ne peut être vide.";
                        break;

                    case "Grade":
                        if (Grade == null)
                            error = "Le grade de la fonction doit être renseigné.";
                        break;

                    case "Entite":
                        if (Entite == null)
                            error = "L'entité de la fonction doit être renseignée.";
                        break;

                    case "Unite":
                        if (Unite == null)
                            error = "L'unité de la fonction doit être renseignée.";
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
                if (this["Grade"] != string.Empty)
                    return this["Grade"]; 
                if (this["Unite"] != string.Empty)
                    return this["Unite"];

                return string.Empty;
            }
        }

        public static string EstimateName(object unite, Grade grade, int numero = 1)
        {
            if (unite is Direction)
            {
                var direction = (Direction)unite;

                switch (grade.Id)
                {
                    case "DG":
                        return "Directeur général";
                    case "DGA":
                        return "Directeur général adjoint";

                    case "DIR":
                        return string.Format("Directeur de la {0}", direction.Sigle);
                    default:
                        break;
                }
            }
            //else if (unite is Departement)
            //{
            //    var departement = (Departement)unite;

            //    if (departement.Direction == null)
            //        return string.Format("Chef d'{0}", departement.Entite.ToString());
            //    else
            //    {
            //        if (departement.Denomination.ToLower().NoAccent().Contains("secretariat"))
            //            return departement.Denomination.Replace("Secrétariat", "Secrétaire").Replace("Secretariat", "Secrétaire").Replace("secrétariat", "Secrétaire").Replace("secretariat", "Secrétaire");

            //        return string.Format("Chef de departement {0}", departement.Denomination);
            //    }
                
            //}
            //else if (unite is Entite)
            //{
            //    var bureau = (Entite)unite;

            //    var main = "";
            //    if (bureau.Denomination.ToLower().NoAccent().Contains("secretariat"))
            //        main = bureau.Denomination.Replace("Secrétariat", "Secrétaire").Replace("Secretariat", "Secrétaire").Replace("secrétariat", "Secrétaire").Replace("secretariat", "Secrétaire");
            //    else
            //        main = string.Format("Chef de bureau {0}", bureau.Denomination);

            //    return string.Format("{0} {1}", main, bureau.NombreChefs > 1 ? numero + "" : "").Trim();
            //}

            return string.Empty;
        }

        public static string EstimateName(Fonction fonction, int numero = 1)
        {
            return EstimateName(fonction.Unite, fonction.Grade, numero);
        }

        public string EstimatedName
        {
            get
            {
                return EstimateName(Unite, Grade);
            }
        }
    }
}
