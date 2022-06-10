using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{

    /// <summary>
    /// XXXXSetting 数据库数据储存对象
    /// 在实际业务逻辑中的业务对象 继承该对象 并丰富数据与操作用于实现业务逻辑
    /// </summary>
    public class RouterGroupSetting
    {
        /// <summary>
        /// 全局ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 路由组名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 策略名 决定了该成交路由组工作运行策略
        /// </summary>
        public QSEnumRouterStrategy Strategy { get; set; }

        /// <summary>
        /// 域ID
        /// </summary>
        public int domain_id { get; set; }

        
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        
    }

    /// <summary>
    /// 路由组
    /// </summary>
    public class RouterGroupImpl : RouterGroupSetting, RouterGroup
    {
        static  NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();

        ConcurrentDictionary<int, RouterItem> routeritemmap = new ConcurrentDictionary<int, RouterItem>();

        /// <summary>
        /// 域
        /// </summary>
        public Domain Domain { get; internal set; }



        /// <summary>
        /// 返回所有路由项目
        /// </summary>
        public IEnumerable<RouterItem> RouterItems
        {
            get
            {
                return routeritemmap.Values;
            }
        }

        #region 获得平仓Broker
        /// <summary>
        /// 获得平仓Broker,根据持仓所在的Broker Token通过Token查找到对应的Broker
        /// </summary>
        /// <returns></returns>
        public IBroker GetBroker(string token)
        {
            //查找路由项目的主帐户标识
            return routeritemmap.Values.Where(item => item.GetBrokerToken().Equals(token)).Select(item=>item.Broker).FirstOrDefault();
        }

        


        #endregion


        #region 获得开仓Broker
        Random rd = new Random(Util.ToTLTime());

        /// <summary>
        /// 返回所有可用开仓的路由项目
        /// </summary>
        /// <returns></returns>
        IEnumerable<RouterItem> GetRouterItemsForOpen()
        {
            return routeritemmap.Values.Where(r => r.Active).Where(r => r.Broker != null);
        }

        /// <summary>
        /// 按优先级别排序获得可开仓路由项目
        /// </summary>
        /// <returns></returns>
        IEnumerable<RouterItem> GetRouterItemsForOpenSorted()
        {
            return routeritemmap.Values.Where(r => r.Active).Where(r => r.Broker != null).OrderBy(r => r.priority);
        }

        /// <summary>
        /// 返回默认的开仓通道，根据策略给出当前可用的开仓通道
        /// </summary>
        /// <returns></returns>
        public IBroker GetBroker(Order o, decimal margintouse)
        {
            if (this.Strategy == QSEnumRouterStrategy.Priority)
            {
                return PriorityBroker(o, margintouse);
            }
            else if (this.Strategy == QSEnumRouterStrategy.Stochastic)
            {
                return StochasticBroker(o, margintouse);
            }
            else
            {
                return StochasticBroker(o, margintouse);
            }

        }

        IBroker StochasticBroker(Order o, decimal margintouse)
        {
            //目前实现优随机可用选择
            IBroker[] brokers = GetRouterItemsForOpen().Where(v => v.IsBrokerAvabile()).Where(v => v.AcceptEntryOrder(o, margintouse)).Select(v => v.Broker).ToArray();
            if (brokers.Length < 1)
            {
                return null;
            }
            int idx = rd.Next(0, brokers.Length);
            IBroker broker = brokers[idx];
            logger.Info(string.Format("Stochastic Strategy Select Broker[{0}]", broker.Token));
            return broker;
        }

        IBroker PriorityBroker(Order o, decimal margintouse)
        {
            IEnumerable<RouterItem> routerItemList = GetRouterItemsForOpenSorted();
            IBroker[] brokers = routerItemList.Where(v => v.IsBrokerAvabile()).Where(v => v.AcceptEntryOrder(o, margintouse)).Select(v => v.Broker).ToArray();
            if (brokers.Length < 1)
            {
                return null;
            }
            IBroker broker = brokers[0];
            logger.Info(string.Format("Priority Strategy Select Broker[{0}]", broker.Token));
            return broker;
        }


        
        #endregion



        //public void Start()
        //{
        //    foreach (IBroker b in GetBrokers())
        //    {
        //        if (b != null && !b.IsLive)
        //        {
        //            b.Start();
        //        }
        //    }
        //}

        /// <summary>
        /// 获得所有成交通道 
        /// 不保证成交通道已经启动或逻辑上可用
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<IBroker> GetBrokers()
        //{
        //    return GetVendors().Where(v => v.Broker != null).Select(v => v.Broker);
        //}

        ///// <summary>
        ///// 获得所有可用的Broker用于开仓
        ///// 底层通道需启动，并且实盘通道满足设定的资金条件
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<IBroker> GetAvabileBrokers()
        //{
        //    return GetVendors().Where(v => v.IsAvabile()).Select(v => v.Broker);
        //}

        /// <summary>
        /// 将Broker添加到路由组
        /// </summary>
        /// <param name="broker"></param>
        public void AppendRouterItem(RouterItem item)
        {
            if (routeritemmap.Keys.Contains(item.ID))
            {
                logger.Debug(string.Format("RouteItem[{0}] existed,can not append again", item.ID));
                return;
            }
            //将路由条目添加到路由组
            routeritemmap.TryAdd(item.ID, item);
            //if (brokermap.Keys.Contains(broker.Token))
            //{
            //    Util.Debug(string.Format("Broker[{0}] existed,can not append again", broker.Token));
            //    return;
            //}
            //添加到map
            //brokermap.TryAdd(broker.Token, broker);
            //prioritymap.TryAdd(broker.Token, priority);
        }

        /// <summary>
        /// 将Broker从路由组删除
        /// </summary>
        /// <param name="broker"></param>
        public void RemoveRouterItem(RouterItem item)
        {
            if (!routeritemmap.Keys.Contains(item.ID))
            {
                logger.Debug(string.Format("RouteItem[{0}] do not exist,can not remove", item.ID));
                return;
            }
            RouterItem rmitem = null;
            routeritemmap.TryRemove(item.ID, out rmitem);

            //if (!brokermap.Keys.Contains(broker.Token))
            //{
            //    Util.Debug(string.Format("Broker[{0}] do not exist,can not remove", broker.Token));
            //    return;
            //}
            //IBroker rb = null;
            //brokermap.TryRemove(broker.Token,out rb);
            //int ri = 0;
            //prioritymap.TryRemove(broker.Token, out ri);
        }

       

    }
}
