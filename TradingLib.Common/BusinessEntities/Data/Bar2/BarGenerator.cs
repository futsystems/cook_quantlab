using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    /// <summary>
    /// Bar数据累加器 用于处理行情数据更新Bar数据 比如累加成交量 更新 OHLC等
    /// 具体Bar开始和结束由IFrequencyGeneratro进行管理
    /// </summary>
    public class BarGenerator
    {
        private NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();

        private BarConstructionType _barConstructionType;
        public BarConstructionType BarConstructionType { get { return _barConstructionType; } }

        private bool _updated;//是否更新过OHLC
        public bool Updated { get { return _updated; } }


        /// <summary>
        /// Tick数据不进行更新 在处理Bar数据时进行更新
        /// </summary>
        //private BarImpl _partialBar;
        //public Bar BarPartialBar { get { return _partialBar; } }

        private BarImpl _currentPartialBar;
        public BarImpl PartialBar { get { return _currentPartialBar; } }


        private Symbol _symbol;
        public Symbol Symbol { get { return _symbol; } }

        private bool _isTickSent;
        public bool TickWareSent { get { return _updated; } }

        public DateTime BarEndTime { get {return _currentPartialBar.EndTime; } }

        BarFrequency _freq;
        public BarGenerator(Symbol symbol,BarFrequency freq,BarConstructionType type)
        {
            this._freq = freq;
            this._symbol = symbol;
            this._barConstructionType = type;
            //初始化PartialBar
            this.CloseBar(DateTime.MinValue);

        }


        public void ProcessBar(Bar bar)
        { 
        
        }

        /// <summary>
        /// 处理实时行情数据
        /// </summary>
        /// <param name="tick"></param>
        public void ProcessTick(Tick tick)
        {
            //if (tick.Type == EnumTickType.TIME) return; 需要通过Time来触发一个PendingPartialBar
            //更新Ask Bid Volume TradeCount 数据
            if (tick.HasAsk())
            {
                this._currentPartialBar.Ask = (double)tick.AskPrice;
                this._currentPartialBar.EmptyBar = false;
            }
            if (tick.HasBid())
            {
                this._currentPartialBar.Bid = (double)tick.BidPrice;
                this._currentPartialBar.EmptyBar = false;
            }
            if (tick.IsTrade())
            {
                
                //this._currentPartialBar.OpenInterest = tick.OpenInterest;
                this._currentPartialBar.EmptyBar = false;
                this._currentPartialBar.TradeCount++;

                //成交量计算
                //1.成交数量累加 问题是数据异常导致某个成交出现多次 会造成成交量错误
                this._currentPartialBar.Volume += tick.Size;
                //2.通过行情系统总成交量相减 获得准确的区间成交量
                //if (this._currentPartialBar.FirstTick == null)
                //{
                //    this._currentPartialBar.Volume = tick.Size;//Bar的第一个成交 则取该成交数量为vol
                //}
                //else
                //{
                //    this._currentPartialBar.Volume = this._currentPartialBar.FirstTick.Size + (tick.Vol - this._currentPartialBar.FirstTick.Vol);//通过Vol相减获得成交量 避免多个成交数据造成的误差
                //}


            }
            //记录时间
            //this._currentPartialBar.BarUpdateTime = tick.Time;

            //根据BarConstructionType来获得当前值并进行更新
            double value = 0;
            bool needUpdate = false;
            switch (this._barConstructionType)
            {
                case BarConstructionType.Default:
                case BarConstructionType.Trade:
                    {
                        if (tick.IsTrade())
                        {
                            value = tick.Price;
                            needUpdate = true;
                        }
                        break;
                    }
                case BarConstructionType.Ask:
                    {
                        if (tick.HasAsk())
                        {
                            value = tick.AskPrice;
                            needUpdate = true;
                        }
                        break;
                    }
                case BarConstructionType.Bid:
                    {
                        if (tick.HasBid())
                        {
                            value = tick.BidPrice;
                            needUpdate = true;
                        }
                        break;
                    }
                default:
                    throw new ArgumentException("not supported contructiontype");
            }

            if (needUpdate)
            {
                //新进入的一个Bar,如果没有更新过Bar则获得的第一个Trade设定为OHLC后面的Trade再更新HLC,获得该Bar的第一个成交数据
                if (!this._updated)
                {
                    this._currentPartialBar.Open = (double)value;
                    this._currentPartialBar.High = (double)value;
                    this._currentPartialBar.Low = (double)value;
                    this._updated = true;
                    //保存第一个Tick
                    this._currentPartialBar.FirstTick = tick;
                    this._currentPartialBar.TradingDay = GetTradingDay(_symbol.SecurityFamily, tick.DateTime());//Bar第一个Tick的时间来判定交易日

                }
                else if ((double)value > this._currentPartialBar.High)
                {
                    this._currentPartialBar.High = (double)value;
                }
                else if ((double)value < this._currentPartialBar.Low)
                {
                    this._currentPartialBar.Low = (double)value;
                }

                this._currentPartialBar.Close = (double)value;

                //标记Tick数据已经处理 进而发送
                this._isTickSent = true;
                this._currentPartialBar.LastTick = tick;

            }

            if (NewTick != null)
            { 
                NewTick(new NewTickEventArgs(this._symbol,tick,this._currentPartialBar));//
            }
        }

        /// <summary>
        /// 判定某个合约交易日信息
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="exTime"></param>
        /// <returns></returns>
        public static int GetTradingDay(SecurityFamily sec, DateTime exTime)
        {
            TradingRange range = sec.MarketTime.JudgeRange(exTime);//根据交易所时间判定当前品种所属交易小节
            if (range == null) return 0;
            DateTime tradingday = range.TradingDay(exTime);
            //if (sec.Exchange.IsInHoliday(tradingday)) return 0;
            return tradingday.ToTLDate();
        }

        /// <summary>
        /// Bar事件 当有Bar结束是对外发送
        /// </summary>
        public event Action<SingleBarEventArgs> NewBar;

        /// <summary>
        /// 行情事件 附带当前Bar数据
        /// </summary>
        public event Action<NewTickEventArgs> NewTick;


        /// <summary>
        /// 产生一个Bar
        /// 该操作由外部的IFreqGenerator根据具体的条件来进行调用触发
        /// </summary>
        /// <param name="barEndTime"></param>
        public void SendNewBar(DateTime nextEndTime)
        {
            bool nodata = !this._updated && this._currentPartialBar.Close == 0;//update表示是否更新过OHLC
            //当前Tick我们无法确认该Bar数据已经结束,需要下一个Tick的时间来判定该Bar是否结束，比如10:30:59秒，在该秒内可能有多个Tick数据，只有当10:31:00这个Tick过来或定时器触发时候才表面10:30分这个Bar结束了
            BarImpl barClosed = this.CloseBar(nextEndTime);
            SingleBarEventArgs e = new SingleBarEventArgs(this._symbol, new BarImpl(barClosed), barClosed.EndTime, this._isTickSent);

            //如果没有更新过数据 则手工更新 通过AskBid来更新OHLC
            if (nodata)
            {
                bool flag2 = (e.Bar.Bid != 0.0) && !double.IsNaN((double)e.Bar.Bid);
                bool flag3 = (e.Bar.Ask != 0.0) && !double.IsNaN((double)e.Bar.Ask);
                if (flag2 && flag3)
                {
                    e.Bar.Open = e.Bar.High = e.Bar.Low = e.Bar.Close = ((e.Bar.Bid + e.Bar.Ask) / 2.0);
                }
                else if (flag2)
                {
                    e.Bar.Open = e.Bar.High = e.Bar.Low = e.Bar.Close = e.Bar.Bid;
                }
                else if (flag3)
                {
                    e.Bar.Open = e.Bar.High = e.Bar.Low = e.Bar.Close = e.Bar.Ask;
                }
            }

            //Bar时间有效 且Bar不为空则触发该Bar
            if (e.Bar.EndTime != System.DateTime.MinValue && !e.Bar.EmptyBar)//EmptyBar是只没有获得任何一个行情的Bar为空Bar
            {
#if DEBUG
                logger.Debug("Bar Closed:" + e.Bar.ToString());
#endif
                if (NewBar != null)
                {
                    NewBar(e);
                }
            }
        }

        /// <summary>
        /// 设定Bar起始时间
        /// </summary>
        /// <param name="barStartTime"></param>
        public void SetBarEndTime(DateTime barEndTime)
        {

            this._currentPartialBar.EndTime = barEndTime;
            //this._partialBar.EndTime = barEndTime;
        }

        /// <summary>
        /// 结束一个Bar数据
        /// 结束一个Bar的时候会同时生成下一个Bar数据
        /// </summary>
        /// <param name="barEndTime"></param>
        private BarImpl CloseBar(DateTime nextEndTime)
        {
             //设定Bar结束时间
            //if (this._currentPartialBar != null)
            //{
            //    this._currentPartialBar.BarEndTime = barEndTime;
            //}
//#if DEBUG
//            logger.Debug(string.Format("Close Bar:{0}", this._currentPartialBar != null ? this._currentPartialBar.ToString() : "Null"));
//#endif   
            BarImpl data = this._currentPartialBar;
            this._isTickSent = false;
            //重新创建临时Bar数据
            this._currentPartialBar = new BarImpl(this._symbol.Symbol, this._freq, nextEndTime);
            this._updated = false;

            if (data != null)
            {
                //默认下一个Bar的高开低收等于上一个Bar的收盘价 如果有有效tick驱动后 Open为该Bar内第一个Tick的数据
                this._currentPartialBar.Open = this._currentPartialBar.Close = this._currentPartialBar.High = this._currentPartialBar.Low = data.Close;
                this._currentPartialBar.Bid = data.Bid;
                this._currentPartialBar.Ask = data.Ask;
            }

            //this._partialBar = this._currentPartialBar.Clone();
            return data;
        }
    
    }

   
}
