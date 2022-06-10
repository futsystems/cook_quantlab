using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 出入金通知
    /// 用于向客户端通知 出入金操作情况
    /// </summary>
    public class TradingNoticeNotify : NotifyResponsePacket
    {
        public TradingNoticeNotify()
        {
            _type = MessageTypes.TRADINGNOTICENOTIFY;
        }

        /// <summary>
        /// 发送时间
        /// </summary>
        public int SendTime { get; set; }

        string _content = string.Empty;
        /// <summary>
        /// 消息内容
        /// </summary>
        public string NoticeContent
        {
            get { return _content; }
            set
            {
                _content = value.Replace(',', ' ').Replace('^', ' ');
            }
        }



        public override string ContentSerialize()
        {
            return string.Format("{0},{1}", this.SendTime, this.NoticeContent);
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');
            this.SendTime = int.Parse(rec[0]);
            this.NoticeContent = rec[1];
        }
    }
}