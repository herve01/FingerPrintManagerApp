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
    public class NiveauEtudeViewModel : PageViewModel
    {
        private static object _lock = new object();
        private ObservableCollection<NiveauEtude> niveaux;

        private bool editing = false;
        public ICollectionView NiveauxView { get; private set; }

        public NiveauEtudeViewModel() : base()
        {
            niveaux = new ObservableCollection<NiveauEtude>();
            BindingOperations.EnableCollectionSynchronization(niveaux, _lock);

            NiveauxView = (CollectionView)CollectionViewSource.GetDefaultView(niveaux);
            NiveauxView.SortDescriptions.Add(new SortDescription("Niveau", ListSortDirection.Ascending));
            //Filtering
            NiveauxView.Filter = OnFilterNiveau;

            FilterText = string.Empty;

            InitSave();
            MenuInit();
        }

        private bool OnFilterNiveau(object obj)
        {
            var niveau = obj as NiveauEtude;

            if (niveau == null)
                return false;

            var pattern = FilterText.Trim().ToLower().NoAccent();

            return niveau.Intitule.ToLower().NoAccent().Contains(pattern) || 
                niveau.GradeRecrutement.Intitule.ToLower().NoAccent().Contains(pattern) ||
                niveau.GradeRecrutement.Id.ToLower().NoAccent().Contains(pattern);

        }

        private string _action;
        private string _filterText;
        private int _count;
        private NiveauEtude _niveau;
        private bool _niveauLoading;

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
                    NiveauxView.Refresh();
                    RaisePropertyChanged(() => FilterText);
                }
            }
        }
        public int NiveauCount
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
                    RaisePropertyChanged(() => NiveauCount);
                }
            }
        }
        public NiveauEtude Niveau
        {
            get
            {
                return this._niveau;
            }
            set
            {
                if (_niveau != value)
                {
                    _niveau = value;
                    RaisePropertyChanged(() => Niveau);
                }
            }
        }
        public bool NiveauLoading
        {
            get
            {
                return this._niveauLoading;
            }
            set
            {
                if (_niveauLoading != value)
                {
                    _niveauLoading = value;
                    RaisePropertyChanged(() => NiveauLoading);
                }
            }
        }

        #region Commands
        
        protected override async Task LoadData()
        {
            UpdateTimer.IsEnabled = false;

            await LoadNiveauChanges();

            LastDataUpdateTime = DateTime.Now;

            UpdateTimer.IsEnabled = true;
        }

        protected override async Task Load(object param)
        {
            if (!IsInit)
            {
                await LoadNiveaux();
                LastDataUpdateTime = DateTime.Now;
                IsInit = true;
            }

            UpdateTimer.IsEnabled = true;
        }

        async Task LoadNiveaux()
        {
            NiveauLoading = true;

            NiveauCount = new NiveauEtudeDao().Count();

            niveaux.Clear();

            await Task.Run(() => new NiveauEtudeDao().GetAllAsync(niveaux));

            NiveauLoading = false;
        }

        async Task LoadNiveauChanges()
        {
            NiveauCount = new NiveauEtudeDao().Count();

            var list = await Task.Run(() => new NiveauEtudeDao().GetAllAsync(LastDataUpdateTime.AddSeconds(-5)));

            list.ForEach(d => {
                var _d = niveaux.ToList().Find(e => e.Equals(d));

                if (_d != null) niveaux.Remove(_d);

                niveaux.Add(d);
            });

            NiveauxView.Refresh();
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
                if (new NiveauEtudeDao().Add(Niveau) > 0)
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Niveau_Etude + "",
                            string.Format("Enregistrement du niveau '{0}' (ID : {1}).", Niveau.Intitule, Niveau.Id)
                        );

                    niveaux.Add(Niveau);
                    NiveauCount++;
                    NiveauxView.Refresh();
                    Status = "Niveau enregistré avec succès !";
                    InitSave();
                }
            }
            else
            {
                var clone = (NiveauEtude)Niveau.Clone();
                clone.Id = string.Empty;

                niveaux.Remove(Niveau);

                if (!niveaux.Contains(clone))
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Niveau_Etude + "",
                            string.Format("Modification du niveau '{0}' (ID : {1}).", Niveau.Intitule, Niveau.Id)
                        );

                    if (new NiveauEtudeDao().Update(Niveau) > 0)
                    {
                        Status = "Niveau modifié avec succès !";
                        niveaux.Add(Niveau);
                        NiveauxView.Refresh();
                        InitSave();
                    }
                }
                else
                {
                    Status = "Un niveau avec la même description existe déjà !";
                    Niveau.CancelEdit();
                    niveaux.Add(Niveau);
                    NiveauxView.Refresh();
                }

            }
        }

        private void InitSave()
        {
            Niveau = new NiveauEtude();
            Action = "Enregistrer";
            Title = "Nouveau niveau";
            editing = false;
        }

        private void MenuInit()
        {
            Name = "Niveau";
            OptionItem = new OptionItem()
            {
                Name = "Niveau",
                ToolTip = "Niveau",
                IconPathData = "M0,21.050974L17,21.050974 17,23.050974 0,23.050974z M5.0000104,14.05096L22.00001,14.05096 22.00001,16.05096 5.0000104,16.05096z M10.074988,6.9759254L27.074988,6.9759254 27.074988,8.9759254 10.074988,8.9759254z M15.049012,0L32.049012,0 32.049012,2 15.049012,2z"
            };
        }

        private bool CanSave()
        {
            return Niveau.Error == string.Empty;
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
                Niveau.CancelEdit();

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
            if (param is NiveauEtude)
            {
                Niveau = (NiveauEtude)param;
                Niveau.BeginEdit();
                editing = true;
                Action = "Modifier";
                Title = "Modification du niveau";
            }
        }

        private bool CanEdit(object param)
        {
            var dom = param as NiveauEtude;

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

            if (param is NiveauEtude)
            {
                var niveau = (NiveauEtude)param;

                var msg = string.Format("Êtes - vous sûr(e) de vouloir supprimer le niveau <<{0}>> ?", niveau.Intitule);

                if (MyMsgBox.Show(msg, "Humager", MyMsgBoxButton.YesNoCancel, MyMsgBoxIcon.Warning) == DialogueResult.Yes)
                {
                    if (new NiveauEtudeDao().Delete(niveau) > 0)
                    {
                        Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Niveau_Etude + "",
                            string.Format("Suppression du niveau '{0}' (ID : {1}).", niveau.Intitule, niveau.Id)
                        );

                        niveaux.Remove(niveau);
                        NiveauCount--;

                        Status = "Niveau supprimé avec succès !";
                    }
                    else
                    {
                        Status = "Suppression du niveau échouée ! Vérifiez que vous êtes bien connecté au serveur de données, " +
                            "sinon ce niveau est rélié à d'autres objets et ne peut donc pas être supprimé.";
                    }
                }

            }
        }

        private bool CanDelete(object param)
        {
            var dom = param as NiveauEtude;

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
