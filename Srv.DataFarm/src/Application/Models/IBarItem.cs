using System;

namespace TradingLib.API
{
    public interface IBarItem
    {
        DateTime EndTime { get; set;}

        string Symbol { get; set; }

        int Interval { get; set; }

        BarInterval IntervalType { get; set; }

        double Open { get; set; }

        double High { get; set; }

        double Low { get; set; }

        double Close { get; set; }

        double Volume { get; set; }

        int TradeCount { get; set; }
        
    }
}