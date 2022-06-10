using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    /// <summary>
    /// 结算方式
    /// 按交易逐笔结算还是逐日结算
    /// </summary>
    public enum QSEnumSettleType
    {
        [Description("逐日")]
        ByDate,
        [Description("逐笔")]
        ByTrade,
    }
}
