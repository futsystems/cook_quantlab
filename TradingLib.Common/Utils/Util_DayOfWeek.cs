using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    public static class Util_DayOfWeek
    {
        /// <summary>
        /// 返回下一个Weekday
        /// 星期一->星期二
        /// 
        /// 星期六->星期日
        /// 星期日->星期一
        /// 
        /// 以此类推
        /// </summary>
        /// <param name="weekday"></param>
        /// <returns></returns>
        public static DayOfWeek NextWeekDay(this DayOfWeek weekday)
        {
            if (weekday == DayOfWeek.Saturday)
            {
                weekday = DayOfWeek.Sunday;
            }
            else
            {
                weekday += 1;
            }
            return weekday;
        }
    }
}
