using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    public enum  QSEnumChargeType
    {
        /// <summary>
        /// 相对收取，在标准手续费上加收一定金额
        /// </summary>
        [Description("相对收取(在基础费率基础上加收以上数值)")]
        Relative,

        /// <summary>
        /// 绝对收取,按某值进行绝对收取
        /// </summary>
        [Description("绝对收取(在基础费率基础以该数值直接收取")]
        Absolute,

        /// <summary>
        /// 上浮一定比例收取,0.1 上浮10%
        /// </summary>
        [Description("上浮百分比收取(在基础费率基础上浮一定比例收取")]
        Percent,
    }
}
