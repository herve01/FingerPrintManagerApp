using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class BureauDao : Dao<Bureau>
    {
        public BureauDao()
        {
            TableName = "bureau";
        }

        public override int Add(Bureau instance)
        {
            try
            {
                object directionId = null;
                object divisionId = null;

                if (instance.Division != null)
                    divisionId = instance.Division.Id;
                //else
                //    directionId = instance.Direction.Id;


                var id = Helper.TableKeyHelper.GetKey(TableName);

                Request.CommandText = "insert into bureau(id, division_id, direction_id, denomination, nombre_chefs, mission, adding_date, last_update_time) " +
                    "values(@v_id, @v_division_id, @v_direction_id, @v_denomination, @v_nombre_chefs, @v_mission, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_division_id", DbType.String, divisionId));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_direction_id", DbType.String, directionId));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_denomination", DbType.String, instance.Denomination));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_nombre_chefs", DbType.Int32, instance.NombreChefs));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mission", DbType.String, instance.Mission));

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

        public int Add(DbCommand command, Bureau bureau)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(bureau);
        }

        public async Task<int> AddAsync(Bureau instance)
        {
            try
            {
                object directionId = null;
                object divisionId = null;

                if (instance.Division != null)
                    divisionId = instance.Division.Id;
                //else
                //    directionId = instance.Direction.Id;


                var id = Helper.TableKeyHelper.GetKey(TableName);

                Request.CommandText = "insert into bureau(id, division_id, direction_id, denomination, nombre_chefs, mission, adding_date, last_update_time) " +
                    "values(@v_id, @v_division_id, @v_direction_id, @v_denomination, @v_nombre_chefs, @v_mission, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_division_id", DbType.String, divisionId));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_direction_id", DbType.String, directionId));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_denomination", DbType.String, instance.Denomination));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_nombre_chefs", DbType.Int32, instance.NombreChefs));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mission", DbType.String, instance.Mission));

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

        public async Task<int> AddAsync(DbCommand command, Bureau instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public async Task<int> AddAsync(List<Bureau> bureaux)
        {
            Request.Transaction = Connection.BeginTransaction();

            foreach (var bureau in bureaux)
            {
                Request.Parameters.Clear();

                if (await AddAsync(bureau) <= 0)
                {
                    Request.Transaction.Rollback();
                    return -1;
                }
            }

            Request.Transaction.Commit();

            return bureaux.Count;
        }

        public override int Update(Bureau instance, Bureau old = null)
        {
            try
            {

                Request.CommandText = "update bureau " +
                    "set denomination = @v_denomination, " +
                    "division_id = @v_division_id, " +
                    "mission = @v_mission, " +
                    "nombre_chefs = @v_nombre_chefs, " +
                    "last_update_time = now() " +
                    "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_denomination", DbType.String, instance.Denomination));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_division_id", DbType.String, instance.Division.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mission", DbType.String, instance.Mission));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_nombre_chefs", DbType.Int32, instance.NombreChefs));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -6;
            }
        }

        public override int Delete(Bureau instance)
        {
            try
            {

                Request.CommandText = "delete from bureau " +
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

        private Bureau Create(Dictionary<string, object> row, bool withParent, bool withFonctions)
        {
            Bureau instance = new Bureau();

            instance.Id = row["id"].ToString();
            instance.Denomination = row["denomination"].ToString();
            instance.NombreChefs = int.Parse(row["nombre_chefs"].ToString());
            instance.Division = new DivisionDao().Get(row["division_id"].ToString());

            if (!(row["mission"] is DBNull))
                instance.Mission = row["mission"].ToString();

            if (withParent)
            {
                if (!(row["division_id"] is DBNull))
                {
                    instance.Division = new DivisionDao().Get(row["division_id"].ToString());
                }
                //else if (!(row["direction_id"] is DBNull))
                //    instance.Direction = new DirectionDao().Get(row["direction_id"].ToString());
            }

            if (withFonctions)
                instance.Fonctions = new FonctionDao().GetAll(instance);

            return instance;
        }

        public int Count()
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from bureau";

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
                    "from " +
                    "(" +
                    "(select B.* " +
                    " from bureau B " +
                    " inner join division D " +
                    " on B.division_id = D.id " +
                    " where D.entite_id = @v_entite_id " +
                    ")" +
                    "UNION " +
                    "(select B.* " +
                    " from bureau B " +
                    " where @v_entite_id = '000001' and B.division_id is null " +
                    ")" +
                    ")" +
                    "as U";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public Bureau Get(string id)
        {
            Bureau instance = null;
            Dictionary<string, object> _instance = null;

            try
            {
                Request.CommandText = "select * " +
                    "from bureau " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _instance = Map(Reader);

                Reader.Close();

                if (_instance != null)
                    instance = Create(_instance, true, false);

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }

        public Bureau Get( bool withFonctions = false)
        {
            Bureau instance = null;
            Dictionary<string, object> _instance = null;

            try
            {
                Request.CommandText = "select * " +
                    "from bureau " +
                    "where direction_id = @v_direction_id";

                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_direction_id", DbType.String, direction.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _instance = Map(Reader);

                Reader.Close();

                if (_instance != null)
                {
                    instance = Create(_instance, false, withFonctions);
                    //instance.Direction = direction;
                }

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }

        public async Task<List<Bureau>> GetAllAsync()
        {
            var intances = new List<Bureau>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from bureau ";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Bureau bureau = Create(item, true, false);
                    intances.Add(bureau);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<Bureau>> GetAllAsync(Entite entite, DateTime lastUpdateTime)
        {
            var intances = new List<Bureau>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select B.* " +
                    " from bureau B " +
                    " inner join division D " +
                    " on B.division_id = D.id " +
                    " where D.entite_id = @v_entite_id and B.adding_date >= @v_time or B.last_update_time >= @v_time";


                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_time", DbType.DateTime, lastUpdateTime));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var bureau = Create(item, true, false);
                    intances.Add(bureau);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task GetAllAsync(Entite entite, ObservableCollection<Bureau> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select U.* " +
                    "from " +
                    "(" +
                    "(select B.* " +
                    " from bureau B " +
                    " inner join division D " +
                    " on B.division_id = D.id " +
                    " where D.entite_id = @v_entite_id " +
                    ")" +
                    "UNION " +
                    "(select B.* " +
                    " from bureau B " +
                    " where @v_entite_id = '000001' and B.division_id is null " +
                    ")" +
                    ")" +
                    "as U";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Bureau bureau = Create(item, true, false);

                    if (bureau.Division != null)
                        bureau.Division.Entite = entite;

                    collection.Add(bureau);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

        }

        public List<Bureau> GetAll(Departement division, bool withFonctions = false)
        {
            var intances = new List<Bureau>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from bureau " +
                    "where division_id = @v_division_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_division_id", DbType.String, division.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Bureau bureau = Create(item, false, withFonctions);
                    bureau.Division = division;
                    intances.Add(bureau);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<Bureau>> GetAllAsync(Departement division, bool withFonctions = false)
        {
            var intances = new List<Bureau>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from bureau " +
                    "where division_id = @v_division_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_division_id", DbType.String, division.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Bureau bureau = Create(item, false, withFonctions);
                    bureau.Division = division;
                    intances.Add(bureau);
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
                { "division_id", reader["division_id"] },
                { "direction_id", reader["direction_id"] },
                { "denomination", reader["denomination"] },
                { "nombre_chefs", reader["nombre_chefs"] },
                { "mission", reader["mission"] }
            };
        }
    }
}


