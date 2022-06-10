using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class QryTradingParamsRequest : RequestPacket
    {
        public string Account { get; set; }
        public QryTradingParamsRequest()
        {
            _type = MessageTypes.QRYTRADINGPARAMS;
            this.Account = string.Empty;
        }

        public override string ContentSerialize()
        {
            return this.Account;
        }

        public override void ContentDeserialize(string contentstr)
        {
            this.Account = contentstr;
        }
    }

    public class RspQryTradingParamsResponse : RspResponsePacket
    {
        public RspQryTradingParamsResponse()
        {
            _type = MessageTypes.TRADINGPARAMSRESPONSE;
            this.Account = string.Empty;
            this.IncludeCloseProfit = true;
            this.MarginPriceType = QSEnumMarginPrice.OpenPrice;
            this.Algorithm = QSEnumAlgorithm.AG_All;
        }

        /// <summary>
        /// 交易帐户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 可用是否包含平仓
        /// </summary>
        public bool IncludeCloseProfit { get; set; }

        /// <summary>
        /// 保证金价格类型
        /// </summary>
        public QSEnumMarginPrice MarginPriceType { get; set; }

        /// <summary>
        /// 浮动盈亏算法
        /// </summary>
        public QSEnumAlgorithm Algorithm { get; set; }


        public override string ResponseSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.Account);
            sb.Append(d);
            sb.Append((int)this.MarginPriceType);
            sb.Append(d);
            sb.Append(this.IncludeCloseProfit ? "True" : "False");
            sb.Append(d);
            sb.Append((int)this.Algorithm);

            return sb.ToString();
        }

        public override void ResponseDeserialize(string content)
        {
            string[] rec = content.Split(',');
            this.Account = rec[0];
            this.MarginPriceType = (QSEnumMarginPrice)(int.Parse(rec[1]));
            this.IncludeCloseProfit = rec[2] == "True" ? true : false;
            this.Algorithm = (QSEnumAlgorithm)(int.Parse(rec[3]));
        }

    }
}