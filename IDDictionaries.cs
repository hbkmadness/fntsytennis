using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB
{
    public class IDToName
    {
        public static readonly Dictionary<int, string> idToName = new Dictionary<int, string>
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
            { 104198, "Avrg5"},
    //{26, "S Williams"},
    //{27, "A Kerber"},
    //{28, "A Radwanska"},
    //{29, "M Keys"},
    //{30, "S Halep"},
    //{31, "V Azarenka"},
    //{32, "D Cibulkova"},
    //{33, "K Pliskova"},
    //{34, "S Kuznecowa"},
    //{35, "J Konta"},
    //{36, "P Kvitova"},
    //{37, "C Wozniacki"},
    //{38, "E Svitolina"},
    //{39, "S Stephens"},
    //{40, "V Williams"},
    //{41, "C Navarro"},
    //{42, "B Strycova"},
    //{43, "G Muguruza"},
    //{44, "T Basinszky"},
    //{45, "E Vescnina"},
    //{46, "V Kulibic"},
    //{47, "N Osaka"},
    //{48, "D Kasatkina"},
    //{49, "L Sigermund"},
    //{50, "K Bertens"}
};
    }

    public class IDToPrice
    {
        public static readonly Dictionary<int, double> idToPrice = new Dictionary<int, double>
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
    //{26, "S Williams"},
    //{27, "A Kerber"},
    //{28, "A Radwanska"},
    //{29, "M Keys"},
    //{30, "S Halep"},
    //{31, "V Azarenka"},
    //{32, "D Cibulkova"},
    //{33, "K Pliskova"},
    //{34, "S Kuznecowa"},
    //{35, "J Konta"},
    //{36, "P Kvitova"},
    //{37, "C Wozniacki"},
    //{38, "E Svitolina"},
    //{39, "S Stephens"},
    //{40, "V Williams"},
    //{41, "C Navarro"},
    //{42, "B Strycova"},
    //{43, "G Muguruza"},
    //{44, "T Basinszky"},
    //{45, "E Vescnina"},
    //{46, "V Kulibic"},
    //{47, "N Osaka"},
    //{48, "D Kasatkina"},
    //{49, "L Sigermund"},
    //{50, "K Bertens"}
};
    }

    public class AvrgPlayers
    {
        public static readonly List<int> avrgIDs = new List<int>
        {
            105023,104259,105487,104898,104198
        };
    }
}
