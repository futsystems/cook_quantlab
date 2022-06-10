using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    /// <summary>
    /// 合约列别
    /// </summary>
    public enum QSEnumSymbolType
    {
        /// <summary>
        /// 标准合约
        /// </summary>
        [Description("标准合约")]
        Standard,

        /// <summary>
        /// 月连续01-12连续
        /// </summary>
        [Description("月连续")]
        MonthContinuous,

    }
}
