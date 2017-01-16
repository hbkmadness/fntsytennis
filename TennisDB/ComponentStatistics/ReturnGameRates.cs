using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB.ComponentStatistics
{
    using DefaultMap = Dictionary<Enums.CourtTypes, double>;
    [Serializable]
    public class ReturnGameRates
    {
        public readonly DefaultMap firstServeReturnedWinRate;
        public readonly DefaultMap secondServeReturnedWinRate;

        public ReturnGameRates(DefaultMap _firstServeReturnedWinRate, DefaultMap _secondServeReturnedWinRate)
        {
            firstServeReturnedWinRate = _firstServeReturnedWinRate;
            secondServeReturnedWinRate = _secondServeReturnedWinRate;
        }
    }
}
