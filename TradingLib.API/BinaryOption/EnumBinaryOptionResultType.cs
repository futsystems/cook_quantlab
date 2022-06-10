using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    public enum EnumBinaryOptionResultType
    {

        [Description("持有")]
        HOLD,
        [Description("盈利")]
        InTheMoney,
        [Description("亏损")]
        OutOfTheMoney,
    }
}
