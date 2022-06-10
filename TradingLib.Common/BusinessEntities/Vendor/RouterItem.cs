using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    public class RouterItemSetting
    {
        /// <summary>
        /// 全局序号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int priority { get; set; }

        /// <summary>
        /// 路由组ID
        /// </summary>
        public int routegroup_id { get; set; }

        /// <summary>
        /// 实盘帐号ID
        /// </summary>
        public int vendor_id { get; set; }

        /// <summary>
        /// 通道ID
        /// </summary>
        public int Connector_ID { get; set; }

        /// <summary>
        /// 保证金限额
        /// </summary>
        public decimal MarginLimit { get; set; }

        /// <summary>
        /// 接受委托规则
        /// </summary>
        public string rule { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Active { get; set; }

    }
    /// <summary>
    /// 路由与路由组映射关系
    /// </summary>
    public class RouterItemImpl : RouterItemSetting, RouterItem
    {
        RouterGroup _routegroup = null;
        /// <summary>
        /// 路由组ID 所属路由组
        /// </summary>
        //[NoJsonExportAttr()]
        [Newtonsoft.Json.JsonIgnore]
        public RouterGroup RouteGroup { get { return _routegroup; } set { _routegroup = value; } }
        //Vendor _vendor = null;
        ///// <summary>
        ///// 实盘帐号对象
        ///// </summary>
        //[NoJsonExportAttr()]
        //public Vendor Vendor { get { return _vendor; } set { _vendor = value; } }

        /// <summary>
        /// 绑定成交通道
        /// </summary>
        /// <param name="broker"></param>
        public void BindBroker(IBroker broker)
        {
            _broker = broker;
        }

        IBroker _broker = null;
        //[NoJsonExportAttr()]
        [Newtonsoft.Json.JsonIgnore]
        public IBroker Broker { get { return _broker; } set { _broker = value; } }
    }


}
