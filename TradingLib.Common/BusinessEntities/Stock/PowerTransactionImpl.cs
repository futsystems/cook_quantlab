using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.Common
{
    /// <summary>
    /// 除权操作
    /// </summary>
    public class PowerTransactionImpl:PowerTransaction
    {
        /// <summary>
        /// 结算日
        /// </summary>
        public int Settleday { get; set; }

        /// <summary>
        /// 交易账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }


        /// <summary>
        /// 持仓
        /// </summary>
        public int Size { get; set; }


        /// <summary>
        /// 分红金额
        /// </summary>
        public decimal Dividend { get; set; }


        /// <summary>
        /// 送/配 股数
        /// </summary>
        public int Shares { get; set; }


        /// <summary>
        /// 应支付金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
