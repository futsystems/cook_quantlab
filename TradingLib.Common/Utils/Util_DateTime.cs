using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    public static class DateTimeUtils
    {
        const double DIFF_OA2JULIAND = 2415018.5;
        public static double ToJulianDate(this DateTime date)
        {
            return date.ToOADate().OADateToJulianDate();
        }


        
        public static double OADateToJulianDate(this double oadate)
        {
            return oadate + DIFF_OA2JULIAND;
        }


        public static double JulianDateToOADate(this double jdate)
        {
            return jdate - DIFF_OA2JULIAND;
        }

        /// <summary>
        /// 判断某个时间是否是工作日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsWorkDay(this DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                return false;
            return true;
        }

        /// <summary>
        /// 求某个时间的下一个工作日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime NextWorkDay(this DateTime dt)
        {
            //在当前日期上加一日
            DateTime workday = dt.Date.AddDays(1);
            //循环判断workday是否是节假日 如果不是则加一日
            while (true)
            {
                if (workday.IsWorkDay())
                {
                    return workday;
                }
                workday = workday.Date.AddDays(1);
            }
        }

        /// <summary>
        /// 求某个时间的上一个工作日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime LastWorkDay(this DateTime dt)
        {
            //在当前日期上加一日
            DateTime workday = dt.Date.AddDays(-1);
            //循环判断workday是否是节假日 如果不是则加一日
            while (true)
            {
                if (workday.IsWorkDay())
                {
                    return workday;
                }
                workday = workday.Date.AddDays(-1);
            }
        }

        public static int ToTLDate(this DateTime dt)
        {
            return Util.ToTLDate(dt);
        }

        public static int ToTLTime(this DateTime dt)
        {
            return Util.ToTLTime(dt);
        }

        public static long ToTLDateTime(this DateTime dt)
        {
            return Util.ToTLDateTime(dt);
        }



        /// <summary>
        /// 获得某个时间的第一天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this DateTime dt)
        {
            return dt.AddDays(1 - dt.Day);
        }

        /// <summary>
        /// 获得某个时间的最后一天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime dt)
        {
            return dt.AddDays(1 - dt.Day).AddMonths(1).AddDays(-1);
        }
        
        public static ulong ToTimeStamp(this DateTime dt)
        {
            var unixDateTime = (dt.ToUniversalTime() - DateTime.UnixEpoch).TotalMilliseconds;
            return (ulong)unixDateTime;
        }
        
        public static DateTime ToDateTime(this ulong timestamp)
        {
            return DateTime.UnixEpoch.AddMilliseconds(timestamp).ToLocalTime();
        }
    }
}
