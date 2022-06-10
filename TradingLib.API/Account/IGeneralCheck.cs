using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    /// <summary>
    /// 开仓委托资金检查
    /// 查询账户可下单最大数量
    /// </summary>
    public interface IGeneralCheck
    {
        /// <summary>
        /// 检查账户资金是否充足
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        bool CheckEquityAdequacy(Order order, out string msg);

        /// <summary>
        /// 查询账户可下单最大数量
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        int CheckMaxOrderSize(Symbol symbol,bool side,QSEnumOffsetFlag offset);
    }
}
