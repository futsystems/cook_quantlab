using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 货币汇率
    /// </summary>
    public class ExchangeRate
    {

        public ExchangeRate()
        {
            this.ID = 0;
            this.Domain_ID = 0;
            this.Settleday = 0;
            this.Currency = CurrencyType.RMB;
            this.AskRate = 1;
            this.IntermediateRate = 1;
            this.BidRate = 1;
            this.UpdateTime = Util.ToTLDateTime();
        }

        /// <summary>
        /// 数据库全局ID
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// 分区编号
        /// </summary>
        public int Domain_ID { get; set; }

        /// <summary>
        /// 结算日 标注该汇率数据属于哪个结算日
        /// </summary>
        public int Settleday { get; set; }

        /// <summary>
        /// 货币类别
        /// </summary>
        public CurrencyType Currency { get; set; }

        /// <summary>
        /// 卖价
        /// </summary>
        public decimal AskRate { get; set; }

        /// <summary>
        /// 中间价
        /// </summary>
        public decimal IntermediateRate { get; set; }

        /// <summary>
        /// 买价
        /// </summary>
        public decimal BidRate { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public long UpdateTime { get; set; }

        public static ExchangeRate Deserialize(string content)
        {
            ExchangeRate rate = new ExchangeRate();
            string[] rec = content.Split(',');
            rate.ID = int.Parse(rec[0]);
            rate.Settleday = int.Parse(rec[1]);
            rate.Currency = rec[2].ParseEnum<CurrencyType>();
            rate.AskRate = decimal.Parse(rec[3]);
            rate.IntermediateRate = decimal.Parse(rec[4]);
            rate.BidRate = decimal.Parse(rec[5]);
            rate.UpdateTime = long.Parse(rec[6]);
            rate.Domain_ID = int.Parse(rec[7]);
            return rate;
        }

        public static string Serialize(ExchangeRate rate)
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(rate.ID);
            sb.Append(d);
            sb.Append(rate.Settleday);
            sb.Append(d);
            sb.Append(rate.Currency);
            sb.Append(d);
            sb.Append(rate.AskRate);
            sb.Append(d);
            sb.Append(rate.IntermediateRate);
            sb.Append(d);
            sb.Append(rate.BidRate);
            sb.Append(d);
            sb.Append(rate.UpdateTime);
            sb.Append(d);
            sb.Append(rate.Domain_ID);
            return sb.ToString();
        }
    }
}
