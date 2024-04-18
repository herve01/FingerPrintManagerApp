using System;
using System.Data.Common;

namespace FingerPrintManagerApp.Dao
{
    public class ConnectionHelper
    {
        private static DbConnection _connection;

        public static DbConnection GetConnection()
        {
            if (_connection == null)
            {
                var connectionString = string.Format("server={0};user={1};password={2};database={3};port={4}",
                    DbConfig.ServerName,
                    DbConfig.DbUser,
                    DbConfig.DbPassword,
                    DbConfig.DbName,
                    DbConfig.DbPort);

                try
                {
                    _connection = DbProviderFactories.GetFactory(DbConfig.Providers[DbConfig.DbInvariant]).CreateConnection();
                    _connection.ConnectionString = connectionString;
                    _connection.Open();

                }
                catch (Exception ex)
                {
                    _connection = null;
                }
            }

            return _connection;
        }
    }
}
