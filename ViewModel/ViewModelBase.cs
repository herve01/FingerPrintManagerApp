using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace FingerPrintManagerApp.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static string GetPropertyName<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            return member.Member.Name;
        }

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            RaisePropertyChanged(GetPropertyName(propertyExpression));
        }

        protected void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }

        protected string _status;
        public string Status
        {
            get
            {
                return this._status;
            }
            set
            {
                _status = value;
                RaisePropertyChanged(() => Status);
            }
        }

        public bool EntityRight { get; set; }
    }
}
