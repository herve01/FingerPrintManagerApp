using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class DepartementDao : Dao<Departement>
    {
        public DepartementDao(DbConnection connection = null) : base(connection)
        {
            TableName = "departement";
        }

        public override int Add(Departement instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into departement(id, direction_id, denomination, mission, created_at, updated_at) " +
                    "values(@v_id, @v_direction_id, @v_denomination, @v_mission, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));          
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_direction_id", DbType.String, instance.Direction.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_denomination", DbType.String, instance.Denomination));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mission", DbType.String, instance.Mission));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int Add(DbCommand command, Departement departement)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(departement);
        }

        public async Task<int> AddAsync(Departement instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into departement(id, direction_id, denomination, mission, created_at, updated_at) " +
                     "values(@v_id, @v_direction_id, @v_denomination, @v_mission, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_direction_id", DbType.String, instance.Direction.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_denomination", DbType.String, instance.Denomination));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mission", DbType.String, instance.Mission));

                var feed = await Request.ExecuteNonQueryAsync();

                return feed;
            }
            catch (Exception)
            {
                return -3;
            }
        }

        public async Task<int> AddAsync(DbCommand command, Departement instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public override int Update(Departement instance, Departement old = null)
        {
            try
            {

                Request.CommandText = "update departement " +
                    "set denomination = @v_denomination, " +
                    "direction_id = @v_direction_id, " +
                    "mission = @v_mission, " +
                    "updated_at = now() " +
                    "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_denomination", DbType.String, instance.Denomination));
              
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

        public override int Delete(Departement instance)
        {
            try
            {

                Request.CommandText = "delete from departement " +
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

        private Departement Create(Dictionary<string, object> row, bool withDirection = false)
        {
            var instance = new Departement();

            instance.Id = row["id"].ToString();
            instance.Denomination = row["denomination"].ToString();

            if (!(row["mission"] is DBNull))
                instance.Mission = row["mission"].ToString();

            if (withDirection && !(row["direction_id"] is DBNull))
                instance.Direction = new DirectionDao().Get(row["direction_id"].ToString());

             return instance;
        }

        public int Count()
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from departement";

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public int Count(Direction direction)
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from departement " +
                    "where direction_id = @v_direction_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_direction_id", DbType.String, direction.Id));

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public Departement Get(string id)
        {
            Departement instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from departement " +
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

        public Departement Get(Direction direction)
        {
            Departement instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from departement " +
                    "where direction_id = @v_direction_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_direction_id", DbType.String, direction.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _instances = Map(Reader);

                Reader.Close();

                if (_instances != null)
                {
                    instance = Create(_instances);
                    instance.Direction = direction;
                }

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }

        public List<Departement> GetAll(Direction direction)
        {
            var intances = new List<Departement>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from departement " +
                    "where direction_id = @v_direction_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_direction_id", DbType.String, direction.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var departement = Create(item);
                    departement.Direction = direction;
                    intances.Add(departement);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<Departement>> GetAllAsync(Direction direction)
        {
            var intances = new List<Departement>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from departement " +
                    "where direction_id = @v_direction_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_direction_id", DbType.String, direction.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Departement departement = Create(item);
                    departement.Direction = direction;
                    intances.Add(departement);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<Departement>> GetAllAsync(DateTime lastUpdateTime)
        {
            var intances = new List<Departement>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from departement " +
                    "where created_at >= @v_time or updated_at >= @v_time";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_time", DbType.DateTime, lastUpdateTime));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Departement departement = Create(item, true);
                    intances.Add(departement);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task GetAllAsync(Direction direction, ObservableCollection<Departement> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from departement " +
                    "where direction_id = @v_direction_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_direction_id", DbType.String, direction.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Departement departement = Create(item);
                    departement.Direction = direction;
                    collection.Add(departement);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

        }


        public async Task GetAllAsync(ObservableCollection<Departement> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from departement ";

                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_direction_id", DbType.String, direction.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Departement departement = Create(item, true);
                    //departement.Direction = direction;
                    collection.Add(departement);
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
                { "direction_id", reader["direction_id"] },
                { "denomination", reader["denomination"] },
                { "mission", reader["mission"] }
            };
        }

    }
}


