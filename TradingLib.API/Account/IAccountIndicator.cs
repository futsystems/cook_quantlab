using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface IAccountIndicator
    {
        /// <summary>
        /// 交易账户产生委托回报事件
        /// </summary>
        event OrderDelegate GotOrderEvent;

        /// <summary>
        /// 交易账户产生成交回报事件
        /// </summary>
        event FillDelegate GotFillEvent;

        /// <summary>
        /// 新的平仓明细生成事件
        /// </summary>
        event Action<Trade, PositionCloseDetail> GotPositionCloseDetailEvent;

        /// <summary>
        /// 新的持仓明细生成事件
        /// </summary>
        event Action<Trade, PositionDetail> GotPositionDetailEvent;


        /// <summary>
        /// 触发交易账户的委托事件
        /// </summary>
        void FireOrderEvent(Order o);

        /// <summary>
        /// 触发交易账户的成交事件
        /// </summary>
        /// <param name="o"></param>
        void FireFillEvent(Trade f);

        /// <summary>
        /// 触发持仓明细生成事件
        /// </summary>
        /// <param name="f"></param>
        /// <param name="detail"></param>
        void FirePositoinDetailEvent(Trade f, PositionDetail detail);

        /// <summary>
        /// 触发平仓明细事件
        /// </summary>
        /// <param name="f"></param>
        /// <param name="close"></param>
        void FirePositionCloseDetailEvent(Trade f, PositionCloseDetail close);
    }
}
