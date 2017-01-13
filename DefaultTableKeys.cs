using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB
{
    class DefaultTableKeys
    {
        private DefaultTableKeys(string value) { Value = value; }

        public string Value { get; set; }

        public static DefaultTableKeys WinnerID { get { return new DefaultTableKeys("winner_id"); } }
        public static DefaultTableKeys LoserID { get { return new DefaultTableKeys("loser_id"); } }
        public static DefaultTableKeys Aces { get { return new DefaultTableKeys("ace"); } }

        public static DefaultTableKeys DoubleFaults { get { return new DefaultTableKeys("df"); } }

        public static DefaultTableKeys Served { get { return new DefaultTableKeys("svpt"); } }
    }
}
