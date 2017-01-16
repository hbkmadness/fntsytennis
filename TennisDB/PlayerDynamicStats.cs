using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB
{
    class PlayerDynamicStats
    {
        public Dictionary<Enums.CourtTypes,double> setsPlayed;
        public PlayerDynamicStats()
        {

            this.setsPlayed = new Dictionary<Enums.CourtTypes, double>();

            foreach(Enums.CourtTypes type in Enum.GetValues(typeof(Enums.CourtTypes)))
            {
                this.setsPlayed.Add(type, 0);
            }
        }
    }
}
