using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public enum  QSEnumHedgeFlag:byte
    {
        /// <summary>
        /// 套保
        /// </summary>
        Hedge=1,

        /// <summary>
        /// 逃离
        /// </summary>
        Arbitrage,

        /// <summary>
        /// 投机
        /// </summary>
        Speculation,

    }
}
