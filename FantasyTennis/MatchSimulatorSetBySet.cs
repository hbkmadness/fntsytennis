using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTennis
{
    using DefaultMap = Dictionary<TennisDB.Enums.CourtTypes, double>;
    class MatchSimulatorSetBySet : MatchSimulatorBase
    {
        public TennisDB.TennisPlayer p1;
        public TennisDB.TennisPlayer p2;
        public TennisDB.Enums.CourtTypes courtType;
        public bool grandSlam;
        public bool males;

        //returns a proper value in certain situation. Since it is possible the value to be -1, this function makes sure that at least 
        //it was tried to take another value based on the ratings in the same situation but with other court types
        //if there were again no situation like that it returns -1
        private double tryGetProperRating(TennisDB.TennisPlayer p, string componentName)
        {
            var ratings = grandSlam && males ? ((DefaultMap)p.setBySetStats.max5SetsMatchStats[componentName]): ((DefaultMap)p.setBySetStats.max3SetsMatchStats[componentName]);
            double realRating = ratings[courtType];

            if (realRating == -1)
            {
                double sum = 0;
                double count = 0;
                foreach (TennisDB.Enums.CourtTypes court in Enum.GetValues(typeof(TennisDB.Enums.CourtTypes)))
                {
                    if(ratings[court] != -1)
                    {
                        sum += ratings[court];
                        count++;
                    }
                }

                realRating = count > 0 ? sum / count : -1;
            }

            return realRating;
        }

        private Dictionary<string,double> createSituationMap(TennisDB.TennisPlayer p)
        {
            Dictionary<string, double> dic = new Dictionary<string, double>();
            double valueToBeAdded;
            if (grandSlam && males)
            {
                valueToBeAdded = tryGetProperRating(p, "winFirstSet");
            }
            else
            {
                valueToBeAdded = tryGetProperRating(p, "match3SetsWinFirstSetRate");
            }

            //if(valueToBeAdded == -1)
            //{
            //    double[] rates = {
            //        p.setBySetStats.max3SetsMatchStats.match3SetsWinFirstSetRate[TennisDB.Enums.CourtTypes.HARD],
            //        p.setBySetStats.max3SetsMatchStats.match3SetsWinFirstSetRate[TennisDB.Enums.CourtTypes.CLAY],
            //        p.setBySetStats.max3SetsMatchStats.match3SetsWinFirstSetRate[TennisDB.Enums.CourtTypes.GRASS]};
            //    //valueToBeAdded = avrRates(rates);
            //    valueToBeAdded = valueToBeAdded == -1 ? p.winRates.winRates[courtType] : valueToBeAdded;
            //}
            //dic.Add("", numOfSets == 3 ? valueToBeAdded);

            return null;
        }

        public MatchSimulatorSetBySet(TennisDB.TennisPlayer _p1, TennisDB.TennisPlayer _p2, TennisDB.Enums.CourtTypes _courtType,
            bool _grandSlam, bool _males)
        {
            this.p1 = _p1;
            this.p2 = _p2;
            this.courtType = _courtType;
            this.grandSlam = _grandSlam;
            this.males = _males;
        }
        public override MatchResult simulateMatch()
        {
            int numOfSets = males && grandSlam ? 5 : 3;
            createSituationMap(p1);

            return null;
        }
    }
}
