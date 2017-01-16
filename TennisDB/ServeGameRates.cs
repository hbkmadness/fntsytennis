using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB
{
    using DefaultMap = Dictionary<Enums.CourtTypes, double>;
    [Serializable]
    public class ServeGameRates
    {
        public DefaultMap firstServeInRate;
        public DefaultMap firstServeWinRate;
        public DefaultMap secondServeWinRate;

        public ServeGameRates(DefaultMap _firstServeInRate, DefaultMap _firstServeWinRate, DefaultMap _secondServeWinRate)
        {
            firstServeInRate = _firstServeInRate;
            firstServeWinRate = _firstServeWinRate;
            secondServeWinRate = _secondServeWinRate;
        }
    }
}
