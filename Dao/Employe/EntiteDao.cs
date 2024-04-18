using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class EntiteDao : Dao<Entite>
    {
        public EntiteDao()
        {
            TableName = "entite";
        }

        public override int Add(Entite instance)
        {
            try
            {
                Request.Transaction = Connection.BeginTransaction();

                var id = Helper.TableKeyHelper.GetKey(TableName, false);

                Request.CommandText = "insert into entite(id, direction_id, zone_id, numero, avenue, commune_id, est_principale, type, adding_date, last_update_time) " +
                    "values(@v_id, @v_direction_id, @v_zone_id, @v_numero, @v_avenue, @v_commune_id, @v_est_principale, @v_type, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_direction_id", DbType.String, instance.Direction.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_zone_id", DbType.String, instance.Zone.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_numero", DbType.String, instance.Address.Number));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_avenue", DbType.String, instance.Address.Street));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_commune_id", DbType.Int32, instance.Address.Commune.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_principale", DbType.Boolean, instance.EstPrincipale));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_type", DbType.String, instance.Type));

                var feed = Request.ExecuteNonQuery();

                if (feed <= 0)
                {
                    Request.Transaction.Rollback();
                    return -1;
                }

                instance.Id = id;

                feed = new DivisionDao().Add(Request, instance.Division);

                if (feed <= 0)
                {
                    instance.Id = null;
                    Request.Transaction.Rollback();
                    return -2;
                }

                Request.Transaction.Commit();

                return feed;
            }
            catch (Exception)
            {
                Request.Transaction.Rollback();
                return -3;
            }
        }

        public int Add(DbCommand command, Entite entite)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(entite);
        }

        public async Task<int> AddAsync(Entite instance)
        {
            try
            {
                Request.Transaction = Connection.BeginTransaction();

                var id = Helper.TableKeyHelper.GetKey(TableName, false);

                Request.CommandText = "insert into entite(id, direction_id, zone_id, numero, avenue, commune_id, est_principale, type, adding_date, last_update_time) " +
                    "values(@v_id, @v_direction_id, @v_zone_id, @v_numero, @v_avenue, @v_commune_id, @v_est_principale, @v_type, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_direction_id", DbType.String, instance.Direction.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_zone_id", DbType.String, instance.Zone.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_numero", DbType.String, instance.Address.Number));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_avenue", DbType.String, instance.Address.Street));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_commune_id", DbType.Int32, instance.Address.Commune.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_principale", DbType.Boolean, instance.EstPrincipale));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_type", DbType.String, instance.Type));

                var feed = await Request.ExecuteNonQueryAsync();

                if (feed <= 0)
                {
                    Request.Transaction.Rollback();
                    return -1;
                }

                instance.Id = id;

                feed = await new DivisionDao().AddAsync(Request, instance.Division);

                if (feed <= 0)
                {
                    instance.Id = null;
                    Request.Transaction.Rollback();
                    return -2;
                }

                Request.Transaction.Commit();

                return feed;
            }
            catch (Exception)
            {
                Request.Transaction.Rollback();
                return -3;
            }
        }

        public async Task<int> AddAsync(DbCommand command, Entite instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public override int Update(Entite instance, Entite old = null)
        {
            try
            {
                Request.CommandText = "update entite " +
                    "set direction_id = @v_direction_id, " +
                    "zone_id = @v_zone_id, " +
                    "numero = @v_numero, " +
                    "avenue = @v_avenue, " +
                    "commune_id = @v_commune_id, " +
                    "type = @v_type, " +
                    "est_principale = @v_est_principale, " +
                    "last_update_time = now() " +
                    "where id = @v_id ;";

                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_direction_id", DbType.String, instance.Direction?.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_zone_id", DbType.String, instance.Zone.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_numero", DbType.String, instance.Address.Number));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_avenue", DbType.String, instance.Address.Description));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_commune_id", DbType.Int32, instance.Address.Commune.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_type", DbType.String, instance.Type));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_principale", DbType.Boolean, instance.EstPrincipale));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public override int Delete(Entite instance)
        {
            try
            {
                Request.CommandText = "delete from entite " +
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

        private Entite Create(Dictionary<string, object> row, bool withDirection)
        {
            Entite instance = new Entite();

            instance.Id = row["id"].ToString();
            instance.Zone = new ZoneDao().Get(int.Parse(row["zone_id"].ToString()));
            instance.Address.Commune = new CommuneDao().GetCommune(Convert.ToInt32(row["commune_id"].ToString()));
            instance.Address.Number = row["numero"].ToString();
            instance.Address.Street = row["avenue"].ToString();
            instance.Type = Util.ToEntiteType(row["type"].ToString());
            instance.EstPrincipale = Convert.ToBoolean(row["est_principale"].ToString());
            instance.Division = new DivisionDao().Get(instance);

            //if (withDirection)
            //    instance.Direction = new DirectionProvincialeDao().Get(row["direction_id"].ToString());

            return instance;

        }

        public int Count()
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from entite";

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public Entite Get(string id)
        {
            Entite instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from entite " +
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



        public List<Entite> GetAll()
        {
            var intances = new List<Entite>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from entite ";

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Entite entite = Create(item, true);
                    intances.Add(entite);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<Entite>> GetAllAsync()
        {
            var intances = new List<Entite>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from entite " +
                    "order by est_principale desc";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Entite entite = Create(item, true);
                    intances.Add(entite);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<Entite>> GetAllAsync(DateTime lastUpdateTime)
        {
            var intances = new List<Entite>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from entite " +
                    "where adding_date >= @v_time or last_update_time >= @v_time";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_time", DbType.DateTime, lastUpdateTime));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var direction_interne = Create(item, false);
                    intances.Add(direction_interne);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        
        public async Task GetAllAsync(ObservableCollection<Entite> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from entite";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Entite entite = Create(item, true);
                    collection.Add(entite);
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
                { "zone_id", reader["zone_id"] },
                { "numero", reader["numero"] },
                { "avenue", reader["avenue"] },
                { "type", reader["type"] },
                { "commune_id", reader["commune_id"] },
                { "est_principale", reader["est_principale"] }
            };
        }

    }
}


