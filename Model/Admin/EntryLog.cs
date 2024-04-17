using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FingerPrintManagerApp.Model.Admin
{
    public class EntryLog : INotifyPropertyChanged, ICloneable
    {
        public string EntryId { get; set; }

        private string _event;
        public string Event
        {
            get
            {
                return _event;
            }
            set
            {
                if (value != _event)
                {
                    _event = value;
                    NotifyPropertyChanged();
                }

            }
        }

        private User _user;
        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                if (!value.Equals(_user))
                {
                    _user = value;
                    NotifyPropertyChanged();
                }

            }
        }

        private DateTime _entryDate;
        public DateTime EntryDate
        {
            get
            {
                return _entryDate;
            }
            set
            {
                if (!value.Equals(_entryDate))
                {
                    _entryDate = value;
                    NotifyPropertyChanged();
                }

            }
        }

        private string _entity;
        public string Entity
        {
            get
            {
                return _entity;
            }
            set
            {
                if (!value.Equals(_entity))
                {
                    _entity = value;
                    NotifyPropertyChanged();
                }

            }
        }

        private string _ipAddress;
        public string IPAddress
        {
            get
            {
                return _ipAddress;
            }
            set
            {
                if (!value.Equals(_ipAddress))
                {
                    _ipAddress = value;
                    NotifyPropertyChanged();
                }

            }
        }

        private string _machineName;
        public string MachineName
        {
            get
            {
                return _machineName;
            }
            set
            {
                if (!value.Equals(_machineName))
                {
                    _machineName = value;
                    NotifyPropertyChanged();
                }

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var log = (EntryLog)obj;

            return (!string.IsNullOrWhiteSpace(EntryId) && EntryId == log.EntryId);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
