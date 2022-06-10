///////////////////////////////////////////////////////////////////////////////////////
// 查询合约信息
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
    public class QrySymbolRequest : RequestPacket
    {
        /// <summary>
        /// 交易所
        /// </summary>
        public string ExchID { get; set; }
        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// 品种
        /// </summary>
        public string Security { get; set; }

        /// <summary>
        /// 获取某种合约
        /// </summary>
        public SecurityType SecurityType { get; set; }

        public QrySymbolRequest()
        {
            _type = MessageTypes.QRYSYMBOL;
            ExchID = string.Empty;
            Security = string.Empty;
            Symbol = string.Empty;
        }

        
        public override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(ExchID);
            sb.Append(d);
            sb.Append(Security);
            sb.Append(d);
            sb.Append(Symbol);
            sb.Append(d);
            sb.Append(SecurityType.ToString());
            return sb.ToString();
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');
            ExchID = rec[0];
            Security = rec[1];
            Symbol = rec[2];
            SecurityType = (API.SecurityType)Enum.Parse(typeof(API.SecurityType), rec[3]);
        }
    }

    public class RspQrySymbolResponse : RspResponsePacket
    {
        public RspQrySymbolResponse()
        {
            _type = MessageTypes.SYMBOLRESPONSE;
            InstrumentToSend = new Instrument();
        }

        public Instrument InstrumentToSend { get; set; }
        public override string ResponseSerialize()
        {
            return InstrumentToSend.GetSerializedString();
        }

        public override void ResponseDeserialize(string content)
        {
            InstrumentToSend = Instrument.Deserialize(content);
        }
    }


   
    
    
}
