using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB
{
    using DefMap = Dictionary<CourtTypes, double>;
    [Serializable] public class TennisPlayer
    {
        public int id = 0;
        public double price = 0;
        public double currentFormRate = 0.8;

        public WinRates winRates = new WinRates(0);
        public DefMap straight3SetsWinsRate;
        public DefMap straight3SetsLossRate;

        public DefMap straight5SetsWinsRate;
        public DefMap straight5SetsLossRate;

        public DefMap donutLossRate;
        public DefMap opponentWins4GamesInOneSetRate;
        public DefMap winMoreThan4GamesInOneSetWhenLostRate;

        public DefMap tieBreakRatio;
        public DefMap tieBreakWinRatio;

        public DefMap pointsServedPerSet;
        public DefMap acesAvrgPerSet;
        public DefMap doubleFaultsAvrgPerSet;

        public DefMap firstServeInRate;

        public DefMap firstServeWinRate;
        public DefMap firstServeReturnedWinRate;

        public DefMap secondServeWinRate;
        public DefMap secondServeReturnedWinRate;

        public DefMap bpMadePerSet;
        public DefMap bpFacedPerSet;

        public DefMap bpWonRates;
        public DefMap bpSavedRates;

        public TennisPlayer(int _id, double _price, WinRates _wrs, DefMap straight3SetsWinsRate, DefMap straight3SetsLossRate, DefMap straight5SetsWinsRate,
            DefMap straight5SetsLossRate, DefMap donutLossRate, DefMap opponentWins4GamesInOneSetRate, DefMap winMoreThan4GamesInOneSetWhenLostRate, DefMap tieBreakRatio, DefMap tieBreakWinRatio,
            DefMap pointsServedPerSet, DefMap acesAvrgPerSet, DefMap doubleFaultsAvrgPerSet, DefMap firstServeInRate, DefMap firstServeWinRate, DefMap firstServeReturnedWinRate, DefMap secondServeWinRate,
            DefMap secondServeReturnedWinRate, DefMap bpMadePerSet, DefMap bpFacedPerSet, DefMap bpWonRates, DefMap bpSavedRates)
        {
            this.id = _id;
            this.price = _price;
            this.winRates = _wrs;

            this.straight3SetsWinsRate = straight3SetsWinsRate;
            this.straight3SetsLossRate = straight3SetsLossRate;

            this.straight5SetsWinsRate = straight5SetsWinsRate;
            this.straight5SetsLossRate = straight5SetsLossRate;

            this.donutLossRate = donutLossRate;
            this.opponentWins4GamesInOneSetRate = opponentWins4GamesInOneSetRate;
            this.winMoreThan4GamesInOneSetWhenLostRate = winMoreThan4GamesInOneSetWhenLostRate;

            this.tieBreakRatio = tieBreakRatio;
            this.tieBreakWinRatio = tieBreakWinRatio;

            this.pointsServedPerSet = pointsServedPerSet;
            this.acesAvrgPerSet = acesAvrgPerSet;
            this.doubleFaultsAvrgPerSet = doubleFaultsAvrgPerSet;

            this.firstServeInRate = firstServeInRate;

            this.firstServeWinRate = firstServeWinRate;
            this.firstServeReturnedWinRate = firstServeReturnedWinRate;

            this.secondServeWinRate = secondServeWinRate;
            this.secondServeReturnedWinRate = secondServeReturnedWinRate;

            this.bpMadePerSet = bpMadePerSet;
            this.bpFacedPerSet = bpFacedPerSet;

            this.bpWonRates = bpWonRates;
            this.bpSavedRates = bpSavedRates;
    }

    }
}
