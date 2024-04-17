using System;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Presence
{
    public class Presence : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private Periode _periode;
        private Employe.Employe _employe;
        private DateTime _date;
        private ModePointage _mode;
        private DateTime _heureArrivee;
        private DateTime _heureDepart;

        public Presence()
        {
            Id = string.Empty;
        }

        public Periode Periode
        {
            get
            {
                return _periode;
            }
            set
            {
                if (value != _periode)
                {
                    _periode = value;
                    RaisePropertyChanged(() => Periode);
                }
            }
        }
        public Employe.Employe Employe
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
        public ModePointage Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                if (value != _mode)
                {
                    _mode = value;
                    RaisePropertyChanged(() => Mode);
                }
            }
        }
        public DateTime HeureArrivee
        {
            get
            {
                return _heureArrivee;
            }
            set
            {
                if (value != _heureArrivee)
                {
                    _heureArrivee = value;
                    RaisePropertyChanged(() => HeureArrivee);
                }
            }
        }
        public DateTime HeureDepart
        {
            get
            {
                return _heureDepart;
            }
            set
            {
                if (value != _heureDepart)
                {
                    _heureDepart = value;
                    RaisePropertyChanged(() => HeureDepart);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var presence = (Presence)obj;

            return (!string.IsNullOrWhiteSpace(Id) && presence.Id == Id ) || presence.Employe.Equals(Employe) ;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private Presence backup;
        public void BeginEdit()
        {
            backup = Clone() as Presence;
            backup.Employe = Employe.Clone() as Employe.Employe;
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
            Periode = backup.Periode;
            Employe = backup.Employe;
            Date = backup.Date;
            Mode = backup.Mode;
            HeureArrivee = backup.HeureArrivee;
            HeureDepart = backup.HeureDepart;
        }


        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Periode":
                        if (Periode == null)
                            error = "Le periode de la présence doit être renseigner.";
                        break;

                    case "Date":
                        if(Date < DateTime.Now.Date)
                            error = "La date de la présence ne doit pas être inférieure à celle d'aujourd'hui .";
                        break;

                    case "HeureArrivee":
                        if (HeureArrivee.TimeOfDay < DateTime.Now.TimeOfDay)
                            error = "L'heure d'arrivée de la présence ne doit pas être inférieure à celle d'aujourd'hui .";
                        break;

                    case "HeureDepart":
                        if (HeureDepart < HeureArrivee)
                            error = "L'heure de depart de la présence ne doit pas être inférieure à celle d'arrivée .";
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
                if (this["Periode"] != string.Empty)
                    return this["Periode"];
                if (this["Date"] != string.Empty)
                    return this["Date"];
                if (this["HeureArrivee"] != string.Empty)
                    return this["HeureArrivee"];
                if (this["HeureDepart"] != string.Empty)
                    return this["HeureDepart"];

                return string.Empty;
            }
        }
    }
}
