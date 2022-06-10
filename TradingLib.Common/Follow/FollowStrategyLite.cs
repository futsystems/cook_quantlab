using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 跟单策略lite对象 用于管理段显示
    /// </summary>
    public class FollowStrategyInfo
    {
        /// <summary>
        /// 数据库ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 运行状态
        /// </summary>
        public bool Running { get; set; }


    }
}
