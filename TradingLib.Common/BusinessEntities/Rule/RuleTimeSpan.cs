using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    public class RuleTimeSpan
    {
        public int Start { get; set; }

        public int End { get; set; }


        public bool InSpan(int time)
        {
            if (time < Start) return false;
            if (time > End) return false;
            return true;
        }
        public static RuleTimeSpan Deserialize(string val)
        {
            try
            {
                string[] tmp = val.Split('-');
                RuleTimeSpan ts = new RuleTimeSpan();
                ts.Start = int.Parse(tmp[0]);
                ts.End = int.Parse(tmp[1]);
                return ts;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
