using FingerPrintManagerApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao
{
    public class CommuneDao : Dao<Commune>
    {
        public CommuneDao(DbConnection connection = null) : base(connection)
        {
        }

        public override int Add(Commune commune)
        {
            return 0;
        }

        public override int Update(Commune commune, Commune old)
        {
            return Add(commune);
        }

        public override int Delete(Commune obj)
        {
            return 0;
        }

        protected override Dictionary<string, object> Map(DbDataReader row)
        {
            return new Dictionary<string, object>()
            {
                { "id", row["id"] },
                { "nom", row["nom"] },
                { "type", row["type"] },
                { "zone_id", row["zone_id"] }
            };
        }
        
        private Commune Create(Dictionary<string, object> row, bool withZone)
        {
            var commune = new Commune()
            {
                Id = int.Parse(row["id"].ToString()),
                Nom = row["nom"].ToString(),
                Type = DbUtil.ToCommuneType(row["type"].ToString())
            };

            if (withZone)
                commune.Zone = new ZoneDao().Get(int.Parse(row["zone_id"].ToString()));
            
            return commune;
        }

        public Commune GetCommune(int id)
        {
            Commune commune = null;
            Dictionary<string, object> _commune = null;

            try
            {
                Request.CommandText = "select * " +
                    "from commune " +
                    "where id = @v_id";
                
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.Int32, id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _commune = Map(Reader);

                Reader.Close();

                if (_commune != null)
                    commune = Create(_commune, true);
            }
            catch (Exception)
            {
            }

            return commune;
        }

        public List<Commune> GetCommunes()
        {
            var communes = new List<Commune>();
            var _communes = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from commune " +
                    "order by nom desc";

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _communes.Add(Map(Reader));

                Reader.Close();

                foreach (var row in _communes)
                    communes.Add(Create(row, true));
                
            }
            catch (Exception)
            {
            }

            return communes;
        }

        public async Task<List<Commune>> GetCommunesAsync()
        {
            var communes = new List<Commune>();
            var _communes = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from commune " +
                    "order by nom desc";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _communes.Add(Map(Reader));

                Reader.Close();

                foreach (var row in _communes)
                    communes.Add(Create(row, true));

            }
            catch (Exception)
            {
            }

            return communes;
        }

        public async Task GetCommunesAsync(ObservableCollection<Commune> collection)
        {
            var _communes = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from commune " +
                    "order by nom desc";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _communes.Add(Map(Reader));

                Reader.Close();

                foreach (var row in _communes)
                    collection.Add(Create(row, true));

            }
            catch (Exception)
            {
            }
            
        }

        public List<Commune> GetCommunesByZone(Zone zone)
        {
            var communes = new List<Commune>();
            var _communes = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from commune " +
                    "where zone_id = @v_zone_id " +
                    "order by nom desc";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_zone_id", DbType.Int32, zone.Id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _communes.Add(Map(Reader));

                Reader.Close();

                foreach (var row in _communes)
                {
                    var commune = Create(row, false);
                    commune.Zone = zone;
                    communes.Add(commune);
                }
            }
            catch (Exception)
            {
            }

            return communes;
        }

        public async Task<List<Commune>> GetCommunesByZoneAsync(Zone zone)
        {
            var communes = new List<Commune>();
            var _communes = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from commune " +
                    "where zone_id = @v_zone_id " +
                    "order by nom desc";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_zone_id", DbType.Int32, zone.Id));

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _communes.Add(Map(Reader));

                Reader.Close();

                foreach (var row in _communes)
                {
                    var commune = Create(row, false);
                    commune.Zone = zone;
                    communes.Add(commune);
                }
            }
            catch (Exception)
            {
            }

            return communes;
        }

    }
}
