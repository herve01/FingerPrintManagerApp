using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ARG.Controls
{
    public class ARGMonthCalendar : Calendar
    {
        public static DependencyProperty AppointmentsProperty = DependencyProperty.Register("Appointments", typeof(ObservableCollection<Appointment>), typeof(Calendar));
        
        public ObservableCollection<Appointment> Appointments
        {
            get { return (ObservableCollection<Appointment>)GetValue(AppointmentsProperty); }
            set { SetValue(AppointmentsProperty, value); }
        }

        public static DependencyProperty IsAppointedProperty = DependencyProperty.Register("IsAppointed", typeof(bool), typeof(Calendar));
        
        public bool IsAppointed
        {
            get { return (bool)GetValue(IsAppointedProperty); }
            set { SetValue(IsAppointedProperty, value); }
        }

        static ARGMonthCalendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ARGMonthCalendar), new FrameworkPropertyMetadata(typeof(ARGMonthCalendar)));
        }
        
        public ARGMonthCalendar() : base()
        {
            SetValue(AppointmentsProperty, new ObservableCollection<Appointment>());
        }
    }

    public class Appointment
    {
        public DateTime Day { get; set; }
        public string Subject { get; set; }
    }
}
