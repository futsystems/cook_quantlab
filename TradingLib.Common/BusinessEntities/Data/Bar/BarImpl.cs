using System;
using System.IO;
using System.Collections.Generic;
using TradingLib.API;

namespace TradingLib.Common
{

    /// <summary>
    /// A single bar of price data, which represents OHLC and volume for an interval of time.
    /// </summary>
    [Serializable]
    public class BarImpl : Bar
    {
        int _id = 0;
        public int ID { get { return _id; } set { _id = value; } }
        string _sym = "";
        public string Symbol { get { return _sym; } set { _sym = value; } }

        string _exchange = string.Empty;
        public string Exchange { get { return _exchange; } set { _exchange = value; } }

        private double h = double.MinValue;//最高价
        public double High { get { return h; } set { h = value; } }

        private double l = double.MaxValue;//最低价
        public double Low { get { return l; } set { l = value; } }

        private double o = 0;//开盘价
        public double Open { get { return o; } set { o = value; } }

        private double c = 0;//收盘价
        public double Close { get { return c; } set { c = value; } }

        private double ask = 0;
        public double Ask { get { return ask; } set { ask = value; } }

        private double bid = 0;
        public double Bid { get { return bid; } set { bid = value; } }

        private double v = 0;//成交量
        public double Volume { get { return v; } set { v = value; } }

        private double oi = 0;//持仓
        public double OpenInterest { get { return oi; } set { oi = value; } }

        private int _tradesCount = 0;
        public int TradeCount { get { return _tradesCount; } set { _tradesCount = value; } }

        private int _tradingday = 0;
        /// <summary>
        /// 交易日
        /// </summary>
        public int TradingDay { get { return _tradingday; } set { _tradingday = value; } }



        bool _empty = true;
        /// <summary>
        /// 是否有成交更新过该Bar
        /// </summary>
        public bool EmptyBar { get { return _empty; } set { _empty = value; } }


        BarInterval _intervalType = BarInterval.CustomTime;
        /// <summary>
        /// 频率类别
        /// </summary>
        public BarInterval IntervalType { get { return _intervalType; } set { _intervalType=value; } }

        private int units = 60;
        /// <summary>
        /// 间隔数
        /// </summary>
        public int Interval { get { return units; } set { units = value; } }


        DateTime _endtime = DateTime.MinValue;
        /// <summary>
        /// Bar结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return _endtime; }
            set { _endtime = value; }
        }

        Tick _firstTick = null;
        /// <summary>
        /// 生成Bar的第一个Tick
        /// </summary>
        public Tick FirstTick { get { return _firstTick; } set { _firstTick = value; } }

        Tick _lastTick = null;
        /// <summary>
        /// 生成Bar的最后一个Tick
        /// </summary>
        public Tick LastTick { get { return _lastTick; } set { _lastTick = value; } }



        


        public BarImpl(string symbol, BarFrequency bf, DateTime endTime)
        {
            this._sym = symbol;
            this._endtime = endTime;
            this._intervalType = bf.Type;
            this.units = bf.Interval;

        }

        public void CopyData(BarImpl source)
        {
            this.Open = source.Open;
            this.High = source.High;
            this.Low = source.Low;
            this.Close = source.Close;
            this.Volume = source.Volume;
            this.OpenInterest = source.OpenInterest;
            this.TradeCount = source.TradeCount;
            this.TradingDay = source.TradingDay;
            this.Ask = source.Ask;
            this.Bid = source.Bid;
        }
        public Bar Clone()
        {
            return new BarImpl(this);
        }
        //public BarImpl(BarImpl b)
        //    :this(b as Bar)
        //{
        //    _firstTick = b._firstTick;
        //    _lastTick = b.LastTick;
        //}
        public BarImpl(Bar b)
        {
            _sym = b.Symbol;
            h = b.High;
            l = b.Low;
            o = b.Open;
            c = b.Close;

            v = b.Volume;
            oi = b.OpenInterest;

            ask = b.Ask;
            bid = b.Bid;

            _empty = b.EmptyBar;
            _tradesCount = b.TradeCount;
            _tradingday = b.TradingDay;

            _intervalType = b.IntervalType;
            units = b.Interval;

            _endtime = b.EndTime;
        }

        public BarImpl(int interval)
        {
            units = interval;
            _intervalType = BarInterval.CustomTime;

        }

        public BarImpl()
            : this(60)
        { 
        
        }

        
        /// <summary>
        /// bt是用来计算一天中的第几根Bar是用序号来计算的
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        //private int bt(int time) 
        //{
        //    // get time elapsed to this point
        //    int elap = Util.FT2FTS(time);
        //    // get seconds per bar
        //    int secperbar = Interval;
        //    // get number of this bar in the day for this interval
        //    int bcount = (int)((double)elap / secperbar);
        //    return bcount;
        //}

        /// <summary>
        /// Accepts the specified tick.
        /// </summary>
        /// <param name="t">The tick you want to add to the bar.</param>
        /// <returns>true if the tick is accepted, false if it belongs to another bar.</returns>
        //public bool newTick(Tick k)
        //{
        //    TickImpl t = (TickImpl)k;
        //    if (_sym == "") _sym = t.Symbol;
        //    if (_sym != t.Symbol) throw new InvalidTick();
        //    //if (_time == 0) { _time = t.time; bardate = t.date; }
        //    if (_updatetime == 0) { _starttime = Util.ToDateTime(_bardate, bt(t.Time)); }
        //    if (_bardate != t.Date) DAYEND = true;
        //    else DAYEND = false;
        //    // check if this bar's tick//如果该bar不在改时间段中则return false
        //    if ((bt(t.Time) != _starttime.ToTLTime()) || (_bardate != t.Date)) return false; 
        //    // if tick doesn't have trade or index, ignore
        //    if (!t.isTrade && !t.isIndex) return true; //我们只能通过trade来进行bar的形成，没有成交的ask bid不能作为bar数据
        //    _tradesCount++; // count it 累计该bar内的trade trade/ask/bid
        //    _new = _tradesCount == 1;//是否是新bar的标准  tradesinbar==1
        //    // only count volume on trades, not indicies
        //    if (!t.isIndex) v += t.Size; // add trade size to bar volume 如果不是质数 则bar的volume通过trades来进行累加
        //    //更新bar的o h l c 数据
        //    if (o == 0) o = t._trade;//如果open为0 赋初值
        //    if (t._trade > h) h = t._trade;
        //    if (t._trade < l) l = t._trade;
        //    c = t._trade;
        //    return true;
        //}

        public override string ToString()
        {
            return string.Format("{7}/{6} {0}-OHLC({1},{2},{3},{4},{5},{8})", this.Symbol, this.Open, this.High, this.Low, this.Close, this.Volume, this.EndTime.ToTLDateTime(),this.TradingDay,this.OpenInterest);
        }

        #region 读写Bar不能修改 否则会造成数据格式不兼容
        /// <summary>
        /// 将Bar数据写入
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="bar"></param>
        public static void Write(BinaryWriter writer,BarImpl bar)
        {
            writer.Write(Util.ToTLDateTime(bar.EndTime));
            writer.Write(bar.Symbol);
            writer.Write((int)bar.IntervalType);
            writer.Write(bar.Interval);

            writer.Write(bar.Ask);
            writer.Write(bar.Bid);
            writer.Write(bar.Open);
            writer.Write(bar.High);
            writer.Write(bar.Low);
            writer.Write(bar.Close);

            writer.Write(bar.Volume);
            writer.Write(bar.OpenInterest);
            writer.Write(bar.TradeCount);
            writer.Write(bar.TradingDay);
            writer.Write(bar.EmptyBar);
            writer.Write(bar.ID);
        }

        /// <summary>
        /// 读取Bar数据
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static BarImpl Read(BinaryReader reader)
        {
            BarImpl bar = new BarImpl();
            long date = reader.ReadInt64();
            bar.EndTime = Util.ToDateTime(date);
            bar.Symbol = reader.ReadString();
            bar.IntervalType = (BarInterval)reader.ReadInt32();
            bar.Interval = reader.ReadInt32();

            bar.Ask = reader.ReadDouble();
            bar.Bid = reader.ReadDouble();
            bar.Open = reader.ReadDouble();
            bar.High = reader.ReadDouble();
            bar.Low = reader.ReadDouble();
            bar.Close = reader.ReadDouble();

            bar.Volume = reader.ReadInt32();
            bar.OpenInterest = reader.ReadInt32();
            bar.TradeCount = reader.ReadInt32();
            bar.TradingDay = reader.ReadInt32();
            bar.EmptyBar = reader.ReadBoolean();
            bar.ID = reader.ReadInt32();
            return bar;
        }
        #endregion


        /// <summary>
        /// 序列化bar
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string Serialize(Bar b)
        {
            const char d = ',';
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(b.EndTime);
            sb.Append(d);
            sb.Append(b.Symbol);
            sb.Append(d);
            sb.Append((int)b.IntervalType);
            sb.Append(d);
            sb.Append(b.Interval);
            sb.Append(d);
            sb.Append(b.Ask);
            sb.Append(d);
            sb.Append(b.Bid);
            sb.Append(d);
            sb.Append(b.Open);
            sb.Append(d);
            sb.Append(b.High);
            sb.Append(d);
            sb.Append(b.Low);
            sb.Append(d);
            sb.Append(b.Close);
            sb.Append(d);
            sb.Append(b.Volume);
            sb.Append(d);
            sb.Append(b.OpenInterest);
            sb.Append(d);
            sb.Append(b.TradeCount);
            sb.Append(d);
            sb.Append(b.TradingDay);
            sb.Append(d);
            sb.Append(b.EmptyBar);
            return sb.ToString();
        }

        /// <summary>
        /// 反序列化bar
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Bar Deserialize(string msg)
        {
            string[] r = msg.Split(',');
            Bar b = new BarImpl();
            b.EndTime = DateTime.Parse(r[0]);
            b.Symbol = r[1];
            b.IntervalType = (BarInterval)int.Parse(r[2]);
            b.Interval = int.Parse(r[3]);
            b.Ask = double.Parse(r[4]);
            b.Bid = double.Parse(r[5]);

            b.Open = double.Parse(r[6]);
            b.High = double.Parse(r[7]);
            b.Low = double.Parse(r[8]);
            b.Close = double.Parse(r[9]);

            b.Volume = int.Parse(r[10]);
            b.OpenInterest = int.Parse(r[11]);
            b.TradeCount = int.Parse(r[12]);
            b.TradingDay = int.Parse(r[13]);
            b.EmptyBar = bool.Parse(r[14]);

            return b;


        }

        /// <summary>
        /// convert a bar into an array of ticks
        /// 将bar转换成Tick数据 通过 4分法将o h l c 形成对应的 trade tick
        /// </summary>
        /// <param name="bar"></param>
        /// <returns></returns>
        
//        public static Tick[] ToTick(Bar bar)
//        {
//            if (!bar.isValid) return new Tick[0];
//            List<Tick> list = new List<Tick>();
//            list.Add(TickImpl.NewTrade(bar.Symbol, bar.BarDate, bar.BarStartTime.ToTLTime(), (decimal)bar.Open,
//(int)((double)bar.Volume / 4), string.Empty));
//            list.Add(TickImpl.NewTrade(bar.Symbol, bar.BarDate, bar.BarStartTime.ToTLTime(),
//(decimal)bar.High, (int)((double)bar.Volume / 4), string.Empty));
//            list.Add(TickImpl.NewTrade(bar.Symbol, bar.BarDate, bar.BarStartTime.ToTLTime(), (decimal)bar.Low,
//(int)((double)bar.Volume / 4), string.Empty));
//            list.Add(TickImpl.NewTrade(bar.Symbol, bar.BarDate, bar.BarStartTime.ToTLTime(),
//(decimal)bar.Close, (int)((double)bar.Volume / 4), string.Empty));
//            return list.ToArray();
//        }
        
        /// <summary>
        /// parses message into a structured bar request
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        //public static BarRequest ParseBarRequest(string msg)
        //{
        //    string[] r = msg.Split(',');
        //    BarRequest br  = new BarRequest();
        //    br.Symbol = r[(int)BarRequestField.Symbol];
        //    br.Interval = Convert.ToInt32(r[(int)BarRequestField.BarInt]);
        //    br.StartDate = int.Parse(r[(int)BarRequestField.StartDate]);
        //    br.StartTime = int.Parse(r[(int)BarRequestField.StartTime]);
        //    br.EndDate= int.Parse(r[(int)BarRequestField.EndDate]);
        //    br.EndTime = int.Parse(r[(int)BarRequestField.EndTime]);
        //    br.CustomInterval = int.Parse(r[(int)BarRequestField.CustomInterval]);
        //    br.ID = long.Parse(r[(int)BarRequestField.ID]);
        //    br.Client = r[(int)BarRequestField.Client];
        //    return br;
        //}

        ///// <summary>
        ///// request historical data for today
        ///// </summary>
        ///// <param name="symbol"></param>
        ///// <param name="interval"></param>
        ///// <returns></returns>
        //public static string BuildBarRequest(string symbol, BarInterval interval)
        //{
        //    return BuildBarRequest(new BarRequest(symbol, (int)interval, Util.ToTLDate(), 0, Util.ToTLDate(), Util.ToTLTime(),string.Empty));
        //}
        ///// <summary>
        ///// bar request for symbol and interval from previous date through present time
        ///// </summary>
        ///// <param name="symbol"></param>
        ///// <param name="interval"></param>
        ///// <param name="startdate"></param>
        ///// <returns></returns>
        //public static string BuildBarRequest(string symbol, BarInterval interval, int startdate)
        //{
        //    return BuildBarRequest(new BarRequest(symbol, (int)interval, startdate, 0, Util.ToTLDate(), Util.ToTLTime(),string.Empty));
        //}
        //public static string BuildBarRequest(string symbol, int interval, int startdate)
        //{
        //    return BuildBarRequest(new BarRequest(symbol, interval, startdate, 0, Util.ToTLDate(), Util.ToTLTime(), string.Empty));
        //}
        ///// <summary>
        ///// builds bar request
        ///// </summary>
        ///// <param name="br"></param>
        ///// <returns></returns>
        //public static string BuildBarRequest(BarRequest br)
        //{
        //    string[] r = new string[] 
        //    {
        //        br.Symbol,
        //        br.Interval.ToString(),
        //        br.StartDate.ToString(),
        //        br.StartTime.ToString(),
        //        br.EndDate.ToString(),
        //        br.EndTime.ToString(),
        //        br.ID.ToString(),
        //        br.CustomInterval.ToString(),
        //        br.Client,
        //    };
        //    return string.Join(",", r);
            
        //}

        //计算多少个bar对应的时间点
        public static DateTime DateFromBarsBack(int barsback, BarInterval intv) { return DateFromBarsBack(barsback, intv, DateTime.Now); }
        public static DateTime DateFromBarsBack(int barsback, BarInterval intv, DateTime enddate) { return DateFromBarsBack(barsback, (int)intv, enddate); }
        public static DateTime DateFromBarsBack(int barsback, int interval) { return DateFromBarsBack(barsback, interval, DateTime.Now); }
        public static DateTime DateFromBarsBack(int barsback, int interval, DateTime enddate)
        {
           return enddate.Subtract(new TimeSpan(0,0,interval*barsback));
        }
        //计算从某个时间以来有多少个bar
        public static int BarsBackFromDate(BarInterval interval, int startdate) { return BarsBackFromDate(interval, startdate, Util.ToTLDate()); }
        public static int BarsBackFromDate(BarInterval interval, int startdate, int enddate) { return BarsBackFromDate(interval, Util.ToDateTime(startdate, 0), Util.ToDateTime(enddate,Util.ToTLTime())); }
        public static int BarsBackFromDate(BarInterval interval, DateTime startdate, DateTime enddate)
        {
            double start2endseconds = enddate.Subtract(startdate).TotalSeconds;
            int bars = (int)((double)start2endseconds / (int)interval);
            return bars;
        }

        /// <summary>
        /// build bar request for certain # of bars back from present 获得自当前时间开始多少个bar
        /// </summary>
        /// <param name="sym"></param>
        /// <param name="barsback"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        //public static string BuildBarRequestBarsBack(string sym, int barsback, int interval)
        //{
        //    DateTime n = DateTime.Now;
        //    return BarImpl.BuildBarRequest(new BarRequest(sym, interval, Util.ToTLDate(BarImpl.DateFromBarsBack(barsback, interval, n)), Util.ToTLTime(BarImpl.DateFromBarsBack(barsback, interval, n)), Util.ToTLDate(n), Util.ToTLTime(n), string.Empty));
        //}

        
    }



}
