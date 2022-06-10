using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TradingLib.API
{
    /// <summary>
    /// 委托产生类别
    /// 在设计结构中委托起源于3个地方
    /// 1.分帐户侧，交易客户端，管理端发起的委托
    /// 2.委托路由，委托路由如果需要将委托分拆成多个委托，则分拆后的委托属于委托路由
    /// 3.成交侧，成交侧如果需要将委托分拆成多个委托，则分拆后的委托属于成交侧
    /// 这里相当于是2级分解
    /// 分帐户侧分解成路由侧，路由侧委托在成交接口侧再次进行分解
    /// 
    /// 成交则通过委托便后进行关联
    /// </summary>
    public enum QSEnumOrderBreedType
    {
        /// <summary>
        /// Account产生的委托
        /// </summary>
        ACCT,

        /// <summary>
        /// OrderRouter产生的委托
        /// </summary>
        ROUTER,

        /// <summary>
        /// Broker产生的委托
        /// </summary>
        BROKER,
    }
}
