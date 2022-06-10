///////////////////////////////////////////////////////////////////////////////////////
// 查询结算确认
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
    public class QrySettleInfoConfirmRequest:RequestPacket
    {
        /// <summary>
        /// 交易帐号
        /// </summary>
        public string Account { get; set; }

        public QrySettleInfoConfirmRequest()
        {
            _type = MessageTypes.QRYSETTLEINFOCONFIRM;
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

    public class RspQrySettleInfoConfirmResponse : RspResponsePacket
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
        public RspQrySettleInfoConfirmResponse()
        {
            _type = MessageTypes.SETTLEINFOCONFIRMRESPONSE;
            
        }

        public override string ResponseSerialize()
        {
            return this.TradingAccount+","+ConfirmDay.ToString() + "," + ConfirmTime.ToString();

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
