using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    /// <summary>
    /// 消息编码类别
    /// </summary>
    public enum EnumMessageCodeType
    {
        /// <summary>
        /// 明文字符串拼接
        /// </summary>
        PLAINTEXT,
        /// <summary>
        /// 二进制
        /// </summary>
        BINARY,
    }
}
