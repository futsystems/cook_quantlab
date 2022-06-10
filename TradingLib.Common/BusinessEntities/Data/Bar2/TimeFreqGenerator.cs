using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    /// <summary>
    /// 以时间为间隔的Bar数据生成器 通过CreateFrequencyGenerator获得具体的Bar生成器
    /// </summary>
    public class TimeFrequency : FrequencyPlugin
    {
        private NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        TimeSpan _barLength;
        public TimeSpan BarLength { get { return _barLength; } }

        BarFrequency _freq;
        public override BarFrequency BarFrequency { get { return _freq; } }


        /// <summary>
        /// Determines whether two FrequencyPlugin instances are equal
        /// </summary>
        /// <param name="obj">FrequencyPlugin used for comparison</param>
        /// <returns>true if they are equal, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            TimeFrequency timeFrequency = obj as TimeFrequency;
            return timeFrequency != null && this.BarLength == timeFrequency.BarLength;
        }

        /// <summary>
        /// Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            return this.BarLength.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Time[{0}s]", this._freq.Interval);
        }
        /// <summary>
        /// 全复制
        /// </summary>
        /// <returns></returns>
        public override FrequencyPlugin Clone()
        {
            return (TimeFrequency)base.MemberwiseClone();
        }


        //int _comparecode;
        //public int CompareCode { get { return _comparecode; } }

        /// <summary>
        /// 初始化一个TimeFrequency对象
        /// </summary>
        /// <param name="freq"></param>
        public TimeFrequency(BarFrequency freq)
        {
            if (freq.Type != BarInterval.CustomTime)
            {
                throw new ArgumentException("TimeFrequency need Time Based BarFrequency");
            }
            _barLength = new TimeSpan(0, 0, freq.Interval);

            _freq = freq;
            //_comparecode = freq.Interval * 10000 + (int)BarFrequency.Type;//通过这种方式获得为唯一的comparecode
        }

        /// <summary>
        /// 获得对应的Bar生成器
        /// </summary>
        /// <returns></returns>
        public override IFrequencyGenerator CreateFrequencyGenerator()
        {
            return new TimeFreqGenerator(this.BarLength);
        }

        /// <summary>
        /// 计算给定时刻的下一个周期结束时刻
        /// </summary>
        /// <param name="date"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static DateTime BarEndTime(DateTime date, TimeSpan period)
        {
            DateTime time = RoundTime(date, period);
            TimeSpan span = period;
            if (span.TotalDays > 1.0)
            {
                span = TimeSpan.FromDays(1.0);
            }
            while (RoundTime(time, period) <= date)
            {
                time = time.Add(span);
            }
            return RoundTime(time, period);
        }

        /// <summary>
        /// 获得季度结束
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        static int GetQuarterMonth(int month)
        {
            if (month == 1 || month == 2 || month == 3) return 4;
            if (month == 4 || month == 5 || month == 6) return 7;
            if (month == 7 || month == 8 || month == 9) return 10;
            return 1;
        }
        /// <summary>
        /// 计算给定时刻的当前周期开始时刻
        /// </summary>
        /// <param name="date"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static DateTime RoundTime(DateTime date, TimeSpan period)
        {
            DateTime time;
            if (period.TotalDays < 7.0)
            {
                time = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            }
            else if (period.TotalDays < 30.0)
            {
                time = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);// 小于月的周期 则为当前星期的星期一
                while (time.DayOfWeek != DayOfWeek.Monday)
                {
                    time = time.AddDays(-1.0);
                }
            }
            else if (period.TotalDays < 90)//大于1个月小于1季 按月
            {
                time = new DateTime(date.Year, date.Month, 1, 0, 0, 0);
                if (period.TotalDays == 30.0) //按30天/月为周期 则为该月第一天
                {
                    return time;
                }
            }
            else if (period.TotalDays < 365)//大于1个季度小于1年 按季
            {
                int month = GetQuarterMonth(date.Month);
                time = new DateTime(date.Year, month, 1, 0, 0, 0).AddMonths(-3);//减去3个月再减去1天 则为该季度开始时间
                if (period.TotalDays == 90.0) //按30天/月为周期 则为该月第一天
                {
                    return time;
                }
            }
            else
            {
                time = new DateTime(date.Year, 1, 1, 0, 0, 0);
                if (period.TotalDays == 365.0) //按365天/年为周期 则为该年的第一天 
                {
                    return time;
                }
            }

            long num = date.Ticks - time.Ticks;//计算时间差值 除周期取余,当前时间减去余数则为当前时间的Round
            long num2 = 0;
            if (period.Ticks != 0)
            {
                num2 = num % period.Ticks;
            }
            return new DateTime(date.Ticks - num2);
        }

        /// <summary>
        /// 计算某个时间所处周期
        /// 该周期为Bar的结束值
        /// 未使用 待修正
        /// </summary>
        /// <param name="date"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static DateTime RoundTime20(DateTime date, TimeSpan period)
        {
            DateTime time;
            if (period.TotalDays < 7.0)
            {
                time = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            }
            else if (period.TotalDays < 30.0)
            {
                time = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);// 小于月的周期 则为当前星期的周日
                while (time.DayOfWeek != DayOfWeek.Sunday)
                {
                    time = time.AddDays(1);
                }
            }
            else if (period.TotalDays < 365.0)//大于1个月小于1年 按月
            {
                time = new DateTime(date.Year, date.Month, 1, 0, 0, 0);
                if (period.TotalDays == 30.0) //按30天/月为周期 则为该月第一天
                {
                    return time.AddMonths(1);
                }
            }
            else
            {
                time = new DateTime(date.Year,1, 1, 0, 0, 0);
                if (period.TotalDays == 365.0) //按365天/年为周期 则为该年的第一天 
                {
                    return time.AddYears(1);
                }
            }

            long num = date.Ticks - time.Ticks;//计算时间差值 除周期取余,当前时间减去余数则为当前周期的开始 再加一个周期为该周期的结束
            long num2 = 0;
            if (period.Ticks != 0)
            {
                num2 = num % period.Ticks;
            }
            return new DateTime(date.Ticks - num2 + period.Ticks);
        }

        public override bool IsTimeBased { get { return true; } }


        /// <summary>
        /// 以时间为间隔的Bar数据生成器
        /// </summary>
        internal class TimeFreqGenerator:IFrequencyGenerator
        {
            private NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();

            bool _updated = false;
            TimeSpan _interval;
            /// <summary>
            /// Bar数据累加器
            /// </summary>
            BarGenerator _generator;

            int _units = 0;
            public TimeFreqGenerator(TimeSpan barLength)
            {
                this._interval = barLength;
                this._updated = false;
                this._units = (int)barLength.TotalSeconds;
            }

            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="symbol"></param>
            /// <param name="type"></param>
            public void Initialize(Symbol symbol, BarConstructionType type)
            {
                this._generator = new BarGenerator(symbol, new BarFrequency(BarInterval.CustomTime, this._units), type);
                this._generator.NewBar += new Action<SingleBarEventArgs>(_generator_NewBar);
                this._generator.NewTick += new Action<NewTickEventArgs>(_generator_NewTick);
            }

            /// <summary>
            /// Tick附带实时Bar事件
            /// </summary>
            public event Action<NewTickEventArgs> NewTickEvent;

            /// <summary>
            /// Bar事件
            /// </summary>
            public event Action<SingleBarEventArgs> NewBarEvent;

            void _generator_NewTick(NewTickEventArgs obj)
            {
                if (NewTickEvent != null)
                    NewTickEvent(obj);
            }

            void _generator_NewBar(SingleBarEventArgs obj)
            {
                if (NewBarEvent != null)
                    NewBarEvent(obj);
            }


            public void ProcessBar(Bar bar)
            { 
            
            }

            /// <summary>
            /// 处理实时行情
            /// </summary>
            /// <param name="k"></param>
            public void ProcessTick(Tick k)
            {
                this.UpdateTime(k.DateTime());
                this._generator.ProcessTick(k);

                //if (k.UpdateType == "E")
                //{
                //    if (k.MarketOpen == false)
                //    { 
                //        //取当前周期的下一个周期 并发送当前Bar
                //        DateTime nextend = TimeFrequency.BarEndTime(this._generator.BarEndTime, this._interval);
                //        this._generator.SendNewBar(nextend);
                //    }
                //}
            }

            public void UpdateTime(DateTime datetime)
            {
                DateTime endtime = TimeFrequency.BarEndTime(datetime, this._interval);
                //没有处理过tick数据 则更新当前的round时间为当前Bar的开始时间 closebar会执行update=false
                if (!this._updated)
                {
#if DEBUG
                    //logger.Info(string.Format("DateTime:{0} SetBarEndTime:{1}", datetime, endtime));
#endif
                    this._generator.SetBarEndTime(endtime);
                    this._updated = true;
                }
                //如果roundtime大于PartialBar的起始时间 越过了一个Bar数据 调用generator发送Bar同时设定BarStartTime
                if (endtime > this._generator.PartialBar.EndTime)
                {
                    //取下一个Bar时间 根据当前BarStartTime计算下一个BarStarTime
                    DateTime nextend = TimeFrequency.BarEndTime(this._generator.BarEndTime, this._interval);
                    if (endtime < nextend)
                    {
                        throw new Exception("Error in time rounding logic");
                    }
                    //发送当前Generator中的Bar数据 同时设定下一个Bar的开始时间
                    this._generator.SendNewBar(nextend);//结束时间按Bar的开始时间以及间隔计算获得
                    this._generator.SetBarEndTime(endtime);//Bar的结束时间按当前实际时间Round获得
                }
            }

            /*
             * 
             *  public DateTime NextTimeUpdateNeeded
            {
                get
                {
                    if (!this._updated)
                    {
                        return DateTime.MinValue;
                    }
                    return TimeFrequency.NextRoundedTime(this._generator.BarStartTime, this._interval); 10:01:00 开始10:02分结束 10：02进入下一个Bar
                }
            }
            **/

            /// <summary>
            /// 下次Bar关闭时间
            /// </summary>
            public DateTime NextTimeUpdateNeeded
            {
                get
                {
                    if (!this._updated)
                    {
                        return DateTime.MinValue;
                    }
                    return this._generator.BarEndTime;// Frequency.BarEndTime(this._generator.BarEndTime, this._interval); 10:01的Bar Tick时间10：00：21 会与10：02进行比较
                }
            }

        }
    }
}
