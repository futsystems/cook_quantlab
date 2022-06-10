using System;
using TradingLib.API;


namespace UniCryptoLab.Models
{
    public class ReqAddSyncBarTask
    {
        public string Exchange { get; set; }

        public string Symbol { get; set; }

        //public BarInterval IntervalType { get; set; }

        //public int Interval { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
        
        
    }
}