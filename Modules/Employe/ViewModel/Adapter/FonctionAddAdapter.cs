using FingerPrintManagerApp.Model.Employe;
using FingerPrintManagerApp.ViewModel;

namespace FingerPrintManagerApp.Modules.Employe.ViewModel.Adapter
{
    public class FonctionAddAdapter : ViewModelBase
    {
        public FonctionAddAdapter(Fonction fonction)
        {
            Fonction = fonction;
        }

        private Fonction _fonction;
        public Fonction Fonction
        {
            get
            {
                return _fonction;
            }
            set
            {
                if (value != _fonction)
                {
                    _fonction = value;
                    RaisePropertyChanged(() => Fonction);
                }
            }
        }
    }
}
