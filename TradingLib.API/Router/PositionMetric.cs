using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.API
{
    public interface PositionMetric
    {
        /// <summary>
        /// 合约
        /// </summary>
        string Symbol { get; }

        /// <summary>
        /// brokertoken
        /// </summary>
        string Token { get; }

        /// <summary>
        /// 多头持有仓位
        /// </summary>
        int LongHoldSize { get; }

        /// <summary>
        /// 空头持仓仓位
        /// </summary>
        int ShortHoldSize { get;}


        /// <summary>
        /// 多头待开数量
        /// </summary>
        int LongPendingEntrySize { get;}

        /// <summary>
        /// 多头待平数量
        /// </summary>
        int LongPendingExitSize { get;}

        /// <summary>
        /// 多方可以平掉的数量
        /// </summary>
        int LongCanExitSize { get; }

        /// <summary>
        /// 空头待开数量
        /// </summary>
        int ShortPendingEntrySize { get;}

        /// <summary>
        /// 空头待平数量
        /// </summary>
        int ShortPendingExitSize { get;}

        /// <summary>
        /// 空头可以平掉的数量
        /// </summary>
        int ShortCanExitSaize { get; }

    }
}
