using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.Common
{

    public class FatherSonOrderPair
    {
        public FatherSonOrderPair(Order father)
        {
            this.FatherOrder = father;
            this.SonOrders = new List<Order>();
        }
        public Order FatherOrder { get; set; }

        public List<Order> SonOrders { get; set; }
    }
    /// <summary>
    /// 委托分拆管理器
    /// 用于将某个委托分拆成多个委托然后对外处理
    /// 输入侧，操作父委托，系统按照分解逻辑分解后，将委托分解成子委托，然后输出子委托的操作
    /// 当有子委托回报时,调用子委托回报输入,对外输出父委托回报
    /// 该组件实现了将某个委托按一定逻辑分拆后下发到子委托操作端
    /// 然后从子委托操作端获得回报处理后，再处理成父委托回报对外输出
    /// 
    /// 父委托编号->父委托
    /// 父委托编号->子委托列表
    /// 子委托编号->父委托
    /// </summary>
    public class OrderSplitTracker
    {
        static NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        string _token = string.Empty;
        public OrderSplitTracker(string name)
        {
            _token = name;
        }
        string Token { get { return _token; } }
        //保存父委托
        ConcurrentDictionary<long, Order> fatherOrder_Map = new ConcurrentDictionary<long, Order>();
        Order FatherID2Order(long id)
        {
            if (fatherOrder_Map.Keys.Contains(id))
            {
                return fatherOrder_Map[id];
            }
            return null;
        }


        //用于通过父委托ID找到对应的子委托
        ConcurrentDictionary<long, List<Order>> fatherSonOrder_Map = new ConcurrentDictionary<long, List<Order>>();//父子子委托映射关系
        //通过父委托ID找到对应的子委托对
        List<Order> FatherID2SonOrders(long id)
        {
            if (fatherSonOrder_Map.Keys.Contains(id))
                return fatherSonOrder_Map[id];
            return null;
        }

        //用于通过子委托ID找到对应的父委托
        ConcurrentDictionary<long, Order> sonFathOrder_Map = new ConcurrentDictionary<long, Order>();
        /// <summary>
        /// 通过子委托ID找到对应的父委托
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Order SonID2FatherOrder(long id)
        {
            if (sonFathOrder_Map.Keys.Contains(id))
                return sonFathOrder_Map[id];
            return null;
        }

        //子委托id与子委托映射
        ConcurrentDictionary<long, Order> sonOrder_Map = new ConcurrentDictionary<long, Order>();
        Order SonID2SonOrder(long id)
        {
            if (sonOrder_Map.Keys.Contains(id))
                return sonOrder_Map[id];
            return null;
        }

        /// <summary>
        /// 获得通过该分解器发送的某个子委托
        /// </summary>
        /// <param name="id"></param>
        public Order SentSonOrder(long id)
        {
            return SonID2SonOrder(id);
        }
        /// <summary>
        /// 清空内存状态
        /// </summary>
        public void Clear()
        {
            fatherOrder_Map.Clear();
            fatherSonOrder_Map.Clear();
            sonFathOrder_Map.Clear();
            sonOrder_Map.Clear();
        }
        /// <summary>
        /// 恢复父子委托关系
        /// </summary>
        /// <param name="father"></param>
        /// <param name="sonOrders"></param>
        public void ResumeOrder(FatherSonOrderPair pair)
        {
            fatherOrder_Map.TryAdd(pair.FatherOrder.id, pair.FatherOrder);
            fatherSonOrder_Map.TryAdd(pair.FatherOrder.id, pair.SonOrders);
            foreach(Order o in pair.SonOrders)
            {
                sonFathOrder_Map.TryAdd(o.id, pair.FatherOrder);
                sonOrder_Map.TryAdd(o.id, o);
            }
        }

        #region 对外发送子委托操作
        /// <summary>
        /// 发送子委托
        /// </summary>
        public event OrderDelegate SendSonOrderEvent;
        void SendSonOrder(Order o)
        {
            if (SendSonOrderEvent != null)
                SendSonOrderEvent(o);
        }

        /// <summary>
        /// 取消子委托
        /// </summary>
        public event OrderDelegate CancelSonOrderEvent;
        void CancelSonOrder(Order o)
        {
            if (CancelSonOrderEvent != null)
              CancelSonOrderEvent(o);
        }
        #endregion

        #region 对外发送父委托回报
        /// <summary>
        /// 获得父委托回报
        /// </summary>
        public event OrderDelegate GotFatherOrderEvent;
        void GotFatherOrder(Order o)
        {
            if (GotFatherOrderEvent != null)
                GotFatherOrderEvent(o);
        }

        /// <summary>
        /// 获得父成交回报
        /// </summary>
        public event FillDelegate GotFatherFillEvent;
        void GotFatherFill(Trade f)
        {
            if (GotFatherFillEvent != null)
                GotFatherFillEvent(f);
        }
        /// <summary>
        /// 获得父委取消
        /// </summary>
        public event LongDelegate GotFatherCancelEvent;
        void GotFatherCancel(long oid)
        {
            if (GotFatherCancelEvent != null)
                GotFatherCancelEvent(oid);
        }

        /// <summary>
        /// 获得父委托错误回报
        /// </summary>
        public event OrderErrorDelegate GotFatherOrderErrorEvent;
        void GotFatherOrderError(Order o, RspInfo info)
        {
            if (GotFatherOrderErrorEvent != null)
                GotFatherOrderErrorEvent(o, info);
        }

        public event OrderActionErrorDelegate GotFatherOrderActionErrorEvent;
        void GotFatherOrderActionError(OrderAction action,RspInfo info)
        {
            if (GotFatherOrderActionErrorEvent != null)
                GotFatherOrderActionErrorEvent(action, info);
        }
        #endregion



        /// <summary>
        /// 分解委托
        /// </summary>
        public event Func<Order, List<Order>> SplitOrdereEvent;
        List<Order> SplitOrder(Order o)
        {
            if (SplitOrdereEvent != null)
                return SplitOrdereEvent(o);
            return new List<Order>();
        }


        #region 接受父委托端输入
        /// <summary>
        /// 发送父委托
        /// </summary>
        /// <param name="fathOrder"></param>
        public void SendFatherOrder(Order fathOrder,List<Order> sons=null)
        {
            logger.Info("OrderSplitTracker[" + this.Token + "] Send FatherOrder:" + fathOrder.GetOrderInfo());
            //1.分拆委托

            List<Order> sonOrders = (sons==null?SplitOrder(fathOrder):sons);//分拆该委托 如果发送委托时候已经指定了子委托

            Order fo = new OrderImpl(fathOrder);
            //2.将委托加入映射map
            fatherOrder_Map.TryAdd(fo.id, fo);//保存付委托映射关系
            fatherSonOrder_Map.TryAdd(fo.id, sonOrders);//保存父委托到子委托映射关系//这里没有复制委托 其他地方更新该委托可能委托也会被同步更新
            
            //2.统一发送子委托
            foreach (Order order in sonOrders)
            {
                sonFathOrder_Map.TryAdd(order.id, fo);//保存子委托到父委托映射关系
                sonOrder_Map.TryAdd(order.id, order);//保存子委托
                SendSonOrder(order);
            }
            //3.更新父委托状态
            //如果对应的子委托有正常提交的,那么父委托就是处于提交状态
            if (sonOrders.Any(so => so.Status == QSEnumOrderStatus.Submited))
            {
                fo.Status = QSEnumOrderStatus.Submited;
            }
            //如果子委托状态为拒绝,则父委托的状态也为拒绝
            if (sonOrders.All(so => so.Status == QSEnumOrderStatus.Reject))
            {
                fo.Status = QSEnumOrderStatus.Reject;
            }
            //同步本地状态 接口发送委托 依靠orderstatus 来判断委托是否发送成功
            fathOrder.Status = fo.Status;

            logger.Info("父子委托关系链条 " + fathOrder.id + "->[" + string.Join(",", sonOrders.Select(so => so.id)) + "] CopyID:" + fo.CopyID.ToString());
        }

        /// <summary>
        /// 取消父委托
        /// </summary>
        /// <param name="oid"></param>
        public void CancelFatherOrder(long oid)
        {
            Order fatherOrder = FatherID2Order(oid);
            if (fatherOrder != null)
            {
                logger.Info("OrderSplitTracker[" + this.Token + "] 取消父委托:" + fatherOrder.GetOrderInfo());
                List<Order> sonOrders = FatherID2SonOrders(fatherOrder.id);//获得子委托

                //如果所有委托均不可撤销 正常委托Opened PartFilled是可以撤销的
                //如果委托处于提交状态但是没有获得CTP回报,此时委托处于Submited,但是如果这个时候撤单就会发生处于submit 不进行撤单，但是后来委托回报又回来了，则状态会发生混乱
                if (sonOrders.All(o => !o.IsPending())) //处于Submit的委托 可能CTP回报回报慢导致状态没有进入Opened
                {
                    logger.Info(string.Format("All SonOrder Can not be canceled,father status:{0} notify father cancel internal", fatherOrder.Status));
                    //如果子委托全部为拒绝 则父委托为拒绝
                    if (sonOrders.All(o => o.Status == QSEnumOrderStatus.Reject))
                    {
                        fatherOrder.Status = QSEnumOrderStatus.Reject;
                    }
                    fatherOrder.Status = QSEnumOrderStatus.Canceled;
                    fatherOrder.Comment = string.Empty;//清空回报记录

                    GotFatherOrder(fatherOrder);
                    //?是否去除单独的Cancel回报
                    if (fatherOrder.Status == QSEnumOrderStatus.Canceled)
                    {
                        GotFatherCancel(fatherOrder.id);
                    }

                }
                else
                {
                    //如果子委托状态处于pending状态
                    /* 底层接口委托初始状态为Submit 正常情况接口会立刻返回Opened,PartFilled/Filled 或者直接拒绝
                     * 
                     * */
                    foreach (Order o in sonOrders)
                    {
                        if (o.IsPending())//如果委托处于待成交状态 则发送撤单指令
                        {
                            CancelSonOrder(o);
                        }
                    }
                }
                
            }
            else
            {
                logger.Warn("Order:" + oid.ToString() + " is not in platform_order_map in broker");
            }
        }
        #endregion


        #region 子委托端 交易信息输入
        /// <summary>
        /// 获得子委托回报
        /// 注本地维护了子委托内存数据
        /// 同时该委托是其余组件进行分拆了
        /// BrokerSplit分拆委托后将委托提供给委托分拆器
        /// </summary>
        /// <param name="o"></param>
        public void GotSonOrder(Order o)
        {
            //更新子委托数据完毕后 通过子委托找到父委托 然后转换状态并发送
            Order fatherOrder = SonID2FatherOrder(o.id);//获得父委托
            List<Order> sonOrders = FatherID2SonOrders(fatherOrder.id);//获得子委托列表

            Order sonorder = SonID2SonOrder(o.id);
            //更新委托
            sonorder.Status = o.Status;//更新委托状态
            sonorder.Comment = o.Comment;//填充状态信息
            sonorder.FilledSize = o.FilledSize;//成交数量
            sonorder.Size = o.Size;//更新委托当前数量

            //fatherOrder.OrderSysID = fatherOrder.OrderSeq.ToString();//父委托OrderSysID编号 取系统的OrderSeq
            //父委托编号赋值
            //1对1
            if (sonOrders.Count == 1)
            {
                fatherOrder.OrderSysID = o.OrderSysID;
            }
            else //1对多
            {
                fatherOrder.OrderSysID = fatherOrder.OrderSeq.ToString();//父委托OrderSysID编号 取系统的OrderSeq
            }

            //更新父委托状态 成交数量 状态 以及 状态信息
            int lastfilledsize = fatherOrder.FilledSize;
            fatherOrder.FilledSize = sonOrders.Sum(so => so.FilledSize);//累加成交数量
            fatherOrder.Size = sonOrders.Sum(so => so.UnsignedSize) * (o.Side ? 1 : -1);//累加未成交数量
            //fatherOrder.Comment = "";//填入状态信息

            QSEnumOrderStatus oldstatus = fatherOrder.Status;//原始委托状态
            bool fillsizechanged = lastfilledsize != fatherOrder.FilledSize;//成交数量变化
            //组合状态
            QSEnumOrderStatus fstatus = fatherOrder.Status;
            //子委托全部成交 则父委托为全部成交
            //if (sonOrders.All(so => so.Status == QSEnumOrderStatus.Filled))//所有filled则为filled
            //    fstatus = QSEnumOrderStatus.Filled;
            //子委托任一待成交,则父委托为待成交
            if (sonOrders.Any(so => so.Status == QSEnumOrderStatus.Opened))//任何一个委托为opened则为open
                fstatus = QSEnumOrderStatus.Opened;
            //子委托全部拒绝,则父委托为拒绝
            else if (sonOrders.All(so => so.Status == QSEnumOrderStatus.Reject))//所有拒绝则为拒绝
                fstatus = QSEnumOrderStatus.Reject;
            //子委托有任一取消,则父委托为取消
            else if (sonOrders.Any(so => so.Status == QSEnumOrderStatus.Canceled))//任何一个取消则为取消
                fstatus = QSEnumOrderStatus.Canceled;
            //子委托有一个委托为部分成交
            //else if (sonOrders.Any(so => so.Status == QSEnumOrderStatus.PartFilled))//部分成交
            //{
            //    //另一个委托为取消，则父委托为取消
            //    if (sonOrders.Any(so => so.Status == QSEnumOrderStatus.Canceled))
            //        fstatus = QSEnumOrderStatus.Canceled;

            //    //另一个委托为拒绝,则父委托为取消
            //    //if (sonOrders.Any(so => so.Status == QSEnumOrderStatus.Reject))
            //    fstatus = QSEnumOrderStatus.PartFilled;
            //}

           
            fatherOrder.Status = fstatus;
            fatherOrder.Comment = o.Comment;
            if (fatherOrder.Status != QSEnumOrderStatus.Canceled && fatherOrder.Status != QSEnumOrderStatus.Reject)
            {
                logger.Info("fater order in pending stage,filledsize:" + fatherOrder.FilledSize.ToString() + " totalsize:" + fatherOrder.TotalSize.ToString());
                if (fatherOrder.FilledSize == 0)//成交数量为0 则为open状态
                {
                    fatherOrder.Status = QSEnumOrderStatus.Opened;
                }
                else if (fatherOrder.FilledSize < Math.Abs(fatherOrder.TotalSize)) //成交数量小于总数量 则为partfilled
                {
                    fatherOrder.Status = QSEnumOrderStatus.PartFilled;
                }
                else //成交数量等于总数量 则为filled
                {
                    fatherOrder.Status = QSEnumOrderStatus.Filled;
                }
            }

            logger.Info("更新父委托:" + fatherOrder.GetOrderInfo());
            //委托状态没有变化 并且 成交数量也没有变化
            if (oldstatus == fatherOrder.Status && !fillsizechanged)
            {
                logger.Debug("FatherOrder do not chagen,will not notify client");
            }
            else
            {
                GotFatherOrder(fatherOrder);
            }

            if (fatherOrder.Status == QSEnumOrderStatus.Canceled)
            {
                GotFatherCancel(fatherOrder.id);
            }
        }

        /// <summary>
        /// 获得子委托成交回报
        /// </summary>
        /// <param name="f"></param>
        public void GotSonFill(Trade f)
        {
            //付委托对应的成交
            Order fatherOrder = SonID2FatherOrder(f.id);//获得父委托
            Trade fill = (Trade)(new OrderImpl(fatherOrder));

            //设定价格 数量 以及日期信息
            fill.xSize = f.UnsignedSize * (f.Side ? 1 : -1);
            fill.xPrice = (decimal)f.xPrice;

            fill.xDate = f.xDate;
            fill.xTime = f.xTime;
            fill.Broker = f.Broker;
            //远端成交编号
            //fill.BrokerTradeID = trade.BrokerTradeID;
            //其余委托类的相关字段在Order处理中获得
            logger.Info("获得父成交:" + fill.GetTradeDetail());
            GotFatherFill(fill);
        }

        /// <summary>
        /// 获得子委托错误回报
        /// </summary>
        /// <param name="o"></param>
        /// <param name="error"></param>
        public void GotSonOrderError(Order o, RspInfo error)
        {
            Order fatherOrder = SonID2FatherOrder(o.id);//获得父委托
            List<Order> sonOrders = FatherID2SonOrders(fatherOrder.id);//获得所有子委托
            Order sonorder = SonID2SonOrder(o.id);
            //更新委托
            sonorder.Status = o.Status;//更新委托状态
            sonorder.Comment = o.Comment;//填充状态信息
            sonorder.FilledSize = o.FilledSize;//成交数量
            sonorder.Size = o.Size;//更新委托当前数量

            RspInfo info = new RspInfoImpl();
            info.ErrorID = error.ErrorID;
            info.ErrorMessage = error.ErrorMessage;

            bool isrejected = (fatherOrder.Status == QSEnumOrderStatus.Reject);
            //所有子委托为拒绝则父委托为拒绝
            if (sonOrders.All(so => so.Status == QSEnumOrderStatus.Reject))
            {
                fatherOrder.Status = QSEnumOrderStatus.Reject;
            }
            //如果部分拒绝如何？另一部分处于成交状态，或者等待成就状态
            //fatherOrder.Status = QSEnumOrderStatus.Reject;
            fatherOrder.Comment = info.ErrorMessage;
            logger.Info("更新父委托:" + fatherOrder.GetOrderInfo());
            //父委托已经对外回报过拒绝则不再对外回报
            if (!isrejected)
                GotFatherOrderError(fatherOrder, info);
        }

        /// <summary>
        /// 获得子委托操作错误回报
        /// </summary>
        /// <param name="a"></param>
        /// <param name="error"></param>
        public void GotSonOrderActionError(OrderAction a, RspInfo error)
        {
            Order fatherOrder = SonID2FatherOrder(a.OrderID);//获得父委托
            List<Order> sonOrders = FatherID2SonOrders(fatherOrder.id);//获得所有子委托

            RspInfo info = new RspInfoImpl();
            info.ErrorID = error.ErrorID;
            info.ErrorMessage = error.ErrorMessage;

            OrderAction action = new OrderActionImpl();
            action.OrderID = fatherOrder.id;
            action.Account = fatherOrder.Account;
            action.ActionFlag = a.ActionFlag;

            GotFatherOrderActionError(action, info);
        }

        #endregion

    }
}
