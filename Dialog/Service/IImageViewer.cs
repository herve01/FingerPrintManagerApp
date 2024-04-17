
using System.Collections.Generic;

namespace FingerPrintManagerApp.Dialog.Service
{
    public interface IImageViewer
    {
        void ShowViewer(List<byte[]> images, int initIndex = 0);
    }
}
