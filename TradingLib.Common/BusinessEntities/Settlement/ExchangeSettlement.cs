using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 交易所结算
    /// 某个交易帐户 某个交易所下的结算数据
    /// </summary>
    public class ExchangeSettlementImpl : ExchangeSettlement
    {
        public ExchangeSettlementImpl()
        {
            this.Settleday = 0;
            this.Account = string.Empty;
            this.CloseProfitByDate = 0;
            this.PositionProfitByDate = 0;
            this.Commission = 0;
            this.Exchange = string.Empty;
            this.Settled = false;
        }

        /// <summary>
        /// 结算日
        /// </summary>
        public int Settleday { get; set; }

        /// <summary>
        /// 帐户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 结算平仓盈亏
        /// </summary>
        public decimal CloseProfitByDate { get; set; }


        /// <summary>
        /// 盯市浮动盈亏
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
        /// 交易所
        /// </summary>
        public string Exchange { get; set; }


        /// <summary>
        /// 是否结算
        /// </summary>
        public bool Settled { get; set; }


        public static string Serialize(ExchangeSettlement settle)
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(settle.Settleday);
            sb.Append(d);
            sb.Append(settle.Account);
            sb.Append(d);
            sb.Append(settle.CloseProfitByDate);
            sb.Append(d);
            sb.Append(settle.PositionProfitByDate);
            sb.Append(d);
            sb.Append(settle.AssetBuyAmount);
            sb.Append(d);
            sb.Append(settle.AssetSellAmount);
            sb.Append(settle.Exchange);
            sb.Append(d);
            sb.Append(settle.Settled);
           
            return sb.ToString();
        }

        public new static ExchangeSettlement Deserialize(string message)
        {
            string[] rec = message.Split(',');
            ExchangeSettlementImpl settle = new ExchangeSettlementImpl();
            settle.Settleday = int.Parse(rec[0]);
            settle.Account = rec[1];
            settle.CloseProfitByDate = decimal.Parse(rec[2]);
            settle.PositionProfitByDate = decimal.Parse(rec[3]);
            settle.AssetBuyAmount = decimal.Parse(rec[4]);
            settle.AssetSellAmount = decimal.Parse(rec[5]);
            settle.Exchange = rec[6];
            settle.Settled = bool.Parse(rec[7]);
            return settle;
        }
    }
}
