using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    public class QryNoticeRequest:RequestPacket
    {
        public string Account { get; set; }
        public QryNoticeRequest()
        {
            _type = MessageTypes.QRYNOTICE;
        }

        public override string ContentSerialize()
        {
            return this.Account;
        }

        public override void ContentDeserialize(string contentstr)
        {
            this.Account = contentstr;
        }
    }

    public class RspQryNoticeResponse : RspResponsePacket
    {
        public RspQryNoticeResponse()
        {
            _type = MessageTypes.NOTICERESPONSE;
            NoticeContent = string.Empty;
        }

        public string NoticeContent { get; set; }

        public override string ResponseSerialize()
        {

            return this.NoticeContent;

        }

        public override void ResponseDeserialize(string content)
        {
            this.NoticeContent = content;
        }
    }
}
