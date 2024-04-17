using System;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Employe
{
    public class Grade : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private string _intitule;
        private string _type;
        private int _niveau;
        private string _description;

        public Grade()
        {
            Id = string.Empty;
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
        public string Type
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
        public int Niveau
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var grade = (Grade)obj;

            return (!string.IsNullOrWhiteSpace(Id) && grade.Id == Id) || (!string.IsNullOrWhiteSpace(Intitule) && Intitule.ToLower() == grade.Intitule);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private Grade backup;
        public void BeginEdit()
        {
            backup = Clone() as Grade;
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
            Intitule = backup.Intitule;
            Type = backup.Type;
            Niveau = backup.Niveau;
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
                            error = "L'intitulé du grade ne peut être vide.";
                        break;

                    case "Type":
                        if (string.IsNullOrWhiteSpace(Type))
                            error = "Le type de grade ne peut être vide.";
                        break;

                    case "Niveau":
                        if (Niveau < 0)
                            error = "Le niveau de grade doit être strictement positif.";
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
                else if (this["Type"] != string.Empty)
                    return this["Type"];
                else if (this["Niveau"] != string.Empty)
                    return this["Niveau"];

                return string.Empty;
            }
        }

        public static int AGENT_NIVEAU_MIN = 0;
        public static int CB_NIVEAU = 7;
        public static int CD_NIVEAU = 8;
        public static int DIR_NIVEAU = 9;
    }
}
