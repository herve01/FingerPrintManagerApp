using FingerPrintManagerApp.Dao.Employe;
using FingerPrintManagerApp.Extension;
using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Admin.Util;
using FingerPrintManagerApp.Model.Employe;
using FingerPrintManagerApp.ViewModel;
using FingerPrintManagerApp.ViewModel.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace FingerPrintManagerApp.Modules.Employe.ViewModel
{
    public class DomaineEtudeViewModel : PageViewModel
    {
        private static object _lock = new object();
        private ObservableCollection<DomaineEtude> domaines;

        private bool editing = false;
        public ICollectionView DomainesView { get; private set; }

        public DomaineEtudeViewModel() : base()
        {
            domaines = new ObservableCollection<DomaineEtude>();
            BindingOperations.EnableCollectionSynchronization(domaines, _lock);

            DomainesView = (CollectionView)CollectionViewSource.GetDefaultView(domaines);
            DomainesView.SortDescriptions.Add(new SortDescription("Intitule", ListSortDirection.Ascending));
            //Filtering
            DomainesView.Filter = OnFilterDomaine;

            FilterText = string.Empty;

            InitSave();
            MenuInit();
        }

        private bool OnFilterDomaine(object obj)
        {
            var domaine = obj as DomaineEtude;

            if (domaine == null)
                return false;

            var pattern = FilterText.Trim().ToLower().NoAccent();

            return domaine.Intitule.ToLower().NoAccent().Contains(pattern);

        }

        private string _action;
        private string _filterText;
        private int _count;
        private DomaineEtude _domaine;
        private bool _domaineLoading;

        public string Action
        {
            get
            {
                return this._action;
            }
            set
            {
                if (_action != value)
                {
                    _action = value;
                    RaisePropertyChanged(() => Action);
                }
            }
        }
        public string FilterText
        {
            get
            {
                return this._filterText;
            }
            set
            {
                if (_filterText != value)
                {
                    _filterText = value;
                    DomainesView.Refresh();
                    RaisePropertyChanged(() => FilterText);
                }
            }
        }
        public int DomaineCount
        {
            get
            {
                return this._count;
            }
            set
            {
                if (_count != value)
                {
                    _count = value;
                    RaisePropertyChanged(() => DomaineCount);
                }
            }
        }
        public DomaineEtude Domaine
        {
            get
            {
                return this._domaine;
            }
            set
            {
                if (_domaine != value)
                {
                    _domaine = value;
                    RaisePropertyChanged(() => Domaine);
                }
            }
        }
        public bool DomaineLoading
        {
            get
            {
                return this._domaineLoading;
            }
            set
            {
                if (_domaineLoading != value)
                {
                    _domaineLoading = value;
                    RaisePropertyChanged(() => DomaineLoading);
                }
            }
        }

        #region Commands
        
        protected override async Task LoadData()
        {
            UpdateTimer.IsEnabled = false;

            await LoadDomaineChanges();

            LastDataUpdateTime = DateTime.Now;

            UpdateTimer.IsEnabled = true;
        }

        protected override async Task Load(object param)
        {
            if (!IsInit)
            {
                await LoadDomaines();
                LastDataUpdateTime = DateTime.Now;
                IsInit = true;
            }

            UpdateTimer.IsEnabled = true;
        }

        async Task LoadDomaines()
        {
            DomaineLoading = true;

            DomaineCount = new DomaineEtudeDao().Count();

            domaines.Clear();

            await Task.Run(() => new DomaineEtudeDao().GetAllAsync(domaines));

            DomaineLoading = false;
        }

        async Task LoadDomaineChanges()
        {
            DomaineCount = new DomaineEtudeDao().Count();

            var list = await Task.Run(() => new DomaineEtudeDao().GetAllAsync(LastDataUpdateTime.AddSeconds(-5)));

            list.ForEach(d => {
                var _d = domaines.ToList().Find(e => e.Equals(d));

                if (_d != null) domaines.Remove(_d);

                domaines.Add(d);
            });

            DomainesView.Refresh();
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(p => Save(), p => CanSave());

                return _saveCommand;
            }
        }

        private void Save()
        {
            if (!editing)
            {
                if (new DomaineEtudeDao().Add(Domaine) > 0)
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Domaine_Etude + "",
                            string.Format("Enregistrement du domaine '{0}' (ID : {1}).", Domaine.Intitule, Domaine.Id)
                        );

                    domaines.Add(Domaine);
                    DomaineCount++;
                    DomainesView.Refresh();
                    Status = "Domaine enregistré avec succès !";
                    InitSave();
                }
            }
            else
            {
                var clone = (DomaineEtude)Domaine.Clone();
                clone.Id = string.Empty;

                domaines.Remove(Domaine);

                if (!domaines.Contains(clone))
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Domaine_Etude + "",
                            string.Format("Modification du domaine '{0}' (ID : {1}).", Domaine.Intitule, Domaine.Id)
                        );

                    if (new DomaineEtudeDao().Update(Domaine) > 0)
                    {
                        Status = "Domaine modifié avec succès !";
                        domaines.Add(Domaine);
                        DomainesView.Refresh();
                        InitSave();
                    }
                }
                else
                {
                    Status = "Un domaine avec la même description existe déjà !";
                    Domaine.CancelEdit();
                    domaines.Add(Domaine);
                    DomainesView.Refresh();
                }

            }
        }

        private void InitSave()
        {
            Domaine = new DomaineEtude();
            Action = "Enregistrer";
            Title = "Nouveau domaine";
            editing = false;
        }

        private void MenuInit()
        {
            Name = "Domaine";
            OptionItem = new OptionItem()
            {
                Name = "Domaine",
                ToolTip = "Domaine",
                IconPathData = "M9.3299972,10.196C7.4389917,10.196 5.9029972,11.733995 5.9029972,13.625001 5.9029972,15.513002 7.4389917,17.052004 9.3299972,17.052004 11.221995,17.052004 12.759988,15.513002 12.759988,13.625001 12.759988,11.733995 11.221995,10.196 9.3299972,10.196z M25.036991,4.377991C24.703983,4.3849948 24.367985,4.4579928 24.044987,4.6029971 22.758991,5.179993 22.180988,6.6969917 22.758991,7.9839943 23.335987,9.2709973 24.853993,9.8459943 26.141985,9.2700055 27.427981,8.6889966 28.005984,7.1759956 27.427981,5.8860024 26.994982,4.9219974 26.033983,4.3560031 25.036991,4.377991z M7.7989923,4.3000033L10.934,4.3000033 11.100001,7.1640022 11.100001,7.265992C11.617991,7.4080055,12.108988,7.6109932,12.566996,7.867997L12.626992,7.8079994 14.83799,5.9499977 17.055992,8.1689923 15.147988,10.311998 15.081993,10.378999C15.341988,10.836992,15.544991,11.330003,15.688988,11.847993L15.771996,11.847993 18.646994,12.097993 18.646994,15.233995 15.782997,15.401994 15.688988,15.401994C15.544991,15.918001,15.341988,16.410998,15.081993,16.867998L15.139992,16.927004 16.996987,19.136996 14.779,21.354998 12.635995,19.448 12.566996,19.379C12.108988,19.636996,11.617991,19.839007,11.100001,19.981997L11.100001,20.073 10.934,22.948 7.7989923,22.948 7.5469934,20.082995 7.5469934,19.979998C7.0319945,19.836001,6.5420043,19.632998,6.0870029,19.375002L6.0200016,19.440996 3.8690013,21.354998 1.6519919,19.136996 3.499998,16.935 3.5760021,16.860995C3.3189988,16.403994,3.1170029,15.914995,2.9730057,15.401994L2.8749985,15.401994 0,15.233995 0,12.097993 2.8639969,11.847993 2.9730057,11.847993C3.1170029,11.332994,3.3189988,10.843995,3.5760021,10.388002L3.5059947,10.319993 1.5939932,8.1689923 3.8099956,5.9499977 6.0140049,7.8000038 6.0870029,7.8719948C6.5420043,7.6159981,7.0319945,7.4129952,7.5469934,7.268998L7.5469934,7.1750038z M26.443987,0L28.629976,0.83000182 28.045992,2.8970037 28.017977,2.9689947C28.332979,3.1989902,28.619997,3.4709933,28.876986,3.7769933L28.938997,3.7469945 30.974976,3.0419927 31.932984,5.1770022 30.03598,6.1660011 29.96698,6.1959999C30.026978,6.5939949,30.037995,6.9889992,30.001984,7.3789986L30.060974,7.4019936 32,8.3410047 31.168976,10.526994 29.126986,9.8860028 29.06,9.8609937C28.827974,10.175996,28.556002,10.464006,28.247988,10.720994L28.274996,10.778993 28.98099,12.813997 26.84598,13.770997 25.85599,11.871995 25.826983,11.808C25.430988,11.867997,25.032993,11.880006,24.642994,11.843995L24.619984,11.899995 23.682987,13.839006 21.494986,13.009004 22.138996,10.964006 22.162983,10.899995C21.847996,10.666993,21.560995,10.392,21.302984,10.086L21.243994,10.112001 19.233984,10.877 18.276983,8.7429975 20.147986,7.6949928 20.220984,7.6640022C20.160986,7.2669991,20.149985,6.8719948,20.185995,6.4819954L20.121985,6.4579933 18.160986,5.578995 18.991995,3.3929906 21.056997,3.9770055 21.131994,4.0050052C21.363989,3.6920016,21.634983,3.4059909,21.939991,3.1519933L21.910984,3.0829931 21.144995,1.074997 23.280995,0.11499023 24.327992,1.9889985 24.360996,2.0639954C24.755985,2.0050049,25.149983,1.9920044,25.537983,2.0269928L25.563984,1.9609986z"
            };
        }

        private bool CanSave()
        {
            return Domaine.Error == string.Empty;
        }

        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new RelayCommand(p => Cancel(), p => CanCancel());

                return _cancelCommand;
            }
        }

        private void Cancel()
        {
            if (editing)
                Domaine.CancelEdit();

            InitSave();
        }

        private bool CanCancel()
        {
            return true;
        }

        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new RelayCommand(p => Edit(p), p => CanEdit(p));

                return _editCommand;
            }
        }

        private void Edit(object param)
        {
            if (param is DomaineEtude)
            {
                Domaine = (DomaineEtude)param;
                Domaine.BeginEdit();
                editing = true;
                Action = "Modifier";
                Title = "Modification du domaine";
            }
        }

        private bool CanEdit(object param)
        {
            var dom = param as DomaineEtude;

            if (dom == null)
                return false;

            return !editing;
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(p => Delete(p), p => CanDelete(p));

                return _deleteCommand;
            }
        }

        private void Delete(object param)
        {
            Status = string.Empty;

            if (param is DomaineEtude)
            {
                var domaine = (DomaineEtude)param;

                var msg = string.Format("Êtes - vous sûr(e) de vouloir supprimer le domaine <<{0}>> ?", domaine.Intitule);

                if (MyMsgBox.Show(msg, "Humager", MyMsgBoxButton.YesNoCancel, MyMsgBoxIcon.Warning) == DialogueResult.Yes)
                {
                    if (new DomaineEtudeDao().Delete(domaine) > 0)
                    {
                        Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Domaine_Etude + "",
                            string.Format("Suppression du domaine '{0}' (ID : {1}).", domaine.Intitule, domaine.Id)
                        );

                        domaines.Remove(domaine);
                        DomaineCount--;

                        Status = "Domaine supprimé avec succès !";
                    }
                    else
                    {
                        Status = "Suppression du domaine échouée ! Vérifiez que vous êtes bien connecté au serveur de données, " +
                            "sinon ce domaine est rélié à d'autres objets et ne peut donc pas être supprimé.";
                    }
                }

            }
        }

        private bool CanDelete(object param)
        {
            var dom = param as DomaineEtude;

            if (dom == null)
                return false;

            return !editing;
        }

        private RelayCommand _saveCommand;
        private RelayCommand _cancelCommand;
        private RelayCommand _editCommand;
        private RelayCommand _deleteCommand;

        #endregion

    }
}
