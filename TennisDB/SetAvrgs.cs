using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB
{
    using DefaultMap = Dictionary<Enums.CourtTypes, double>;
    [Serializable]
    //All numbers here are the number of the stats average per set
    public class SetAvrgs
    {
        public DefaultMap pointsServed;
        public DefaultMap aces;
        public DefaultMap doubleFaults;

        public DefaultMap bpMade;
        public DefaultMap bpFaced;

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
