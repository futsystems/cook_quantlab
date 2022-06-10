using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.API
{

    /// <summary>
    /// 二元期权参数接口
    /// 定义了二元期权参数 根据期权类别的不同,有不同的参数
    /// </summary>
    public interface BinaryOption
    {
        /// <summary>
        /// 合约ID
        /// </summary>
        string ContractID { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        string Symbol { get; set; }

        /// <summary>
        /// 二元期权类别
        /// </summary>
        EnumBinaryOptionType OptionType { get; set; }

        /// <summary>
        /// 时间间隔
        /// </summary>
        EnumBinaryOptionTimeSpan TimeSpanType { get; set; }

        /// <summary>
        /// 合约开始时间
        /// </summary>
        long BornTime { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        long ExpireTime { get; set; }

        /// <summary>
        /// 回报率
        /// </summary>
        decimal Rate { get; set; }

        ////Upper/Lower Target在区间期权为上下边界 在Above/Below为上下StrikePrice
        /// <summary>
        /// 区间上边界
        /// </summary>
        decimal UpperTarget { get; set; }

        /// <summary>
        /// 区间下边界
        /// </summary>
        decimal LowerTarget { get; set; }       



    }
}
