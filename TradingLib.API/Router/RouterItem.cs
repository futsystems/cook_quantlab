using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface RouterItem
    {
        /// <summary>
        /// 全局序号
        /// </summary>
        int ID { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        int priority { get; set; }

        /// <summary>
        /// 路由组ID 所属路由组
        /// </summary>
        RouterGroup RouteGroup { get; }

        /// <summary>
        /// 实盘帐号ID
        /// </summary>
        //Vendor Vendor { get; }

        /// <summary>
        /// 实盘通道Broker接口
        /// </summary>
        IBroker Broker { get; set; }


        /// <summary>
        /// 保证金限额
        /// </summary>
        decimal MarginLimit { get; set; }


        /// <summary>
        /// 接受委托规则
        /// </summary>
        string rule { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        bool Active { get; set; }
    }
}
