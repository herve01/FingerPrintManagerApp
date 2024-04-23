using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class EmployeGradeDao : Dao<EmployeGrade>
    {
        public EmployeGradeDao(DbConnection connection = null) : base(connection)
        {
            TableName = "employe_grade";
        }

        public override int Add(EmployeGrade instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into employe_grade(id, employe_id, grade_id, type, est_initial, date, created_at, updated_at) " +
                    "values(@v_id, @v_employe_id, @v_grade_id, @v_type, @v_est_initial, @v_date, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_grade_id", DbType.String, instance.Grade.Id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_acte_id", DbType.String, instance.Acte.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_type", DbType.String, instance.Type));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_initial", DbType.Boolean, instance.EstInitial));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.Date, instance.Date));

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

        public int Add(DbCommand command, EmployeGrade employe_grade)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(employe_grade);
        }

        public async Task<int> AddAsync(EmployeGrade instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into employe_grade(id, employe_id, grade_id, type, est_initial, date, created_at, updated_at) " +
                    "values(@v_id, @v_employe_id, @v_grade_id, @v_type, @v_est_initial, @v_date, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_grade_id", DbType.String, instance.Grade.Id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_acte_id", DbType.String, instance.Acte.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_type", DbType.String, instance.Type));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_initial", DbType.Boolean, instance.EstInitial));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.Date, instance.Date));

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

        public async Task<int> AddAsync(DbCommand command, EmployeGrade instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public async Task<int> AddAsync(List<EmployeGrade> instances)
        {
            try
            {
                Request.Transaction = Connection.BeginTransaction();
                OwnAction = false;
                foreach (var instance in instances)
                {
                    Request.Parameters.Clear();

                    if (await AddAsync(instance) <= 0)
                    {
                        Request.Transaction.Rollback();
                        return -1;
                    }
                }

                Request.Transaction.Commit();

                return instances.Count;
            }
            catch (Exception)
            {
                Request.Transaction.Rollback();
                return -2;
            }
        }

        public override int Update(EmployeGrade instance, EmployeGrade old = null)
        {
            try
            {

                Request.CommandText = "update employe_grade " +
                    "set employe_id = @v_employe_id, " +
                    "grade_id = @v_grade_id, " +
                    "est_initial = @v_est_initial, " +
                    "type = @v_type, " +
                    "date = @v_date, " +
                    "updated_at = now() " +
                    "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_grade_id", DbType.String, instance.Grade.Id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_acte_id", DbType.String, instance.Acte.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_initial", DbType.Boolean, instance.EstInitial));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_type", DbType.String, instance.Type));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.Date, instance.Date));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -6;
            }
        }

        public override int Delete(EmployeGrade instance)
        {
            try
            {

                Request.CommandText = "delete from employe_grade " +
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

        private EmployeGrade Create(Dictionary<string, object> row, bool withEmploye)
        {
            var instance = new EmployeGrade();

            instance.Id = row["id"].ToString();
            instance.Grade = new GradeDao().Get(row["grade_id"].ToString());
            //instance.Acte = new ActeNominationDao().Get(row["acte_id"].ToString());
            instance.EstInitial = bool.Parse(row["est_initial"].ToString());
            instance.Date = DateTime.Parse(row["date"].ToString());
            instance.Type = Util.ToGradeEmployeType(row["type"].ToString());

            if (withEmploye)
                instance.Employe = new EmployeDao().Get(row["employe_id"].ToString());

            return instance;
        }

        public int Count()
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from employe_grade";

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public EmployeGrade Get(string id)
        {
            EmployeGrade instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from employe_grade " +
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

        public EmployeGrade GetCurrent(Model.Employe.Employe employe, GradeEmployeType type = GradeEmployeType.Commissionnement)
        {
            EmployeGrade instance = null;
            Dictionary<string, object> _instance = null;

            try
            {
                Request.CommandText = "select * " +
                    "from employe_grade " +
                    "where employe_id = @v_employe_id and type = @v_type " +
                    "order by date desc " +
                    "limit 1";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_type", DbType.String, type));

                Reader = Request.ExecuteReader();

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

        public EmployeGrade GetInitial(Model.Employe.Employe employe)
        {
            EmployeGrade instance = null;
            Dictionary<string, object> _instance = null;

            try
            {
                Request.CommandText = "select * " +
                    "from employe_grade " +
                    "where employe_id = @v_employe_id and est_initial = 1";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));

                Reader = Request.ExecuteReader();

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

        public async Task<List<EmployeGrade>> GetAllAsync(Model.Employe.Employe employe)
        {
            var intances = new List<EmployeGrade>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from employe_grade " +
                    "where employe_id = @v_employe_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var employe_grade = Create(item, false);
                    employe_grade.Employe = employe;
                    intances.Add(employe_grade);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task GetAllAsync(Entite entite, ObservableCollection<EmployeGrade> collection, int limit = 100)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select eg.* " +
                    "from employe_grade eg " +
                    "inner join grade g " +
                    "on eg.grade_id = g.id " +
                    "where eg.type = 'Officiel' and (@v_siege = 1 or get_employe_current_entite(eg.employe_id) = @v_entite_id) " +
                    "order by eg.date desc " +
                    "limit @v_limit ";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_siege", DbType.Int32, entite.EstPrincipale ? 1 : 0));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_limit", DbType.Int32, limit));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    collection.Add(Create(item, true));
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
                { "employe_id", reader["employe_id"] },
                { "grade_id", reader["grade_id"] },
                { "est_initial", reader["est_initial"] },
                { "date", reader["date"] },
                { "type", reader["type"] }
            };
        }
    }
}


