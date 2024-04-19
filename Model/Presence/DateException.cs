using FingerPrintManagerApp.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Presence
{
    public class DateException : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private string _description;
        private DateTime _date;

        public DateException()
        {
            Id = string.Empty;
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

        public override string ToString()
        {
            return Date.ToString("ddMM"); 
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var month = (DateException)obj;

            return (!string.IsNullOrWhiteSpace(Id) && month.Id == Id) ;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private DateException backup;
        public void BeginEdit()
        {
            backup = Clone() as DateException;
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
            Description = backup.Description;
            Date = backup.Date;
        }


        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Description":
                        if (string.IsNullOrWhiteSpace(Description))
                            error = "La designtion ne peut être vide.";
                        break;

                    //case "Direction":
                    //    if (Direction == null)
                    //        error = "La direction de la month doit être renseignée.";
                    //    break;

                    //case "Date":
                    //    if (string.IsNullOrWhiteSpace(Date))
                    //        error = "La mission de la month ne peut être vide.";
                    //    break;

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
                if (this["Description"] != string.Empty)
                    return this["Description"];

                //else if (this["Date"] != string.Empty)
                //    return this["Date"];
                return string.Empty;
            }
        }

    }
}
