using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class DomaineEtudeDao : Dao<DomaineEtude>
    {
        public DomaineEtudeDao()
        {
            TableName = "domaine_etude";
        }

        public override int Add(DomaineEtude instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GetKey(TableName);

                Request.CommandText = "insert into domaine_etude(id, intitule, adding_date, last_update_time) " +
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

        public int Add(DbCommand command, DomaineEtude domaine_etude)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(domaine_etude);
        }

        public async Task<int> AddAsync(DomaineEtude instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GetKey(TableName);

                Request.CommandText = "insert into domaine_etude(id, intitule, adding_date, last_update_time) " +
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

        public async Task<int> AddAsync(DbCommand command, DomaineEtude instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public override int Update(DomaineEtude instance, DomaineEtude old = null)
        {
            try
            {

                Request.CommandText = "update domaine_etude " +
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

        public override int Delete(DomaineEtude instance)
        {
            try
            {

                Request.CommandText = "delete from domaine_etude " +
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

        private DomaineEtude Create(Dictionary<string, object> row)
        {
            DomaineEtude instance = new DomaineEtude();

            instance.Id = row["id"].ToString();
            instance.Intitule = row["intitule"].ToString();

            return instance;
        }

        public int Count()
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from domaine_etude";

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public DomaineEtude Get(string id)
        {
            DomaineEtude instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from domaine_etude " +
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

        public DomaineEtude GetCurrent()
        {
            DomaineEtude instance = null;
            Dictionary<string, object> _instance = null;

            try
            {
                Request.CommandText = "select * " +
                    "from domaine_etude limit 1";


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

        public async Task<List<DomaineEtude>> GetAllAsync()
        {
            var intances = new List<DomaineEtude>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from domaine_etude ";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    DomaineEtude domaine_etude = Create(item);
                    intances.Add(domaine_etude);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<DomaineEtude>> GetAllAsync(DateTime lastUpdateTime)
        {
            var intances = new List<DomaineEtude>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from domaine_etude " +
                    "where adding_date >= @v_time or last_update_time >= @v_time";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_time", DbType.DateTime, lastUpdateTime));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    DomaineEtude domaine_etude = Create(item);
                    intances.Add(domaine_etude);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task GetAllAsync(ObservableCollection<DomaineEtude> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from domaine_etude";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    DomaineEtude domaine_etude = Create(item);
                    collection.Add(domaine_etude);
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
            };
        }
    }
}


