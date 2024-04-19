using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class DirectionProvincialeDao : Dao<DirectionProvinciale>
    {
        public DirectionProvincialeDao(DbConnection connection = null) : base(connection)
        {
            TableName = "direction_provinciale";
        }

        public override int Add(DirectionProvinciale instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into direction_provinciale(id, est_generale, province_id, created_at, updated_at) " +
                    "values(@v_id, @v_est_generale, @v_province_id, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_generale", DbType.String, instance.EstGenerale));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_province_id", DbType.String, instance.Province));

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

        public int Add(DbCommand command, DirectionProvinciale direction_provinciale)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(direction_provinciale);
        }

        public async Task<int> AddAsync(DirectionProvinciale instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into direction_provinciale(id, est_generale, province_id, created_at, updated_at) " +
                    "values(@v_id, @v_est_generale, @v_province_id, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_generale", DbType.String, instance.EstGenerale));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_province_id", DbType.String, instance.Province));

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

        public async Task<int> AddAsync(DbCommand command, DirectionProvinciale instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public override int Update(DirectionProvinciale instance, DirectionProvinciale old)
        {
            try
            {

                Request.CommandText = "update direction_provinciale " +
                    "set province_id = @v_province_id, " +
                    "est_generale = @v_est_generale, " +
                    "updated_at = now() " +
                    "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_generale", DbType.String, instance.EstGenerale));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_province_id", DbType.String, instance.Province));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -6;
            }
        }

        public override int Delete(DirectionProvinciale instance)
        {
            try
            {

                Request.CommandText = "delete from direction_provinciale " +
                    "where id=@v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private DirectionProvinciale Create(Dictionary<string, object> row, bool withEntites)
        {
            var instance = new DirectionProvinciale();

            instance.Id = row["id"].ToString();
            instance.EstGenerale = Convert.ToBoolean(row["est_generale"].ToString());
            instance.Province = new ProvinceDao().Get(Convert.ToInt32(row["province_id"].ToString()));

            if (withEntites)
                instance.Entites = new EntiteDao().GetAll(instance);

            return instance;

        }

        public int Count()
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from direction_provinciale";

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public DirectionProvinciale Get(string id)
        {
            DirectionProvinciale instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from direction_provinciale " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _instances = Map(Reader);

                Reader.Close();

                if (_instances != null)
                    instance = Create(_instances, false);

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }


        public DirectionProvinciale Get(Entite entite)
        {
            DirectionProvinciale instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from direction_provinciale " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, entite.Direction.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _instances = Map(Reader);

                Reader.Close();

                if (_instances != null)
                    instance = Create(_instances, false);

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }
        public async Task<List<DirectionProvinciale>> GetAllAsync()
        {
            var intances = new List<DirectionProvinciale>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from direction_provinciale ";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    DirectionProvinciale direction_provinciale = Create(item, true);
                    intances.Add(direction_provinciale);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task GetAllAsync(ObservableCollection<DirectionProvinciale> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from direction_provinciale";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    DirectionProvinciale direction_provinciale = Create(item, false);
                    collection.Add(direction_provinciale);
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
                { "est_generale", reader["est_generale"] },
                { "province_id", reader["province_id"] }
            };
        }

    }
}


