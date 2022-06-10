using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface CashTransaction
    {
        /// <summary>
        /// 出入金编号
        /// </summary>
        string TxnID { get; set; }

        /// <summary>
        /// 交易帐户
        /// </summary>
        string Account { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        decimal Amount { get; set; }

        /// <summary>
        /// 交易类别
        /// </summary>
        QSEnumCashOperation TxnType { get; set; }


        /// <summary>
        /// 资金类别
        /// </summary>
        QSEnumEquityType EquityType { get; set; }

        /// <summary>
        /// 出入金备注 用于与其他系统编号建立关联
        /// </summary>
        string TxnRef { get; set; }

        /// <summary>
        /// 银行账户
        /// </summary>
        string BankAccount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        string Comment { get; set; }

        /// <summary>
        /// 结算日
        /// </summary>
        int Settleday { get; set; }

        /// <summary>
        /// 结算标识
        /// </summary>
        bool Settled { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        long DateTime { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        string Operator { get; set; }
    }
}
