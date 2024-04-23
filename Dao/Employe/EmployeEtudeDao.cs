using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class EmployeEtudeDao : Dao<EmployeEtude>
    {
        public EmployeEtudeDao(DbConnection connection = null) : base(connection)
        {
            TableName = "employe_etude";
        }

        public override int Add(EmployeEtude instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into employe_etude(id, employe_id, niveau_id, domaine_id, annee_obtention, created_at, updated_at) " +
                    "values(@v_id, @v_employe_id, @v_niveau_id, @v_domaine_id, @v_annee_obtention, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_niveau_id", DbType.String, instance.Niveau.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_domaine_id", DbType.String, instance.Domaine?.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_annee_obtention", DbType.Int32, instance.Annee));

                var feed = Request.ExecuteNonQuery();

                if (feed > 0)
                    instance.Id = id;


                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int Add(DbCommand command, EmployeEtude employe_etude)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(employe_etude);
        }

        public async Task<int> AddAsync(EmployeEtude instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into employe_etude(id, employe_id, niveau_id, domaine_id, annee_obtention, created_at, updated_at) " +
                    "values(@v_id, @v_employe_id, @v_niveau_id, @v_domaine_id, @v_annee_obtention, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_niveau_id", DbType.String, instance.Niveau.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_domaine_id", DbType.String, instance.Domaine?.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_annee_obtention", DbType.Int32, instance.Annee));

                var feed = await Request.ExecuteNonQueryAsync();

                if (feed > 0)
                    instance.Id = id;

                return feed;
            }
            catch (Exception)
            {
                return -6;
            }
        }

        public async Task<int> AddAsync(DbCommand command, EmployeEtude instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public override int Update(EmployeEtude instance, EmployeEtude old)
        {
            try
            {

                Request.CommandText = "update employe_etude " +
                    "set niveau_id = @v_niveau_id, " +
                    "employe_id = @v_employe_id, " +
                    "domaine_id = @v_domaine_id, " +
                    "annee_obtention = @v_annee_obtention, " +
                    "updated_at = now() " +
                    "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_niveau_id", DbType.String, instance.Niveau.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_domaine_id", DbType.String, instance.Domaine?.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_annee_obtention", DbType.Int32, instance.Annee));
                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -6;
            }
        }

        public override int Delete(EmployeEtude instance)
        {
            try
            {

                Request.CommandText = "delete from employe_etude " +
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

        private EmployeEtude Create(Dictionary<string, object> row, bool withEmploye)
        {
            EmployeEtude instance = new EmployeEtude();

            instance.Id = row["id"].ToString();
            instance.Niveau = new NiveauEtudeDao().Get(row["niveau_id"].ToString());
            instance.Annee = int.Parse(row["annee_obtention"].ToString());

            if (!(row["domaine_id"] is DBNull))
                instance.Domaine = new DomaineEtudeDao().Get(row["domaine_id"].ToString());

            if (withEmploye)
                instance.Employe = new EmployeDao().Get(row["employe_id"].ToString());

            return instance;
        }

        public int Count()
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from employe_etude";

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public EmployeEtude Get(string id)
        {
            EmployeEtude instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from employe_etude " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _instances = Map(Reader);

                Reader.Close();

                if (_instances != null)
                    instance = Create(_instances, true);

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }

        public EmployeEtude Get(Model.Employe.Employe employe)
        {
            EmployeEtude instance = null;
            Dictionary<string, object> _instance = null;

            try
            {
                Request.CommandText = "select ee.* " +
                    "from employe_etude ee " +
                    "inner join niveau_etude ne " +
                    "on ee.niveau_id = ne.id " +
                    "where ee.employe_id = @v_employe_id " +
                    "order by ne.niveau desc, annee_obtention desc " +
                    "limit 1";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _instance = Map(Reader);

                Reader.Close();

                if (_instance != null)
                {
                    instance = Create(_instance, false);
                    instance.Employe = employe;
                }

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }

        public List<EmployeEtude> GetAll(Model.Employe.Employe employe)
        {
            var intances = new List<EmployeEtude>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from employe_etude " +
                    "where employe_id = @v_employe_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var employe_etude = Create(item, false);
                    employe_etude.Employe = employe;
                    intances.Add(employe_etude);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        protected override Dictionary<string, object> Map(DbDataReader reader)
        {
            return new Dictionary<string, object>()
            {
                { "id", reader["id"] },
                { "employe_id", reader["employe_id"] },
                { "niveau_id", reader["niveau_id"] },
                { "domaine_id", reader["domaine_id"] },
                { "annee_obtention", reader["annee_obtention"] }
            };
        }
    }
}


