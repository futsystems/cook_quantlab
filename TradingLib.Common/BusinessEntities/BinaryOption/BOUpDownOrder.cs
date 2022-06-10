using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 上涨 下跌 二元期权
    /// </summary>
    public  class BOUpDownOrder:BinaryOptionOrderImpl
    {

        public BOUpDownOrder(string symbol,decimal amount,EnumBinaryOptionSideType side,EnumBinaryOptionTimeSpan ts)
            :base()
        {
            this.Symbol = symbol;
            this.Amount = amount;
            this.Side = side;
            this.TimeSpanType = ts;
            this.OptionType = EnumBinaryOptionType.CallPut;
        }
    }
}
