//using System;
//using System.Collections.Generic;
//using System.Text;
//using TradingLib.API;
//namespace TradingLib.Common
//{
//    public class TimeIntervalData : IntervalData
//    {
//        public event SymBarIntervalDelegate NewBar;
//        public List<decimal> open() { return opens; }
//        public List<decimal> close() { return closes; }
//        public List<decimal> high() { return highs; }
//        public List<decimal> low() { return lows; }
//        public List<long> vol() { return vols; }
//        public List<int> date() { return dates; }
//        public List<int> time() { return times; }
//        public List<int> tick() { return ticks; }
//        public List<double> oaDateTime() { return OADateTime; }
//        public bool isRecentNew() { return _isRecentNew; }
//        public int Count() { return _Count; }
//        public int Last() { return _Count - 1; }
//        public TimeIntervalData(int unitsPerInterval)
//        {
//            intervallength = unitsPerInterval;
//        }
//        public void Reset()
//        {
//            opens.Clear();
//            closes.Clear();
//            highs.Clear();
//            lows.Clear();
//            dates.Clear();
//            times.Clear();
//            vols.Clear();
//            OADateTime.Clear();
//            _Count = 0;
//        }
//        //id对应的是唯一的bar序号
//        void newbar(long id)
//        {
//            _Count++;
//            opens.Add(0);
//            closes.Add(0);
//            highs.Add(0);
//            lows.Add(decimal.MaxValue);
//            vols.Add(0);
//            times.Add(0);
//            dates.Add(0);
//            OADateTime.Add(0);
//            ids.Add(id);
//        }
//        //增加一个Bar
//        public void addbar(Bar mybar)
//        {
//            _Count++;
//            closes.Add((decimal)mybar.Close);
//            opens.Add((decimal)mybar.Open);
//            dates.Add(mybar.BarDate);
//            highs.Add((decimal)mybar.High);
//            lows.Add((decimal)mybar.Low);
//            vols.Add(mybar.Volume);
//            times.Add(mybar.BarStartTime.ToTLTime());
//            OADateTime.Add(Util.ToDateTime(mybar.BarDate, mybar.BarStartTime.ToTLTime()).ToOADate());
//            ids.Add(getbarid(mybar.BarStartTime.ToTLTime(), mybar.BarDate, intervallength));
//        }
//        long curr_barid = -1;
//        int intervallength = 60;
//        internal List<decimal> opens = new List<decimal>();
//        internal List<decimal> closes = new List<decimal>();
//        internal List<decimal> highs = new List<decimal>();
//        internal List<decimal> lows = new List<decimal>();
//        internal List<long> vols = new List<long>();
//        internal List<int> dates = new List<int>();
//        internal List<int> times = new List<int>();
//        internal List<int> ticks = new List<int>();
//        internal List<long> ids = new List<long>();
//        internal List<double> OADateTime = new List<double>();
//        internal int _Count = 0;
//        internal bool _isRecentNew = false;
//        public Bar GetBar(int index, string symbol)
//        {
//            Bar b = new BarImpl();
//            if (index >= _Count) return b;
//            else if (index < 0)
//            {
//                index = _Count - 1 + index;
//                if (index < 0) return b;
//            }
//            //b = new BarImpl(opens[index], highs[index], lows[index], closes[index], vols[index], dates[index], times[index], symbol,intervallength);
//            //if (index == Last()) b.isNew = _isRecentNew;
//            return null;
//        }
//        public Bar GetBar(string symbol) { return GetBar(Last(), symbol); }
//        public void newTick(Tick k)
//        {
//            // ignore quotes
//            if (k.Trade == 0) return;
//            // get the barcount
//            long barid = getbarid(k.Time, k.Date, intervallength);
//            // if not current bar
//            if (barid != curr_barid)
//            {
//                // create a new one
//                newbar(barid);
//                // mark it
//                _isRecentNew = true;
//                // make it current
//                curr_barid = barid;
//                // set time
//                times[times.Count - 1] = k.Time;
//                // set date
//                dates[dates.Count - 1] = k.Date;
//                //set oadate
//                OADateTime[OADateTime.Count - 1] = Util.ToDateTime(k.Date, k.Time).ToOADate();
//            }
//            else _isRecentNew = false;
//            // blend tick into bar
//            // open
//            if (opens[Last()] == 0) opens[Last()] = k.Trade;
//            // high
//            if (k.Trade > highs[Last()]) highs[Last()] = k.Trade;
//            // low
//            if (k.Trade < lows[Last()]) lows[Last()] = k.Trade;
//            // close
//            closes[Last()] = k.Trade;
//            // volume
//            if (k.Size >= 0)
//                vols[Last()] += k.Size;
//            // notify barlist
//            if (_isRecentNew)
//                NewBar(k.Symbol, intervallength);
//        }
//        public void newPoint(string symbol, decimal p, int time, int date, int size)
//        {

//            // get the barcount
//            long barid = getbarid(time,date,intervallength);
//            // if not current bar
//            if (barid != curr_barid)
//            {
//                // create a new one
//                newbar(barid);
//                // mark it
//                _isRecentNew = true;
//                // make it current
//                curr_barid = barid;
//                // set time
//                times[times.Count - 1] = time;
//                // set date
//                dates[dates.Count - 1] = date;
//                //set oadate
//                OADateTime[OADateTime.Count - 1] = Util.ToDateTime(date,time).ToOADate();
//            }
//            else _isRecentNew = false;
//            // blend tick into bar
//            // open
//            if (opens[Last()] == 0) opens[Last()] = p;
//            // high
//            if (p > highs[Last()]) highs[Last()] = p;
//            // low
//            if (p < lows[Last()]) lows[Last()] = p;
//            // close
//            closes[Last()] = p;
//            // volume
//            if (size>=0)
//                vols[Last()] += size;
//            // notify barlist
//            if (_isRecentNew)
//                NewBar(symbol, intervallength);

//        }
//        /// <summary>
//        /// 通过时间来获得bar的序号
//        /// </summary>
//        /// <param name="time"></param>
//        /// <param name="date"></param>
//        /// <param name="intervallength"></param>
//        /// <returns></returns>
//        static internal long getbarid(int time, int date, int intervallength)
//        {
//            // get time elapsed to this point
//            int elap = Util.FT2FTS(time);
//            // get number of this bar in the day for this interval(获得该bar所在当天时间中的排序)
//            long bcount = (int)((double)elap / intervallength);
//            // add the date to the front of number to make it unique //某天的序号加上日期就为该Bar的唯一序号
//            bcount += (long)date * 10000;//20150101
//            return bcount;
//        }

//    }
//}
