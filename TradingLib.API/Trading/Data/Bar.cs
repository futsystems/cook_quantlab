using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface Bar
    {
        /// <summary>
        /// 合约
        /// </summary>
        string Symbol { get; set;}

        /// <summary>
        /// 最高价
        /// </summary>
        double High { get; set;}

        /// <summary>
        /// 最低价
        /// </summary>
        double Low { get; set; }

        /// <summary>
        /// 开盘价
        /// </summary>
        double Open { get; set; }

        /// <summary>
        /// 收盘价
        /// </summary>
        double Close { get; set; }

        /// <summary>
        /// 成交量
        /// </summary>
        double Volume { get; set; }

        /// <summary>
        /// 持仓量
        /// </summary>
        double OpenInterest { get; set; }

        /// <summary>
        /// Ask价
        /// </summary>
        double Ask { get; set; }

        /// <summary>
        /// Bid价
        /// </summary>
        double Bid { get; set; }

        /// <summary>
        /// 成交笔数
        /// </summary>
        int TradeCount { get; set; }

        /// <summary>
        /// Bar所处交易日
        /// </summary>
        int TradingDay { get; set; }

        /// <summary>
        /// Bar结束时间
        /// </summary>
        DateTime EndTime { get; set; }

        /// <summary>
        /// 频率类别
        /// </summary>
        BarInterval IntervalType { get; set; }

        /// <summary>
        /// 间隔数
        /// </summary>
        int Interval { get; set; }

        /// <summary>
        /// 复制一个Bar数据
        /// </summary>
        /// <returns></returns>
        Bar Clone();

        /// <summary>
        /// 该Bar是否为空
        /// </summary>
        bool EmptyBar { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum BarInterval
    {
        /// <summary>
        /// custom volume bars
        /// </summary>
        CustomVol = -3,
        /// <summary>
        /// custom tick bars
        /// </summary>
        CustomTicks = -2,
        /// <summary>
        /// custom interval length
        /// </summary>
        CustomTime = -1,
        //!!do not change the value of CustomTime,CustomTicks,CustomVol

        /// <summary>
        /// One-minute intervals
        /// </summary>
        Minute = 60,
        /// <summary>
        /// Thress-minute intervals
        /// </summary>
        ThreeMin = 180,
        /// <summary>
        /// Five-minute interval
        /// </summary>
        FiveMin = 300,
        /// <summary>
        /// FifteenMinute intervals
        /// </summary>
        FifteenMin = 900,
        /// <summary>
        /// Hour-long intervals
        /// </summary>
        ThirtyMin = 1800,
        /// <summary>
        /// Hour-long intervals
        /// </summary>
        Hour = 3600,
        /// <summary>
        /// Day-long intervals
        /// </summary>
        Day = 86400,
    }
}
