using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace FingerPrintManagerApp.Dao
{
    public class DbUtil
    {
        public static bool IsConnectionActive()
        {
            using (var connection = DbProviderFactories.GetFactory(DbConfig.Providers[DbConfig.DbInvariant]).CreateConnection())
            {
                var connectionString = string.Format("server={0};user={1};password={2};database={3};port={4}",
                    DbConfig.ServerName,
                    DbConfig.DbUser,
                    DbConfig.DbPassword,
                    DbConfig.DbUser,
                    DbConfig.DbPassword);

                connection.ConnectionString = connectionString;

                try
                {
                    connection.Open();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static DbParameter CreateParameter(DbCommand cmd, string paramName, DbType type, object value, ParameterDirection direction = ParameterDirection.Input)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.DbType = type;
            param.Value = value;
            param.Direction = direction;

            return param;
        }

        public static UserState ToUserState(string state)
        {
            switch (state)
            {
                case "Bloqué":
                    return UserState.Bloqué;

                case "Fonctionnel":
                    return UserState.Fonctionnel;

                default:
                    return UserState.Fonctionnel;
            }
        }

        public static UserType ToUserType(string type)
        {
            switch (type)
            {
                case "ADMIN":
                    return UserType.ADMIN;

                case "USER":
                    return UserType.USER;

                default:
                    return UserType.USER;
            }
        }

        public static Sex ToSex(string sex)
        {
            switch (sex)
            {
                case "Homme":
                    return Sex.Homme;
                case "Femme":
                    return Sex.Femme;

                default:
                    return Sex.Homme;
            }
        }

        public static TypeZone ToZoneType(string type)
        {
            switch (type)
            {
                case "Ville":
                    return TypeZone.Ville;

                case "Territoire":
                    return TypeZone.Territoire;

                default:
                    return TypeZone.Ville;
            }
        }

        public static TypeCommune ToCommuneType(string type)
        {
            switch (type)
            {
                case "Commune":
                    return TypeCommune.Commune;

                case "Chefférie":
                    return TypeCommune.Chefférie;

                case "Secteur":
                    return TypeCommune.Secteur;

                default:
                    return TypeCommune.Commune;
            }
        }
        public static DataTable DicToTable(List<Dictionary<string, object>> list)
        {
            DataTable result = new DataTable();
            if (list.Count == 0)
                return result;

            foreach (var entry in list[0])
                result.Columns.Add(new DataColumn(entry.Key, entry.Value.GetType()));

            foreach (var dic in list)
            {
                var row = new object[dic.Count];

                int k = 0;
                foreach (var entry in dic)
                    row[k++] = entry.Value;

                result.Rows.Add(row);
            }

            return result;
        }

        public static DataTable DicToTable(List<Dictionary<string, object>> data, List<Type> types)
        {
            DataTable result = new DataTable();
            if (data.Count == 0)
                return result;

            int i = 0;
            foreach (var entry in data[0])
            {
                var type = types[i];
                result.Columns.Add(new DataColumn(entry.Key, type));

                i++;
            }


            foreach (var dic in data)
            {
                var row = new object[dic.Count];

                int k = 0;
                foreach (var entry in dic)
                    row[k++] = entry.Value is DBNull ? null : entry.Value;

                result.Rows.Add(row);
            }

            return result;
        }

        public static string DateToEnglishDateFormat(DateTime date)
        {
            return date.Year + "-" + date.Month + "-" + date.Day;
        }

        public static string DateToEnglishTimeFormat(DateTime date)
        {
            return date.Year + "-" + date.Month + "-" + date.Day + " " + date.Hour + ":" + date.Minute + ":" + date.Second;
        }
        
    }
}
