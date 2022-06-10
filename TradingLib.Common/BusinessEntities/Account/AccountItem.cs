using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 交易账户对象
    /// 用于管理端与交易端加载
    /// </summary>
    public class AccountItem
    {
        /// <summary>
        /// 交易帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 交易帐户类别
        /// </summary>
        public QSEnumAccountCategory Category { get; set; }

        /// <summary>
        /// 委托路由类别
        /// </summary>
        public QSEnumOrderTransferType OrderRouteType { get; set; }

        /// <summary>
        /// 当前交易状态
        /// </summary>
        public bool Execute { get; set; }

        /// <summary>
        /// 日内交易
        /// </summary>
        public bool IntraDay { get; set; }

        /// <summary>
        /// 货币
        /// </summary>
        public CurrencyType Currency { get; set; }

        /// <summary>
        /// 上期权益
        /// </summary>
        public decimal LastEquity { get; set; }

        /// <summary>
        /// 当前权益
        /// </summary>
        public decimal NowEquity { get; set; }

        /// <summary>
        /// 信用额度
        /// 通过配资服务放出的信用额度
        /// </summary>
        public decimal Credit { get; set; }

        /// <summary>
        /// 平仓利润
        /// </summary>
        public decimal RealizedPL { get; set; }

        /// <summary>
        /// 未平仓利润
        /// </summary>
        public decimal UnRealizedPL { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public decimal Commission { get; set; }

        /// <summary>
        /// 净利
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// 入金
        /// </summary>
        public decimal CashIn { get; set; }

        /// <summary>
        /// 出金
        /// </summary>
        public decimal CashOut { get; set; }

        /// <summary>
        /// 总占用资金 = 个品种占用资金之和
        /// </summary>
        public decimal MoneyUsed { get; set; }

        /// <summary>
        /// 帐户标识
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 帐户所属管理员全局ID
        /// </summary>
        public int MGRID { get; set; }

        /// <summary>
        /// 是否已经删除
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// 路由组
        /// </summary>
        public int RG_ID { get; set; }

        /// <summary>
        /// 手续费模板ID
        /// </summary>
        public int Commissin_ID { get; set; }

        /// <summary>
        /// 保证金模板ID
        /// </summary>
        public int Margin_ID { get; set; }

        /// <summary>
        /// 交易参数模板
        /// </summary>
        public int ExStrategy_ID { get; set; }

        /// <summary>
        /// 是否处于登入状态
        /// </summary>
        public bool IsLogin { get; set; }

        /// <summary>
        /// 登入地址
        /// </summary>
        //public string SessionInfo { get; set; }


        /// <summary>
        /// 绑定的主帐户信息
        /// </summary>
        //public string ConnectorToken { get; set; }

        /// <summary>
        /// 绑定主帐户连接状态
        /// </summary>
        //public bool MAcctConnected { get; set; }

        /// <summary>
        /// 主帐户风控规则
        /// </summary>
        //public string MAcctRiskRule { get; set; }


        /// <summary>
        /// 警告状态
        /// </summary>
        public bool IsWarn { get; set; }

        /// <summary>
        /// 警告内容
        /// </summary>
        public string WarnMessage { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Memo { get; set; }

        public static string Serialize(AccountItem account)
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(account.Account);
            sb.Append(d);
            sb.Append(account.Category.ToString());
            sb.Append(d);
            sb.Append(account.OrderRouteType.ToString());
            sb.Append(d);
            sb.Append(account.Execute.ToString());
            sb.Append(d);
            sb.Append(account.IntraDay.ToString());
            sb.Append(d);
            sb.Append(account.LastEquity.ToString());
            sb.Append(d);
            sb.Append(account.NowEquity.ToString());
            sb.Append(d);
            sb.Append(account.RealizedPL.ToString());
            sb.Append(d);
            sb.Append(account.UnRealizedPL.ToString());
            sb.Append(d);
            sb.Append(account.Commission.ToString());
            sb.Append(d);
            sb.Append(account.Profit.ToString());
            sb.Append(d);
            sb.Append(account.CashIn.ToString());
            sb.Append(d);
            sb.Append(account.CashOut.ToString());
            sb.Append(d);
            sb.Append(account.MoneyUsed.ToString());
            sb.Append(d);
            sb.Append(account.Name);
            sb.Append(d);
            //sb.Append(account.Broker);
            sb.Append(d);
            //sb.Append(account.BankID);
            sb.Append(d);
            //sb.Append(account.BankAC);
            sb.Append(d);
            //sb.Append(account.PosLock.ToString());
            sb.Append(d);
            sb.Append(account.MGRID.ToString());
            sb.Append(d);
            sb.Append(account.Deleted.ToString());
            sb.Append(d);
            sb.Append(account.RG_ID);
            sb.Append(d);
            sb.Append(account.IsLogin);
            sb.Append(d);
            //sb.Append();
            sb.Append(d);
            //sb.Append("");
            sb.Append(d);
            sb.Append(account.Commissin_ID);
            sb.Append(d);
            sb.Append(account.Credit);
            sb.Append(d);
            //sb.Append(account.CreditSeparate);
            sb.Append(d);
            sb.Append(account.Margin_ID);
            sb.Append(d);
            sb.Append(account.ExStrategy_ID);
            sb.Append(d);
            //sb.Append(account.ConnectorToken);
            sb.Append(d);
            //sb.Append(account.MAcctConnected);
            sb.Append(d);
            //sb.Append(account.MAcctRiskRule);
            sb.Append(d);
            sb.Append(account.Currency);
            sb.Append(d);
            sb.Append(account.IsWarn);
            sb.Append(d);
            sb.Append(account.WarnMessage);
            sb.Append(d);
            sb.Append(account.Memo);


            return sb.ToString();
        }

        public static AccountItem Deserialize(string msg)
        {
            string[] rec = msg.Split(',');
            AccountItem account = new AccountItem();
            account.Account = rec[0];
            account.Category = (QSEnumAccountCategory)Enum.Parse(typeof(QSEnumAccountCategory), rec[1]);
            account.OrderRouteType = (QSEnumOrderTransferType)Enum.Parse(typeof(QSEnumOrderTransferType), rec[2]);
            account.Execute = bool.Parse(rec[3]);
            account.IntraDay = bool.Parse(rec[4]);
            account.LastEquity = decimal.Parse(rec[5]);
            account.NowEquity = decimal.Parse(rec[6]);
            account.RealizedPL = decimal.Parse(rec[7]);
            account.UnRealizedPL = decimal.Parse(rec[8]);
            account.Commission = decimal.Parse(rec[9]);
            account.Profit = decimal.Parse(rec[10]);
            account.CashIn = decimal.Parse(rec[11]);
            account.CashOut = decimal.Parse(rec[12]);
            account.MoneyUsed = decimal.Parse(rec[13]);
            account.Name = rec[14];
            //account.Broker = rec[15];
            //account.BankID = int.Parse(rec[16]);
            //account.BankAC = rec[17];
            //account.PosLock = bool.Parse(rec[18]);
            account.MGRID = int.Parse(rec[19]);
            account.Deleted = bool.Parse(rec[20]);
            account.RG_ID = int.Parse(rec[21]);
            account.IsLogin = bool.Parse(rec[22]);
            //account.SessionInfo = rec[23];
            //account.SideMargin = bool.Parse(rec[24]);
            account.Commissin_ID = int.Parse(rec[25]);
            account.Credit = decimal.Parse(rec[26]);
            //account.CreditSeparate = bool.Parse(rec[27]);
            account.Margin_ID = int.Parse(rec[28]);
            account.ExStrategy_ID = int.Parse(rec[29]);
            //account.ConnectorToken = rec[30];
            //account.MAcctConnected = bool.Parse(rec[31]);
            //account.MAcctRiskRule = rec[32];
            account.Currency = (CurrencyType)Enum.Parse(typeof(CurrencyType), rec[33]);
            account.IsWarn = bool.Parse(rec[34]);
            account.WarnMessage = rec[35];
            account.Memo = rec[36];

            return account;
        }
    }
}
