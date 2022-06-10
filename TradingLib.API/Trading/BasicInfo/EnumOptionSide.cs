using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public enum QSEnumOptionSide : byte
    {
        /// <summary>
        /// 看涨期权
        /// </summary>
        CALL,

        /// <summary>
        /// 看跌期权
        /// </summary>
        PUT,

        /// <summary>
        /// 无
        /// </summary>
        NULL,
    }
}
