using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class BarDataV
    {
        Symbol _symbol;
        /// <summary>
        /// 合约
        /// </summary>
        public Symbol Symbol { get { return _symbol; } }


        BarFrequency _freq;
        /// <summary>
        /// 频率对象
        /// </summary>
        public BarFrequency BarFrequency { get { return _freq; } }

        public BarDataV(Symbol symbol, BarFrequency freq)
        {
            _symbol = symbol;
            _freq = freq;

            
        }
        SortedDictionary<long, decimal> open = new SortedDictionary<long, decimal>();

    }
}
