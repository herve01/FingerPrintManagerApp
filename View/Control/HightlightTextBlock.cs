using FingerPrintManagerApp.ViewModel.Extension;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ARG.Controls
{
    public class HightlightTextBlock : TextBlock
    {
        public HightlightTextBlock() : base()
        {
            HightlightBackground = Brushes.Yellow;
            HightlightForeground = Brushes.Black;
        }

        public static readonly DependencyProperty HightlightTextProperty = DependencyProperty.Register("HightlightText", typeof(string), typeof(HightlightTextBlock), new FrameworkPropertyMetadata(null, OnDataChanged));
        public static readonly DependencyProperty HightlightBackgroundProperty = DependencyProperty.Register("HightlightBackground", typeof(Brush), typeof(HightlightTextBlock), new FrameworkPropertyMetadata(null, OnDataChanged));
        public static readonly DependencyProperty HightlightForegroundProperty = DependencyProperty.Register("HightlightForeground", typeof(Brush), typeof(HightlightTextBlock), new FrameworkPropertyMetadata(null, OnDataChanged));

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tb = (HightlightTextBlock)d;

            if (tb.Text.Trim().Length == 0)
                return;

            var textLower = tb.Text.Trim().ToLower().NoAccent();
            var toFind = ((string)e.NewValue ?? string.Empty).ToLower().NoAccent();

            var firstIndex = textLower.IndexOf(toFind);
            var firstStr = string.Empty;
            var foundStr = string.Empty;

            if (firstIndex != -1)
            {
                firstStr = tb.Text.Substring(0, firstIndex);
                foundStr = tb.Text.Substring(firstIndex, toFind.Length);
            }

            var endIndex = firstIndex != -1 ? firstIndex + toFind.Length : 0;

            var endStr = tb.Text.Substring(endIndex, tb.Text.Length - endIndex);

            tb.Inlines.Clear();
            var run = new Run();
            run.Text = firstStr;
            tb.Inlines.Add(run);

            run = new Run();
            run.Background = tb.HightlightBackground;
            run.Foreground = tb.HightlightForeground;
            run.Text = foundStr;
            tb.Inlines.Add(run);

            run = new Run();
            run.Text = endStr;

            tb.Inlines.Add(run);

        }

        public string HightlightText
        {
            get { return (string)GetValue(HightlightTextProperty); }
            set { SetValue(HightlightTextProperty, value); }
        }

        public Brush HightlightBackground
        {
            get { return (Brush)GetValue(HightlightBackgroundProperty); }
            set { SetValue(HightlightBackgroundProperty, value); }
        }

        public Brush HightlightForeground
        {
            get { return (Brush)GetValue(HightlightForegroundProperty); }
            set { SetValue(HightlightForegroundProperty, value); }
        }
    }
}
