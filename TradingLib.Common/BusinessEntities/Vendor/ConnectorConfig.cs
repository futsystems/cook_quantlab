using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.Common
{

    /// <summary>
    /// 成交接口配置信息 
    /// 设定服务器地址 端口 登入用户名 和密码等相关参数
    /// </summary>
    public class ConnectorConfig
    {
        /// <summary>
        /// 数据库编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string srvinfo_ipaddress { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int srvinfo_port { get; set; }

        /// <summary>
        /// 扩展字段1
        /// </summary>
        public string srvinfo_field1 { get; set; }

        /// <summary>
        /// 扩展字段2
        /// </summary>
        public string srvinfo_field2 { get; set; }

        /// <summary>
        /// 扩展字段3
        /// </summary>
        public string srvinfo_field3 { get; set; }

        /// <summary>
        /// 登入名
        /// </summary>
        public string usrinfo_userid { get; set; }

        /// <summary>
        /// 登入密码
        /// </summary>
        public string usrinfo_password { get; set; }

        /// <summary>
        /// 扩展字段1
        /// </summary>
        public string usrinfo_field1 { get; set; }

        /// <summary>
        /// 扩展字段2
        /// </summary>
        public string usrinfo_field2 { get; set; }

        /// <summary>
        /// 接口定义编号
        /// </summary>
        public int interface_fk { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 接口
        /// </summary>
        //[NoJsonExportAttr()]
        [Newtonsoft.Json.JsonIgnore]
        public ConnectorInterface Interface { get; set; }


        /// <summary>
        /// 对应的实盘帐户全局ID
        /// </summary>
        public int vendor_id { get; set; }

        /// <summary>
        /// 域ID
        /// </summary>
        public int domain_id { get; set; }

        /// <summary>
        /// 域
        /// </summary>
        //[NoJsonExportAttr()]
        [Newtonsoft.Json.JsonIgnore]
        public Domain Domain { get; internal set; }


        /// <summary>
        /// 是否需要绑定Vendor
        /// </summary>
        public bool NeedVendor { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid
        {
            get
            {
                return this.Domain != null && this.Interface != null && this.Interface.IsValid;
            }
        }
    }
}
