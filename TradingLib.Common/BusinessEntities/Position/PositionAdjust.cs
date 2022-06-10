using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 持仓调整对象
    /// 用于调整持仓状态
    /// 表明在某个持仓对象上 按什么价格 买入或者卖 出多少数量的价格
    /// </summary>
    internal class PositionAdjust
    {

        public PositionAdjust(PositionDetail detail)
        {
            this.Account = detail.Account;
            this.Symbol = detail.Symbol;
            this.oSymbol = detail.oSymbol;
            this.xPrice = detail.SettlementPrice;//持仓明细 将昨日结算时的持仓明细加载到内存恢复当日持仓状态，对应的价格为结算价格
            this.xSize = detail.Side ? detail.Volume : -1 * detail.Volume;//positiondetail 不带方向
            this.ClosedPL = 0;
        }

        public PositionAdjust(Trade fill)
        {
            this.Account = fill.Account;
            this.Symbol = fill.Symbol;
            this.oSymbol = fill.oSymbol;
            this.xPrice = fill.xPrice;
            this.xSize = fill.xSize;
            this.ClosedPL = 0;
        }

        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public decimal ClosedPL { get; set; }

        /// <summary>
        /// 交易帐号
        /// </summary>
        public string Account { get; set; }

        string _sym = string.Empty;
        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get { return oSymbol != null ? oSymbol.Symbol : _sym; } set { _sym = value; } }

        /// <summary>
        /// 合约对象
        /// </summary>
        public Symbol oSymbol { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int xSize { get; set; }

        public bool IsLong { get { return xSize > 0; } }

        public bool IsShort { get { return xSize < 0; } }

        public bool IsFlat { get { return xSize == 0; } }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal xPrice { get; set; }


        public override string ToString()
        {
            return this.Account + "-" + this.Symbol + " " + this.xSize.ToString() + "@" + this.xPrice.ToString();
        }
        /// <summary>
        /// 持仓调整是否有效
        /// </summary>
        public bool IsValid
        {
            get
            {
                return (this.Symbol != null) && (this.xPrice != 0 && this.xSize != 0);
            }
        }

    }
}
