using FingerPrintManagerApp.ViewModel;
using System.Collections.Generic;

namespace FingerPrintManagerApp.Dialog.Service
{
    public class ImageViewer : IImageViewer
    {
        public void ShowViewer(List<byte[]> images, int initIndex)
        {
            var vm = new VisionnesuseViewModel(images, initIndex);
            new View.VisionneuseView() { DataContext = vm }.ShowDialog();
        }
    }
}
