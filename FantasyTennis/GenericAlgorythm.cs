using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTennis
{
    class GenericAlgorythm
    {
        public static int ITEMS_PER_BAG = 8;
        public static double MAX_PRICE = 88.4;

        public int maxPopulation;

        public BagItemsStorage storage;
        public SortedSet<Bag> population;

        public GenericAlgorythm(IEnumerable<KeyValuePair<int, double>> playersStats, Enums.FantasyGames game)
        {
            this.population = new SortedSet<Bag>(new BagComparer());
            this.storage = new BagItemsStorage(playersStats, game);
            this.maxPopulation = 5;
        }

        public void populate()
        {
            Random rndmizer = new Random();
            long i = 0;
            for (i = 0; i < 1000; i++)
            {
                HashSet<int> indexes = new HashSet<int>();
                while (indexes.Count < GenericAlgorythm.ITEMS_PER_BAG)
                {
                    int newIndex = rndmizer.Next(this.storage.items.Count);
                    indexes.Add(newIndex);
                }

                Bag newBag = new Bag();
                foreach (int number in indexes)
                {
                    newBag.addItem(this.storage.items.ElementAt(number));
                }

                Bag lastItem = population.Min;
                if (newBag.getSumPrice() <= GenericAlgorythm.MAX_PRICE)
                {
                    if (lastItem == null || lastItem.getSumValues() < newBag.getSumValues())
                    {
                        population.Add(newBag);
                        if (population.Count > this.maxPopulation)
                        {
                            population.Remove(lastItem);
                        }
                    }
                }
            }

        }

        public void runAlgorythm()
        {
            Random rndmizer = new Random();
            for (long i = 0; i < 10; i++)
            {
                Bag bag1 = population.Max;
                Bag bag2 = population.ElementAt(rndmizer.Next(population.Count - 1) + 1);
                Bag newBag = new Bag();

                //make them together
                for (int j = 0; j < GenericAlgorythm.ITEMS_PER_BAG/2; j++)
                {
                    newBag.addItem(bag1.content.ElementAt(j));
                }

                for (int j = 0; j < bag2.content.Count; j++)
                {
                    BagItem item = bag2.content.ElementAt(j);
                    newBag.addItem(item);
                }

                if (newBag.content.Count < GenericAlgorythm.ITEMS_PER_BAG)
                {
                    continue;
                }

                //mutate
                int randomIndexToMutate = rndmizer.Next(GenericAlgorythm.ITEMS_PER_BAG);
                int randomTennisPlayerToAdd = rndmizer.Next(this.storage.items.Count);

                newBag.content.RemoveAt(randomIndexToMutate);
                newBag.addItem(this.storage.items.ElementAt(randomTennisPlayerToAdd));

                Bag minBag = this.population.Min;

                if (newBag.getSumPrice() <= GenericAlgorythm.MAX_PRICE && minBag.getSumValues() < newBag.getSumValues())
                {
                    bool added = this.population.Add(newBag);
                    if (population.Count > this.maxPopulation)
                    {
                        population.Remove(minBag);
                    }
                }

            }
        }
    }
}
