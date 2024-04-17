
namespace FingerPrintManagerApp.Dialog.Service
{
    public interface IFilePathProvider
    {
        string GetLoadPath(string filter = "");
        string[] GetLoadPaths(string filter = "");
        string GetSavePath();
    }
}
