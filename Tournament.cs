using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTennis
{
    class Tournament
    {

        public int numOfPlayers;
        public int rounds;

        public TennisDB.CourtTypes courtType;
        public bool grandSlam;
        public bool males;
        public int winnerPoints;
        public int runnerUpPoints;

        public Dictionary<int, double> playerPoints;

        public List<TennisDB.TennisPlayer> players;
        public List<Tuple<TennisDB.TennisPlayer, TennisDB.TennisPlayer>> firstRoundDrawList;

        public DrawsTree draws;

        public int currentRound = 1;

        public Tournament(List<TennisDB.TennisPlayer> _players, List<Tuple<TennisDB.TennisPlayer, TennisDB.TennisPlayer>> drawsList, int rounds,
            int winPoints, int runUpPoints, TennisDB.CourtTypes court,
            bool grandSlam, bool males)
        {
            this.players = _players;
            this.numOfPlayers = this.players.Count;
            this.rounds = rounds;

            this.firstRoundDrawList = drawsList;
            this.winnerPoints = winPoints;
            this.runnerUpPoints = runUpPoints;
            this.courtType = court;

            this.grandSlam = grandSlam;
            this.males = males;

            this.draws = new DrawsTree(rounds, this.firstRoundDrawList);
            this.playerPoints = new Dictionary<int, double>();

            this.players.ForEach((player) =>
            {
                playerPoints.Add(player.id, 0);
            });
        }

        public void simulateNextRound(){
            List<MatchNode> matchesToBePlayed = this.draws.getNodesAtRound(currentRound);

            matchesToBePlayed.ForEach((match) =>
            {
                if (match.p1 == null)
                {
                    match.parent.addPlayer(match.p2);
                    return;
                }
                if (match.p2 == null)
                {
                    match.parent.addPlayer(match.p1);
                    return;
                }

                MatchSimulator simulator = new MatchSimulator(match.p1, match.p2, this.courtType, this.grandSlam, this.males);
                MatchResult result = simulator.simulateMatch();
                match.setWinnerAndLoser(result.winnerID);

                playerPoints[match.winner.id] += result.winnerPoints;
                playerPoints[match.loser.id] += result.loserPoints;

                if (this.currentRound != this.rounds)
                {
                    match.parent.addPlayer(match.winner);
                }
            });

            this.currentRound++;
        }

        public void simulateTournament()
        {
            while (this.currentRound <= this.rounds)
            {
                this.simulateNextRound();
            }

            //adds the last bonus points for the winner and the second
            playerPoints[this.draws.root.winner.id] += this.winnerPoints;
            playerPoints[this.draws.root.loser.id] += this.runnerUpPoints;
        }

    }
}
