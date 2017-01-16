using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB.ComponentStatistics
{
    using DefaultMap = Dictionary<Enums.CourtTypes, double>;
    [Serializable]
    public class BreakPointsRates
    {
        public readonly DefaultMap bpWonRates;
        public readonly DefaultMap bpSavedRates;

        public BreakPointsRates(DefaultMap _bpWonRates, DefaultMap _bpSavedRates)
        {
            bpWonRates = _bpWonRates;
            bpSavedRates = _bpSavedRates;
        }
    }
}
