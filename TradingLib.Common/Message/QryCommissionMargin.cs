using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{

    /// <summary>
    /// 查询合约手续费
    /// </summary>
    public class QryInstrumentCommissionRateRequest : RequestPacket
    {
        /// <summary>
        /// 查询合约手续费率
        /// </summary>
        public QryInstrumentCommissionRateRequest()
        {
            _type = MessageTypes.QRYINSTRUMENTCOMMISSIONRATE;
            this.TradingAccount = string.Empty;
            this.Exchange = string.Empty;
            this.Symbol = string.Empty;
        }

        /// <summary>
        /// 交易帐户
        /// </summary>
        public string TradingAccount { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }


        public string Exchange { get; set; }

        public override string ContentSerialize()
        {
            return TradingAccount + "," + Symbol +"," + this.Exchange;
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');
            this.TradingAccount = rec[0];
            this.Symbol = rec[1];
            this.Exchange = rec[2];
        }
    }

    /// <summary>
    /// 合约手续费率回报
    /// </summary>
    public class RspQryInstrumentCommissionRateResponse:RspResponsePacket
    {
        public RspQryInstrumentCommissionRateResponse()
        {
            _type = MessageTypes.INSTRUMENTCOMMISSIONRATERESPONSE;

            this.TradingAccount = string.Empty;
            this.Symbol = string.Empty;
            this.OpenRatioByMoney = 0;
            this.OpenRatioByVolume = 0;
            this.CloseRatioByMoney = 0;
            this.CloseRatioByVolume = 0;
            this.CloseTodayRatioByMoney = 0;
            this.CloseTodayRatioByVolume = 0;
        }

        /// <summary>
        /// 交易帐户
        /// </summary>
        public string TradingAccount { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// 开仓手续费率 按比例收取
        /// </summary>
        public decimal OpenRatioByMoney { get; set; }

        /// <summary>
        /// 开仓手续费 按量收取
        /// </summary>
        public decimal OpenRatioByVolume { get; set; }

        /// <summary>
        /// 平今手续费率 按比例收取
        /// </summary>
        public decimal CloseRatioByMoney { get; set; }

        /// <summary>
        /// 平今手续费 按量收取
        /// </summary>
        public decimal CloseRatioByVolume { get; set; }

        /// <summary>
        /// 平昨手续费率 按比例收取
        /// </summary>
        public decimal CloseTodayRatioByMoney { get; set; }

        /// <summary>
        /// 平昨手续费 按量收取
        /// </summary>
        public decimal CloseTodayRatioByVolume { get; set; }


        public void FillCommissionCfg(CommissionConfig cfg)
        {
            this.TradingAccount = cfg.Account;
            this.Symbol = cfg.Symbol;
            this.OpenRatioByMoney = cfg.OpenRatioByMoney;
            this.OpenRatioByVolume = cfg.OpenRatioByVolume;
            this.CloseRatioByMoney = cfg.CloseRatioByMoney;
            this.CloseRatioByVolume = cfg.CloseRatioByVolume;
            this.CloseTodayRatioByMoney = cfg.CloseTodayRatioByMoney;
            this.CloseTodayRatioByVolume = cfg.CloseTodayRatioByVolume;

        }
        public override string ResponseSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.TradingAccount);
            sb.Append(d);
            sb.Append(this.Symbol);
            sb.Append(d);
            sb.Append(this.OpenRatioByMoney);
            sb.Append(d);
            sb.Append(this.OpenRatioByVolume);
            sb.Append(d);
            sb.Append(this.CloseRatioByMoney);
            sb.Append(d);
            sb.Append(this.CloseRatioByVolume);
            sb.Append(d);
            sb.Append(this.CloseTodayRatioByMoney);
            sb.Append(d);
            sb.Append(this.CloseTodayRatioByVolume);
            return sb.ToString();
        }

        public override void ResponseDeserialize(string content)
        {
            string[] rec = content.Split(',');
            this.TradingAccount = rec[0];
            this.Symbol = rec[1];
            this.OpenRatioByMoney = decimal.Parse(rec[2]);
            this.OpenRatioByVolume = decimal.Parse(rec[3]);
            this.CloseRatioByMoney = decimal.Parse(rec[4]);
            this.CloseRatioByVolume = decimal.Parse(rec[5]);
            this.CloseTodayRatioByMoney = decimal.Parse(rec[6]);
            this.CloseTodayRatioByVolume = decimal.Parse(rec[7]);

        }
    }


    public class QryInstrumentMarginRateRequest : RequestPacket
    {
        /// <summary>
        /// 查询合约手续费率
        /// </summary>
        public QryInstrumentMarginRateRequest()
        {
            _type = MessageTypes.QRYINSTRUMENTMARGINRATE;
            this.TradingAccount = string.Empty;
            this.Symbol = string.Empty;
            this.Exchange = string.Empty;
        }

        /// <summary>
        /// 交易帐户
        /// </summary>
        public string TradingAccount { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange { get; set; }

        public override string ContentSerialize()
        {
            return TradingAccount + "," + Symbol + "," + this.Exchange;
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');
            this.TradingAccount = rec[0];
            this.Symbol = rec[1];
            this.Exchange = rec[2];
        }
    }


    public class RspQryInstrumentMarginRateResponse : RspResponsePacket
    {
        public RspQryInstrumentMarginRateResponse()
        {
            _type = MessageTypes.INSTRUMENTMARGINRATERESPONSE;
            this.TradingAccount = string.Empty;
            this.LongMarginRatioByMoney = 0;
            this.ShortMarginRatioByMoney = 0;
            this.LongMarginRatioByVolume = 0;
            this.ShortMarginRatioByVoume = 0;
        }

        public string TradingAccount { get; set; }

        public string Symbol { get; set; }

        /// <summary>
        /// 多头保证金你率
        /// </summary>
        public decimal LongMarginRatioByMoney { get; set; }

        /// <summary>
        /// 按量
        /// </summary>
        public decimal LongMarginRatioByVolume { get; set; }

        /// <summary>
        /// 空头保证金 按金额 10%
        /// </summary>
        public decimal ShortMarginRatioByMoney { get; set; }


        /// <summary>
        /// 空头保证金按量 每手5000
        /// </summary>
        public decimal ShortMarginRatioByVoume { get; set; }

        public void FillMarginCfg(MarginConfig cfg)
        {
            this.LongMarginRatioByMoney = cfg.LongMarginRatioByMoney;
            this.LongMarginRatioByVolume = cfg.LongMarginRatioByVolume;
            this.ShortMarginRatioByMoney = cfg.ShortMarginRatioByMoney;
            this.ShortMarginRatioByVoume = cfg.ShortMarginRatioByVoume;

        }


        public override string ResponseSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.TradingAccount);
            sb.Append(d);
            sb.Append(this.Symbol);
            sb.Append(d);
            sb.Append(this.LongMarginRatioByMoney);
            sb.Append(d);
            sb.Append(this.LongMarginRatioByVolume);
            sb.Append(d);
            sb.Append(this.ShortMarginRatioByMoney);
            sb.Append(d);
            sb.Append(this.ShortMarginRatioByVoume);
            return sb.ToString();
        }

        public override void ResponseDeserialize(string content)
        {
            string[] rec = content.Split(',');
            this.TradingAccount = rec[0];
            this.Symbol = rec[1];
            this.LongMarginRatioByMoney = decimal.Parse(rec[2]);
            this.LongMarginRatioByVolume = decimal.Parse(rec[3]);
            this.ShortMarginRatioByMoney = decimal.Parse(rec[4]);
            this.ShortMarginRatioByVoume = decimal.Parse(rec[5]);
        }
    }
}
