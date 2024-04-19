using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class DirectionDao : Dao<Direction>
    {
        public DirectionDao(DbConnection connection = null) : base(connection)
        {
            TableName = "direction";
        }

        public override int Add(Direction instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into direction(id, denomination, sigle, mission, created_at, updated_at) " +
                    "values(@v_id, @v_denomination, @v_sigle, @v_mission, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_denomination", DbType.String, instance.Denomination));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_sigle", DbType.String, instance.Sigle));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mission", DbType.String, instance.Mission));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int Add(DbCommand command, Direction direction)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(direction);
        }

        public async Task<int> AddAsync(Direction instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into direction(id, denomination, sigle, mission, created_at, updated_at) " +
                    "values(@v_id, @v_denomination, @v_sigle, @v_mission, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_denomination", DbType.String, instance.Denomination));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_sigle", DbType.String, instance.Sigle));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mission", DbType.String, instance.Mission));

                var feed = await Request.ExecuteNonQueryAsync();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<int> AddAsync(DbCommand command, Direction instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public override int Update(Direction instance, Direction old = null)
        {
            try
            {

                Request.CommandText = "update direction " +
                    "set denomination = @v_denomination, " +
                    "sigle = @v_sigle, " +
                    "mission = @v_mission, " +
                    "updated_at = now() " +
                    "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_denomination", DbType.String, instance.Denomination));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_sigle", DbType.String, instance.Sigle));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mission", DbType.String, instance.Mission));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -6;
            }
        }

        public override int Delete(Direction instance)
        {
            try
            {

                Request.CommandText = "delete from direction " +
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

        private Direction Create(Dictionary<string, object> row, bool withDivisions, bool withFonctions)
        {
            var instance = new Direction();

            instance.Id = row["id"].ToString();
            instance.Denomination = row["denomination"].ToString();
            instance.Sigle = row["sigle"].ToString();
            instance.EstGenerale = bool.Parse(row["est_generale"].ToString());
           
            if (!(row["mission"] is DBNull))
                instance.Mission = row["mission"].ToString();

            return instance;

        }

        public int Count()
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from direction";

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public Direction Get(string id)
        {
            Direction instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from direction " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _instances = Map(Reader);

                Reader.Close();

                if (_instances != null)
                    instance = Create(_instances, false, false);

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }

        public async Task<List<Direction>> GetAllAsync(bool withFonctions = false)
        {
            var intances = new List<Direction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from direction " +
                    "order by est_generale desc, denomination asc";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var instance = Create(item, true, withFonctions);
                    intances.Add(instance);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<Direction>> GetAllAsync(DateTime lastUpdateTime)
        {
            var intances = new List<Direction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from direction " +
                    "where created_at >= @v_time or updated_at >= @v_time";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_time", DbType.DateTime, lastUpdateTime));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var direction = Create(item, true, false);
                    intances.Add(direction);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task GetAllAsync(ObservableCollection<Direction> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from direction";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Direction direction = Create(item, false, false);
                    collection.Add(direction);
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
                { "denomination", reader["denomination"] },
                { "sigle", reader["sigle"] },
                { "mission", reader["mission"] },
                { "est_generale", reader["est_generale"] }
            };
        }
    }
}
