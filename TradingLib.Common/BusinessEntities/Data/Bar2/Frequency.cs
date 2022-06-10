using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    /// <summary>
    /// 维护了Bar数据
    /// </summary>
    public class Frequency
    {
        /// <summary>
        /// 只读Bar数据
        /// </summary>
        public QList<Bar> Bars { get; set; }

        /// <summary>
        /// 可写Bar数据 
        /// 这里的读写针对的是对应数据结构位置上的对象 获得对象引用后依然可以修改该对象
        /// </summary>
        public QList<Bar> WriteableBars { get; set; }

        public DateTime CurrentBarEndTime { get; set; }

        public Symbol Symbol { get; set; }

        public Dictionary<Frequency, QList<DateTime>> DestFrequencyConversion { get; set; }

        internal bool SynchronizeBars { get; set; }

        FrequencyManager.FreqKey _key = null;

        public FrequencyPlugin FrequencySettings { get; set; }

        public Frequency(FrequencyManager.FreqKey key, bool synchronizeBars)
        {
            this._key = key;
            this.Symbol = key.Symbol;
            this.FrequencySettings = key.Settings;
            this.SynchronizeBars = synchronizeBars;
            this.WriteableBars = new QList<Bar>();
            this.Bars = this.WriteableBars.AsReadOnly();
            this.DestFrequencyConversion = new Dictionary<Frequency, QList<DateTime>>();
        }

        

        public ISeries Open { get { return this.GetBarElementSeries(QSEnumBarElement.Open); } }
        public ISeries High { get { return this.GetBarElementSeries(QSEnumBarElement.High); } }
        public ISeries Low { get { return this.GetBarElementSeries(QSEnumBarElement.Low); } }
        public ISeries Close { get { return this.GetBarElementSeries(QSEnumBarElement.Close); } }
        public ISeries Volume { get { return this.GetBarElementSeries(QSEnumBarElement.Volume); } }
        public ISeries Ask { get { return this.GetBarElementSeries(QSEnumBarElement.Ask); } }
        public ISeries Bid { get { return this.GetBarElementSeries(QSEnumBarElement.Bid); } }

        /// <summary>
        /// Returns the index bar with the specified end date, or the bar that
        /// contains the specified date
        /// </summary>
        /// <param name="barEndDate">Specified end date.</param>
        /// <returns>Index of located bar, otherwise -1 if the bar for that end date was not found.</returns>
        public int LookupEndDate(System.DateTime barEndDate)
        {
            int num = this.x954e89ce87b3f10e(barEndDate);
            if (num < 0)
            {
                return num;
            }
            if (this.Bars.LookBack(num).EndTime == barEndDate)
            {
                num++;
                if (num >= this.Bars.Count)
                {
                    return -1;
                }
            }
            return this.x31273f27c463a242(num);
        }

        internal int LookupStartDate(System.DateTime barStartTime)
        {
            int num = this.x954e89ce87b3f10e(barStartTime);
            if (num < 0)
            {
                return num;
            }
            while (num > 0 && this.Bars.LookBack(num - 1).EndTime == barStartTime)
            {
                num--;
            }
            return num;
        }

        private int x31273f27c463a242(int xe151e765248d06d8)
        {
            System.DateTime barStartTime = this.Bars.LookBack(xe151e765248d06d8).EndTime;
            while (xe151e765248d06d8 > 0 && this.Bars.LookBack(xe151e765248d06d8 - 1).EndTime == barStartTime)
            {
                xe151e765248d06d8--;
            }
            return xe151e765248d06d8;
        }

        private int x954e89ce87b3f10e(DateTime xb21f13a9707ad954)
        {
            if (this.Bars.Count == 0)
            {
                return -1;
            }
            int num = 0;
            int num2 = System.Math.Min(this.Bars.Count - 1, 4);
            while (this.Bars.LookBack(num2).EndTime > xb21f13a9707ad954 && num2 > 0)
            {
                num = num2;
                num2 *= 2;
                if (num2 > this.Bars.Count - 1)
                {
                    num2 = this.Bars.Count - 1;
                    break;
                }
            }
            while (num2 - 1 > num)
            {
                int num3 = (num2 + num) / 2;
                System.DateTime barStartTime = this.Bars.LookBack(num3).EndTime;
                if (barStartTime == xb21f13a9707ad954)
                {
                    return num3;
                }
                if (barStartTime < xb21f13a9707ad954)
                {
                    num2 = num3;
                }
                else
                {
                    num = num3;
                }
            }
            if (this.Bars.LookBack(num).EndTime <= xb21f13a9707ad954)
            {
                return num;
            }
            if (this.Bars.LookBack(num2).EndTime <= xb21f13a9707ad954)
            {
                return num2;
            }
            return -1;
        }


        private ISeries GetBarElementSeries(QSEnumBarElement element)
        {
            return new FrequencyBarElementSeries(this, element);
        }


        public event Action<SingleBarEventArgs> SingleBarEvent;
        internal void OnNewBar(SingleBarEventArgs args)
        {
            if (SingleBarEvent != null)
                SingleBarEvent(args);
        }

        public event Action<NewTickEventArgs> NewTickEvent;
        internal void SendTick(Tick tick)
        {
            if (NewTickEvent != null)
            {
                NewTickEventArgs newTickEventArgs = new NewTickEventArgs(this.Symbol,tick,this.WriteableBars.PartialItem);
                if (newTickEventArgs.PartialBar == null)
                {
                    return;
                }
                NewTickEvent(newTickEventArgs);
            }
        }

    }
}
