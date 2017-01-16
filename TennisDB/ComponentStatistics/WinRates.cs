using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB.ComponentStatistics
{
    [Serializable] public class WinRates
    {
        public double defaultWinRate = 0;
        public Dictionary<Enums.CourtTypes, double> winRates;

        public WinRates(double _defaultWinRate, double grassWin = 0, double clayWin = 0, double hardWin = 0)
        {
            this.defaultWinRate = _defaultWinRate;
            winRates = new Dictionary<Enums.CourtTypes, double>();
            winRates[Enums.CourtTypes.CLAY] = clayWin;
            winRates[Enums.CourtTypes.HARD] = hardWin;
            winRates[Enums.CourtTypes.GRASS] = hardWin;
        }

        public double getWinRate(Enums.CourtTypes type)
        {
            if (winRates.ContainsKey(type))
            {
                return winRates[type];
            }

            return defaultWinRate;
        }

    }
}
