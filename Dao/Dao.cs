﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace FingerPrintManagerApp.Dao
{
    public abstract class Dao<T>
    {
        public abstract int Add(T obj);
        public abstract int Delete(T obj);
        public abstract int Update(T newObj, T oldObj);

        protected abstract Dictionary<string, object> Map(DbDataReader row);

        protected DbConnection Connection;
        protected DbCommand Request;
        protected DbDataAdapter Adapter;
        protected DbDataReader Reader;
        protected DataTable Table;
        protected bool OwnAction = true;
        protected string AutoIncrementFunction = string.Empty;
        protected string TableName = string.Empty;

        public Dao()
        {
            try
            {
                Connection = ConnectionHelper.GetConnection();
                Request = Connection.CreateCommand();
                AutoIncrementFunction = DbConfig.AutoIncrementFunctions[DbConfig.DbInvariant];
                Adapter = DbProviderFactories.GetFactory(DbConfig.Providers[DbConfig.DbInvariant]).CreateDataAdapter();
                Table = new DataTable();

                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();
            }
            catch (Exception)
            {

            }
        }
    }
}
