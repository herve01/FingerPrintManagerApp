using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class FonctionDao : Dao<Fonction>
    {
        public FonctionDao(DbConnection connection = null) : base(connection)
        {
            TableName = "fonction";
        }

        public override int Add(Fonction instance)
        {
            try
            {

                object uniteId = instance.Niveau == UniteType.Direction ? (instance.Unite as Direction).Id 
                    : (instance.Niveau == UniteType.Departement ? (instance.Unite as Departement).Id : (instance.Unite as Entite).Id);

                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into fonction(id, grade_id, intitule, niveau, unite_id, entite_id, description, created_at, updated_at) " +
                    "values(@v_id, @v_grade_id, @v_intitule, @v_niveau, @v_unite_id, @v_entite_id, @v_description, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_grade_id", DbType.String, instance.Grade.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_intitule", DbType.String, instance.Intitule));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_niveau", DbType.String, instance.Niveau));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, uniteId));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, instance.Entite.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_description", DbType.String, instance.Description));

                var feed = Request.ExecuteNonQuery();

                if (feed > 0)
                    instance.Id = id;


                return feed;
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
                return -1;
            }
        }

        public int Add(DbCommand command, Fonction fonction)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(fonction);
        }

        public async Task<int> AddAsync(Fonction instance)
        {
            try
            {
                object uniteId = instance.Niveau == UniteType.Direction ? (instance.Unite as Direction).Id
                              : (instance.Niveau == UniteType.Departement ? (instance.Unite as Departement).Id : (instance.Unite as Entite).Id);

                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into fonction(id, grade_id, intitule, niveau, unite_id, entite_id, description, created_at, updated_at) " +
                    "values(@v_id, @v_grade_id, @v_intitule, @v_niveau, @v_unite_id, @v_entite_id, @v_description, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_grade_id", DbType.String, instance.Grade.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_intitule", DbType.String, instance.Intitule));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_niveau", DbType.String, instance.Niveau));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, uniteId));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, instance.Entite.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_description", DbType.String, instance.Description));

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

        public async Task<int> AddAsync(DbCommand command, Fonction instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public async Task<int> AddAsync(List<Fonction> instances)
        {
            try
            {
                Request.Transaction = Connection.BeginTransaction();

                foreach (var fonction in instances)
                {
                    Request.Parameters.Clear();

                    if (await AddAsync(fonction) <= 0)
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

        public override int Update(Fonction instance, Fonction old)
        {
            try
            {
                object uniteId = instance.Niveau == UniteType.Direction ? (instance.Unite as Direction).Id
                              : (instance.Niveau == UniteType.Direction ? (instance.Unite as Departement).Id : (instance.Unite as Entite).Id);

                Request.CommandText = "update fonction " +
                    "set niveau = @v_niveau, " +
                    "grade_id = @v_grade_id, " +
                    "intitule = @v_intitule, " +
                    "niveau = @v_niveau, " +
                    "unite_id = @v_unite_id, " +
                    "entite_id = @v_entite_id, " +
                    "description = @v_description, " +
                    "updated_at = now() " +
                    "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_grade_id", DbType.String, instance.Grade.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_intitule", DbType.String, instance.Intitule));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_niveau", DbType.String, instance.Niveau));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, uniteId));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, instance.Entite.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_description", DbType.String, instance.Description));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public override int Delete(Fonction instance)
        {
            try
            {

                Request.CommandText = "delete from fonction " +
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

        private Fonction Create(Dictionary<string, object> row, bool withEntite, bool withUnite, bool withGrade = true)
        {
            var instance = new Fonction();

            instance.Id = row["id"].ToString();
            instance.Intitule = row["intitule"].ToString();
            instance.Description = row["description"].ToString();

            instance.Niveau = Util.ToUniteType(row["niveau"].ToString());

            if(withEntite)
                instance.Grade = new GradeDao(Connection).Get(row["grade_id"].ToString());

            if (withUnite)
            {
                var niveau = Util.ToUniteType(row["niveau"].ToString());
                switch (niveau)
                {
                    case UniteType.Direction:
                        instance.Unite = new DirectionDao().Get(row["unite_id"].ToString());
                        break;
                    case UniteType.Departement:
                        instance.Unite = new DepartementDao().Get(row["unite_id"].ToString());
                        break;
                    case UniteType.Agence:
                        instance.Unite = new EntiteDao().Get(row["unite_id"].ToString());
                        break;
                    default:
                        break;
                }
            }

            if (withEntite)
                instance.Entite = new EntiteDao().Get(row["entite_id"].ToString());

            return instance;
        }

        public int Count()
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from fonction";

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
                    "from fonction " +
                    "where entite_id = @v_entite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public Fonction Get(string id)
        {
            Fonction instance = null;
            Dictionary<string, object> _instance = null;

            try
            {
                Request.CommandText = "select * " +
                    "from fonction " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _instance = Map(Reader);

                Reader.Close();

                if (_instance != null)
                    instance = Create(_instance, true, true);

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }

        public async Task<List<Fonction>> GetAllAsync()
        {
            var intances = new List<Fonction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from fonction ";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Fonction fonction = Create(item, true, true);
                    intances.Add(fonction);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public List<Fonction> GetAll(Direction direction)
        {
            var intances = new List<Fonction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from fonction " +
                    "where niveau = 'Direction' and unite_id = @v_unite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, direction.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var fonction = Create(item, false, false, false);
                    fonction.Unite = direction;
                    intances.Add(fonction);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public List<Fonction> GetAll(Departement departement)
        {
            var intances = new List<Fonction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from fonction " +
                    "where niveau = 'Departement' and unite_id = @v_unite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, departement.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var fonction = Create(item, false, false);
                    fonction.Unite = departement;
                    intances.Add(fonction);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public List<Fonction> GetAll(Entite entite)
        {
            var intances = new List<Fonction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from fonction " +
                    "where niveau = 'Agence' and unite_id = @v_unite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, entite.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var fonction = Create(item, false, false);
                    fonction.Unite = entite;
                    intances.Add(fonction);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<Fonction>> GetAllAsync(DateTime lastUpdateTime)
        {
            var intances = new List<Fonction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from fonction " +
                    "where created_at >= @v_time or updated_at >= @v_time";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_time", DbType.DateTime, lastUpdateTime));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Fonction fonction = Create(item, false, true);
                    intances.Add(fonction);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<Fonction>> GetAllAsync(Entite entite)
        {
            var intances = new List<Fonction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from fonction " +
                    "where entite_id = @v_entite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var fonction = Create(item, false, true);
                    fonction.Entite = entite;
                    intances.Add(fonction);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task GetAllAsync(Entite entite, ObservableCollection<Fonction> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from fonction " +
                    "where entite_id = @v_entite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var fonction = Create(item, false, true);
                    fonction.Entite = entite;
                    collection.Add(fonction);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }
        }

        public async Task GetAllAsync(ObservableCollection<Fonction> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from fonction";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    Fonction fonction = Create(item, true, true);
                    collection.Add(fonction);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

        }

        public async Task<List<Fonction>> GetAllAsync(Direction direction)
        {
            var intances = new List<Fonction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from fonction " +
                    "where niveau = 'Direction' and unite_id = @v_unite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, direction.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var fonction = Create(item, false, false);
                    fonction.Unite = direction;
                    intances.Add(fonction);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<Fonction>> GetAllAsync(Departement departement)
        {
            var intances = new List<Fonction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from fonction " +
                    "where niveau = 'Departement' and unite_id = @v_unite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, departement.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var fonction = Create(item, false, false);
                    fonction.Unite = departement;
                    intances.Add(fonction);
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
                { "grade_id", reader["grade_id"] },
                { "intitule", reader["intitule"] },
                { "niveau", reader["niveau"] },
                { "unite_id", reader["unite_id"] },
                { "entite_id", reader["entite_id"] },
                { "description", reader["description"] },
            };
        }
    }
}


