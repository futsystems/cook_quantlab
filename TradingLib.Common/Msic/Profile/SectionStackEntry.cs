using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    public class SectionStackEntry
    {
        public DateTime TimeStarted;
        public SectionStackEntry(string name)
        {
            this.Name = name;
        }
        public string Name {get;set;}
    }


}
