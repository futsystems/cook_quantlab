///////////////////////////////////////////////////////////////////////////////////////
// 用于查询客户端基本帐户信息
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
    public class QryAccountInfoRequest:RequestPacket
    {
        public string Account { get; set; }

        public QryAccountInfoRequest()
        {
            _type = MessageTypes.QRYACCOUNTINFO;
            Account = string.Empty;
        }

        public override string ContentSerialize()
        {
            return Account;
        }

        public override void ContentDeserialize(string contentstr)
        {
            Account = contentstr;
        }

    }

    public class RspQryAccountInfoResponse : RspResponsePacket
    {

        public AccountInfo AccInfo { get; set; }
        public RspQryAccountInfoResponse()
        {
            _type = MessageTypes.ACCOUNTINFORESPONSE;
            AccInfo = null;
        }

        public override string ResponseSerialize()
        {
            return AccountInfo.Serialize(AccInfo);
        }

        public override void ResponseDeserialize(string content)
        {
            AccInfo = AccountInfo.Deserialize(content);
        }

    }
}
