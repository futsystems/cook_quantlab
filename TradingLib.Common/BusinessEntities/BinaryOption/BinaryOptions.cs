using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// CallPut 二元期权
    /// </summary>
    public class BinaryOptionCallPut : BinaryOptionImpl
    {
        public BinaryOptionCallPut(string symbol, EnumBinaryOptionTimeSpan type,decimal rate)
        {
            this.Symbol = symbol;
            this.OptionType = EnumBinaryOptionType.CallPut;
            this.TimeSpanType = type;

            long now = Util.ToTLDateTime();
            this.BornTime = BinaryOptionImpl.CalcBornTime(now, type);
            //计算合约到期日
            this.ExpireTime = BinaryOptionImpl.CalcExpireTime(now, type);

            this.Rate = rate;
            this.ContractID = "{0}-{1}-{2}-{3}".Put(this.Symbol, this.OptionType, this.TimeSpanType,this.ExpireTime);
        }
    }

    /// <summary>
    /// Above/Below 二元期权
    /// </summary>
    public class BinaryOptionAboveBelow : BinaryOptionImpl
    {
        public BinaryOptionAboveBelow(string symbol, EnumBinaryOptionTimeSpan type, decimal uptarget, decimal downtarget, decimal rate)
        {
            this.Symbol = symbol;
            this.OptionType = EnumBinaryOptionType.AboveDown;
            this.TimeSpanType = type;
            long now = Util.ToTLDateTime();
            this.BornTime = BinaryOptionImpl.CalcBornTime(now, type);
            //计算合约到期日
            this.ExpireTime = BinaryOptionImpl.CalcExpireTime(now, type);

            this.Rate = rate;
            this.ContractID = "{0}-{1}-{2}-{3}".Put(this.Symbol, this.OptionType, this.TimeSpanType, this.ExpireTime);
        }
    }


}
