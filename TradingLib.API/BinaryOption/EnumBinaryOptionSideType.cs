using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    /// <summary>
    /// 定义了二元期权方向类别
    /// </summary>
    public enum EnumBinaryOptionSideType
    {
        [Description("上涨")]
        Call,
        [Description("下跌")]
        Put,
        [Description("价上")]
        Above,
        [Description("价下")]
        Down,
        [Description("区间内")]
        RangeIn,
        [Description("区间外")]
        RangeOut,
        [Description("上触")]
        TouchUp,
        [Description("下触")]
        TouchDown,
        [Description("无上触")]
        NoTouchUp,
        [Description("无下触")]
        NoTouchDown,

    }
}
