using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections;
using TradingLib.API;
using System.Threading;


namespace TradingLib.Common
{
    /// <summary>
    /// Used to watch a stream of ticks, and send alerts when the stream goes idle for a specified time.
    /// </summary>
    public class TickWatcher
    {
        private NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();

        public void GotTick(Tick k) { newTick(k); }
        public bool isValid { get { return _continue; } }
        
        /// <summary>
        ///  returns count of symbols that have ticked at least once
        /// </summary>
        public int Count { get { return _last.Count; } }
        /// <summary>
        /// gets last time a tick was received for symbol
        /// </summary>
        /// <param name="sym"></param>
        /// <returns></returns>
        public int this[string sym]
        {
            get
            {
                int lasttime = 0;
                if (_last.TryGetValue(sym, out lasttime))
                    return lasttime;
                return 0;
            }
        }


        public IEnumerator GetEnumerator()
        {
            foreach (string s in _last.Keys)
                yield return s;
        }
        private Dictionary<string, int> _last = new Dictionary<string, int>();
        //private Dictionary<string, bool> _lastlive = new Dictionary<string, bool>();


        int _symboletickdelay = 5;
        public int SymbolIdleSpan { get { return _symboletickdelay; } set { _symboletickdelay = value; } }
        public bool IsSymbolTickLive(string symbol)
        {
            int lasttick = 0;
            if (_last.TryGetValue(symbol, out lasttick))
            {
                int span = Util.FTDIFF(lasttick, this.RecentTime);
                return span > _symboletickdelay ? false : true; //判断该行情的最后一次Tick时间和当前最新Tick时间，如果超过设定范围则该合约的tick出现idle状态 
            }
            else //如果没有记录过该合约 则该合约处于非活动状态
            {
                return false;
            }
        }
        /// <summary>
        /// alert thrown when AlertThreshold is exceeded for a symbol
        /// </summary>
        public event SymDelegate GotAlert;

        /// <summary>
        /// 当行情有延误时触发
        /// </summary>
        //public event SymDelegate GotTickDelayed;


        /// <summary>
        /// 当某个合约第一个行情达到
        /// </summary>
        public event SymDelegate GotFirstTick;
        private bool _alertonfirst = true;
        /// <summary>
        /// 某个合约第一个行情到达时候是否进行通知
        /// </summary>
        public bool FireFirstTick { get { return _alertonfirst; } set { _alertonfirst = value; } }


        private int _defaultwait = 5;
        /// <summary>
        /// minimum threshold in seconds when no tick updates have been received for a single symbol, alerts can be thrown.
        /// 多少时间内某个合约没有收到行情则报警
        /// </summary>
        public int AlertThreshold { get { return _defaultwait; } set { _defaultwait = value; } }

        /// <summary>
        /// gets list of symbols that have never had ticks pass through watcher
        /// 从未收到过行情的合约
        /// </summary>
        public string SymbolsNeverTicked
        {
            get
            {
                List<string> syms = new List<string>(this._ast.Count);
                for (int i = 0; i<_ast.Count; i++)
                {
                    string sym = _ast.getlabel(i);
                    if (this[sym] == 0)
                        syms.Add(sym);
                }
                return string.Join(",",syms.ToArray());
            }
        }
        /// <summary>
        /// gets stringified symbols which have had ticks pass through the watcher
        /// 收到过行情的合约
        /// </summary>
        public string SymbolsTicked
        {
            get
            {
                List<string> syms = new List<string>(_last.Count);
                foreach (string sym in _last.Keys)
                    if (_last[sym]!=0)
                        syms.Add(sym);
                return string.Join(",", syms.ToArray());
            }
        }



        System.ComponentModel.BackgroundWorker _bw = null;
        volatile int _lasttime = 0;
        /// <summary>
        /// most recent time received
        /// 最新行情时间
        /// </summary>
        public int RecentTime { get { return _lasttime; } }

        int _ticks = -1;
        /// <summary>
        /// gets count of ticks which have passed through watcher
        /// </summary>
        public int TickCount { get { return _ticks; } }

        int _firsttime = 0;
        public int TickStartTime { get { return _firsttime; } }


        /// <summary>
        /// 判断某个Tick是否有延迟
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        //public bool IsTickDelay(Tick k,int timewait=5)
        //{
        //    int span = Util.FTDIFF(_lasttime, k.Time);
        //    return span > _defaultwait;
        //}
        /// <summary>
        /// Watches the specified tick.
        /// Alerts if wait time exceeded.
        /// </summary>
        /// <param name="tick">The tick.</param>
        /// <returns></returns>
        public bool newTick(Tick k) 
        {
            //最近行情时间
            _lasttime = k.Time;

            //设定firsttime
            if (_firsttime < 0)
                _firsttime = k.Time;

            //需要进行livecheck 并且收到的行情数量大于我们设定的tick数 进行行情检查
            if (_livecheck && (_ticks++ > CheckLiveAfterTickCount))
            {
                bool dmatch = k.Date == Util.ToTLDate();//行情日期一致
                bool tmatch = Util.FTDIFF(_lasttime, Util.ToTLTime()) < CheckLiveMaxDelaySec;//行情时间在我们设定的延迟范围内
                //日期和时间均吻合,则表明当前行情系统在线
                _islive = dmatch && tmatch;
                logger.Info("TickStream live check status:" + _islive.ToString() + " tickdata:" + k.Date.ToString() + " tick time:" + k.Time.ToString());
                _livecheck = false;
            }

            //这里需要排除历史行情快照的加载
            //是否所有合约都有行情数据          单个合约行情警报         单个行情首个Tick通知
            if ((AllsymbolsTicking != null) || (GotAlert != null) || (GotFirstTick != null))
            {
                int last = k.Time;
                //检查是否已经记录该合约的信息
                // ensure we are storing per-symbol times
                if (!_last.TryGetValue(k.Symbol,out last))
                {
                    //1.添加该合约的时间记录
                    _last.Add(k.Symbol, k.Time);

                    //2.如果我们需要报告第一个tick，则我们触发firsttick事件
                    if (_alertonfirst) // if we're notifying when first tick arrives, do it.
                        if (GotFirstTick != null)
                            GotFirstTick(k.Symbol);

                    //2.如果外部ticktracker不为空 则比较是否已经包含了所有symbol
                    if (_ast != null)
                    {
                        if (!alltrading && (_ast.Count == Count))
                        {
                            alltrading = true;
                            if (AllsymbolsTicking != null)
                                AllsymbolsTicking(Util.ToTLTime());
                        }
                    }
                    last = k.Time;
                    return false;
                }

                // store time 保存该合约的时间
                _last[k.Symbol] = k.Time;

                // if alerts requested, check for idle symbol 
                //如果行情当前时间与本地保存的上一次行情时间间隔大于阀值，触发报警
                if (GotAlert != null)
                {
                    int span = Util.FTDIFF(last, k.Time);
                    bool alert = span > this.AlertThreshold;
                    if (alert)
                        GotAlert(k.Symbol);
                    return alert;
                }

                
            }
            return false; 
        }

        #region SendAlerts 内部主动发起的发送警报
        /// <summary>
        /// send alerts for idle symbols using current time as comparison point
        /// </summary>
        /// <returns></returns>
        public int SendAlerts() { return SendAlerts(DateTime.Now); }

        /// <summary>
        /// Sends the alerts for tickstreams who have gone idle based on the provided datetime.
        /// </summary>
        /// <param name="date">The current datetime.</param>
        public int SendAlerts(DateTime time)
        {
            return SendAlerts(Util.DT2FT(time), _defaultwait);
        }

        /// <summary>
        /// sends alerts for i
        /// </summary>
        /// <param name="date"></param>
        public int SendAlerts(int time)
        {
            return SendAlerts(time, _defaultwait);
        }

        /// <summary>
        /// Sends the alerts for tickstreams who have gone idle based on the provided datetime.
        /// </summary>
        /// <param name="date">The datetime.</param>
        /// <param name="AlertSecondsWithoutTick">The alert seconds without tick.</param>
        public int SendAlerts(int time, int AlertSecondsWithoutTick) 
        {
            int c = 0;
            foreach (string sym in _last.Keys.ToArray())
            {
                if (GotAlert != null)
                {
                    int ticktime = _last[sym];
                    if (Util.FTDIFF(ticktime, time) > AlertSecondsWithoutTick)
                    {
                        logger.Info("time:" + time.ToString() + " sym lasttime:" + _last[sym].ToString());
                        c++;
                        GotAlert(sym);
                    }
                }
            }
            return c;
        }

        #endregion



        bool _continue = true;

        public event Int32Delegate PollProcess;
        long _pollint = 0;
        //默认1秒检查一次行情通道情况
        public const int DEFAULTPOLLINT = 1000;
        public int BackgroundPollInterval { get { return (int)_pollint; } set { _pollint = (long)Math.Abs(value); if (_pollint == 0) Stop(); } }
        
        public TickWatcher(int BackgroundPollIntervalms) : this(BackgroundPollIntervalms,null) { }
        public TickWatcher() : this(DEFAULTPOLLINT,null) { }
        public TickWatcher(bool islive) : this(islive ? DEFAULTPOLLINT : 0, null) { }
        public TickWatcher(bool islive, GenericTrackerI symtracker) : this(islive ? DEFAULTPOLLINT : 0, symtracker) { }
        public TickWatcher(GenericTrackerI symboltracker) : this(DEFAULTPOLLINT, symboltracker) { }

        /// <summary>
        /// creates a tickwatcher and polls specificed millseconds
        /// if timer has expired, sends alert.
        /// Background polling occurs in addition to tick-induced time checks.
        /// </summary>
        /// <param name="BackgroundPollIntervalms">Value in millseconds to wait between checks.  0 = disable background checks</param>
        public TickWatcher(int BackgroundPollIntervalms, GenericTrackerI symboltracker)
        {
            _ast = symboltracker;
            _pollint = (long)Math.Abs(BackgroundPollIntervalms);
            //if (_pollint != 0)
            //    Start();
        }

        public void Start()
        {
            if ((_bw ==null) && (_pollint!=0))
            {
                _bw = new System.ComponentModel.BackgroundWorker();
                _bw.DoWork += new System.ComponentModel.DoWorkEventHandler(_bw_DoWork);
                _bw.WorkerSupportsCancellation = true;
                _bw.RunWorkerAsync();
            }
            else if ((_bw!=null) && !_continue)
            {
                _continue = true;
                _bw.RunWorkerAsync();
            }
        }



        bool sentmissingfirstticks = false;

        bool alltrading = false;
        //bool massalert = false;
        //int _lastmass = 0;
        GenericTrackerI _ast = null;
        /// <summary>
        /// gets reference to active symbol tracker
        /// </summary>
        public GenericTrackerI ActiveSymbolTracker { get { return _ast; } }

        public event Int32Delegate StarttimeAndMissingTicks;

        /// <summary>
        /// 如果所有合约行情均到到 对外触发
        /// </summary>
        public event Int32Delegate AllsymbolsTicking;

        int _stoptime = 0;
        int _starttime = 0;
        /// <summary>
        /// 是否在时间段内报警
        /// </summary>
        public bool TimeSpanSetted { get { return _starttime + _stoptime != 0; } }

        /// <summary>
        /// 开始报警时间
        /// </summary>
        public int StartAlertTime { get { return _starttime; } set { _starttime = value; } }

        /// <summary>
        /// 停止报警时间
        /// </summary>
        public int StopAlertTime { get { return _stoptime; } set { _stoptime = value; } }

        int _defaultmass = 10;
        /// <summary>
        /// minimum threshold when no ticks have been received for many symbols
        /// 多少秒内没有收到任何行情
        /// </summary>
        public int MassAlertThreshold { get { return _defaultmass; } set { _defaultmass = value; } }


        bool _ismassalertcleared = false;
        /// <summary>
        /// will be true if mass alerted existed previously and was cleared.
        /// this value can only be checked once as it will reset to false once read
        /// 如果之前有过行情中断报警并且报警被取消过 该值读取后就归位
        /// </summary>
        public bool isMassAlertCleared { get { bool v = _ismassalertcleared; if (v) _ismassalertcleared = !v; return v; } }

        bool _alerting = false;
        /// <summary>
        /// 是否处于MassAlert报警状态中
        /// </summary>
        public bool isMassAlerting { get { return _alerting; } }

        /// <summary>
        /// alert thrown when no ticks have arrived since AlertThreshold.
        /// Time of last tick is provided.
        /// 如果上次行情以来 在设定时间段内没有任何行情则报警
        /// </summary>
        public event Int32Delegate GotMassAlert;

        /// <summary>
        /// 如果报警后，又有行情到达，则报警取消
        /// </summary>
        public event Int32Delegate GotMassAlertCleard;

        int _setspantime = 0;
        /// <summary>
        /// 整体的交易时间段
        /// 90000-113000
        /// 130000 - 151500
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="stopttime"></param>
        public void UpdateTimeSpan(int starttime,int stopttime)
        {
            _starttime = starttime;
            _stoptime = stopttime;
            _setspantime = Util.ToTLTime();//记录设定时间
        }

        /// <summary>
        /// 是否在设定的时间段内
        /// </summary>
        bool IsInTimeSpan
        {
            get
            {
                int now = Util.ToTLTime();
                if (now >= _starttime && now < _stoptime)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// 重置当处于非行情时间段时候，重置相关属性
        /// </summary>
        public void Reset()
        {
            _starttime = 0;
            _stoptime = 0;
            _lasttime = 0;

            _islive = false;
            _livecheck = true;//重新做行情激活检查
            _ticks = -1;
        }

        void _bw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (_continue)
            {
                if (_bw.CancellationPending || e.Cancel)
                    break;
                if (PollProcess != null)
                    PollProcess(_lasttime);
                //如果启动后没有获得任何旱情 recenttime为0，这里需要加入机制 用于触发当没有任何行情到达时 触发massalert
                //Util.Debug("行情检查,MassAlertThreshold:" + this.MassAlertThreshold.ToString() + " RecentTime:" + this.RecentTime.ToString() + " StartTime:" + this.StartAlertTime.ToString() + " StopTime:" + this.StopAlertTime.ToString(), QSEnumDebugLevel.INFO);
                //合约整体异常    批量合约数值  最近行情到达时间
                //需要对外触发行情异常警报
                if (GotMassAlert != null)
                {
                    if(this.TimeSpanSetted && this.IsInTimeSpan && this.MassAlertThreshold!=0)//设定了起止时间 当前处于该时间段内 设定了MassAlertThreshold
                    {
                        int span = 0;
                        if (_lasttime == 0)//如果没有行情过来 则判断离start多少时间
                        {
                            //有可能是盘中启动 设定的timesapn 这里以设定时的时间为准
                            span = Util.FTDIFF(_starttime>_setspantime?_starttime:_setspantime, Util.ToTLTime());
                        }
                        else//如果有行情过来更新过lasttime,则比较_lasttime
                        {
                            span = Util.FTDIFF(_lasttime, Util.ToTLTime());
                        }

                        //注意这里如果报警没有得到正常处理,以后的报警如何处理？是否允许多次尝试的机会？
                        bool alert = (span > this.MassAlertThreshold);
                        if (alert && !_alerting)//需要报警并且没有触发过报警则对外报警
                        {
                            _alerting = true;
                            GotMassAlert(_lasttime);
                        }
                        else if (!alert && _alerting)//massalert不需要报警 且当前处于报警状态，则取消报警
                        {
                            _alerting = false;
                            _ismassalertcleared = true;
                            GotMassAlertCleard(_lasttime);
                        }
                    }
                }
                //行情处于激活状态 需要检查行情的延迟状态
                //if (isLive)
                //{
                //    //用当前最新行情时间作为标准 去判断维护列表中的合约 合约的最后更新时间与当前最新Tick时间差超过设定范围，认为该行情处于异常状态
                //    SendAlerts(_lasttime);
                //}

                //有开始时间，且最近时间大于开时间，且StarttimeAndMissingTicks没有触发过，
                if (!alltrading && !sentmissingfirstticks && (_starttime!=0) && (_lasttime > _starttime))
                {
                    sentmissingfirstticks = true;
                    if (StarttimeAndMissingTicks != null)
                        StarttimeAndMissingTicks(Util.ToTLTime());
                }
                System.Threading.Thread.Sleep((int)_pollint);
            }
        }

        int _livecheckafterXticks = 10;
        /// <summary>
        /// wait to do live test after X ticks have arrived
        ///多少个行情数据到达后检查行情整体工作情况
        /// </summary>
        public int CheckLiveAfterTickCount { get { return _livecheckafterXticks; } set { _livecheckafterXticks = value; } }

        int _livetickdelaymax = 5;
        /// <summary>
        /// if a tick is within this many seconds of current system time on same day, tick stream is considered live and reports can be sent
        /// 
        /// </summary>
        public int CheckLiveMaxDelaySec { get { return _livetickdelaymax; } set { _livetickdelaymax = value; } }

        bool _livecheck = true;
        bool _islive = false;
        /// <summary>
        /// 当前行情系统是否在线
        /// </summary>
        public bool isLive { get { return _islive; } }


        public void Stop()
        {
            // flag to stop
            _continue = false;
            _bw.CancelAsync();
        }

    }
}
