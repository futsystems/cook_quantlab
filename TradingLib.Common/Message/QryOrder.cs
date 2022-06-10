///////////////////////////////////////////////////////////////////////////////////////
// 查询委托
// 
//
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 查询委托请求
    /// 用于向服务端提交一个查询委托的请求
    /// 
    /// </summary>
    public class QryOrderRequest:RequestPacket
    {

        public QryOrderRequest()
        {
            _type = MessageTypes.QRYORDER;
            Account = string.Empty;
            Symbol = string.Empty;
            ExchID = string.Empty;
            OrderExchID = string.Empty;
            StartTime = 0;
            EndTime = 0;
        }
        
        /// <summary>
        /// 查询的交易帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 查询的合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 交易所编号
        /// </summary>
        public string ExchID { get; set; }

        /// <summary>
        /// 交易所的委托编号
        /// </summary>
        public string OrderExchID { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public int StartTime { get; set; }

        /// <summary>
        /// 截至时间
        /// </summary>
        public int EndTime { get; set; }

        /// <summary>
        /// 系统分配的委托ID
        /// </summary>
        public long OrderID { get; set; }

        public override bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(Account))
                    return false;
                return true;
            }
        }
        public override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.Account);
            sb.Append(d);
            sb.Append(this.Symbol);
            sb.Append(d);
            sb.Append(this.ExchID);
            sb.Append(d);
            sb.Append(this.OrderExchID);
            sb.Append(d);
            sb.Append(StartTime.ToString());
            sb.Append(d);
            sb.Append(EndTime.ToString());
            sb.Append(d);
            sb.Append(OrderID.ToString());
            return sb.ToString();
        }

        public override void ContentDeserialize(string reqstr)
        {
            string[] rec = reqstr.Split(',');
            this.Account = rec[0];
            this.Symbol = rec[1];
            this.ExchID = rec[2];
            this.OrderExchID = rec[3];
            int i=0;
            int.TryParse(rec[4],out i);
            this.StartTime = i;
            int.TryParse(rec[5], out i);
            this.EndTime = i;
            long orderid=0;
            long.TryParse(rec[6], out orderid);
            this.OrderID = orderid;

        }

        

        

    }

    public class RspQryOrderResponse : RspResponsePacket
    {
        public RspQryOrderResponse()
        {
            _type = MessageTypes.ORDERRESPONSE;
            this.OrderToSend = null;
        }

        Order _order = null;
        public Order OrderToSend { get { return _order; } set { _order = value; } }

        public override string ResponseSerialize()
        {
            if (OrderToSend == null)
                return "";
            return OrderImpl.Serialize(OrderToSend);
        }

        public override void ResponseDeserialize(string content)
        {
            if (string.IsNullOrEmpty(content))
                return;
            OrderToSend = OrderImpl.Deserialize(content);
        }
    }


}
