using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    /// <summary>
    /// 确认结算单
    /// </summary>
    public class ConfirmSettlementRequest : RequestPacket
    {
        public string Account { get; set; }
        public ConfirmSettlementRequest()
        {
            _type = MessageTypes.CONFIRMSETTLEMENT;
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

    public class RspConfirmSettlementResponse : RspResponsePacket
    {

        public string TradingAccount { get; set; }
        /// <summary>
        /// 确认日期
        /// </summary>
        public int ConfirmDay { get; set; }

        /// <summary>
        /// 确认时间
        /// </summary>
        public int ConfirmTime { get; set; }
        public RspConfirmSettlementResponse()
        {
            _type = MessageTypes.CONFIRMSETTLEMENTRESPONSE;

        }

        public override string ResponseSerialize()
        {
            return this.TradingAccount + "," + ConfirmDay.ToString() + "," + ConfirmTime.ToString();

        }

        public override void ResponseDeserialize(string content)
        {
            string[] rec = content.Split(',');
            TradingAccount = rec[0];
            ConfirmDay = int.Parse(rec[1]);
            ConfirmTime = int.Parse(rec[2]);
        }


    }
}
