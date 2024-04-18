﻿using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class EmployeFonctionDao : Dao<EmployeFonction>
    {
        public EmployeFonctionDao()
        {
            TableName = "employe_fonction";
        }

        public override int Add(EmployeFonction instance)
        {
            try
            {
                if (OwnAction)
                    Request.Transaction = Connection.BeginTransaction();

                var id = Helper.TableKeyHelper.GetKey(TableName);

                Request.CommandText = "insert into employe_fonction(id, employe_id, fonction_id, acte_id, date, type, adding_date, last_update_time) " +
                    "values(@v_id, @v_employe_id, @v_fonction_id, @v_acte_id, @v_date, @v_type, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_fonction_id", DbType.String, instance.Fonction.Id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_acte_id", DbType.String, instance.Acte.Id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_type", DbType.String, instance.Type));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.Date, instance.Date));
               
                var feed = Request.ExecuteNonQuery();

                if (feed <= 0)
                {
                    if (OwnAction)
                        Request.Transaction.Rollback();
                    return -1;
                }

                if (instance.Type == FonctionEmployeType.Officiel)
                    if (new EmployeGradeDao().Add(Request, instance.GradeAssocie) <= 0)
                    {
                        if (OwnAction)
                            Request.Transaction.Rollback();
                        return -2;
                    }

                instance.Id = id;

                if (OwnAction)
                    Request.Transaction.Commit();

                return feed;
            }
            catch (Exception)
            {
                if (OwnAction)
                    Request.Transaction.Rollback();

                return -3;
            }
        }

        public int Add(DbCommand command, EmployeFonction employe_fonction)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(employe_fonction);
        }

        public async Task<int> AddAsync(EmployeFonction instance)
        {
            try
            {
                if (OwnAction)
                    Request.Transaction = Connection.BeginTransaction();

                var id = Helper.TableKeyHelper.GetKey(TableName);

                Request.CommandText = "insert into employe_fonction(id, employe_id, fonction_id, acte_id, date, type, adding_date, last_update_time) " +
                    "values(@v_id, @v_employe_id, @v_fonction_id, @v_acte_id, @v_date, @v_type, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_fonction_id", DbType.String, instance.Fonction.Id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_acte_id", DbType.String, instance.Acte.Id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_type", DbType.String, instance.Type));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.Date, instance.Date));

                var feed = await Request.ExecuteNonQueryAsync();

                if (feed <= 0)
                {
                    if (OwnAction)
                        Request.Transaction.Rollback();
                    return -1;
                }

                if (instance.Type == FonctionEmployeType.Officiel)
                    if (await new EmployeGradeDao().AddAsync(Request, instance.GradeAssocie) <= 0)
                    {
                        if (OwnAction)
                            Request.Transaction.Rollback();
                        return -2;
                    }

                instance.Id = id;

                if (OwnAction)
                    Request.Transaction.Commit();

                return feed;
            }
            catch (Exception)
            {
                if (OwnAction)
                    Request.Transaction.Rollback();
                return -3;
            }
        }

        public async Task<int> AddAsync(DbCommand command, EmployeFonction instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public async Task<int> AddAsync(EmployeFonction instance, Affectation affectation)
        {
            try
            {
                Request.Transaction = Connection.BeginTransaction();
                Request.Parameters.Clear();

                OwnAction = false;

                var feed = await AddAsync(instance);

                if (feed <= 0)
                {
                    Request.Transaction.Rollback();
                    return -1;
                }

                if (await new AffectationDao().AddAsync(Request, affectation) <= 0)
                {
                    Request.Transaction.Rollback();
                    return -2;
                }

                Request.Transaction.Commit();
                return 2;
            }
            catch (Exception)
            {
                Request.Transaction.Rollback();
                return -3;
            }
        }

        public override int Update(EmployeFonction instance, EmployeFonction old = null)
        {
            try
            {

                Request.CommandText = "update employe_fonction " +
                    "set employe_id = @v_employe_id, " +
                    "fonction_id = @v_fonction_id, " +
                    "acte_id = @v_acte_id, " +
                    "type = @v_type, " +
                    "date = @v_date, " +
                    "last_update_time = now() " +
                    "where id = @v_id;";

                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_fonction_id", DbType.String, instance.Fonction.Id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_acte_id", DbType.String, instance.Acte.Id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_type", DbType.String, instance.Type));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.Date, instance.Date));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public override int Delete(EmployeFonction instance)
        {
            try
            {

                Request.CommandText = "delete from employe_fonction " +
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

        public int End(EmployeFonction instance)
        {
            try
            {
                Request.CommandText = "update employe_fonction " +
                    "set date_fin = @v_date " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.Date, instance.DateFin));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int End(DbCommand command, EmployeFonction employe_fonction)
        {
            Request = command;
            Request.Parameters.Clear();

            return End(employe_fonction);
        }

        public int Pause(EmployeFonction instance)
        {
            try
            {
                Request.CommandText = "update employe_fonction " +
                    "set state = 'Pause' " +
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

        public int Pause(DbCommand command, EmployeFonction employe_fonction)
        {
            Request = command;
            Request.Parameters.Clear();

            return Pause(employe_fonction);
        }

        private EmployeFonction Create(Dictionary<string, object> row, bool withEmploye)
        {
            var instance = new EmployeFonction();

            instance.Id = row["id"].ToString();
            instance.Fonction = new FonctionDao().Get(row["fonction_id"].ToString());
            //instance.Acte = new ActeNominationDao().Get(row["acte_id"].ToString());
            //instance.Type = Util.ToFonctionEmployeType(row["type"].ToString());
            //instance.State = Util.ToFonctionState(row["state"].ToString());
            //instance.Date = DateTime.Parse(row["date"].ToString());

            if (!(row["date_fin"] is DBNull))
                instance.DateFin = DateTime.Parse(row["date_fin"].ToString());

            //if (withEmploye)
            //    instance.Employe = new EmployeDao().Get(row["employe_id"].ToString());

            return instance;
        }

        public int Count()
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from employe_fonction";

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public EmployeFonction Get(string id)
        {
            EmployeFonction instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from employe_fonction " +
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

        public EmployeFonction GetCurrent(Model.Employe.Employe employe, bool interim = false)
        {
            EmployeFonction instance = null;
            Dictionary<string, object> _instance = null;

            var op = interim ? "=" : "!=";

            try
            {
                Request.CommandText = string.Format("select * " +
                    "from employe_fonction " +
                    "where employe_id = @v_employe_id and type {0} 'Interim' and date_fin is null " +
                    "order by date desc " +
                    "limit 1", op);

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _instance = Map(Reader);

                Reader.Close();

                if (_instance != null)
                {
                    instance = Create(_instance, false);
                    //instance.Employe = employe;
                }

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }

        public List<EmployeFonction> GetAllInterim(Model.Employe.Employe employe)
        {
            List<EmployeFonction> instances = null;
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from employe_fonction " +
                    "where type = 'Interim' and date_fin is not null and employe_id = @v_employe_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                {
                    instances = new List<EmployeFonction>();

                    while (Reader.Read())
                        _instances.Add(Map(Reader));
                }

                Reader.Close();


                foreach (var item in _instances)
                {
                    EmployeFonction employe_fonction = Create(item, false);
                    employe_fonction.Employe = employe;
                    instances.Add(employe_fonction);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instances;
        }

        public async Task<List<EmployeFonction>> GetAllAsync(Model.Employe.Employe employe)
        {
            var intances = new List<EmployeFonction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from employe_fonction " +
                    "where employe_id = @v_employe_id ";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    EmployeFonction employe_fonction = Create(item, false);
                    employe_fonction.Employe = employe;
                    intances.Add(employe_fonction);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

       
        public async Task<List<EmployeFonction>> GetAllRunningAsync(Division division)
        {
            var intances = new List<EmployeFonction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select EF.* " +
                    "from employe_fonction EF " +
                    "inner join fonction F " +
                    "on EF.fonction_id = F.id " +
                    "where EF.date_fin is null and F.niveau = 'Division' and F.unite_id = @v_unite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, division.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var fonction = Create(item, true);
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

        public async Task<List<EmployeFonction>> GetAllRunningAsync(Bureau bureau)
        {
            var intances = new List<EmployeFonction>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select EF.* " +
                    "from employe_fonction EF " +
                    "inner join fonction F " +
                    "on EF.fonction_id = F.id " +
                    "where EF.date_fin is null and F.niveau = 'Bureau' and F.unite_id = @v_unite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, bureau.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var fonction = Create(item, true);
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
                { "employe_id", reader["employe_id"] },
                { "fonction_id", reader["fonction_id"] },
                { "acte_id", reader["acte_id"] },
                { "type", reader["type"] },
                { "state", reader["state"] },
                { "date", reader["date"] },
                { "date_fin", reader["date_fin"] }
            };
        }
    }
}

