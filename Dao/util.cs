using FingerPrintManagerApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerPrintManagerApp.Dao
{
    public class util
    {
        public static TypeZone ToTypeZone(string value)
        {

            switch (value)
            {
                case "Ville":
                    return TypeZone.Ville;

                case "Territoire":
                    return TypeZone.Territoire;

                default:
                    return TypeZone.Ville;
            }
        }
    }
}
