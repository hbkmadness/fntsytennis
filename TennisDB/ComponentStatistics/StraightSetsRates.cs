using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB.ComponentStatistics
{
    using DefaultMap = Dictionary<Enums.CourtTypes, double>;
    [Serializable]
    public class StraightSetsRates
    {
        public readonly DefaultMap straight3SetsWinsRate;
        public readonly DefaultMap straight3SetsLossRate;

        public readonly DefaultMap straight5SetsWinsRate;
        public readonly DefaultMap straight5SetsLossRate;

        public StraightSetsRates(DefaultMap _straight3SetsWinsRate, DefaultMap _straight3SetsLossRate,
            DefaultMap _straight5SetsWinsRate, DefaultMap _straight5SetsLossRate)
        {
            this.straight3SetsWinsRate = _straight3SetsWinsRate;
            straight3SetsLossRate = _straight3SetsLossRate;
            straight5SetsWinsRate = _straight5SetsWinsRate;
            straight5SetsLossRate = _straight5SetsLossRate;
        }
    }
}
