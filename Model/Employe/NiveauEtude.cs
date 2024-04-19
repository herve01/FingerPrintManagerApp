using System;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Employe
{
    public class NiveauEtude : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private string _intitule;
        private int _niveau;
        private bool _aDomaine;
        private Grade _gradeRecrutement;

        public NiveauEtude()
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

        public bool ADomaine
        {
            get
            {
                return _aDomaine;
            }
            set
            {
                if (value != _aDomaine)
                {
                    _aDomaine = value;
                    RaisePropertyChanged(() => ADomaine);
                }
            }
        }

        public Grade GradeRecrutement
        {
            get
            {
                return _gradeRecrutement;
            }
            set
            {
                if (value != _gradeRecrutement)
                {
                    _gradeRecrutement = value;
                    RaisePropertyChanged(() => GradeRecrutement);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var niveau = (NiveauEtude)obj;

            return (!string.IsNullOrWhiteSpace(Id) && niveau.Id == Id) || (!string.IsNullOrWhiteSpace(Intitule) && Intitule.ToLower() == niveau.Intitule);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private NiveauEtude backup;
        public void BeginEdit()
        {
            backup = Clone() as NiveauEtude;
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
                            error = "L'intitulé du niveau d'études ne peut être vide.";
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
                return string.Empty;
            }
        }
    }
}
