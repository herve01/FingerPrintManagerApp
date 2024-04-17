using FingerPrintManagerApp.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Employe
{
    public class Bureau : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private Division _division;
  
        private string _denomination;
        private string _mission;
        private int _nombreChefs;

        public Bureau()
        {
            Id = string.Empty;
            NombreChefs = 1;
            Fonctions = new List<Fonction>();
        }

        public Division Division
        {
            get
            {
                return _division;
            }
            set
            {
                if (value != _division)
                {
                    _division = value;
                    RaisePropertyChanged(() => Division);

                   
                }
            }
        }


        public string Denomination
        {
            get
            {
                return _denomination;
            }
            set
            {
                if (value != _denomination)
                {
                    _denomination = value;
                    RaisePropertyChanged(() => Denomination);
                }
            }
        }  
        
        public string Mission
        {
            get
            {
                return _mission;
            }
            set
            {
                if (value != _mission)
                {
                    _mission = value;
                    RaisePropertyChanged(() => Mission);
                }
            }
        }

        public int NombreChefs
        {
            get
            {
                return _nombreChefs;
            }
            set
            {
                if (value != _nombreChefs && value >= 1)
                {
                    _nombreChefs = value;
                    RaisePropertyChanged(() => NombreChefs);
                }
            }
        }

        public List<Fonction> Fonctions { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var bureau = (Bureau)obj;

            return (!string.IsNullOrWhiteSpace(Id) && bureau.Id == Id) || (!string.IsNullOrWhiteSpace(Denomination) && Denomination.ToLower() == bureau.Denomination);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private Bureau backup;
        public void BeginEdit()
        {
            backup = Clone() as Bureau;
            backup.Division = Division.Clone() as Division;
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
            Division = backup.Division;
            Denomination = backup.Denomination;
            Mission = backup.Mission;
            NombreChefs = backup.NombreChefs;
        }


        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Denomination":
                        if (string.IsNullOrWhiteSpace(Denomination))
                            error = "La dénomination du bureau ne peut être vide.";
                        break;

                    case "Division":
                        if (Division == null)
                            error = "La division du bureau doit être renseignée.";
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
                if (this["Denomination"] != string.Empty)
                    return this["Denomination"];
                else if (this["Division"] != string.Empty)
                    return this["Division"];

                return string.Empty;
            }
        }

        public string Description
        {
            get
            {
                var bureau = string.Format("{0}{1}", Denomination.ToLower().NoAccent().Contains("secretariat") ? "" : "Bureau ", Denomination);

                if (Division != null)
                    return string.Format("{1}, {0}", Division.Description, bureau);
                //else
                //    return string.Format("{1}, {0}", Direction.Description, bureau);

                return null;
            }
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
