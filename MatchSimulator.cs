using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTennis
{
    class MatchSimulator
    {
        private static Random rndmizer = new Random();

        public TennisDB.TennisPlayer p1;
        public TennisDB.TennisPlayer p2;
        public TennisDB.H2HStats h2hStats;
        public TennisDB.CourtTypes courtType;
        public bool grandSlam;
        public bool males;

        public MatchSimulator(TennisDB.TennisPlayer _p1, TennisDB.TennisPlayer _p2, TennisDB.H2HStats _h2hStats, TennisDB.CourtTypes _courtType, bool _grandSlam, bool _males)
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
            return p1.formAtBeginningOfTournament * (0.5 * h2hCourt + 0.35 * h2hLast10 + 0.15 * p1.winRates.getWinRate(this.courtType));
        }

        private double getWinCoef5Sets(TennisDB.TennisPlayer p1, double h2hCourt, double h2hLast10, double h2h5Sets)
        {
            return p1.formAtBeginningOfTournament * (0.4 * h2h5Sets + 0.35 * h2hCourt + 0.15 * h2hLast10 + 0.1 * p1.winRates.getWinRate(this.courtType));
        }

        private SetResult simulateSet(TennisDB.TennisPlayer winner, TennisDB.TennisPlayer loser)
        {
            double winnerPoints = 0;
            double loserPoints = 0;
            double rnd;

            double tieBreakRate = winner.tieBreakRatio[courtType] * loser.tieBreakRatio[courtType];

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
                if (moreThan4Games = rnd <= (winner.opponentWins4GamesInOneSetRate[courtType] * loser.winMoreThan4GamesInOneSetWhenLostRate[courtType]))
                {
                    loserPoints += 1;
                }
            }

            winnerPoints += winner.acesAvrgPerSet[courtType] * 0.2;
            loserPoints += loser.acesAvrgPerSet[courtType] * 0.2;

            winnerPoints += winner.doubleFaultsAvrgPerSet[courtType] * -0.2;
            loserPoints += loser.doubleFaultsAvrgPerSet[courtType] * -0.2;

            double firstServesPlayWinnerGames = winner.pointsServedPerSet[courtType] * winner.firstServeInRate[courtType];
            double secondServesPlayWinnerGames = winner.pointsServedPerSet[courtType] - firstServesPlayWinnerGames;

            double firstServesPlayLoserGames = winner.pointsServedPerSet[courtType] * winner.firstServeInRate[courtType];
            double secondServesPlayLoserGames = winner.pointsServedPerSet[courtType] - firstServesPlayWinnerGames;

            winnerPoints += (firstServesPlayLoserGames * (1 - loser.firstServeWinRate[courtType] + winner.firstServeReturnedWinRate[courtType]) / 2) * 0.2;
            loserPoints += (firstServesPlayWinnerGames * (1 - winner.firstServeWinRate[courtType] + loser.firstServeReturnedWinRate[courtType]) / 2) * 0.2;

            winnerPoints += (secondServesPlayLoserGames * (1 - loser.secondServeWinRate[courtType] + winner.secondServeReturnedWinRate[courtType]) / 2) * 0.1;
            loserPoints += (secondServesPlayWinnerGames * (1 - winner.secondServeWinRate[courtType] + loser.secondServeReturnedWinRate[courtType]) / 2) * 0.1;

            winnerPoints += ((loser.bpFacedPerSet[courtType] + winner.bpMadePerSet[courtType]) / 2) * ((winner.bpWonRates[courtType] + loser.bpSavedRates[courtType]) / 2) * 0.5;
            loserPoints += ((winner.bpFacedPerSet[courtType] + loser.bpMadePerSet[courtType]) / 2) * ((loser.bpWonRates[courtType] + winner.bpSavedRates[courtType]) / 2) * 0.5;

            winnerPoints += ((loser.bpMadePerSet[courtType]+winner.bpFacedPerSet[courtType])/2)*((winner.bpSavedRates[courtType]+ loser.bpWonRates[courtType])/2) * 0.1;
            loserPoints += ((winner.bpMadePerSet[courtType] + loser.bpFacedPerSet[courtType]) / 2) * ((loser.bpSavedRates[courtType] + winner.bpWonRates[courtType]) / 2) * 0.1;

            return new SetResult(winnerPoints, loserPoints, tieBreak, moreThan4Games);
        }

        public MatchResult simulateMatch()
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
                straightWinRate = winner.straight5SetsWinsRate[courtType] * loser.straight5SetsLossRate[courtType];
            }
            else
            {
                straightWinRate = winner.straight3SetsWinsRate[courtType] * loser.straight3SetsLossRate[courtType];
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
