using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB.ComponentStatistics
{
    using DefaultMap = Dictionary<Enums.CourtTypes, double>;
    [Serializable]
    public class SetBySetStats5Sets
    {
        public DefaultMap winFirstSet;
        public DefaultMap winSecondSet_0_1;
        public DefaultMap winSecondSet_1_0;
        public DefaultMap winThirdSet_2_0;
        public DefaultMap winThirdSet_1_1;
        public DefaultMap winThirdSet_0_2;
        public DefaultMap winFourthSet_1_2;
        public DefaultMap winFourthSet_2_1;
        public DefaultMap winFifth_2_0;
        public DefaultMap winFifth_0_2;
        public DefaultMap winFifth;

        public SetBySetStats5Sets()
        {
        winFirstSet= new DefaultMap();
        winSecondSet_0_1= new DefaultMap();
        winSecondSet_1_0= new DefaultMap();
        winThirdSet_2_0= new DefaultMap();
        winThirdSet_1_1= new DefaultMap();
        winThirdSet_0_2= new DefaultMap();
        winFourthSet_1_2= new DefaultMap();
        winFourthSet_2_1= new DefaultMap();
        winFifth_2_0= new DefaultMap();
        winFifth_0_2= new DefaultMap();
        winFifth= new DefaultMap();
    }
    }
}
