using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTennis
{
    class SetResult
    {
        public double winnerPoints = 0;
        public double loserPoints = 0;
        public bool tieBreak;
        public bool moreThan4Sets;

        public SetResult(double _winPts, double _lsrPts, bool _tieBreak, bool _moreThan4Sets)
        {
            this.winnerPoints = _winPts;
            this.loserPoints = _lsrPts;
            this.tieBreak = _tieBreak;
            this.moreThan4Sets = _moreThan4Sets;
        }
    }
}
