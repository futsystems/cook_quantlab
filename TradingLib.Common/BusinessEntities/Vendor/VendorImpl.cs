//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using TradingLib.API;
//using TradingLib.Mixins.Json;

//namespace TradingLib.Common
//{

//    public class VendorSetting
//    {
//        /// <summary>
//        /// 数据库编号
//        /// </summary>
//        public int ID { get; set; }

//        /// <summary>
//        /// 帐户姓名
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// 期货公司
//        /// </summary>
//        public string FutCompany { get; set; }


//        /// <summary>
//        /// 描述信息
//        /// </summary>
//        public string Description { get; set; }


//        /// <summary>
//        /// 上次结算权益
//        /// </summary>
//        public decimal LastEquity { get; set; }


//        /// <summary>
//        /// 保证金限制
//        /// 可以设定百分比或绝对金额
//        /// </summary>
//        public decimal MarginLimit { get; set; }

//        /// <summary>
//        /// 通道标识，在绑定Broker时设定
//        /// 解绑时置空
//        /// </summary>
//        public string BrokerToken { get; set; }

//        /// <summary>
//        /// 域ID
//        /// </summary>
//        public int domain_id { get; set; }

        
//    }
//    /// <summary>
//    /// 实盘帐户对象
//    /// </summary>
//    public class VendorImpl : VendorSetting,Vendor
//    {

//        IBroker _broker = null;
//        /// <summary>
//        /// 该实盘帐户对应的Broker对象 Broker对象用于业务操作 比如下单，撤单等
//        /// 实盘帐户对象维护了逻辑部分的数据比如资金,是否可用等。Broker只是底层的业务对象
//        /// </summary>
//        [NoJsonExportAttr()]
//        public IBroker Broker { get { return _broker; } }

//        /// <summary>
//        /// 所属域
//        /// </summary>
//        [NoJsonExportAttr()]
//        public Domain Domain { get; internal set; }


//        /// <summary>
//        /// Broker加载时 绑定该Broker的通道设置
//        /// </summary>
//        /// <param name="broker"></param>
//        public void BindBroker(IBroker broker)
//        {
//            _broker = broker;
//            this.BrokerToken = broker.Token;
//        }

//        /// <summary>
//        /// 解绑通道
//        /// </summary>
//        public void UnBindBroker()
//        {
//            _broker = null;
//            this.BrokerToken = string.Empty;
//        }
//    }
//}
