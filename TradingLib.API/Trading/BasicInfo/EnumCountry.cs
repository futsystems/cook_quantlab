using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;


namespace TradingLib.API
{
    public enum Country
    {
        [Description("中国")] 
        CN,
        [Description("美国")] 
        USA,
        [Description("英国")] 
        UK,
        [Description("新加坡")]
        SG,
        [Description("德国")]
        DE,
    }
}
