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

                if (string.IsNullOrWhiteSpace(DbConfig.DbUser) || string.IsNullOrWhiteSpace(DbConfig.DbPassword) ||
                    string.IsNullOrWhiteSpace(DbConfig.DbName) || string.IsNullOrWhiteSpace(DbConfig.DbPort) ||
                    string.IsNullOrWhiteSpace(DbConfig.ServerName))
                    return null;

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
                catch (Exception)
                {
                    _connection = null;
                }
            }

            return _connection;
        }

        public static void DeleteConnection()
        {
            try
            {
                if (_connection != null)
                    _connection.Close();

                _connection = null;
            }
            catch (Exception)
            {
            }
        }

        public static DbConnection GetNewInstance()
        {
            if (string.IsNullOrWhiteSpace(DbConfig.DbUser) || string.IsNullOrWhiteSpace(DbConfig.DbPassword) ||
                    string.IsNullOrWhiteSpace(DbConfig.DbName) || string.IsNullOrWhiteSpace(DbConfig.DbPort) ||
                    string.IsNullOrWhiteSpace(DbConfig.ServerName))
                return null;

            var connectionString = string.Format("server={0};user={1};password={2};database={3};port={4}",
                    DbConfig.ServerName,
                    DbConfig.DbUser,
                    DbConfig.DbPassword,
                    DbConfig.DbName,
                    DbConfig.DbPort);

            var invariant = DbConfig.Providers[DbConfig.DbInvariant];

            try
            {
                var connection = DbProviderFactories.GetFactory(invariant).CreateConnection();
                connection.ConnectionString = connectionString;
                connection.Open();

                return connection;

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
