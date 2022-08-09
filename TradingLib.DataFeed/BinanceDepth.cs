using System.Collections.Generic;

namespace TradingLib.DataFeed
{
    public class BinanceDepth
    {
        public long LastUpdateId { get; set; }

        public List<decimal[]> Bids { get; set; }
        
        public List<decimal[]> Asks { get; set; }

    }
    
    
}