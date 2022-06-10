using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    public class SectionStats
    {
        // Fields
        public TimeSpan LocalTime;
        public long TimesCalled;
        public TimeSpan TotalTime;

        // Methods
        public SectionStats(string name)
        {
            this.Name = name;
            this.TimesCalled = 0;
            this.LocalTime = TimeSpan.Zero;
            this.TotalTime = TimeSpan.Zero;
        }

        // Properties
        public string Name { get; private set; }
    }
}
