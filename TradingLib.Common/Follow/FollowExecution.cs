using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class FollowExecution
    {
        /// <summary>
        /// 结算日
        /// </summary>
        public int Settleday { get; set; }

        /// <summary>
        /// 策略ID
        /// </summary>
        public int StrategyID { get; set; }

        /// <summary>
        /// 跟单键
        /// </summary>
        public string FollowKey { get; set; }

        /// <summary>
        /// 初始信号Token
        /// </summary>
        public string SourceSignal { get; set; }

        /// <summary>
        /// 信号信息
        /// </summary>
        public string SignalInfo { get; set; }

        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }


        /// <summary>
        /// 交易方向
        /// </summary>
        public bool Side { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 开仓时间
        /// </summary>
        public long OpenTime { get; set; }

        /// <summary>
        /// 开仓均价
        /// </summary>
        public decimal OpenAvgPrice { get; set; }

        /// <summary>
        /// 开仓滑点
        /// </summary>
        public decimal OpenSlip { get; set; }


        /// <summary>
        /// 平仓时间
        /// </summary>
        public long CloseTime { get; set; }

        /// <summary>
        /// 平仓均价
        /// </summary>
        public decimal CloseAvgPrice { get; set; }

        /// <summary>
        /// 平仓滑点
        /// </summary>
        public decimal CloseSlip { get; set; }


        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public decimal RealizedPL { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public decimal Commission { get; set; }

        /// <summary>
        /// 盈利
        /// </summary>
        public decimal Profit { get; set; }

    }
}
