using System;
using TradingLib.API;

namespace UniCryptoLab.Models
{
    public class BarItem: IBarItem
    {
        public DateTime EndTime { get; set; }

        public string Symbol { get; set; }

        public int Interval { get; set; }

        public BarInterval IntervalType { get; set; }

        public double Open { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Close { get; set; }
        
        public double Volume { get; set; }

        public int TradeCount { get; set; }
    }
}