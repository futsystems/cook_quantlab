using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    public class Profiler
    {
        // Fields
        private Dictionary<string, SectionStats> __sectionStatslist = new Dictionary<string, SectionStats>();
        private List<SectionStackEntry> _sectionStackEntrylist = new List<SectionStackEntry>();
        public static readonly Profiler Instance = new Profiler();
        private DateTime xb713e88dd5b915a0;

        // Methods
        public void EnterSection(string name)
        {
            SectionStats stats2;
            DateTime now = DateTime.Now;
            if (this._sectionStackEntrylist.Count > 0)
            {
                SectionStackEntry entry = this._sectionStackEntrylist[this._sectionStackEntrylist.Count - 1];
                SectionStats stats = this.__sectionStatslist[entry.Name];
                TimeSpan span = now.Subtract(this.xb713e88dd5b915a0);
                stats.LocalTime += span;
            }
            //当name为新增 则创建 并加入到__sectionStatslist
            if (!this.__sectionStatslist.TryGetValue(name, out stats2))
            {
                stats2 = new SectionStats(name);
                this.__sectionStatslist[name] = stats2;
            }
            //调用次数累加
            stats2.TimesCalled++;
            SectionStackEntry item = new SectionStackEntry(name)
            {
                TimeStarted = now
            };
            this._sectionStackEntrylist.Add(item);
            this.xb713e88dd5b915a0 = now;
        }

        public string GetStatsString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Name\t\tTimes Called\tLocal Time\tTotal Time");
            foreach (SectionStats stats in this.__sectionStatslist.Values)
            {
                builder.AppendFormat("{0}\t\t{1}\t{2:f5}\t\t{3:f5}\r\n", new object[] { stats.Name, stats.TimesCalled, stats.LocalTime.TotalSeconds, stats.TotalTime.TotalSeconds });
            }
            return builder.ToString();
        }

        public void LeaveSection()
        {
            DateTime now = DateTime.Now;
            SectionStackEntry entry = this._sectionStackEntrylist[this._sectionStackEntrylist.Count - 1];
            SectionStats stats = this.__sectionStatslist[entry.Name];
            TimeSpan span = (TimeSpan)(now - this.xb713e88dd5b915a0);
            stats.LocalTime += span;//
            span = (TimeSpan)(now - entry.TimeStarted);//记录进入函数的时间,当前时间-进入函数时间
            stats.TotalTime += span;

            this._sectionStackEntrylist.RemoveAt(this._sectionStackEntrylist.Count - 1);
            this.xb713e88dd5b915a0 = now;
        }

        public void Reset()
        {
            if (this._sectionStackEntrylist.Count > 0)
            {
                //throw new QSQuantError("Cannot reset profiler while there are items on its stack");
            }
        }
    }


}
