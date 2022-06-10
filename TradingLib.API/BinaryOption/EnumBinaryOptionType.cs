using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace TradingLib.API
{
    /// <summary>
    /// 期权类别
    /// </summary>
    public enum EnumBinaryOptionType
    {
        /// <summary>
        /// 涨跌期权 StrikePrice为当前价格
        /// Call:期权到期时 市场价格在StrikePrice之上为InTheMoney否则OutOfTheMoney
        /// Put:期权到期时 市场价格在StrikePrice之下为InTheMoney否则OutOfTheMoney
        /// </summary>
        [Description("涨跌期权")]
        CallPut,
        /// <summary>
        /// 高低期权 StrikePrice为系统指定的一个价格
        /// Above:期权到期时 市场价在指定的Target之上为InTheMoney否则OutOfTheMoney
        /// </summary>
        [Description("高低期权")]
        AboveDown,
        /// <summary>
        /// 区间期权
        /// RangeIn:期权到期时 市场价格在区间内为InTheMoney否则OutOfTheMoney
        /// RangeOut:期权到期时 市场价格在区间之外为InThenMoney否则为OutOfTheMoney
        /// </summary>
        [Description("区间期权")]
        Range,
        /// <summary>
        /// 触式期权 在规定时间内 价格触及边界
        /// </summary>
        [Description("有触期权")]
        Touch,
        /// <summary>
        /// 无触式期权 在规定时间内 价格没有触及边界
        /// </summary>
        [Description("无触期权")]
        NoTouch,
    }
}
