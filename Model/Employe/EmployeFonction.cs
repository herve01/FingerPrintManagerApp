using System;

namespace FingerPrintManagerApp.Model.Employe
{
    public class EmployeFonction : ModelBase
    {
        private Fonction _fonction;
        private FonctionEmployeType _type;
        private DateTime _dateFin;
        private EmployeGrade _grade;
        private bool _isRequired;
        private FonctionState _state;

        public EmployeFonction()
        {
            Id = string.Empty;
            GradeAssocie = new EmployeGrade();
            IsRequired = true;
        }
        public Fonction Fonction
        {
            get
            {
                return _fonction;
            }
            set
            {
                if (value != _fonction)
                {
                    _fonction = value;
                    RaisePropertyChanged(() => Fonction);

                    GradeAssocie.Grade = Fonction?.Grade;
                }
            }
        }

        public EmployeGrade GradeAssocie
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
                    RaisePropertyChanged(() => GradeAssocie);
                    RaisePropertyChanged(() => Grade);
                }
            }
        }

        public Grade Grade
        {
            get
            {
                return GradeAssocie?.Grade;
            }
        }



        public FonctionEmployeType Type
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
                    RaisePropertyChanged(() => EstInterim);
                }
            }
        }

        public FonctionState State
        {
            get
            {
                return _state;
            }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    RaisePropertyChanged(() => State);
                }
            }
        }

        public DateTime DateFin
        {
            get
            {
                return _dateFin;
            }
            set
            {
                if (value != _dateFin)
                {
                    _dateFin = value;
                    RaisePropertyChanged(() => DateFin);
                }
            }
        }

        public bool IsRequired
        {
            get
            {
                return _isRequired;
            }
            set
            {
                if (value != _isRequired)
                {
                    _isRequired = value;
                    RaisePropertyChanged(() => IsRequired);
                }
            }
        }

        public bool EstInterim
        {
            get
            {
                return Type == FonctionEmployeType.Interim;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var fonction = (EmployeFonction)obj;

            return (!string.IsNullOrWhiteSpace(Id) && fonction.Id == Id);
        }

        
        public void CancelEdit()
        {
            //var backup = backup as EmployeFonction;

            //if (backup == null)
            //    return;

            //Fonction = backup.Fonction;
            //Type = backup.Type;
            //DateFin = backup.DateFin;
        }


        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                if (IsRequired)
                {
                    switch (columnName)
                    {
                        case "Fonction":
                            if (Fonction == null)
                                error = "La fonction doit être renseignée.";
                            //else if (Acte != null)
                            //{
                            //    if (Acte.Type == ActeType.Ordonnance)
                            //    {
                            //        if (!Fonction.Grade.Id.Contains("DG"))
                            //            error = "La nomination de fonction par ordonnance est réservée aux seuls directeurs généraux.";
                            //    }
                            //    else
                            //    {
                            //        if (Fonction.Grade.Id.Contains("DG"))
                            //            error = "Seuls les directeurs généraux sont nommés par ordonnance.";
                            //    }
                            //}
                            break;

                        default:
                           
                            break;
                    }
                }

                return error;
            }
        }

        public string Error
        {
            get
            {
                if (this["Fonction"] != string.Empty)
                    return this["Fonction"];

                return Error;
            }
        }
    }
}
