using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;
using FantasyTennis;

namespace FantasyTennisGame
{
    static class Program
    {
        //Generates matches from given IDs, if an id is -1 => the player is null and the other 1 will pass free
        static List<Tuple<TennisDB.TennisPlayer, TennisDB.TennisPlayer>> generateMatches(List<int> ids, List<TennisDB.TennisPlayer> db)
        {
            List<Tuple<TennisDB.TennisPlayer, TennisDB.TennisPlayer>> returnList = new List<Tuple<TennisDB.TennisPlayer, TennisDB.TennisPlayer>>();
            int idCounter = 0;
            while (idCounter < ids.Count)
            {
                int id1 = ids.ElementAt(idCounter);
                int id2 = ids.ElementAt(idCounter + 1);
                TennisDB.TennisPlayer tp1 = null, tp2 = null;
                if (id1 != -1)
                {
                    tp1 = db.Find((player) =>
                    {
                        return player.id == ids.ElementAt(idCounter);
                    });
                }

                if (id2 != -1)
                {
                    tp2 = db.Find((player) =>
                    {
                        return player.id == ids.ElementAt(idCounter + 1);
                    });

                    returnList.Add(new Tuple<TennisDB.TennisPlayer, TennisDB.TennisPlayer>(tp1, tp2));
                    idCounter += 2;
                }
            }

            return returnList;
        }

        //Generates random players where it should in the list
        static List<int> generateRandomPlayers(List<int> ids, bool males = true)
        {
            Random rnd = new Random();
            var idsArray = ids.ToArray();
            for (int i = 0; i < ids.Count; i++)
            {
                if (idsArray[i] == -2)
                {
                    int rndNum = rnd.Next(males ? TennisDB.StaticData.AvrgPlayers.avrgIDsMales.Count : TennisDB.StaticData.AvrgPlayers.avrgIDsFemales.Count);
                    idsArray[i] = males ? TennisDB.StaticData.AvrgPlayers.avrgIDsMales.ElementAt(rndNum) : TennisDB.StaticData.AvrgPlayers.avrgIDsFemales.ElementAt(rndNum);
                }
            }

            return new List<int>(idsArray);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            TennisDB.DataReader dbMales = new TennisDB.DataReader();
            TennisDB.DataReader dbFemales = new TennisDB.DataReader(false);

            BinaryFormatter bf = new BinaryFormatter();

            //UNCOMMENT THIS SO YOU CAN MAKE A NEW PLAYER DB

            //FileStream fsMales = new FileStream("playersStatsMales.dat", FileMode.Create);
            //FileStream fsFemales = new FileStream("playersStatsFemales.dat", FileMode.Create);

            //var playersMales = dbMales.execute();
            //var playersFemales = dbFemales.execute();

            //bf.Serialize(fsMales, playersMales);
            //bf.Serialize(fsFemales, playersFemales);

            //----------------------------------------


            //COMMENT THIS WHEN YOU MAKE THE DB AND THEN JUST UNCOMMENT IT SO IT READS FROM THE FILE DIRECTLY

            FileStream fsMales = new FileStream("playersStatsMales.dat", FileMode.Open);
            FileStream fsFemales = new FileStream("playersStatsFemales.dat", FileMode.Open);

            var playersMales = (List<TennisDB.TennisPlayer>)bf.Deserialize(fsMales);
            var playersFemales = (List<TennisDB.TennisPlayer>)bf.Deserialize(fsFemales);

            //-------------------------------------------------------------------------

            List<int> idsMales = new List<int> {
                104918,-2, -2,-2, -2,-2, -2,-2, 104545,-2, -2,-2, -2,-2, -2,106298,
                104607,-2, -2,-2, -2,-2, -2,103819, -2,-2, -2,-2, -2,-2, -2,105453,
                104527,-2, -2,-2, -2,-2, -2,-2, 104655,-2, -2,-2, -2,-2, -2,106401,
                104542,-2, -2,-2, -2,-2, -2,106058, -2,-2, -2,-2, -2,-2, -2,105227,
                104792,-2, -2,-2, -2,-2, -2,-2, 100644,-2, -2,-2, -2,-2, -2,104745,
                105138,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,-2, 104180,-2, -2,105683,
                106233,-2, -2,-2, -2,-2, -2,-2, 103333,-2, -2,-2, -2,-2 ,-2,105676,
                105777,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,104925
            };
            idsMales = Program.generateRandomPlayers(idsMales);
            var listWithMatchesMales = Program.generateMatches(idsMales, playersMales);

            List<int> idsFemales = new List<int> {
                201493,-2, -2,-2, -2,203530, -2,-2, 214082,-2, -2,-2, -2,-2, -2,-2,
                201521,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,202469,
                201594,-2, -2,-2, -2,-2, -2,-2, 202428,-2, -2,-2, -2,-2, -2,200748,
                202494,-2, -2,-2, -2,-2, -2,-2, 201696,-2, -2,-2, -2,-2, -2,201320,
                201662,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,201490,
                201455,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,201474,
                201495,-2, -2,-2, -2,-2, -2,-2, 201496,-2, -2,-2, -2,211768, -2,202427,
                201366,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,200033

            };
            idsFemales = Program.generateRandomPlayers(idsFemales, false);
            var listWithMatchesFemales = Program.generateMatches(idsFemales, playersFemales);

            Dictionary<int, double> pointsPerId = new Dictionary<int, double>();

            int tournamentsNumber = 10;
            //Simulate all the tournaments
            for (int i = 0; i < tournamentsNumber; i++)
            {
                Tournament tournament = new Tournament(dbMales, playersMales, listWithMatchesMales, 60, 36, TennisDB.Enums.CourtTypes.HARD, true, true);
                Tournament tournamentFemales = new Tournament(dbFemales, playersFemales, listWithMatchesFemales, 60, 36, TennisDB.Enums.CourtTypes.HARD, true, false);

                List<Tournament> allTournaments = new List<Tournament>() {tournament, tournamentFemales };
                allTournaments.ForEach((tourn) =>
                {
                    tourn.simulateTournament();

                    foreach (var entry in tourn.playerPoints)
                    {
                        if (pointsPerId.ContainsKey(entry.Key))
                        {
                            pointsPerId[entry.Key] += entry.Value;
                        }
                        else
                        {
                            pointsPerId.Add(entry.Key, entry.Value);
                        }
                    }
                });
            }

            //make the results with avrg score
            var keys = new List<int>(pointsPerId.Keys);
            foreach (var key in keys)
            {
                pointsPerId[key] = pointsPerId[key] / tournamentsNumber;
            }

            var filteredPointsPerID = pointsPerId.Where((pair) =>
            {
                return !TennisDB.StaticData.AvrgPlayers.avrgIDsMales.Contains(pair.Key) && !TennisDB.StaticData.AvrgPlayers.avrgIDsFemales.Contains(pair.Key);
            });

            //show results
            foreach (var entry in filteredPointsPerID.OrderByDescending((pair) => pair.Value))
            {
                Console.WriteLine("{0} have won {1} Tournement points average, interesting indeed, much info, big funny.", TennisDB.StaticData.IDToName.iDToName(entry.Key), entry.Value);
            }

            GenericAlgorythm gen = new GenericAlgorythm(filteredPointsPerID, FantasyTennis.Enums.FantasyGames.FANTASY_LEAGUE);
            gen.populate();
            gen.runAlgorythm();

            Console.Write("The new max Value is: " + gen.population.Max.getSumValues() + "\n");
            Console.Write("With Price of: " + gen.population.Max.getSumPrice() + "\n");
            Console.Write("With Ids: \n");
            gen.population.Max.content.ForEach((item) =>
            {
                Console.Write(TennisDB.StaticData.IDToName.iDToName(item.id) + "\n");
            });

            //Tournament tournament = new Tournament(db, players, listWithMatches, 30, 16, TennisDB.Enums.Enums.CourtTypes.HARD, true, true);
            //tournament.simulateTournament();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1(tournament.draws,tournament.rounds));

            if (true)
            {

            }
        }
    }
}
