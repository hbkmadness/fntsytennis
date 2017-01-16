using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTennis
{
    public class MatchResult
    {
        public int winnerID;
        public double winnerPoints = 0;
        public double loserPoints = 0;
        public int numOfTieBreaks = 0;
        //number of sets with more than 4 games won by the loser but without playing tiebreak
        public int numOf4GamesWon = 0;

        public MatchResult(int _id, double _winPts, double _lsrPts, int _numOfTieBreaks, int _numOf4GamesWon)
        {
            this.winnerID = _id;
            this.winnerPoints = _winPts;
            this.loserPoints = _lsrPts;
            this.numOfTieBreaks = _numOfTieBreaks;
            this.numOf4GamesWon = _numOf4GamesWon;
        }
    }
}
