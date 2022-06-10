using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    public enum QSEnumSymbolStyleTypes
    {
        NumStyle,
        LetterShortStyle,
        LetterLongStyle,
    }

    public class FeedRegisterInfo
    {
        public SymbolInfo Info { get; set; }
        public string Exchange { get; set; }
        public string Symbol { get; set; }
    }
    public class SymbolInfo
    {
        public const string TYPE_SPOT = "SPOT";
        public const string TYPE_FUTURES = "FUTURES";
        public const string TYPE_PERPETUAL = "PERPETUAL";
            
        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange {get;set;}
        
        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 基础资产
        /// </summary>
        public string Base { get; set; }
        
        /// <summary>
        /// 报价资产
        /// </summary>
        public string Quote { get; set; }
        
        /// <summary>
        /// SPOT FUTURES OPTION PERPETUAL INDEX CREDIT CONTRACT
        /// </summary>
        public string  SymbolType { get; set; }
        
        public static SymbolInfo ParseSymbol(string symbol)
        {
            try
            {
                SymbolInfo info = new SymbolInfo();
                info.Symbol = symbol;

                var tmp = symbol.Split("_",2);
                info.SymbolType = tmp[0];
                if (info.SymbolType == "SPOT")
                {
                    var tmp2 = tmp[1].Split("_");
                    if (tmp2.Length == 2)
                    {
                        info.Base = tmp2[0];
                        info.Quote = tmp2[1];
                    }
                }
                return info;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }


}
