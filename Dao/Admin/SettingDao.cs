using FingerPrintManagerApp.Model.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Admin
{
    public class SettingDao : Dao<Setting>
    {
        public override int Add(Setting param)
        {
            return 0;
        }

        public override int Update(Setting param, Setting old = null)
        {
            return 0;
        }

        public override int Delete(Setting param)
        {
            return 0;
        }

        public Setting GetSetting(string key)
        {
            Setting setting = null;

            try
            {
                Request.CommandText = "select * " +
                    "from finger_print_manager_db_info " +
                    "where param = @v_key";

                Request.Parameters.Clear();

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_key", DbType.String, key));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                {
                    setting = new Setting()
                    {
                        Key = key,
                        Value = Reader["valeur"]
                    };
                }

                Reader.Close();

                return setting;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Setting> GetSettingAsync(string key)
        {
            Setting setting = null;

            try
            {
                Request.CommandText = "select * " +
                    "from arg_store_db_info " +
                    "where param = @v_key";

                Request.Parameters.Clear();

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_key", DbType.String, key));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows && Reader.Read())
                {
                    setting = new Setting()
                    {
                        Key = key,
                        Value = Reader["valeur"]
                    };
                }

                Reader.Close();

                return setting;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int SetSetting(string key, string value)
        {
            try
            {
                Request.Parameters.Clear();

                Request.CommandText = "update arg_store_db_info " +
                    "set valeur = @v_value " +
                    "where param = @v_key";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_key", DbType.String, key));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_value", DbType.String, value));

                return Request.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return -1;
            }
        }

        protected override Dictionary<string, object> Map(DbDataReader row)
        {
            return null;
        }
    }
}
