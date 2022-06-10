using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    /// <summary>
    /// 交易小节 结算标识
    /// 用于标注交易小节属于当前交易日还是下一个交易日
    /// </summary>
    public enum QSEnumRangeSettleFlag
    {
        /// <summary>
        /// 属于当前交易日
        /// </summary>
        T,
        /// <summary>
        /// 属于下一交易日
        /// </summary>
        T1
    }

    /// <summary>
    /// 交易小节接口
    /// </summary>
    public interface TradingRange
    {
        /// <summary>
        /// 结算标识
        /// </summary>
        QSEnumRangeSettleFlag SettleFlag { get; }
        /// <summary>
        /// 开始日
        /// </summary>
        DayOfWeek StartDay { get; }
        /// <summary>
        /// 结束日
        /// </summary>
        DayOfWeek EndDay { get; }
        /// <summary>
        /// 开始时间
        /// </summary>
        int StartTime { get; }
        /// <summary>
        /// 结束时间
        /// </summary>
        int EndTime { get; }

        /// <summary>
        /// 收盘时间段标识
        /// 用于标注在该交易小节 收盘
        /// </summary>
        bool MarketClose { get; }
    }
}
