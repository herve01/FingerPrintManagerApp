using DirectX.Capture;
using Microsoft.VisualBasic.Devices;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FingerPrintManagerApp.Dialog.Service;
using FingerPrintManagerApp.Model.Helper;

namespace FingerPrintManagerApp.Dialog.View
{
    /// <summary>
    /// Logique d'interaction pour CaptureView.xaml
    /// </summary>
    public partial class CaptureView : Window
    {

        WebCam webcam;

        int webcamDeviceId = 0;
        System.Windows.Size clipSize;

        public BitmapSource Image { get; set; }
        
        public CaptureView()
        {
            InitializeComponent();
            clipSize = new System.Windows.Size(296, 380);
        }
        
        bool Capture = false;

        private void btPhoto_Click(object sender, RoutedEventArgs e)
        {
            capturer();
        }

        #region gestion de la capture

        void capturer()
        {
            webcam.Stop();   
            // permet de lire le son pendant la capture 
            Audio audio = new Audio();
            audio.Play(Properties.Resources.TakePhoto1, Microsoft.VisualBasic.AudioPlayMode.Background);

            Capture = true;

            cvsCrop.Visibility = System.Windows.Visibility.Visible;
            InitCrop();
            Crop();
        }

        bool isWebCamOpened = false;

        void initPhoto()
        {
            if (!isWebCamOpened)
            {
                Capture = false;
                webcam.Start((ulong)webcamDeviceId);
                isWebCamOpened = true;
            }

            cvsCrop.Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Capture)
            {
                isWebCamOpened = false;
                initPhoto();
            }

            btLoad.Visibility = Visibility.Visible;
            btClean.Visibility = Visibility.Collapsed;
        }

        private void btClose_Click_1(object sender, RoutedEventArgs e)
        {
           webcam.Stop();
            Close();
        }

        private void Border_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {
                ;
            }

        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (Capture)
            {
                Crop();

                this.Close();
            }
            else
                MyMsgBox.Show("Veuillez capturer en premier la photo !", "Avertissement", MyMsgBoxButton.OK, MyMsgBoxIcon.Warning);
        }

        OpenFileDialog ofd;

        private void btCharger_Click_1(object sender, RoutedEventArgs e)
        {
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                webcam.Stop();
                try
                {
                    imgSource.Source = ImageUtil.BitmapToBitmapImage(new Bitmap(ofd.FileName));
                    
                    Capture = true;
                    isWebCamOpened = false;

                    cvsCrop.Visibility = System.Windows.Visibility.Visible;

                    if (imgSource.Source.Width > 0)
                    {
                        InitCrop();
                        Crop();
                    }
                    
                }
                catch (Exception)
                {

                    MyMsgBox.Show("Le fichier n'a pas été correctement chargé. Assurez-vous qu'il s'agit bien d'un fichier image.", "Erreur de chargement", MyMsgBoxButton.OK, MyMsgBoxIcon.Error);
                }

            }
        }

        void InitCrop()
        {
            //clipRectangle.Width = clipSize.Width;
            //clipRectangle.Height = clipSize.Height;

            var hR = imgSource.Source.Height / clipSize.Height;
            var estimatedWith = imgSource.Source.Width / hR;

            if (estimatedWith < clipSize.Width)
            {
                clipRectangle.Width = estimatedWith;
                clipRectangle.Height = clipSize.Height * clipRectangle.Width / clipSize.Width;
            }

            if (imgSource.ActualHeight > imgSource.ActualWidth)
            {
                ratio = imgSource.ActualHeight / imgSource.ActualWidth;
                isWidthGreaterThanHeight = false;
            }
            else
            {
                ratio = imgSource.ActualWidth / imgSource.ActualHeight;
                isWidthGreaterThanHeight = true;
            }

            img_ah = isWidthGreaterThanHeight ? img_h: img_w * ratio;
            img_aw = !isWidthGreaterThanHeight ? img_w : img_h * ratio;

            clipRectangle.SetValue(Canvas.LeftProperty, (canvas.Width - clipRectangle.Width) / 2);
            clipRectangle.SetValue(Canvas.TopProperty, (canvas.Height - clipRectangle.Height) / 2);

            min_x = (canvas.Width - imgSource.ActualWidth) / 2;
            min_y = (canvas.Height - imgSource.ActualHeight) / 2;

            max_x = min_x + imgSource.ActualWidth;
            max_y = min_y + imgSource.ActualHeight;
            
        }

        private void btWebCam_Click_1(object sender, RoutedEventArgs e)
        {
            isWebCamOpened = false;
            initPhoto();
        }

        void Crop()
        {
            var ratio = imgSource.Source.Height / imgSource.ActualHeight;

            int width = Convert.ToInt32(Convert.ToDouble(clipRectangle.Width));
            int height = Convert.ToInt32(Convert.ToDouble(clipRectangle.Height));

            int x = Convert.ToInt32(Convert.ToDouble(clipRectangle.GetValue(Canvas.LeftProperty)));
            int y = Convert.ToInt32(Convert.ToDouble(clipRectangle.GetValue(Canvas.TopProperty)));

            try
            {
                var src_img = new Bitmap(ImageUtil.BitmapImageToBitmap((BitmapSource)imgSource.Source), (int)imgSource.Source.Width, (int)imgSource.Source.Height);

                var rect = new Int32Rect(Convert.ToInt32(x - Math.Round((double)(canvas.Width - imgSource.ActualWidth) / 2)), Convert.ToInt32(y - Math.Round((double)(canvas.Height - imgSource.ActualHeight) / 2)), width, height);
                rect.X = Convert.ToInt32(rect.X * ratio);
                rect.Y = Convert.ToInt32(rect.Y * ratio);
                rect.Width = Convert.ToInt32(rect.Width * ratio);
                rect.Height = Convert.ToInt32(rect.Height * ratio);

                CroppedBitmap cb = new CroppedBitmap(ImageUtil.BitmapToBitmapImage(src_img), rect);

                Image = cb;
                croppedImg.Source = Image;
            }
            catch (Exception)
            {
            }
            
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            webcam = new WebCam();
            webcam.InitializeWebCam(ref imgSource);

            initPhoto();

            ofd = new OpenFileDialog();
            ofd.Filter = "Fichiers images|*.JPEG;*.jpg;*.png;*.gif";

            img_h = imgSource.Height;
            img_w = imgSource.Width;

            FilterCollection cameras = new Filters().VideoInputDevices;
            if (cameras != null)
            {
                cmbCamera.Items.Clear();
                foreach (Filter camera in cameras)
                {
                    cmbCamera.Items.Add(camera.Name);
                }

                cmbCamera.SelectedIndex = 0;
            }

        }

        bool captured = false;
        double x_sp, x_cvs, y_sp, y_cvs;

        private void clipRectangle_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Input.Mouse.Capture(clipRectangle);

            captured = true;

            x_sp = Canvas.GetLeft(clipRectangle);
            y_sp = Canvas.GetTop(clipRectangle);

            x_cvs = e.GetPosition(canvas).X;
            y_cvs = e.GetPosition(canvas).Y;

            croppedImg.Source = null;
        }

        double img_ah;
        double img_aw;
        double min_x;
        double min_y;
        double max_x;
        double max_y;
        double img_w;
        double img_h;
        double ratio;
        bool isWidthGreaterThanHeight;

        private void clipRectangle_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            double x = e.GetPosition(canvas).X;
            double y = e.GetPosition(canvas).Y;

            if (captured)
            {
                x_sp += x - x_cvs;
                if (hasDraggable(0, clipRectangle.Width))
                {
                    Canvas.SetLeft(clipRectangle, x_sp);
                    x_cvs = x;
                }
                
                y_sp += y - y_cvs;

                if (hasDraggable(1, clipRectangle.Height))
                {
                    Canvas.SetTop(clipRectangle, y_sp);
                    y_cvs = y;
                }


            }
        }

        bool hasDraggable(int pos, double taille)
        {
            switch (pos)
            { 
                case 0 :
                    return x_sp >= min_x && (x_sp + taille) <= img_aw + min_x;
                case 1 :
                    return y_sp >= min_y && (y_sp + taille) <= img_ah + min_y;
                default:
                    return false;
            }
        }

        private void clipRectangle_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            if (captured)
            {
                System.Windows.Input.Mouse.Capture(null);
                captured = false;
                Crop();
            }
        }

        private void cmbCamera_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCamera.SelectedValue != null)
            {
                webcamDeviceId = cmbCamera.SelectedIndex;
                initPhoto();
            }
        }

        private void btnChooseCamera_Click_1(object sender, RoutedEventArgs e)
        {
            popupCamera.IsOpen = !popupCamera.IsOpen;
        }

        void rotateImage(int angle)
        {
            if (imgSource.Source != null)
            {
                try
                {
                    TransformedBitmap tb = new TransformedBitmap();
                    var bi = imgSource.Source;

                    tb.BeginInit();
                    tb.Source = (BitmapSource)bi;

                    RotateTransform rt = new RotateTransform(angle);
                    tb.Transform = rt;
                    tb.EndInit();

                    imgSource.Source = tb;

                    InitCrop();
                    Crop();
                }
                catch (Exception)
                {

                }
            }
            
            
        }

        private void transformButtonClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button bt = sender as System.Windows.Controls.Button;
            switch (bt.Name)
            {
                case "leftRotate":
                    rotateImage(-90);
                    break;

                case "rightRotate":
                    rotateImage(90);
                    break;

            }
        }

        private void sizeButtonClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button bt = sender as System.Windows.Controls.Button;
            switch (bt.Name)
            {
                case "btPlus":
                    AdjustSize(0);
                    break;

                case "btMinus":
                    AdjustSize(1);
                    break;

            }
        }

        void AdjustSize(int op)
        {
            if (imgSource.Source != null)
            {
                double clipY = clipRectangle.Height;
                double clipX = clipRectangle.Width;

                int sizeY = clipY >= 200 ? 20 : 10;
                int sizeX = clipX >= 200 ? 16 : 8;

                sizeY = op == 0 ? sizeY : -sizeY;
                sizeX = op == 0 ? sizeX : -sizeX;

                clipY += sizeY;

                if (clipY >= 100 && clipY <= clipSize.Height)
                {
                    switch (op)
                    {
                        case 0:
                            clipRectangle.Height += sizeY;
                            clipRectangle.Width += sizeX;

                            double x = (double)clipRectangle.GetValue(Canvas.LeftProperty);
                            double y = (double)clipRectangle.GetValue(Canvas.TopProperty);

                            if (clipRectangle.Width + x > max_x)
                                clipRectangle.SetValue(Canvas.LeftProperty, x - sizeX);

                            if (clipRectangle.Height + y > max_y)
                                clipRectangle.SetValue(Canvas.TopProperty, y - sizeY);

                            break;

                        case 1:
                            clipRectangle.Height += sizeY;
                            clipRectangle.Width += sizeX;
                            break;
                        default:
                            break;
                    }

                    Crop();
                }
            }
           
           
        }

        private void btDrag_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button bt = e.Source as System.Windows.Controls.Button;
            switch (bt.Name)
            {
                case "btUp":
                    double up = (double)clipRectangle.GetValue(Canvas.TopProperty) - 5;
                    if (up >= min_y)
                    {
                        clipRectangle.SetValue(Canvas.TopProperty, up);
                        Crop();
                    }
                    break;

                case "btDown":
                    double down = (double)clipRectangle.GetValue(Canvas.TopProperty) + 5;
                    if (down + clipRectangle.Height <= max_y)
                    {
                        clipRectangle.SetValue(Canvas.TopProperty, down);
                        Crop();
                    }
                    break;

                case "btLeft":
                    double left = (double)clipRectangle.GetValue(Canvas.LeftProperty) - 5;
                    if (left >= min_x)
                    {
                        clipRectangle.SetValue(Canvas.LeftProperty, left);
                        Crop();
                    }
                    break;

                case "btRight":
                    double right = (double)clipRectangle.GetValue(Canvas.LeftProperty) + 5;
                    if (right + clipRectangle.Width <= max_x)
                    {
                        clipRectangle.SetValue(Canvas.LeftProperty, right);
                        Crop();
                    }
                    break;

                default:
                    break;
            }
        }

    }
}
