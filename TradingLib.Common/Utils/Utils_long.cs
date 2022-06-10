using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    public static class Utils_long
    {
        public static DateTime ToDateTime(this long val)
        {
            return Util.ToDateTime(val);
        }
    }
}
