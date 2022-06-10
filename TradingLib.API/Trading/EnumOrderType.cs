using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    public enum QSEnumOrderType
    {
        [Description("市价")]
        Market,
        [Description("限价")]
        Limit,
        [Description("突破")]
        Stop,
        [Description("突破限价")]
        StopLimit,//当价格突破某个价格时以某个limit价格触发委托
        [Description("跟踪止盈")]
        TrailingLimit,
        [Description("移动市价")]
        TrailingStop,//当价格从最高位回吐trailing后,触发该stop委托
    }


   

    

   

}
