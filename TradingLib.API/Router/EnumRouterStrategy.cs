using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    public enum QSEnumRouterStrategy
    {
        /// <summary>
        /// 优先级
        /// 按设定的优先级顺序进行选择数字越小优先级越大
        /// </summary>
        [Description("优先级")]
        Priority,

        /// <summary>
        /// 随机
        /// 随机的从接口队列中获得可用队列
        /// </summary>
        [Description("随机")]
        Stochastic

    }
}
