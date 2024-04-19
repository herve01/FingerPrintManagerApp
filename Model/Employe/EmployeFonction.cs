using System;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Employe
{
    public class EmployeFonction : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private Fonction _fonction;
        private FonctionEmployeType _type;
        private DateTime _dateFin;
        private EmployeGrade _grade;
        private Employe _employe;
        private bool _isRequired;
        private FonctionState _state;
        private DateTime _date;

        public EmployeFonction()
        {
            Id = string.Empty;
            GradeAssocie = new EmployeGrade();
            IsRequired = true;
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private EmployeFonction backup;
        public void BeginEdit()
        {
            backup = Clone() as EmployeFonction;
        }

        public void EndEdit()
        {
          
        }
        public void CancelEdit()
        {
            if (backup == null)
                return;

            Fonction = backup.Fonction;
            Type = backup.Type;
            DateFin = backup.DateFin;
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
                            break;

                        default:
                            break;
                    }
                }

                return error;
            }
        }

        public virtual string Error
        {
            get
            {
                if (this["Fonction"] != string.Empty)
                    return this["Fonction"];

                return string.Empty;
            }
        }
    }
}
