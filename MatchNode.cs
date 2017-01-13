using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTennis
{
    class MatchNode
    {
        public MatchNode parent = null;
        public MatchNode leftChild = null;
        public MatchNode rightChild = null;

        public TennisDB.TennisPlayer p1 = null;
        public TennisDB.TennisPlayer p2 = null;

        public TennisDB.TennisPlayer winner;
        public TennisDB.TennisPlayer loser;

        

        public int round = 0;

        public MatchNode(MatchNode parent, int round, TennisDB.TennisPlayer p1 = null, TennisDB.TennisPlayer p2 = null)
        {
            this.parent = parent;
            this.round = round;
            this.p1 = p1;
            this.p2 = p2;
        }

        public void addPlayer(TennisDB.TennisPlayer p)
        {
            if (p1 == null)
            {
                this.p1 = p;
            }
            else
            {
                this.p2 = p;
            }
        }

        public void setWinnerAndLoser(int winnerID){
            this.winner = p1.id == winnerID ? p1 : p2;
            this.loser = this.winner.id == p1.id ? p2 : p1;
        }
    }
}
