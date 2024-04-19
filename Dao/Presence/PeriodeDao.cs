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
    public class PeriodeDao : Dao<Periode>
    {
        public PeriodeDao(DbConnection connection = null) : base(connection)
        {
            TableName = "periode";
        }
        public override int Add(Periode instance)
        {
            try
            {
                var id = instance.Mois.ToString("D2") + instance.Annee;

                Request.CommandText = "insert into periode(id, mois, annee, created_at, updated_at) " +
                    "values(@v_id, @v_mois, @v_annee, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mois", DbType.Int32, instance.Mois));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_annee", DbType.Int32, instance.Annee));
             
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

        public int Add(DbCommand command, Periode periode)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(periode);
        }

        public async Task<int> AddAsync(Periode instance)
        {
            try
            {
                var id = instance.Mois.ToString("D2") + instance.Annee;

                Request.CommandText = "insert into periode(id, mois, annee, created_at, updated_at) " +
                    "values(@v_id, @v_mois, @v_annee, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mois", DbType.Int32, instance.Mois));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_annee", DbType.Int32, instance.Annee));

                var feed = await Request.ExecuteNonQueryAsync();

                if (feed > 0)
                    instance.Id = id;

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<int> AddAsync(DbCommand command, Periode instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public override int Update(Periode instance, Periode old = null)
        {
            try
            {

                Request.CommandText = "update periode " +
                    "set mois = @v_mois, " +
                    "annee = @v_annee, " +
                    "date = @v_date, " +
                    "updated_at = now() " +
                    "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mois", DbType.Int32, instance.Mois));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_annee", DbType.Int32, instance.Annee));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -6;
            }
        }

        public override int Delete(Periode instance)
        {
            try
            {

                Request.CommandText = "delete from periode " +
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

        private Periode Create(Dictionary<string, object> row)
        {
            var instance = new Periode();

            instance.Id = row["id"].ToString();
            instance.Mois = int.Parse( row["mois"].ToString());
            instance.Annee = int.Parse(row["annee"].ToString());
           
            return instance;

        }

        public int Count()
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from periode";

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public Periode Get(string id)
        {
            Periode instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from periode " +
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

        public Periode Get(DateTime date)
        {
            Periode instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from periode " +
                    "where annee = @v_annee and mois = @v_mois";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mois", DbType.Int32, date.Month));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_annee", DbType.Int32, date.Year));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _instances = Map(Reader);

                Reader.Close();

                if (_instances != null)
                    instance = Create(_instances);
                else
                {
                    instance = new Periode()
                    {
                        Mois = date.Month,
                        Annee = date.Year
                    };

                    Request.Parameters.Clear();

                    if (Add(instance) <= 0)
                        instance = null;
                }

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }

        public async Task<Periode> GetAsync(DateTime date)
        {
            Periode instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from periode " +
                    "where annee = @v_annee and mois = @v_mois";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_mois", DbType.Int32, date.Month));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_annee", DbType.Int32, date.Year));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows && Reader.Read())
                    _instances = Map(Reader);

                Reader.Close();

                if (_instances != null)
                    instance = Create(_instances);
                else
                {
                    instance = new Periode()
                    {
                        Mois = date.Month,
                        Annee = date.Year
                    };

                    Request.Parameters.Clear();

                    if (await AddAsync(instance) <= 0)
                        instance = null;
                }

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }

        public async Task<List<Periode>> GetAllAsync()
        {
            var intances = new List<Periode>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from periode " +
                    "order by annee desc, mois asc";

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

        public async Task<List<Periode>> GetAllAsync(DateTime lastUpdateTime)
        {
            var intances = new List<Periode>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from periode " +
                    "where created_at >= @v_time or updated_at >= @v_time";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_time", DbType.DateTime, lastUpdateTime));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var periode = Create(item);
                    intances.Add(periode);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task GetAllAsync(ObservableCollection<Periode> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from periode";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Periode periode = Create(item);
                    collection.Add(periode);
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
                { "mois", reader["mois"] },
                { "annee", reader["annee"] },
            };
        }
    }
}
