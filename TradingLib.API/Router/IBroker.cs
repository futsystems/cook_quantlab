using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TradingLib.API
{
    public interface IBroker :IConnecter
    {

        /// <summary>
        /// 向Broker发送Order
        /// </summary>
        /// <param name="o"></param>
        void SendOrder(Order o);
        /// <summary>
        /// 向Broker发送二元期权委托
        /// </summary>
        /// <param name="o"></param>
        void SendOrder(BinaryOptionOrder o);
        /// <summary>
        /// 向broker取消一个order
        /// </summary>
        /// <param name="oid"></param>
        void CancelOrder(long oid);

        /// <summary>
        /// 用于交易通道中需要有Tick进行驱动的逻辑,比如委托触发等
        /// </summary>
        /// <param name="k"></param>
        //void GotTick(Tick k);


        #region 事件
        /// <summary>
        /// 当有成交时候回报客户端
        /// </summary>
        event FillDelegate GotFillEvent;

        /// <summary>
        /// 委托正确回报时回报客户端
        /// </summary>
        event OrderDelegate GotOrderEvent;

        /// <summary>
        /// 委托错误回报
        /// </summary>
        event OrderErrorDelegate GotOrderErrorEvent;

        /// <summary>
        /// 委托操作错误回报
        /// </summary>
        event OrderActionErrorDelegate GotOrderActionErrorEvent;

        /// <summary>
        /// 撤单正确回报时回报客户端
        /// </summary>
        event LongDelegate GotCancelEvent;
        #endregion

        /// <summary>
        /// 获得成交接口所有委托
        /// </summary>
        IEnumerable<Order> Orders { get;}

        /// <summary>
        /// 获得成交接口所有成交
        /// </summary>
        IEnumerable<Trade> Trades { get; }

        /// <summary>
        /// 获得成交接口所有持仓
        /// </summary>
        IEnumerable<Position> Positions { get; }

        /// <summary>
        /// 返回所有持仓状态统计数据
        /// </summary>
        IEnumerable<PositionMetric> PositionMetrics { get; }

        /// <summary>
        /// 获得某个合约的持仓状态统计数据
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        PositionMetric GetPositionMetric(string symbol);

        /// <summary>
        /// 计算开仓委托提交后预计持仓增加量
        /// 返回0标识不增加或则减少
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        int GetPositionAdjustment(Order o);

        /// <summary>
        /// 启动 并输出msg
        /// </summary>
        /// <param name="msg"></param>
        bool Start(out string msg);


        /// <summary>
        /// 执行交易所结算
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="settleday"></param>
        void SettleExchange(Exchange exchange, int settleday);
        
    }
}
