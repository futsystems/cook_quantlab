using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class QryContractBankRequest:RequestPacket
    {
        public QryContractBankRequest()
        {
            _type = MessageTypes.QRYCONTRACTBANK;

        }

        public override string ContentSerialize()
        {
            return base.ContentSerialize();
        }

        public override void ContentDeserialize(string contentstr)
        {
            base.ContentDeserialize(contentstr);
        }
    }

    public class RspQryContractBankResponse : RspResponsePacket
    {
        public RspQryContractBankResponse()
        {
            _type = MessageTypes.CONTRACTBANKRESPONSE;
            this.BankName = string.Empty;
            this.BankID = string.Empty;
            this.BankBrchID = string.Empty;
        }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 银行编号
        /// </summary>
        public string BankID { get; set; }

        /// <summary>
        /// 银行分支编号
        /// </summary>
        public string BankBrchID { get; set; }


        public override string ResponseSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.BankName);
            sb.Append(d);
            sb.Append(this.BankID);
            sb.Append(d);
            sb.Append(this.BankBrchID);

            return sb.ToString();
        }

        public override void ResponseDeserialize(string content)
        {
            string[] rec = content.Split(',');
            this.BankName = rec[0];
            this.BankID = rec[1];
            this.BankBrchID = rec[2];
        }

    }
}
