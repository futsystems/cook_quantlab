using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    

    public class TradeCompare : IEqualityComparer<Trade>
    {
        /// <summary>
        /// 比较2个成交是否相同
        /// 交易帐户相同，交易编号相同,交易日起相同 则这个2个委托是相同的
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(Trade x, Trade y)
        {
            if ((x.Account == y.Account) && (x.TradeID == y.TradeID) && (x.xDate == y.xDate)) return true;
            return false;
        }

        public int GetHashCode(Trade obj)
        {
            return string.Format("{0}-{1}-{2}", obj.Account, obj.xDate, obj.TradeID).GetHashCode();
        }
    }
}
