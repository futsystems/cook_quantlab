using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 用于提供合约信息，供客户端查询合约使用
    /// 内部使用的Symbol包含了大量依赖和逻辑不便于在客户端去重现
    /// Instrument将一些字段和数据直接置于Instrument
    /// </summary>
    public class Instrument
    {

        public Instrument()
        {
            Symbol = string.Empty;
            Name = string.Empty;
            Security = string.Empty;
            ExchangeID = string.Empty;
            EntryCommission = 0;
            ExitCommission = 0;
            Margin = 0;
            SecurityType = API.SecurityType.NIL;
            Multiple = 0;
            PriceTick = 0;
            ExpireMonth = 0;
            ExpireDate = 0;
            Tradeable = false;
            Currency = CurrencyType.RMB;

        }
        /// <summary>
        /// 合约编号
        /// </summary>
        public string Symbol { get; set; }


        /// <summary>
        /// 合约中文名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 品种编号
        /// </summary>
        public string Security { get; set; }

        /// <summary>
        /// 交易所编号
        /// </summary>
        public string ExchangeID { get; set; }

        /// <summary>
        /// 开仓手续费
        /// </summary>
        public decimal EntryCommission { get; set; }

        /// <summary>
        /// 平仓手续费
        /// </summary>
        public decimal ExitCommission { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public decimal Margin { get; set; }

        /// <summary>
        /// 品种类别
        /// </summary>
        public SecurityType SecurityType { get; set; }

        /// <summary>
        /// 乘数
        /// </summary>
        public int Multiple { get; set; }


        /// <summary>
        /// 最小价格变动
        /// </summary>
        public decimal PriceTick { get; set; }

        /// <summary>
        /// 到期月份
        /// </summary>
        public int ExpireMonth { get; set; }

        /// <summary>
        /// 到期日
        /// </summary>
        public int ExpireDate { get; set; }

        /// <summary>
        /// 是否可以交易
        /// </summary>
        public bool Tradeable { get; set; }

        /// <summary>
        /// 货币
        /// </summary>
        public CurrencyType Currency { get; set; }
        string _serializedstring = string.Empty;
        public string GetSerializedString()
        {
            if (string.IsNullOrEmpty(_serializedstring))
            {
                _serializedstring = Serialize(this);
            }
            return _serializedstring;
        }

        public static string Serialize(Instrument instrument)
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(instrument.Symbol);//0
            sb.Append(d);
            sb.Append(instrument.Name);//1
            sb.Append(d);
            sb.Append(instrument.Security);//2
            sb.Append(d);
            sb.Append(instrument.ExchangeID);//3
            sb.Append(d);
            sb.Append(instrument.EntryCommission);//4
            sb.Append(d);
            sb.Append(instrument.ExitCommission);//5
            sb.Append(d);
            sb.Append(instrument.Margin);//6
            sb.Append(d);
            sb.Append(instrument.SecurityType.ToString());//7
            sb.Append(d);
            sb.Append(instrument.Multiple);//8
            sb.Append(d);
            sb.Append(instrument.PriceTick);//9
            sb.Append(d);
            sb.Append(instrument.ExpireMonth);//10
            sb.Append(d);
            sb.Append(instrument.ExpireDate);
            sb.Append(d);
            sb.Append(instrument.Tradeable);
            sb.Append(d);
            sb.Append(instrument.Currency);
            return sb.ToString();
        }


        public static Instrument Deserialize(string str)
        {
            string[] rec = str.Split(',');
            Instrument instrument = new Instrument();
            instrument.Symbol = rec[0];
            instrument.Name = rec[1];
            instrument.Security = rec[2];
            instrument.ExchangeID = rec[3];
            instrument.EntryCommission = decimal.Parse(rec[4]);
            instrument.ExitCommission = decimal.Parse(rec[5]);
            instrument.Margin = decimal.Parse(rec[6]);
            instrument.SecurityType = (SecurityType)Enum.Parse(typeof(SecurityType), rec[7]);
            instrument.Multiple = int.Parse(rec[8]);
            instrument.PriceTick = decimal.Parse(rec[9]);
            instrument.ExpireMonth = int.Parse(rec[10]);
            instrument.ExpireDate = int.Parse(rec[11]);
            instrument.Tradeable = bool.Parse(rec[12]);
            instrument.Currency = rec[13].ParseEnum<CurrencyType>();
            return instrument;
        }


    }
}
