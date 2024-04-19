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
    public class DirectionViewModel : PageViewModel
    {
        private static object _lock = new object();
        private ObservableCollection<Direction> directions;

        private bool editing = false;
        public ICollectionView DirectionsView { get; private set; }

        public DirectionViewModel() : base()
        {
            directions = new ObservableCollection<Direction>();
            BindingOperations.EnableCollectionSynchronization(directions, _lock);

            DirectionsView = (CollectionView)CollectionViewSource.GetDefaultView(directions);
            DirectionsView.SortDescriptions.Add(new SortDescription("EstGenerale", ListSortDirection.Descending));
            DirectionsView.SortDescriptions.Add(new SortDescription("Denomination", ListSortDirection.Ascending));
            //Filtering
            DirectionsView.Filter = OnFilterDirection;

            FilterText = string.Empty;

            InitSave();
            MenuInit();
        }

        private bool OnFilterDirection(object obj)
        {
            var direction = obj as Direction;

            if (direction == null)
                return false;

            var pattern = FilterText.Trim().ToLower().NoAccent();

            return direction.Denomination.ToLower().NoAccent().Contains(pattern) || direction.Sigle.ToLower().Contains(pattern);

        }

        private string _action;
        private string _filterText;
        private int _count;
        private Direction _direction;
        private bool _directionLoading;
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
                    DirectionsView.Refresh();
                    RaisePropertyChanged(() => FilterText);
                }
            }
        }
        public int DirectionCount
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
                    RaisePropertyChanged(() => DirectionCount);
                }
            }
        }
        public Direction Direction
        {
            get
            {
                return this._direction;
            }
            set
            {
                if (_direction != value)
                {
                    _direction = value;
                    RaisePropertyChanged(() => Direction);
                }
            }
        }
        public bool DirectionLoading
        {
            get
            {
                return this._directionLoading;
            }
            set
            {
                if (_directionLoading != value)
                {
                    _directionLoading = value;
                    RaisePropertyChanged(() => DirectionLoading);
                }
            }
        }

        #region Commands
        
        protected override async Task LoadData()
        {
            UpdateTimer.IsEnabled = false;

            await LoadDirectionChanges();

            LastDataUpdateTime = DateTime.Now;

            UpdateTimer.IsEnabled = true;
        }

        protected override async Task Load(object param)
        {
            if (!IsInit)
            {
                await LoadDirections();
                LastDataUpdateTime = DateTime.Now;
                IsInit = true;
            }

            UpdateTimer.IsEnabled = true;
        }

        async Task LoadDirections()
        {
            DirectionLoading = true;

            DirectionCount = new DirectionDao().Count();

            directions.Clear();

            await Task.Run(() => new DirectionDao().GetAllAsync(directions));

            DirectionLoading = false;

            DirectionsView.Refresh();
        }

        async Task LoadDirectionChanges()
        {
            DirectionCount = new DirectionDao().Count();

            var list = await Task.Run(() => new DirectionDao().GetAllAsync(LastDataUpdateTime.AddSeconds(-5)));

            list.ForEach(d => {
                var _d = directions.ToList().Find(e => e.Equals(d));

                if (_d != null) directions.Remove(_d);

                directions.Add(d);
            });

            DirectionsView.Refresh();
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
                if (new DirectionDao().Add(Direction) > 0)
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Direction + "",
                            string.Format("Enregistrement de la direction '{0}' (ID : {1}).", Direction.Denomination, Direction.Id)
                        );

                    directions.Add(Direction);
                    DirectionCount++;
                    DirectionsView.Refresh();
                    Status = "Direction enregistrée avec succès !";
                    InitSave();
                }
            }
            else
            {
                var clone = (Direction)Direction.Clone();
                clone.Id = string.Empty;

                directions.Remove(Direction);

                if (!directions.Contains(clone))
                {
                    Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Direction + "",
                            string.Format("Modification de la direction '{0}' (ID : {1}).", Direction.Denomination, Direction.Id)
                        );

                    if (new DirectionDao().Update(Direction) > 0)
                    {
                        Status = "Direction modifiée avec succès !";
                        directions.Add(Direction);
                        DirectionsView.Refresh();
                        InitSave();
                    }
                }
                else
                {
                    Status = "Une direction avec la même description existe déjà !";
                    Direction.CancelEdit();
                    directions.Add(Direction);
                    DirectionsView.Refresh();
                }

            }
        }

        private void InitSave()
        {
            Direction = new Direction();
            //Direction.Secretariat = new Bureau()
            //{
            //    Direction = Direction,
            //    Denomination = "Secrétariat de la direction",
            //    Mission = "Effectuer toutes les fonctions administratives en étroite collaboration avec son directeur"
            //};

            Action = "Enregistrer";
            Title = "Nouvelle direction";
            editing = false;
        }

        private void MenuInit()
        {
            Name = "Direction";
            OptionItem = new OptionItem()
            {
                Name = "Direction",
                ToolTip = "Direction",
                IconPathData = "M1.9890013,1.9890003L1.9890013,17.906003 29.844015,17.906003 29.844015,1.9890003z M0,0L31.834015,0 31.834015,19.896003 16.829017,19.896003 16.829017,31.005015C16.829016,31.555003 16.383018,32.000011 15.834009,32.000011 15.285014,32.000011 14.839016,31.555003 14.839017,31.005015L14.839017,19.896003 0,19.896003z"
            };
        }

        private bool CanSave()
        {
            return Direction.Error == string.Empty;
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
                Direction.CancelEdit();

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
            if (param is Direction)
            {
                Direction = (Direction)param;
                Direction.BeginEdit();
                editing = true;
                Action = "Modifier";
                Title = "Modification de la direction";
            }
        }

        private bool CanEdit(object param)
        {
            var dir = param as Direction;

            if (dir == null)
                return false;

            return !dir.EstGenerale && !editing;
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

            if (param is Direction)
            {
                var direction = (Direction)param;

                var msg = string.Format("Êtes - vous sûr(e) de vouloir supprimer la direction <<{0}>> ?", direction.Denomination);

                if (MyMsgBox.Show(msg, "Humager", MyMsgBoxButton.YesNoCancel, MyMsgBoxIcon.Warning) == DialogueResult.Yes)
                {
                    if (new DirectionDao().Delete(direction) > 0)
                    {
                        Dao.Admin.LogUtil.AddEntry(
                            AppConfig.CurrentUser,
                            DbUtil.Entity.Direction + "",
                            string.Format("Suppression de la direction '{0}' (ID : {1}).", direction.Denomination, direction.Id)
                        );

                        directions.Remove(direction);
                        DirectionCount--;

                        Status = "Direction supprimée avec succès !";
                    }
                    else
                    {
                        Status = "Suppression de la direction échouée ! Vérifiez que vous êtes bien connecté au serveur de données, " +
                            "sinon cette direction est réliée à d'autres objets et ne peut donc pas être supprimée.";
                    }
                }

            }
        }

        private bool CanDelete(object param)
        {
            var dir = param as Direction;

            if (dir == null)
                return false;

            return !dir.EstGenerale && !editing;
        }

        private RelayCommand _saveCommand;
        private RelayCommand _cancelCommand;
        private RelayCommand _editCommand;
        private RelayCommand _deleteCommand;

        #endregion

    }
}
