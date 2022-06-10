using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 委托插入请求
    /// </summary>
    public class BOOrderInsertRequest : RequestPacket
    {
        public BOOrderInsertRequest()
        {
            _type = MessageTypes.BOSENDORDER;
            Order = null;

        }

        public BinaryOptionOrder Order { get; set; }

        public override string ContentSerialize()
        {
            if (this.Order == null) return string.Empty;
            return BinaryOptionOrderImpl.Serialize(this.Order);
            
        }

        public override void ContentDeserialize(string reqstr)
        {
            if (string.IsNullOrEmpty(reqstr))
                this.Order = null;
            this.Order = BinaryOptionOrderImpl.Deserialize(reqstr);

        }

    }

    /// <summary>
    /// 委托回报通知
    /// </summary>
    public class BOOrderNotify : NotifyResponsePacket
    {
        public BOOrderNotify()
        {
            _type = MessageTypes.BOORDERNOTIFY;
        }
        public BinaryOptionOrder Order { get; set; }

        public override string ContentSerialize()
        {
            if (this.Order == null) return string.Empty;
            return BinaryOptionOrderImpl.Serialize(this.Order);
        }

        public override void ContentDeserialize(string contentstr)
        {
            if (string.IsNullOrEmpty(contentstr))
                this.Order = null;
            this.Order = BinaryOptionOrderImpl.Deserialize(contentstr);
        }
    }


    /// <summary>
    /// 委托插入错误回报通知
    /// </summary>
    public class BOOrderErrorNotify : ErrorNotifyResponsePacket
    {
        public BOOrderErrorNotify()
        {
            _type = MessageTypes.BOORDERERRORNOTIFY;
        }

        public BinaryOptionOrder Order { get; set; }

        public override string NotifySerialize()
        {
            if (this.Order == null) return string.Empty;
            return BinaryOptionOrderImpl.Serialize(this.Order);
        }

        public override void NotifyDeserialize(string notify)
        {
            if (string.IsNullOrEmpty(notify))
                this.Order = null;
            this.Order = BinaryOptionOrderImpl.Deserialize(notify);
        }
    }


}
