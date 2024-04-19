using FingerPrintManagerApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao
{
    public class ContinentDao : Dao<Continent>
    {
        public ContinentDao(DbConnection connection = null) : base(connection)
        {
        }

        public override int Add(Continent continent)
        {
            return 0;
        }

        public override int Update(Continent continent, Continent old)
        {
            return Add(continent);
        }

        public override int Delete(Continent obj)
        {
            return 0;
        }

        protected override Dictionary<string, object> Map(DbDataReader row)
        {
            return new Dictionary<string, object>()
            {
                { "id", row["id"] },
                { "nom", row["nom"] }
            };
        }
        
        private Continent Create(Dictionary<string, object> row, bool withPays = false)
        {
            var continent = new Continent()
            {
                Id = int.Parse(row["id"].ToString()),
                Nom = row["nom"].ToString(),
            };

            if(withPays)
                continent.Pays = new PaysDao().GetPays(int.Parse(row["id"].ToString()));

                       
            return continent;
        }

        public Continent GetContinent(int id)
        {
            Continent continent = null;
            Dictionary<string, object> _continent = null;

            try
            {
                Request.CommandText = "select * " +
                    "from continent " +
                    "where id = @v_id";
                
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.Int32, id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _continent = Map(Reader);

                Reader.Close();

                if (_continent != null)
                    continent = Create(_continent);
            }
            catch (Exception)
            {
            }

            return continent;
        }

        public List<Continent> GetContinents()
        {
            var continents = new List<Continent>();
            var _continents = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from continent " +
                    "order by nom desc";

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _continents.Add(Map(Reader));

                Reader.Close();

                foreach (var row in _continents)
                    continents.Add(Create(row));
                
            }
            catch (Exception)
            {
            }

            return continents;
        }

        public async Task<List<Continent>> GetContinentsAsync()
        {
            var continents = new List<Continent>();
            var _continents = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from continent " +
                    "order by nom desc";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _continents.Add(Map(Reader));

                Reader.Close();

                foreach (var row in _continents)
                    continents.Add(Create(row));

            }
            catch (Exception)
            {
            }

            return continents;
        }

        public async Task GetContinentsAsync(ObservableCollection<Continent> collection)
        {
            var _continents = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from continent " +
                    "order by nom desc";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _continents.Add(Map(Reader));

                Reader.Close();

                foreach (var row in _continents)
                    collection.Add(Create(row));

            }
            catch (Exception)
            {
            }
            
        }
    }
}
