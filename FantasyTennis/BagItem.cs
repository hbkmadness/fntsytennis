using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTennis
{
    class BagItem
    {
        public int id;
        public double value;
        public double price;

        public BagItem(int _id, double _value, double _price)
        {
            this.id = _id;
            this.value = _value;
            this.price = _price;
        }
    }
}
