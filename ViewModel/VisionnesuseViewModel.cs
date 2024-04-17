using System.Windows;
using System.Windows.Input;
using FingerPrintManagerApp.ViewModel.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Data;

namespace FingerPrintManagerApp.ViewModel
{
    public class VisionnesuseViewModel : ViewModelBase
    {
        ObservableCollection<byte[]> images;
        int selectedIndex = 0;

        public ICollectionView ImagesView { get; private set; }
        
        public VisionnesuseViewModel(List<byte[]> images, int initIndex)
        {
            this.images = new ObservableCollection<byte[]>(images);
            selectedIndex = initIndex;

            ImagesView = (CollectionView)CollectionViewSource.GetDefaultView(this.images);

            ImagesView.CurrentChanged += ImagesView_CurrentChanged;
        }

        private void ImagesView_CurrentChanged(object sender, System.EventArgs e)
        {
            if(isInit && ImagesView.CurrentPosition > -1)
                selectedIndex = ImagesView.CurrentPosition;
        }

        private byte[] _selectedImage;
        public byte[] SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                if (_selectedImage != value)
                {
                    _selectedImage = value;
                    RaisePropertyChanged(() => SelectedImage);
                }
            }
        }

        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                    _loadCommand = new RelayCommand(p => Load());

                return _loadCommand;
            }
        }

        bool isInit = false;
        private void Load()
        {
            ImagesView.MoveCurrentToPosition(selectedIndex);
            isInit = true;
        }
        
        public ICommand PrevCommand
        {
            get
            {
                if (_prevCommand == null)
                    _prevCommand = new RelayCommand(p => Prev(), p => CanPrev());

                return _prevCommand;
            }
        }

        private void Prev()
        {
            ImagesView.MoveCurrentToPrevious();
        }

        private bool CanPrev()
        {
            return selectedIndex > 0;
        }

        public ICommand NextCommand
        {
            get
            {
                if (_nextCommand == null)
                    _nextCommand = new RelayCommand(p => Next(), p => CanNext());

                return _nextCommand;
            }
        }

        private void Next()
        {
            ImagesView.MoveCurrentToNext();
        }

        private bool CanNext()
        {
            return selectedIndex < images.Count - 1;
        }

        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand(p => Close(p));

                return _closeCommand;
            }
        }

        private void Close(object p)
        {
            var win = p as Window;

            if (win != null)
                win.Close();
        }

        private RelayCommand _loadCommand;
        private RelayCommand _prevCommand;
        private RelayCommand _nextCommand;
        private RelayCommand _closeCommand;
    }
}
