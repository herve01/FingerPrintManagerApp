using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class AffectationDao : Dao<Affectation>
    {
        public AffectationDao(DbConnection connection = null) : base(connection)
        {
            TableName = "affectation";
        }

        public override int Add(Affectation instance)
        {
            try
            {
                if (OwnAction)
                    Request.Transaction = Connection.BeginTransaction();

                //object uniteId = instance.Niveau == UniteType.Direction ? (instance.Unite as Direction).Id
                //     : (instance.Niveau == UniteType.Direction ? (instance.Unite as Division).Id : (instance.Unite as Bureau).Id);

                var id = Helper.TableKeyHelper.GenerateKey(TableName);
                
                Request.CommandText = "insert into affectation(id, employe_id, ancienne_entite_id, nouvelle_entite_id, acte_id, niveau, unite_id, date, created_at, updated_at) " +
                    "values(@v_id, @v_employe_id, @v_ancienne_entite_id, @v_nouvelle_entite_id, @v_acte_id, @v_niveau, @v_unite_id, @v_date, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_ancienne_entite_id", DbType.String, instance.AncienneEntite?.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_nouvelle_entite_id", DbType.String, instance.Entite.Id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_acte_id", DbType.String, instance.Acte?.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_niveau", DbType.String, instance.Niveau));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, uniteId));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.Date, instance.Date));

                var feed = Request.ExecuteNonQuery();

                if (feed <= 0)
                {
                    if (OwnAction)
                        Request.Transaction.Rollback();

                    return -1;
                }

                instance.Id = id;

                if (new EmployeDao().SetEstAffecte(Request, instance.Employe, true) <= 0)
                {
                    if (OwnAction)
                        Request.Transaction.Rollback();

                    return -2;
                }

                instance.Employe.EstAffecte = true;

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

        public int Add(DbCommand command, Affectation affectation)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(affectation);
        }

        public async Task<int> AddAsync(Affectation instance)
        {
            try
            {
                if (OwnAction)
                    Request.Transaction = Connection.BeginTransaction();

                //object uniteId = instance.Niveau == UniteType.Direction ? (instance.Unite as Direction).Id
                //     : (instance.Niveau == UniteType.Direction ? (instance.Unite as Division).Id : (instance.Unite as Bureau).Id);

                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "insert into affectation(id, employe_id, ancienne_entite_id, nouvelle_entite_id, acte_id, niveau, unite_id, date, created_at, updated_at) " +
                    "values(@v_id, @v_employe_id, @v_ancienne_entite_id, @v_nouvelle_entite_id, @v_acte_id, @v_niveau, @v_unite_id, @v_date, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_ancienne_entite_id", DbType.String, instance.AncienneEntite?.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_nouvelle_entite_id", DbType.String, instance.Entite.Id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_acte_id", DbType.String, instance.Acte?.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_niveau", DbType.String, instance.Niveau));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, uniteId));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.Date, instance.Date));

                var feed = await Request.ExecuteNonQueryAsync();

                if (feed <= 0)
                {
                    if (OwnAction)
                        Request.Transaction.Rollback();

                    return -1;
                }

                instance.Id = id;

                if (new EmployeDao().SetEstAffecte(Request, instance.Employe, true) <= 0)
                {
                    if (OwnAction)
                        Request.Transaction.Rollback();

                    return -2;
                }

                instance.Employe.EstAffecte = true;

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

        public async Task<int> AddAsync(DbCommand command, Affectation instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public override int Update(Affectation instance, Affectation old)
        {
            try
            {
                //object uniteId = instance.Niveau == UniteType.Direction ? (instance.Unite as Direction).Id
                //    : (instance.Niveau == UniteType.Direction ? (instance.Unite as Division).Id : (instance.Unite as Bureau).Id);
                
                Request.CommandText = "update affectation " +
                    "set employe_id = @v_employe_id, " +
                    "ancienne_entite_id = @v_ancienne_entite_id, " +
                    "nouvelle_entite_id = @v_nouvelle_entite_id, "+
                    "acte_id = @v_acte_id, " +
                    "niveau = @v_niveau, " +
                    "unite_id = @v_unite_id, " +
                    "date = @v_date, " +
                    "updated_at = now() " +
                    "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_ancienne_entite_id", DbType.String, instance.AncienneEntite.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_nouvelle_entite_id", DbType.String, instance.Entite.Id));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_acte_id", DbType.String, instance.Acte));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_niveau", DbType.Single, instance.Niveau));
                //Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_unite_id", DbType.String, uniteId));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date", DbType.Date, instance.Date));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        
        public override int Delete(Affectation instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GenerateKey(TableName);

                Request.CommandText = "delete from affectation " +
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
        
        private Affectation Create(Dictionary<string, object> row, bool withEmploye)
        {
            var instance = new Affectation();
           
            instance.Id = row["id"].ToString();
            instance.Date = DateTime.Parse( row["date"].ToString());
            instance.Entite = new EntiteDao().Get(row["nouvelle_entite_id"].ToString());
            
            instance.Niveau = Util.ToUniteType(row["niveau"].ToString());

            //var niveau = Util.ToUniteType(row["niveau"].ToString());
            //switch (niveau)
            //{
            //    case UniteType.Direction:
            //        instance.Unite = new DirectionDao().Get(row["unite_id"].ToString());
            //        break;
            //    case UniteType.Division:
            //        instance.Unite = new DivisionDao().Get(row["unite_id"].ToString());
            //        break;
            //    case UniteType.Bureau:
            //        instance.Unite = new BureauDao().Get(row["unite_id"].ToString());
            //        break;
            //    default:
            //        break;
            //}

            //if (!(row["acte_id"] is DBNull))
            //    instance.Acte = new ActeNominationDao().Get(row["acte_id"].ToString());

            if (!(row["ancienne_entite_id"] is DBNull))
                instance.AncienneEntite = new EntiteDao().Get(row["ancienne_entite_id"].ToString());

            if (withEmploye)
                instance.Employe = new EmployeDao().Get(row["employe_id"].ToString());

            return instance;

        }

        public int Count(Entite entite)
        {
            try
            {
                var siege = entite.EstPrincipale ? 1 : 0;

                Request.CommandText = "select count(*) " +
                    "from affectation " +
                    "where @v_est_siege = 1 or ancienne_entite_id = @v_entite_id or nouvelle_entite_id = @v_entite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_siege", DbType.Int32, siege));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                return int.Parse(Request.ExecuteScalar().ToString());
                
            }
            catch (Exception)
            {
            }

            return 0;
        }

        public Affectation Get(string id)
        {
            Affectation affectation = null;
            Dictionary<string, object> _affectation = null;

            try
            {
                Request.CommandText = "select * " +
                    "from affectation " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _affectation = Map(Reader);

                Reader.Close();

                if (_affectation != null)
                    affectation = Create(_affectation, true);

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return affectation;
        }

        public Affectation GetCurrent(Model.Employe.Employe employe)
        {
            Affectation affectation = null;
            Dictionary<string, object> _affectation = null;

            try
            {
                Request.CommandText = "select * " +
                    "from affectation " +
                    "where employe_id = @v_employe_id " +
                    "order by id desc " +
                    "limit 1";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _affectation = Map(Reader);

                Reader.Close();

                if (_affectation != null)
                {
                    affectation = Create(_affectation, false);
                    affectation.Employe = employe;
                }

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return affectation;
        }

        public async Task<List<Affectation>> GetAllAsync()
        {
            var affectationes = new List<Affectation>();
            var _affectationes = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from affectation";
                
                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _affectationes.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _affectationes)
                {
                    Affectation affectation = Create(item, true);
                    affectationes.Add(affectation);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return affectationes;
        }

        public async Task GetAllAsync(ObservableCollection<Affectation> collection)
        {
            var _affectationes = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from affectation ";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _affectationes.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _affectationes)
                {
                    Affectation affectation = Create(item, true);
                    collection.Add(affectation);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }
            
        }

        public async Task GetAllAsync(Entite entite, ObservableCollection<Affectation> collection)
        {
            var _affectationes = new List<Dictionary<string, object>>();

            try
            {
                var siege = entite.EstPrincipale ? 1 : 0;

                Request.CommandText = "select * " +
                    "from affectation " +
                    "where @v_est_siege = 1 or ancienne_entite_id = @v_entite_id or nouvelle_entite_id = @v_entite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_est_siege", DbType.Int32, siege));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _affectationes.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _affectationes)
                {
                    Affectation affectation = Create(item, true);
                    collection.Add(affectation);
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
                { "ancienne_entite_id", reader["ancienne_entite_id"] },
                { "nouvelle_entite_id", reader["nouvelle_entite_id"] },
                { "acte_id", reader["acte_id"] },
                { "niveau", reader["niveau"] },
                { "unite_id", reader["unite_id"] },
                { "date", reader["date"] }
            };
        }
    }
}


