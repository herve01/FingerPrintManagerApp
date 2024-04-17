using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FingerPrintManagerApp.Model.Employe
{
    public class Employe : ModelBase, IEditableObject, IDataErrorInfo, ICloneable
    {
        private string _matricule;
        private string _nom;
        private string _postNom;
        private string _prenom;
        private Sex _sexe;
        private byte[] _photo;
        private EtatCivil _etatCivil;
        private string _lieuNaissance;
        private DateTime _dateNaissance;
        private string _immatriculationCNSSAP;
        private EmployeGrade _gradeEngagement;
        private EmployeFonction _currentFonctionNomination;
        private Affectation _currentAffectation;
        private Affectation _lastAffectation;

        private EmployeEtude _currentHighEtude;

        private Province _provinceOrigine;
        private string _personneContact;
        private string _qualiteContact;
        
        private bool _estMecaniseSalaire;
        private bool _estMecanisePrime;
        private bool _estRecense;
        private bool _estAffecte;
        private bool _estActif;

        /* Adresse et Téléphone */
        private string _telephone;
        private string _email;

        private Address _address;

        private string _conjoint;
        private string _telephoneConjoint;

        public List<EnfantEmploye> Enfants { get; set; }
      
        public List<EmployeEtude> Etudes { get; set; }
        public List<EmployeEmpreinte> Empreintes { get; set; }
        public List<EmployeFonction> FonctionsInterim { get; set; }

        public Employe()
        {
            Address = new Address() { IsRequired = true };
            Id = string.Empty;
            Enfants = new List<EnfantEmploye>();
          
            Etudes = new List<EmployeEtude>();
            Empreintes = new List<EmployeEmpreinte>();
            GradeEngagement = new EmployeGrade() { Employe = this, EstInitial = true };
            //CurrentGradeNomination = new EmployeGrade() { Employe = this, Type = GradeEmployeType.Commissionnement };
            //CurrentStatutaireGradeNomination = new EmployeGrade() { Employe = this, Type = GradeEmployeType.Officiel };
            
            CurrentAffectation = new Affectation() { Employe = this };
            CurrentHighEtude = new EmployeEtude() { Employe = this };
        }
       
        public string Matricule
        {
            get
            {
                return _matricule;
            }
            set
            {
                if (value != _matricule)
                {
                    _matricule = value;
                    RaisePropertyChanged(() => Matricule);
                }
            }
        }

        public string Nom
        {
            get
            {
                return _nom;
            }
            set
            {
                if (value != _nom)
                {
                    _nom = value;
                    RaisePropertyChanged(() => Nom);
                    RaisePropertyChanged(() => Name);
                    RaisePropertyChanged(() => Initials);
                }
            }
        }
        public string PostNom
        {
            get
            {
                return _postNom;
            }
            set
            {
                if (value != _postNom)
                {
                    _postNom = value;
                    RaisePropertyChanged(() => PostNom);
                    RaisePropertyChanged(() => Name);
                    RaisePropertyChanged(() => Initials);
                }
            }
        }    
        public string Prenom
        {
            get
            {
                return _prenom;
            }
            set
            {
                if (value != _prenom)
                {
                    _prenom = value;
                    RaisePropertyChanged(() => Prenom);
                    RaisePropertyChanged(() => Name);
                    RaisePropertyChanged(() => Initials);
                }
            }
        }
        public Sex Sexe
        {
            get
            {
                return _sexe;
            }
            set
            {
                if (value != _sexe)
                {
                    _sexe = value;
                    RaisePropertyChanged(() => Sexe);
                }
            }
        }

        public byte[] Photo
        {
            get
            {
                return _photo;
            }
            set
            {
                if (value != _photo)
                {
                    _photo = value;
                    RaisePropertyChanged(() => Photo);
                }
            }
        }

        public EtatCivil EtatCivil
        {
            get
            {
                return _etatCivil;
            }
            set
            {
                if (value != _etatCivil)
                {
                    _etatCivil = value;
                    RaisePropertyChanged(() => EtatCivil);
                    RaisePropertyChanged(() => EstMarie);

                    if (!EstMarie)
                        Conjoint = TelephoneConjoint = null;
                }
            }
        }

        public string LieuNaissance
        {
            get
            {
                return _lieuNaissance;
            }
            set
            {
                if (value != _lieuNaissance)
                {
                    _lieuNaissance = value;
                    RaisePropertyChanged(() => LieuNaissance);
                }
            }
        }

        public DateTime DateNaissance
        {
            get
            {
                return _dateNaissance;
            }
            set
            {
                if (value != _dateNaissance)
                {
                    _dateNaissance = value;
                    RaisePropertyChanged(() => DateNaissance);
                    RaisePropertyChanged(() => Age);
                }
            }
        }

        public Province ProvinceOrigine
        {
            get
            {
                return _provinceOrigine;
            }
            set
            {
                if (value != _provinceOrigine)
                {
                    _provinceOrigine = value;
                    RaisePropertyChanged(() => ProvinceOrigine);
                }
            }
        }

        public string ImmatriculationCNSSAP
        {
            get
            {
                return _immatriculationCNSSAP;
            }
            set
            {
                if (value != _immatriculationCNSSAP)
                {
                    _immatriculationCNSSAP = value;
                    RaisePropertyChanged(() => ImmatriculationCNSSAP);
                }
            }
        }
        
        public EmployeGrade GradeEngagement
        {
            get
            {
                return _gradeEngagement;
            }
            set
            {
                if (value != _gradeEngagement)
                {
                    _gradeEngagement = value;
                    RaisePropertyChanged(() => GradeEngagement);
                    //RaisePropertyChanged(() => AnneesService);
                }
            }
        }



        public EmployeFonction CurrentFonctionNomination
        {
            get
            {
                return _currentFonctionNomination;
            }
            set
            {
                if (value != _currentFonctionNomination)
                {
                    _currentFonctionNomination = value;
                    RaisePropertyChanged(() => CurrentFonctionNomination);
                    //RaisePropertyChanged(() => CurrentGrade);
                }
            }
        }

        public Affectation CurrentAffectation
        {
            get
            {
                return _currentAffectation;
            }
            set
            {
                if (value != _currentAffectation)
                {
                    _currentAffectation = value;
                    RaisePropertyChanged(() => CurrentAffectation);
                }
            }
        }

        public Affectation LastAffectation
        {
            get
            {
                return _lastAffectation;
            }
            set
            {
                if (value != _lastAffectation)
                {
                    _lastAffectation = value;
                    RaisePropertyChanged(() => LastAffectation);
                }
            }
        }

        public EmployeEtude CurrentHighEtude
        {
            get
            {
                return _currentHighEtude;
            }
            set
            {
                if (value != _currentHighEtude)
                {
                    _currentHighEtude = value;
                    RaisePropertyChanged(() => CurrentHighEtude);
                }
            }
        }



        public string PersonneContact
        {
            get
            {
                return _personneContact;
            }
            set
            {
                if (value != _personneContact)
                {
                    _personneContact = value;
                    RaisePropertyChanged(() => PersonneContact);
                }
            }
        }
        public string QualiteContact
        {
            get
            {
                return _qualiteContact;
            }
            set
            {
                if (value != _qualiteContact)
                {
                    _qualiteContact = value;
                    RaisePropertyChanged(() => QualiteContact);
                }
            }
        }

   

        public bool EstActif
        {
            get
            {
                return this._estActif;
            }
            set
            {
                if (_estActif != value)
                {
                    _estActif = value;

                    RaisePropertyChanged(() => EstActif);

                    if (!EstActif)
                        EstAffecte = false;
                }
            }
        }

        public bool EstMarie
        {
            get
            {
                return EtatCivil == EtatCivil.Marié;
            }
            
        }


        public bool EstMecaniseSalaire
        {
            get
            {
                return _estMecaniseSalaire;
            }
            set
            {
                if (value != _estMecaniseSalaire)
                {
                    _estMecaniseSalaire = value;
                    RaisePropertyChanged(() => EstMecaniseSalaire);
                }
            }
        }
        public bool EstMecanisePrime
        {
            get
            {
                return _estMecanisePrime;
            }
            set
            {
                if (value != _estMecanisePrime)
                {
                    _estMecanisePrime = value;
                    RaisePropertyChanged(() => EstMecanisePrime);
                }
            }
        }
        public bool EstRecense
        {
            get
            {
                return _estRecense;
            }
            set
            {
                if (value != _estRecense)
                {
                    _estRecense = value;
                    RaisePropertyChanged(() => EstRecense);
                }
            }
        }
        public bool EstAffecte
        {
            get
            {
                return _estAffecte;
            }
            set
            {
                if (value != _estAffecte)
                {
                    _estAffecte = value;
                    RaisePropertyChanged(() => EstAffecte);
                }
            }
        }
        public string Telephone
        {
            get
            {
                return _telephone;
            }
            set
            {
                if (value != _telephone)
                {
                    _telephone = value;
                    RaisePropertyChanged(() => Telephone);
                }
            }
        }
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (value != _email)
                {
                    _email = value;
                    RaisePropertyChanged(() => Email);
                }
            }
        }

        public Address Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (value != _address)
                {
                    _address = value;
                    RaisePropertyChanged(() => Address);
                }
            }
        }
        public string Conjoint
        {
            get
            {
                return _conjoint;
            }
            set
            {
                if (value != _conjoint)
                {
                    _conjoint = value;
                    RaisePropertyChanged(() => Conjoint);
                }
            }
        }
        public string TelephoneConjoint
        {
            get
            {
                return _telephoneConjoint;
            }
            set
            {
                if (value != _telephoneConjoint)
                {
                    _telephoneConjoint = value;
                    RaisePropertyChanged(() => TelephoneConjoint);
                }
            }
        }

        public DateTime TargetRetraiteTime { get; set; }

        public int Age
        {
            get
            {
                return (DateTime.Today.Year - DateNaissance.Year) - (DateTime.Today.Month < DateNaissance.Month ? 1 :
                    DateTime.Today.Month > DateNaissance.Month ? 0 : DateTime.Today.Day < DateNaissance.Day ? 1 : 0);
            }
        }

        public int TargetRetraiteAge
        {
            get
            {
                return (TargetRetraiteTime.Year - DateNaissance.Year) - (TargetRetraiteTime.Month < DateNaissance.Month ? 1 :
                    TargetRetraiteTime.Month > DateNaissance.Month ? 0 : TargetRetraiteTime.Day < DateNaissance.Day ? 1 : 0);
            }
        }

        //public int AnneesService
        //{
        //    get
        //    {
        //        if (string.IsNullOrWhiteSpace(GradeEngagement.Id))
        //            return 0;

        //        return (DateTime.Today.Year - GradeEngagement.Date.Year) - (DateTime.Today.Month < GradeEngagement.Date.Month ? 1 :
        //            DateTime.Today.Month > GradeEngagement.Date.Month ? 0 : DateTime.Today.Day < GradeEngagement.Date.Day ? 1 : 0);
        //    }
        //}

        //public int TargetServiceAnneesService
        //{
        //    get
        //    {
        //        if (string.IsNullOrWhiteSpace(GradeEngagement.Id))
        //            return 0;

        //        return (TargetRetraiteTime.Year - GradeEngagement.Date.Year) - (TargetRetraiteTime.Month < GradeEngagement.Date.Month ? 1 :
        //            TargetRetraiteTime.Month > GradeEngagement.Date.Month ? 0 : TargetRetraiteTime.Day < GradeEngagement.Date.Day ? 1 : 0);
        //    }
        //}

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var employe = (Employe)obj;

            return (!string.IsNullOrWhiteSpace(Id) && employe.Id == Id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private Employe backup;
        public void BeginEdit()
        {
            backup = Clone() as Employe;
        }

        public void EndEdit()
        {
        }
        public void CancelEdit()
        {
            if (backup == null)
                return;

            Id = backup.Id;
            Nom = backup.Nom;
            PostNom = backup.PostNom;
            Prenom = backup.Prenom;
            Sexe = backup.Sexe;
            Photo = backup.Photo;
            EtatCivil = backup.EtatCivil;

            LieuNaissance = backup.LieuNaissance;
            DateNaissance = backup.DateNaissance;
            ImmatriculationCNSSAP = backup.ImmatriculationCNSSAP;
            GradeEngagement = backup.GradeEngagement;
        
            CurrentFonctionNomination = backup.CurrentFonctionNomination;
            CurrentAffectation = backup.CurrentAffectation;

            ProvinceOrigine = backup.ProvinceOrigine;

            PersonneContact = backup.PersonneContact;
            QualiteContact = backup.QualiteContact;

            EstMecaniseSalaire = backup.EstMecaniseSalaire;
            EstMecanisePrime = backup.EstMecanisePrime;
            EstRecense = backup.EstRecense;
            EstAffecte = backup.EstAffecte;
            Telephone = backup.Telephone;

            Email = backup.Email;
            Address = backup.Address;
            Conjoint = backup.Conjoint;
            TelephoneConjoint = backup.TelephoneConjoint;
        }


        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case "Nom":
                        if (string.IsNullOrWhiteSpace(Nom))
                            error = "Le nom de l'employé ne peut être vide.";
                        break;

                    case "PostNom":
                        if (string.IsNullOrWhiteSpace(PostNom))
                            error = "Le postnom de l'employé ne peut être vide.";
                        break;

                    case "Prenom":
                        if (string.IsNullOrWhiteSpace(Prenom))
                            error = "Le prénom de l'employé ne peut être vide.";
                        break;
                        
                    case "LieuNaissance":
                        if (string.IsNullOrWhiteSpace(LieuNaissance))
                            error = "Le lieu de naissance de l'employé ne peut être vide.";
                        break;

                    //case "Etat":
                    //    if (Etat == null)
                    //        error = "La position administrative de l'employé doit être indiquée.";
                    //    break;

                    case "DateNaissance":
                        if (DateNaissance == new DateTime())
                            error = "La date de naissance de l'employé doit être renseignée.";
                        else if(DateTime.Today.Subtract(DateNaissance).TotalDays / 360 >= 55)
                                error = "La date de naissance de l'employé est trop grande pour un employé encors actif.";
                        else if (DateTime.Today.Subtract(DateNaissance).TotalDays / 360 < 18)
                            error = "La date de naissance de l'employé est trop petite pour un employé ordinaire.";
                        break;

                    case "Email":
                        if (!string.IsNullOrWhiteSpace(Email) && !ValueValidator.IsValidEmail(Email))
                            error = "L'adresse email saisie n'est pas valide.";
                        break;

                    case "Telephone":
                        if (string.IsNullOrWhiteSpace(Telephone))
                            error = "Le téléphone de l'employé ne peut être vide.";
                        else if (!ValueValidator.IsValidPhoneNumber(Telephone))
                            error = "Le numéro de téléphone saisi n'est pas valide.";
                        break;

                    case "PersonneContact":
                        if (string.IsNullOrWhiteSpace(PersonneContact))
                            error = "La personne à contacter doit être spécifiée.";
                        break;

                    case "QualiteContact":
                        if (string.IsNullOrWhiteSpace(QualiteContact))
                            error = "La qualité de la personne à contacter doit être spécifiée.";
                        break;

                    case "Conjoint":
                        if (EstMarie && string.IsNullOrWhiteSpace(Conjoint))
                            error = "Les noms du (de la) conjoint(e) doivent être spécifiés.";
                        break;

                    case "TelephoneConjoint":
                        if (EstMarie)
                        {
                            if (string.IsNullOrWhiteSpace(TelephoneConjoint))
                                error = "Le téléphone du (de la) conjoint(e) doit être spécifié.";
                            else if (!ValueValidator.IsValidPhoneNumber(TelephoneConjoint))
                                error = "Le numéro de téléphone saisi n'est pas valide.";
                        }
                            
                        break;

                    case "GradeEngagement":
                        error = GradeEngagement.Error;
                        break;

                    case "CurrentFonctionNomination":
                        if (CurrentFonctionNomination.IsRequired)
                            error = CurrentFonctionNomination.Error;
                        break;

                    case "Enfants":
                        foreach (var enfant in Enfants)
                            if (enfant.Error != null)
                            {
                                error = enfant.Error;
                                break;
                            }
                        break;

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
                if (this["Nom"] != string.Empty)
                    return this["Nom"];
                else if (this["PostNom"] != string.Empty)
                    return this["PostNom"];
                else if (this["Prenom"] != string.Empty)
                    return this["Prenom"];
                else if (this["Sexe"] != string.Empty)
                    return this["Sexe"];
                else if (this["EtatCivil"] != string.Empty)
                    return this["EtatCivil"];

                else if (this["LieuNaissance"] != string.Empty)
                    return this["LieuNaissance"];
                
                else if (this["DateNaissance"] != string.Empty)
                    return this["DateNaissance"];
                
                else if (this["Email"] != string.Empty)
                    return this["Email"]; 

                else if (this["Telephone"] != string.Empty)
                    return this["Telephone"];
               
                else if (this["ActeEngagement"] != string.Empty)
                    return this["ActeEngagement"];

                return string.Empty;
            }
        }

        public string Name
        {
            get
            {
                return Nom + " " + PostNom + " " + Prenom;
            }
        }

        public string Initials
        {
            get
            {
                return Prenom?.Substring(0,1) + "" + Nom?.Substring(0, 1) + "" + PostNom?.Substring(0, 1);
            }
        }
    }
}
