using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 成交接口配置
    /// 设定外部调用dll所在目录和文件名
    /// </summary>
    public class ConnectorInterface
    {
        /// <summary>
        /// 数据库全局编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 接口类型名
        /// </summary>
        public string type_name { get; set; }

        /// <summary>
        /// 是否是XAPI统一接口
        /// XAPI统一接口是统一将成交接口转换成标准C接口然后通过XAPI访问层统一调用访问
        /// </summary>
        public bool IsXAPI { get; set; }
        /// <summary>
        /// wrapper目录
        /// </summary>
        public string libpath_wrapper { get; set; }

        /// <summary>
        /// wrapper名称
        /// </summary>
        public string libname_wrapper { get; set; }

        /// <summary>
        /// 成交接口目录
        /// </summary>
        public string libpath_broker { get; set; }

        /// <summary>
        /// 成交接口地址
        /// </summary>
        public string libname_broker { get; set; }

        /// <summary>
        /// 是否有效
        /// 加载时验证接口后进行设置
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 通道类别
        /// </summary>
        public QSEnumConnectorType Type { get; set; }



        /// <summary>
        /// 实盘帐户对象ID
        /// </summary>
        public int Vendor_ID { get; set; }
    }

}
