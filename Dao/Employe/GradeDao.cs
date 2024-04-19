using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class GradeDao : Dao<Grade>
    {
        public GradeDao(DbConnection connection = null) : base(connection)
        {
            TableName = "grade";
        }

        public override int Add(Grade instance)
        {
            try
            {
                Request.CommandText = "insert into grade(id, intitule, type, niveau, description, created_at, updated_at) " +
                    "values(@v_id, @v_intitule, @v_type, @v_niveau, @v_description, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_intitule", DbType.String, instance.Intitule));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_type", DbType.String, instance.Type));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_niveau", DbType.Int32, instance.Niveau));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_description", DbType.String, instance.Description));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int Add(DbCommand command, Grade grade)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(grade);
        }

        public async Task<int> AddAsync(Grade instance)
        {
            try
            {
                Request.CommandText = "insert into grade(id, intitule, type, niveau, description, created_at, updated_at) " +
                    "values(@v_id, @v_intitule, @v_type, @v_niveau, @v_description, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_intitule", DbType.String, instance.Intitule));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_type", DbType.String, instance.Type));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_niveau", DbType.Int32, instance.Niveau));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_description", DbType.String, instance.Description));

                var feed = await Request.ExecuteNonQueryAsync();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<int> AddAsync(DbCommand command, Grade instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public override int Update(Grade instance, Grade old)
        {
            try
            {

                Request.CommandText = "update grade " +
                    "set type = @v_type, " +
                    "intitule = @v_intitule, " +
                    "niveau = @v_niveau, " +
                    "description = @v_description, " +
                    "updated_at = now() " +
                    "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_intitule", DbType.String, instance.Intitule));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_type", DbType.String, instance.Type));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_niveau", DbType.Int32, instance.Niveau));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_description", DbType.String, instance.Description));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -6;
            }
        }

        public override int Delete(Grade instance)
        {
            try
            {

                Request.CommandText = "delete from grade " +
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

        private Grade Create(Dictionary<string, object> row)
        {
            Grade instance = new Grade();

            instance.Id = row["id"].ToString();
            instance.Intitule = row["intitule"].ToString();
            instance.Type = row["type"].ToString();
            instance.Niveau = int.Parse(row["niveau"].ToString());
            instance.Description = row["description"].ToString();

            return instance;
        }

        public int Count()
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from grade";

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public Grade Get(string id)
        {
            Grade instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from grade " +
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

        public async Task<List<Grade>> GetAllAsync()
        {
            var intances = new List<Grade>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from grade " +
                    "order by niveau desc ";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Grade grade = Create(item);
                    intances.Add(grade);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }
        public async Task<List<Grade>> GetAllAsync(DateTime lastUpdateTime)
        {
            var intances = new List<Grade>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from grade " +
                    "where created_at >= @v_time or updated_at >= @v_time";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_time", DbType.DateTime, lastUpdateTime));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Grade grade = Create(item);
                    intances.Add(grade);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task GetAllAsync(ObservableCollection<Grade> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from grade " +
                    "order by niveau desc";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Grade grade = Create(item);
                    collection.Add(grade);
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
                { "intitule", reader["intitule"] },
                { "type", reader["type"] },
                { "niveau", reader["niveau"] },
                { "description", reader["description"] }
            };
        }
    }
}


