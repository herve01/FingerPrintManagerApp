using System;
using System.Data;

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
    }
}
