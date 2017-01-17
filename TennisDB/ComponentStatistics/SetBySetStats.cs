using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB.ComponentStatistics
{
    using DefaultMap = Dictionary<Enums.CourtTypes, double>;
    [Serializable]
    public class SetBySetStats
    {
        public readonly SetBySetStats3Sets max3SetsMatchStats;
        public readonly SetBySetStats5Sets max5SetsMatchStats;

        public SetBySetStats(SetBySetStats3Sets sbs3sets, SetBySetStats5Sets sbs5sets)
        {
            max3SetsMatchStats = sbs3sets;
            max5SetsMatchStats = sbs5sets;
        }
    }
}
