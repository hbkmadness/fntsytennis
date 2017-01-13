using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB
{
    public class WinRates
    {
        public double defaultWinRate = 0;
        public Dictionary<CourtTypes, double> winRates;

        public WinRates(double _defaultWinRate, double grassWin = 0, double clayWin = 0, double hardWin = 0)
        {
            this.defaultWinRate = _defaultWinRate;
            winRates = new Dictionary<CourtTypes, double>();
            winRates[CourtTypes.CLAY] = clayWin;
            winRates[CourtTypes.HARD] = hardWin;
            winRates[CourtTypes.GRASS] = hardWin;
        }

        public double getWinRate(CourtTypes type)
        {
            if (winRates.ContainsKey(type))
            {
                return winRates[type];
            }

            return defaultWinRate;
        }

    }
}
