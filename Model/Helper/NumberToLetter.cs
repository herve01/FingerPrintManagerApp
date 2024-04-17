using System.Collections.Generic;

namespace FingerPrintManagerApp.Model.Helper
{
    public class NumberToLetter
    {
        public long Nombre { get; set; }
        SortedList<int, TreeMilliarld> milliards;
        List<string> milMultis = new List<string>() { "", "mille", "million" };
        string[] unites = { "zéro", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf" };
        string[] dizaines = { "", "dix", "vingt", "trente", "quarante", "cinquante", "soixante", "septante", "quatre-vingt", "nonante" };

        public NumberToLetter(long nombre)
        {
            milliards = new SortedList<int, TreeMilliarld>();
            Nombre = nombre;
            InitBlocs();
        }

        public string GetLetter()
        {
            string lettre = "";

            int k = 0;

            foreach (var item in milliards)
            {
                string letter = "";

                int mil = (item.Value.Nombre.ToString().Length - 1) / 3;

                for (int i = 0; i <= mil; i++)
                {
                    // Ajouter le mot milliard
                    letter = (i == 0 ? " " + (item.Value.Blocs["0"].Nombre == 0 ? (item.Value.Blocs.Count == 1 ? item.Value.Blocs["0"].Lettre : "") : item.Value.Blocs["0"].Lettre) : TreateMille(item.Value.Blocs[i + ""])) + letter;
                    letter = i == 0 ? letter.Trim() : letter;
                }

                letter += (k > 0 ? (item.Value.Nombre > 1 ? " milliards " : " milliard ") : "");

                lettre = letter + lettre;

                k++;
            }


            lettre = lettre.Replace("  ", " ");
            return lettre.Trim();
        }

        string TreateMille(TreeN tr)
        {
            string mil = "";
            mil = tr.Nombre == 0 ? "" : tr.Nombre == 1 ? (tr.MilMulti == "mille" ? "mille " : "un " + tr.MilMulti + " ") : GetLetter(tr.Nombre) + " " + tr.MilMulti + (tr.MilMulti == "mille" ? " " : "s ");

            return mil;
        }

        string GetLetter(int nombre)
        {
            string letter = "";

            int unit = nombre % 10;
            int dix = nombre > 9 ? ((nombre / 10) % 10) : 0;
            int cent = nombre > 99 ? (nombre / 100) : 0;

            if (dix == 0)
            {
                if (unit == 0)
                {
                    letter = cent == 0 ? unites[nombre] + "" : "";
                }
                else
                {
                    letter = unites[unit];
                }
            }
            else if (dix == 1)
            {
                letter = unites[dix * 10 + unit] + "";
            }
            else if (dix == 7 || dix == 9)
            {
                letter = dizaines[dix - 1] + (unit == 1 && dix == 7 ? "-et-" : "-") + unites[10 + unit];
            }
            else
            {
                letter += dizaines[dix];

                if (unit == 1)
                {
                    letter += dix == 8 ? "-un" : "-et-un";
                }
                else if (unit == 0)
                {
                    letter += (dix == 8 ? "s" : "");
                }
                else
                {
                    letter += "-" + unites[unit];
                }
            }

            letter = (cent > 0 ? (unit == 0 && dix == 0 ? (cent == 1 ? "cent " : unites[cent] + " cents ") : (cent == 1 ? "cent " : unites[cent] + " cent ")) : "") + letter;

            return letter.Trim();
        }

        void InitBlocs()
        {
            string nbre = Nombre + "";

            int j = 0, k = 0, l = 0, m = 0;

            string blocS = "";
            string milliard = "";

            SortedList<string, TreeN> blocs = new SortedList<string, TreeN>();

            for (int i = nbre.Length - 1; i >= 0; i--)
            {

                blocS = nbre[i] + blocS;
                k++;

                if (m == 9)
                {
                    m = 0;
                    j = 0;
                    TreeMilliarld tree = new TreeMilliarld();
                    tree.Blocs = blocs;
                    tree.Nombre = long.Parse(milliard);
                    tree.Position = l;

                    milliards.Add(l, tree);

                    milliard = "";

                    blocs = new SortedList<string, TreeN>();

                    l++;
                }

                m++;

                if (k == 3 || i == 0)
                {
                    k = 0;

                    TreeN tree = new TreeN();
                    tree.Position = j;
                    tree.Nombre = int.Parse(blocS);
                    tree.Lettre = GetLetter(tree.Nombre);
                    tree.MilMulti = this.milMultis[j];

                    blocs.Add(j + "", tree);

                    milliard = blocS + milliard;

                    blocS = "";

                    j++;
                }

                if (i == 0)
                {

                    TreeMilliarld tree = new TreeMilliarld();
                    tree.Blocs = blocs;
                    tree.Nombre = long.Parse(milliard);
                    tree.Position = l;

                    milliards.Add(l, tree);
                }

            }
        }
    }

    public struct TreeN
    {
        public int Nombre;
        public string Lettre;
        public int Position;
        public string MilMulti;
    }

    public struct TreeMilliarld
    {
        public int Position;
        public long Nombre;
        public SortedList<string, TreeN> Blocs;
    }
}
