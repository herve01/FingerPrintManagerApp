using System;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Employe
{
    public class EmployeEmpreinte : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private Employe _employe;
        private byte[] _image;
        private byte[] _template;
        private Fingers _finger;
        private int _size;

        public EmployeEmpreinte()
        {
            Id = string.Empty;
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

        public byte[] Template
        {
            get
            {
                return _template;
            }
            set
            {
                if (value != _template)
                {
                    _template = value;
                    RaisePropertyChanged(() => Template);
                }
            }
        }

        public byte[] Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (value != _image)
                {
                    _image = value;
                    RaisePropertyChanged(() => Image);
                }
            }
        }

        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                if (value != _size)
                {
                    _size = value;
                    RaisePropertyChanged(() => Size);
                }
            }
        }

        public Fingers Finger
        {
            get
            {
                return _finger;
            }
            set
            {
                if (value != _finger)
                {
                    _finger = value;
                    RaisePropertyChanged(() => Finger);
                    RaisePropertyChanged(() => Doigt);
                }
            }
        }

        public string Doigt
        {
            get
            {
                switch (Finger)
                {
                    case Fingers.LL:
                        return "Auriculaire gauche";
                    case Fingers.LR:
                        return "Annulaire gauche";
                    case Fingers.LM:
                        return "Majeur gauche";
                    case Fingers.LI:
                        return "Index gauche";
                    case Fingers.LT:
                        return "Pouce gauche";
                    case Fingers.RT:
                        return "Pouce droit";
                    case Fingers.RI:
                        return "Index droit";
                    case Fingers.RM:
                        return "Majeur droit";
                    case Fingers.RR:
                        return "Annulaire droit";
                    case Fingers.RL:
                        return "Auriculaire droit";
                    default:
                        return string.Empty;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var empreinte = (EmployeEmpreinte)obj;

            return (!string.IsNullOrWhiteSpace(Id) && empreinte.Id == Id) || empreinte.Finger == Finger;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private EmployeEmpreinte backup;
        public void BeginEdit()
        {
            backup = Clone() as EmployeEmpreinte;
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
            Image = backup.Image;
            Template = backup.Template;
            Size = backup.Size;  
            
        }


        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Employe":
                        if (Employe == null)
                            error = "L'employé doit être renseigné.";
                        break;

                    case "Image":
                        if (Image == null)
                            error = "Veuillez capturer une empreinte digitale depuis le scanner d'empreintes connecté.";
                        break;

                    case "Template":
                        if (Template == null)
                            error = "Veuillez capturer une empreinte digitale depuis le scanner d'empreintes connecté.";
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
                if (this["Employe"] != string.Empty)
                    return this["Employe"];
                else if (this["Image"] != string.Empty)
                    return this["Image"];
                else if (this["Template"] != string.Empty)
                    return this["Template"];
                return string.Empty;
            }
        }
    }
}
