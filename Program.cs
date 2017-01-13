using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FantasyTennis
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            //TennisPlayerDB db = new TennisPlayerDB();

            //TennisPlayer p1 = db.db[1];
            //TennisPlayer p2 = db.db[2];
            //TennisPlayer p3 = db.db[3];
            //TennisPlayer p4 = db.db[4];

            //Tuple<TennisPlayer,TennisPlayer> sf1 = new Tuple<TennisPlayer,TennisPlayer>(p1,p2);
            //Tuple<TennisPlayer,TennisPlayer> sf2 = new Tuple<TennisPlayer,TennisPlayer>(p3,p4);
            //List<Tuple<TennisPlayer,TennisPlayer>> list = new List<Tuple<TennisPlayer,TennisPlayer>>();
            //List<TennisPlayer> playerList = new List<TennisPlayer>();
            //playerList.Add(p1);
            //playerList.Add(p2);
            //playerList.Add(p3);
            //playerList.Add(p4);

            //list.Add(sf1);
            //list.Add(sf2);

            //Dictionary<int,int> dc = new Dictionary<int,int>{
            //{1,0},
            //{2,0},
            //{3,0},
            //{4,0}
            //};

            //for (int i = 0; i < 1000; i++)
            //{
            //    Tournament tournament = new Tournament(playerList, list, 2, 30, 16, CourtTypes.Grass, false, true);

            //    tournament.simulateTournament();

            //    dc[tournament.draws.root.winner.id]++;
            //}

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

            //var players = db.execute();
             
            if (true)
            {

            }
        }
    }
}
