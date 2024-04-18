using System.Collections.Generic;

namespace FingerPrintManagerApp.Dao
{
    public class DbConfig
    {
        static SortedList<string, string> _providers = null;
        public static SortedList<string, string> Providers
        {
            get
            {
                if (_providers == null)
                {
                    _providers = new SortedList<string, string>();
                    _providers.Add("SQL Server", "System.Data.SqlClient");
                    _providers.Add("MySQL", "MySql.Data.MySqlClient");
                    _providers.Add("Oracle Client", "System.Data.OleDb");
                }

                return _providers;
            }
        }

        static SortedList<string, string> _autoIncrementFunctions = null;
        public static SortedList<string, string> AutoIncrementFunctions
        {
            get
            {
                if (_autoIncrementFunctions == null)
                {
                    _autoIncrementFunctions = new SortedList<string, string>();
                    _autoIncrementFunctions.Add("SQL Server", "System.Data.SqlClient");
                    _autoIncrementFunctions.Add("MySQL", "last_insert_id()");
                    _autoIncrementFunctions.Add("Oracle Client", "System.Data.OleDb");
                }

                return _autoIncrementFunctions;
            }
        }

        public static string DbName { get; set; }
        public static string DbInvariant { get; set; }
        public static string DbPort { get; set; }
        public static string DbUser { get; set; }
        public static string DbPassword { get; set; }
        public static string ServerName { get; set; }

    }
}
