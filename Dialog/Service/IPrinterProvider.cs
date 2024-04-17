using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dialog.Service
{
    public interface IPrinterProvider
    {
        string ChoosePrinter();
    }
}
