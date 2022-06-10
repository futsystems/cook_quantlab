using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class FrequencyNewBarEventArgs : System.EventArgs
    {
        /// <summary>
        /// 一个FreqKey - SigleBarEventArgs  Map
        /// </summary>
        public Dictionary<FrequencyManager.FreqKey, SingleBarEventArgs> FrequencyEvents
        {
            get;
            set;
        }

        public FrequencyNewBarEventArgs()
        {
            this.FrequencyEvents = new Dictionary<FrequencyManager.FreqKey, SingleBarEventArgs>();
        }
    }

    public class NewTickEventArgs : EventArgs
    {
        public Tick Tick { get; set; }

        public Bar PartialBar { get; set; }

        public Symbol Symbol { get; set; }

        public NewTickEventArgs(Symbol symbol, Tick k, Bar bar)
        {
            this.Symbol = symbol;
            this.Tick = k;
            this.PartialBar = bar;
        }
    }

    public class PartialBarUpdateEventArgs : EventArgs
    {
        public BarImpl PartialBar { get; set; }

        public Symbol Symbol { get; set; }

        public PartialBarUpdateEventArgs(Symbol symbol, BarImpl partialbar)
        {
            this.Symbol = symbol;
            this.PartialBar = partialbar;
        }
    }

    public class SingleBarEventArgs:EventArgs
    {
        public bool TickWereSent { get; set; }

        public DateTime BarEndTime { get; set; }

        public Bar Bar { get; set; }

        public Symbol Symbol { get; set; }

        public SingleBarEventArgs(Symbol symbol, Bar bar, DateTime barEndTime, bool tickSent)
        {
            this.Symbol = symbol;
            this.Bar = bar;
            this.BarEndTime = barEndTime;
            this.TickWereSent = tickSent;
        }
    }


}
