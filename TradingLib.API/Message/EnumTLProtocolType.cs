using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public enum EnumFrontType
    {
        /// <summary>
        /// 直连
        /// </summary>
        Direct,
        /// <summary>
        /// 仿CTP
        /// </summary>
        SimCTP,
        /// <summary>
        /// TL文本协议 Socket前置
        /// </summary>
        TLSocket,
        /// <summary>
        /// XL二进制协议
        /// </summary>
        XLTinny,
        /// <summary>
        /// websocket
        /// </summary>
        WebSocket,
    }
}
