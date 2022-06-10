using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.Common
{
    public class JsonObjectBase
    {

    }

    public static class JsonObjectBaseUtils
    {
        public static string ToJson(this JsonObjectBase obj)
        {
            return obj.SerializeObject();
        }
    }
}
