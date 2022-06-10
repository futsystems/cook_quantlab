using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 请求查询注册银行帐号
    /// </summary>
    public class QryRegisterBankAccountRequest:RequestPacket
    {

        public QryRegisterBankAccountRequest()
        {
            _type = MessageTypes.QRYREGISTERBANKACCOUNT;
        }

        public string TradingAccount { get; set; }


        public override string ContentSerialize()
        {
            return this.TradingAccount;
        }

        public override void ContentDeserialize(string contentstr)
        {
            this.TradingAccount = contentstr;
        }
    }


    /// <summary>
    /// 注册银行帐号返回
    /// </summary>
    public class RspQryRegisterBankAccountResponse : RspResponsePacket
    {
        public RspQryRegisterBankAccountResponse()
        {
            _type = MessageTypes.REGISTERBANKACCOUNTRESPONSE;

            this.TradingAccount = string.Empty;
            this.BankName = string.Empty;
            this.BankID = string.Empty;
            this.BankAC = string.Empty;
        }


        /// <summary>
        /// 交易帐户
        /// </summary>
        public string TradingAccount { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get;set; }

        /// <summary>
        /// 银行ID
        /// </summary>
        public string BankID { get; set; }


        /// <summary>
        /// 银行交易帐号
        /// </summary>
        public string BankAC { get; set; }


        public override string  ResponseSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';

            sb.Append(this.TradingAccount);
            sb.Append(d);
            sb.Append(this.BankName);
            sb.Append(d);
            sb.Append(this.BankID);
            sb.Append(d);
            sb.Append(this.BankAC);
            return sb.ToString();
        }

        public override void  ResponseDeserialize(string content)
        {
            string[] rec = content.Split(',');

            this.TradingAccount = rec[0];
            this.BankName = rec[1];
            this.BankID = rec[2];
            this.BankAC = rec[3];
        }
    }
}
