using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB
{
    public class IDToName
    {
        public static readonly Dictionary<int, string> idToNameMales = new Dictionary<int, string>
        {
            { 104925, "Novak Djokovic" },
            { 104918, "Andy Murray" },
            { 105683, "Milos Raonic"},
            { 105453, "Key Nishikori"},
            { 103819, "Roger Federer"},
            { 104792, "Gael Monfis"},
            { 104527, "Stan Wawrinka"},
            { 104745, "Rafael Nadal"},
            { 105227, "Marin Cilic"},
            { 106401, "Nick Kyrgos"},
            { 104542, "Tsonga"},
            { 106233, "Dominic Thiem"},
            { 104545, "John Isner"},
            { 105223, "Juan Martin Del Potro" },
            { 105676, "David Goffin"},
            { 106058, "Jack Sock"},
            { 104607, "Tomas Berdych"},
            { 100644, "Alexander Zverev"},
            { 105138, "Roberto Bautista Agut"},
            { 105777, "Grigor Dimitrov"},
            { 104755, "Richard Gasquet"},
            { 104180, "Gilles Muller"},
            { 106298, "Lucas Poullie"},
            { 103333, "Ivo Karlovic"},
            { 104655, "Pablo Cuevas"},
            { 105023, "Avrg1"},
            { 104259, "Avrg2"},
            { 105487, "Avrg3"},
            { 104898, "Avrg4"},
            { 104198, "Avrg5"}
        };

        public static readonly Dictionary<int, string> idToNameFemales = new Dictionary<int, string>
        {
        {200033, "S Williams"},
        {201493, "A Kerber"},
        {201474, "A Radwanska"},
        {201619, "M Keys"},
        {201594, "S Halep"},
        {201458, "V Azarenka"},
        {201495, "D Cibulkova"},
        {201662, "K Pliskova"},
        {201320, "S Kuznecowa"},
        {202427, "J Konta"},
        {201520, "P Kvitova"},
        {201496, "C Wozniacki"},
        {202494, "E Svitolina"},
        {201585, "S Stephens"},
        {200748, "V Williams"},
        {201521, "C Navarro"},
        {201366, "B Strycova"},
        {202469, "G Muguruza"},
        {201490, "T Basinszky"},
        {201455, "E Vesnina"},
        {203530, "V Golubic"},
        {211768, "N Osaka"},
        {214082, "D Kasatkina"},
        {201696, "L Siegemund"},
        {202428, "K Bertens"},
        { 201512, "Avrg1"},
        { 201611, "Avrg2"},
        { 202446, "Avrg3"},
        { 201709, "Avrg4"},
        { 201506, "Avrg5"}
        };

        public static string iDToName(int id)
        {
            return TennisDB.IDToName.idToNameMales.ContainsKey(id) ? TennisDB.IDToName.idToNameMales[id] : TennisDB.IDToName.idToNameFemales[id];
        }
    }

    public class IDToPrice
    {
        public static readonly Dictionary<int, double> idToPriceFantasyTennisLeagueMales = new Dictionary<int, double>
        {
        {104925, 22.62 },
        {104918, 24.02 },
        {105683, 13.26},
        {105453, 12.64},
        {103819, 12.08},
        {104792, 11.77},
        {104527, 11.6},
        {104745, 10.92},
        {105227, 10.8},
        {106401, 10.29},
        {104542, 10.03},
        {106233, 9.55},
        {104545, 9.46},
        {105223, 9.43 },
        {105676, 9.32},
        {106058, 8.93},
        {104607, 8.67},
        {100644, 8.67},
        {105138, 8.57},
        {105777, 8.09},
        {104755, 7.92},
        {104180, 7.76},
        {106298, 7.58},
        {103333, 7.5},
        {104655, 7.19}
        };

        public static readonly Dictionary<int, double> idToPriceFantasyTennisLeagueFemales = new Dictionary<int, double>
        {
        {200033, 19.07},
        {201493, 13.26},
        {201474, 12.48},
        {201619, 12.2},
        {201594, 11.16},
        {201458, 10.97},
        {201495, 10.9},
        {201662, 10.23},
        {201320, 9.75},
        {202427, 9.34},
        {201520, 9.22},
        {201496, 8.8},
        {202494, 8.53},
        {201585, 8.21},
        {200748, 8.17},
        {201521, 8.13},
        {201366, 7.9},
        {202469, 7.76},
        {201490, 7.6},
        {201455, 7.5},
        {203530, 7.49},
        {211768, 7.29},
        {214082, 7.26},
        {201696, 7.04},
        {202428, 6.89}
        };

        public static double idToPrice(int id)
        {
            return TennisDB.IDToPrice.idToPriceFantasyTennisLeagueMales.ContainsKey(id) ? TennisDB.IDToPrice.idToPriceFantasyTennisLeagueMales[id] : TennisDB.IDToPrice.idToPriceFantasyTennisLeagueFemales[id];
        }
    }

    public class AvrgPlayers
    {
        public static readonly List<int> avrgIDsMales = new List<int>
        {
            105023,104259,105487,104898,104198
        };

        public static readonly List<int> avrgIDsFemales = new List<int>
        {
            201512,201611,202446,201709,201506
        };
    }
}
