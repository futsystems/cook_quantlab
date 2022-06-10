using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 委托比较器
    /// 用于比较2个委托是否是相同的委托
    /// 从数据库加载数据时，默认的ORM加载逻辑是从tm_orders和log_orders进行加载
    /// 如果加载的日期已经进行过交易数据转储则会出现数据重复，这需要进行去重，用到委托比较
    /// </summary>
    public class OrderCompare : IEqualityComparer<Order>
    {


        /// <summary>
        /// 比较2个委托是否相同
        /// 交易帐户相同，交易编号相同,交易日起相同 则这个2个委托是相同的
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(Order x, Order y)
        {
            if ((x.Account == y.Account) && (x.id == y.id) && (x.Date == y.Date)) return true;
            return false;
        }

        public int GetHashCode(Order obj)
        {
            return string.Format("{0}-{1}-{2}", obj.Account, obj.Date, obj.id).GetHashCode();
        }
    }
}
