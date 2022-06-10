using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class BarRequest
    {
        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public int StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public int EndDate { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public int StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public int EndTime { get; set; }
        /// <summary>
        /// 间隔类别
        /// </summary>
        public BarInterval BarInterval { get; set; }
        
        /// <summary>
        /// 间隔数量
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 请求Bar数量
        /// </summary>
        public int Count { get; set; }

        public DateTime StartDateTime { get { return Util.ToDateTime(StartDate, StartTime); } }
        public DateTime EndDateTime { get { return Util.ToDateTime(EndDate, EndTime); } }

        public BarRequest()
        {
            this.Symbol = "";
            this.StartDate = 0;
            this.StartTime = 0;
            this.EndDate = 0;
            this.EndTime = 0;
            this.Count = 0;
            this.BarInterval = BarInterval.Minute;
            this.Interval = 0;
        }
        public BarRequest(string symbol,int startdate, int starttime, int enddate, int endtime,BarInterval barInterval,int interval=0)
        {
            this.Symbol = symbol;
            this.StartDate = startdate;
            this.StartTime = starttime;
            this.EndDate = enddate;
            this.EndTime = endtime;
            this.Count = 0;
            this.BarInterval = barInterval;
            this.Interval = interval;
            
            if (interval != 0)
            {
                switch (this.BarInterval)
                { 
                    case API.BarInterval.CustomTicks:
                    case API.BarInterval.CustomTime:
                    case API.BarInterval.CustomVol:
                        return;
                    default:
                        throw new ArgumentException("interval only work with CustomTicks,CustomTime,CustomVol");
                }
            }
        }

        public BarRequest(string symbol, int count, BarInterval barInterval, int interval=0)
        {
            this.Symbol = symbol;

            this.StartDate = 0;
            this.StartTime = 0;
            this.EndDate = 0;
            this.EndTime = 0;
            this.Count = count;

            this.BarInterval = barInterval;
            this.Interval = interval;

            if (interval != 0)
            {
                switch (this.BarInterval)
                {
                    case API.BarInterval.CustomTicks:
                    case API.BarInterval.CustomTime:
                    case API.BarInterval.CustomVol:
                        return;
                    default:
                        throw new ArgumentException("interval only work with CustomTicks,CustomTime,CustomVol");
                }
            }
        }


        public override string ToString()
        {
            return Symbol + " " + Interval + " " + StartDateTime + "->" + EndDateTime;
        }

    }

}
