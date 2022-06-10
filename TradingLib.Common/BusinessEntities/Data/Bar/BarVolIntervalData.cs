//using System;
//using System.Collections.Generic;
//using System.Text;
//using TradingLib.API;
//namespace TradingLib.Common
//{
//    /// <summary>
//    /// 通过成交量来形成对应的bar
//    /// 这里的tick/time/vol是对应的Freqency引擎 用于驱动bar数据的形成
//    /// </summary>
//    public class VolIntervalData : IntervalData
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
//        public VolIntervalData(int unitsPerInterval)
//        {
//            intervallength = unitsPerInterval;
//        }
//        void newbar()
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
//        }
//        public void addbar(Bar mybar)
//        {
//            _Count++;
//            closes.Add((decimal)mybar.Close);
//            opens.Add((decimal)mybar.Open);
//            dates.Add(mybar.BarDate);
//            highs.Add((decimal)mybar.High);
//            lows.Add((decimal)mybar.Close);
//            vols.Add(mybar.Volume);
//            times.Add(mybar.BarStartTime.ToTLTime());
//            OADateTime.Add(Util.ToDateTime(mybar.BarDate, mybar.BarStartTime.ToTLTime()).ToOADate());
//        }
//        public void Reset()
//        {
//            _Count = 0;
//            opens.Clear();
//            closes.Clear();
//            highs.Clear();
//            lows.Clear();
//            dates.Clear();
//            times.Clear();
//            vols.Clear();
//            OADateTime.Clear();
//        }
//        int curr_barid = -1;
//        int intervallength = 60;
//        internal List<decimal> opens = new List<decimal>();
//        internal List<decimal> closes = new List<decimal>();
//        internal List<decimal> highs = new List<decimal>();
//        internal List<decimal> lows = new List<decimal>();
//        internal List<long> vols = new List<long>();
//        internal List<int> dates = new List<int>();
//        internal List<int> times = new List<int>();
//        internal List<int> ticks = new List<int>();
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
//            // process data
//            // if we have no bars or 
//            if ((curr_barid == -1) || (vols[curr_barid] + k.Size > intervallength))
//            {
//                // create a new one
//                newbar();
//                // mark it
//                _isRecentNew = true;
//                // make it current
//                curr_barid++;
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
//            // don't set volume for index
//            if (k.Size >= 0)
//                vols[Last()] += k.Size;

//            // notify barlist
//            if (_isRecentNew)
//                NewBar(k.Symbol, intervallength);
//        }
//        public void newPoint(string symbol, decimal p, int time, int date, int size)
//        {
//            // if we have no bars or 
//            if ((curr_barid == -1) || (vols[curr_barid] + size > intervallength))
//            {
//                // create a new one
//                newbar();
//                // mark it
//                _isRecentNew = true;
//                // make it current
//                curr_barid++;
//                // set time
//                times[times.Count - 1] = time;
//                // set date
//                dates[dates.Count - 1] = date;
//                //set oadate
//                OADateTime[OADateTime.Count - 1] = Util.ToDateTime(date, time).ToOADate();
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
//            // don't set volume for index
//            if (size>=0) 
//                vols[Last()] += size;
//            // notify barlist
//            if (_isRecentNew)
//                NewBar(symbol, intervallength);

//        }


//    }

//}
