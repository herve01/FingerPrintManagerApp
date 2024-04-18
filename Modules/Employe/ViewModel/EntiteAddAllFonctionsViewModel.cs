using FingerPrintManagerApp.Dao.Employe;
using FingerPrintManagerApp.Dialog.Service;
using FingerPrintManagerApp.Extension;
using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Admin.Util;
using FingerPrintManagerApp.Model.Employe;
using FingerPrintManagerApp.ViewModel.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace FingerPrintManagerApp.Modules.Employe.ViewModel
{
    public class EntiteAddAllFonctionsViewModel : DialogViewModelBase
    {
        private ObservableCollection<Fonction> fonctions;
        // Collection Views
        public ICollectionView FonctionsView { get; private set; }

        public EntiteAddAllFonctionsViewModel(Entite entite, List<Fonction> fonctions)
        {
            Entite = entite;
            
            this.fonctions = new ObservableCollection<Fonction>();
            FonctionsView = (CollectionView)CollectionViewSource.GetDefaultView(this.fonctions);
            FonctionsView.SortDescriptions.Add(new SortDescription("Grade.Niveau", ListSortDirection.Descending));

            if (entite.EstPrincipale)
                FonctionsView.GroupDescriptions.Add(new PropertyGroupDescription("Direction.Denomination"));
            else
                FonctionsView.GroupDescriptions.Add(new PropertyGroupDescription("Entite"));

            FonctionsView.Filter = OnFonctionFilter;

            FonctionCount = fonctions.Count;

            FilterText = string.Empty;

            fonctions.ForEach(f => this.fonctions.Add(f));
        }

        private bool OnFonctionFilter(object obj)
        {
            var fonction = obj as Fonction;

            if (fonction == null)
                return false;

            var pattern = FilterText.Trim().ToLower().NoAccent();
            var unite = string.Empty;

            //if (fonction.Niveau == UniteType.Direction)
            //    unite = ((DirectionInterne)fonction.Unite).Denomination;
            //else if (fonction.Niveau == UniteType.Division)
            //    unite = ((Division)fonction.Unite).Denomination;
            //if (fonction.Niveau == UniteType.Bureau)
            //    unite = ((Bureau)fonction.Unite).Denomination;

            return fonction.Grade.Id.ToLower().Contains(pattern) || unite.ToLower().NoAccent().Contains(pattern);
        }

        private bool _fonctionLoading;
        private Entite _entite;
        private int _count;
        private string _filterText;

        public bool FonctionLoading
        {
            get
            {
                return this._fonctionLoading;
            }
            set
            {
                if (_fonctionLoading != value)
                {
                    _fonctionLoading = value;
                    RaisePropertyChanged(() => FonctionLoading);
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
        public int FonctionCount
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
                    RaisePropertyChanged(() => FonctionCount);
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
                    RaisePropertyChanged(() => FilterText);
                    FonctionsView.Refresh();
                }
            }
        }

        #region Commands

        protected override async Task Load(object param = null)
        {
            Status = string.Empty;
            Title = string.Format("Fonctions - {0}", Entite);
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new AsyncCommand(p => Save(p), p => CanSave());

                return _saveCommand;
            }
        }

        private async Task Save(object param)
        {
            Status = string.Empty;

            var msg = string.Format("Voulez-vous vraiment confirmer l'enregistrement de {0} fonctions de {1} ?", FonctionCount, Entite);

            if (MyMsgBox.Show(msg, "Humager", MyMsgBoxButton.YesNoCancel, MyMsgBoxIcon.Warning) != DialogueResult.Yes)
                return;

            if (await new FonctionDao().AddAsync(fonctions.ToList()) > 0)
            {
                Dao.Admin.LogUtil.AddEntry(
                    AppConfig.CurrentUser,
                    DbUtil.Entity.Fonction + "",
                    string.Format("Ajout de {0} fonctions de {1}.", FonctionCount, Entite)
                );

                Status = "Fonctions enregistrées avec succès !";

                await Task.Delay(3000);

                this.CloseDialogWithResult(param as Window, DialogResult.Yes);
            }
            else
            {
                Status = "Enregistrement échoué. Vérifiez votre connexion au serveur puis réessayez.";
            }
        }

        private bool CanSave()
        {
            foreach (var fonction in fonctions)
                if (fonction.Error != string.Empty)
                    return false;

            return true;
        }

        protected override void Close(object param)
        {
            this.CloseDialogWithResult(param as Window, DialogResult.No);
        }

        private AsyncCommand _saveCommand;

        #endregion
    }
}
