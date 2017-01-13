using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB
{
    public class H2HStats
    {
        public double winRatioOnThisTypeOfCourt;
        public double winRatioLast10Matches;

        public H2HStats(double winRatioCourt, double winRatioLast10)
        {
            this.winRatioOnThisTypeOfCourt = winRatioCourt;
            this.winRatioLast10Matches = winRatioLast10;
        }
    }
}
