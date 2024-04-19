using FingerPrintManagerApp.Model;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace FingerPrintManagerApp.Dao.Helper
{
    public class TableKeyHelper
    {
        public static string GetKey(string tableName, bool dependsOnEntite = true)
        {
            try
            {
                var connection = ConnectionHelper.GetConnection();

                var request = connection.CreateCommand();

                request.CommandText = "get_entite_table_key";
                request.CommandType = CommandType.StoredProcedure;

                request.Parameters.Add(DbUtil.CreateParameter(request, "@v_entite_id", DbType.String, dependsOnEntite ? Model.AppConfig.EntiteId : string.Empty));
                request.Parameters.Add(DbUtil.CreateParameter(request, "@v_table_name", DbType.String, tableName));

                return request.ExecuteScalar().ToString();
                
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GenerateKey(string tableName, string add = "")
        {
            var user = AppConfig.CurrentUser;
            var random = new Random();
            var key = (user?.Id ?? "Herve") + tableName + DateTime.Now.ToUniversalTime().ToLongDateString() + " " + DateTime.Now.ToUniversalTime().ToLongTimeString() + add + random.Next(0, 10000);

            var md5 = MD5.Create();

            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(key));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString().ToUpper();
        }
    }
}
