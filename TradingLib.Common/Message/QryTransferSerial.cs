using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    /// <summary>
    /// 查询出入金
    /// </summary>
    public class QryTransferSerialRequest:RequestPacket
    {
        public QryTransferSerialRequest()
        {
            _type = MessageTypes.QRYTRANSFERSERIAL;
            this.TradingAccount = string.Empty;
            this.BankID = string.Empty;
        }

        /// <summary>
        /// 交易帐号
        /// </summary>
        public string TradingAccount { get; set; }

        /// <summary>
        /// 对应银行的流水号
        /// </summary>
        public string BankID { get; set; }

        public override string ContentSerialize()
        {
            return this.TradingAccount + "," + this.BankID;
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');

            this.TradingAccount = rec[0];
            this.BankID = rec[1];
        }
    }

    /// <summary>
    /// 查询出入金回报
    /// </summary>
    public class RspQryTransferSerialResponse : RspResponsePacket
    {
        public RspQryTransferSerialResponse()
        {
            _type = MessageTypes.TRANSFERSERIALRESPONSE;
            this.CashTransaction = null;
        }

        public CashTransaction CashTransaction { get; set; }
        

        public override string ResponseSerialize()
        {
            if (this.CashTransaction == null)
                return string.Empty;
            return CashTransactionImpl.Serialize(this.CashTransaction);
        }

        public override void ResponseDeserialize(string content)
        {
            if (string.IsNullOrEmpty(content))
                this.CashTransaction = null;
            this.CashTransaction = CashTransactionImpl.Deserialize(content);
        }
    }
}
