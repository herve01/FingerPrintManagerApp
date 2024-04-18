using FingerPrintManagerApp.Model;
using FingerPrintManagerApp.Model.Employe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao
{
    public class ZoneDao : Dao<Zone>
    {
        public ZoneDao()
        {
            TableName = "zone";
        }

        public override int Add(Zone instance)
        {
            return 0;
        }

        public int Add(DbCommand command, Zone zone)
        {
            Request = command;
            Request.Parameters.Clear();

            OwnAction = false;

            return Add(zone);
        }


        public override int Update(Zone instance, Zone old)
        {
            return 0;
        }

        public override int Delete(Zone instance)
        {
            return 0;
        }

        private Zone Create(Dictionary<string, object> row, bool withProvince, bool withCommunes)
        {
            var instance = new Zone();

            instance.Id = int.Parse(row["id"].ToString());
            instance.Nom = row["nom"].ToString();
            instance.Type = DbUtil.ToZoneType(row["type"].ToString());

            if (withProvince)
                instance.Province = new ProvinceDao().Get(Convert.ToInt32(row["province_id"]));

            if (withCommunes)
                instance.Communes = new CommuneDao().GetCommunesByZone(instance);

            return instance;

        }

        public int Count()
        {
            try
            {
                Request.CommandText = "select count(*) " +
                    "from zone";

                return int.Parse(Request.ExecuteScalar().ToString());

            }
            catch (Exception)
            {
            }

            return 0;
        }

        public Zone Get(int id)
        {
            Zone zone = null;
            Dictionary<string, object> _zone = null;

            try
            {
                Request.CommandText = "select * " +
                    "from zone " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.Int32, id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _zone = Map(Reader);

                Reader.Close();

                if (_zone != null)
                    zone = Create(_zone, true, false);

            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return zone;
        }

        public async Task<List<Zone>> GetAllAsync()
        {
            var zonees = new List<Zone>();
            var _zonees = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from zone";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _zonees.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _zonees)
                {
                    Zone zone = Create(item, true, false);
                    zonees.Add(zone);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

            return zonees;
        }

        public async Task GetAllAsync(ObservableCollection<Zone> collection)
        {
            var _zonees = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from zone ";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _zonees.Add(Map(Reader));

                Reader.Close();

                foreach (var item in _zonees)
                {
                    Zone zone = Create(item, true, false);
                    collection.Add(zone);
                }
            }
            catch (Exception)
            {
                if (Reader != null && !Reader.IsClosed)
                    Reader.Close();
            }

        }

        public List<Zone> GetZonesByProvince(Province province)
        {
            var instances = new List<Zone>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from zone " +
                    "where province_id = @v_id " +
                    "order by nom";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, province.Id));

                Reader = Request.ExecuteReader();

                if (Reader != null && Reader.HasRows)
                {
                    while (Reader.Read())
                        _instances.Add(Map(Reader));
                }

                Reader.Close();

                foreach (var row in _instances)
                {
                    var instance = Create(row, false, true);
                    instance.Province = province;
                    instances.Add(instance);
                }

            }
            catch (Exception)
            {
                if (Reader != null)
                    Reader.Close();
            }

            return instances;
        }

        public List<Zone> GetZonesByProvinceAsync(Province province, bool withCommunes = false)
        {
            var instances = new List<Zone>();
            var _instances = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from zone " +
                    "where province_id = @v_id " +
                    "order by nom";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, province.Id));

                Reader = Request.ExecuteReader();

                if (Reader != null && Reader.HasRows)
                {
                    while (Reader.Read())
                        _instances.Add(Map(Reader));
                }

                Reader.Close();

                foreach (var row in _instances)
                {
                    var instance = Create(row, false, withCommunes);
                    instance.Province = province;
                    instances.Add(instance);
                }

            }
            catch (Exception)
            {
                if (Reader != null)
                    Reader.Close();
            }

            return instances;
        }

        protected override Dictionary<string, object> Map(DbDataReader reader)
        {
            return new Dictionary<string, object>()
            {
                { "id", reader["id"] },
                { "nom", reader["nom"] },
                { "type", reader["type"] },
                { "province_id", reader["province_id"] },
                { "adding_date", reader["adding_date"] }
            };
        }
    }
}


