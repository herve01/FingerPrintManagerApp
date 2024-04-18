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

            var division = (Departement)obj;

            return (!string.IsNullOrWhiteSpace(Id) && division.Id == Id) || (!string.IsNullOrWhiteSpace(Denomination) && Denomination.ToLower() == division.Denomination);
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
                            error = "La dénomination de la division ne peut être vide.";
                        break;

                    //case "Direction":
                    //    if (Direction == null)
                    //        error = "La direction de la division doit être renseignée.";
                    //    break;

                    //case "Mission":
                    //    if (string.IsNullOrWhiteSpace(Mission))
                    //        error = "La mission de la division ne peut être vide.";
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
            //var division = new Departement()
            //{
            //    Direction = entite,
            //    Denomination = "Division provinciale",
            //    Mission = "Gestion intégrale de l'agence."
            //};

            //division.Bureaux.Add(new Bureau()
            //{
            //    Denomination = "Ressources humaines",
            //    Division = division
            //});

            //division.Bureaux.Add(new Bureau()
            //{
            //    Denomination = "Gestion administrative et financière",
            //    Division = division
            //});

            //division.Bureaux.Add(new Bureau()
            //{
            //    Denomination = "Secrétariat",
            //    Division = division
            //});

            //division.Bureaux.Add(new Bureau()
            //{
            //    Denomination = "Production",
            //    Division = division
            //});

            //division.Bureaux.Add(new Bureau()
            //{
            //    Denomination = "Etudes et planification",
            //    Division = division
            //});

            //division.Bureaux.Add(new Bureau()
            //{
            //    Denomination = "Magasinage et distribution",
            //    Division = division
            //});

            //division.Bureaux.Add(new Bureau()
            //{
            //    Denomination = "BANTIC",
            //    Division = division
            //});

            //int k = 1;

            //while (k < 5)
            //{
            //    division.Bureaux.Add(new Bureau()
            //    {
            //        Denomination = "Point de vente " + k,
            //        Division = division
            //    });
            //    k++;
            //}

            //return division;

            return null;
        }

        public string Description
        {
            get
            {
                var division = string.Format("{0}{1}", Denomination.ToLower().NoAccent().Contains("division") ? "" : "Division ", Denomination);

                //return string.Format("{1}{0}", Direction != null ? ", " + Direction.Description : "", division);

                return division;
            }
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
