using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTennis
{
    class BagComparer : IComparer<Bag>
    {
        public int Compare(Bag b1, Bag b2)
        {
            var diff = b1.getSumValues() - b2.getSumValues();
            return diff == 0 ? 0 : (diff < 0 ? -1 : 1);
        }
    }
}
