using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 定义了一个合约和频率对
    /// </summary>
    public class SymbolFreq
    {
        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 频率参数
        /// </summary>
        public BarFrequency BarFrequency { get; set; }

        
        public SymbolFreq(string symbol, BarFrequency freq)
        {
            this.Symbol = symbol;
            this.BarFrequency = freq;
        }

        public override bool Equals(object obj)
        {
            if (obj is SymbolFreq)
            {
                SymbolFreq sf = obj as SymbolFreq;
                return sf.Symbol == this.Symbol && sf.BarFrequency.Equals(this.BarFrequency);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Symbol.GetHashCode() ^ this.BarFrequency.GetHashCode();
        }


        /// <summary>
        /// 用于生成唯一文件名 保存数据
        /// </summary>
        /// <returns></returns>
        public string ToUniqueId()
        {
            return this.Symbol + "-" + this.BarFrequency.ToUniqueId();
        }


    }
}
