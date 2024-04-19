using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class NiveauEtudeDao : Dao<NiveauEtude>
    {
        public NiveauEtudeDao(DbConnection connection = null) : base(connection)
        {
            TableName = "niveau_etude";
        }

        public override int Add(NiveauEtude instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GetKey(TableName);

                Request.CommandText = "insert into niveau_etude(id, intitule, adding_date, last_update_time) " +
                    "values(@v_id, @v_intitule, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_intitule", DbType.String, instance.Intitule));

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

        public int Add(DbCommand command, NiveauEtude niveau_etude)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(niveau_etude);
        }

        public async Task<int> AddAsync(NiveauEtude instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GetKey(TableName);

                Request.CommandText = "insert into niveau_etude(id, intitule, adding_date, last_update_time) " +
                    "values(@v_id, @v_intitule, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_intitule", DbType.String, instance.Intitule));

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

        public async Task<int> AddAsync(DbCommand command, NiveauEtude instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public override int Update(NiveauEtude instance, NiveauEtude old = null)
        {
            try
            {

                Request.CommandText = "update niveau_etude " +
                    "set intitule = @v_intitule, " +
                    "last_update_time = now() " +
                    "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_intitule", DbType.String, instance.Intitule));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -6;
            }
        }

        public override int Delete(NiveauEtude instance)
        {
            try
            {

                Request.CommandText = "delete from niveau_etude " +
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

        private NiveauEtude Create(Dictionary<string, object> row)
        {
            var instance = new NiveauEtude();

            instance.Id = row["id"].ToString();
            instance.Intitule = row["intitule"].ToString();
            instance.Niveau = int.Parse(row["niveau"].ToString());
            instance.ADomaine = bool.Parse(row["a_domaine"].ToString());
            instance.GradeRecrutement = new GradeDao().Get(row["grade_recrutement_id"].ToString());
            return instance;
        }

        public int Count()
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from niveau_etude";

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public NiveauEtude Get(string id)
        {
            NiveauEtude instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from niveau_etude " +
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

        public async Task<List<NiveauEtude>> GetAllAsync(DateTime lastUpdateTime)
        {
            var intances = new List<NiveauEtude>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from niveau_etude " +
                    "where adding_date >= @v_time or last_update_time >= @v_time";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_time", DbType.DateTime, lastUpdateTime));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var niveau = Create(item);
                    intances.Add(niveau);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<NiveauEtude>> GetAllAsync()
        {
            var intances = new List<NiveauEtude>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from niveau_etude " +
                    "order by niveau";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    NiveauEtude niveau_etude = Create(item);
                    intances.Add(niveau_etude);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task GetAllAsync(ObservableCollection<NiveauEtude> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from niveau_etude " +
                    "order by niveau";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    NiveauEtude niveau_etude = Create(item);
                    collection.Add(niveau_etude);
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
                { "intitule", reader["intitule"] },
                { "niveau", reader["niveau"] },
                { "a_domaine", reader["a_domaine"] },
                { "grade_recrutement_id", reader["grade_recrutement_id"] }
            };
        }
    }
}


