using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.Common
{
    public class BarFrequency
    {

        /// <summary>
        /// 间隔数
        /// </summary>
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }

        }
        int _interval;

        /// <summary>
        /// 类别
        /// </summary>
        public BarInterval Type
        {
            get
            {
                return _type;
            }
            set { _type = value; }
        }
        BarInterval _type;

        TimeSpan timespan;
        public TimeSpan TimeSpan
        {
            get
            {
                if (timespan != null)
                    return timespan;
                else
                    timespan = new TimeSpan(0, 0, _interval);
                return timespan;
            }
        }

        //public BarFrequency(BarInterval interval)
        //{
        //    switch (interval)
        //    {
        //        case BarInterval.Day:
        //        case BarInterval.FifteenMin:
        //        case BarInterval.FiveMin:
        //        case BarInterval.Hour:
        //        case BarInterval.Minute:
        //        case BarInterval.ThreeMin:
        //        case BarInterval.ThirtyMin:
        //            {
        //                _type = BarInterval.CustomTime;
        //                _interval = (int)interval;
        //            }
        //            break;
        //        default:
        //            _type = BarInterval.CustomTime;
        //            _interval = 60;
        //            break;
        //    }
        //    timespan = new TimeSpan(0, 0, _interval);
        //    return;
        //}

        public BarFrequency(BarInterval type, int interval)
        {
            _interval = interval;
            switch (type)
            {
                case BarInterval.CustomTime:
                    timespan = new TimeSpan(0, 0, _interval);
                    _type = type;
                    break;
                case BarInterval.Day:
                    timespan = TimeSpan.FromDays(_interval);
                    _type = type;
                    break;
                case BarInterval.Minute:
                    timespan = TimeSpan.FromSeconds(60 * _interval);//1
                    _type = BarInterval.CustomTime;
                    _interval = 60 * _interval;
                    break;
                case BarInterval.ThreeMin:
                    timespan = TimeSpan.FromSeconds(180 * _interval);//3
                    _type = BarInterval.CustomTime;
                    _interval = 180;
                    break;
                case BarInterval.FiveMin:
                    timespan = TimeSpan.FromSeconds(300 * _interval);//5
                    _type = BarInterval.CustomTime;
                    _interval = 300 * _interval;
                    break;
                case BarInterval.FifteenMin:
                    timespan = TimeSpan.FromSeconds(900 * _interval);//15
                    _type = BarInterval.CustomTime;
                    _interval = 900 * _interval;
                    break;
                case BarInterval.ThirtyMin:
                    timespan = TimeSpan.FromSeconds(1800 * _interval);//30
                    _type = BarInterval.CustomTime;
                    _interval = 1800 * _interval;
                    break;
                case BarInterval.Hour:
                    timespan = TimeSpan.FromSeconds(3600 * _interval);//60
                    _type = BarInterval.CustomTime;
                    _interval = 3600 * _interval;
                    break;
                case BarInterval.CustomTicks:
                case BarInterval.CustomVol:
                    _type = type;
                    break;
                default:
                    _type = BarInterval.CustomTime;
                    break;
            }
            //timespan = new TimeSpan(0, 0, _interval);
            return;
        }

        public static bool operator ==(BarFrequency a, BarFrequency b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(BarFrequency a, BarFrequency b)
        {
            return !a.Equals(b);
        } 

        public override bool Equals(object obj)
        {
            if (obj is BarFrequency)
            {
                BarFrequency freq = obj as BarFrequency;
                return freq.Interval == this.Interval && freq.Type == this.Type;
            }
            return false;
        }

        static BarFrequency _minute = new BarFrequency(BarInterval.CustomTime, 60);
        public static BarFrequency Minute { get { return _minute; } }

        static BarFrequency _day = new BarFrequency(BarInterval.Day, 1);
        public static BarFrequency Day { get { return _day; } }

        public override string ToString()
        {
            return "Freq Type:" + this.Type.ToString() + " Interval:" + this.Interval.ToString();
        }

        public string ToUniqueId()
        {
            return string.Format("{0}-{1}",this.Type,this.Interval);
        }

        public override int GetHashCode()
        {
            return this.ToUniqueId().GetHashCode();
        }
    }
}
