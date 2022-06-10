using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    /// <summary>
    /// 仓位操作类型 开仓 平仓 加仓 减仓
    /// </summary>
    public enum QSEnumPosOperation
    {
        [Description("未记录")]
        UNKNOWN,
        [Description("开仓")]
        EntryPosition,//开仓
        [Description("加仓")]
        AddPosition,//加仓
        [Description("平仓")]
        ExitPosition,//平仓
        [Description("减仓")]
        DelPosition,//减仓
    }
}
