using FingerPrintManagerApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao
{
    public class PaysDao : Dao<Pays>
    {
        public PaysDao(DbConnection connection = null) : base(connection)
        {
        }

        public override int Add(Pays pays)
        {
            return 0;
        }

        public override int Update(Pays pays, Pays old)
        {
            return Add(pays);
        }

        public override int Delete(Pays obj)
        {
            return 0;
        }

        protected override Dictionary<string, object> Map(DbDataReader row)
        {
            return new Dictionary<string, object>()
            {
                { "id", row["id"] },
                { "continent_id", row["continent_id"] },
                { "french_name", row["french_name"] },
                { "english_name", row["english_name"] },
                { "surface", row["surface"] },
                { "code_2", row["code_2"] },
                { "svg_map", row["svg_map"] },
                { "svg_map_view_port", row["svg_map_view_port"] }
            };
        }
        
        private Pays Create(Dictionary<string, object> row, bool withContinent)
        {
            var pays = new Pays()
            {
                Id = row["id"].ToString(),
                FrenchName = row["french_name"].ToString(),
                EnglishName = row["english_name"].ToString(),
                Surface = float.Parse(row["surface"].ToString()),
                Code2 = row["code_2"].ToString(),
                SvgMag = row["svg_map"].ToString(),
                SvgMagViewPort = row["svg_map_view_port"].ToString()
            };

            if (withContinent)
                pays.Continent = new ContinentDao().GetContinent(int.Parse(row["continent_id"].ToString()));
                
            
            return pays;
        }

        public Pays GetPays(string id)
        {
            Pays pays = null;
            Dictionary<string, object> _pays = null;

            try
            {
                Request.CommandText = "select * " +
                    "from pays " +
                    "where id = @v_id";
                
                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_id", DbType.String, id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows && Reader.Read())
                    _pays = Map(Reader);

                Reader.Close();

                if (_pays != null)
                    pays = Create(_pays, true);
            }
            catch (Exception)
            {
            }

            return pays;
        }

        public List<Pays> GetPays()
        {
            var pays = new List<Pays>();
            var _pays = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from pays " +
                    "order by french_name desc";

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _pays.Add(Map(Reader));

                Reader.Close();

                foreach (var row in _pays)
                    pays.Add(Create(row, false));
                
            }
            catch (Exception)
            {
            }

            return pays;
        }

        public List<Pays> GetPays(int id)
        {
            var pays = new List<Pays>();
            var _pays = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from pays " +
                    "where continent_id = @v_continent_id " +
                    "order by french_name desc";

                Request.Parameters.Add(DbUtil.CreateParameter(Request, "@v_continent_id", DbType.String, id));

                Reader = Request.ExecuteReader();

                if (Reader.HasRows)
                    while (Reader.Read())
                        _pays.Add(Map(Reader));

                Reader.Close();

                foreach (var row in _pays)
                    pays.Add(Create(row, false));

            }
            catch (Exception)
            {
            }

            return pays;
        }

        public async Task<List<Pays>> GetAllAsync()
        {
            var pays = new List<Pays>();
            var _pays = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from pays " +
                    "order by french_name desc";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _pays.Add(Map(Reader));

                Reader.Close();

                foreach (var row in _pays)
                    pays.Add(Create(row, true));

            }
            catch (Exception)
            {
            }

            return pays;
        }

        public async Task GetAllAsync(ObservableCollection<Pays> collection)
        {
            var _pays = new List<Dictionary<string, object>>();

            try
            {
                Request.CommandText = "select * " +
                    "from pays " +
                    "order by french_name desc";

                Reader = await Request.ExecuteReaderAsync();

                if (Reader.HasRows)
                    while (await Reader.ReadAsync())
                        _pays.Add(Map(Reader));

                Reader.Close();

                foreach (var row in _pays)
                    collection.Add(Create(row, false));

            }
            catch (Exception)
            {
            }
            
        }
    }
}
