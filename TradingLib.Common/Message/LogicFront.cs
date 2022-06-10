using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

//定义前置与逻辑服务器之间业务消息
namespace TradingLib.Common
{

    public class LogicLiveRequest : RequestPacket
    {
        public LogicLiveRequest()
        {
            _type = MessageTypes.LOGICLIVEREQUEST;
        }

        public override string ContentSerialize()
        {
            return "";
        }

        public override void ContentDeserialize(string contentstr)
        {
            
        }
    }


    public class LogicLiveResponse : RspResponsePacket
    {
        public LogicLiveResponse()
        {
            _type = MessageTypes.LOGICLIVERESPONSE;
            this.Status = string.Empty;
        }

        public string Status { get; set; }
        public override string ResponseSerialize()
        {
            return "Live";
        }

        public override void ResponseDeserialize(string content)
        {
            this.Status = content;
        }
    }

    /// <summary>
    /// 注销客户端通知
    /// </summary>
    public class NotifyClearClient : NotifyResponsePacket
    {
        public NotifyClearClient()
        {
            _type = MessageTypes.NOTIFYCLEARCLIENT;
            this.SessionID = string.Empty;
        }

        public string SessionID { get; set; }

        public override string Serialize()
        {
            return string.IsNullOrEmpty(this.SessionID) ? string.Empty : this.SessionID;
        }

        public override void Deserialize(string content)
        {
            this.SessionID = content;
        }
    }

    public class NotifyRebooMQSrv : NotifyResponsePacket
    {
        public NotifyRebooMQSrv()
        {
            _type = MessageTypes.NOTIFYREBOOTMQSRV;
            this.CloseClient = false;
        }

        public bool CloseClient { get; set; }
        public override string Serialize()
        {
            return this.CloseClient.ToString();
        }

        public override void Deserialize(string content)
        {
            this.CloseClient = bool.Parse(content);
        }
    }

}
