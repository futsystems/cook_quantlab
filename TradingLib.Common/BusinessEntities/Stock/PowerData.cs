using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    /// <summary>
    /// 除权数据
    /// </summary>
    public class PowerData
    {
        /// <summary>
        /// 结算日
        /// </summary>
        public int Settleday { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 每股分红
        /// </summary>
        public decimal Dividend { get; set; }


        /// <summary>
        /// 每股送多少股 10送1等
        /// </summary>
        public decimal DonateShares { get; set; }

        /// <summary>
        /// 每股配多少股 10配2,配股价为10
        /// </summary>
        public decimal RationeShares { get; set; }


        /// <summary>
        /// 配股价
        /// </summary>
        public decimal RationePrice { get; set; }
    }
}
