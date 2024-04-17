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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FingerPrintManagerApp.View.Control
{
    /// <summary>
    /// Logique d'interaction pour CircleProgressBar.xaml
    /// </summary>
    public partial class CircleProgressBar : UserControl
    {
        public static readonly DependencyProperty IndicatorBrushProperty = DependencyProperty.Register("IndicatorBrush", typeof(Brush), typeof(CircleProgressBar));
        public Brush IndicatorBrush
        {
            get
            {
                return (Brush)this.GetValue(IndicatorBrushProperty);
            }
            set
            {
                this.SetValue(IndicatorBrushProperty, value);
            }
        }

        public static readonly DependencyProperty BackgroundBrushProperty = DependencyProperty.Register("BackgroundBrush", typeof(Brush), typeof(CircleProgressBar));
        public Brush BackgroundBrush
        {
            get
            {
                return (Brush)this.GetValue(BackgroundBrushProperty);
            }
            set
            {
                this.SetValue(BackgroundBrushProperty, value);
            }
        }

        public static readonly DependencyProperty ProgressBorderBrushProperty = DependencyProperty.Register("ProgressBorderBrush", typeof(Brush), typeof(CircleProgressBar));
        public Brush ProgressBorderBrush
        {
            get
            {
                return (Brush)this.GetValue(ProgressBorderBrushProperty);
            }
            set
            {
                this.SetValue(ProgressBorderBrushProperty, value);
            }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(CircleProgressBar));
        public int Value
        {
            get
            {
                return (int)this.GetValue(ValueProperty);
            }
            set
            {
                this.SetValue(ValueProperty, value);
            }
        }

        public CircleProgressBar()
        {
            InitializeComponent();
        }
    }
}
