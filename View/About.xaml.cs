using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace FingerPrintManagerApp.View
{
    /// <summary>
    /// Logique d'interaction pour start.xaml
    /// </summary>
    public partial class About : Window
    {
        Storyboard closingAmin;

        public About()
        {
            InitializeComponent();

            closingAmin = (Storyboard)TryFindResource("winCloseAnim");
            closingAmin.Completed += ClosingAmin_Completed;
        }

        private void ClosingAmin_Completed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //txtEdition.Text = Properties.Settings.Default.edition;
            //txtVersion.Text = Properties.Settings.Default.version;
        }

        private void btClose_Click_1(object sender, RoutedEventArgs e)
        {
            //this.Close();
            closingAmin.Begin(this);
        }

        private void Grid_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {
            }
        }

    }
}
