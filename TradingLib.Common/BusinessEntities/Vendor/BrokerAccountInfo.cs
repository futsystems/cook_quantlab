using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    public class BrokerAccountInfo
    {
        /// <summary>
        /// 昨日权益
        /// </summary>
        public decimal LastEquity { get; set; }

        /// <summary>
        /// 当日入金
        /// </summary>
        public decimal CashIn { get; set; }

        /// <summary>
        /// 当日出金
        /// </summary>
        public decimal CashOut { get; set; }

        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public decimal CloseProfit { get; set; }

        /// <summary>
        /// 持仓盈亏
        /// </summary>
        public decimal PositionProfit { get; set; }

        /// <summary>
        /// 当日手续费
        /// </summary>
        public decimal Commission { get; set; }

    }
}
