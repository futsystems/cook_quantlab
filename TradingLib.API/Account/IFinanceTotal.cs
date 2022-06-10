using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    /// <summary>
    /// 帐户财务信息接口
    /// </summary>
    public interface IFinanceTotal
    {
        /// <summary>
        /// 上期权益
        /// </summary>
        decimal LastEquity { get; set; }

        /// <summary>
        /// 昨日优先资金
        /// </summary>
        decimal LastCredit { get; set; }

        /// <summary>
        /// 当前权益
        /// </summary>
        decimal NowEquity { get; }

        /// <summary>
        /// 信用金额
        /// </summary>
        decimal Credit { get; }

        /// <summary>
        /// 平仓利润
        /// </summary>
        decimal RealizedPL { get; }

        /// <summary>
        /// 未平仓利润
        /// </summary>
        decimal UnRealizedPL { get; }

        /// <summary>
        /// 手续费
        /// </summary>
        decimal Commission { get; }

        /// <summary>
        /// 净利
        /// </summary>
        decimal Profit { get; }

        /// <summary>
        /// 入金
        /// </summary>
        decimal CashIn { get; }

        /// <summary>
        /// 出金
        /// </summary>
        decimal CashOut { get; }

        /// <summary>
        /// 优先资金入金
        /// </summary>
        decimal CreditCashIn { get; }

        /// <summary>
        /// 优先资金出金
        /// </summary>
        decimal CreditCashOut { get; }

        /// <summary>
        /// 保证金占用
        /// </summary>
        decimal Margin { get;}

        /// <summary>
        /// 保证金冻结
        /// </summary>
        decimal MarginFrozen { get;}

        /// <summary>
        /// 总占用资金 = 所有品种占用资金之和
        /// 总资金使用量 = 占用保证金 + 冻结保证金
        /// </summary>
        decimal MoneyUsed { get; }

        /// <summary>
        /// 证券市值
        /// </summary>
        decimal SecurityMarketValue { get; }

        /// <summary>
        /// 总净值 帐户当前权益=总净值
        /// </summary>
        decimal TotalLiquidation { get; }

        /// <summary>
        /// 帐户总可用资金=总净值 - 总资金使用量
        /// </summary>
        decimal AvabileFunds { get; }

    }
}
