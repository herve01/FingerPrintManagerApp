
namespace FingerPrintManagerApp.Model.Employe
{
    public class EmployeGrade : ModelBase
    {
        
        private bool _estInitial;
        private GradeEmployeType _type;
        private Grade _grade;
        private Employe _employe;

        public EmployeGrade()
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

        public bool EstInitial
        {
            get
            {
                return _estInitial;
            }
            set
            {
                if (value != _estInitial)
                {
                    _estInitial = value;
                    RaisePropertyChanged(() => EstInitial);
                }
            }
        }

        public GradeEmployeType Type
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var grade = (EmployeGrade)obj;

            return (!string.IsNullOrWhiteSpace(Id) && grade.Id == Id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }



        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Grade":
                        if (Grade == null)
                            error = "Le grade de l'employé doit être renseigné.";
                        break;

                   
                }

                return error;
            }
        }

        public string Error
        {
            get
            {
                if (this["Grade"] != string.Empty)
                    return this["Grade"];

                return Error;
            }
        }
    }
}
