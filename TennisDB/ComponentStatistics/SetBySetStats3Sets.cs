using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDB.ComponentStatistics
{
    using DefaultMap = Dictionary<Enums.CourtTypes, double>;
    [Serializable]
    public class SetBySetStats3Sets
    {
        public DefaultMap match3SetsWinFirstSetRate;
        public DefaultMap match3SetsWinSecondSet_0_1_Rate;
        public DefaultMap match3SetsWinSecondSet_1_0_Rate;
        public DefaultMap match3SetsWinThirdSet_1_0_Rate;
        public DefaultMap match3SetsWinThirdSet_0_1_Rate;

        public SetBySetStats3Sets()
        {
            match3SetsWinFirstSetRate = new DefaultMap();
            match3SetsWinSecondSet_0_1_Rate = new DefaultMap();
            match3SetsWinSecondSet_1_0_Rate = new DefaultMap();
            match3SetsWinThirdSet_1_0_Rate = new DefaultMap();
            match3SetsWinThirdSet_0_1_Rate = new DefaultMap();
        }

        public SetBySetStats3Sets(DefaultMap _match3SetsWinFirstSetRate, DefaultMap _match3SetsWinSecondSet_0_1_Rate, DefaultMap _match3SetsWinSecondSet_1_0_Rate,
            DefaultMap _match3SetsWinThirdSet_1_0_Rate, DefaultMap _match3SetsWinThirdSet_0_1_Rate)
        {
            match3SetsWinFirstSetRate = _match3SetsWinFirstSetRate;
            match3SetsWinSecondSet_0_1_Rate = _match3SetsWinSecondSet_0_1_Rate;
            match3SetsWinSecondSet_1_0_Rate = _match3SetsWinSecondSet_1_0_Rate;
            match3SetsWinThirdSet_1_0_Rate = _match3SetsWinThirdSet_1_0_Rate;
            match3SetsWinThirdSet_0_1_Rate = _match3SetsWinThirdSet_0_1_Rate;
        }
    }
}
