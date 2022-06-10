using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface MarketTime
    {
        /// <summary>
        /// 时间段名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 时间段描述
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Range列表
        /// </summary>
        SortedDictionary<string, TradingRange> RangeList { get; }

        /// <summary>
        /// 获得当前交易小节
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        TradingRange JudgeRange(DateTime systime);


        /// <summary>
        /// 收盘时间 交易所不同的品种有不同的交易小节设定，品种的收盘时间可能不同
        /// </summary>
        int CloseTime { get; }
    }
}
