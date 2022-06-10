using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    /// <summary>
    /// 持仓回报持仓标识
    /// 表面该持仓回报的持仓类型
    /// 
    /// </summary>
    public enum QSEnumPositionDirectionType
    {
        [Description("多")]//该持仓信息描述看多持仓
        Long=1,
        [Description("空")]//该持仓信息描述看空持仓
        Short=2,
        [Description("净")]//该持仓信息描述净持仓
        Net=3,
        [Description("双向")]//该持仓信息描述的可变双向持仓
        BothSide=4,
    }
}
