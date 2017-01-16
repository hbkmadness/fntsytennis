using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB.ComponentStatistics
{
    using DefaultMap = Dictionary<Enums.CourtTypes, double>;
    [Serializable]
    //All numbers here are the number of the stats average per set
    public class SetAvrgs
    {
        public readonly DefaultMap pointsServed;
        public readonly DefaultMap aces;
        public readonly DefaultMap doubleFaults;

        public readonly DefaultMap bpMade;
        public readonly DefaultMap bpFaced;

        public SetAvrgs(DefaultMap _pointsServed, DefaultMap _aces, DefaultMap _doubleFaults,
            DefaultMap _bpMade, DefaultMap _bpFaced)
        {
            pointsServed = _pointsServed;
            aces = _aces;
            doubleFaults = _doubleFaults;
            bpMade = _bpMade;
            bpFaced = _bpFaced;
        }
    }
}
