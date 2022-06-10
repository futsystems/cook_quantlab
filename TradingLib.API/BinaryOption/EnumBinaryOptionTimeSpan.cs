using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    public enum EnumBinaryOptionTimeSpan
    {
        [Description("1分钟")]
        MIN1=1,
        [Description("2分钟")]
        MIN2=2,
        [Description("5分钟")]
        MIN5=5,
        [Description("10分钟")]
        MIN10=10,
        [Description("15分钟")]
        MIN15=15,
        [Description("30分钟")]
        MIN30=30,
        [Description("60分钟")]
        MIN60=60,
    }
}
