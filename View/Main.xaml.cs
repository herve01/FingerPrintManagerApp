using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FingerPrintManagerApp.Global.Views
{
    /// <summary>
    /// Logique d'interaction pour Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
        }

        private void borderUser_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            popupUser.IsOpen = !popupUser.IsOpen;
        }
        
    }
}
