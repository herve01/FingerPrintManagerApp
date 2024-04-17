using Microsoft.Win32;

namespace FingerPrintManagerApp.Dialog.Service
{
    public class FilePathProvider : IFilePathProvider
    {
        public string GetLoadPath(string filter = "Fichiers images|*.JPEG;*.jpg;*.png;")
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = filter;
            string filePath = null;
            bool? dialogResult = ofd.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                filePath = ofd.FileName;
            }

            return filePath;
        }

        public string[] GetLoadPaths(string filter = "Fichiers images|*.JPEG;*.jpg;*.png;")
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = filter;
            ofd.Multiselect = true;
            string[] filePaths = null;
            bool? dialogResult = ofd.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                filePaths = ofd.FileNames;
            }

            return filePaths;
        }

        public string GetSavePath()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Fichiers images|*.JPEG;*.jpg;*.png;";
            string filePath = null;
            bool? dialogResult = ofd.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                filePath = ofd.FileName;
            }

            return filePath;
        }
    }
}
