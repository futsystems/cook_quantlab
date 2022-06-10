using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    /// <summary>
    /// 客户端位置
    /// </summary>
    public interface ILocation
    {
        /// <summary>
        /// 前置编号
        /// </summary>
        string FrontID { get; set; }

        /// <summary>
        /// 客户端编号
        /// </summary>
        string ClientID { get; set; }
    }
}
