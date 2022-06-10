///////////////////////////////////////////////////////////////////////////////////////
// 用于查询投资者信息比如姓名等
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
    public class QryInvestorRequest:RequestPacket
    {

        public string Account { get; set; }
        public QryInvestorRequest()
        {
            _type = MessageTypes.QRYINVESTOR;
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

    /// <summary>
    /// 查询投资者返回
    /// </summary>
    public class RspQryInvestorResponse : RspResponsePacket
    {
        public RspQryInvestorResponse()
        {
            _type = MessageTypes.INVESTORRESPONSE;
            TradingAccount = string.Empty;
            Email = string.Empty;
            Mobile = string.Empty;
            NickName = string.Empty;
        }

        /// <summary>
        /// 交易帐户
        /// </summary>
        public string TradingAccount { get; set; }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }


        public override string ResponseSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d=',';
            sb.Append(this.TradingAccount);
            sb.Append(d);
            sb.Append(this.Email);
            sb.Append(d);
            sb.Append(this.Mobile);
            sb.Append(d);
            sb.Append(this.NickName);
            return sb.ToString();
        }

        public override void ResponseDeserialize(string content)
        {
            string[] rec = content.Split(',');
            this.TradingAccount = rec[0];
            this.Email = rec[1];
            this.Mobile = rec[2];
            this.NickName = rec[3];
        }
    }
}
