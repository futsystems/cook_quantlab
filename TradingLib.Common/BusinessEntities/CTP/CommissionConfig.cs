using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 手续费设置
    /// </summary>
    public class CommissionConfig
    {
        public CommissionConfig()
        {
            this.Account = string.Empty;
            this.Symbol = string.Empty;
            this.OpenRatioByMoney = 0;
            this.OpenRatioByVolume = 0;
            this.CloseRatioByMoney = 0;
            this.CloseRatioByVolume = 0;
            this.CloseTodayRatioByMoney = 0;
            this.CloseTodayRatioByVolume = 0;
        }

        public bool IsValid { get { return (string.IsNullOrEmpty(this.Symbol) || string.IsNullOrEmpty(this.Account))?false:true; } }
        /// <summary>
        /// 交易帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 开仓手续费 按手数
        /// </summary>
        public decimal OpenRatioByVolume { get; set; }

        /// <summary>
        /// 开仓手续费 按金额
        /// </summary>
        public decimal OpenRatioByMoney { get; set; }

        /// <summary>
        /// 平仓手续费 按手数
        /// </summary>
        public decimal CloseRatioByVolume { get; set; }

        /// <summary>
        /// 平仓手续费 按金额
        /// </summary>
        public decimal CloseRatioByMoney { get; set; }


        /// <summary>
        /// 平仓手续费 按手数
        /// </summary>
        public decimal CloseTodayRatioByVolume { get; set; }

        /// <summary>
        /// 平仓手续费 按金额
        /// </summary>
        public decimal CloseTodayRatioByMoney { get; set; }

    }
}
