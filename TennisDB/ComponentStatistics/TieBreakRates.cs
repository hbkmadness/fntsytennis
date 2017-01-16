using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB.ComponentStatistics
{
    using DefaultMap = Dictionary<Enums.CourtTypes, double>;
    [Serializable]
    public class TieBreakRates
    {
        public DefaultMap tieBreakRatio;
        public DefaultMap tieBreakWinRatio;

        public TieBreakRates(DefaultMap _tieBreakRatio, DefaultMap _tieBreakWinRatio)
        {
            tieBreakRatio = _tieBreakRatio;
            tieBreakWinRatio = _tieBreakWinRatio;
        }
    }
}
