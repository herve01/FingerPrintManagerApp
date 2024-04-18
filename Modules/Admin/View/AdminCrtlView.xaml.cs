using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace FingerPrintManagerApp.Modules.Admin.View
{
    /// <summary>
    /// Logique d'interaction pour AdminCrtlView.xaml
    /// </summary>
    public partial class AdminCrtlView : UserControl
    {
        // Animation variables
        Storyboard expandMO;
        Storyboard unexpandMO;

        public AdminCrtlView()
        {
            InitializeComponent();

            // Animation initialization
            expandMO = TryFindResource("expandMenuOption") as Storyboard;
            expandMO.Completed += ExpandMO_Completed;

            unexpandMO = TryFindResource("unexpandMenuOption") as Storyboard;
            unexpandMO.Completed += UnexpandMO_Completed;

            timer = new DispatcherTimer();
            timer.Interval = new System.TimeSpan(0, 0, 5);
            timer.IsEnabled = false;
            timer.Tick += Timer_Tick;

        }

        DispatcherTimer timer;
        private void Timer_Tick(object sender, System.EventArgs e)
        {
            try
            {
                if (!isAnimStopped)
                    unexpandMO.Begin(this);

                isAnimStopped = false;
            }
            catch (System.Exception)
            {
            }
            
        }

        private void ExpandMO_Completed(object sender, System.EventArgs e)
        {
            isExpanded = true;
            timer.IsEnabled = true;
        }

        private void UnexpandMO_Completed(object sender, System.EventArgs e)
        {
            isExpanded = false;
            isAnimStopped = false;
            timer.IsEnabled = false;
        }

        bool isAnimStopped = false;
        private void stpOption_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isAnimStopped = true;
            timer.IsEnabled = false;
        }

        private void stpOption_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isAnimStopped)
            {
                isAnimStopped = false;
                timer.IsEnabled = true;
            }

        }
        
        bool isExpanded = false;
        private void btnMenu_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isExpanded)
                {
                    unexpandMO.Begin(this);
                }
                else
                    expandMO.Begin(this);
            }
            catch (System.Exception)
            {
            }
        }
        
        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            popupInfo.IsOpen = !popupInfo.IsOpen;
        }

        private void scrollMenu_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
        }

    }
}
