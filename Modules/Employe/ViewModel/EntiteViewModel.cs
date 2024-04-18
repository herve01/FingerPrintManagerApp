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
    public class EntiteViewModel : PageViewModel
    {
        private static object _lock = new object();
        private ObservableCollection<Entite> entites;
        private ObservableCollection<Province> provinces;
        private ObservableCollection<Zone> zones;
        private ObservableCollection<Commune> communes;

        private bool editing = false;
        public ICollectionView EntitesView { get; private set; }
        public ICollectionView CommunesView { get; private set; }
        public ICollectionView ProvincesView { get; private set; }
        public ICollectionView ZonesView { get; private set; }

        public EntiteViewModel()
        {
            entites = new ObservableCollection<Entite>();
            BindingOperations.EnableCollectionSynchronization(entites, _lock);

            EntitesView = (CollectionView)CollectionViewSource.GetDefaultView(entites);
            EntitesView.SortDescriptions.Add(new SortDescription("EstPrincipale", ListSortDirection.Descending));
            EntitesView.SortDescriptions.Add(new SortDescription("Description", ListSortDirection.Ascending));
            //Filtering
            EntitesView.Filter = OnFilterEntite;

            provinces = new ObservableCollection<Province>();
            ProvincesView = (CollectionView)CollectionViewSource.GetDefaultView(provinces);
            ProvincesView.SortDescriptions.Add(new SortDescription("Nom", ListSortDirection.Ascending));

            zones = new ObservableCollection<Zone>();
            ZonesView = (CollectionView)CollectionViewSource.GetDefaultView(zones);
            ZonesView.SortDescriptions.Add(new SortDescription("Nom", ListSortDirection.Ascending));

            communes = new ObservableCollection<Commune>();
            CommunesView = (CollectionView)CollectionViewSource.GetDefaultView(communes);
            CommunesView.SortDescriptions.Add(new SortDescription("Nom", ListSortDirection.Ascending));

            FilterText = string.Empty;

            InitSave();
            MenuInit();
        }

        private bool OnFilterEntite(object obj)
        {
            var entite = obj as Entite;

            if (entite == null)
                return false;

            var pattern = FilterText.Trim().ToLower().NoAccent();

            return entite.Description.ToLower().NoAccent().Contains(pattern) || entite.Zone.ToString().ToLower().NoAccent().Contains(pattern);

        }

        private string _action;
        private string _filterText;
        private int _count;
        private Entite _entite;
        private bool _entiteLoading;
        private Province _selectedProvince;
        private Zone _selectedZone;

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
                    EntitesView.Refresh();
                    RaisePropertyChanged(() => FilterText);
                }
            }
        }
        public int EntiteCount
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
                    RaisePropertyChanged(() => EntiteCount);
                }
            }
        }

        public Entite Entite
        {
            get
            {
                return this._entite;
            }
            set
            {
                if (_entite != value)
                {
                    _entite = value;
                    RaisePropertyChanged(() => Entite);
                }
            }
        }

        public Province SelectedProvince
        {
            get
            {
                return this._selectedProvince;
            }
            set
            {
                if (_selectedProvince != value)
                {
                    _selectedProvince = value;
                    RaisePropertyChanged(() => SelectedProvince);
                    LoadZones();
                }
            }
        }

        public Zone SelectedZone
        {
            get
            {
                return this._selectedZone;
            }
            set
            {
                if (_selectedZone != value)
                {
                    _selectedZone = value;
                    RaisePropertyChanged(() => SelectedZone);

                    Entite.Zone = SelectedZone;

                    LoadCommunes();
                }
            }
        }

        public bool EntiteLoading
        {
            get
            {
                return this._entiteLoading;
            }
            set
            {
                if (_entiteLoading != value)
                {
                    _entiteLoading = value;
                    RaisePropertyChanged(() => EntiteLoading);
                }
            }
        }

        #region Commands

        protected override async Task LoadData()
        {
            UpdateTimer.IsEnabled = false;

            if (provinces.Count == 0)
                await LoadProvinces();

            await LoadEntiteChanges();

            LastDataUpdateTime = DateTime.Now;

            UpdateTimer.IsEnabled = true;
        }

        protected override async Task Load(object param)
        {
            if (!IsInit)
            {
                await LoadEntites();
                await LoadProvinces();
                LastDataUpdateTime = DateTime.Now;
                IsInit = true;
            }

            UpdateTimer.IsEnabled = true;
        }

        async Task LoadEntites()
        {
            EntiteLoading = true;

            entites.Clear();

            EntiteCount = new EntiteDao().Count();

            await Task.Run(() => new EntiteDao().GetAllAsync(entites));

            EntiteLoading = false;

            EntitesView.Refresh();
        }

        private async Task LoadProvinces()
        {
            provinces.Clear();

            var list = await Task.Run(() => new Dao.ProvinceDao().GetAllAsync(true));

            list.ForEach(c => { provinces.Add(c);});

            if (editing)
            {
                ProvincesView.MoveCurrentTo(list.Find(c => c.Equals(Entite.Zone.Province)));
            }
            else
            {
                ProvincesView.MoveCurrentToFirst();
            }

        }

        void LoadZones()
        {
            zones.Clear();

            if (SelectedProvince != null)
            {
                SelectedProvince.Zones.ForEach(c => zones.Add(c));

                if (editing)
                    ZonesView.MoveCurrentTo(SelectedProvince.Zones.Find(c => c.Equals(Entite.Zone)));
                else
                    ZonesView.MoveCurrentToFirst();
            }

        }

        void LoadCommunes()
        {
            communes.Clear();

            if (SelectedZone != null)
            {
                SelectedZone.Communes.ForEach(c => communes.Add(c));

                if (editing)
                    CommunesView.MoveCurrentTo(SelectedZone.Communes.Find(c => c.Equals(Entite.Address.Commune)));
                else
                    CommunesView.MoveCurrentToFirst();
            }

        }

        async Task LoadEntiteChanges()
        {
            EntiteCount = new EntiteDao().Count();

            var list = await Task.Run(() => new EntiteDao().GetAllAsync(LastDataUpdateTime.AddSeconds(-5)));

            list.ForEach(d => {
                var _d = entites.ToList().Find(e => e.Equals(d));

                if (_d != null) entites.Remove(_d);

                entites.Add(d);
            });

            EntitesView.Refresh();
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new AsyncCommand(p => Save(), p => CanSave());

                return _saveCommand;
            }
        }

        private async Task Save()
        {
            Status = string.Empty;

            EntiteLoading = true;

            if (!editing)
            {
                //Entite.Direction = AppConfig.CurrentUser.Entite.Direction;

                if (await new EntiteDao().AddAsync(Entite) > 0)
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Entite + "",
                            string.Format("Enregistrement de l'entité '{0}' (ID : {1}).", Entite.Description, Entite.Id)
                        );

                    entites.Add(Entite);
                    EntiteCount++;
                    EntitesView.Refresh();
                    Status = "Entité enregistrée avec succès !";
                    InitSave();
                }
            }
            else
            {
                var clone = (Entite)Entite.Clone();
                clone.Id = string.Empty;

                entites.Remove(Entite);

                if (!entites.Contains(clone))
                {
                    if (new EntiteDao().Update(Entite) > 0)
                    {
                        Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Entite + "",
                            string.Format("Modification de l'entité '{0}' (ID : {1}).", Entite.Description, Entite.Id)
                        );

                        Status = "Entité modifiée avec succès !";
                        entites.Add(Entite);
                        EntitesView.Refresh();
                        InitSave();
                    }
                }
                else
                {
                    Status = "Une entite avec la même description existe déjà !";
                    Entite.CancelEdit();
                    entites.Add(Entite);
                    EntitesView.Refresh();
                }

            }

            EntiteLoading = false;
        }

        private void InitSave()
        {
            Entite = new Entite();
            Entite.Division = Departement.BuildProvincial(Entite);

            Action = "Enregistrer";
            editing = false;
            Title = "Nouvelle entité";

            ProvincesView.MoveCurrentToFirst();
        }

        private void MenuInit()
        {
            Name = "Entité";
            OptionItem = new OptionItem()
            {
                Name = "Entité",
                ToolTip = "Entité",
                IconPathData = "M7.2000166,25.899994L7.2000166,31 9.0000049,31 9.0000049,25.899994z M4.2999903,25.899994L4.2999903,31 6.2000156,31 6.2000156,25.899994z M1.3999944,25.899994L1.3999944,31 3.2999898,31 3.2999898,25.899994z M7.2000166,19.700012L7.2000166,24.800018 9.0000049,24.800018 9.0000049,19.700012z M4.2999903,19.700012L4.2999903,24.800018 6.2000156,24.800018 6.2000156,19.700012z M1.3999944,19.700012L1.3999944,24.800018 3.2999898,24.800018 3.2999898,19.700012z M0.10000611,18.399994L10.4,18.399994 10.4,32 0.10000611,32z M0,16.399994L10.4,16.399994 10.4,17.5 0,17.5z M5.7999912,12.800018L5.7999912,15 7.6000105,15 7.6000105,12.899994z M2.8999954,12.800018L2.8999954,15 4.7000147,15 4.7000147,12.899994z M5.7999912,9.6000061L5.7999912,11.700012 7.6000105,11.700012 7.6000105,9.6000061z M2.8999954,9.6000061L2.8999954,11.700012 4.7000147,11.700012 4.7000147,9.6000061z M1.7999887,8.8000183L8.7000171,8.8000183 8.7000171,15.600006 1.7999887,15.600006z M5.000003,0L5.3999969,0 5.3999969,5.8999939 7.6000105,5.8999939 7.6000105,7 9.0000049,7 9.0000049,7.7000122 1.3999944,7.7000122 1.3999944,7 2.7999893,7 2.7999893,5.8999939 5.000003,5.8999939z"
            };
        }

        private bool CanSave()
        {
            return Entite.Error == string.Empty;
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
                Entite.CancelEdit();

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
            if (param is Entite)
            {
                Entite = (Entite)param;
                Entite.BeginEdit();
                editing = true;
                Action = "Modifier";
                Title = "Modification de l'entité";

                ProvincesView.MoveCurrentTo(provinces.ToList().Find(d => d.Equals(Entite.Zone.Province)));
                ZonesView.MoveCurrentTo(zones.ToList().Find(d => d.Equals(Entite.Zone)));
            }
        }

        private bool CanEdit(object param)
        {
            var entite = param as Entite;

            return entite != null && !entite.EstPrincipale && !editing;
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

            if (param is Entite)
            {
                var entite = (Entite)param;

                var msg = string.Format("Êtes - vous sûr(e) de vouloir supprimer l'entité <<{0}>> ?", entite.Description);

                if (MyMsgBox.Show(msg, "Humager", MyMsgBoxButton.YesNoCancel, MyMsgBoxIcon.Warning) == DialogueResult.Yes)
                {
                    if (new EntiteDao().Delete(entite) > 0)
                    {
                        Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Entite + "",
                            string.Format("Suppression de l'entité '{0}' (ID : {1}).", entite.Description, entite.Id)
                        );

                        entites.Remove(entite);
                        EntiteCount--;

                        Status = "Entité supprimée avec succès !";
                    }
                    else
                    {
                        Status = "Suppression de l'entite échouée ! Vérifiez que vous êtes bien connecté au serveur de données, " +
                            "sinon cette entité est réliée à d'autres objets et ne peut donc pas être supprimée.";
                    }
                }

            }
        }

        private bool CanDelete(object param)
        {
            var entite = param as Entite;

            return entite != null && !entite.EstPrincipale && !editing;
        }

        private AsyncCommand _saveCommand;
        private RelayCommand _cancelCommand;
        private RelayCommand _editCommand;
        private RelayCommand _deleteCommand;

        #endregion

    }
}
