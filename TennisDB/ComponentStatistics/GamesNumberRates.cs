using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB.ComponentStatistics
{
    using DefaultMap = Dictionary<Enums.CourtTypes, double>;
    [Serializable]
    public class GamesNumberRates
    {
        public readonly DefaultMap donutLossRate;
        public readonly DefaultMap opponentWins4GamesInOneSetRate;
        public readonly DefaultMap winMoreThan4GamesInOneSetWhenLostRate;

        public GamesNumberRates(DefaultMap _donutLossRate, DefaultMap _opponentWins4GamesInOneSetRate, DefaultMap _winMoreThan4GamesInOneSetWhenLostRate)
        {
            donutLossRate = _donutLossRate;
            opponentWins4GamesInOneSetRate = _opponentWins4GamesInOneSetRate;
            winMoreThan4GamesInOneSetWhenLostRate = _winMoreThan4GamesInOneSetWhenLostRate;
        }
    }
}
