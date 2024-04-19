using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Presence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;


namespace FingerPrintManagerApp.Dao.Presence
{
    public class DateExceptionDao : Dao<DateException>
    {
        public DateExceptionDao(DbConnection connection = null) : base(connection)
        {
            TableName = "date_exception";
        }
        public override int Add(DateException instance)
        {
            return 0;
        }

        public override int Update(DateException instance, DateException old = null)
        {
            return 0;
        }

        public override int Delete(DateException instance)
        {
            return 0;
        }

        private DateException Create(Dictionary<string, object> row)
        {
            var instance = new DateException();

            instance.Id = row["id"].ToString();
            instance.Description = row["description"].ToString();
            instance.Date = DateTime.Parse(row["date"].ToString());
           
            return instance;

        }

        public DateException Get(string id)
        {
            DateException instance = null;
            Dictionary<string, object> _instances = null;

            try
            {
                Request.CommandText = "select * " +
                    "from date_exception " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _instances = Map(Reader);

                Reader.Close();

                if (_instances != null)
                    instance = Create(_instances);

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return instance;
        }

        public async Task<List<DateException>> GetAllAsync()
        {
            var intances = new List<DateException>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from date_exception ";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var instance = Create(item);
                    intances.Add(instance);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task<List<DateException>> GetAllAsync(DateTime lastUpdateTime)
        {
            var intances = new List<DateException>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from date_exception " +
                    "where created_at >= @v_time or updated_at >= @v_time";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_time", DbType.DateTime, lastUpdateTime));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    var date_exception = Create(item);
                    intances.Add(date_exception);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return intances;
        }

        public async Task GetAllAsync(ObservableCollection<DateException> collection)
        {
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from date_exception";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _instances.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _instances)
                {
                    DateException date_exception = Create(item);
                    collection.Add(date_exception);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

        }

        protected override Dictionary<string, object> Map(DbDataReader reader)
        {
            return new Dictionary<string, object>()
            {
                { "id", reader["id"] },
                { "description", reader["description"] },
                { "date", reader["date"] },
            };
        }
    }
}
