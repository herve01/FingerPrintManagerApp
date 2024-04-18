using FingerPrintManagerApp.Model.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao.Admin
{
    public class EntryLogDao : Dao<EntryLog>
    {
        public EntryLogDao()
        {
            TableName = "entry_log";
        }

        public override int Add(EntryLog entry)
        {
            try
            {
                var id = Helper.TableKeyHelper.GetKey(TableName);

                Request.CommandText = "insert into entry_log(id, user_id, event, entry_date, entity, ip_address, machine_name) " +
                    "values(@v_id, @v_user_id, @v_event, @v_entry_date, @v_entity, @v_ip_address, @v_machine_name)";

                Request.Parameters.Clear();

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_user_id", DbType.String, entry.User.Id));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_event", DbType.String, entry.Event));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entry_date", DbType.DateTime, entry.EntryDate));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_entity", DbType.String, entry.Entity));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_ip_address", DbType.String, entry.IPAddress));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_machine_name", DbType.String, entry.MachineName));

                var feed = Request.ExecuteNonQuery();

                if (feed > 0)
                    entry.EntryId = id;

                return feed;
            }
            catch(Exception)
            {
                return -1;
            }
        }

        public override int Update(EntryLog entry, EntryLog old)
        {
            return 0;
        }

        public override int Delete(EntryLog entry)
        {
            return 0;
        }

        public async Task GetLogEntriesAsync(ObservableCollection<EntryLog> collection, DateTime fromDate, DateTime toDate)
        {
            var _entrys = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from entry_log " +
                    "where date(entry_date) >= @v_from and date(entry_date) <= @v_to " +
                    "order by entry_date desc";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_from", DbType.DateTime, fromDate));
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_to", DbType.DateTime, toDate));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while(Reader.Read())
                        _entrys.Add(Map(Reader));

                Reader.Close();

                foreach (var row in _entrys)
                    collection.Add(CreateEntryLog(row));
            }
            catch (Exception)
            {
                if (Reader != null)
                    Reader.Close();
            }

        }

        private EntryLog CreateEntryLog(Dictionary<string, object> row)
        {
            var entry = new EntryLog()
            {
                EntryId = row["id"].ToString(),
                Event = row["event"].ToString(),
                EntryDate = DateTime.Parse(row["entry_date"].ToString()),
                Entity = row["entity"].ToString(),
                IPAddress = row["ip_address"].ToString(),
                MachineName = row["machine_name"].ToString(),
                User = new UserDao().GetUser(row["user_id"].ToString())
            };

            return entry;
        }

        protected override Dictionary<string, object> Map(DbDataReader row)
        {
            return new Dictionary<string, object>()
            {
                { "id", row["id"] },
                { "event", row["event"] },
                { "entry_date", row["entry_date"] },
                { "entity", row["entity"] },
                { "user_id", row["user_id"] },
                { "ip_address", row["ip_address"] },
                { "machine_name", row["machine_name"] }
            };
        }
    }
}
