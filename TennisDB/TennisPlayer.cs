using System;
using System.Collections.Generic;
using TennisDB.ComponentStatistics;

namespace TennisDB
{
    using DefaultMap = Dictionary<Enums.CourtTypes, double>;
    [Serializable] public class TennisPlayer
    {
        public readonly int id = 0;
        public readonly double price = 0;
        public readonly double formAtBeginningOfTournament = 0.8;

        public readonly WinRates winRates = new WinRates(0);

        public readonly StraightSetsRates straightSets;
        public readonly GamesNumberRates gamesNumbers;

        public readonly SetAvrgs setAvrgs;

        public readonly TieBreakRates tieBreakRates;

        public readonly ServeGameRates serveGameRates;
        public readonly ReturnGameRates returnGamesRates;

        public readonly BreakPointsRates breakPointsRates;

        public readonly SetBySetStats setBySetStats;

        public TennisPlayer(int _id, double _price, WinRates _wrs, StraightSetsRates _straightSets, GamesNumberRates _gamesNumbers, TieBreakRates _tieBreakRates,
            SetAvrgs _setAvrgs, ServeGameRates _serveGameRates, ReturnGameRates _returnGamesRates, BreakPointsRates _breaksPointsRates, SetBySetStats _setBySetStats = null,
            double _formAtBeginningOfTournament = 0.8)
        {
            this.id = _id;
            this.price = _price;
            this.winRates = _wrs;

            this.straightSets = _straightSets;
            this.gamesNumbers = _gamesNumbers;

            this.setAvrgs = _setAvrgs;

            this.tieBreakRates = _tieBreakRates;

            this.serveGameRates = _serveGameRates;
            this.returnGamesRates = _returnGamesRates;

            this.breakPointsRates = _breaksPointsRates;
            this.setBySetStats = _setBySetStats;

            this.formAtBeginningOfTournament = _formAtBeginningOfTournament;
    }

    }
}
