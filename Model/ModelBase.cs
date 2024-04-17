using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace FingerPrintManagerApp.Model
{
    public class ModelBase : INotifyPropertyChanged
    {
        protected bool _isSelected;
        private string _id;

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    RaisePropertyChanged(() => IsSelected);
                }
            }
        }

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged(() => Id);
                }
            }
        }

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
    }
}
