using FingerPrintManagerApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao
{
    public class ProvinceDao : Dao<Province>
    {
        public ProvinceDao()
        {
        }

        public override int Add(Province province)
        {
            return 0;
        }

        public override int Update(Province province, Province old)
        {
            return Add(province);
        }

        public override int Delete(Province obj)
        {
            return 0;
        }

        protected override Dictionary<string, object> Map(DbDataReader row)
        {
            return new Dictionary<string, object>()
            {
                { "id", row["id"] },
                { "nom", row["nom"] },
            };
        }

        private Province Create(Dictionary<string, object> row, bool withZones = false)
        {
            var province = new Province()
            {
                Id = int.Parse(row["id"].ToString()),
                Nom = row["nom"].ToString()
            };

            if (withZones)
                province.Zones = new ZoneDao().GetZonesByProvince(province);

            return province;
        }

        public Province Get(int id)
        {
            Province province = null;
            Dictionary<string, object> _province = null;

            try
            {
                Request.CommandText = "select * " +
                    "from province " +
                    "where id = @v_id";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.Int32, id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _province = Map(Reader);

                Reader.Close();

                if (_province != null)
                    province = Create(_province, false);
            }
            catch (Exception)
            {
            }

            return province;
        }

        public List<Province> GetAll(bool withZones = false)
        {
            var provinces = new List<Province>();
            var _provinces = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from province " +
                    "order by nom desc";

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _provinces.Add(Map(Reader));

                Reader.Close();

                foreach (var row in _provinces)
                    provinces.Add(Create(row, withZones));

            }
            catch (Exception)
            {
            }

            return provinces;
        }

        public async Task<List<Province>> GetAllAsync(bool withZones = false)
        {
            var provinces = new List<Province>();
            var _provinces = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from province " +
                    "order by nom desc";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _provinces.Add(Map(Reader));

                Reader.Close();

                foreach (var row in _provinces)
                    provinces.Add(Create(row, withZones));

            }
            catch (Exception)
            {
            }

            return provinces;
        }

        public async Task GetAllAsync(ObservableCollection<Province> collection, bool withZones = false)
        {
            var _provinces = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from province " +
                    "order by nom desc";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _provinces.Add(Map(Reader));

                Reader.Close();

                foreach (var row in _provinces)
                    collection.Add(Create(row, withZones));

            }
            catch (Exception)
            {
            }
        }

    }
}
