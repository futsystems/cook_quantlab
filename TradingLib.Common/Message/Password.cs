using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class ReqChangePasswordRequest:RequestPacket
    {
        public ReqChangePasswordRequest()
        {
            _type = MessageTypes.REQCHANGEPASS;
        }

        /// <summary>
        /// 交易帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }

        public override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.Account);
            sb.Append(d);
            sb.Append(this.OldPassword);
            sb.Append(d);
            sb.Append(this.NewPassword);
            return sb.ToString();
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');
            this.Account = rec[0];
            this.OldPassword = rec[1];
            this.NewPassword = rec[2];
        }


    }

    public class RspReqChangePasswordResponse : RspResponsePacket
    {
        public RspReqChangePasswordResponse()
        {
            _type = MessageTypes.CHANGEPASSRESPONSE;
        }

        
    }
}
