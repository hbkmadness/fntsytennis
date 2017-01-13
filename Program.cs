using FantasyTennisGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FantasyTennis
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
        static List<int> generateRandomPlayers(List<int> ids)
        {
            Random rnd = new Random();
            var idsArray = ids.ToArray();
            for (int i = 0; i < ids.Count; i++)
            {
                if(idsArray[i] == -2)
                {
                    int rndNum = rnd.Next(TennisDB.AvrgPlayers.avrgIDs.Count);
                    idsArray[i] = TennisDB.AvrgPlayers.avrgIDs.ElementAt(rndNum);
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
            //GenericAlgorythm gen = new GenericAlgorythm();
            //gen.populate();
            //gen.runAlgorythm();

            //Console.Write("The new max Value is: " + gen.population.Max.getSumValues() + "\n");
            //Console.Write("With Price of: " + gen.population.Max.getSumPrice() + "\n");
            //Console.Write("With Ids: \n");
            //gen.population.Max.content.ForEach((item) =>
            //{
            //    Console.Write(item.id + "\n");
            //});


            TennisDB.DataReader db = new TennisDB.DataReader();
            BinaryFormatter bf = new BinaryFormatter();

            //UNCOMMENT THIS SO YOU CAN MAKE A NEW PLAYER DB

            //FileStream fs = new FileStream("playersStats.dat", FileMode.Create);
            //var players = db.execute();
            //bf.Serialize(fs, players);

            //----------------------------------------


            //COMMENT THIS WHEN YOU MAKE THE DB AND THEN JUST UNCOMMENT IT SO IT READS FROM THE FILE DIRECTLY

            FileStream fs = new FileStream("playersStats.dat", FileMode.Open);
            var players = (List<TennisDB.TennisPlayer>)bf.Deserialize(fs);

            //-------------------------------------------------------------------------

            List<int> ids = new List<int> {
                104925,-2, -2,-2, -2,-2, -2,-2, 103333,-2, -2,-2, -2,-2, -2,-2,
                104542,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,105453,
                103819,-2, -2,-2, -2,-2, -2, 105777, 106233,-2, -2,-2, -2,-2, -2,105676,
                105227,-2, -2,-2, -2,-2, -2,105138, 106401,-2, -2,104655, -2,-2, -2,104607,
                104745,-2, -2,-2, -2,-2, -2,-2, 104792,-2, -2,-2, -2,-2, -2,-2,
                105683,106298, -2,-2, -2,-2, -2,-2, 106058,-2, -2,-2, -2,-2, -2,104527,
                -2,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,-2, -2,104545,
                -2,-2, -2,-2, -2,-2, 104180,-2, -2,-2, -2,-2, -2,-2, 100644,104918
            };
            ids = Program.generateRandomPlayers(ids);

            var listWithMatches = Program.generateMatches(ids, players);

            Dictionary<int, int> dc = new Dictionary<int, int>();

            for (int i = 0; i < 5000; i++)
            {
                Tournament tournament = new Tournament(db, players, listWithMatches, 30, 16, TennisDB.CourtTypes.HARD, true, true);

                tournament.simulateTournament();

                if (dc.ContainsKey(tournament.draws.root.winner.id))
                {
                    dc[tournament.draws.root.winner.id]++;
                }
                else
                {
                    dc.Add(tournament.draws.root.winner.id, 1);
                }
            }

            foreach (var entry in dc)
            {
                Console.WriteLine("{0} have won {1} Tournements, interesting indeed, much info, big funny.", TennisDB.IDToName.idToName[entry.Key], entry.Value);
            }

            //Tournament tournament = new Tournament(db, players, listWithMatches, 30, 16, TennisDB.CourtTypes.HARD, true, true);
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
