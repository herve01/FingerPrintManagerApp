using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using FingerPrintManagerApp.Dao.Employe;
using FingerPrintManagerApp.Model.Admin;
using FingerPrintManagerApp.Model.Employe;

namespace FingerPrintManagerApp.Dao.Admin
{
    public class UserDao : Dao<User>
    {
        public UserDao()
        {
            TableName = "user";
        }

        public override int Add(User user)
        {
            try
            {
                var id = Helper.TableKeyHelper.GetKey(TableName);

                var hash = PasswordStorage.CreateHash(user.PassWd);
                var split = hash.Split(':');
                var salt = split[0];
                var pwd = string.Format("{0}:{1}", split[1], split[2]);
                
                Request.CommandText = "insert into user(id, nom, prenom, type, username, sexe, email, passwd, m_salt, telephone, entite_id, adding_date, last_update_time) " +
                    "values (@v_id, @v_nom, @v_prenom, @v_type, @v_username, @v_sexe, @v_email, @v_passwd, @v_m_salt, @v_telephone, @v_entite_id, now(), now())";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_nom", DbType.String, user.Nom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_prenom", DbType.String, user.Prenom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_type", DbType.String, user.Type));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_username", DbType.String, user.UserName));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_sexe", DbType.String, user.Sex));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_email", DbType.String, user.Email));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_passwd", DbType.String, pwd));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_m_salt", DbType.String, salt));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_telephone", DbType.String, user.Telephone));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, user.Entite.Id));

                var feed = Request.ExecuteNonQuery();

                if (feed > 0)
                    user.Id = id;

                return feed;

            }
            catch (Exception)
            {
                return -1;
            }


        }

        public override int Update(User user, User old = null)
        {
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                Connection.Open();
                Request.Transaction = Connection.BeginTransaction();

                Request.CommandText = "update user " +
                    "set nom = @v_nom, " +
                    "prenom = @v_prenom, " +
                    "type = @v_type, " +
                    "username = @v_username, " +
                    "sexe = @v_sexe, " +
                    "email = @v_email, " +
                    "telephone = @v_telephone, " +
                    "entite_id = @v_entite_id, " +
                    "last_update_time = now() " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_nom", DbType.String, user.Nom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_prenom", DbType.String, user.Prenom));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_type", DbType.String, user.Type));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_username", DbType.String, user.UserName));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_sexe", DbType.String, user.Sex));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_email", DbType.String, user.Email));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_telephone", DbType.String, user.Telephone));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.Int32, user.Entite.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.Int32, user.Id));

                return Request.ExecuteNonQuery();
                
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int Count(Entite entite)
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from user " +
                    "where @v_siege = 1 or entite_id = @v_entite_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_siege", DbType.Int32, entite.EstPrincipale ? 1 : 0));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public User GetUser(string username, string passwd)
        {
            User user = null;
            Dictionary<string, object> _user = null;

            try
            {
                Request.CommandText = "select * " +
                    "from user " +
                    "where username = @v_username or email = @v_username";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_username", DbType.String, username));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                {
                    var uPwd = Reader["passwd"].ToString();
                    var uSalt = Reader["m_salt"].ToString();

                    if (PasswordStorage.VerifyPassword(passwd, uSalt, uPwd))
                        _user = Map(Reader);
                }

                Reader.Close();

                if (_user != null)
                    user = Create(_user, true);
            }
            catch (Exception)
            {
            }

            return user;
        }

        public async Task<User> GetUserAsync(string username, string passwd)
        {
            User user = null;
            Dictionary<string, object> _user = null;

            try
            {
                Request.CommandText = "select * " +
                    "from user " +
                    "where username = @v_username or email = @v_username";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_username", DbType.String, username));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows && Reader.Read())
                {
                    var uPwd = Reader["passwd"].ToString();
                    var uSalt = Reader["m_salt"].ToString();

                    if (PasswordStorage.VerifyPassword(passwd, uSalt, uPwd))
                        _user = Map(Reader);
                }

                Reader.Close();

                if (_user != null)
                    user = Create(_user, true);
            }
            catch (Exception)
            {
            }

            return user;
        }

        public User GetUser(string id)
        {
            User user = null;
            Dictionary<string, object> _user = null;

            try
            {
                Request.CommandText = "select * " +
                    "from user " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _user = Map(Reader);

                Reader.Close();

                if (_user != null)
                {
                    user = Create(_user, true);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return user;
        }

        public User Create(Dictionary<string, object> row, bool withEntite)
        {
            var user = new User()
            {
                Id = row["id"].ToString(),
                Nom = row["nom"].ToString(),
                Prenom = row["prenom"].ToString(),
                UserName = row["username"].ToString(),
                Sex = DbUtil.ToSex(row["sexe"].ToString()),
                Email = row["email"].ToString(),
                Telephone = row["telephone"].ToString(),
                Type = DbUtil.ToUserType(row["type"].ToString()),
                Etat = bool.Parse(row["etat"].ToString()) ? UserState.Fonctionnel : UserState.Bloqué
            };

            if (withEntite)
                user.Entite = new EntiteDao().Get(row["entite_id"].ToString());
            
            return user;
        }

        public override int Delete(User user)
        {
            try
            {
                Request.CommandText = "delete from user " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, user.Id));

                return Request.ExecuteNonQuery();

            }
            catch (Exception)
            {
                return -1;
            }

        }

        public int Lock(User user)
        {
            try
            {
                Request.CommandText = "update user " +
                    "set etat = @v_etat " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, user.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_etat", DbType.Boolean, 0));


                var id = Request.ExecuteNonQuery();

                if (id > 0)
                    user.Etat = UserState.Bloqué;

                return id;

            }
            catch (Exception)
            {
                return -1;
            }

        }

        public int Unlock(User user)
        {
            try
            {
                Request.CommandText = "update user " +
                    "set etat = @v_etat " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, user.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_etat", DbType.Boolean, 1));


                var id = Request.ExecuteNonQuery();

                if (id > 0)
                    user.Etat = UserState.Fonctionnel;

                return id;

            }
            catch (Exception)
            {
                return -1;
            }

        }

        public int SetPasswd(User user, string newPasswd)
        {
            try
            {
                var hash = PasswordStorage.CreateHash(newPasswd);
                var split = hash.Split(':');
                var salt = split[0];
                var pwd = string.Format("{0}:{1}", split[1], split[2]);

                Request.CommandText = "update user " +
                    "set passwd = @v_passwd," +
                    "m_salt = @v_m_salt " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_passwd", DbType.String, pwd));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_m_salt", DbType.String, salt));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, user.Id));

                int feedback = Request.ExecuteNonQuery();

                if (feedback > 0)
                    user.PassWd = newPasswd;

                return feedback;

            }
            catch (Exception)
            {
                return -1;
            }

        }

        public bool CheckPassword(User user, string passwd)
        {
            try
            {
                Request.CommandText = "select passwd, m_salt " +
                    "from user " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.Int32, user.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                {
                    var uPwd = Reader["passwd"].ToString();
                    var uSalt = Reader["m_salt"].ToString();

                    Reader.Close();

                    return PasswordStorage.VerifyPassword(passwd, uSalt, uPwd);
                }

                Reader.Close();
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return false;
        }

        public bool IsCurrentPasswd(User user, string passwd)
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from user " +
                    "where id = @v_id and passwd = password(@v_passwd) ";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.Int32, user.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_passwd", DbType.String, passwd));

                return Convert.ToInt32(Request.ExecuteScalar()) > 0;

            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<string> GetAllUsernames()
        {
            var usernames = new List<string>();

            try
            {
                Request.CommandText = "select distinct username " +
                    "from user " +
                    "order by username";

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        usernames.Add(Reader["username"].ToString());
                    }
                }

                Reader.Close();
            }
            catch (Exception)
            {

            }

            return usernames;
        }

        public async Task<List<string>> GetAllUsernamesAsync()
        {
            var usernames = new List<string>();

            try
            {
                Request.CommandText = "select distinct username " +
                    "from user " +
                    "order by username";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        usernames.Add(Reader["username"].ToString());
                    }
                }

                Reader.Close();
            }
            catch (Exception)
            {
            }

            return usernames;
        }

        public List<string> GetAllEmails()
        {
            List<string> emails = new List<string>();

            try
            {
                Request.CommandText = "select email " +
                    "from user " +
                    "order by email";

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        emails.Add(Reader["email"].ToString());
                    }
                }

                Reader.Close();

            }
            catch (Exception)
            {
            }

            return emails;
        }

        public List<User> GetUsers()
        {
            var users = new List<User>();
            var _users = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from user " +
                    "order by nom, prenom";

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        _users.Add(Map(Reader));
                    }
                }

                Reader.Close();

                foreach (var item in _users)
                {
                    users.Add(Create(item, true));
                }

            }
            catch (Exception)
            {
                if (Reader != null)
                    Reader.Close();

                return null;
            }

            return users;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var users = new List<User>();
            var _users = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from user " +
                    "order by nom, prenom";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        _users.Add(Map(Reader));
                    }
                }

                Reader.Close();

                foreach (var item in _users)
                {
                    users.Add(Create(item, true));
                }

            }
            catch (Exception)
            {
                if (Reader != null)
                    Reader.Close();

                return null;
            }

            return users;
        }

        public async Task<List<User>> GetAllAsync(Entite entite, DateTime lastUpdateTime)
        {
            var intances = new List<User>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from user " +
                    "where (@v_siege = 1 or entite_id = @v_entite_id) and last_update_time >= @v_time ";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_siege", DbType.Int32, entite.EstPrincipale ? 1 : 0));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_time", DbType.DateTime, lastUpdateTime));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var user = Create(item, true);
                    intances.Add(user);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task GetUsersAsync(Entite entite, ObservableCollection<User> collection)
        {
            var _users = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from user " +
                    "where @v_siege = 1 or entite_id = @v_entite_id " +
                    "order by nom, prenom";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_siege", DbType.Int32, entite.EstPrincipale ? 1 : 0));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entite_id", DbType.String, entite.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        _users.Add(Map(Reader));
                    }
                }

                Reader.Close();

                foreach (var item in _users)
                    collection.Add(Create(item, true));

            }
            catch (Exception)
            {
                if (Reader != null)
                    Reader.Close();
            }

        }
        
        public async Task<List<string>> GetAllEmailsAsync()
        {
            List<string> emails = new List<string>();

            try
            {
                Request.CommandText = "select email " +
                    "from user " +
                    "order by email";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        emails.Add(Reader["email"].ToString());
                    }
                }

                Reader.Close();

            }
            catch (Exception)
            {
            }

            return emails;
        }

        protected override Dictionary<string, object> Map(DbDataReader row)
        {
            return new Dictionary<string, object>()
            {
                { "id", row["id"] },
                { "nom", row["nom"] },
                { "prenom", row["prenom"] },
                { "sexe", row["sexe"] },
                { "username", row["username"] },
                { "email", row["email"] },
                { "type", row["type"] },
                { "telephone", row["telephone"] },
                { "etat", row["etat"] },
                { "entite_id", row["entite_id"] }
            };
        }
    }
}
