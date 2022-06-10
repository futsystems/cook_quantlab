using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    /// <summary>
    /// 
    /// </summary>
    public interface RouterGroup
    {
        /// <summary>
        /// 按token返回IBroker成交接口
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        IBroker GetBroker(string token);

        /// <summary>
        /// 获得默认开仓Broker
        /// </summary>
        /// <returns></returns>
        IBroker GetBroker(Order o,decimal margintouse);

        /// <summary>
        /// 将路由添加到路由组
        /// </summary>
        /// <param name="broker"></param>
        /// <param name="priority"></param>
        void AppendRouterItem(RouterItem item);

        /// <summary>
        /// 将路由从路由组删除
        /// </summary>
        /// <param name="broker"></param>
        void RemoveRouterItem(RouterItem item);

        /// <summary>
        /// 路由项目
        /// </summary>
        IEnumerable<RouterItem> RouterItems { get; }

        /// <summary>
        /// 全局ID
        /// </summary>
        int ID { get; set; }

        /// <summary>
        /// 路由组名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 路由策略
        /// </summary>
        QSEnumRouterStrategy Strategy { get; set; }

        /// <summary>
        /// 主域ID
        /// </summary>
        Domain Domain { get;}
    }
}
