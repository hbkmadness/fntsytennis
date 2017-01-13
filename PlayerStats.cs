using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB
{
    class PlayerStats
    {
        public Dictionary<CourtTypes,double> setsPlayed;
        public PlayerStats()
        {

            this.setsPlayed = new Dictionary<CourtTypes, double>();

            foreach(CourtTypes type in Enum.GetValues(typeof(CourtTypes)))
            {
                this.setsPlayed.Add(type, 0);
            }
        }
    }
}
