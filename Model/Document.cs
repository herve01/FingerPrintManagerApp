using System.Collections.Generic;

namespace FingerPrintManagerApp.Model
{
    public class Document : ModelBase
    {
        public Document()
        {
            Pages = new List<Page>();
        }

        private byte[] _file;
        public byte[] File
        {
            get
            {
                return this._file;
            }
            set
            {
                if (_file != value)
                {
                    _file = value;
                    RaisePropertyChanged(() => File);
                }
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (_name != value) { 

                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        private List<Page> _pages;
        public List<Page> Pages
        {
            get
            {
                return _pages;
            }
            set
            {
                if (_pages != value)
                {
                    _pages = value;

                    if (_pages != null)
                    {
                        if (_pages.Count > 0)
                            FirstPage = _pages[0];

                    }

                    RaisePropertyChanged(() => Pages);
                }
            }
        }

        public void ComputeSize()
        {
            //Pages.ForEach(p => p.ComputeSize());
        }

        public Size MaxPageSize
        {
            get
            {
                var size = new Size();

                foreach (var page in Pages)
                {
                    if (page.Size.Width > size.Width)
                        size.Width = page.Size.Width;
                }

                return size;
            }
        }

        private int _pageCount;
        public int PageCount
        {
            get
            {
                return _pageCount;
            }
            set
            {
                if (_pageCount != value)
                {
                    _pageCount = value;
                    RaisePropertyChanged(() => PageCount);
                }
            }
        }
        
        private Page _firstPage;
        public Page FirstPage
        {
            get
            {
                return _firstPage;
            }
            set
            {
                if (_firstPage != value)
                {
                    _firstPage = value;
                    RaisePropertyChanged(() => FirstPage);
                }
            }
        }

        public void Add(Page page)
        {
            Pages.Add(page);

            if (Pages.Count == 1)
                FirstPage = page;

        }

    }
}
