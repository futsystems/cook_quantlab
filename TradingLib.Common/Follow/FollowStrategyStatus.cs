using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 跟单策略统计
    /// </summary>
    public class FollowStrategyStatus
    {
        /// <summary>
        /// 跟单策略编号
        /// </summary>
        public int StrategyID { get; set; }

        /// <summary>
        /// 信号平仓盈亏
        /// </summary>
        public decimal SignalRealizedPL { get; set; }

        /// <summary>
        /// 信号浮动盈亏
        /// </summary>
        public decimal SignalUnRealizedPL { get; set; }

        /// <summary>
        /// 跟单平仓盈亏
        /// </summary>
        public decimal FollowRealizedPL { get; set; }

        /// <summary>
        /// 跟单浮动盈亏
        /// </summary>
        public decimal FollowUnRealizedPL { get; set; }


        /// <summary>
        /// 所有开仓跟单次数
        /// </summary>
        public int TotalEntryCount { get; set; }

        /// <summary>
        /// 所有开仓跟单次数（成功）
        /// </summary>
        public int TotalEntrySuccessCount { get; set; }

        /// <summary>
        /// 所有滑点数量
        /// </summary>
        public decimal TotalSlip { get; set; }

        /// <summary>
        /// 累计开仓滑点
        /// </summary>
        public decimal TotalEntrySlip { get; set; }

        /// <summary>
        /// 累计平仓滑点
        /// </summary>
        public decimal TotalExitSlip { get; set; }

        /// <summary>
        /// 信号源个数
        /// </summary>
        public int SignalCount { get; set; }


        /// <summary>
        /// 跟单策略工作状态
        /// </summary>
        public QSEnumFollowWorkState WorkState { get; set; }

    }
}
