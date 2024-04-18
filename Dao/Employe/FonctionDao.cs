﻿using FingerPrintManagerApp.Model.Employe;
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
        public FonctionDao()
        {
            TableName = "fonction";
        }

        public override int Add(Fonction instance)
        {
            try
            {

                //object uniteId = instance.Niveau == UniteType.Direction ? (instance.Unite as DirectionInterne).Id 
                //    : (instance.Niveau == UniteType.Direction ? (instance.Unite as Division).Id : (instance.Unite as Bureau).Id);

                var id = Helper.TableKeyHelper.GetKey(TableName);

                Request.CommandText = "insert into fonction(id, grade_id, intitule, niveau, unite_id, entite_id, description, adding_date, last_update_time) " +
                    "values(@v_id, @v_grade_id, @v_intitule, @v_niveau, @v_unite_id, @v_entite_id, @v_description, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_grade_id", DbType.String, instance.Grade.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_intitule", DbType.String, instance.Intitule));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_niveau", DbType.String, instance.Niveau));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, uniteId));
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
                //object uniteId = instance.Niveau == UniteType.Direction ? (instance.Unite as DirectionInterne).Id
                //         : (instance.Niveau == UniteType.Direction ? (instance.Unite as Division).Id : (instance.Unite as Bureau).Id);
                
                var id = Helper.TableKeyHelper.GetKey(TableName);

                Request.CommandText = "insert into fonction(id, grade_id, intitule, niveau, unite_id, entite_id, description, adding_date, last_update_time) " +
                    "values(@v_id, @v_grade_id, @v_intitule, @v_niveau, @v_unite_id, @v_entite_id, @v_description, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_grade_id", DbType.String, instance.Grade.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_intitule", DbType.String, instance.Intitule));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_niveau", DbType.String, instance.Niveau));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, uniteId));
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
                //object uniteId = instance.Niveau == UniteType.Direction ? (instance.Unite as DirectionInterne).Id
                //    : (instance.Niveau == UniteType.Direction ? (instance.Unite as Division).Id : (instance.Unite as Bureau).Id);
            
                Request.CommandText = "update fonction " +
                    "set niveau = @v_niveau, " +
                    "grade_id = @v_grade_id, " +
                    "intitule = @v_intitule, " +
                    "niveau = @v_niveau, " +
                    "unite_id = @v_unite_id, " +
                    "entite_id = @v_entite_id, " +
                    "description = @v_description, " +
                    "last_update_time = now() " +
                    "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_grade_id", DbType.String, instance.Grade.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_intitule", DbType.String, instance.Intitule));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_niveau", DbType.String, instance.Niveau));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, uniteId));
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

        private Fonction Create(Dictionary<string, object> row, bool withEntite, bool withUnite)
        {
            var instance = new Fonction();

            instance.Id = row["id"].ToString();
            instance.Intitule = row["intitule"].ToString();
            instance.Description = row["description"].ToString();

            instance.Niveau = Util.ToUniteType(row["niveau"].ToString());
            instance.Grade = new GradeDao().Get(row["grade_id"].ToString());

            if (withUnite)
            {
                var niveau = Util.ToUniteType(row["niveau"].ToString());
                switch (niveau)
                {
                    //case UniteType.Direction:
                    //    instance.Unite = new DirectionDao().Get(row["unite_id"].ToString());
                    //    break;
                    case UniteType.Division:
                        instance.Unite = new DivisionDao().Get(row["unite_id"].ToString());
                        break;
                    case UniteType.Bureau:
                        instance.Unite = new BureauDao().Get(row["unite_id"].ToString());
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

        public List<Fonction> GetAll(Entite entite)
        {
            var intances = new List<Fonction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from fonction " +
                    "where entite_id = @v_entite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
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



        public List<Fonction> GetAll(Division division)
        {
            var intances = new List<Fonction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from fonction " +
                    "where niveau = 'Division' and unite_id = @v_unite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, division.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var fonction = Create(item, false, false);
                    fonction.Unite = division;
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

        public List<Fonction> GetAll(Bureau bureau)
        {
            var intances = new List<Fonction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from fonction " +
                    "where niveau = 'Bureau' and unite_id = @v_unite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, bureau.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var fonction = Create(item, false, false);
                    fonction.Unite = bureau;
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
                    "where adding_date >= @v_time or last_update_time >= @v_time";

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

        //public async Task<List<Fonction>> GetAllAsync()
        //{
        //    var intances = new List<Fonction>();
        //    var _instances = new List<Dictionary<string, object>>();

        //    try
        //    {
        //        Request.CommandText = "select * " +
        //            "from fonction " +
        //            "where niveau = 'Direction' and unite_id = @v_unite_id";

        //        //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, direction.Id));

        //        Reader = await Request.ExecuteReaderAsync();

        //        if (Reader.HasRows)
        //            while (Reader.Read())
        //                _instances.Add(Map(Reader));

        //        Reader.Close();

        //        foreach (var item in _instances)
        //        {
        //            var fonction = Create(item, false, false);
        //            fonction.Unite = direction;
        //            intances.Add(fonction);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        if (Reader != null && !Reader.IsClosed)
        //            Reader.Close();
        //    }

        //    return intances;
        //}

        public async Task<List<Fonction>> GetAllAsync(Division division)
        {
            var intances = new List<Fonction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from fonction " +
                    "where niveau = 'Division' and unite_id = @v_unite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, division.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var fonction = Create(item, false, false);
                    fonction.Unite = division;
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

        public async Task<List<Fonction>> GetAllAsync(Bureau bureau)
        {
            var intances = new List<Fonction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from fonction " +
                    "where niveau = 'Bureau' and unite_id = @v_unite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, bureau.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var fonction = Create(item, false, false);
                    fonction.Unite = bureau;
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

