using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 二元期权
    /// 1.涨跌期权,到期时价格超过执行价或低于执行价
    /// 2.区间期权,到期时价格在区间之内或在区间之外
    /// 3.一触即付,在到期之前价格价格上触或下触
    /// 4.
    /// </summary>
    public class BinaryOptionImpl:BinaryOption
    {
        public BinaryOptionImpl()
        {
            this.Symbol = string.Empty;
            this.OptionType = EnumBinaryOptionType.CallPut;
            this.TimeSpanType = EnumBinaryOptionTimeSpan.MIN1;
            this.BornTime = 0;
            this.ExpireTime = 0;

            this.Rate = 0.7M;
            this.UpperTarget = 0M;
            this.LowerTarget = 0M;
        }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 二元期权类别
        /// </summary>
        public EnumBinaryOptionType OptionType { get; set; }

        /// <summary>
        /// 时间间隔
        /// </summary>
        public EnumBinaryOptionTimeSpan TimeSpanType { get; set; }

        /// <summary>
        /// 合约开始时间
        /// </summary>
        public long BornTime { get; set; }

        /// <summary>
        /// 合约到期时间
        /// </summary>
        public long ExpireTime { get; set; }

        /// <summary>
        /// 回报率
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// 区间上边界
        /// </summary>
        public decimal UpperTarget { get; set; }

        /// <summary>
        /// 区间下边界
        /// </summary>
        public decimal LowerTarget { get; set; }

        /// <summary>
        /// 合约编号
        /// EUR/US-CallPut-Expired
        /// Symbol-OptionType-Exipred
        /// 合约-二元期权类别-到期日 键值可以唯一
        /// 
        /// </summary>
        public string ContractID { get; set; }
        
        public static TimeSpan TimeSpanTypeToTimeSpan(EnumBinaryOptionTimeSpan tstype)
        {
            switch (tstype)
            {
                case EnumBinaryOptionTimeSpan.MIN1: return TimeSpan.FromMinutes(1);
                case EnumBinaryOptionTimeSpan.MIN2: return TimeSpan.FromMinutes(2);
                case EnumBinaryOptionTimeSpan.MIN5: return TimeSpan.FromMinutes(5);
                case EnumBinaryOptionTimeSpan.MIN10: return TimeSpan.FromMinutes(10);
                case EnumBinaryOptionTimeSpan.MIN15: return TimeSpan.FromMinutes(15);
                case EnumBinaryOptionTimeSpan.MIN30: return TimeSpan.FromMinutes(30);
                case EnumBinaryOptionTimeSpan.MIN60: return TimeSpan.FromMinutes(60);
                default:
                    return TimeSpan.FromMinutes(0);
            }
        }
        /// <summary>
        /// 按某个时间计算二元期权到期时间
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static long CalcExpireTime(long entrytime, EnumBinaryOptionTimeSpan type)
        {
            TimeSpan ts = BinaryOptionImpl.TimeSpanTypeToTimeSpan(type);
            DateTime dt = TimeFrequency.BarEndTime(Util.ToDateTime(entrytime), ts);
            return dt.ToTLDateTime();
        }

        /// <summary>
        /// 按某个时间计算二元期权开始时间
        /// </summary>
        /// <param name="entrytime"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static long CalcBornTime(long entrytime, EnumBinaryOptionTimeSpan type)
        {
            TimeSpan ts = BinaryOptionImpl.TimeSpanTypeToTimeSpan(type);
            DateTime dt = TimeFrequency.RoundTime(Util.ToDateTime(entrytime), ts);
            return dt.ToTLDateTime();
        }
        /// <summary>
        /// 输出二元期权合约信息
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            switch(this.OptionType)
            {
                case EnumBinaryOptionType.CallPut:
                    //CallPut CN1603@MIN2 Exp:20160319120300
                    return "BO:{0} {1}@{2} Exp:{3}".Put(this.OptionType, this.Symbol, this.TimeSpanType, this.ExpireTime);
                case EnumBinaryOptionType.Range:
                    //Range[2673.5-2123.4] CN1603@MIN5 Exp:20160319120300
                    return "BO:{0}[{1}-{2}] {3}@{4} Exp:{4}".Put(this.OptionType, this.UpperTarget, this.LowerTarget, this.Symbol, this.TimeSpanType, this.ExpireTime);
                default:
                    return "Not Supported";
            }
        }

        public static string Serialize(BinaryOption bo)
        {
            StringBuilder sb = new StringBuilder();
            char d = '-';
            sb.Append(bo.Symbol);
            sb.Append(d);
            sb.Append(bo.OptionType);
            sb.Append(d);
            sb.Append(bo.TimeSpanType);
            sb.Append(d);
            sb.Append(bo.ExpireTime);
            sb.Append(d);
            sb.Append(bo.Rate);
            sb.Append(d);
            sb.Append(bo.UpperTarget);
            sb.Append(d);
            sb.Append(bo.LowerTarget);

            return sb.ToString();

        }

        public static BinaryOption Deserialize(string message)
        {
            BinaryOption bo = new BinaryOptionImpl();
            string[] rec = message.Split('-');
            bo.Symbol = rec[0];
            bo.OptionType = rec[1].ParseEnum<EnumBinaryOptionType>();
            bo.TimeSpanType = rec[2].ParseEnum<EnumBinaryOptionTimeSpan>();
            bo.ExpireTime = long.Parse(rec[3]);
            bo.Rate = long.Parse(rec[4]);
            bo.UpperTarget = decimal.Parse(rec[5]);
            bo.LowerTarget = decimal.Parse(rec[6]);

            return bo;
        }
    }
}
