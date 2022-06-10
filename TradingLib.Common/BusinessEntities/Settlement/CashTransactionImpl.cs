using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class CashTransactionImpl:CashTransaction
    {
        public string TxnID { get; set; }
        /// <summary>
        /// 交易帐户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 交易类别
        /// </summary>
        public QSEnumCashOperation TxnType { get; set; }

        /// <summary>
        /// 资金类别
        /// </summary>
        public QSEnumEquityType EquityType { get; set; }

        /// <summary>
        /// 出入金备注 用于与其他系统编号建立关联
        /// </summary>
        public string TxnRef { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 结算日
        /// </summary>
        public int Settleday { get; set; }

        /// <summary>
        /// 结算标识
        /// </summary>
        public bool Settled { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public long DateTime { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 银行账户
        /// </summary>
        public string BankAccount { get; set; }

        public static string Serialize(CashTransaction c)
        {
            StringBuilder sb = new StringBuilder();
            char d =',';
            sb.Append(c.Account);
            sb.Append(d);
            sb.Append(c.Amount);
            sb.Append(d);
            sb.Append(c.TxnType);
            sb.Append(d);
            sb.Append(c.EquityType);
            sb.Append(d);
            sb.Append(c.TxnRef);
            sb.Append(d);
            sb.Append(c.Comment);
            sb.Append(d);
            sb.Append(c.Settleday);
            sb.Append(d);
            sb.Append(c.Settled);
            sb.Append(d);
            sb.Append(c.DateTime);
            sb.Append(d);
            sb.Append(c.Operator);
            sb.Append(d);
            sb.Append(c.TxnID);
            sb.Append(d);
            sb.Append(c.BankAccount);
            return sb.ToString();
        }

        public static CashTransaction Deserialize(string msg)
        {
            string[] rec = msg.Split(',');
            CashTransactionImpl s = new CashTransactionImpl();
            s.Account = rec[0];
            s.Amount =decimal.Parse(rec[1]);
            s.TxnType = (QSEnumCashOperation)Enum.Parse(typeof(QSEnumCashOperation),rec[2]);
            s.EquityType = (QSEnumEquityType)Enum.Parse(typeof(QSEnumEquityType),rec[3]);
            s.TxnRef = rec[4];
            s.Comment = rec[5];
            s.Settleday = int.Parse(rec[6]);
            s.Settled = bool.Parse(rec[7]);
            s.DateTime = long.Parse(rec[8]);
            s.Operator = rec[9];
            s.TxnID = rec[10];
            s.BankAccount = rec[11];
            return s;
        
        }
    }
}
