using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    

    /// <summary>
    /// 委托操作请求
    /// </summary>
    public class OrderActionRequest : RequestPacket
    {
        public OrderActionRequest()
        {
            _type = MessageTypes.SENDORDERACTION;
            OrderAction = new OrderActionImpl();

        }

        public OrderAction OrderAction { get; set; }


        public override string ContentSerialize()
        {
            return OrderActionImpl.Serialize(OrderAction);
        }

        public override void ContentDeserialize(string reqstr)
        {
            OrderAction = OrderActionImpl.Deserialize(reqstr);
        }
    }


    /// <summary>
    /// 委托插入请求
    /// </summary>
    public class OrderInsertRequest : RequestPacket
    {
        public OrderInsertRequest()
        {
            _type = MessageTypes.SENDORDER;
            Order = new OrderImpl();

        }

        public Order Order { get; set; }

        public override string ContentSerialize()
        {
            return OrderImpl.Serialize(Order);
        }

        public override void ContentDeserialize(string reqstr)
        {
            Order = OrderImpl.Deserialize(reqstr);
        }

    }

   
    /// <summary>
    /// 行情 Response的统一封装 行情不需要Serialize,Deserizlize通过直接赋值进行
    /// </summary>
    public class TickNotify : NotifyResponsePacket
    {
        public TickNotify()
        {
            _type = MessageTypes.TICKNOTIFY;
            this.Tick = null;
        }

        public Tick Tick { get; set; }

        public override string Serialize()
        {
            if (this.Tick == null)
                return string.Empty;
            return TickImpl.Serialize(Tick);
        }

        public override void Deserialize(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                this.Tick = TickImpl.Deserialize(content);
            }
        }
    }

    
    /// <summary>
    /// 委托回报通知
    /// </summary>
    public class OrderNotify : NotifyResponsePacket
    {
        public OrderNotify()
        {
            _type = MessageTypes.ORDERNOTIFY;
        }
        public Order Order { get; set; }

        public override string ContentSerialize()
        {
            return OrderImpl.Serialize(Order);
        }

        public override void ContentDeserialize(string contentstr)
        {
            Order = OrderImpl.Deserialize(contentstr);
        }
    }

    /// <summary>
    /// 委托插入错误回报通知
    /// </summary>
    public class ErrorOrderNotify : ErrorNotifyResponsePacket
    {
        public ErrorOrderNotify()
        {
            _type = MessageTypes.ERRORORDERNOTIFY;
        }

        public Order Order { get; set; }

        public override string NotifySerialize()
        {
            return OrderImpl.Serialize(Order);
        }

        public override void NotifyDeserialize(string notify)
        {
            Order = OrderImpl.Deserialize(notify);
        }
    }

    /// <summary>
    /// 成交回报通知
    /// </summary>
    public class TradeNotify : NotifyResponsePacket
    {
        public TradeNotify()
        {
            _type = MessageTypes.EXECUTENOTIFY;
        }
        public Trade Trade { get; set; }

        public override string ContentSerialize()
        {
            return TradeImpl.Serialize(Trade);
        }

        public override void ContentDeserialize(string contentstr)
        {
            Trade = TradeImpl.Deserialize(contentstr);
        }
    }



    
    /// <summary>
    /// 委托操作回报通知
    /// </summary>
    public class OrderActionNotify : NotifyResponsePacket
    {
        public OrderActionNotify()
        {
            _type = MessageTypes.ORDERACTIONNOTIFY;
            OrderAction = new OrderActionImpl();
        }

        public OrderAction OrderAction { get; set; }

        public override void ContentDeserialize(string content)
        {
            OrderAction = OrderActionImpl.Deserialize(content);
        }

        public override string ContentSerialize()
        {
            return OrderActionImpl.Serialize(OrderAction);
        }
    }

    /// <summary>
    /// 委托操作错误回报通知
    /// </summary>
    public class ErrorOrderActionNotify : ErrorNotifyResponsePacket
    {
        public ErrorOrderActionNotify()
        {
            _type = MessageTypes.ERRORORDERACTIONNOTIFY;
            
        }

        public OrderAction OrderAction { get; set; }

        public override string NotifySerialize()
        {
            return OrderActionImpl.Serialize(OrderAction);
        }

        public override void NotifyDeserialize(string contentstr)
        {
            OrderAction = OrderActionImpl.Deserialize(contentstr);
        }
    }

    /// <summary>
    /// 持仓更新通知中采用的是accountposition用于加入更多的持仓数据
    /// 
    /// </summary>
    public class PositionNotify : NotifyResponsePacket
    {
        public PositionNotify()
        {
            _type = MessageTypes.POSITIONUPDATENOTIFY;
            Position = new PositionEx();
        }

        public PositionEx Position { get; set; }

        public override string ContentSerialize()
        {
            return PositionEx.Serialize(Position);
        }

        public override void ContentDeserialize(string contentstr)
        {
            Position = PositionEx.Deserialize(contentstr);
        }
    }

    /// <summary>
    /// 隔夜持仓明细通知
    /// 客户端如果通过交易数据累加获得当前数据则需要从隔夜持仓上叠加当日委托与成交来获得当前最新的交易状态
    /// 隔夜持仓通知 主要用于管理端恢复日内交易状态
    /// </summary>
    public class HoldPositionNotify : NotifyResponsePacket
    {
        public HoldPositionNotify()
        {
            _type = MessageTypes.OLDPOSITIONNOTIFY;//持仓数据通知 用于获得隔夜持仓数据
            this.PositionDetail = new PositionDetailImpl();
        }

        public PositionDetail PositionDetail { get; set; }
        public override string ContentSerialize()
        {
            return PositionDetailImpl.Serialize(this.PositionDetail);
        }

        public override void ContentDeserialize(string contentstr)
        {
            this.PositionDetail = PositionDetailImpl.Deserialize(contentstr);
        }

    }
}
