using System.ComponentModel;

namespace FingerPrintManagerApp.Model
{
    public class Address : ModelBase, IDataErrorInfo
    {
        public bool IsRequired { get; set; }

        private string _number;
        public string Number
        {
            get { return _number; }
            set
            {
                if(_number != value)
                {
                    _number = value;
                    RaisePropertyChanged(() => Number);
                    RaisePropertyChanged(() => Description);
                }
            }
        }

        private string _street;
        public string Street
        {
            get { return _street; }
            set
            {
                if (_street != value)
                {
                    _street = value;
                    RaisePropertyChanged(() => Street);
                    RaisePropertyChanged(() => Description);
                }
            }
        }

        private Commune _commune;
        public Commune Commune
        {
            get { return _commune; }
            set
            {
                if (_commune != value)
                {
                    _commune = value;
                    RaisePropertyChanged(() => Commune);
                    RaisePropertyChanged(() => Description);
                }
            }
        }

        public string Error
        {
            get
            {
                if (this["Street"] != string.Empty)
                    return this["Street"];
                if (this["Number"] != string.Empty)
                    return this["Number"];
                if (this["Commune"] != string.Empty)
                    return this["Commune"];


                return string.Empty;
            }
        }

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                if (!IsRequired)
                    return error;

                switch (columnName)
                {
                    case "Street":
                        if (IsRequired && string.IsNullOrWhiteSpace(Street))
                            error = "La rue ou l'avenue ne peut être vide.";
                        break;

                    case "Number":
                        if (IsRequired && string.IsNullOrWhiteSpace(Number))
                            error = "Le numéro ne peut être vide.";
                        break;

                    case "Commune":
                        if (IsRequired &&  Commune == null)
                            error = "La commune ne peut être vide.";
                        break;

                    default:
                        break;
                }

                return error;
            }
        }

        public override string ToString()
        {
            if (Number == null && Street == null && Commune == null)
                return null;

            return (Number ?? string.Empty) + ", " + (Street ?? string.Empty) + ", " + (Commune != null ? Commune.ToString() : string.Empty);
        }

        public string Description
        {
            get
            {
                return ToString();
            }
        }
    }
}
