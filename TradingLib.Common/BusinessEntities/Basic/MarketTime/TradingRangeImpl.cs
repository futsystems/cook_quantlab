using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /* 
     *  交易时间段对象
     * 
     * 
     * 
     * 
     * 
     * 
     * **/

   

    /// <summary>
    /// 交易时间小节
    /// </summary>
    public class TradingRangeImpl:TradingRange
    {
        public TradingRangeImpl()
        {
            this.SettleFlag = QSEnumRangeSettleFlag.T;
            this.StartDay = DayOfWeek.Monday;
            this.StartTime = 0;
            this.EndDay = DayOfWeek.Tuesday;
            this.EndDay = 0;
            this.MarketClose = false;
        }

        public TradingRangeImpl(DayOfWeek startday, int starttime, DayOfWeek endday, int endtime, QSEnumRangeSettleFlag flag = QSEnumRangeSettleFlag.T, bool marketclose = false)
        {
            this.StartDay = startday;
            this.StartTime = starttime;
            this.EndDay = endday;
            this.EndTime = endtime;
            this.SettleFlag = flag;
            this.MarketClose = marketclose;
        }
        /// <summary>
        /// 结算标识
        /// </summary>
        public QSEnumRangeSettleFlag SettleFlag { get; set; }
        /// <summary>
        /// 开始日
        /// </summary>
        public DayOfWeek StartDay { get; set; }
        /// <summary>
        /// 结束日
        /// </summary>
        public DayOfWeek EndDay { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public int StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public int EndTime { get; set; }

        /// <summary>
        /// 收盘时间段标识
        /// 用于标注在该交易小节 收盘
        /// </summary>
        public bool MarketClose { get; set; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public static string Serialize(TradingRange range)
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            //sb.Append("#");
            sb.Append(range.StartDay);
            sb.Append(d);
            sb.Append(range.StartTime);
            sb.Append(d);
            sb.Append(range.EndDay);
            sb.Append(d);
            sb.Append(range.EndTime);
            sb.Append(d);
            sb.Append(range.SettleFlag);
            sb.Append(d);
            sb.Append(range.MarketClose);
            return sb.ToString();
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static TradingRangeImpl Deserialize(string message)
        {
            if (string.IsNullOrEmpty(message))
                return null;
            string[] rec = message.Split(',');
            if (rec.Length < 5) return null;

            TradingRangeImpl range = new TradingRangeImpl();
            range.StartDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), rec[0]);
            range.StartTime = int.Parse(rec[1]);
            range.EndDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), rec[2]);
            range.EndTime = int.Parse(rec[3]);
            range.SettleFlag = (QSEnumRangeSettleFlag)Enum.Parse(typeof(QSEnumRangeSettleFlag), rec[4]);
            range.MarketClose = bool.Parse(rec[5]);
            return range;
        }

        public override bool Equals(object obj)
        {
            if(obj is TradingRange)
            {
                TradingRange t = obj as TradingRange;
                return (this.StartDay == t.StartDay && this.StartTime == t.StartTime && this.EndDay == t.EndDay && this.EndTime == t.EndTime && this.SettleFlag == t.SettleFlag);
            }
            return false;
        }


        string _key = null;
        /// <summary>
        /// 交易小节 键值
        /// </summary>
        public string RangeKey
        {
            get
            {

                if (_key == null)
                {
                    _key = string.Format("{0}-{1:d6}-{2}-{3:d6}-{4}", (int)this.StartDay, this.StartTime, (int)this.EndDay, this.EndTime, this.SettleFlag);
                }
                return _key;
            }
        }

        public override string ToString()
        {
            return RangeKey;
        }
        public override int GetHashCode()
        {
            return RangeKey.GetHashCode();
        }


    }
}
