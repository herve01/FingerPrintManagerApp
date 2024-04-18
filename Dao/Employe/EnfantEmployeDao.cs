using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Employe
{
    public class EnfantEmployeDao : Dao<EnfantEmploye>
    {
        public EnfantEmployeDao()
        {
            TableName = "enfant_employe";
        }

        public override int Add(EnfantEmploye instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GetKey(TableName);

                Request.CommandText = "insert into enfant_employe(id, employe_id, nom, post_nom, prenom, sexe, date_naissance, adding_date, last_update_time) " +
                    "values(@v_id, @v_employe_id, @v_nom, @v_post_nom, @v_prenom, @v_sexe, @v_date_naissance, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_nom", DbType.String, instance.Nom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_post_nom", DbType.String, instance.PostNom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_prenom", DbType.String, instance.Prenom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_sexe", DbType.String, instance.Sexe));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date_naissance", DbType.Date, instance.DateNaissance));
               
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

        public int Add(DbCommand command, EnfantEmploye enfant_employe)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(enfant_employe);
        }

        public async Task<int> AddAsync(EnfantEmploye instance)
        {
            try
            {
                var id = Helper.TableKeyHelper.GetKey(TableName);

                Request.CommandText = "insert into enfant_employe(id, employe_id, nom, post_nom, prenom, sexe, date_naissance, adding_date, last_update_time) " +
                    "values(@v_id, @v_employe_id, @v_nom, @v_post_nom, @v_prenom, @v_sexe, @v_date_naissance, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_nom", DbType.String, instance.Nom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_post_nom", DbType.String, instance.PostNom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_prenom", DbType.String, instance.Prenom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_sexe", DbType.String, instance.Sexe));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date_naissance", DbType.Date, instance.DateNaissance));

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

        public async Task<int> AddAsync(DbCommand command, EnfantEmploye instance)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return await AddAsync(instance);
        }

        public override int Update(EnfantEmploye instance, EnfantEmploye old = null)
        {
            try
            {

                Request.CommandText = "update enfant_employe " +
                    "set nom = @v_nom, " +
                    "employe_id = @v_employe_id, " +
                    "post_nom = @v_post_nom, " +
                    "prenom = @v_prenom, " +
                    "sexe = @v_sexe, " +
                    "date_naissance = @v_date_naissance, " +
                    "last_update_time = now() " +
                    "where id = @v_id;";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, instance.Employe.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_nom", DbType.String, instance.Nom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_post_nom", DbType.String, instance.PostNom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_prenom", DbType.String, instance.Prenom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_sexe", DbType.String, instance.Sexe));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_date_naissance", DbType.Date, instance.DateNaissance));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, instance.Id));

                var feed = Request.ExecuteNonQuery();

                return feed;
            }
            catch (Exception)
            {
                return -6;
            }
        }

        public override int Delete(EnfantEmploye instance)
        {
            try
            {

                Request.CommandText = "delete from enfant_employe " +
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

        private EnfantEmploye Create(Dictionary<string, object> row, bool withEmploye)
        {
            var instance = new EnfantEmploye();

            instance.Id = row["id"].ToString();
            instance.Nom = row["nom"].ToString();
            instance.PostNom = row["post_nom"].ToString();
            instance.Prenom = row["prenom"].ToString();
            instance.Sexe = Util.ToSexeType(row["sexe"].ToString());
            instance.DateNaissance = DateTime.Parse(row["date_naissance"].ToString());

            if (withEmploye)
                instance.Employe = new EmployeDao().Get(row["employe_id"].ToString());

            return instance;
        }

        public int Count(Model.Employe.Employe employe)
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from enfant_employe " +
                    "where employe_id = @v_employe_id ";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public EnfantEmploye Get(string id)
        {
            EnfantEmploye instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from enfant_employe " +
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

        public async Task<List<EnfantEmploye>> GetAllAsync(Model.Employe.Employe employe)
        {
            var intances = new List<EnfantEmploye>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from enfant_employe " +
                    "where employe_id = @v_employe_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    EnfantEmploye enfant_employe = Create(item, false);
                    enfant_employe.Employe = employe;
                    intances.Add(enfant_employe);
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
                { "nom", reader["nom"] },
                { "post_nom", reader["post_nom"] },
                { "prenom", reader["prenom"] },
                { "sexe", reader["sexe"] },
                { "date_naissance", reader["date_naissance"] }
            };
        }

        #region Reporting
        public async Task<DataTable> GetEmployeEnfantsReportAsync(Model.Employe.Employe employe)
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                Request.CommandText = "select employe_id, nom, post_nom, prenom, sexe, date_naissance " +
                    "from enfant_employe " +
                    "where employe_id = @v_employe_id " +
                    "order by date_naissance desc";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_employe_id", DbType.String, employe.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                        list.Add(new Dictionary<string, object>()
                        {
                            { "employe_id", Reader["employe_id"] },
                            { "nom", Reader["nom"] },
                            { "post_nom", Reader["post_nom"] },
                            { "prenom", Reader["prenom"] },
                            { "sexe", Reader["sexe"] },
                            { "date_naissance", Reader["date_naissance"] }
                        });

                    Reader.Close();

                    var table = DbUtil.DicToTable(list);
                    return table;
                }
                else
                {
                    var st = Reader.GetSchemaTable();
                    var table = new DataTable();
                    foreach (DataRow row in st.Rows)
                    {
                        table.Columns.Add(row["ColumnName"].ToString(), Type.GetType(row["DataType"].ToString()));
                    }

                    Reader.Close();

                    return table;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }
        #endregion
    }
}


