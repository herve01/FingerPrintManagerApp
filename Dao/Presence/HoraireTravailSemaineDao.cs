using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Presence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;


namespace FingerPrintManagerApp.Dao.Presence
{
    public class HoraireTravailSemaineDao : Dao<HoraireTravailSemaine>
    {
        public HoraireTravailSemaineDao(DbConnection connection = null) : base(connection)
        {
            TableName = "horaire_travail_semaine";
        }
        public override int Add(HoraireTravailSemaine instance)
        {
            return 0;
        }

        public override int Update(HoraireTravailSemaine instance, HoraireTravailSemaine old = null)
        {
            return 0;
        }

        public override int Delete(HoraireTravailSemaine instance)
        {
            return 0;
        }

        private HoraireTravailSemaine Create(Dictionary<string, object> row)
        {
            var instance = new HoraireTravailSemaine();

            instance.Id = row["id"].ToString();
            instance.Designation = row["designation"].ToString();
            instance.EstOuvrable = bool.Parse(row["est_ouvrable"].ToString());
            instance.HeureDebut = DateTime.Parse(row["heure_debut"].ToString());
            instance.HeureFin = DateTime.Parse(row["heure_fin"].ToString());

            return instance;

        }

        public HoraireTravailSemaine Get(string id)
        {
            HoraireTravailSemaine instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from horaire_travail_semaine " +
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

        public async Task<List<HoraireTravailSemaine>> GetAllAsync()
        {
            var intances = new List<HoraireTravailSemaine>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from horaire_travail_semaine ";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var instance = Create(item);
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

        public async Task<List<HoraireTravailSemaine>> GetAllAsync(DateTime lastUpdateTime)
        {
            var intances = new List<HoraireTravailSemaine>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from horaire_travail_semaine " +
                    "where created_at >= @v_time or updated_at >= @v_time";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_time", DbType.DateTime, lastUpdateTime));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var horaire_travail_semaine = Create(item);
                    intances.Add(horaire_travail_semaine);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task GetAllAsync(ObservableCollection<HoraireTravailSemaine> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from horaire_travail_semaine";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    HoraireTravailSemaine horaire_travail_semaine = Create(item);
                    collection.Add(horaire_travail_semaine);
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
                { "designation", reader["designation"] },
                { "est_ouvrable", reader["est_ouvrable"] },
                { "heure_debut", reader["heure_debut"] },
                { "heure_fin", reader["heure_fin"] },
            };
        }
    }
}
