using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    /// <summary>
    /// 除权操作
    /// </summary>
    public interface PowerTransaction
    {
        /// <summary>
        /// 结算日
        /// </summary>
        int Settleday { get; set; }

        /// <summary>
        /// 交易账户
        /// </summary>
        string Account { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        string Symbol { get; set; }


        /// <summary>
        /// 持仓
        /// </summary>
        int Size { get; set; }


        /// <summary>
        /// 分红金额
        /// </summary>
        decimal Dividend { get; set; }


        /// <summary>
        /// 送/配 股数
        /// </summary>
        int Shares { get; set; }


        /// <summary>
        /// 应支付金额
        /// </summary>
        decimal Amount { get; set; }
    }
}
