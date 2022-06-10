using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    public enum  QSEnumEquityType
    {
        /// <summary>
        /// 帐户自有资金，劣后资金
        /// </summary>
        [Description("劣后资金")]
        OwnEquity,
        [Description("优先资金")]
        CreditEquity,
    }
}
