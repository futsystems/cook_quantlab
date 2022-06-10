using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{

    /// <summary>
    /// 管理端扩展请求
    /// </summary>
    public class MGRContribRequest:RequestPacket
    {
        public MGRContribRequest()
        {
            _type = MessageTypes.MGR_REQ_CONTRIB;
            this.ModuleID = string.Empty;
            this.CMDStr = string.Empty;
            this.Parameters = string.Empty;
        }

        /// <summary>
        /// 模块ID
        /// </summary>
        public string ModuleID { get; set; }

        /// <summary>
        /// 命令名
        /// </summary>
        public string CMDStr { get; set; }

        /// <summary>
        /// 命令参数
        /// </summary>
        public string Parameters { get; set; }

        public override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(ModuleID);
            sb.Append(d);
            sb.Append(this.CMDStr);
            sb.Append(d);
            sb.Append(this.Parameters);
            return sb.ToString();
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(new char[]{','}, 3);
            this.ModuleID = rec[0];
            this.CMDStr = rec[1];
            this.Parameters = rec[2];
        }
    }

    /// <summary>
    /// 管理端扩展通知回报
    /// </summary>
    public class NotifyMGRContribNotify : NotifyResponsePacket
    {
        public NotifyMGRContribNotify()
        {
            _type = MessageTypes.MGR_RTN_CONTRIB;
        }

        /// <summary>
        /// 模块ID
        /// </summary>
        public string ModuleID { get; set; }

        /// <summary>
        /// 命令名
        /// </summary>
        public string CMDStr { get; set; }

        /// <summary>
        /// 返回的Json字符串
        /// </summary>
        public string Result { get; set; }

        public override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.ModuleID);
            sb.Append(d);
            sb.Append(this.CMDStr);
            sb.Append(d);
            sb.Append(this.Result);

            return sb.ToString();
        }

        public override void ContentDeserialize(string content)
        {
            string[] rec = content.Split(new char[] { ',' }, 3);
            this.ModuleID = rec[0];
            this.CMDStr = rec[1];
            this.Result = rec[2];
        }
    }

    /// <summary>
    /// 管理端扩展应答回报
    /// </summary>
    public class RspMGRContribResponse : RspResponsePacket
    {
        public RspMGRContribResponse()
        {
            _type = MessageTypes.MGR_RSP_CONTRIB;
        }

        /// <summary>
        /// 模块ID
        /// </summary>
        public string ModuleID { get; set; }

        /// <summary>
        /// 命令名
        /// </summary>
        public string CMDStr { get; set; }

        /// <summary>
        /// 返回的Json字符串
        /// </summary>
        public string Result { get; set; }


        public override string ResponseSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.ModuleID);
            sb.Append(d);
            sb.Append(this.CMDStr);
            sb.Append(d);
            sb.Append(this.Result);

            return sb.ToString();
        }

        public override void ResponseDeserialize(string content)
        {
            string[] rec = content.Split(new char[] { ',' }, 3);
            this.ModuleID = rec[0];
            this.CMDStr = rec[1];
            this.Result = rec[2];
        }
    }

    /// <summary>
    /// 信息回报
    /// 操作完成或异常回报
    /// </summary>
    public class RspMGRResponse : RspResponsePacket
    {
        public RspMGRResponse()
        {
            _type = MessageTypes.MGR_RSP;
        }

        public override void ResponseDeserialize(string content)
        {
            base.ResponseDeserialize(content);
        }

        public override string ResponseSerialize()
        {
            return base.ResponseSerialize();
        }
    }
}
