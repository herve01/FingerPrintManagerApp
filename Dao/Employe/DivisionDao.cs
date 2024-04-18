using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class DivisionDao : Dao<Division>
    {
        public DivisionDao()
        {
            TableName = "division";
        }

        public override int Add(Division instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GetKey(TableName);

                Request.CommandText = "insert into division(id, direction_id, entite_id, denomination, mission, adding_date, last_update_time) " +
                    "values(@v_id, @v_direction_id, @v_entite_id, @v_denomination, @v_mission, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, instance.Entite.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_denomination", DbType.String, instance.Denomination));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mission", DbType.String, instance.Mission));

                var feed = Request.ExecuteNonQuery();

                if (feed <= 0)
                    return -1;

                instance.Id = id;

                var dao = new BureauDao();

                foreach (var bureau in instance.Bureaux)
                    if (dao.Add(Request, bureau) <= 0)
                        return -2;

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int Add(DbCommand command, Division division)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(division);
        }

        public async Task<int> AddAsync(Division instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GetKey(TableName);

                Request.CommandText = "insert into division(id, direction_id, entite_id, denomination, mission, adding_date, last_update_time) " +
                    "values(@v_id, @v_direction_id, @v_entite_id, @v_denomination, @v_mission, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, instance.Entite.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_denomination", DbType.String, instance.Denomination));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mission", DbType.String, instance.Mission));

                var feed = await Request.ExecuteNonQueryAsync();

                if (feed <= 0)
                    return -1;

                instance.Id = id;

                var dao = new BureauDao();

                foreach (var bureau in instance.Bureaux)
                    if (await dao.AddAsync(Request, bureau) <= 0)
                        return -2;

                return feed;
            }
            catch (Exception)
            {
                return -3;
            }
        }

        public async Task<int> AddAsync(DbCommand command, Division instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public override int Update(Division instance, Division old = null)
        {
            try
            {

                Request.CommandText = "update division " +
                    "set denomination = @v_denomination, " +
                    "direction_id = @v_direction_id, " +
                    "mission = @v_mission, " +
                    "last_update_time = now() " +
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

        public override int Delete(Division instance)
        {
            try
            {

                Request.CommandText = "delete from division " +
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

        private Division Create(Dictionary<string, object> row, bool withDirection, bool withBureaux, bool withFonctions)
        {
            var instance = new Division();

            instance.Id = row["id"].ToString();
            instance.Denomination = row["denomination"].ToString();

            if (!(row["mission"] is DBNull))
                instance.Mission = row["mission"].ToString();

            //if (withDirection && !(row["direction_id"] is DBNull))
            //    instance.Direction = new DirectionDao().Get(row["direction_id"].ToString());

            if (withBureaux)
                instance.Bureaux = new BureauDao().GetAll(instance, withFonctions);

            if (withFonctions)
                instance.Fonctions = new FonctionDao().GetAll(instance);

            return instance;
        }

        public int Count()
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from division";

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public int Count(Entite entite)
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from division " +
                    "where entite_id = @v_entite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public Division Get(string id)
        {
            Division instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from division " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _instances = Map(Reader);

                Reader.Close();

                if (_instances != null)
                    instance = Create(_instances, true, false, false);

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }

        public Division Get(Entite entite)
        {
            Division instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from division " +
                    "where entite_id = @v_entite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _instances = Map(Reader);

                Reader.Close();

                if (_instances != null)
                {
                    instance = Create(_instances, true, false, true);
                    instance.Entite = entite;
                }

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }

        public List<Division> GetAll(bool withFonctions)
        {
            var intances = new List<Division>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from division " +
                    "where direction_id = @v_direction_id";

                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_direction_id", DbType.String, direction.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var division = Create(item, false, true, withFonctions);
                    //division.Direction = direction;
                    intances.Add(division);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<Division>> GetAllAsync(Entite entite)
        {
            var intances = new List<Division>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from division " +
                    "where entite_id = @v_entite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Division division = Create(item, true, false, false);
                    division.Entite = entite;
                    intances.Add(division);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<Division>> GetAllAsync(DateTime lastUpdateTime)
        {
            var intances = new List<Division>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from division " +
                    "where adding_date >= @v_time or last_update_time >= @v_time";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_time", DbType.DateTime, lastUpdateTime));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Division division = Create(item, true, false, false);
                    intances.Add(division);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<Division>> GetAllAsync(bool withFonctions = false)
        {
            var intances = new List<Division>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from division " +
                    "direction_id = @v_direction_id";

                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_direction_id", DbType.String, direction.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Division division = Create(item, false, true, withFonctions);
                    //division.Direction = direction;
                    intances.Add(division);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task GetAllAsync(Entite entite, ObservableCollection<Division> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from division " +
                    "where entite_id = @v_entite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Division division = Create(item, true, false, false);
                    division.Entite = entite;
                    collection.Add(division);
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


