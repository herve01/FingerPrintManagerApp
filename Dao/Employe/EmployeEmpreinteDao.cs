using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class EmployeEmpreinteDao : Dao<EmployeEmpreinte>
    {
        public EmployeEmpreinteDao(DbConnection connection = null) : base(connection)
        {
            TableName = "employe_empreinte";
        }

        public override int Add(EmployeEmpreinte instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into employe_empreinte(id, employe_id, size, empreinte_image, empreinte_template, finger, created_at, updated_at) " +
                    "values(@v_id, @v_employe_id, @v_size, @v_empreinte_image, @v_empreinte_template, @v_finger, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_size", DbType.Int32, instance.Size));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_empreinte_image", DbType.Binary, instance.Image));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_empreinte_template", DbType.Binary, instance.Template));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_finger", DbType.String, instance.Finger));

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

        public int Add(DbCommand command, EmployeEmpreinte employe_empreinte)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(employe_empreinte);
        }

        public async Task<int> AddAsync(EmployeEmpreinte instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into employe_empreinte(id, employe_id, size, empreinte_image, empreinte_template, finger, created_at, updated_at) " +
                    "values(@v_id, @v_employe_id, @v_size, @v_empreinte_image, @v_empreinte_template, @v_finger, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_size", DbType.Int32, instance.Size));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_empreinte_image", DbType.Binary, instance.Image));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_empreinte_template", DbType.Binary, instance.Template));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_finger", DbType.String, instance.Finger));

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

        public async Task<int> AddAsync(DbCommand command, EmployeEmpreinte instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public async Task<int> AddAsync(List<EmployeEmpreinte> instances)
        {
            if (OwnAction)
                Request.Transaction = Connection.BeginTransaction();

            foreach (var instance in instances)
            {
                Request.Parameters.Clear();

                if (await AddAsync(instance) <= 0)
                {
                    if (OwnAction)
                        Request.Transaction.Rollback();

                    return -1;
                }
            }

            if (OwnAction)
                Request.Transaction.Commit();

            return instances.Count;
        }

        public async Task<int> AddAsync(DbCommand command, List<EmployeEmpreinte> instances)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instances);
        }

        public override int Update(EmployeEmpreinte instance, EmployeEmpreinte old)
        {
            try
            {

                Request.CommandText = "update employe_empreinte " +
                    "set employe_id = @v_employe_id, " +
                    "size = @v_size, " +
                    "empreinte_template = @v_empreinte_template, " +
                    "empreinte_image = @v_empreinte_image, " +
                    "finger = @v_finger, " +
                    "updated_at = now() " +
                    "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_size", DbType.Int32, instance.Size));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_empreinte_image", DbType.Binary, instance.Image));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_empreinte_template", DbType.Binary, instance.Template));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_finger", DbType.String, instance.Finger));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -6;
            }
        }

        public override int Delete(EmployeEmpreinte instance)
        {
            try
            {

                Request.CommandText = "delete from employe_empreinte " +
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

        public int Count()
        {
            try
            {

                Request.CommandText = "select count(*) " +
                    "from employe_empreinte ";

                return int.Parse(Request.ExecuteScalar().ToString());
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private EmployeEmpreinte Create(Dictionary<string, object> row, bool withEmploye)
        {
            var instance = new EmployeEmpreinte();

            instance.Id = row["id"].ToString();
            instance.Size = int.Parse(row["size"].ToString());
            instance.Image = (byte[])row["empreinte_image"];
            instance.Template = (byte[])row["empreinte_template"];
            instance.Finger = Util.ToFingers(row["finger"].ToString());

            if (withEmploye)
                instance.Employe = new EmployeDao().Get(row["employe_id"].ToString());

            return instance;
        }

        public async Task<EmployeEmpreinte> GetAsync(byte[] template, float minScore = 90)
        {
            EmployeEmpreinte instance = null;
            Dictionary<string, object> _instance = null;

            try
            {
                Request.CommandText = "select * " +
                    "from employe_empreinte ";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                    {
                        _instance = Map(Reader);
                        var _template = (byte[])_instance["empreinte_template"];

                        if (Util.FingerPrintsMatchingScore(template, _template) >= minScore)
                            break;

                        _instance = null;
                    }

                Reader.Close();

                if (_instance != null)
                    instance = Create(_instance, true);

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }

        public async Task<List<EmployeEmpreinte>> GetAllAsync(int offset, int limit)
        {
            var intances = new List<EmployeEmpreinte>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from employe_empreinte " +
                    "limit @v_off_set, @v_limit";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_off_set", DbType.Int32, offset));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_limit", DbType.Int32, limit));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var instance = Create(item, true);
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

        public List<EmployeEmpreinte> GetAll(Model.Employe.Employe employe)
        {
            var intances = new List<EmployeEmpreinte>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from employe_empreinte " +
                    "where employe_id = @v_employe_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var instance = Create(item, false);
                    instance.Employe = employe;
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

        public async Task<List<EmployeEmpreinte>> GetAllAsync(Model.Employe.Employe employe)
        {
            var intances = new List<EmployeEmpreinte>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from employe_empreinte " +
                    "where employe_id = @v_employe_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var instance = Create(item, false);
                    instance.Employe = employe;
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

        protected override Dictionary<string, object> Map(DbDataReader reader)
        {
            return new Dictionary<string, object>()
            {
                { "id", reader["id"] },
                { "employe_id", reader["employe_id"] },
                { "size", reader["size"] },
                { "empreinte_image", reader["empreinte_image"] },
                { "empreinte_template", reader["empreinte_template"] },
                { "finger", reader["finger"] }
            };
        }
    }
}


