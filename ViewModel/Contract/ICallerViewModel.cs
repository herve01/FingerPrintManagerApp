namespace FingerPrintManagerApp.ViewModel.Contract
{
    public interface ICallerViewModel
    {
        void AddObject(object obj);
        void InsertObject(object obj);
        void RemoveObject(object obj);
        void EndEditObject(object obj);
        void DeleteObject(object obj);
        bool ContainsObject(object obj);
    }
}
