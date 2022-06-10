using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public enum EnumMarketSessionType
    { 
        /// <summary>
        /// 集合竞价时间段
        /// </summary>
        CallAuction,
        /// <summary>
        /// 连续竞价时间段
        /// </summary>
        Continuous,
    }

    /// <summary>
    /// 交易所时间段
    /// </summary>
    public class MarketSession
    {

        public MarketSession(DateTime start, DateTime end, EnumMarketSessionType sessionType = EnumMarketSessionType.Continuous)
        {
            this.Start = start;
            this.End = end;
            this.SessionType = sessionType;
            this._timeKey = this.Start.ToTLDateTime();
        }

        long _timeKey = 0;
        public long TimeKey
        {
            get
            {
                return _timeKey;
            }
        }
        /// <summary>
        /// 交易时间段类别
        /// </summary>
        public EnumMarketSessionType SessionType { get; private set; }

        /// <summary>
        /// 交易时间段开始
        /// </summary>
        public DateTime Start { get; private set; }

        /// <summary>
        /// 交易时间段结束
        /// </summary>
        public DateTime End { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}-{1}", this.Start.ToString("MM/dd HH:mm"), this.End.ToString("MM/dd HH:mm"));
        }

        /// <summary>
        /// 生成SessionString用于查询合约时附带 分时图绘制
        /// </summary>
        /// <returns></returns>
        public string ToSessionString()
        {
            return string.Format("{0}-{1}", this.Start.ToTLDateTime(), this.End.ToTLDateTime());
        }


    }


    /// <summary>
    /// 交易日对象
    /// 用于定义一个交易日 并界定对应的交易小节
    /// 
    /// 系统运行时 对应交易所开盘时间点执行定时任务 判定当前是否交易如果不交易则不执行初始化操作，如果是交易则执行初始化操作
    /// 并加载当前交易信息将TradingDay执行初始化
    /// 
    /// 由于交易所收盘时刻以及节假日，周末等情况，会导致交易所交易日与常规的自然日有很大不同
    /// 比如原油 星期日下午18点开始进入交易 到星期一下午17点结束，交易记录的结算日都记到星期一
    /// 比如上期所 星期五夜盘-星期六凌晨 的交易记录的结算日都记到星期一
    /// </summary>
    public class MarketDay
    {

        public MarketDay(int tradingday,IEnumerable<MarketSession> sessionList)
        {
            this.TradingDay = tradingday;
            foreach (var session in sessionList)
            {
                marketSessionMap.Add(session.TimeKey, session);
            }
        }

        /// <summary>
        /// 交易日
        /// </summary>
        public int TradingDay { get; set; }

        /// <summary>
        /// 所有交易时间段
        /// </summary>
        public IEnumerable<MarketSession> MarketSessions { get { return marketSessionMap.Values; } }


        SortedDictionary<long, MarketSession> marketSessionMap = new SortedDictionary<long, MarketSession>();

        /// <summary>
        /// 开盘时间
        /// </summary>
        public DateTime MarketOpen
        {
            get
            {
                if (marketSessionMap.Count == 0)
                    throw new Exception("MarketDay have no session avabile");
                return marketSessionMap.First().Value.Start;
            }
        }

        /// <summary>
        /// 判定某个时间是否在交易日内
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public bool IsInMarketDay(DateTime datetime)
        {
            if (datetime > this.MarketClose) return false;
            if (datetime < this.MarketOpen) return false;
            return true;
        }
        /// <summary>
        /// 收盘时间
        /// </summary>
        public DateTime MarketClose
        {
            get
            {
                if (marketSessionMap.Count == 0)
                    throw new Exception("MarketDay have no session avabile");
                return marketSessionMap.Last().Value.End;
            }
        }

        /// <summary>
        /// 注rangelist是date所对应的weekday的TradingRangeList
        /// </summary>
        /// <param name="date"></param>
        /// <param name="rangelist"></param>
        /// <returns></returns>
        public static MarketDay CreateMarketDay(DateTime date, List<TradingRange> rangelist)
        {
            DateTime sstart, send;
            List<MarketSession> sessionList = new List<MarketSession>();
            foreach (var range in rangelist)
            {
                DateTime seek = date;
                while (seek.DayOfWeek != range.StartDay)
                {
                    seek = seek.AddDays(-1);
                }
                sstart = Util.ToDateTime(seek.ToTLDate(), range.StartTime);
                seek = date;
                while (seek.DayOfWeek != range.EndDay)
                {
                    seek = seek.AddDays(-1);
                }
                send = Util.ToDateTime(seek.ToTLDate(), range.EndTime);

                MarketSession ms = new MarketSession(sstart, send);
                sessionList.Add(ms);
            }
            return new MarketDay(date.ToTLDate(), sessionList);

        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", this.TradingDay, string.Join(",", this.MarketSessions.Select(s => s.ToString()).ToArray()));
        }

        public string ToSessionString()
        {
            return string.Format("{0}:{1}", this.TradingDay, string.Join(" ", this.MarketSessions.Select(s => s.ToSessionString()).ToArray()));
        }
    }
}
