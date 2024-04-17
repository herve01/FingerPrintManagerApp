
using FingerPrintManagerApp.Model.Helper;

namespace FingerPrintManagerApp.Dialog.Service
{
    public class PhotoCapture : IPhotoCapture
    {
        public byte[] GetBytes()
        {
            var capteur = new View.CaptureView();
            capteur.ShowDialog();

            return capteur.Image != null ? ImageUtil.BitmapImageToByte(capteur.Image) : null;
        }
    }
}
