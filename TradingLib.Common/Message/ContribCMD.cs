using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{

    public class ContribRequest : RequestPacket
    {
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

        public ContribRequest()
        {
            _type = API.MessageTypes.CONTRIBREQUEST;
        }

        public override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.ModuleID);
            sb.Append(d);
            sb.Append(this.CMDStr);
            sb.Append(d);
            sb.Append(this.Parameters);
            return sb.ToString();
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(new char[] { ',' }, 3);
            this.ModuleID = rec[0];
            this.CMDStr = rec[1];
            this.Parameters = rec[2];
        }

    }

    public class RspContribResponse : RspResponsePacket
    {
        public RspContribResponse()
        {
            _type = MessageTypes.CONTRIBRESPONSE;
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
}
