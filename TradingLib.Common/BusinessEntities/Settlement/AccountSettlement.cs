using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    /// <summary>
    /// 保证金交易类
    /// 期货 交易以持仓保证金来计算资金占用，持仓浮动盈亏以及平仓盈亏需要单独计算 用于体现账户权益变化
    /// 
    /// 证券交易类
    /// 股票 买入 卖出以证券的实际价格进行，买入资产后 账户资金减少，资产增加，卖出资产后 账户资金增加，资产减少。不用单独计算平仓盈亏与浮动盈亏
    /// 将买入，卖出资产的交易额单独计算差额即为账户资金的变化
    /// </summary>
    public class AccountSettlement
    {
        /// <summary>
        /// 结算日
        /// </summary>
        public int Settleday { get; set; }

        /// <summary>
        /// 交易帐户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 昨日权益
        /// </summary>
        public decimal LastEquity { get; set; }

        /// <summary>
        /// 昨日信用额度
        /// </summary>
        public decimal LastCredit { get; set; }


        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public decimal CloseProfitByDate { get; set; }

        /// <summary>
        /// 持仓盈亏
        /// </summary>
        public decimal PositionProfitByDate { get; set; }

        /// <summary>
        /// 资产买入金额
        /// </summary>
        public decimal AssetBuyAmount { get; set; }

        /// <summary>
        /// 资产卖出金额
        /// </summary>
        public decimal AssetSellAmount { get; set; }


        /// <summary>
        /// 手续费
        /// </summary>
        public decimal Commission { get; set; }

        /// <summary>
        /// 入金
        /// </summary>
        public decimal CashIn { get; set; }

        /// <summary>
        /// 出金
        /// </summary>
        public decimal CashOut { get; set; }

        /// <summary>
        /// 信用入金
        /// </summary>
        public decimal CreditCashIn { get; set; }

        /// <summary>
        /// 信用出金
        /// </summary>
        public decimal CreditCashOut { get; set; }


        /// <summary>
        /// 结算权益
        /// </summary>
        public decimal EquitySettled { get; set; }

        /// <summary>
        /// 结算信用额度
        /// </summary>
        public decimal CreditSettled { get; set; }
    }
}
