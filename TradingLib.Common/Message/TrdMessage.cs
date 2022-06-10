using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 交易服务的其他类别信息
    /// </summary>
    public struct TrdMessage
    {
        public MessageTypes Type;
        public string Message;
        public string ClientID;
        public TrdMessage(string msg, MessageTypes type, string clientid)
        {
            Type = type;
            Message = msg;
            ClientID = clientid;
        }
    }
}
