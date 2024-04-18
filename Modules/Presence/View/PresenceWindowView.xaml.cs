using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace FingerPrintManagerApp.Modules.Presence.View
{
    /// <summary>
    /// Logique d'interaction pour PresenceWindowView.xaml
    /// </summary>
    public partial class PresenceWindowView : Window
    {
        Storyboard successSB;
        Storyboard failSB;

        public PresenceWindowView()
        {
            InitializeComponent();

            successSB = TryFindResource("SuccessScanAnim") as Storyboard;
            failSB = TryFindResource("FailScanAnim") as Storyboard;
        }

        private void This_Loaded(object sender, RoutedEventArgs e)
        {
            var dp = DependencyPropertyDescriptor.FromProperty(TextBlock.TextProperty, typeof(TextBlock));
            dp.AddValueChanged(txtStatus, (s, a) =>
            {
                var text = ((TextBlock)s).Text;
                if (text == "Success")
                    successSB.Begin(this);
                else if (text == "Fail")
                    failSB.Begin(this);
            });
        }

        bool isFull = false;
        private void BtnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (isFull)
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                isFull = false;
                // Full
                BtnFullScreen.Tag = "M11.585977,18.999021L12.999977,20.41302 3.4147511,30.000045 9.999999,30.000045 9.999999,32.000045 0,32.000045 0,22.000045 2,22.000045 2,28.586798z M20.414059,18.998996L29.999999,28.586804 29.999999,22.000045 31.999999,22.000045 31.999999,32.000045 21.999999,32.000045 21.999999,30.000045 28.585288,30.000045 18.999996,20.412996z M21.999999,0L31.999999,0 31.999999,9.999999 29.999999,9.999999 29.999999,3.4131746 20.413977,13.001045 18.999977,11.587039 28.585168,2 21.999999,2z M0,0L9.999999,0 9.999999,2 3.4148293,2 13.000021,11.587039 11.586021,13.001045 2,3.4131756 2,9.999999 0,9.999999z";
            }
            else
            {
                this.WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                isFull = true;
                // Unfull
                BtnFullScreen.Tag = "M20,19.999998L29.514984,19.999998 26.171997,23.345001 32,29.172005 29.17099,32 23.34198,26.172005 20,29.514999z M2.4839783,19.999998L12,19.999998 12,29.514999 8.6569824,26.172005 2.8279724,32 0,29.172005 5.8269958,23.345001z M29.17099,0L32,2.828999 26.171997,8.6559991 29.514984,12.000002 20,12.000002 20,2.485001 23.34198,5.8289995z M2.8279724,0L8.6569824,5.8289995 12,2.485001 12,12.000002 2.4839783,12.000002 5.8269958,8.6559991 0,2.828999z";
            }
        }
    }
}
