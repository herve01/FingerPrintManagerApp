using FingerPrintManagerApp.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Employe
{
    public class Departement : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
    
        private Direction _direction;
        private string _denomination;
        private string _mission;

        public Departement()
        {
            Id = string.Empty;
        }

  
        public Direction Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                if (value != _direction)
                {
                    _direction = value;
                    RaisePropertyChanged(() => Direction);
                }
            }
        }
        public string Denomination
        {
            get
            {
                return _denomination;
            }
            set
            {
                if (value != _denomination)
                {
                    _denomination = value;
                    RaisePropertyChanged(() => Denomination);
                }
            }
        }    
        public string Mission
        {
            get
            {
                return _mission;
            }
            set
            {
                if (value != _mission)
                {
                    _mission = value;
                    RaisePropertyChanged(() => Mission);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var departement = (Departement)obj;

            return (!string.IsNullOrWhiteSpace(Id) && departement.Id == Id) || (!string.IsNullOrWhiteSpace(Denomination) && Denomination.ToLower() == departement.Denomination);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private Departement backup;
        public void BeginEdit()
        {
            backup = Clone() as Departement;
      
        }

        public void EndEdit()
        {
            ;
        }
        public void CancelEdit()
        {
            if (backup == null)
                return;

            Id = backup.Id;
            
            Denomination = backup.Denomination;
            Mission = backup.Mission;
        }


        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Denomination":
                        if (string.IsNullOrWhiteSpace(Denomination))
                            error = "La dénomination de la departement ne peut être vide.";
                        break;

                    //case "Direction":
                    //    if (Direction == null)
                    //        error = "La direction de la departement doit être renseignée.";
                    //    break;

                    //case "Mission":
                    //    if (string.IsNullOrWhiteSpace(Mission))
                    //        error = "La mission de la departement ne peut être vide.";
                    //    break;

                    default:
                        break;
                }

                return error;
            }
        }

        public virtual string Error
        {
            get
            {
                if (this["Denomination"] != string.Empty)
                    return this["Denomination"];
                else if (this["Direction"] != string.Empty)
                    return this["Direction"];
                //else if (this["Mission"] != string.Empty)
                //    return this["Mission"];
                return string.Empty;
            }
        }

        public static Departement BuildProvincial(Direction entite)
        {
            //var departement = new Departement()
            //{
            //    Direction = entite,
            //    Denomination = "Division provinciale",
            //    Mission = "Gestion intégrale de l'agence."
            //};

            //departement.Bureaux.Add(new Bureau()
            //{
            //    Denomination = "Ressources humaines",
            //    Division = departement
            //});

            //departement.Bureaux.Add(new Bureau()
            //{
            //    Denomination = "Gestion administrative et financière",
            //    Division = departement
            //});

            //departement.Bureaux.Add(new Bureau()
            //{
            //    Denomination = "Secrétariat",
            //    Division = departement
            //});

            //departement.Bureaux.Add(new Bureau()
            //{
            //    Denomination = "Production",
            //    Division = departement
            //});

            //departement.Bureaux.Add(new Bureau()
            //{
            //    Denomination = "Etudes et planification",
            //    Division = departement
            //});

            //departement.Bureaux.Add(new Bureau()
            //{
            //    Denomination = "Magasinage et distribution",
            //    Division = departement
            //});

            //departement.Bureaux.Add(new Bureau()
            //{
            //    Denomination = "BANTIC",
            //    Division = departement
            //});

            //int k = 1;

            //while (k < 5)
            //{
            //    departement.Bureaux.Add(new Bureau()
            //    {
            //        Denomination = "Point de vente " + k,
            //        Division = departement
            //    });
            //    k++;
            //}

            //return departement;

            return null;
        }

        public string Description
        {
            get
            {
                var departement = string.Format("{0}{1}", Denomination.ToLower().NoAccent().Contains("departement") ? "" : "Division ", Denomination);

                //return string.Format("{1}{0}", Direction != null ? ", " + Direction.Description : "", departement);

                return departement;
            }
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
