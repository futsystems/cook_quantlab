using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface ITimeSeries
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        DateTime TimeStamp { get; set; }
    }
}
