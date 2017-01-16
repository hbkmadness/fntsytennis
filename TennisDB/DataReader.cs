using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB
{
    public class DataReader
    {
        public static List<String> tableNamesMan = new List<string> { "atp_matches_2016$", "atp_matches_2015$", "atp_matches_2014$", "atp_matches_2013$", "atp_matches_2012$", "atp_matches_2011$",
                "atp_matches_2010$", "atp_matches_2009$", "atp_matches_2008$","atp_matches_2007$","atp_matches_2006$" };

        public static List<String> tableNamesWoman = new List<string> { "wta_matches_2016$", "wta_matches_2015$", "wta_matches_2014$", "wta_matches_2013$", "wta_matches_2012$", "wta_matches_2011$" };

        //the connection with the db
        SqlConnection connection;
        //all names of the tables that are going to be used for the queries
        List<String> tablesNames;
        //Map that connects the enum for the court types and the string representation of them in the tables
        Dictionary<Enums.CourtTypes, String> courtTypesKeys;
        //all the IDs of players 
        List<int> playersIDs;

        //Dictionary that holds for each id some basic stats that were calculated at some point of the program running, this is used so no additional queries will be needed after the first one
        Dictionary<int, PlayerDynamicStats> playerStats;

        public DataReader(bool male = true)
        {
            connection = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TestDB;Data Source=.\\SQLEXPRESS;MultipleActiveResultSets=true;");

            tablesNames = male ? DataReader.tableNamesMan : DataReader.tableNamesWoman;

            courtTypesKeys = new Dictionary<Enums.CourtTypes, string> { { Enums.CourtTypes.CLAY, "'Clay'" }, { Enums.CourtTypes.HARD, "'Hard'" }, { Enums.CourtTypes.GRASS, "'Grass'" } };

            playersIDs = new List<int>();

            foreach (var entry in male ? StaticData.IDToName.idToNameMales : StaticData.IDToName.idToNameFemales)
            {
                playersIDs.Add(entry.Key);
            }

            playerStats = new Dictionary<int, PlayerDynamicStats>();
        }

        private List<SqlDataReader> executeQuery(string query, int lastYears = 6)
        {
            List<SqlDataReader> returnList = new List<SqlDataReader>();
            int counter = 0;

            tablesNames.ForEach((table) =>
            {
                if (counter < lastYears)
                {
                    SqlCommand cmd = new SqlCommand(String.Format(query, table), connection);
                    try
                    {
                        returnList.Add(cmd.ExecuteReader());
                    }
                    catch (System.Data.SqlClient.SqlException exc)
                    {

                    }
                    counter++;
                }

            });

            return returnList;
        }

        private double calculateWinRateForTypeOfCourt(int id, string courtType)
        {
            double numberMatches = 0;

            //Take the number of all matches won
            double matchesWon = 0;
            List<SqlDataReader> matchesReaderWin = this.executeQuery(String.Format("SELECT COUNT(*) FROM {0} WHERE winner_id = {1} AND surface = {2}", "{0}", id, courtType));

            matchesReaderWin.ForEach((matchRead) =>
            {
                while (matchRead.Read())
                {
                    matchesWon += matchRead.GetInt32(0);
                }
            });

            //take the number of all matches lost
            double matchesLost = 0;
            List<SqlDataReader> matchesReaderLoss = this.executeQuery(String.Format("SELECT COUNT(*) FROM {0} WHERE loser_id = {1} AND surface = {2}", "{0}", id, courtType));

            matchesReaderLoss.ForEach((matchRead) =>
            {
                while (matchRead.Read())
                {
                    matchesLost += matchRead.GetInt32(0);
                }
            });

            numberMatches = matchesWon + matchesLost;

            return numberMatches > 0 ? matchesWon / numberMatches : 0;
        }

        private double calculateDefaultWinRate(int id)
        {
            double numberMatches = 0;

            //Take the number of all matches won
            double matchesWon = 0;
            List<SqlDataReader> matchesReaderWin = this.executeQuery(String.Format("SELECT COUNT(*) FROM {0} WHERE winner_id = {1}", "{0}", id));

            matchesReaderWin.ForEach((matchRead) =>
            {
                while (matchRead.Read())
                {
                    matchesWon += matchRead.GetInt32(0);
                }
            });

            //take the number of all matches lost
            double matchesLost = 0;
            List<SqlDataReader> matchesReaderLoss = this.executeQuery(String.Format("SELECT COUNT(*) FROM {0} WHERE loser_id = {1}", "{0}", id));

            matchesReaderLoss.ForEach((matchRead) =>
            {
                while (matchRead.Read())
                {
                    matchesLost += matchRead.GetInt32(0);
                }
            });

            numberMatches = matchesWon + matchesLost;

            return numberMatches > 0 ? matchesWon / numberMatches : 0;
        }

        private List<double> calculateAllWinRates(int id)
        {
            List<double> list = new List<double>();
            list.Add(calculateWinRateForTypeOfCourt(id, courtTypesKeys[Enums.CourtTypes.CLAY]));
            list.Add(calculateWinRateForTypeOfCourt(id, courtTypesKeys[Enums.CourtTypes.HARD]));
            list.Add(calculateWinRateForTypeOfCourt(id, courtTypesKeys[Enums.CourtTypes.GRASS]));
            list.Add(calculateDefaultWinRate(id));

            return list;
        }

        //Returns a dictionary with the results set by set (key: number of set, tuple: games of winner, games of loser)
        private Dictionary<int, Tuple<int, int>> getMatchResultSetBySet(string result)
        {
            Dictionary<int, Tuple<int, int>> retDic = new Dictionary<int, Tuple<int, int>>();
            string[] difSets = result.Split(' ');
            int numOfSet = 1;
            foreach (string set in difSets)
            {
                if (set != "RET" && set != "DEF")
                {
                    if (set != "" && set != "W/O")
                    {
                        string[] games = set.Split('-');
                        int gamesp1 = int.Parse(games[0]);
                        int gamesp2 = int.Parse(games[1][0].ToString());

                        retDic.Add(numOfSet, new Tuple<int, int>(gamesp1, gamesp2));
                    }
                }
                else
                {
                    retDic.Add(numOfSet, new Tuple<int, int>(-1, -1));
                }

                numOfSet++;
            }

            return retDic;
        }

        //Returns the match result only counting the sets, e.g. 2-0 ,2-1, 0-3 and etc.
        private Tuple<int, int> getNormilizedMathResult(Dictionary<int, Tuple<int, int>> result)
        {
            int p1Sets = 0, p2Sets = 0;

            foreach (var entry in result)
            {

                int gamesP1 = entry.Value.Item1;
                int gamesP2 = entry.Value.Item2;

                if (gamesP1 > gamesP2)
                {
                    p1Sets++;
                }
                else
                {
                    p2Sets++;
                }
            }

            return new Tuple<int, int>(p1Sets, p2Sets);
        }

        //Returns the rate of straight set wins or losses depending on the maximum sets that could be played
        private double getStraightSetsWin(int id, DefaultTableKeys winKey, int numOfSets, Enums.CourtTypes court)
        {
            List<SqlDataReader> allMatches = this.executeQuery(String.Format("SELECT score FROM {0} WHERE {1} = {2} AND best_of = {3} AND surface = {4}", "{0}", winKey.Value, id, numOfSets, courtTypesKeys[court]));
            double matchCount = 0;
            double straightWinsCount = 0;

            allMatches.ForEach((matchesBySeason) =>
            {
                while (matchesBySeason.Read())
                {
                    matchCount++;
                    String score = matchesBySeason.GetString(0);

                    var result = getNormilizedMathResult(getMatchResultSetBySet(score));
                    if (result.Item2 == 0)
                    {
                        straightWinsCount++;
                    }
                }

            });

            return matchCount > 0 ? straightWinsCount / matchCount : 0;
        }

        //Returns a tuple of the rates that the player let the opponent wins at least 4 games when a sets is won or he wins 4 sets when loses
        //The first item is when the player has won the set but let the opponent have at least 4 games
        //The second item is when the player has lost the set but won at least 4 games
        private Tuple<double, double> getAtLeast4GamesWon(int id, Enums.CourtTypes court)
        {
            List<SqlDataReader> allMatches = this.executeQuery(String.Format("SELECT score, winner_id FROM {0} WHERE (winner_id = {1} OR loser_id = {1}) AND surface = {2}", "{0}", id, courtTypesKeys[court]));
            double setsWonCount = 0;
            double setsLetOpponentHave4Games = 0;

            double setsLostCount = 0;
            double setsWonAtLeast4Games = 0;

            allMatches.ForEach((matchesBySeason) =>
            {
                while (matchesBySeason.Read())
                {
                    String score = matchesBySeason.GetString(0);
                    double winnerId = matchesBySeason.GetDouble(1);

                    var setBySet = getMatchResultSetBySet(score);

                    foreach (var setResult in setBySet)
                    {
                        //If he is the winner => the first number in the set result is the number of games he won =>
                        //If this number is bigger than the second he has won the set => we check if the other 1 has more than 4 games
                        //if it's not bigger he lost => check if he has won more than 4 games

                        //else if he is not the winner his numbers are the second item and check the same thing
                        if (winnerId == id)
                        {
                            if (setResult.Value.Item1 > setResult.Value.Item2)
                            {
                                setsWonCount++;
                                if (setResult.Value.Item2 >= 4)
                                {
                                    setsLetOpponentHave4Games++;
                                }
                            }
                            else
                            {
                                setsLostCount++;
                                if (setResult.Value.Item1 >= 4)
                                {
                                    setsWonAtLeast4Games++;
                                }
                            }
                        }
                        else
                        {
                            if (setResult.Value.Item2 > setResult.Value.Item1)
                            {
                                setsWonCount++;
                                if (setResult.Value.Item1 >= 4)
                                {
                                    setsLetOpponentHave4Games++;
                                }
                            }
                            else
                            {
                                setsLostCount++;
                                if (setResult.Value.Item2 >= 4)
                                {
                                    setsWonAtLeast4Games++;
                                }
                            }
                        }
                    }
                }
            });

            return new Tuple<double, double>(setsWonCount>0 ?setsLetOpponentHave4Games / setsWonCount : 0,
                setsLostCount>0? setsWonAtLeast4Games / setsLostCount : 0);
        }

        //Returns a tuple with
        //Item 1 is the tie break ratio (how often the player plays tiebreaks)
        //Item 2 is the tie break win ratio
        private Tuple<double, double> getTieBreakRate(int id, Enums.CourtTypes court)
        {
            List<SqlDataReader> allMatches = this.executeQuery(String.Format("SELECT score, winner_id FROM {0} WHERE (winner_id = {1} OR loser_id = {1}) AND surface = {2}", "{0}", id, courtTypesKeys[court]));
            double allSetsPlayed = 0;
            double tieBreaksPlayed = 0;
            double tieBreaksWon = 0;

            allMatches.ForEach((matchesBySeason) =>
            {
                while (matchesBySeason.Read())
                {
                    String score = matchesBySeason.GetString(0);
                    double winnerId = matchesBySeason.GetDouble(1);

                    var setBySet = getMatchResultSetBySet(score);

                    foreach (var setResult in setBySet)
                    {
                        if (setResult.Value.Item1 == 7 || setResult.Value.Item2 == 7)
                        {
                            tieBreaksPlayed++;
                            //If he is the winner => the first number in the set result is the number of games he won =>
                            //If this number is bigger than the second he has won the set => we check if the other 1 has more than 4 games
                            //if it's not bigger he lost => check if he has won more than 4 games

                            //else if he is not the winner his numbers are the second item and check the same thing
                            if (winnerId == id)
                            {
                                if (setResult.Value.Item1 == 7)
                                {
                                    tieBreaksWon++;
                                }
                            }
                            else
                            {
                                if (setResult.Value.Item2 == 7)
                                {
                                    tieBreaksWon++;
                                }
                            }
                        }

                        allSetsPlayed++;
                    }
                }
            });

            this.playerStats[id].setsPlayed[court] = allSetsPlayed;

            return new Tuple<double, double>(allSetsPlayed > 0 ? tieBreaksPlayed / allSetsPlayed : 0,
               tieBreaksPlayed>0 ? tieBreaksWon / tieBreaksPlayed : 0);
        }

        //Returns the avrg per set number for specific value key from the table. Used for aces, doublefaults and etc.
        private double getValueAvrgPerSet(int id, Enums.CourtTypes court, DefaultTableKeys valueKey)
        {
            double numOfValues = 0;
            List<SqlDataReader> allMatches = this.executeQuery(String.Format("SELECT w_{0}, l_{0}, winner_id FROM {1} WHERE (winner_id = {2} OR loser_id = {2}) AND surface = {3}", valueKey.Value, "{0}", id, courtTypesKeys[court]));

            allMatches.ForEach((matchesBySeason) =>
            {
                while (matchesBySeason.Read())
                {
                    double winnerId = matchesBySeason.GetDouble(2);
                    double valueToAdd = 0;

                    if (winnerId == id)
                    {
                        try
                        {
                            valueToAdd = matchesBySeason.GetDouble(0);
                        }
                        catch (SqlNullValueException p)
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            valueToAdd = matchesBySeason.GetDouble(1);
                        }
                        catch (SqlNullValueException p)
                        {

                        }
                    }
                    numOfValues += valueToAdd;
                }
            });

            return this.playerStats[id].setsPlayed[court] > 0 ? numOfValues / this.playerStats[id].setsPlayed[court] : 0;
        }

        //Returns the ratios of winning when serving 1st serve and ratios of winning when playing vs 1st serv
        //Item 1: Ratio of 1st serve won
        //Item 2: Ratio of winning vs 1st Serve
        private Tuple<double, double> getRatioOfWinningVs1stServe(int id, Enums.CourtTypes court)
        {
            List<SqlDataReader> allMatches = this.executeQuery(String.Format("SELECT winner_id, w_1stIn, w_1stWon, l_1stIn, l_1stWon FROM {0} WHERE (winner_id = {1} OR loser_id = {1}) AND surface = {2}", "{0}", id, courtTypesKeys[court]));

            double firstServesPlayedAgaintsSum = 0;
            double firstServesPlayedAgaintsWonSum = 0;

            double firstServesSum = 0;
            double firstServesWonSum = 0;

            allMatches.ForEach((matchesBySeason) =>
            {
                while (matchesBySeason.Read())
                {
                    double winner_id = matchesBySeason.GetDouble(0);
                    if (winner_id == id)
                    {
                        try
                        {
                            double firstServesServed = matchesBySeason.GetDouble(1);
                            double firstServesWon = matchesBySeason.GetDouble(2);
                            firstServesSum += firstServesServed;
                            firstServesWonSum += firstServesWon;

                            double srvsPlayedVs = matchesBySeason.GetDouble(3);
                            double opponentWon = matchesBySeason.GetDouble(4);
                            firstServesPlayedAgaintsSum += srvsPlayedVs;
                            firstServesPlayedAgaintsWonSum += (srvsPlayedVs - opponentWon);
                        }
                        catch (SqlNullValueException p)
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            double firstServesServed = matchesBySeason.GetDouble(3);
                            double firstServesWon = matchesBySeason.GetDouble(4);
                            firstServesSum += firstServesServed;
                            firstServesWonSum += firstServesWon;

                            double srvsPlayedVs = matchesBySeason.GetDouble(1);
                            double opponentWon = matchesBySeason.GetDouble(2);
                            firstServesPlayedAgaintsSum += srvsPlayedVs;
                            firstServesPlayedAgaintsWonSum += (srvsPlayedVs - opponentWon);
                        }
                        catch (SqlNullValueException p)
                        {

                        }
                    }
                }
            });

            return new Tuple<double, double>(firstServesSum > 0 ? firstServesWonSum / firstServesSum : 0,
                firstServesPlayedAgaintsSum>0 ? firstServesPlayedAgaintsWonSum / firstServesPlayedAgaintsSum : 0);
        }

        //The same as for the first serv but for the second. There is some difference in the query.
        private Tuple<double, double> getRatioOfWinningVs2ndServe(int id, Enums.CourtTypes court)
        {
            List<SqlDataReader> allMatches = this.executeQuery(String.Format("SELECT winner_id, (w_svpt- w_df - w_1stIn), w_2ndWon, (l_svpt- l_df - l_1stIn), l_2ndWon FROM {0} WHERE (winner_id = {1} OR loser_id = {1}) AND surface = {2}", "{0}", id, courtTypesKeys[court]));

            double secondServesPlayedAgaintsSum = 0;
            double secondServesPlayedAgaintsWonSum = 0;

            double secondServesSum = 0;
            double secondServesWonSum = 0;

            allMatches.ForEach((matchesBySeason) =>
            {
                while (matchesBySeason.Read())
                {
                    double winnerID = matchesBySeason.GetDouble(0);
                    if (winnerID == id)
                    {
                        try
                        {
                            double secondServesServed = matchesBySeason.GetDouble(1);
                            double secondServesWon = matchesBySeason.GetDouble(2);
                            secondServesSum += secondServesServed;
                            secondServesWonSum += secondServesWon;

                            double srvsPlayedVs = matchesBySeason.GetDouble(3);
                            double opponentWon = matchesBySeason.GetDouble(4);
                            secondServesPlayedAgaintsSum += srvsPlayedVs;
                            secondServesPlayedAgaintsWonSum += (srvsPlayedVs - opponentWon);
                        }
                        catch (SqlNullValueException p)
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            double secondServesServed = matchesBySeason.GetDouble(3);
                            double secondServesWon = matchesBySeason.GetDouble(4);
                            secondServesSum += secondServesServed;
                            secondServesWonSum += secondServesWon;

                            double srvsPlayedVs = matchesBySeason.GetDouble(1);
                            double opponentWon = matchesBySeason.GetDouble(2);
                            secondServesPlayedAgaintsSum += srvsPlayedVs;
                            secondServesPlayedAgaintsWonSum += (srvsPlayedVs - opponentWon);
                        }
                        catch (SqlNullValueException p)
                        {

                        }
                    }
                }
            });

            return new Tuple<double, double>(secondServesSum>0? secondServesWonSum / secondServesSum : 0,
                secondServesPlayedAgaintsSum>0? secondServesPlayedAgaintsWonSum / secondServesPlayedAgaintsSum : 0);
        }

        //Returns how much break points per set avrgs
        //Item 1 break points made
        //Item 2 break points faced
        private Tuple<double, double> getBreakPointsAvrgPerSet(int id, Enums.CourtTypes court)
        {
            List<SqlDataReader> allMatches = this.executeQuery(String.Format("SELECT winner_id, w_bpFaced, l_bpFaced FROM {0} WHERE (winner_id = {1} OR loser_id = {1}) AND surface = {2}", "{0}", id, courtTypesKeys[court]));

            double bpMadeSum = 0;
            double bpFacedSum = 0;

            allMatches.ForEach((matchesBySeason) =>
            {
                while (matchesBySeason.Read())
                {
                    double winnerId = matchesBySeason.GetDouble(0);
                    if (winnerId == id)
                    {
                        try
                        {
                            bpMadeSum += matchesBySeason.GetDouble(1);
                            bpFacedSum += matchesBySeason.GetDouble(2);
                        }
                        catch (SqlNullValueException p)
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            bpFacedSum += matchesBySeason.GetDouble(1);
                            bpMadeSum += matchesBySeason.GetDouble(2);
                        }
                        catch (SqlNullValueException p)
                        {
                        }
                    }
                }
            });

            return new Tuple<double, double>(this.playerStats[id].setsPlayed[court] > 0 ? bpMadeSum / this.playerStats[id].setsPlayed[court] : 0,
               this.playerStats[id].setsPlayed[court] > 0 ? bpFacedSum / this.playerStats[id].setsPlayed[court] : 0);
        }

        //Returns tuple of break points ratios
        //Item 1 the ratio of bp won
        //Item 2 the ratio of bp saved
        private Tuple<double, double> getBreakPointsRatios(int id, Enums.CourtTypes court)
        {
            List<SqlDataReader> allMatches = this.executeQuery(String.Format("SELECT winner_id, w_bpSaved, w_bpFaced, l_bpSaved, l_bpFaced FROM {0} WHERE (winner_id = {1} OR loser_id = {1}) AND surface = {2}", "{0}", id, courtTypesKeys[court]));

            double bpMadeSum = 0;
            double bpWonSum = 0;

            double bpFacedSum = 0;
            double bpSavedSum = 0;

            allMatches.ForEach((matchesBySeason) =>
            {
                while (matchesBySeason.Read())
                {
                    double winnerId = matchesBySeason.GetDouble(0);
                    if (winnerId == id)
                    {
                        try
                        {
                            bpMadeSum += matchesBySeason.GetDouble(4);
                            bpWonSum += matchesBySeason.GetDouble(4) - matchesBySeason.GetDouble(3);

                            bpFacedSum += matchesBySeason.GetDouble(2);
                            bpSavedSum += matchesBySeason.GetDouble(2) - matchesBySeason.GetDouble(1);
                        }
                        catch (SqlNullValueException p)
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            bpMadeSum += matchesBySeason.GetDouble(2);
                            bpWonSum += matchesBySeason.GetDouble(2) - matchesBySeason.GetDouble(1);

                            bpFacedSum += matchesBySeason.GetDouble(4);
                            bpSavedSum += matchesBySeason.GetDouble(4) - matchesBySeason.GetDouble(3);
                        }
                        catch (SqlNullValueException p)
                        {
                        }
                    }
                }
            });

            return new Tuple<double, double>(bpWonSum > 0 ? bpMadeSum / bpWonSum : 0,
               bpFacedSum>0 ? bpSavedSum / bpFacedSum : 0);
        }

        private double getDonutRatio(int id, Enums.CourtTypes court)
        {
            List<SqlDataReader> allMatches = this.executeQuery(String.Format("SELECT score, winner_id FROM {0} WHERE (winner_id = {1} OR loser_id = {1}) AND surface = {2}", "{0}", id, courtTypesKeys[court]));
            double donuts = 0;

            allMatches.ForEach((matchesBySeason) =>
            {
                while (matchesBySeason.Read())
                {
                    String score = matchesBySeason.GetString(0);
                    double winnerId = matchesBySeason.GetDouble(1);

                    var setBySet = getMatchResultSetBySet(score);

                    foreach (var setResult in setBySet)
                    {
                        if (winnerId == id)
                        {
                            if (setResult.Value.Item1 == 0)
                            {
                                donuts++;
                            }
                        }
                        else
                        {
                            if (setResult.Value.Item2 == 0)
                            {
                                donuts++;
                            }
                        }
                    }
                }
            });

            return this.playerStats[id].setsPlayed[court] > 0 ? donuts / this.playerStats[id].setsPlayed[court] : 0;
        }

        private double getFirstServeInRatio(int id, Enums.CourtTypes court)
        {
            List<SqlDataReader> allMatches = this.executeQuery(String.Format("SELECT winner_id, w_svpt, w_1stIn, l_svpt, l_1stIn FROM {0} WHERE (winner_id = {1} OR loser_id = {1}) AND surface = {2}", "{0}", id, courtTypesKeys[court]));
            double svPoints = 0;
            double firstServerIn = 0;

            allMatches.ForEach((matchesBySeason) =>
            {
                while (matchesBySeason.Read())
                {
                    double winnerId = matchesBySeason.GetDouble(0);

                    if (winnerId == id)
                    {
                        try
                        {
                            svPoints += matchesBySeason.GetDouble(1);
                            firstServerIn += matchesBySeason.GetDouble(2);
                        }
                        catch (SqlNullValueException p)
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            svPoints += matchesBySeason.GetDouble(3);
                            firstServerIn += matchesBySeason.GetDouble(4);
                        }
                        catch (SqlNullValueException p)
                        {
                        }
                    }
                }
            });

            return svPoints > 0 ? firstServerIn / svPoints : 0;
        }

        public List<TennisPlayer> execute()
        {
            List<TennisPlayer> playerReaded = new List<TennisPlayer>();
            connection.Open();
            foreach (int playerID in playersIDs)
            {
                this.playerStats.Add(playerID, new PlayerDynamicStats());

                //Winning match rates
                List<double> rates = this.calculateAllWinRates(playerID);

                Dictionary<Enums.CourtTypes, double> straight3WinsRatesDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, this.getStraightSetsWin(playerID, DefaultTableKeys.WinnerID, 3,Enums.CourtTypes.CLAY) },
                    {Enums.CourtTypes.HARD, this.getStraightSetsWin(playerID, DefaultTableKeys.WinnerID, 3,Enums.CourtTypes.HARD) },
                    {Enums.CourtTypes.GRASS, this.getStraightSetsWin(playerID, DefaultTableKeys.WinnerID, 3,Enums.CourtTypes.GRASS) },
                };

                Dictionary<Enums.CourtTypes, double> straight5WinsRatesDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, this.getStraightSetsWin(playerID, DefaultTableKeys.WinnerID, 5,Enums.CourtTypes.CLAY) },
                    {Enums.CourtTypes.HARD, this.getStraightSetsWin(playerID, DefaultTableKeys.WinnerID, 5,Enums.CourtTypes.HARD) },
                    {Enums.CourtTypes.GRASS, this.getStraightSetsWin(playerID, DefaultTableKeys.WinnerID, 5,Enums.CourtTypes.GRASS) },
                };

                Dictionary<Enums.CourtTypes, double> straight3LosesRatesDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, this.getStraightSetsWin(playerID, DefaultTableKeys.LoserID, 3,Enums.CourtTypes.CLAY) },
                    {Enums.CourtTypes.HARD, this.getStraightSetsWin(playerID, DefaultTableKeys.LoserID, 3,Enums.CourtTypes.HARD) },
                    {Enums.CourtTypes.GRASS, this.getStraightSetsWin(playerID, DefaultTableKeys.LoserID, 3,Enums.CourtTypes.GRASS) },
                };

                Dictionary<Enums.CourtTypes, double> straight5LosesRatesDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, this.getStraightSetsWin(playerID, DefaultTableKeys.LoserID, 5,Enums.CourtTypes.CLAY) },
                    {Enums.CourtTypes.HARD, this.getStraightSetsWin(playerID, DefaultTableKeys.LoserID, 5,Enums.CourtTypes.HARD) },
                    {Enums.CourtTypes.GRASS, this.getStraightSetsWin(playerID, DefaultTableKeys.LoserID, 5,Enums.CourtTypes.GRASS) },
                };

                var moreThan4GamesRatesClay = getAtLeast4GamesWon(playerID, Enums.CourtTypes.CLAY);
                var moreThan4GamesRatesHard = getAtLeast4GamesWon(playerID, Enums.CourtTypes.HARD);
                var moreThan4GamesRatesGrass = getAtLeast4GamesWon(playerID, Enums.CourtTypes.GRASS);

                Dictionary<Enums.CourtTypes, double> letOpponentWin4GamesDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, moreThan4GamesRatesClay.Item1 },
                    {Enums.CourtTypes.HARD, moreThan4GamesRatesHard.Item1 },
                    {Enums.CourtTypes.GRASS, moreThan4GamesRatesHard.Item1 },
                };

                Dictionary<Enums.CourtTypes, double> win4GamesWhenLostSetDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, moreThan4GamesRatesClay.Item2 },
                    {Enums.CourtTypes.HARD, moreThan4GamesRatesHard.Item2 },
                    {Enums.CourtTypes.GRASS, moreThan4GamesRatesHard.Item2 },
                };

                var tieBreakRatiosClay = getTieBreakRate(playerID, Enums.CourtTypes.CLAY);
                var tieBreakRatiosHard = getTieBreakRate(playerID, Enums.CourtTypes.HARD);
                var tieBreakRatiosGrass = getTieBreakRate(playerID, Enums.CourtTypes.GRASS);

                Dictionary<Enums.CourtTypes, double> tieBreakPlayRatioDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, tieBreakRatiosClay.Item1 },
                    {Enums.CourtTypes.HARD, tieBreakRatiosHard.Item1 },
                    {Enums.CourtTypes.GRASS, tieBreakRatiosGrass.Item1 },
                };

                Dictionary<Enums.CourtTypes, double> tieBreakWinRatioDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, tieBreakRatiosClay.Item2 },
                    {Enums.CourtTypes.HARD, tieBreakRatiosHard.Item2 },
                    {Enums.CourtTypes.GRASS, tieBreakRatiosGrass.Item2 },
                };

                var acesAvrgPerSetClay = getValueAvrgPerSet(playerID, Enums.CourtTypes.CLAY, DefaultTableKeys.Aces);
                var acesAvrgPerSetHard = getValueAvrgPerSet(playerID, Enums.CourtTypes.HARD, DefaultTableKeys.Aces);
                var acesAvrgPerSetGrass = getValueAvrgPerSet(playerID, Enums.CourtTypes.GRASS, DefaultTableKeys.Aces);

                Dictionary<Enums.CourtTypes, double> acesAvrgPerSetDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, acesAvrgPerSetClay },
                    {Enums.CourtTypes.HARD, acesAvrgPerSetHard },
                    {Enums.CourtTypes.GRASS, acesAvrgPerSetGrass },
                };

                var dfAvrgPerSetClay = getValueAvrgPerSet(playerID, Enums.CourtTypes.CLAY, DefaultTableKeys.DoubleFaults);
                var dfAvrgPerSetHard = getValueAvrgPerSet(playerID, Enums.CourtTypes.HARD, DefaultTableKeys.DoubleFaults);
                var dfAvrgPerSetGrass = getValueAvrgPerSet(playerID, Enums.CourtTypes.GRASS, DefaultTableKeys.DoubleFaults);

                Dictionary<Enums.CourtTypes, double> dfAvrgPerSetDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, dfAvrgPerSetClay },
                    {Enums.CourtTypes.HARD, dfAvrgPerSetHard },
                    {Enums.CourtTypes.GRASS, dfAvrgPerSetGrass },
                };

                var pointsServedClay = getValueAvrgPerSet(playerID, Enums.CourtTypes.CLAY, DefaultTableKeys.Served);
                var pointsServedHard = getValueAvrgPerSet(playerID, Enums.CourtTypes.HARD, DefaultTableKeys.Served);
                var pointsServedGrass = getValueAvrgPerSet(playerID, Enums.CourtTypes.GRASS, DefaultTableKeys.Served);

                Dictionary<Enums.CourtTypes, double> pointsServedPerSetDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, pointsServedClay },
                    {Enums.CourtTypes.HARD, pointsServedHard },
                    {Enums.CourtTypes.GRASS, pointsServedGrass },
                };

                var fstSrvWinClay = getRatioOfWinningVs1stServe(playerID, Enums.CourtTypes.CLAY);
                var fstSrvWinHard = getRatioOfWinningVs1stServe(playerID, Enums.CourtTypes.HARD);
                var fstSrvWinGrass = getRatioOfWinningVs1stServe(playerID, Enums.CourtTypes.GRASS);

                Dictionary<Enums.CourtTypes, double> firstServeWinRatioDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, fstSrvWinClay.Item1 },
                    {Enums.CourtTypes.HARD, fstSrvWinHard.Item1 },
                    {Enums.CourtTypes.GRASS, fstSrvWinGrass.Item1 },
                };

                Dictionary<Enums.CourtTypes, double> wonVsFirstServeRatiosDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, fstSrvWinClay.Item2 },
                    {Enums.CourtTypes.HARD, fstSrvWinHard.Item2 },
                    {Enums.CourtTypes.GRASS, fstSrvWinGrass.Item2 },
                };

                var sndSrvClay = getRatioOfWinningVs2ndServe(playerID, Enums.CourtTypes.CLAY);
                var sndSrvHard = getRatioOfWinningVs2ndServe(playerID, Enums.CourtTypes.HARD);
                var sndSrvGrass = getRatioOfWinningVs2ndServe(playerID, Enums.CourtTypes.GRASS);

                Dictionary<Enums.CourtTypes, double> secondServeWinRatioDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, sndSrvClay.Item1 },
                    {Enums.CourtTypes.HARD, sndSrvHard.Item1 },
                    {Enums.CourtTypes.GRASS, sndSrvGrass.Item1 },
                };

                Dictionary<Enums.CourtTypes, double> wonVsSecondServeRatiosDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, sndSrvClay.Item2 },
                    {Enums.CourtTypes.HARD, sndSrvHard.Item2 },
                    {Enums.CourtTypes.GRASS, sndSrvGrass.Item2 },
                };

                var bpClay = getBreakPointsAvrgPerSet(playerID, Enums.CourtTypes.CLAY);
                var bpHard = getBreakPointsAvrgPerSet(playerID, Enums.CourtTypes.HARD);
                var bpGrass = getBreakPointsAvrgPerSet(playerID, Enums.CourtTypes.GRASS);

                Dictionary<Enums.CourtTypes, double> bpMadePerSetDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, bpClay.Item1 },
                    {Enums.CourtTypes.HARD, bpHard.Item1 },
                    {Enums.CourtTypes.GRASS, bpGrass.Item1 },
                };

                Dictionary<Enums.CourtTypes, double> bpFacedPerSetDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, bpClay.Item2 },
                    {Enums.CourtTypes.HARD, bpHard.Item2 },
                    {Enums.CourtTypes.GRASS, bpGrass.Item2 },
                };

                var bpRatesClay = getBreakPointsRatios(playerID, Enums.CourtTypes.CLAY);
                var bpRatesHard = getBreakPointsRatios(playerID, Enums.CourtTypes.HARD);
                var bpRatesGrass = getBreakPointsRatios(playerID, Enums.CourtTypes.GRASS);

                Dictionary<Enums.CourtTypes, double> bpWonRatesDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, bpRatesClay.Item1 },
                    {Enums.CourtTypes.HARD, bpRatesHard.Item1 },
                    {Enums.CourtTypes.GRASS, bpRatesGrass.Item1 },
                };

                Dictionary<Enums.CourtTypes, double> bpSavedRatesDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, bpRatesClay.Item2 },
                    {Enums.CourtTypes.HARD, bpRatesHard.Item2 },
                    {Enums.CourtTypes.GRASS, bpRatesGrass.Item2 },
                };

                var donutClayRatio = getDonutRatio(playerID, Enums.CourtTypes.CLAY);
                var donutHardRatio = getDonutRatio(playerID, Enums.CourtTypes.HARD);
                var donutGrassRatio = getDonutRatio(playerID, Enums.CourtTypes.GRASS);

                Dictionary<Enums.CourtTypes, double> donutRatiosDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, donutClayRatio },
                    {Enums.CourtTypes.HARD, donutHardRatio },
                    {Enums.CourtTypes.GRASS, donutGrassRatio },
                };

                var firstSvInClay = getFirstServeInRatio(playerID, Enums.CourtTypes.CLAY);
                var firstSvInHard = getFirstServeInRatio(playerID, Enums.CourtTypes.HARD);
                var firstSvInGrass = getFirstServeInRatio(playerID, Enums.CourtTypes.GRASS);

                Dictionary<Enums.CourtTypes, double> firstServeInRatioDic = new Dictionary<Enums.CourtTypes, double>
                {
                    {Enums.CourtTypes.CLAY, firstSvInClay },
                    {Enums.CourtTypes.HARD, firstSvInHard },
                    {Enums.CourtTypes.GRASS, firstSvInGrass },
                };

                playerReaded.Add(new TennisPlayer(playerID, 0, new WinRates(rates[3], rates[2], rates[0], rates[1]), new StraightSetsRates(straight3WinsRatesDic, straight3LosesRatesDic, straight5WinsRatesDic, straight5LosesRatesDic),
                    new GamesNumberRates(donutRatiosDic, letOpponentWin4GamesDic, win4GamesWhenLostSetDic), new TieBreakRates(tieBreakPlayRatioDic, tieBreakWinRatioDic),new SetAvrgs(pointsServedPerSetDic, acesAvrgPerSetDic, dfAvrgPerSetDic, bpMadePerSetDic, bpFacedPerSetDic), 
                    new ServeGameRates(firstServeInRatioDic, firstServeWinRatioDic, secondServeWinRatioDic), new ReturnGameRates(wonVsFirstServeRatiosDic, wonVsSecondServeRatiosDic), new BreakPointsRates(bpWonRatesDic, bpSavedRatesDic)
                    ));
            }

            connection.Close();

            return playerReaded;
        }

        //Returns the H2H stats for player 1 and player 2 depending on player 1 for last 10 matches and given type of court
        public H2HStats getH2HStats(int idPlayer1, int idPlayer2, Enums.CourtTypes courtType)
        {
            connection.Open();

            List<SqlDataReader> allMatchesWinnerPlayer1ThisCourt = this.executeQuery(String.Format("SELECT COUNT(*) FROM {0} WHERE (winner_id = {1} AND loser_id = {2}) AND surface = {3}", "{0}", idPlayer1, idPlayer2, courtTypesKeys[courtType]));
            List<SqlDataReader> allMatchesWinnerPlayer2ThisCourt = this.executeQuery(String.Format("SELECT COUNT(*) FROM {0} WHERE (winner_id = {2} AND loser_id = {1}) AND surface = {3}", "{0}", idPlayer1, idPlayer2, courtTypesKeys[courtType]));

            double sumWinsPlayer1 = 0;
            double sumWinsPlayer2 = 0;

            allMatchesWinnerPlayer1ThisCourt.ForEach((winsBySeason) =>
            {
                while (winsBySeason.Read())
                {
                    double winsThisSeason = winsBySeason.GetInt32(0);
                    sumWinsPlayer1 += winsThisSeason;
                }
            });

            allMatchesWinnerPlayer2ThisCourt.ForEach((winsBySeason) =>
            {
                while (winsBySeason.Read())
                {
                    double winsThisSeason = winsBySeason.GetInt32(0);
                    sumWinsPlayer2 += winsThisSeason;
                }
            });

            double winRateForThisCourt = (sumWinsPlayer1 + sumWinsPlayer2) > 0 ? sumWinsPlayer1 / (sumWinsPlayer1 + sumWinsPlayer2) : 0;


            List<SqlDataReader> allMatches = this.executeQuery(String.Format("SELECT winner_id FROM {0} WHERE (winner_id = {1} AND loser_id = {2}) OR (winner_id = {2} AND loser_id = {1})", "{0}", idPlayer1, idPlayer2));

            double sumAllWinsPlayer1 = 0;
            double sumAllWinsPlayer2 = 0;
            int counter = 0;
            allMatches.ForEach((matchBySeason) =>
            {
                while (matchBySeason.Read() && counter < 10)
                {
                    double winnerID = matchBySeason.GetDouble(0);
                    if (winnerID == idPlayer1)
                    {
                        sumAllWinsPlayer1++;
                    }
                    else
                    {
                        sumAllWinsPlayer2++;
                    }

                    counter++;
                }
            });

            double winRateLast10Matches = (sumAllWinsPlayer1 + sumAllWinsPlayer2) > 0 ? sumAllWinsPlayer1 / (sumAllWinsPlayer1 + sumAllWinsPlayer2) : 0;

            connection.Close();

            return new H2HStats(winRateForThisCourt, winRateLast10Matches);
        }
    }
}
