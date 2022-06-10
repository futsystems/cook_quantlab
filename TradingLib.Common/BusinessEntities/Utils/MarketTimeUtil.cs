using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public static class MarketTimeUtil
    {
        /// <summary>
        /// 获得WeekDay与TradingRange列表映射
        /// 将某个MarketTime的交易小节 按收盘WeekDay进行组织 用于生成MarketDay
        /// </summary>
        /// <param name="mt"></param>
        /// <returns></returns>
        public static Dictionary<DayOfWeek, List<TradingRange>> GetRangeOfWeekDay(this MarketTime mt)
        {
            Dictionary<DayOfWeek, List<TradingRange>> dayRangeMap = new Dictionary<DayOfWeek, List<TradingRange>>();
            //遍历所有收盘小节 有收盘的weekday就是有交易日的
            foreach (var range in mt.RangeList.Values.Where(rg => rg.MarketClose))
            {
                dayRangeMap.Add(range.EndDay, new List<TradingRange>());
            }

            //将交易小节放到交易日列表中
            //开始WeekDay 等于 结束WeekDay
            foreach (var range in mt.RangeList.Values)
            {
                if (range.SettleFlag == QSEnumRangeSettleFlag.T)
                {
                    dayRangeMap[range.StartDay].Add(range);
                }
                if (range.SettleFlag == QSEnumRangeSettleFlag.T1)
                {
                    DayOfWeek nextday = range.StartDay.NextWeekDay();
                    while (!dayRangeMap.Keys.Contains(nextday))
                    {
                        nextday = nextday.NextWeekDay();
                    }
                    dayRangeMap[nextday].Add(range);
                }


                //if (range.StartDay == range.EndDay)
                //{
                //    if (range.SettleFlag == QSEnumRangeSettleFlag.T)
                //    {
                //        dayRangeMap[range.StartDay].Add(range);
                //    }
                //    if (range.SettleFlag == QSEnumRangeSettleFlag.T1)
                //    {
                //        DayOfWeek nextday = range.StartDay.NextWeekDay();

                //        //if (range.StartDay == DayOfWeek.Saturday)
                //        //{
                //        //    nextday = DayOfWeek.Sunday;
                //        //}
                //        //else
                //        //{
                //        //    nextday = (range.StartDay + 1);
                //        //}
                //        while (!dayRangeMap.Keys.Contains(nextday))
                //        {
                //            nextday = nextday.NextWeekDay();
                //            //if (nextday == DayOfWeek.Saturday)
                //            //{
                //            //    nextday = DayOfWeek.Sunday;
                //            //}
                //            //else
                //            //{
                //            //    nextday += 1;
                //            //}
                //        }
                //        dayRangeMap[nextday].Add(range);
                //    }
                //}
                ////开始WeekDay 小于 结束WeekDay(当跨越了2个WeekDay则不可能是T,如果是T 表示明天交易日的交易 会进入第今天天)
                //else if (range.StartDay < range.EndDay)
                //{
                //    if (range.SettleFlag == QSEnumRangeSettleFlag.T1)
                //    {
                //        DayOfWeek nextday = range.StartDay.NextWeekDay();
                //        //if (range.StartDay == DayOfWeek.Saturday)
                //        //{
                //        //    nextday = DayOfWeek.Sunday;
                //        //}
                //        //else
                //        //{
                //        //    nextday = (range.StartDay + 1);
                //        //}
                //        while (!dayRangeMap.Keys.Contains(nextday))
                //        {
                //            nextday = nextday.NextWeekDay();
                //            //if (nextday == DayOfWeek.Saturday)
                //            //{
                //            //    nextday = DayOfWeek.Sunday;
                //            //}
                //            //else
                //            //{
                //            //    nextday += 1;
                //            //}
                //        }
                //        dayRangeMap[nextday].Add(range);
                //    }

                //    //当跨越了2个weekday 则不可能是T如果是T表示明天交易日的交易 会进入第jint天.
                //    //只有前一天的交易日 算入今天 没有明天的交易算入今天
                //}

            }

            return dayRangeMap;
        }
    }
}
