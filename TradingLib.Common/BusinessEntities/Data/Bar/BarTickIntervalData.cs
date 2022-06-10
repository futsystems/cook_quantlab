//using System;
//using System.Collections.Generic;
//using System.Text;
//using TradingLib.API;
//namespace TradingLib.Common
//{
//    public class TickIntervalData : IntervalData
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
//        public TickIntervalData(int unitsPerInterval)
//        {
//            intervallength = unitsPerInterval;
//        }
//        void newbar()
//        {
//            _Count++;
//            ticks.Add(0);
//            opens.Add(0);
//            closes.Add(0);
//            highs.Add(0);
//            lows.Add(decimal.MaxValue);
//            vols.Add(0);
//            times.Add(0);
//            dates.Add(0);
//            OADateTime.Add(0);
//        }
//        /// <summary>
//        /// 新增加一个bar
//        /// </summary>
//        /// <param name="mybar"></param>
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
//        /// <summary>
//        /// 重置
//        /// </summary>
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
//        }

//        //数据结构
//        int curr_barid = -1;
//        int intervallength = 60;
//        internal List<int> ticks = new List<int>();
//        internal List<decimal> opens = new List<decimal>();
//        internal List<decimal> closes = new List<decimal>();
//        internal List<decimal> highs = new List<decimal>();
//        internal List<decimal> lows = new List<decimal>();
//        internal List<long> vols = new List<long>();
//        internal List<int> dates = new List<int>();
//        internal List<int> times = new List<int>();
//        internal List<double> OADateTime = new List<double>();
//        internal int _Count = 0;
//        internal bool _isRecentNew = false;
//        /// <summary>
//        /// 通过序号获得bar数据
//        /// </summary>
//        /// <param name="index"></param>
//        /// <param name="symbol"></param>
//        /// <returns></returns>
//        public Bar GetBar(int index, string symbol)
//        {
//            Bar b = new BarImpl();
//            if (index >= _Count) return b;//序号大于当前bar数
//            else if (index < 0)//index<0 倒数
//            {
//                index = _Count - 1 + index;
//                if (index < 0) return b;
//            }
//            //生成对应的bar数据
//            //b = new BarImpl(opens[index], highs[index], lows[index], closes[index], vols[index], dates[index], times[index], symbol,intervallength);
//            ////如果该序号为最后一个序号 则b.isnew为最新
//            //if (index == Last()) b.isNew = _isRecentNew;
//            return null;
//        }
//        /// <summary>
//        /// 获得最新的bar
//        /// </summary>
//        /// <param name="symbol"></param>
//        /// <returns></returns>
//        public Bar GetBar(string symbol) { return GetBar(Last(), symbol); }
//        /// <summary>
//        /// 获得一个Tick数据
//        /// </summary>
//        /// <param name="k"></param>
//        public void newTick(Tick k) 
//        { 
//            // ignore quotes ask bid直接返回
//            if (k.Trade == 0) return;
//            // if we have no bars or we'll exceed our interval length w/this tick
//            //如果当前bar序号为-1 或者ticks[] 为我们设定的区间大小,则新增一个bar
//            if ((curr_barid == -1) || (ticks[curr_barid] == intervallength))
//            {
//                // create a new one
//                newbar();
//                // mark it
//                _isRecentNew = true;
//                // make it current current序号递增
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
//            // store value of Last
//            int l = Last();
//            // open
//            if (opens[l] == 0) opens[l] = k.Trade;
//            // high
//            if (k.Trade > highs[l]) highs[l] = k.Trade;
//            // low
//            if (k.Trade < lows[l]) lows[l] = k.Trade;
//            // close
//            closes[l] = k.Trade;
//            // count ticks
//            ticks[l]++;
//            // don't set volume for index
//            if (k.Size > 0)
//                vols[l] += k.Size;            
//            // notify barlist 如果是新bar则通知 barlist
//            if (_isRecentNew)
//                NewBar(k.Symbol, intervallength);
//        }
//        /// <summary>
//        /// 通过数值p来生成bar
//        /// </summary>
//        /// <param name="symbol"></param>
//        /// <param name="p"></param>
//        /// <param name="time"></param>
//        /// <param name="date"></param>
//        /// <param name="size"></param>
//        public void newPoint(string symbol, decimal p, int time, int date, int size)
//        {
//            // if we have no bars or we'll exceed our interval length w/this tick
//            if ((curr_barid == -1) || (ticks[curr_barid] == intervallength))
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
//                OADateTime[OADateTime.Count - 1] = Util.ToDateTime(date,time).ToOADate();
//            }
//            else _isRecentNew = false;
//            // blend tick into bar
//            // store value of Last
//            int l = Last();
//            // open
//            if (opens[l] == 0) opens[l] = p;
//            // high
//            if (p > highs[l]) highs[l] = p;
//            // low
//            if (p < lows[l]) lows[l] = p;
//            // close
//            closes[l] = p;
//            // count ticks
//            ticks[l]++;
//            // don't set volume for index
//            if (size>0)
//                vols[l] += size;
//            // notify barlist
//            if (_isRecentNew)
//                NewBar(symbol, intervallength);

//        }

//    }
//}
