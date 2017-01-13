using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTennis
{
    class BagItemsStorage
    {
        public int size;

        public List<BagItem> items;

        public BagItemsStorage()
        {
            this.items = new List<BagItem>();

            this.items.Add(new BagItem(2, 96.75, 24.02));
            this.items.Add(new BagItem(1, 91.12, 22.62));
            this.items.Add(new BagItem(3, 53.42, 13.26));
            this.items.Add(new BagItem(4, 50.9, 12.64));
            this.items.Add(new BagItem(5, 69.53, 12.08));
            this.items.Add(new BagItem(6, 47.43, 11.77));
            this.items.Add(new BagItem(7, 46.73, 11.6));
            this.items.Add(new BagItem(8, 43.98, 10.92));
            this.items.Add(new BagItem(9, 43.52, 10.8));
            this.items.Add(new BagItem(10, 41.45, 10.29));
            this.items.Add(new BagItem(11, 40.41, 10.03));
            this.items.Add(new BagItem(12, 38.48, 9.55));
            this.items.Add(new BagItem(13, 38.1, 9.46));
            this.items.Add(new BagItem(14, 37.99, 9.43));
            this.items.Add(new BagItem(15, 37.53, 9.32));
            this.items.Add(new BagItem(16, 35.98, 8.93));
            this.items.Add(new BagItem(17, 34.94, 8.67));
            this.items.Add(new BagItem(18, 34.93, 8.67));
            this.items.Add(new BagItem(19, 34.52, 8.57));
            this.items.Add(new BagItem(20, 32.6, 8.09));
            this.items.Add(new BagItem(21, 31.89, 7.92));
            this.items.Add(new BagItem(22, 31.26, 7.76));
            this.items.Add(new BagItem(23, 30.52, 7.58));
            this.items.Add(new BagItem(24, 30.2, 7.5));
            this.items.Add(new BagItem(25, 28.97, 7.19));
            this.items.Add(new BagItem(26, 109.71, 19.07));
            this.items.Add(new BagItem(27, 53.42, 13.26));
            this.items.Add(new BagItem(28, 50.27, 12.48));
            this.items.Add(new BagItem(29, 49.16, 12.2));
            this.items.Add(new BagItem(30, 44.94, 11.16));
            this.items.Add(new BagItem(31, 55.25, 10.97));
            this.items.Add(new BagItem(32, 43.89, 10.9));
            this.items.Add(new BagItem(33, 41.23, 10.23));
            this.items.Add(new BagItem(34, 39.26, 9.75));
            this.items.Add(new BagItem(35, 37.63, 9.34));
            this.items.Add(new BagItem(36, 37.14, 9.22));
            this.items.Add(new BagItem(37, 35.47, 8.8));
            this.items.Add(new BagItem(38, 34.37, 8.53));
            this.items.Add(new BagItem(39, 33.08, 8.21));
            this.items.Add(new BagItem(40, 32.91, 8.17));
            this.items.Add(new BagItem(41, 32.76, 8.13));
            this.items.Add(new BagItem(42, 31.8, 7.9));
            this.items.Add(new BagItem(43, 31.25, 7.76));
            this.items.Add(new BagItem(44, 30.61, 7.6));
            this.items.Add(new BagItem(45, 30.19, 7.5));
            this.items.Add(new BagItem(46, 30.16, 7.49));
            this.items.Add(new BagItem(47, 29.35, 7.29));
            this.items.Add(new BagItem(48, 29.27, 7.26));
            this.items.Add(new BagItem(49, 28.35, 7.04));
            this.items.Add(new BagItem(50, 27.74, 6.89));
        }
    }
}
