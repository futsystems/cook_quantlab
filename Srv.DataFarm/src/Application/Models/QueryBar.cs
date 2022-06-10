using System;

namespace UniCryptoLab.Models
{
    public class QueryBar
    {
        public string Exchange { get; set; }

        public string Symbol { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int StartIndex { get; set; }

        public int MaxCount { get; set; }
    }
}