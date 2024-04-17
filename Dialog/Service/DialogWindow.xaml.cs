using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FingerPrintManagerApp.Dialog.Service
{
    /// <summary>
    /// Logique d'interaction pour addRegister.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow()
        {
            InitializeComponent();

        }

        private void Header_MouseDown_1(object sender, MouseButtonEventArgs e)
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
