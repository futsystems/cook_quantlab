using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public static class SecurityUtil
    {

       
        /// <summary>
        /// 返回某天及以前多少个MarketDay 以map形式返回
        /// 指定时间 并返回该时间之前的一组MarketDay(可以包含指定时间当天的MarketDay)
        /// </summary>
        /// <param name="security"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Dictionary<int,MarketDay> GetMarketDays(this SecurityFamily security,DateTime end,int lastCnt)
        {
            Dictionary<DayOfWeek, List<TradingRange>> dayRangeMap = security.MarketTime.GetRangeOfWeekDay();
            DateTime date = end;
            Dictionary<int, MarketDay> mdmap = new Dictionary<int, MarketDay>();
            while (mdmap.Count < lastCnt)
            {
                DayOfWeek dayofweek = date.DayOfWeek;
                List<TradingRange> rangelist = null;
                //如果当前日期是交易日 则通过tradinglist 生成MarketDay
                if (dayRangeMap.TryGetValue(dayofweek, out rangelist))
                {
                    var item = MarketDay.CreateMarketDay(date, rangelist);
                    mdmap.Add(item.TradingDay,item);
                }
                date = date.AddDays(-1);
            }
            return mdmap;
        }

        /// <summary>
        /// 获得某天上一个交易日
        /// </summary>
        /// <param name="security"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static MarketDay GetLastMarketDay(this SecurityFamily security, DateTime date)
        {
            Dictionary<DayOfWeek, List<TradingRange>> dayRangeMap = security.MarketTime.GetRangeOfWeekDay();
           
            MarketDay lastmd = null;
            DateTime seek = date;
            while (lastmd == null || lastmd.TradingDay >= date.ToTLDate())
            {
                seek = seek.AddDays(-1);
                DayOfWeek dayofweek = seek.DayOfWeek;
                List<TradingRange> rangelist = null;
                if (dayRangeMap.TryGetValue(dayofweek, out rangelist))
                {
                    lastmd = MarketDay.CreateMarketDay(seek, rangelist);
                }
                
            }
            return lastmd;
        }

        /// <summary>
        /// 获得某天下一个交易日
        /// </summary>
        /// <param name="security"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static MarketDay GetNextMarketDay(this SecurityFamily security, DateTime date)
        {
            Dictionary<DayOfWeek, List<TradingRange>> dayRangeMap = security.MarketTime.GetRangeOfWeekDay();

            MarketDay nextmd = null;
            DateTime seek = date;
            while (nextmd == null || nextmd.TradingDay <= date.ToTLDate())
            {
                seek = seek.AddDays(1);
                DayOfWeek dayofweek = seek.DayOfWeek;
                List<TradingRange> rangelist = null;
                if (dayRangeMap.TryGetValue(dayofweek, out rangelist))
                {
                    nextmd = MarketDay.CreateMarketDay(seek, rangelist);
                }
                
            }
            return nextmd;
        }

        public static string GetSecurityName(this SecurityFamily sec)
        {
            if (sec != null)
            {
                return sec.Name;
            }
            return "未知";
        }

        public static int GetMultiple(this SecurityFamily sec)
        {
            if (sec != null)
            {
                return sec.Multiple;
            }
            return 1;
        }

        /// <summary>
        /// 获得PriceTick对应的格式化输出样式
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static string GetPriceFormat(this SecurityFamily sec)
        {
            decimal pricetick = sec.PriceTick;
            string[] p = pricetick.ToString().Split('.');
            if (p.Length <= 1)
                return "{0:F0}";
            else
                return "{0:F" + p[1].ToCharArray().Length.ToString() + "}";
        }

        /// <summary>
        /// 获得某个品种的小数位数
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static int GetDecimalPlaces(this SecurityFamily sec)
        {
            return sec.PriceTick.GetDecimalPlaces();
        }


        /// <summary>
        /// 生成合约
        /// </summary>
        /// <param name="sec"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string CreateFutureContract(this SecurityFamily sec,int year,int month)
        {
            
            //中国交易所用数字作为合约后缀
            if (sec.Exchange.Country == Country.CN && sec.Exchange.EXCode != "HKEX")//中国交易所 非香港交易所 合约按中国格式生成
            {
                string str = string.Format("{0}{1:D2}", year, month);
                if (sec.Exchange.EXCode.Equals("CZCE"))
                {
                    return sec.Code + str.ToString().Substring(3);
                }
                else
                {
                    return sec.Code + str.ToString().Substring(2);
                }
            }
            return string.Format("{0}{1}{2}", sec.Code, SymbolImpl.MonthNum2Letter(month.ToString("D2")), year.ToString().Substring(3, 1));
        }

    }
}
