using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FingerPrintManagerApp.Dialog.View
{
    /// <summary>
    /// Logique d'interaction pour DialogWindow.xaml
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

        private void Popup_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}
