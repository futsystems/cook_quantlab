using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 成交侧交易数据维护器
    /// 需要维护内存交易数据和记录到数据库对应表
    /// 同时需要在重启时加载并恢复内存数据状态
    /// parentid
    /// 记录该委托对应的父委托编号
    /// 从数据库读取属于该成交的所有委托
    /// OrderSpliterTracker则从子委托获得对应父委托列表,并加载相关map数据
    /// </summary>
    public class BrokerTracker
    {
        /// <summary>
        /// 委托管理器
        /// </summary>
        OrderTracker _orderTk = null;

        /// <summary>
        /// 持仓管理器
        /// </summary>
        LSPositionTracker _positionTk = null;

        /// <summary>
        /// 成交管理器
        /// </summary>
        TradeTracker _tradeTk = null;

        string _brokerToken = string.Empty;

        #region 持仓创建事件和平仓明细事件
        void NewPositionCloseDetail(Trade trade,PositionCloseDetail detail)
        {
            //在接口侧 数据暴露源头 设定平仓明细的Broker值 和 Breed类型
            /* Broker和Breed字段只是用于数据区分,不涉及具体的业务逻辑关系
             * 字段标注后方便从数据库加载到对应的对象中
             * 
             * **/
            detail.Broker = _brokerToken;
            detail.Breed = QSEnumOrderBreedType.BROKER;
            if (NewPositionCloseDetailEvent != null)
                NewPositionCloseDetailEvent(detail);
        }
        public event Action<PositionCloseDetail> NewPositionCloseDetailEvent;


        void NewPosition(Position pos)
        {
            if (NewPositionEvent != null)
                NewPositionEvent(pos);
        }
        public event Action<Position> NewPositionEvent;

        #endregion

        public BrokerTracker(string token)
        {
            _brokerToken = token;
            _orderTk = new OrderTracker();
            _positionTk = new LSPositionTracker(_brokerToken);
            //对外触发持仓生成和平仓明细事件
            _positionTk.NewPositionCloseDetailEvent += new Action<Trade,PositionCloseDetail>(NewPositionCloseDetail);
            _positionTk.NewPositionEvent += new Action<Position>(NewPosition);

            _tradeTk = new TradeTracker();
        }

        public void Clear()
        {
            _orderTk.Clear();
            _positionTk.Clear();
            _tradeTk.Clear();
        }

        /// <summary>
        /// 获得某个合约的多头持仓
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public Position GetLongPosition(string symbol)
        {
            return _positionTk[symbol, true];
        }

        /// <summary>
        /// 获得某个合约的空头持仓
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public Position GetShortPosition(string symbol)
        {
            return _positionTk[symbol, false];
        }

        /// <summary>
        /// 获得某个合约某个方向的持仓
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        public Position GetPosition(string symbol, bool side)
        {
            return _positionTk[symbol, side];
        }


        /// <summary>
        /// 获得某个合约某个方向的开仓委托
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        public IEnumerable<Order> GetPendingEntryOrders(string symbol, bool side)
        {
            return this.Orders.Where(o => o.Symbol.Equals(symbol) && o.IsPending() && (o.PositionSide == side) && (o.IsEntryPosition));
        }

        /// <summary>
        /// 获得某个合约某个方向的平仓委托
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        public IEnumerable<Order> GetPendingExitOrders(string symbol, bool side)
        {
            return this.Orders.Where(o => o.Symbol.Equals(symbol) && o.IsPending() && (o.PositionSide == side) && (!o.IsEntryPosition));
        }




        /// <summary>
        /// 所有委托
        /// </summary>
        public IEnumerable<Order> Orders
        {
            get
            {
                return _orderTk;
            }
        }

        /// <summary>
        /// 所有成交
        /// </summary>
        public IEnumerable<Position> Positions
        {
            get
            {
                return _positionTk;
            }
        }

        /// <summary>
        /// 所有成交
        /// </summary>
        public IEnumerable<Trade> Trades
        {
            get
            {
                return _tradeTk;
            }
        }


        /// <summary>
        /// 获得委托
        /// </summary>
        /// <param name="o"></param>
        public void GotOrder(Order o)
        {
            _orderTk.GotOrder(o);
        }

        /// <summary>
        /// 获得成交
        /// </summary>
        /// <param name="fill"></param>
        public void GotFill(Trade fill)
        {
            bool accept = false;
            _positionTk.GotFill(fill, out accept);
            if (accept)
            {
                _orderTk.GotFill(fill);
                _tradeTk.GotFill(fill);
            }
        }

        /// <summary>
        /// 响应行情
        /// </summary>
        /// <param name="k"></param>
        public void GotTick(Tick k)
        {
            _positionTk.GotTick(k);
        }

        /// <summary>
        /// 加载历史持仓数据
        /// </summary>
        /// <param name="pos"></param>
        public void GotPosition(PositionDetail pos)
        {
            _positionTk.GotPosition(pos);
        }

        /// <summary>
        /// 清空已结算持仓
        /// </summary>
        public void DropSettled()
        {
            _positionTk.DropSettled();
        }
    }
}
