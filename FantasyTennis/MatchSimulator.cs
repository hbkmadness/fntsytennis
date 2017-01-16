using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTennis
{
    class MatchSimulator : MatchSimulatorBase
    {
        private static Random rndmizer = new Random();

        public TennisDB.TennisPlayer p1;
        public TennisDB.TennisPlayer p2;
        public TennisDB.H2HStats h2hStats;
        public TennisDB.Enums.CourtTypes courtType;
        public bool grandSlam;
        public bool males;

        public MatchSimulator(TennisDB.TennisPlayer _p1, TennisDB.TennisPlayer _p2, TennisDB.H2HStats _h2hStats, TennisDB.Enums.CourtTypes _courtType, bool _grandSlam, bool _males)
        {
            this.p1 = _p1;
            this.p2 = _p2;
            this.h2hStats = _h2hStats;
            this.courtType = _courtType;
            this.grandSlam = _grandSlam;
            this.males = _males;
        }

        private double getWinCoef3Sets(TennisDB.TennisPlayer p1, double h2hCourt, double h2hLast10)
        {
            return p1.formAtBeginningOfTournament * (0.45 * h2hCourt + 0.38 * h2hLast10 + 0.17 * p1.winRates.getWinRate(this.courtType));
        }

        private double getWinCoef5Sets(TennisDB.TennisPlayer p1, double h2hCourt, double h2hLast10, double h2h5Sets)
        {
            return p1.formAtBeginningOfTournament * (0.31 * h2h5Sets + 0.36 * h2hCourt + 0.25 * h2hLast10 + 0.1 * p1.winRates.getWinRate(this.courtType));
        }

        private SetResult simulateSet(TennisDB.TennisPlayer winner, TennisDB.TennisPlayer loser)
        {
            double winnerPoints = 0;
            double loserPoints = 0;
            double rnd;

            double tieBreakRate = winner.tieBreakRates.tieBreakRatio[courtType] * loser.tieBreakRates.tieBreakRatio[courtType];

            winnerPoints += 1;

            rnd = rndmizer.NextDouble();
            //is tie break played
            bool tieBreak = rnd <= tieBreakRate;
            bool moreThan4Games = false;
            if (tieBreak)
            {
                winnerPoints += 1;
                loserPoints += 1;
            }
            else
            {
                //did the loser won 4 games
                rnd = rndmizer.NextDouble();
                if (moreThan4Games = rnd <= (winner.gamesNumbers.opponentWins4GamesInOneSetRate[courtType] * loser.gamesNumbers.winMoreThan4GamesInOneSetWhenLostRate[courtType]))
                {
                    loserPoints += 1;
                }
            }

            winnerPoints += winner.setAvrgs.aces[courtType] * 0.2;
            loserPoints += loser.setAvrgs.aces[courtType] * 0.2;

            winnerPoints += winner.setAvrgs.doubleFaults[courtType] * -0.2;
            loserPoints += loser.setAvrgs.doubleFaults[courtType] * -0.2;

            double firstServesPlayWinnerGames = winner.setAvrgs.pointsServed[courtType] * winner.serveGameRates.firstServeInRate[courtType];
            double secondServesPlayWinnerGames = winner.setAvrgs.pointsServed[courtType] - firstServesPlayWinnerGames;

            double firstServesPlayLoserGames = winner.setAvrgs.pointsServed[courtType] * winner.serveGameRates.firstServeInRate[courtType];
            double secondServesPlayLoserGames = winner.setAvrgs.pointsServed[courtType] - firstServesPlayWinnerGames;

            winnerPoints += (firstServesPlayLoserGames * (1 - loser.serveGameRates.firstServeWinRate[courtType] + winner.returnGamesRates.firstServeReturnedWinRate[courtType]) / 2) * 0.2;
            loserPoints += (firstServesPlayWinnerGames * (1 - winner.serveGameRates.firstServeWinRate[courtType] + loser.returnGamesRates.firstServeReturnedWinRate[courtType]) / 2) * 0.2;

            winnerPoints += (secondServesPlayLoserGames * (1 - loser.serveGameRates.secondServeWinRate[courtType] + winner.returnGamesRates.secondServeReturnedWinRate[courtType]) / 2) * 0.1;
            loserPoints += (secondServesPlayWinnerGames * (1 - winner.serveGameRates.secondServeWinRate[courtType] + loser.returnGamesRates.secondServeReturnedWinRate[courtType]) / 2) * 0.1;

            winnerPoints += ((loser.setAvrgs.bpFaced[courtType] + winner.setAvrgs.bpMade[courtType]) / 2) * ((winner.breakPointsRates.bpWonRates[courtType] + loser.breakPointsRates.bpSavedRates[courtType]) / 2) * 0.5;
            loserPoints += ((winner.setAvrgs.bpFaced[courtType] + loser.setAvrgs.bpMade[courtType]) / 2) * ((loser.breakPointsRates.bpWonRates[courtType] + winner.breakPointsRates.bpSavedRates[courtType]) / 2) * 0.5;

            winnerPoints += ((loser.setAvrgs.bpMade[courtType]+winner.setAvrgs.bpFaced[courtType])/2)*((winner.breakPointsRates.bpSavedRates[courtType]+ loser.breakPointsRates.bpWonRates[courtType])/2) * 0.1;
            loserPoints += ((winner.setAvrgs.bpMade[courtType] + loser.setAvrgs.bpFaced[courtType]) / 2) * ((loser.breakPointsRates.bpSavedRates[courtType] + winner.breakPointsRates.bpWonRates[courtType]) / 2) * 0.1;

            return new SetResult(winnerPoints, loserPoints, tieBreak, moreThan4Games);
        }

        public override MatchResult simulateMatch()
        {
            int numOfMaxSets = this.grandSlam && this.males ? 5 : 3;
            //get the h2h information via p1
            double h2hThisCourtType = this.h2hStats.winRatioOnThisTypeOfCourt;
            double h2hLast10Matches = this.h2hStats.winRatioLast10Matches;

            double p1WinPoints;
            double p2WinPoints;

            //calculate the points of every player
            if (numOfMaxSets == 5)
            {
                double h2h5Sets = 0.3;
                p1WinPoints = this.getWinCoef5Sets(this.p1, h2hThisCourtType, h2hLast10Matches, h2h5Sets);
                p2WinPoints = this.getWinCoef5Sets(this.p2, 1 - h2hThisCourtType, 1 - h2hLast10Matches, 1 - h2h5Sets);
            }
            else
            {
                p1WinPoints = this.getWinCoef3Sets(this.p1, h2hThisCourtType, h2hLast10Matches);
                p2WinPoints = this.getWinCoef3Sets(this.p2, 1 - h2hThisCourtType, 1 - h2hLast10Matches);
            }

            //calculate the winning % of the 1st player
            double p1WinRate = p1WinPoints / (p1WinPoints + p2WinPoints);

            //Random decide the result
            double rnd = rndmizer.NextDouble();

            TennisDB.TennisPlayer winner = rnd <= p1WinRate ? p1 : p2;
            TennisDB.TennisPlayer loser = winner == p1 ? p2 : p1;
            double winnerPoints = 0;
            double loserPoints = 0;

            //calculate the chance of straight sets win
            double straightWinRate;
            if (numOfMaxSets == 5)
            {
                straightWinRate = winner.straightSets.straight5SetsWinsRate[courtType] * loser.straightSets.straight5SetsLossRate[courtType];
            }
            else
            {
                straightWinRate = winner.straightSets.straight3SetsWinsRate[courtType] * loser.straightSets.straight3SetsLossRate[courtType];
            }

            //determine whether or not it is a straight win
            rnd = rndmizer.NextDouble();
            bool straightSets = rnd <= straightWinRate;

            if (straightSets)
            {
                winnerPoints += 3;
            }

            //how many straights are played
            int numberOfSetsPlayed;
            int setsWonByWinner;
            int setsWonByLoser;

            if (numOfMaxSets == 5)
            {
                numberOfSetsPlayed = straightSets ? 3 : (rndmizer.NextDouble() <= 0.65 ? 4 : 5);
                setsWonByWinner = 3;
                setsWonByLoser = numberOfSetsPlayed - setsWonByWinner;
            }
            else
            {
                numberOfSetsPlayed = straightSets ? 2 : 3;
                setsWonByWinner = 2;
                setsWonByLoser = 1;
            }

            int numOfTieBreaks = 0;
            int numOfMoreThan4GamesWonByLoser = 0;
            //simulating sets
            for (int i = 0; i < setsWonByWinner; i++)
            {
                SetResult result = this.simulateSet(winner, loser);
                numOfTieBreaks += result.tieBreak ? 1 : 0;
                numOfMoreThan4GamesWonByLoser += result.moreThan4Sets ? 1 : 0;

                winnerPoints += result.winnerPoints;
                loserPoints += result.loserPoints;
            }

            for (int i = 0; i < setsWonByLoser; i++)
            {
                SetResult result = this.simulateSet(loser, winner);
                numOfTieBreaks += result.tieBreak ? 1 : 0;
                numOfMoreThan4GamesWonByLoser += result.moreThan4Sets ? 1 : 0;

                loserPoints += result.winnerPoints;
                winnerPoints += result.loserPoints;
            }

            return new MatchResult(winner.id, winnerPoints, loserPoints, numOfTieBreaks, numOfMoreThan4GamesWonByLoser);
        }
    }
}
