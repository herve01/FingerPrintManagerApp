using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Employe;
using FingerPrintManagerApp.Model.Presence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;


namespace FingerPrintManagerApp.Dao.Presence
{
    public class PresenceDao : Dao<Model.Presence.Presence>
    {
        public PresenceDao(DbConnection connection = null) : base(connection)
        {
            TableName = "presence";
        }
        public override int Add(Model.Presence.Presence instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into presence(id, periode_id, employe_id, date, mode_pointage, heure_arrivee, created_at, updated_at) " +
                    "values(@v_id, @v_periode_id, @v_employe_id, @v_date, @v_mode_pointage, @v_heure_arrivee, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_periode_id", DbType.String, instance.Periode.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.Date, instance.Date));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mode_pointage", DbType.String, instance.Mode));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_heure_arrivee", DbType.Time, instance.HeureArrivee.TimeOfDay));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int Add(DbCommand command, Model.Presence.Presence presence)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(presence);
        }

        public async Task<int> AddAsync(Model.Presence.Presence instance)
        {
            try
            {

                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into presence(id, periode_id, employe_id, date, mode_pointage, heure_arrivee, created_at, updated_at) " +
                     "values(@v_id, @v_periode_id, @v_employe_id, @v_date,  @v_mode_pointage, @v_heure_arrivee, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_periode_id", DbType.String, instance.Periode.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.Date, instance.Date));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mode_pointage", DbType.String, instance.Mode));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_heure_arrivee", DbType.Time, instance.HeureArrivee.TimeOfDay));

                var feed = await Request.ExecuteNonQueryAsync();

                return feed;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public async Task<int> AddAsync(DbCommand command, Model.Presence.Presence instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public override int Update(Model.Presence.Presence instance, Model.Presence.Presence old = null)
        {
            try
            {

                Request.CommandText = "update presence " +
                    "set periode_id = @v_periode_id, " +
                    "employe_id = @v_employe_id, " +
                    "date = @v_date, " +
                    "mode_pointage = @v_mode_pointage, " +
                    "heure_arrivee = @v_heure_arrivee, " +
                    "heure_depart = @v_heure_depart, " +
                    "updated_at = now() " +
                    "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_periode_id", DbType.String, instance.Periode.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.Date, instance.Date));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mode_pointage", DbType.String, instance.Mode));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_heure_arrivee", DbType.Time, instance.HeureArrivee.TimeOfDay));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_heure_depart", DbType.Time, instance.HeureDepart.TimeOfDay));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public override int Delete(Model.Presence.Presence instance)
        {
            try
            {

                Request.CommandText = "delete from presence " +
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

        public async Task<int> MarkDeparture(Model.Presence.Presence instance)
        {
            try
            {

                Request.CommandText = "update presence " +
                    "set heure_depart = @v_heure_depart, " +
                    "updated_at = now() " +
                    "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_heure_arrivee", DbType.Time, instance.HeureArrivee.TimeOfDay));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_heure_depart", DbType.Time, instance.HeureDepart.TimeOfDay));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = await Request.ExecuteNonQueryAsync();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private Model.Presence.Presence Create(Dictionary<string, object> row, bool withEmploye)
        {
            var instance = new Model.Presence.Presence();

            instance.Id = row["id"].ToString();
            instance.Date = DateTime.Parse(row["date"].ToString());
            instance.Mode = Util.ToModePointage(row["mode_pointage"].ToString());
            instance.HeureArrivee = DateTime.Parse(row["heure_arrivee"].ToString());
            instance.Periode = new PeriodeDao().Get(row["periode_id"].ToString());

            if (!(row["heure_depart"] is DBNull))
                instance.HeureDepart = DateTime.Parse(row["heure_depart"].ToString());

            if (withEmploye)
                instance.Employe = new Employe.EmployeDao().Get(row["employe_id"].ToString());

            return instance;
        }

        public int Count(DateTime date)
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from presence where date = @v_date";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.Date, date));

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public Model.Presence.Presence Get(string id)
        {
            Model.Presence.Presence instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from presence " +
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

        public bool CheckDepaturePointage(Model.Presence.Presence presence)
        {
            try
            {
                Request.CommandText = "select heure_depart " +
                    "from presence " +
                    "where id = @v_presence_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_presence_id", DbType.String, presence.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                {
                    var feed = Reader["heure_depart"];
                    Reader.Close();

                    return !(feed is DBNull);

                }

                Reader.Close();

                return false;

            }
            catch (Exception)
            {
                if (Reader != null)
                    Reader.Close();
            }

            return false;
        }

        public async Task<Model.Presence.Presence> GetAsync(Model.Employe.Employe employe, DateTime date, bool withDepature = false)
        {
            Model.Presence.Presence instance = null;
            Dictionary<string, object> _instance = null;

            try
            {
                Request.CommandText = "select * " +
                    "from presence " +
                    "where employe_id = @v_employe_id and date = @v_date " +
                    "and (@v_depart = 0 or heure_depart is not null)";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.Date, date.Date));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_depart", DbType.Int32, withDepature ? 1 : 0));

                Reader = await Request.ExecuteReaderAsync();

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

        public async Task<List<Model.Presence.Presence>> GetAllAsync(bool withEmploye = false)
        {
            var intances = new List<Model.Presence.Presence>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from presence " +
                    "order by est_generale desc, periode_id asc";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var instance = Create(item, withEmploye);
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

        public async Task<List<Model.Presence.Presence>> GetAllAsync(DateTime lastUpdateTime)
        {
            var intances = new List<Model.Presence.Presence>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from presence " +
                    "where created_at >= @v_time or updated_at >= @v_time";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_time", DbType.DateTime, lastUpdateTime));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var presence = Create(item, false);
                    intances.Add(presence);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task GetAllAsync(ObservableCollection<Model.Presence.Presence> collection, DateTime date)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from presence where date = @v_date";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.Date, date));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Model.Presence.Presence presence = Create(item, true);
                    collection.Add(presence);
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
                { "periode_id", reader["periode_id"] },
                { "employe_id", reader["employe_id"] },
                { "date", reader["date"] },
                { "mode_pointage", reader["mode_pointage"] },
                { "heure_arrivee", reader["heure_arrivee"] },
                { "heure_depart", reader["heure_depart"] }
            };
        }

        public async Task<DataTable> GetRegistrePresenceJournaliereReportAsync(Entite entite, DateTime date)
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                Request.CommandText = "sp_registre_presence_journaliere";
                Request.CommandType = CommandType.StoredProcedure;

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.DateTime, date.Date));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        list.Add(new Dictionary<string, object>()
                        {
                            { "entite_id", AppConfig.CurrentUser.Entite.Id },
                            { "nom", Reader["nom"] },
                            { "post_nom", Reader["post_nom"] },
                            { "prenom", Reader["prenom"] },
                            { "sexe", Reader["sexe"] },
                            { "matricule", Reader["matricule"] },
                            { "heure_arrivee", Reader["heure_arrivee"] },
                            { "heure_depart", Reader["heure_depart"] },
                            { "grade", Reader["grade"] },
                            { "direction", Reader["direction"] },
                            { "entite_name", Reader["entite_name"] },
                            { "est_present", Reader["est_present"] }                    
                        });

                Reader.Close();

                var types = new List<Type>()
                    {
                        typeof(string),
                        typeof(string),
                        typeof(string),
                        typeof(string),
                        typeof(string),
                        typeof(string),
                        typeof(DateTime),
                        typeof(DateTime),
                        typeof(string),
                        typeof(string),
                        typeof(string),
                        typeof(int)
                    };

                var table = DbUtil.DicToTable(list, types);
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

        public async Task<DataTable> GetEmployePresenceReportAsync(Entite entite, int mois, int annee )
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                Request.CommandText = "sp_registre_presence_mensuelle";
                Request.CommandType = CommandType.StoredProcedure;
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mois", DbType.Int32, mois));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_annee", DbType.Int32, annee));

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
                            { "grade", Reader["grade"] },
                            { "direction", Reader["direction"] },
                            { "count_presence", Reader["count_presence"] },
                            { "entite_name", Reader["entite_name"] }
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
    }
}
