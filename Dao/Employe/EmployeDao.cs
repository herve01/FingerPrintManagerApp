using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class EmployeDao : Dao<Model.Employe.Employe>
    {
        public EmployeDao(DbConnection connection = null) : base(connection)
        {
            TableName = "employe";
        }

        public override int Add(Model.Employe.Employe instance)
        {
            try
            {
                Request.Transaction = Connection.BeginTransaction();

                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into employe(id, matricule, nom, post_nom, prenom, sexe, photo, etat_civil, lieu_naissance, date_naissance, " +
                   "province_origine_id, personne_contact, qualite_contact, est_affecte, " +
                   "telephone, email, numero, avenue, commune_id, conjoint, telephone_conjoint, created_at, updated_at) " +

                   "values(@v_id, @v_matricule, @v_nom, @v_post_nom, @v_prenom, @v_sexe, @v_photo, @v_etat_civil, @v_lieu_naissance, @v_date_naissance, " +
                   "@v_province_origine_id, @v_personne_contact, @v_qualite_contact, @v_est_affecte, " +
                   "@v_telephone, @v_email, @v_numero, @v_avenue, @v_commune_id, @v_conjoint, @v_telephone_conjoint, now(), now())";


                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_matricule", DbType.String, instance.Matricule));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_nom", DbType.String, instance.Nom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_post_nom", DbType.String, instance.PostNom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_prenom", DbType.String, instance.Prenom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_sexe", DbType.String, instance.Sexe));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_photo", DbType.Binary, instance.Photo));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_etat_civil", DbType.String, instance.EtatCivil));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_lieu_naissance", DbType.String, instance.LieuNaissance));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date_naissance", DbType.Date, instance.DateNaissance));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_province_origine_id", DbType.Int32, instance.ProvinceOrigine.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_personne_contact", DbType.String, instance.PersonneContact));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_qualite_contact", DbType.String, instance.QualiteContact));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_affecte", DbType.Boolean, instance.EstAffecte));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_telephone", DbType.String, instance.Telephone));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_email", DbType.String, instance.Email));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_numero", DbType.String, instance.Address.Number));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_avenue", DbType.String, instance.Address.Street));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_commune_id", DbType.String, instance.Address.Commune.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_conjoint", DbType.String, instance.Conjoint));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_telephone_conjoint", DbType.String, instance.TelephoneConjoint));


                var feed = Request.ExecuteNonQuery();

                if (feed <= 0)
                {
                    Request.Transaction.Rollback();
                    return -1;
                }

                instance.Id = id;

                foreach (var empreinte in instance.Empreintes)
                    if (new EmployeEmpreinteDao().Add(Request, empreinte) <= 0)
                    {
                        instance.Id = null;
                        Request.Transaction.Rollback();

                        return -2;
                    }

                if (instance.EstAffecte && new AffectationDao().Add(Request, instance.CurrentAffectation) <= 0)
                {
                    instance.Id = null;
                    Request.Transaction.Rollback();

                    return -3;
                }

                if (instance.CurrentFonctionNomination.IsRequired && new EmployeFonctionDao().Add(Request, instance.CurrentFonctionNomination) <= 0)
                {
                    instance.Id = null;
                    Request.Transaction.Rollback();

                    return -4;
                }

                if (!instance.CurrentGrade.Equals(instance.CurrentGrade) && new EmployeGradeDao().Add(Request, instance.CurrentGrade) <= 0)
                {
                    instance.Id = null;
                    Request.Transaction.Rollback();

                    return -5;
                }

                if (!instance.CurrentGrade.Equals(instance.CurrentGradeNomination) && new EmployeGradeDao().Add(Request, instance.CurrentGradeNomination) <= 0)
                {
                    instance.Id = null;
                    Request.Transaction.Rollback();

                    return -6;
                }

                foreach (var enfant in instance.Enfants)
                    if (new EnfantEmployeDao().Add(Request, enfant) <= 0)
                    {
                        instance.Id = null;
                        Request.Transaction.Rollback();

                        return -7;
                    }

                if (new EmployeEtudeDao().Add(Request, instance.CurrentHighEtude) <= 0)
                {
                    instance.Id = null;
                    Request.Transaction.Rollback();

                    return -8;
                }

                Request.Transaction.Commit();

                return feed;
            }
            catch (Exception)
            {
                Request.Transaction.Rollback();
                return -10;
            }
        }

        public int Add(DbCommand command, Model.Employe.Employe employe)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(employe);
        }

        public async Task<int> AddAsync(Model.Employe.Employe instance)
        {
            try
            {
                Request.Transaction = Connection.BeginTransaction();

                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into employe(id, matricule, nom, post_nom, prenom, sexe, photo, etat_civil, lieu_naissance, date_naissance, " +
                   "province_origine_id, personne_contact, qualite_contact, est_affecte, " +
                   "telephone, email, numero, avenue, commune_id, conjoint, telephone_conjoint, created_at, updated_at) " +

                   "values(@v_id, @v_matricule, @v_nom, @v_post_nom, @v_prenom, @v_sexe, @v_photo, @v_etat_civil, @v_lieu_naissance, @v_date_naissance, " +
                   "@v_province_origine_id, @v_personne_contact, @v_qualite_contact, @v_est_affecte, " +
                   "@v_telephone, @v_email, @v_numero, @v_avenue, @v_commune_id, @v_conjoint, @v_telephone_conjoint, now(), now())";


                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_matricule", DbType.String, instance.Matricule));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_nom", DbType.String, instance.Nom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_post_nom", DbType.String, instance.PostNom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_prenom", DbType.String, instance.Prenom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_sexe", DbType.String, instance.Sexe));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_photo", DbType.Binary, instance.Photo));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_etat_civil", DbType.String, instance.EtatCivil));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_lieu_naissance", DbType.String, instance.LieuNaissance));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date_naissance", DbType.Date, instance.DateNaissance));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_province_origine_id", DbType.Int32, instance.ProvinceOrigine.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_personne_contact", DbType.String, instance.PersonneContact));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_qualite_contact", DbType.String, instance.QualiteContact));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_affecte", DbType.Boolean, instance.EstAffecte));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_telephone", DbType.String, instance.Telephone));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_email", DbType.String, instance.Email));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_numero", DbType.String, instance.Address.Number));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_avenue", DbType.String, instance.Address.Street));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_commune_id", DbType.String, instance.Address.Commune.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_conjoint", DbType.String, instance.Conjoint));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_telephone_conjoint", DbType.String, instance.TelephoneConjoint));

                var feed =await Request.ExecuteNonQueryAsync();

                if (feed <= 0)
                {
                    Request.Transaction.Rollback();
                    return -1;
                }

                instance.Id = id;

                foreach (var empreinte in instance.Empreintes)
                    if (await new EmployeEmpreinteDao().AddAsync(Request, empreinte) <= 0)
                    {
                        instance.Id = null;
                        Request.Transaction.Rollback();

                        return -2;
                    }

                if (instance.EstAffecte && await new AffectationDao().AddAsync(Request, instance.CurrentAffectation) <= 0)
                {
                    instance.Id = null;
                    Request.Transaction.Rollback();

                    return -3;
                }

                if (await new EmployeFonctionDao().AddAsync(Request, instance.CurrentFonctionNomination) <= 0)
                {
                    instance.Id = null;
                    Request.Transaction.Rollback();

                    return -4;
                }

                if (!instance.CurrentGradeNomination.Equals(instance.CurrentGrade) && await new EmployeGradeDao().AddAsync(Request, instance.CurrentGrade) <= 0)
                {
                    instance.Id = null;
                    Request.Transaction.Rollback();

                    return -5;
                }

                if (!instance.CurrentGrade.Equals(instance.CurrentGradeNomination) && await new EmployeGradeDao().AddAsync(Request, instance.CurrentGradeNomination) <= 0)
                {
                    instance.Id = null;
                    Request.Transaction.Rollback();

                    return -6;
                }

                foreach (var enfant in instance.Enfants)
                    if (await new EnfantEmployeDao().AddAsync(Request, enfant) <= 0)
                    {
                        instance.Id = null;
                        Request.Transaction.Rollback();

                        return -7;
                    }

                if (await new EmployeEtudeDao().AddAsync(Request, instance.CurrentHighEtude) <= 0)
                {
                    instance.Id = null;
                    Request.Transaction.Rollback();

                    return -8;
                }

                Request.Transaction.Commit();

                return feed;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Request.Transaction.Rollback();
                return -10;
            }
        }

        public async Task<int> AddAsync(DbCommand command, Model.Employe.Employe instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public override int Update(Model.Employe.Employe instance, Model.Employe.Employe old)
        {
            try
            {
                Request.CommandText = "update employe " +
                   "set matricule = @v_matricule, " +
                   "nom = @v_nom, " +
                   "post_nom = @v_post_nom, " +
                   "prenom = @v_prenom, " +
                   "sexe = @v_sexe, " +
                   "photo = @v_photo, " +
                   "etat_civil = @v_etat_civil, " +
                   "lieu_naissance = @v_lieu_naissance, " +
                   "date_naissance = @v_date_naissance, " +
                   "province_origine_id = @v_province_origine_id, " +
                   "personne_contact = @v_personne_contact, " +
                   "qualite_contact = @v_qualite_contact, " +
                   "est_affecte = @v_est_affecte, " +
                   "telephone = @v_telephone, " +
                   "email = @v_email, " +
                   "numero = @v_numero, " +
                   "avenue = @v_avenue, " +
                   "commune_id = @v_commune_id, " +
                   "conjoint = @v_conjoint, " +
                   "telephone_conjoint = @v_telephone_conjoint, " +
                   "updated_at = now() " +
                   "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_matricule", DbType.String, instance.Matricule));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_nom", DbType.String, instance.Nom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_post_nom", DbType.String, instance.PostNom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_prenom", DbType.String, instance.Prenom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_sexe", DbType.String, instance.Sexe));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_photo", DbType.Binary, instance.Photo));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_etat_civil", DbType.String, instance.EtatCivil));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_lieu_naissance", DbType.String, instance.LieuNaissance));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date_naissance", DbType.Date, instance.DateNaissance));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_province_origine_id", DbType.Int32, instance.ProvinceOrigine.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_personne_contact", DbType.String, instance.PersonneContact));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_qualite_contact", DbType.String, instance.QualiteContact));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_affecte", DbType.Boolean, instance.EstAffecte));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_telephone", DbType.String, instance.Telephone));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_email", DbType.String, instance.Email));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_numero", DbType.String, instance.Address.Number));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_avenue", DbType.String, instance.Address.Street));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_commune_id", DbType.String, instance.Address.Commune.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_conjoint", DbType.String, instance.Conjoint));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_telephone_conjoint", DbType.String, instance.TelephoneConjoint));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -6;
            }
        }

        public int Update(DbCommand command, Model.Employe.Employe instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Update(instance, null);
        }

        public async Task<int> UpdateAsync(Model.Employe.Employe instance, Model.Employe.Employe old)
        {
            try
            {
                Request.CommandText = "update employe " +
                   "set matricule = @v_matricule, " +
                   "nom = @v_nom, " +
                   "post_nom = @v_post_nom, " +
                   "prenom = @v_prenom, " +
                   "sexe = @v_sexe, " +
                   "photo = @v_photo, " +
                   "etat_civil = @v_etat_civil, " +
                   "lieu_naissance = @v_lieu_naissance, " +
                   "date_naissance = @v_date_naissance, " +
                   "province_origine_id = @v_province_origine_id, " +
                   "personne_contact = @v_personne_contact, " +
                   "qualite_contact = @v_qualite_contact, " +
                   "est_affecte = @v_est_affecte, " +
                   "telephone = @v_telephone, " +
                   "email = @v_email, " +
                   "numero = @v_numero, " +
                   "avenue = @v_avenue, " +
                   "commune_id = @v_commune_id, " +
                   "conjoint = @v_conjoint, " +
                   "telephone_conjoint = @v_telephone_conjoint, " +
                   "updated_at = now() " +
                   "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_matricule", DbType.String, instance.Matricule));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_nom", DbType.String, instance.Nom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_post_nom", DbType.String, instance.PostNom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_prenom", DbType.String, instance.Prenom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_sexe", DbType.String, instance.Sexe));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_photo", DbType.Binary, instance.Photo));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_etat_civil", DbType.String, instance.EtatCivil));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_lieu_naissance", DbType.String, instance.LieuNaissance));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date_naissance", DbType.Date, instance.DateNaissance));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_province_origine_id", DbType.Int32, instance.ProvinceOrigine.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_personne_contact", DbType.String, instance.PersonneContact));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_qualite_contact", DbType.String, instance.QualiteContact));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_affecte", DbType.Boolean, instance.EstAffecte));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_telephone", DbType.String, instance.Telephone));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_email", DbType.String, instance.Email));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_numero", DbType.String, instance.Address.Number));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_avenue", DbType.String, instance.Address.Street));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_commune_id", DbType.String, instance.Address.Commune.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_conjoint", DbType.String, instance.Conjoint));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_telephone_conjoint", DbType.String, instance.TelephoneConjoint));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));


                var feed = await Request.ExecuteNonQueryAsync();

                return feed;
            }
            catch (Exception)
            {
                return -6;
            }
        }

        public async Task<int> UpdateAsync(DbCommand command, Model.Employe.Employe instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await UpdateAsync(Request, instance);
        }

        public override int Delete(Model.Employe.Employe instance)
        {
            try
            {

                Request.CommandText = "delete from employe " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int SetPosition(DbCommand command, Model.Employe.Employe instance)
        {
            Request = command;
            Request.Parameters.Clear();

            try
            {
                Request.CommandText = "update employe " +
                    "set est_affecte = @v_est_affecte, " +
                    "updated_at = now() " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_affecte", DbType.Boolean, instance.EstAffecte));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int SetEstMecaniseSalaire(DbCommand command, Model.Employe.Employe instance)
        {
            Request = command;
            Request.Parameters.Clear();

            try
            {
                Request.CommandText = "update employe " +
                    "set est_mecanise_salaire = 1, " +
                    "updated_at = now() " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int SetEstMecanisePrime(DbCommand command, Model.Employe.Employe instance)
        {
            Request = command;
            Request.Parameters.Clear();

            try
            {
                Request.CommandText = "update employe " +
                    "set est_mecanise_prime = 1, " +
                    "updated_at = now() " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int SetEstAffecte(DbCommand command, Model.Employe.Employe instance, bool estAffecte)
        {
            Request = command;
            Request.Parameters.Clear();

            try
            {
                Request.CommandText = "update employe " +
                    "set est_affecte = @v_val, " +
                    "updated_at = now() " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_val", DbType.Boolean, estAffecte));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int SetEstRecense(DbCommand command, Model.Employe.Employe instance)
        {
            Request = command;
            Request.Parameters.Clear();

            try
            {
                Request.CommandText = "update employe " +
                    "set est_recense = 1, " +
                    "updated_at = now() " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<int> SetPhoto(Model.Employe.Employe instance, byte[] photo)
        {
            try
            {
                Request.CommandText = "update employe " +
                    "set photo = @v_photo, " +
                    "updated_at = now() " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_photo", DbType.Binary, photo));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = await Request.ExecuteNonQueryAsync();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private Model.Employe.Employe Create(Dictionary<string, object> row)
        {
            var instance = new Model.Employe.Employe();

            instance.Id = row["id"].ToString();
            instance.Nom = row["nom"].ToString();
            instance.PostNom = row["post_nom"].ToString();
            instance.Prenom = row["prenom"].ToString();
            instance.Sexe = Util.ToSexeType(row["sexe"].ToString());
            instance.EtatCivil = Util.ToEtatCivil(row["etat_civil"].ToString());

            instance.LieuNaissance = row["lieu_naissance"].ToString();
            instance.DateNaissance = DateTime.Parse(row["date_naissance"].ToString());

            instance.ProvinceOrigine = new ProvinceDao(Connection).Get(int.Parse(row["province_origine_id"].ToString()));
            instance.PersonneContact = row["personne_contact"].ToString();
            instance.QualiteContact = row["qualite_contact"].ToString();

            instance.EstAffecte = bool.Parse(row["est_affecte"].ToString());
            instance.Telephone = row["telephone"].ToString();
            instance.Email = row["email"].ToString();
            instance.Address.Number = row["numero"].ToString();
            instance.Address.Street = row["avenue"].ToString();
            instance.Address.Commune = new CommuneDao(Connection).GetCommune(int.Parse(row["commune_id"].ToString()));

            instance.CurrentGradeNomination = new EmployeGradeDao().GetCurrent(instance, GradeEmployeType.Officiel);
            instance.CurrentGrade = new EmployeGradeDao().GetInitial(instance);

            instance.LastAffectation = new AffectationDao().GetCurrent(instance);

            if (instance.EstAffecte)
            {
                instance.CurrentAffectation = instance.LastAffectation;
                //instance.CurrentFonctionNomination = new EmployeFonctionDao().GetCurrent(instance);
                instance.FonctionsInterim = new EmployeFonctionDao().GetAllInterim(instance);             
            }

            if (!(row["photo"] is DBNull))
                instance.Photo = (byte[])row["photo"];
        
            if (!(row["matricule"] is DBNull))
                instance.Matricule = row["matricule"].ToString();

            if (!(row["conjoint"] is DBNull))
                instance.Conjoint = row["conjoint"].ToString();

            if (!(row["telephone_conjoint"] is DBNull))
                instance.TelephoneConjoint = row["telephone_conjoint"].ToString();


            instance.Empreintes = new EmployeEmpreinteDao().GetAll(instance);

            return instance;

        }

        public int Count()
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from employe";

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public int Count(string matricule)
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from employe " +
                    "where matricule = @v_matricule";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_matricule", DbType.String, matricule));

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public int Count(string matricule, Model.Employe.Employe employe)
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from employe " +
                    "where matricule = @v_matricule and employe_id != @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_matricule", DbType.String, matricule));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, employe));

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public int Count(Entite entite)
        {
            var estSiege = entite.EstPrincipale ? 1 : 0;

            try
            {
                Request.CommandText = "select count(*) " +
                    "from employe " +
                    "where @v_siege = 1 or get_employe_current_entite(E.id) = @v_entite_id";


                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_siege", DbType.Int32, estSiege));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public Model.Employe.Employe Get(string id)
        {
            Model.Employe.Employe instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from employe " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _instances = Map(Reader);

                Reader.Close();

                if (_instances != null)
                    instance = Create(_instances);

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }

        public Model.Employe.Employe GetCurrent()
        {
            Model.Employe.Employe instance = null;
            Dictionary<string, object> _instance = null;

            try
            {
                Request.CommandText = "select * " +
                    "from employe limit 1";


                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _instance = Map(Reader);

                Reader.Close();

                if (_instance != null)
                    instance = Create(_instance);

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }

        public async Task<List<Model.Employe.Employe>> GetAllAsync()
        {
            var intances = new List<Model.Employe.Employe>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from employe ";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Model.Employe.Employe employe = Create(item);
                    intances.Add(employe);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<Model.Employe.Employe>> GetAllAsync(Entite entite, DateTime lastUpdateTime)
        {
            var intances = new List<Model.Employe.Employe>();
            var _instances = new List<Dictionary<string, object>>();

            var estSiege = entite.EstPrincipale ? 1 : 0;

            try
            {
                Request.CommandText = "select E.* " +
                    "from employe E " +
                    "inner join position_administrative P " +
                    "on E.position_id = P.id " +
                    "where P.is_down = 0 and (@v_siege = 1 or get_employe_current_entite(E.id) = @v_entite_id) " +
                    "and updated_at >= @v_time";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_siege", DbType.Int32, estSiege));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_time", DbType.DateTime, lastUpdateTime));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Model.Employe.Employe employe = Create(item);
                    intances.Add(employe);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task GetAllAsync(ObservableCollection<Model.Employe.Employe> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from employe";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Model.Employe.Employe employe = Create(item);
                    collection.Add(employe);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

        }

        public async Task GetAllAsync(Entite entite, ObservableCollection<Model.Employe.Employe> collection, bool seeAll = true)
        {
            var _instances = new List<Dictionary<string, object>>();

            var estSiege = !seeAll ? 0 : entite.EstPrincipale ? 1 : 0;

            try
            {
                Request.CommandText = "select * " +
                    "from employe " +
                    "where @v_siege = 1 or get_employe_current_entite(id) = @v_entite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_siege", DbType.Int32, estSiege));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Model.Employe.Employe employe = Create(item);
                    collection.Add(employe);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

        }

        public async Task GetAllActifsAsync(Entite entite, ObservableCollection<Model.Employe.Employe> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            var estSiege = entite.EstPrincipale ? 1 : 0;
         

            try
            {
                Request.CommandText = "select E.* " +
                    "from employe E " +
                    "inner join position_administrative P " +
                    "on E.position_id = P.id " +
                    "where P.intitule = 'Actif' and ((@v_siege = 1 and get_employe_current_entite(E.id) is null) or get_employe_current_entite(E.id) = @v_entite_id)";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_siege", DbType.Int32, estSiege));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Model.Employe.Employe employe = Create(item);
                    collection.Add(employe);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

        }

        public async Task GetAllEligibleAsync(Entite entite, DateTime targetDate, int limitAge, int limitService, ObservableCollection<Model.Employe.Employe> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            var estSiege = entite.EstPrincipale ? 1 : 0;

            try
            {
                Request.CommandText = "sp_employes_retraite";
                Request.CommandType = CommandType.StoredProcedure;

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_target_date", DbType.Date, targetDate.Date));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_age_limit", DbType.Int32, limitAge));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_service_limit", DbType.Int32, limitService));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Model.Employe.Employe employe = Create(item);
                    employe.TargetRetraiteTime = targetDate;
                    collection.Add(employe);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

        }

        protected override Dictionary<string, object> Map(DbDataReader reader)
        {
            return new Dictionary<string, object>()
            {
                { "id", reader["id"] },
                { "matricule", reader["matricule"] },
                { "nom", reader["nom"] },
                { "post_nom", reader["post_nom"] },
                { "prenom", reader["prenom"] },
                { "sexe", reader["sexe"] },
                { "photo", reader["photo"] },
                { "etat_civil", reader["etat_civil"] },
                { "lieu_naissance", reader["lieu_naissance"] },
                { "date_naissance", reader["date_naissance"] },
                { "province_origine_id", reader["province_origine_id"] },
                { "personne_contact", reader["personne_contact"] },
                { "qualite_contact", reader["qualite_contact"] },
                { "est_affecte", reader["est_affecte"] },
                { "telephone", reader["telephone"] },
                { "email", reader["email"] },
                { "numero", reader["numero"] },
                { "avenue", reader["avenue"] },
                { "commune_id", reader["commune_id"] },
                { "conjoint", reader["conjoint"] },
                { "telephone_conjoint", reader["telephone_conjoint"] }
            };
        }

        #region Reporting
        public async Task<DataTable> GetListeDeclarativeReportAsync(Entite entite, bool withOfficialGrades = false)
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                Request.CommandText = withOfficialGrades ? "sp_declarative_list_with_official_grade" : "sp_declarative_list";
                Request.CommandType = CommandType.StoredProcedure;
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        list.Add(new Dictionary<string, object>()
                        {

                            { "entite_id", "000001" },
                            { "nom", Reader["nom"] },
                            { "post_nom", Reader["post_nom"] },
                            { "prenom", Reader["prenom"] },
                            { "sexe", Reader["sexe"] },
                            { "matricule", Reader["matricule"] },
                            { "grade_id", Reader["grade_id"] },
                            { "grade_intitule", Reader["grade_intitule"] }
                        });

                Reader.Close();
                
                return DbUtil.DicToTable(list);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }
        public async Task<DataTable> GetListeFemmeReportAsync(Entite entite)
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                Request.CommandText = "sp_get_femme_list_report";
                Request.CommandType = CommandType.StoredProcedure;
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        list.Add(new Dictionary<string, object>()
                        {

                            { "entite_id", "000001" },
                            { "nom", Reader["nom"] },
                            { "post_nom", Reader["post_nom"] },
                            { "prenom", Reader["prenom"] },
                            { "sexe", Reader["sexe"] },
                            { "matricule", Reader["matricule"] },
                            { "grade_id", Reader["grade_id"] },
                            { "grade_intitule", Reader["grade_intitule"] },
                            { "entite_name", Reader["entite_name"] }
                        });

                Reader.Close();

                return DbUtil.DicToTable(list);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }
        public async Task<DataTable> GetAgentsAffectesReportAsync(Entite entite)
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                Request.CommandText = "sp_get_agents_affectes";
                Request.CommandType = CommandType.StoredProcedure;

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        list.Add(new Dictionary<string, object>()
                        {
                            { "entite_id", "000001" },
                            { "entite", Reader["entite"] },
                            { "est_siege", Reader["est_siege"] },
                            { "nom", Reader["nom"] },
                            { "post_nom", Reader["post_nom"] },
                            { "prenom", Reader["prenom"] },
                            { "grade", Reader["grade"] },
                            { "sexe", Reader["sexe"] },
                            { "matricule", Reader["matricule"] },
                            { "direction", Reader["direction"] },
                            { "departement", Reader["departement"] },
                            { "bureau", Reader["bureau"] }
                        });

                Reader.Close();

                return DbUtil.DicToTable(list);
            }
            catch (Exception ex)
            {
                if (Reader != null)
                    Reader.Close();

                Console.Write(ex.Message);

                return null;
            }
        }
        
        public async Task<DataTable> GetAgentDecedeReportAsync()
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                Request.CommandText = "sp_get_agent_decede";
                Request.CommandType = CommandType.StoredProcedure;
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, AppConfig.EntiteId));
                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        list.Add(new Dictionary<string, object>()
                        {
                            { "nom", Reader["nom"] },
                            { "post_nom", Reader["post_nom"] },
                            { "prenom", Reader["prenom"] },
                            { "sexe", Reader["sexe"] },
                            { "grade", Reader["grade"] },
                            { "date_deces", Reader["date_deces"] },
                            { "niveau", Reader["niveau"] },
                            { "entite_id", Reader["entite_id"] }
                        });

                Reader.Close();

                var table = DbUtil.DicToTable(list);
                return table;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        public async Task<DataTable> GetAgentEligibreRetraiteReportAsync(Entite entite, DateTime targetDate, int ageLimit, int serviceLimit)
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                Request.CommandText = "sp_employes_retraite_report";
                Request.CommandType = CommandType.StoredProcedure;
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_target_date", DbType.Date, targetDate));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_age_limit", DbType.Int32, ageLimit));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_service_limit", DbType.Int32, serviceLimit));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        list.Add(new Dictionary<string, object>()
                        {
                            { "nom", Reader["nom"] },
                            { "post_nom", Reader["post_nom"] },
                            { "prenom", Reader["prenom"] },
                            { "sexe", Reader["sexe"] },
                            { "matricule", Reader["matricule"] },
                            { "date_naissance", Reader["date_naissance"] },
                            { "age", Reader["age"] },
                            { "date_engagement", Reader["date_engagement"] },
                            { "anciennete", Reader["anciennete"] },
                            { "grade", Reader["grade"] },
                            { "entite_id", "000001" },
                            { "entite_name", Reader["entite_name"] }
                        });

                Reader.Close();

                var table = DbUtil.DicToTable(list);
                return table;
            }
            catch (Exception ex)
            {
                if (Reader != null)
                    Reader.Close();

                Console.Write(ex.Message);

                return null;
            }
        }

        public async Task<DataTable> GetCarteServiceReportAsync(Model.Employe.Employe employe, byte[] signature)
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                Request.CommandText = "sp_get_carte_service";
                Request.CommandType = CommandType.StoredProcedure;
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        list.Add(new Dictionary<string, object>()
                        {
                            { "entite_id", "000001" },
                            { "numero", Reader["numero"] },
                            { "nom", Reader["nom"] },
                            { "post_nom", Reader["post_nom"] },
                            { "prenom", Reader["prenom"] },
                            { "sexe", Reader["sexe"] },
                            { "matricule", Reader["matricule"] },
                            { "grade", Reader["grade"] },
                            { "fonction", Reader["fonction"] },
                            { "photo", Reader["photo"] },
                            { "code_barre", null },
                            { "signature_dg", signature },
                        });

                Reader.Close();

                list.ForEach(d => d["code_barre"] = Model.Helper.ImageUtil.GetCodeQR(d["matricule"].ToString()));

                return DbUtil.DicToTable(list);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        //public async Task<DataTable> GetPosteVacantReportAsync()
        //{
        //    var list = new List<Dictionary<string, object>>();
        //    try
        //    {
        //        Request.CommandText = "sp_get_agent_mecanise";
        //        Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, AppConfig.EntiteId));
        //        Reader = await Request.ExecuteReaderAsync();

        //        if (Reader.HasRows)
        //            while (Reader.Read())
        //                list.Add(new Dictionary<string, object>()
        //                {
        //                    { "nom", Reader["nom"] },
        //                    { "postnom", Reader["post_nom"] },
        //                    { "prenom", Reader["prenom"] },
        //                    { "sexe", Reader["sexe"] },
        //                    { "grade", Reader["grade"] },
        //                    { "matricule", Reader["matricule"] },
        //                });

        //        Reader.Close();

        //        var table = DbUtil.DicToTable(list);
        //        table.TableName = "declarativeList";
        //        return table;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //        return null;
        //    }
        //}

        public async Task<DataTable> GetAgentMecaniseReportAsync(Entite entite, bool estSalaire = false, bool estPrime = false)
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                
                Request.CommandText = estSalaire != estPrime ? "sp_agents_mecanises" : "sp_agents_mecanises_2";
                               
                Request.CommandType = CommandType.StoredProcedure;
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                if (estPrime != estSalaire)
                {
                    Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_salaire", DbType.Boolean, estSalaire));
                    Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_prime", DbType.Boolean, estPrime));
                }
                else
                    Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mecanise", DbType.Boolean, estSalaire));


                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        list.Add(new Dictionary<string, object>()
                        {
                            { "entite_id", "000001" },
                            { "nom", Reader["nom"] },
                            { "post_nom", Reader["post_nom"] },
                            { "prenom", Reader["prenom"] },
                            { "sexe", Reader["sexe"] },
                            { "grade", Reader["grade"] },
                            { "matricule", Reader["matricule"] },
                            { "niveau", Reader["niveau"] },
                            { "est_mecanise_salaire", Reader["est_mecanise_salaire"] },
                            { "est_mecanise_prime", Reader["est_mecanise_prime"] },
                        });

                Reader.Close();

                var table = DbUtil.DicToTable(list);
                return table;
            }
            catch (Exception ex)
            {
                if (Reader != null)
                    Reader.Close();

                Console.Write(ex.Message);
                return null;
            }
        }

        public async Task<DataTable> GetGradeEffectifReportAsync(Entite entite)
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                Request.Parameters.Clear();

                Request.CommandText = "sp_grade_effectif";
                Request.CommandType = CommandType.StoredProcedure;

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        list.Add(new Dictionary<string, object>()
                        {
                            { "entite_id", "000001" },
                            { "grade_id", Reader["grade_id"] },
                            { "grade", Reader["grade"] },
                            { "effectif", Reader["effectif"] },
                            { "niveau", Reader["niveau"] }
                        });

                Reader.Close();

                var table = DbUtil.DicToTable(list);
                return table;
            }
            catch (Exception ex)
            {
                if (Reader != null)
                    Reader.Close();

                Console.Write(ex.Message);
                return null;
            }
        }

        public async Task<DataTable> GetAgentEnInstanceAffectationReportAsync(Entite entite)
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                Request.CommandText = "sp_agents_non_affectes";
                Request.CommandType = CommandType.StoredProcedure;
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        list.Add(new Dictionary<string, object>()
                        {
                            { "entite_id", "000001" },
                            { "nom", Reader["nom"] },
                            { "post_nom", Reader["post_nom"] },
                            { "prenom", Reader["prenom"] },
                            { "sexe", Reader["sexe"] },
                            { "grade_id", Reader["grade_id"] },
                            { "matricule", Reader["matricule"] },
                            { "niveau_etude", Reader["niveau_etude"] },
                            { "domaine_etude", Reader["domaine_etude"] },
                            { "entite", Reader["entite"] }
                        });

                Reader.Close();

                var table = DbUtil.DicToTable(list);
                table.TableName = "agentEnInstanceAffectation";
                return table;
            }
            catch (Exception ex)
            {
                if (Reader != null)
                    Reader.Close();

                Console.Write(ex.Message);
                return null;
            }
        }

        public async Task<DataTable> GetFicheIndividuelleReportAsync(Model.Employe.Employe employe, byte[] avatar)
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                Request.CommandText = "sp_employe_fiche";
                Request.CommandType = CommandType.StoredProcedure;
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        list.Add(new Dictionary<string, object>()
                        {
                            { "entite_id", "000001" },
                            { "id", Reader["id"] },
                            { "photo", Reader["photo"] is DBNull ? avatar : Reader["photo"] },
                            { "nom", Reader["nom"] },
                            { "post_nom", Reader["post_nom"] },
                            { "prenom", Reader["prenom"] },
                            { "etat_civil", Reader["etat_civil"] },
                            { "lieu_naissance", Reader["lieu_naissance"] },
                            { "date_naissance", Reader["date_naissance"] },
                            { "sexe", Reader["sexe"] },
                            { "niveau_etude", Reader["niveau_etude"] },
                            { "domaine_etude", Reader["domaine_etude"] },
                            { "annee_obtention", Reader["annee_obtention"].ToString().Replace(" ", "") },
                            { "province_origine", Reader["province_origine"] },
                            { "telephone", Reader["telephone"] },
                            { "email", Reader["email"] },
                            { "personne_contact", Reader["personne_contact"] },
                            { "qualite_contact", Reader["qualite_contact"] },
                            { "adresse_avenue", Reader["adresse_avenue"] },
                            { "adresse_numero", Reader["adresse_numero"] },
                            { "adresse_commune", Reader["adresse_commune"] },
                            { "adresse_zone", Reader["adresse_zone"] },
                            { "adresse_province", Reader["adresse_province"] },
                            { "position", Reader["position"] },
                            { "est_recense", Reader["est_recense"] },
                            { "est_mecanise_salaire", Reader["est_mecanise_salaire"] },
                            { "est_mecanise_prime", Reader["est_mecanise_prime"] },
                            { "est_affecte", Reader["est_affecte"] },
                            { "immatriculation_cnssap", Reader["immatriculation_cnssap"] },
                            { "date_engagement", Reader["date_engagement"] },
                            { "acte_engagement", Reader["acte_engagement"] },
                            { "grade_engagement_id", Reader["grade_engagement_id"] },
                            { "grade_engagement_intitule", Reader["grade_engagement_intitule"] },
                            { "matricule", Reader["matricule"] },
                            { "grade_statutaire_id", Reader["grade_statutaire_id"] },
                            { "grade_statutaire_intitule", Reader["grade_statutaire_intitule"] },
                            { "grade_statutaire_acte", Reader["grade_statutaire_acte"] },
                            { "grade_statutaire_date", Reader["grade_statutaire_date"] },
                            { "grade_actuel_id", Reader["grade_actuel_id"] },
                            { "grade_actuel_intitule", Reader["grade_actuel_intitule"] },
                            { "grade_actuel_acte", Reader["grade_actuel_acte"] },
                            { "grade_actuel_date", Reader["grade_actuel_date"] },
                            { "fonction_actuelle_intitule", Reader["fonction_actuelle_intitule"] },
                            { "bureau", Reader["bureau"] },
                            { "departement", Reader["departement"] },
                            { "direction", Reader["direction"] },
                            { "entite", Reader["entite"] },
                            { "conjoint", Reader["conjoint"] },
                            { "telephone_conjoint", Reader["telephone_conjoint"] },
                        });

                Reader.Close();

                var table = DbUtil.DicToTable(list);
                return table;
            }
            catch (Exception ex)
            {
                if (Reader != null)
                    Reader.Close();

                Console.Write(ex.Message);
                return null;
            }
        }

        public async Task<DataTable> GetAgentEnDetachementReportAsync()
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                Request.CommandText = "sp_employes_en_detachement";
                Request.CommandType = CommandType.StoredProcedure;
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, AppConfig.EntiteId));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        list.Add(new Dictionary<string, object>()
                        {
                            { "nom", Reader["nom"] },
                            { "post_nom", Reader["post_nom"] },
                            { "prenom", Reader["prenom"] },
                            { "sexe", Reader["sexe"] },
                            { "grade", Reader["grade"] },
                            { "matricule", Reader["matricule"] },
                            { "instance_affectation", Reader["instance_affectation"] },
                            { "entite_id", Reader["entite_id"] }
                        });

                Reader.Close();

                var table = DbUtil.DicToTable(list);
                return table;
            }
            catch (Exception ex)
            {
                if (Reader != null)
                    Reader.Close();

                Console.Write(ex.Message);
                return null;
            }
        }

        public async Task<DataTable> GetAgentEnDisponibiliteReportAsync()
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                Request.CommandText = "sp_get_agent_en_disponibilite";
                Request.CommandType = CommandType.StoredProcedure;
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, AppConfig.EntiteId));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        list.Add(new Dictionary<string, object>()
                        {
                            { "nom", Reader["nom"] },
                            { "post_nom", Reader["post_nom"] },
                            { "prenom", Reader["prenom"] },
                            { "sexe", Reader["sexe"] },
                            { "grade", Reader["grade"] },
                            { "matricule", Reader["matricule"] },
                            { "raison", Reader["raison"] },
                            { "entite_id", Reader["entite_id"] }
                        });

                Reader.Close();

                var table = DbUtil.DicToTable(list);
                return table;
            }
            catch (Exception ex)
            {
                if (Reader != null)
                    Reader.Close();

                Console.Write(ex.Message);
                return null;
            }
        }

        public async Task<DataTable> GetPresenceJournaliereReportAsync()
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                Request.CommandText = "sp_get_presence_journaliere";
                Request.CommandType = CommandType.StoredProcedure;
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, AppConfig.EntiteId));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        list.Add(new Dictionary<string, object>()
                        {
                            { "nom", Reader["nom"] },
                            { "postnom", Reader["post_nom"] },
                            { "prenom", Reader["prenom"] },
                            { "sexe", Reader["sexe"] },
                            { "grade", Reader["grade"] },
                            { "direction", Reader["direction"] },
                            { "date", Reader["date"] },
                            { "entite_id", Reader["entite_id"] }
                        });

                Reader.Close();

                var table = DbUtil.DicToTable(list);
                table.TableName = "presenceJournaliere";
                return table;
            }
            catch (Exception ex)
            {
                if (Reader != null)
                    Reader.Close();

                Console.Write(ex.Message);
                return null;
            }
        }

        #endregion
    }
}