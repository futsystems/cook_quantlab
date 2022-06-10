using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    /// <summary>
    /// 委托单转发类型,转发到实盘交易接口或者模拟交易接口
    /// </summary>
    public enum QSEnumOrderTransferType
    {
        [Description("实盘")]
        LIVE,
        [Description("模拟")]
        SIM,
    }
}
