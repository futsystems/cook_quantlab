using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public static class Util_long
    {
        /// <summary>
        /// 将long类型的时间转换成DateTime
        /// 20160101123000
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeEx(this long dt, DateTime defaultDateTime)
        { 
            try
            {
                return Util.ToDateTime(dt);
            }
            catch(Exception ex)
            {
                return defaultDateTime;
            }
        }
    }
}
