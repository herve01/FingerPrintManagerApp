using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Employe;
using FingerPrintManagerApp.ViewModel;
using System;
using System.ComponentModel;
using System.Windows.Data;

namespace FingerPrintManagerApp.Modules.Employe.ViewModel.Adapter
{
    public class EnfantAddAdapter : ViewModelBase
    {
        public ICollectionView SexesView { get; set; }

        public EnfantAddAdapter(EnfantEmploye enfant)
        {
            Enfant = enfant;

            var sexes = Enum.GetValues(typeof(Sex));
            SexesView = (CollectionView)CollectionViewSource.GetDefaultView(sexes);
        }

        private EnfantEmploye _enfant;
        public EnfantEmploye Enfant
        {
            get
            {
                return _enfant;
            }
            set
            {
                if (value != _enfant)
                {
                    _enfant = value;
                    RaisePropertyChanged(() => Enfant);
                }
            }
        }
    }
}
