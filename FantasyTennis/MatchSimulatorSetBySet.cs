using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTennis
{
    class MatchSimulatorSetBySet : MatchSimulatorBase
    {
        public TennisDB.TennisPlayer p1;
        public TennisDB.TennisPlayer p2;
        public TennisDB.Enums.CourtTypes courtType;
        public bool grandSlam;
        public bool males;

        private double getProperRating(string setsStatsPropertyName, string componentName)
        {
            return 0.0;
        }

        private Dictionary<string,double> createSituationMap(TennisDB.TennisPlayer p, int numOfSets)
        {
            Dictionary<string, double> dic = new Dictionary<string, double>();
            double valueToBeAdded = p.setBySetStats.max3SetsMatchStats.match3SetsWinFirstSetRate[courtType];
            if(valueToBeAdded == -1)
            {
                double[] rates = {
                    p.setBySetStats.max3SetsMatchStats.match3SetsWinFirstSetRate[TennisDB.Enums.CourtTypes.HARD],
                    p.setBySetStats.max3SetsMatchStats.match3SetsWinFirstSetRate[TennisDB.Enums.CourtTypes.CLAY],
                    p.setBySetStats.max3SetsMatchStats.match3SetsWinFirstSetRate[TennisDB.Enums.CourtTypes.GRASS]};
                //valueToBeAdded = avrRates(rates);
                valueToBeAdded = valueToBeAdded == -1 ? p.winRates.winRates[courtType] : valueToBeAdded;
            }
            //dic.Add("", numOfSets == 3 ? valueToBeAdded);

            return null;
        }
        public override MatchResult simulateMatch()
        {
            int numOfSets = males && grandSlam ? 5 : 3;

            return null;
        }
    }
}
