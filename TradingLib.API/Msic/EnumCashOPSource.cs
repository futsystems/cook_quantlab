using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    public enum QSEnumCashOPSource
    {
        [Description("未知")]
        Unknown,
        [Description("线下汇款")]
        Manual,
        [Description("在线支付")]
        Online,
    }
}
