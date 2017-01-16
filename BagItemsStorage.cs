using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTennis
{
    class BagItemsStorage
    {
        public List<BagItem> items;

        public BagItemsStorage(IEnumerable<KeyValuePair<int, double>> playersStats, FantasyGames game)
        {
            this.items = new List<BagItem>();

            foreach (var playerEntry in playersStats)
            {
                if (game == FantasyGames.FANTASY_LEAGUE)
                {
                    this.items.Add(new BagItem(playerEntry.Key, playerEntry.Value,
                        TennisDB.IDToPrice.idToPrice(playerEntry.Key)));
                }
            }
        }
    }
}
