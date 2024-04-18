using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FingerPrintManagerApp.Modules.Employe.View
{
    /// <summary>
    /// Logique d'interaction pour EmployeDetailsView.xaml
    /// </summary>
    public partial class EmployeDetailsView : UserControl
    {
        public EmployeDetailsView()
        {
            InitializeComponent();
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            popupPrint.IsOpen = !popupPrint.IsOpen;
        }
    }
}
