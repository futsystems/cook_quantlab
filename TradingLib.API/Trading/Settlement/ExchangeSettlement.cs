using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface ExchangeSettlement
    {
        /// <summary>
        /// 结算日
        /// </summary>
        int Settleday { get; set; }

        /// <summary>
        /// 帐户
        /// </summary>
        string Account { get; set; }

        /// <summary>
        /// 结算平仓盈亏
        /// </summary>
        decimal CloseProfitByDate { get; set; }


        /// <summary>
        /// 盯市浮动盈亏
        /// </summary>
        decimal PositionProfitByDate { get; set; }

        /// <summary>
        /// 资产买入金额
        /// </summary>
        decimal AssetBuyAmount { get; set; }

        /// <summary>
        /// 资产卖出金额
        /// </summary>
        decimal AssetSellAmount { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        decimal Commission { get; set; }

        /// <summary>
        /// 交易所
        /// </summary>
        string Exchange { get; set; }

        /// <summary>
        /// 是否已结算
        /// </summary>
        bool Settled { get; set; }
    }
}
