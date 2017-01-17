using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyTennis
{
    class Bag
    {
        public List<BagItem> content;
        public int property = 0;

        public Bag()
        {
            content = new List<BagItem>(8);
        }

        public void addItem(BagItem item){
            if(content.Count < content.Capacity){
                bool contains = false;

                content.ForEach((bagItem) =>
                {
                    if (bagItem.id == item.id)
                    {
                        contains = true;
                    }
                });
                if (!contains)
                {
                    content.Add(item);
                }
            }
        }

        public double getSumValues()
        {
            double sum = 0;
            content.ForEach((item) =>
            {
                sum += item.value;
            });

            return sum;
        }

        public double getSumPrice()
        {
            double sum = 0;
            content.ForEach((item) =>
            {
                sum += item.price;
            });

            return sum;
        }
    }
}
