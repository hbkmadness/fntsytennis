using System;
using System.Collections.Generic;
using TennisDB.ComponentStatistics;

namespace TennisDB
{
    using DefaultMap = Dictionary<Enums.CourtTypes, double>;
    [Serializable] public class TennisPlayer
    {
        public int id = 0;
        public double price = 0;
        public double formAtBeginningOfTournament = 0.8;

        public WinRates winRates = new WinRates(0);

        public StraightSetsRates straightSets;
        public GamesNumberRates gamesNumbers;

        public SetAvrgs setAvrgs;

        public TieBreakRates tieBreakRates;

        public ServeGameRates serveGameRates;
        public ReturnGameRates returnGamesRates;

        public BreakPointsRates breakPointsRates;

        public TennisPlayer(int _id, double _price, WinRates _wrs, StraightSetsRates _straightSets, GamesNumberRates _gamesNumbers, TieBreakRates _tieBreakRates,
            SetAvrgs _setAvrgs, ServeGameRates _serveGameRates, ReturnGameRates _returnGamesRates, BreakPointsRates _breaksPointsRates, double _formAtBeginningOfTournament = 0.8)
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

            this.formAtBeginningOfTournament = _formAtBeginningOfTournament;
    }

    }
}
