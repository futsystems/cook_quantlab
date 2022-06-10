using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class QryMarketDataRequest:RequestPacket
    {
        public QryMarketDataRequest()
        {
            _type = MessageTypes.QRYMARKETDATA;
            
        }

        public string Symbol { get; set; }

        public override string ContentSerialize()
        {
            return this.Symbol;
        }

        public override void ContentDeserialize(string contentstr)
        {
            this.Symbol = contentstr;
        }
    }

    public class RspQryMarketDataResponse : RspResponsePacket
    {
        public RspQryMarketDataResponse()
        {
            _type = MessageTypes.MARKETDATARESPONSE;
            this.TickToSend = new TickImpl();
        }

        public Tick TickToSend { get; set; }

        public override string ResponseSerialize()
        {
            return TickImpl.Serialize(this.TickToSend);
        }

        public override void ResponseDeserialize(string content)
        {
            this.TickToSend = TickImpl.Deserialize(content);
        }
    }
}
