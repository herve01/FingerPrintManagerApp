using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FingerPrintManagerApp
{
    /// <summary>
    /// Logique d'interaction pour MyMsgBox.xaml
    /// </summary>
    public partial class MyMsgBox : Window
    {
        public DialogueResult DialogueResult { get; set; }

        public MyMsgBox()
        {
            InitializeComponent();
            ShowInTaskbar = false;
        }
        public MyMsgBox(string msgText) : this()
        {
            txtMsg.Text = msgText;
            ChangeButton(MyMsgBoxButton.OK);
            cvsIcone.Visibility = Visibility.Collapsed;
            this.ShowDialog();
        }

        public MyMsgBox(string msgText, string caption)
            : this()
        {
            txtMsg.Text = msgText;
            txtCaption.Text = caption;
            ChangeButton(MyMsgBoxButton.OK);
            cvsIcone.Visibility = Visibility.Collapsed;
            this.ShowDialog();
        }

        public MyMsgBox(string msgText, string caption, MyMsgBoxButton buttons) : this()
        {
            txtMsg.Text = msgText;
            txtCaption.Text = caption;
            cvsIcone.Visibility = Visibility.Collapsed;
            ChangeButton(buttons);
            this.ShowDialog();
        }

        public MyMsgBox(string msgText, string caption, MyMsgBoxButton buttons, MyMsgBoxIcon icon)
            : this()
        {
            txtMsg.Text = msgText;
            txtCaption.Text = caption;
            ChangeButton(buttons);
            ChangeIcone(icon);
            this.ShowDialog();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Border_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btOK_Click(object sender, RoutedEventArgs e)
        {
            ChangeDialog(DialogueResult.Ok);
        }

        private void btYes_Click(object sender, RoutedEventArgs e)
        {
            ChangeDialog(DialogueResult.Yes);
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            ChangeDialog(DialogueResult.Cancel);
        }

        private void btNo_Click(object sender, RoutedEventArgs e)
        {
            ChangeDialog(DialogueResult.No);
        }

        void ChangeDialog(DialogueResult dR)
        {
            DialogueResult = dR;
            this.Close();
        }

        void ChangeButton(MyMsgBoxButton buttons)
        {
            foreach (Button button in spButtons.Children.OfType<Button>())
            {
                button.Visibility = Visibility.Collapsed;
            }

            switch (buttons)
            {
                case MyMsgBoxButton.OK:
                    btOK.Visibility = Visibility.Visible;
                    break;

                case MyMsgBoxButton.OKCancel:
                    btOK.Visibility = Visibility.Visible;
                    btCancel.Visibility = Visibility.Visible;
                    DialogueResult = DialogueResult.Cancel;
                    break;
                case MyMsgBoxButton.YesNo:
                    btYes.Visibility = Visibility.Visible;
                    btNo.Visibility = Visibility.Visible;

                    DialogueResult = DialogueResult.No;
                    break;

                case MyMsgBoxButton.YesNoCancel:
                    btYes.Visibility = Visibility.Visible;
                    btCancel.Visibility = Visibility.Visible;
                    btNo.Visibility = Visibility.Visible;
                    DialogueResult = DialogueResult.No;
                    break;
                default:
                    btOK.Visibility = Visibility.Visible;
                    break;
            }
        }

        void ChangeIcone(MyMsgBoxIcon icon)
        {
            foreach (var img in cvsIcone.Children.OfType<Image>())
            {
                img.Visibility = Visibility.Collapsed;
            }

            switch (icon)
            {
                case MyMsgBoxIcon.Success:
                    imgOk.Visibility = Visibility.Visible;
                    break;
                case MyMsgBoxIcon.Warning:
                    imgWarn.Visibility = Visibility.Visible;
                    break;
                case MyMsgBoxIcon.Error:
                    imgWrong.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        public static DialogueResult ShowDialogue()
        {
            MyMsgBox msg = new MyMsgBox();

            return msg.DialogueResult;
        }

        public static void Show(string msgText)
        {
            new MyMsgBox(msgText);
        }

        public static void Show(string msgText, string caption)
        {
            new MyMsgBox(msgText, caption);
        }

        public static DialogueResult Show(string msgText, string caption, MyMsgBoxButton buttons)
        {
            MyMsgBox msg = new MyMsgBox(msgText, caption, buttons);

            return msg.DialogueResult;
        }

        public static DialogueResult Show(string msgText, string caption, MyMsgBoxButton buttons, MyMsgBoxIcon icon)
        {
            MyMsgBox msg = new MyMsgBox(msgText, caption, buttons, icon);

            return msg.DialogueResult;
        }


    }

    public enum MyMsgBoxButton
    {
        OK,
        OKCancel,
        YesNo,
        YesNoCancel
    }

    public enum DialogueResult
    {
        Yes,
        No,
        Cancel,
        Ok
    }

    public enum MyMsgBoxIcon
    {
        Success,
        Warning,
        Error
    }
}
