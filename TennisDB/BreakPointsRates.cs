using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB
{
    using DefaultMap = Dictionary<Enums.CourtTypes, double>;
    [Serializable]
    public class BreakPointsRates
    {
        public DefaultMap bpWonRates;
        public DefaultMap bpSavedRates;

        public BreakPointsRates(DefaultMap _bpWonRates, DefaultMap _bpSavedRates)
        {
            bpWonRates = _bpWonRates;
            bpSavedRates = _bpSavedRates;
        }
    }
}
