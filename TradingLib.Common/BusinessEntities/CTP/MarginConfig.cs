﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    /// <summary>
    /// 保证金设置
    /// </summary>
    public class MarginConfig
    {
        /// <summary>
        /// 交易帐户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 多头保证金(按金额)
        /// </summary>
        public decimal LongMarginRatioByMoney { get; set; }
        /// <summary>
        /// 空头保证金(按金额)
        /// </summary>
        public decimal ShortMarginRatioByMoney { get; set; }
        /// <summary>
        /// 多头保证金按手数
        /// </summary>
        public decimal LongMarginRatioByVolume { get; set; }

        /// <summary>
        /// 空头保证金按手数
        /// </summary>
        public decimal ShortMarginRatioByVoume { get; set; }
    }
}