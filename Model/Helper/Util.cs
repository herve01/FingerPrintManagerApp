using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace FingerPrintManagerApp.Model.Helper
{
    public class Util
    {
        private static int RandomStep = 0;

        public static string RandomSerieNumber()
        {
            var serie = DateTime.Now + "" + ++RandomStep;

            MD5 md5Hash = MD5.Create();

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(serie));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            return sBuilder.ToString().ToUpper();
        }

        public static string ToMonthName(int month)
        {
            var months = new SortedList<int, string>();

            months.Add(1, "Janvier");
            months.Add(2, "Février");
            months.Add(3, "Mars");
            months.Add(4, "Avril");
            months.Add(5, "Mai");
            months.Add(6, "Juin");
            months.Add(7, "Juillet");
            months.Add(8, "Août");
            months.Add(9, "Septembre");
            months.Add(10, "Octobre");
            months.Add(11, "Novembre");
            months.Add(12, "Décembre");

            if (month <= months.Count)
            {
                return months[month];
            }

            return string.Empty;
        }

    }
}
