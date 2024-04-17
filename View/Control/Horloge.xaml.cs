using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ARG.Controls
{
    /// <summary>
    /// Logique d'interaction pour Horloge.xaml
    /// </summary>
    public partial class Horloge : UserControl
    {
        Storyboard seconds;
        Storyboard minutes;
        Storyboard hours;

        public Horloge()
        {
            InitializeComponent();

            seconds = (Storyboard)second.FindResource("sbseconds");
            minutes = (Storyboard)minute.FindResource("sbminutes");
            hours = (Storyboard)hour.FindResource("sbhours");
            
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += timer_Tick;

            timer.Start();
            timer.IsEnabled = false;
        }

        string time = string.Empty;
        private void timer_Tick(object sender, EventArgs e)
        {
            string time = string.Format("{0}:{1}", DateTime.Now.Hour.ToString("D2"), DateTime.Now.Minute.ToString("D2"));
            if (this.time != time)
            {
                txtTime.Text = time;
                this.time = time;
            }
        }

        DispatcherTimer timer;

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            seconds.Begin();
            seconds.Seek(new TimeSpan(0, 0, 0, DateTime.Now.Second, 0));

            minutes.Begin();
            minutes.Seek(new TimeSpan(0, 0, DateTime.Now.Minute, DateTime.Now.Second, 0));

            hours.Begin();
            hours.Seek(new TimeSpan(0, DateTime.Now.Hour % 12, DateTime.Now.Minute, DateTime.Now.Second, 0));

            timer.IsEnabled = true;
        }

        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            seconds.Stop();
            minutes.Stop();
            hours.Stop();

            timer.IsEnabled = false;
        }
    }
}
