///////////////////////////////////////////////////////////////////////////////////////
// 查询成交数据
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
    public class QryTradeRequest:RequestPacket
    {
        public QryTradeRequest()
        {
            _type = MessageTypes.QRYTRADE;
            ExchID = string.Empty;
            Symbol = string.Empty;
            Account = string.Empty;
            StartTime = 0;
            EndTime = 0;
            TradeID = 0;

        }
        

        

        /// <summary>
        /// 交易帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 交易所ID
        /// </summary>
        public string ExchID { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public int StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public int EndTime { get; set; }

        /// <summary>
        /// 成交编号
        /// </summary>
        public long TradeID { get; set; }

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
            sb.Append(Account);
            sb.Append(d);
            sb.Append(Symbol);
            sb.Append(d);
            sb.Append(ExchID);
            sb.Append(d);
            sb.Append(StartTime.ToString());
            sb.Append(d);
            sb.Append(EndTime.ToString());
            sb.Append(d);
            sb.Append(TradeID.ToString());

            return sb.ToString();
        }

        public override void ContentDeserialize(string reqstr)
        {
            string[] rec = reqstr.Split(',');
            this.Account = rec[0];
            this.Symbol = rec[1];
            this.ExchID = rec[2];
            this.StartTime = int.Parse(rec[3]);
            this.EndTime = int.Parse(rec[4]);
            this.TradeID = long.Parse(rec[5]);

        }


    }


    public class RspQryTradeResponse : RspResponsePacket
    {
        public RspQryTradeResponse()
        {
            _type = MessageTypes.TRADERESPONSE;
            this.TradeToSend = null;
        }
        //public RspQryTradeResponse(Trade trade, bool islast)
        //{
        //    _type = MessageTypes.TRADERESPONSE;
        //    TradeToSend = trade;
        //    IsLast = islast;
        //}

        public Trade TradeToSend { get; set; }
        public override string ResponseSerialize()
        {
            if (TradeToSend == null)
                return "";
            return TradeImpl.Serialize(TradeToSend);
        }

        public override void ResponseDeserialize(string content)
        {
            if (string.IsNullOrEmpty(content))
                return;
            TradeToSend = TradeImpl.Deserialize(content);
        }
    }
}
