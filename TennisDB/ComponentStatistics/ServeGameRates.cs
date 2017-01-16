using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB.ComponentStatistics
{
    using DefaultMap = Dictionary<Enums.CourtTypes, double>;
    [Serializable]
    public class ServeGameRates
    {
        public readonly DefaultMap firstServeInRate;
        public readonly DefaultMap firstServeWinRate;
        public readonly DefaultMap secondServeWinRate;

        public ServeGameRates(DefaultMap _firstServeInRate, DefaultMap _firstServeWinRate, DefaultMap _secondServeWinRate)
        {
            firstServeInRate = _firstServeInRate;
            firstServeWinRate = _firstServeWinRate;
            secondServeWinRate = _secondServeWinRate;
        }
    }
}
