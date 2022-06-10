using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;


namespace TradingLib.Common
{
    /// <summary>
    /// 证券品种簇定义
    /// </summary>
    public class SecurityFamilyImpl:SecurityFamily
    {
        public int ID { get; set; }

        /// <summary>
        /// 品种代号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 品种名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 货币类别
        /// </summary>
        public CurrencyType Currency { get; set; }

        /// <summary>
        /// 品种类别
        /// </summary>
        public SecurityType Type { get; set; }

        /// <summary>
        /// 所属交易所
        /// </summary>
        public Exchange Exchange { get; set; }


        /// <summary>
        /// 乘数
        /// </summary>
        public int Multiple { get; set; }


        /// <summary>
        /// 最小价格变动
        /// </summary>
        public decimal PriceTick { get; set; }

        /// <summary>
        /// 是否可交易
        /// </summary>
        public bool Tradeable { get; set; }

        /// <summary>
        /// 底层证券
        /// 某个衍生品证券会依赖于底层证券
        /// 比如沪深300股指期货依赖于沪深300，沪深300股指期权依赖于沪深300
        /// 沪深300不可交易，而起衍生品证券可以进行交易
        /// </summary>
        public SecurityFamily UnderLaying { get; set; }

        /// <summary>
        /// 开仓手续费
        /// </summary>
        public decimal EntryCommission { get; set; }

        /// <summary>
        /// 平仓手续费
        /// </summary>
        public decimal ExitCommission { get; set; }

        /// <summary>
        /// 平今手续费
        /// </summary>
        public decimal ExitCommissionToday { get; set; }

        /// <summary>
        /// 保证金比例
        /// </summary>
        public decimal Margin { get; set; }

        /// <summary>
        /// 额外保证金字段
        /// 用于在基本保证金外提供额外质押
        /// </summary>
        public decimal ExtraMargin { get; set; }

        /// <summary>
        /// 过夜保证金,如果需要过夜则需要提供Maintance保证金
        /// </summary>
        public decimal MaintanceMargin { get; set; }

        int _domainid = 0;
        public int Domain_ID { get { return _domainid; } set { _domainid = value; } }

        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrEmpty(this.Code));
            }
        }
        public override string ToString()
        {
            return ID.ToString() + " Code:" + Code.ToString() + " Name:" + Name.ToString() + " Currency:" + Currency.ToString() + " Exch:" + Util.SafeToString(Exchange) + " Mutil:" + Multiple.ToString() + " PriceTick:" + PriceTick.ToString() + " Tradeable:" + Tradeable.ToString() + " Underlaying:" + Util.SafeToString(UnderLaying) + " EntryC:" + EntryCommission.ToString() + " ExitC:" + ExitCommission.ToString() + " Margin:" + Margin.ToString() + " underlaying_fk:" + Util.SafeToString(underlaying_fk); 
        }

        /// <summary>
        /// 交易时间段 开市时间
        /// </summary>
        public MarketTime MarketTime { get; set; }


        /// <summary>
        /// 该品种对应的行情源
        /// </summary>
        public QSEnumDataFeedTypes DataFeed { get; set; }

        public static string Serialize(SecurityFamilyImpl sec)
        {
            if (sec == null) return string.Empty;
            StringBuilder sb = new StringBuilder();
            char d = ',';

            sb.Append(sec.ID.ToString());
            sb.Append(d);
            sb.Append(sec.Code);
            sb.Append(d);
            sb.Append(sec.Name);
            sb.Append(d);
            sb.Append(sec.Currency.ToString());
            sb.Append(d);
            sb.Append(sec.Type.ToString());
            sb.Append(d);
            sb.Append(sec.Multiple.ToString());//5
            sb.Append(d);
            sb.Append(sec.PriceTick.ToString());
            sb.Append(d);
            sb.Append(sec.Tradeable.ToString());
            sb.Append(d);
            sb.Append(sec.EntryCommission.ToString());
            sb.Append(d);
            sb.Append(sec.ExitCommission.ToString());
            sb.Append(d);
            sb.Append(sec.Margin.ToString());//10
            sb.Append(d);
            sb.Append(sec.ExtraMargin.ToString());
            sb.Append(d);
            sb.Append(sec.MaintanceMargin.ToString());
            sb.Append(d);
            sb.Append(sec.exchange_fk.ToString());//exchange
            sb.Append(d);
            sb.Append(sec.underlaying_fk.ToString());//securityfamily
            sb.Append(d);
            sb.Append(sec.mkttime_fk.ToString());//markettime
            sb.Append(d);
            sb.Append(sec.ExitCommissionToday);
            sb.Append(d);
            sb.Append(sec.DataFeed);
            
            return sb.ToString();
        }

        //对象的嵌套是在对象初始化时候通过fk获得的
        public int exchange_fk { get; set; }
        public int ExchangeFK { get { return this.Exchange != null ? (this.Exchange as ExchangeImpl).ID : 0; } }
        public int underlaying_fk{get;set;}
        public int UnderLayingFK { get { return this.UnderLaying != null ? (this.UnderLaying as SecurityFamilyImpl).ID : 0; } }
        public int mkttime_fk{get;set;}
        public int MarketTimeFK { get { return this.MarketTime != null ? (this.MarketTime as MarketTimeImpl).ID : 0; } }

        public static SecurityFamilyImpl Deserialize(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            SecurityFamilyImpl sec = new SecurityFamilyImpl();
            string[] rec = content.Split(',');
            sec.ID = int.Parse(rec[0]);
            sec.Code = rec[1];
            sec.Name = rec[2];
            sec.Currency = (CurrencyType)Enum.Parse(typeof(CurrencyType), rec[3]);
            sec.Type = (SecurityType)Enum.Parse(typeof(SecurityType), rec[4]);
            sec.Multiple = int.Parse(rec[5]);
            sec.PriceTick = decimal.Parse(rec[6]);
            sec.Tradeable = bool.Parse(rec[7]);
            sec.EntryCommission = decimal.Parse(rec[8]);
            sec.ExitCommission = decimal.Parse(rec[9]);
            sec.Margin = decimal.Parse(rec[10]);
            sec.ExtraMargin = decimal.Parse(rec[11]);
            sec.MaintanceMargin = decimal.Parse(rec[12]);
            sec.exchange_fk = int.Parse(rec[13]);
            sec.underlaying_fk = int.Parse(rec[14]);
            sec.mkttime_fk = int.Parse(rec[15]);
            sec.ExitCommissionToday = decimal.Parse(rec[16]);
            sec.DataFeed = (QSEnumDataFeedTypes)Enum.Parse(typeof(QSEnumDataFeedTypes), rec[17]);

            return sec;
        }
    }
}
