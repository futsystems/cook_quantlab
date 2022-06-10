using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 交易时间段
    /// 交易时间段指定一个特定的时区,同时包含一组交易小节列表
    /// </summary>
    public class MarketTimeImpl : MarketTime
    {
        /// <summary>
        /// 交易小节列表
        /// </summary>
        public SortedDictionary<string, TradingRange> RangeList { get { return _sortRangeList; } }
        SortedDictionary<string, TradingRange> _sortRangeList = new SortedDictionary<string, TradingRange>();

        /// <summary>
        /// 数据库全局编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 时间段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 时间段描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 收盘时间
        /// </summary>
        public int CloseTime { get; set; }

        /// <summary>
        /// 获得某时间 所属交易时间小节
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public TradingRange JudgeRange(DateTime time)
        {
            //DateTime target = GetTargetTime(time);
            //Util.Debug(string.Format("Local: {0} - Target: {1}", time, target));
            foreach (var range in this._sortRangeList.Values)
            {
                if (range.IsInRange(time))
                    return range;
            }
            return null;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public static string Serialize(MarketTimeImpl mt)
        {
            if (mt == null) return string.Empty;
            StringBuilder sb = new StringBuilder();
            char d = ',';

            sb.Append(mt.ID.ToString());
            sb.Append(d);
            sb.Append(mt.Name);
            sb.Append(d);
            sb.Append(mt.Description);
            sb.Append(d);
            sb.Append(mt.CloseTime);
            sb.Append(d);
            sb.Append(mt.SerializeTradingRange());
            return sb.ToString();
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="content"></param>
        public static MarketTimeImpl Deserialize(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            string[] rec = content.Split(new char[] { ',' }, 5);
            MarketTimeImpl mt = new MarketTimeImpl();
            mt.ID = int.Parse(rec[0]);
            mt.Name = rec[1];
            mt.Description = rec[2];
            mt.CloseTime = int.Parse(rec[3]);
            mt.DeserializeTradingRange(rec[4]);
            return mt;
        }


        
        /// <summary>
        /// 用于从数据库加载交易时间对象 将对应字段的值反序列化为交易小节对象
        /// </summary>
        /// <param name="content"></param>
        public void DeserializeTradingRange(string content)
        {
            _sortRangeList.Clear();
            string[] rec = content.Split('#');
            foreach (var s in rec)
            {
                TradingRangeImpl range = TradingRangeImpl.Deserialize(s);
                if (range == null)
                    continue;
                _sortRangeList.Add(range.RangeKey, range);
            }
        }

        /// <summary>
        /// 将交易小节 序列化成字符串
        /// </summary>
        /// <returns></returns>
        public string SerializeTradingRange()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var range in _sortRangeList.Values)
            {
                sb.Append('#');
                sb.Append(TradingRangeImpl.Serialize(range));
            }
            return sb.ToString();
        }


    }


}
