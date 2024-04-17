using System.Windows.Forms;

namespace FingerPrintManagerApp.Dialog.Service
{
    public class FolderPathProvider : IFolderPathProvider
    {
        public string GetLoadPath()
        {
            var ofd = new FolderBrowserDialog();

            string filePath = null;
            var dialogResult = ofd.ShowDialog();

            if (dialogResult == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.SelectedPath))
            {
                filePath = ofd.SelectedPath;
            }

            return filePath;
        }

    }
}
