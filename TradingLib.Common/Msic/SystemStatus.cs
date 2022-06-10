using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class SystemStatus
    {
        public SystemStatus()
        {
            this.StartUpTime = 0;
            this.LastSettleday = 0;
            this.Tradingday = 0;
            this.NextSettleTime = 0;
            this.IsSettleNormal = true;
            this.IsClearCentreLive = false;
            this.TotalAccountNum = 0;

        }

        /// <summary>
        /// 系统启动时间
        /// </summary>
        public long StartUpTime { get; set; }

        /// <summary>
        /// 上个结算日
        /// </summary>
        public int LastSettleday { get; set; }

        /// <summary>
        /// 上个结算日未结算委托记录
        /// 如果上个结算日有未结算委托记录则表明结算过程异常
        /// </summary>
        public int UnsettledAcctOrderNumOfPreSettleday{ get; set; }

        /// <summary>
        /// 上个结算日未结算成交记录
        /// </summary>
        public int UnsettledAcctTradeNumOfPreSettleday { get; set; }

        /// <summary>
        /// 上个结算日未结算交易所结算记录
        /// </summary>
        public int UnsettledExchangeSettlementNumOfPreSettleday { get; set; }

        /// <summary>
        /// 上个结算日未计算委托记录(Broker)
        /// </summary>
        public int UnsettledBrokerOrderNumOfPreSettleday { get; set; }

        /// <summary>
        /// 当前交易日
        /// </summary>
        public int Tradingday { get; set; }

        /// <summary>
        /// 下次结算时间
        /// </summary>
        public long NextSettleTime { get; set; }

        /// <summary>
        /// 结算系统是否正常
        /// </summary>
        public bool IsSettleNormal { get; set; }

        /// <summary>
        /// 结算中心状态
        /// </summary>
        public QSEnumSettleMode SettleMode { get; set; }

        /// <summary>
        /// 清算中心是否开启
        /// </summary>
        public bool IsClearCentreLive { get; set; }

        /// <summary>
        /// 账户总数
        /// </summary>
        public int TotalAccountNum { get; set; }


        /// <summary>
        /// 所有委托数量
        /// </summary>
        public int TotalOrderNum { get; set; }

        /// <summary>
        /// 所有成交数量
        /// </summary>
        public int TotalTradeNum { get; set; }
        public  string Serialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';

            sb.Append(this.StartUpTime);
            sb.Append(d);
            sb.Append(this.LastSettleday);
            sb.Append(d);
            sb.Append(this.Tradingday);
            sb.Append(d);
            sb.Append(this.NextSettleTime);
            sb.Append(d);
            sb.Append(this.IsSettleNormal);
            sb.Append(d);
            sb.Append(this.IsClearCentreLive);
            sb.Append(d);
            sb.Append(this.TotalAccountNum.ToString());
            sb.Append(d);
            sb.Append(this.UnsettledAcctOrderNumOfPreSettleday);
            sb.Append(d);
            sb.Append(this.UnsettledBrokerOrderNumOfPreSettleday);
            sb.Append(d);
            sb.Append(this.TotalOrderNum);
            sb.Append(d);
            sb.Append(this.TotalTradeNum);
            sb.Append(d);
            sb.Append(this.UnsettledExchangeSettlementNumOfPreSettleday);
            sb.Append(d);
            sb.Append(this.UnsettledAcctTradeNumOfPreSettleday);
            return sb.ToString();
        }

        public  void Deserialize(string content)
        {
            string[] rec = content.Split(',');
            this.StartUpTime = long.Parse(rec[0]);
            this.LastSettleday = int.Parse(rec[1]);
            this.Tradingday = int.Parse(rec[2]);
            this.NextSettleTime = long.Parse(rec[3]);
            this.IsSettleNormal = bool.Parse(rec[4]);
            this.IsClearCentreLive = bool.Parse(rec[5]);
            this.TotalAccountNum = int.Parse(rec[6]);
            this.UnsettledAcctOrderNumOfPreSettleday = int.Parse(rec[7]);
            this.UnsettledBrokerOrderNumOfPreSettleday = int.Parse(rec[8]);
            this.TotalOrderNum = int.Parse(rec[9]);
            this.TotalTradeNum = int.Parse(rec[10]);
            this.UnsettledExchangeSettlementNumOfPreSettleday = int.Parse(rec[11]);
            this.UnsettledAcctTradeNumOfPreSettleday = int.Parse(rec[12]);
        }
    }
}
