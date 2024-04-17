using ARG.Controls;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace ARG.Converters
{
    [ValueConversion(typeof(ObservableCollection<Appointment>), typeof(object))]
    public class DayAppointmentsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[1] is DateTime && values[0] is ObservableCollection<Appointment>))
                return new ObservableCollection<Appointment>();

            DateTime date = (DateTime)values[1];

            var appointments = new ObservableCollection<Appointment>();

            foreach (Appointment appointment in (ObservableCollection<Appointment>)values[0])
                if (appointment.Day.Date == date)
                {
                    appointments.Add(appointment);
                }

            return appointments;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
