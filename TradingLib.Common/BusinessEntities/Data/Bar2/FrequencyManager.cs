using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using TradingLib.API;



namespace TradingLib.Common
{
    public class FrequencyManager
    {
        private NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 频率发生器列表
        /// </summary>
        List<FrequencyPlugin> frequencyPluginList = new List<FrequencyPlugin>();

        /// <summary>
        /// 合约Map
        /// </summary>
        ConcurrentDictionary<string, Symbol> registedSymbols = new ConcurrentDictionary<string, Symbol>();

        /// <summary>
        /// 合约对应最新更新时间
        /// </summary>
        ConcurrentDictionary<string, DateTime> symbolUpdateTimeMap = new ConcurrentDictionary<string, DateTime>();

        /// <summary>
        /// 合约对应的第一个行情事件
        /// </summary>
        //Dictionary<string, DateTime> symbolFirstTickTime = new Dictionary<string, DateTime>();

        /// <summary>
        /// FreqKey与FreqInfo的Map
        /// </summary>
        Dictionary<FrequencyManager.FreqKey, FrequencyManager.FreqInfo> freqKeyInfoMap = new Dictionary<FreqKey, FreqInfo>();

        /// <summary>
        /// FreqKey字符串键与FreqInfo的Map
        /// </summary>
        Dictionary<string, FrequencyManager.FreqInfo> freqKeyStrInfoMap = new Dictionary<string, FreqInfo>();


        /// <summary>
        /// 合约与该合约的所有FreqInfo的list Map
        /// </summary>
        Dictionary<Symbol, List<FrequencyManager.FreqInfo>> symbolFreqInfoMap = new Dictionary<Symbol, List<FreqInfo>>();

        bool synchronizeBars;

        /// <summary>
        /// Bar构造取价方式
        /// </summary>
        BarConstructionType constructtype = BarConstructionType.Trade;

        string fmName = "Default";
        /// <summary>
        /// FrequencyManager名称
        /// </summary>
        public string Name { get { return fmName; } }
        

        QSEnumDataFeedTypes datafeed = QSEnumDataFeedTypes.DEFAULT;
        /// <summary>
        /// 用于定义该FrequencyManager处理的行情源
        /// 不同的行情源时间会有差异，导致Bar数据生成异常,但是我们假定同一个行情源所有行情都是按时间顺序进行推送的
        /// </summary>
        public QSEnumDataFeedTypes DataFeed { get { return datafeed; } set { datafeed = value; } }

        /// <summary>
        /// 某个频率生成Bar数据
        /// </summary>
        public event Action<FreqKey, SingleBarEventArgs> NewFreqKeyBarEvent;

        public event Action<FreqKey, PartialBarUpdateEventArgs> FreqKeyPartialBarUpdateEvent;

        void OnFreqKeyBar(FreqKey freqkey, SingleBarEventArgs bar)
        {
            if (NewFreqKeyBarEvent != null)
            {
                NewFreqKeyBarEvent(freqkey, bar);
            }
        }

        void OnFreqKeyPartialBar(FreqKey freqkey, PartialBarUpdateEventArgs bar)
        {
            if (FreqKeyPartialBarUpdateEvent != null)
            {
                FreqKeyPartialBarUpdateEvent(freqkey, bar);
            }
        }


        public FrequencyManager(string name, QSEnumDataFeedTypes datafeed,BarConstructionType type = BarConstructionType.Trade, bool synchronizebars = false)
        {
            this.fmName = name;
            //logger = LogManager.GetLogger(this.fmName);
            this.datafeed = datafeed;
            this.constructtype = type;
            this.synchronizeBars = synchronizebars;

            //添加默认Bar数据发生器
            //frequencyPluginList.Add(new TimeFrequency(new BarFrequency(BarInterval.CustomTime, 30)));//1
            //frequencyPluginList.Add(new TimeFrequency(new BarFrequency(BarInterval.CustomTime, 60)));//1
            //frequencyPluginList.Add(new TimeFrequency(new BarFrequency(BarInterval.CustomTime, 180)));//3
            //frequencyPluginList.Add(new TimeFrequency(new BarFrequency(BarInterval.CustomTime, 300)));//5
            //frequencyPluginList.Add(new TimeFrequency(new BarFrequency(BarInterval.CustomTime, 900)));//15
            //frequencyPluginList.Add(new TimeFrequency(new BarFrequency(BarInterval.CustomTime, 1800)));//30
            //frequencyPluginList.Add(new TimeFrequency(new BarFrequency(BarInterval.CustomTime, 3600)));//60
            
        }

        public void RegisterAllBasicFrequency()
        {
            frequencyPluginList.Add(new TimeFrequency(new BarFrequency(BarInterval.CustomTime, 60)));//1
            // frequencyPluginList.Add(new TimeFrequency(new BarFrequency(BarInterval.CustomTime, 180)));//3
            // frequencyPluginList.Add(new TimeFrequency(new BarFrequency(BarInterval.CustomTime, 300)));//5
            // frequencyPluginList.Add(new TimeFrequency(new BarFrequency(BarInterval.CustomTime, 600)));//10
            // frequencyPluginList.Add(new TimeFrequency(new BarFrequency(BarInterval.CustomTime, 900)));//15
            // frequencyPluginList.Add(new TimeFrequency(new BarFrequency(BarInterval.CustomTime, 1800)));//30
            // frequencyPluginList.Add(new TimeFrequency(new BarFrequency(BarInterval.CustomTime, 3600)));//60
            // frequencyPluginList.Add(new TimeFrequency(new BarFrequency(BarInterval.CustomTime, 7200)));//2Hour
        }


        /// <summary>
        /// 获得某个合约第一个Tick时间
        /// 如果没有记录则返回时间为DateTime.MaxValue
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        //public DateTime GetFirstTickTime(Symbol symbol)
        //{
        //    DateTime t = DateTime.MaxValue;
        //    if (symbolFirstTickTime.TryGetValue(symbol.Symbol,out t))
        //    {
        //        return t;
        //    }
        //    return DateTime.MaxValue;
        //}

        #region 注册频率发生器,合约
        /// <summary>
        /// 注册其他频率发生器 用于生成对应的Bar数据
        /// 注册新的FrequencyPlugin时 需要遍历当前所有合约为每个合约生成对应的数据
        /// </summary>
        /// <param name="settings"></param>
        public void RegisterFrequencies(FrequencyPlugin settings)
        {
            foreach (var symbol in registedSymbols.Values)
            {
                FrequencyManager.FreqKey freqKey = new FrequencyManager.FreqKey(settings.Clone(), symbol);
                this.RegisterFreKey(freqKey);
            }
        }


        /// <summary>
        /// 注册某个合约
        /// </summary>
        /// <param name="symbol"></param>
        public void RegisterSymbol(Symbol symbol)
        {
            if (registedSymbols.Keys.Contains(symbol.Symbol))
            {
                logger.Warn("Symbol:{0} already registed".Put(symbol.Symbol));
                return;
            }

            registedSymbols.TryAdd(symbol.Symbol, symbol);
            symbolUpdateTimeMap.TryAdd(symbol.Symbol, DateTime.MinValue);
            //symbolCorrectBarTimeMap.Add(symbol.Symbol, DateTime.MinValue);//默认时间为最小时间

            //遍历所有发生器列表生成key并注册
            foreach (var t in frequencyPluginList)
            {
                FrequencyManager.FreqKey freqKey = new FrequencyManager.FreqKey(t.Clone(), symbol);
                this.RegisterFreKey(freqKey);
            }
        }

        /// <summary>
        /// 注册一个freqkey
        /// </summary>
        /// <param name="freqkey"></param>
        private void RegisterFreKey(FrequencyManager.FreqKey freqkey)
        {
#if DEBUG
            logger.Debug("RegisterFreKey:" + freqkey.ToString());
#endif
            if (this.freqKeyInfoMap.Keys.Contains(freqkey))
            {
                logger.Warn(string.Format("FreqKey:{0} already registed", freqkey));
                return;
            }

            //添加对应FreqKey的FreqInfo数据
            FrequencyManager.FreqInfo freqInfo = new FrequencyManager.FreqInfo(freqkey, this.synchronizeBars, this);
            freqInfo.Generator.Initialize(freqkey.Symbol, constructtype);

            //1.更新FreqKey到FreqInfo映射
            this.freqKeyInfoMap[freqkey] = freqInfo;
            this.freqKeyStrInfoMap[freqkey.ToFreqKey()] = freqInfo;

            //2.Symbol到FreqInfo List映射
            List<FrequencyManager.FreqInfo> list;
            if (!this.symbolFreqInfoMap.TryGetValue(freqkey.Symbol, out list))
            {
                list = new List<FrequencyManager.FreqInfo>();
                this.symbolFreqInfoMap[freqkey.Symbol] = list;
            }
            list.Add(freqInfo);
        }
        #endregion





        #region 获得频率数据Frequency
        /// <summary>
        /// 获得某个合约 某个频率发生器 的Frequency
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public Frequency GetFrequency(Symbol symbol, FrequencyPlugin settings)
        {
            if (symbol == null)
            {
                throw new System.ArgumentNullException("symbol");
            }
            if (settings == null)
            {
                throw new System.ArgumentNullException("settings");
            }
            settings = settings.Clone();
            FrequencyManager.FreqKey freqKey = new FrequencyManager.FreqKey(settings, symbol);
            return this.GetFrequency(freqKey);
        }

        /// <summary>
        /// 获得某个FreqKey的Frequency数据
        /// </summary>
        /// <param name="frekey"></param>
        /// <returns></returns>
        private Frequency GetFrequency(FrequencyManager.FreqKey freqkey)
        {
            FrequencyManager.FreqInfo target = null;
            if (freqKeyInfoMap.TryGetValue(freqkey, out target))
            {
                return target.Frequency;
            }
            return null;
        }

        /// <summary>
        /// 通过Symbol BarFrequency获得Bar数据集
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="freq"></param>
        /// <returns></returns>
        public Frequency GetFrequency(Symbol symbol, BarFrequency freq)
        {
            string key = string.Format("{0}-{1}-{2}", symbol.SecurityFamily.Exchange.EXCode, symbol.Symbol, freq.ToUniqueId());
            FrequencyManager.FreqInfo target = null;
            if (freqKeyStrInfoMap.TryGetValue(key, out target))
            {
                return target.Frequency;
            }
            return null;
        }

        /// <summary>
        /// 获取某个合约所有周期数据信息
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public IEnumerable<Frequency> GetFrequency(Symbol symbol)
        {
            List<FreqInfo> target = null;
            if (symbolFreqInfoMap.TryGetValue(symbol, out target))
            {
                return target.Select(info=>info.Frequency);
            }
            return new List<Frequency>();
        }

        #endregion




        /// <summary>
        /// 注册频率转换
        /// </summary>
        /// <param name="sourceFrequency"></param>
        /// <param name="destFrequency"></param>
        //public void RegisterFrequencyConversion(Frequency sourceFrequency, Frequency destFrequency)
        //{
        //    if (!sourceFrequency.DestFrequencyConversion.ContainsKey(destFrequency))
        //    {
        //        sourceFrequency.DestFrequencyConversion[destFrequency] = new QList<DateTime>();
        //    }
        //}

        /// <summary>
        /// Converts the lookback index from one frequency to another.
        /// 将源Frequency的回溯值转换成目标Frequency的回溯值
        /// </summary>
        /// <param name="sourceLookBack">Lookback index.</param>
        /// <param name="sourceFrequency">Source frequency</param>
        /// <param name="destFrequency">Destination frequency.</param>
        /// <returns>index of destination frequency bar lookback</returns>
        //public int ConvertLookBack(int sourceLookBack, Frequency sourceFrequency, Frequency destFrequency)
        //{
        //    if (!sourceFrequency.DestFrequencyConversion.ContainsKey(destFrequency))
        //    {
        //        string message = string.Concat(new object[]
        //        {
        //            "Cross-Frequency conversion not set up from ",
        //            sourceFrequency.Symbol,
        //            " ",
        //            sourceFrequency.FrequencySettings.ToString(),
        //            " to ",
        //            destFrequency.Symbol,
        //            " ",
        //            destFrequency.FrequencySettings.ToString(),
        //            ".  Call FrequencyManager.RegisterFrequencyConversion() to enable this."
        //        });
        //        throw new Exception(message);
        //    }
        //    if (sourceLookBack >= sourceFrequency.Bars.Count)
        //    {
        //        return -1;
        //    }
        //    QList<DateTime> rList = sourceFrequency.DestFrequencyConversion[destFrequency];
        //    if (sourceLookBack >= rList.Count)
        //    {
        //        return -1;
        //    }
        //    DateTime barStartTime = rList.LookBack(sourceLookBack);
        //    return destFrequency.LookupStartDate(barStartTime);
        //}


        /// <summary>
        /// 获得某个合约的所有FreqInfo
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private IEnumerable<FrequencyManager.FreqInfo> GetFreqInfosForSymbol(Symbol symbol)
        {
            List<FrequencyManager.FreqInfo> target = null;
            if (symbolFreqInfoMap.TryGetValue(symbol, out target))
            {
                return target;
            }
            return new List<FrequencyManager.FreqInfo>();

        }

        /// <summary>
        /// 获得某个FrequencyBase对应的所有FreqInfo
        /// </summary>
        /// <param name="fb"></param>
        /// <returns></returns>
        private IEnumerable<FrequencyManager.FreqInfo> GetFreqInfosForFrequencyBase(FrequencyPlugin fb)
        {
            List<FrequencyManager.FreqInfo> list = new List<FrequencyManager.FreqInfo>();
            foreach (var pair in freqKeyInfoMap)
            {
                if (pair.Key.Settings.Equals(fb))
                {
                    list.Add(pair.Value);
                }
            }
            return list;
        }



       
        /// <summary>
        /// 获得某个FreqKey对应的FreqInfo数据集
        /// </summary>
        /// <param name="freqkey"></param>
        /// <returns></returns>
        private FrequencyManager.FreqInfo GetFreqInfoForFreqKey(FrequencyManager.FreqKey freqkey)
        {
            
            FrequencyManager.FreqInfo freqInfo;
            if (!this.freqKeyInfoMap.TryGetValue(freqkey, out freqInfo))
            {
                RegisterFreKey(freqkey);
            }
            return this.freqKeyInfoMap[freqkey];
        }


        /// <summary>
        /// 返回所有FreqInfo
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<FrequencyManager.FreqInfo> GetAllFrequencies()
        {
            return this.freqKeyInfoMap.Values;
        }


        //public DateTime CurrentTime { get { return _currentTime; } }


        public void ProcessBar(Bar bar)
        {

        }



        public void Clear(Symbol symbol)
        {
            symbolUpdateTimeMap[symbol.Symbol] = DateTime.MinValue;

            foreach (var freqinfo in GetFreqInfosForSymbol(symbol))
            {
                freqinfo.Clear();
            }


        }
        //public static Profiler pf = new Profiler();
        /// <summary>
        /// 处理行情数据
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="tick"></param>
        public void ProcessTick(Tick tick)
        {
            //if (tick.Symbol != "CLX6") return;

            //pf.EnterSection("PRECHECK  ");
            //非需要处理的行情源
            //if (this.DataFeed != QSEnumDataFeedTypes.DEFAULT && this.datafeed != tick.DataFeed ) return;
            //查找合约
            Symbol symbol = null;
            if (!registedSymbols.TryGetValue(tick.Symbol, out symbol)) return;

            //logger.Info("process tick called,symbol:" + symbol.Symbol);
            DateTime ticktime = tick.DateTime();


            Tick ttick = new TickImpl();
            //pf.LeaveSection();

            //如果时间大于Frequency的当前时间 则需要检查是否有PendingBars需要发送 时间相等则不用发送
            if (ticktime >= symbolUpdateTimeMap[symbol.Symbol])//Tick数据必须按时间顺序进入 如果出现时间错乱则处理逻辑会被打乱 比如 产生一个时间很大的Tick 结果后面正常的Tick数据无法被有效处理
            {
                //pf.EnterSection("TIMECHECK  ");
                IEnumerable<FrequencyManager.FreqInfo> list = this.GetFreqInfosForSymbol(symbol);
                #region A.执行该合约所有频率数据的时间检查 如果越过了下次更新时间 则处理TimeTick,并生成Bar数据并放到eventHolder,清空待发送Bar,清空对应数据集的PartialItem数据
                FrequencyNewBarEventHolder eventHolder = new FrequencyNewBarEventHolder();
                foreach (var freqinfo in list)
                {
                    //如果当前时间大于该频率对应的下次更新时间,则调用该频处理TimeTick Close一个Bar //历史恢复数据时候 通过截取Tick在末尾增加一个时间Tick进行处理
                    if (ticktime >= freqinfo.Generator.NextTimeUpdateNeeded)
                    {
                        //pf.EnterSection("TIMECHECK1");
                        this.FreqInfoProcessTick(ttick, freqinfo);
                        //pf.LeaveSection();
                    }
                    //if (tick.UpdateType == "E")
                    //{
                    //    //FreqInfo处理MarketEvent 类型的Tick用于 Close一个Bar
                    //    FreqInfoProcessTick(tick, freqinfo);
                    //}
                    //如果FreqInfo有待发送的Bar数据 放入eventholder 在处理时间Tick后 有Bar结束 则清空freqInfo的pendingBar同时清空Frequency的partialItem
                    if (freqinfo.PendingBarEvents.Count > 0)
                    {
                        //pf.EnterSection("TIMECHECK2");
                        foreach (SingleBarEventArgs bar in freqinfo.PendingBarEvents)
                        {
                            eventHolder.AddEvent(freqinfo.FreqKey, bar);
#if DEBUG
                            logger.Debug(string.Format("FreqInfo[{0}] Cached Bar:{1}", freqinfo.FreqKey, bar.Bar));
#endif
                        }
#if DEBUG
                        logger.Debug(string.Format("FreqInfo[{0}] Clear PendingBar and PartialBar",freqinfo.FreqKey));
#endif
                        //清空FreqInfo待发送Bar 以及 PartialBar
                        freqinfo.ClearPendingBars();

                        //清空PartialItem同时添加到freqKeyNoPartialBar HashSet中 
                        if (freqinfo.FreqKey.Settings.IsTimeBased)
                        {
                            freqinfo.Frequency.WriteableBars.ClearPartialItem();//清空PartialItem
                            //this.freqKeyNoPartialBar.Add(freqinfo.FreqKey);//将对应的FreqKey添加到发送完毕的HashSet
                        }
                        //pf.LeaveSection();
                    }

                }
                #endregion
                //pf.LeaveSection();

                //pf.EnterSection("SENDBAR    ");
                #region B.如果有待触发Bar数据 则更新Frequency的Bar集合并对外发送Bar数据
                if (eventHolder.EventList.Count > 0)
                {
#if DEBUG
                    //logger.Info(string.Format("Update frequency's collection, send out bar event"));
#endif
                    foreach (FrequencyNewBarEventArgs frequencyEvent in eventHolder.EventList)
                    {
                        foreach (KeyValuePair<FrequencyManager.FreqKey, SingleBarEventArgs> pair in frequencyEvent.FrequencyEvents)
                        {
                            FreqInfo info = this.freqKeyInfoMap[pair.Key];
                            if (info == null)
                            {
                                logger.Warn("FreqKey:{0} havs no FreqInfo avabile".Put(pair.Key));
                            }
#if DEBUG
                            logger.Debug("UpdateBarCollection:" + pair.Value.Bar.ToString() + " QTY:" + info.Frequency.WriteableBars.Count);
#endif
                            //通过FreqKey找到对应的FreqInfo同时更新BarCollection 该操作将会在数据集中添加Bar
                            info.UpdateBarCollection(pair.Value.Bar, pair.Value.BarEndTime);
                            info.SendNewBar(pair.Value);
#if DEBUG
                            logger.Debug(string.Format("FreqInfo[{0}] Emit Bar:{1}", info.FreqKey, pair.Value.Bar));
#endif
                            this.OnFreqKeyBar(info.FreqKey, pair.Value);
                            ////第一个Bar有可能是中途开始的因此数据未必正确 需要记录第二个Bar的更新时间
                            //if(info.Frequency.Bars.Count == 2)
                            //{
                            //    symbolCorrectBarTimeMap[pair.Value.Symbol.Symbol] = pair.Value.Bar.StartTime;
                            //}
                            //this._currentTime = pair.Value.BarEndTime;//更新当前时间为BarEndTime
                        }

                        /** 频率转换
                        //获得FreqInfo对应的Frequency对象 更新Frequency对象的DestFrequencyConversion
                        foreach (KeyValuePair<FrequencyManager.FreqKey, SingleBarEventArgs> pair in frequencyEvent.FrequencyEvents)
                        {
                            Frequency frequency = this.freqKeyInfoMap[pair.Key].Frequency;
                            //目的Bar转换
                            foreach (KeyValuePair<Frequency, QList<DateTime>> conversion in frequency.DestFrequencyConversion)
                            {
                                Frequency key = conversion.Key;
                                QList<DateTime> value = conversion.Value;
                                //设定最大回溯值
                                value.MaxLookBack = frequency.Bars.MaxLookBack;
                                //如果Frequency的Bar数量大于0 则将该Bar的StartTime添加到QList<DateTime>中
                                if (key.Bars.Count > 0)
                                {
                                    value.Add(key.Bars.Current.BarStartTime);
                                }
                            }
                        }
                         * **/
                    }
                }
                #endregion
                //pf.LeaveSection();

                //pf.EnterSection("PROCESSTICK");
                #region C.FreqInfo处理Tick并更新PartialItem
                if (tick.UpdateType == "X")//成交类型的Tick才在最后处理，用于生成新的Bar 时间或事件类的提前处理
                {
                    //遍历所有freqinfo 处理Tick数据并更新Frequency的PartialItem
                    foreach (var freqinfo in list)
                    {
                        //pf.EnterSection("PT01");
                        //FreqInfo处理tick数据
                        FreqInfoProcessTick(tick, freqinfo);
                        //pf.LeaveSection();

                        //pf.EnterSection("PT02");
                        //FreqInfo处理TimeTick数据
                        FreqInfoProcessTimeTick(ttick, freqinfo);
                        //pf.LeaveSection();

                        if (freqinfo.Frequency.WriteableBars.HasPartialItem)
                        {
                            PartialBarUpdateEventArgs arg = new PartialBarUpdateEventArgs(freqinfo.FreqKey.Symbol, freqinfo.Frequency.WriteableBars.PartialItem as BarImpl);
                            OnFreqKeyPartialBar(freqinfo.FreqKey, arg);
                        }
                    }
                }
                #endregion

                //更新该合约的最近Tick更新时间
                symbolUpdateTimeMap[symbol.Symbol] = ticktime;
                //pf.LeaveSection();

            }
            else
            {
                logger.Warn(string.Format("Out of order tick. Received tick for symbol {0} with time {1} when previous tick time was {2}", symbol.Symbol, ticktime, symbolUpdateTimeMap[symbol.Symbol]));
            }
        }


        /// <summary>
        /// Frequency的Bar生成器处理Tick
        /// </summary>
        /// <param name="tick"></param>
        /// <param name="info"></param>
        void FreqInfoProcessTick(Tick tick, FreqInfo info)
        {
            info.Generator.ProcessTick(tick);
        }

        /// <summary>
        /// 更新Frequency的PartialItem为FreqInfo的PendingPartialBar
        /// FreqInfo的PendingPartilaBar由FrequecyGenerator处理Tick数据时 对外触发NewTickEventArgs附带了最新的Bar数据
        /// </summary>
        /// <param name="freqInfo"></param>
        /// <param name="datetime"></param>
        private void FreqInfoProcessTimeTick(Tick tick,FrequencyManager.FreqInfo freqInfo)
        {
            if (tick.Type != EnumTickType.TIME) return;
            //如果FreqInfo.PendingPartialBar为空并且Frequency.WriteableBars没有PartialItem则用调用FreqInfo处理TimeTick用于生成一条PendingPartialBar
            if (freqInfo.PendingPartialBar == null && !freqInfo.Frequency.WriteableBars.HasPartialItem)//当
            {
                //处理对应的时间Tick
                this.FreqInfoProcessTick(tick, freqInfo);
            }
            //freqInfo.PendingPartialBar 由freqgenerator实时行情驱动并更新
            //如果FreqInfo的PendingPartialBar不为空则将该PartialBar复制到WriteableBars的PartialItem 同时将freqInfo.PendingPartialBar置空 表面已经将最新的Bar更新到freqInfo.Frequency.WriteableBars.PartialItem
            if (freqInfo.PendingPartialBar != null)
            {
                freqInfo.Frequency.WriteableBars.PartialItem = freqInfo.PendingPartialBar;
            }
            freqInfo.PendingPartialBar = null;
        }

        /// <summary>
        /// 某个FreqInfo触发Tick事件
        /// </summary>
        /// <param name="info"></param>
        /// <param name="tick"></param>
        internal void OnFrequencyTick(FreqInfo info, Tick tick)
        {

        }



        private void SynchronizeBars(FrequencyManager.FrequencyNewBarEventHolder eventlist)
        {
            logger.Debug("SynchronizeBars");
            //FreqKey-Bar映射
            Dictionary<FrequencyManager.FreqKey, Bar> dictionary = new Dictionary<FrequencyManager.FreqKey, Bar>();
            foreach (FrequencyNewBarEventArgs frequencyEvent in eventlist.EventList)
            {
                Dictionary<FrequencyPlugin, SingleBarEventArgs> dictionary2 = new Dictionary<FrequencyPlugin, SingleBarEventArgs>();
                foreach (KeyValuePair<FrequencyManager.FreqKey, SingleBarEventArgs> current2 in frequencyEvent.FrequencyEvents)
                {
                    if (this.freqKeyInfoMap[current2.Key].FreqKey.Settings.IsTimeBased)
                    {
                        dictionary2[current2.Key.Settings] = current2.Value;
                        dictionary[current2.Key] = current2.Value.Bar;
                    }
                }


                foreach (KeyValuePair<FrequencyPlugin, SingleBarEventArgs> current3 in dictionary2)
                {
                    foreach (Symbol current4 in this.registedSymbols.Values)
                    {
                        //交叉FrequencyBase * Symbol 获得对应的freqkey
                        FrequencyManager.FreqKey freqKey = new FrequencyManager.FreqKey(current3.Key, current4);
                        //如果FrequencyEvents不包含对应的Key 则添加对应的Bar事件，Bar标识为EmptyBar
                        if (!frequencyEvent.FrequencyEvents.ContainsKey(freqKey))
                        {
                            Bar barData = null;
                            //如果有FreqKey对应的Bar设定当前的Bar
                            if (dictionary.ContainsKey(freqKey))
                            {
                                barData = dictionary[freqKey];
                            }
                            else
                            {
                                //不存在则找到该FreqKey对应的Frequency 来获得当前Bar
                                Frequency frequency = this.GetFrequency(freqKey);
                                if (frequency.Bars.Count > 0)
                                {
                                    barData = frequency.Bars.Current;
                                }
                            }
                            if (barData != null)
                            {
                                Bar barData2 = new BarImpl(current3.Value.Bar.Symbol, freqKey.Settings.BarFrequency, current3.Value.Bar.EndTime);
                                barData2.Open = (barData2.Close = (barData2.High = (barData2.Low = barData.Close)));
                                barData2.Bid = barData.Bid;
                                barData2.Ask = barData.Ask;
                                SingleBarEventArgs value = new SingleBarEventArgs(freqKey.Symbol, barData2, current3.Value.BarEndTime, false);
                                frequencyEvent.FrequencyEvents.Add(freqKey, value);
                            }
                        }
                    }
                }
            }
        }


        #region
        private class FrequencyNewBarEventHolder
        {
            public List<FrequencyNewBarEventArgs> EventList { get; set; }

            public FrequencyNewBarEventHolder()
            {
                this.EventList = new List<FrequencyNewBarEventArgs>();
            }

            public void AddEvent(FreqKey freqKey, SingleBarEventArgs args)
            {
                bool flag = false;

                foreach (FrequencyNewBarEventArgs current in this.EventList)//遍历所有列表如果列表中的都包含了FreqKey则需要新增加一个条目(多个Bar累积到一起发送就会造成这样的情况)
                {
                    if (!current.FrequencyEvents.ContainsKey(freqKey))
                    {
                        current.FrequencyEvents.Add(freqKey, args);
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    FrequencyNewBarEventArgs item = new FrequencyNewBarEventArgs();
                    item.FrequencyEvents.Add(freqKey, args);
                    this.EventList.Add(item);
                }
            }
        }
        #endregion

        #region FreqInfo定义
        internal class FreqInfo
        {
            private FrequencyManager _manager;

            private FreqKey _key;
            /// <summary>
            /// FreqKey
            /// </summary>
            public FreqKey FreqKey { get { return _key; } }


            private IFrequencyGenerator _freqgenerator;
            /// <summary>
            /// Frequency数据发生器
            /// </summary>
            public IFrequencyGenerator Generator { get { return _freqgenerator; } }

            private Frequency _frequency;
            /// <summary>
            /// Bar数据集
            /// </summary>
            public Frequency Frequency { get { return _frequency; } }


            private List<SingleBarEventArgs> _pendingBarEvents;
            /// <summary>
            /// 待发送Bar数据
            /// </summary>
            public List<SingleBarEventArgs> PendingBarEvents { get { return _pendingBarEvents; } }

            private Bar _pendingPartialBar;
            /// <summary>
            /// 当前实时Bar数据
            /// </summary>
            public Bar PendingPartialBar { get { return _pendingPartialBar; } set { _pendingPartialBar = value; } }


            /// <summary>
            /// 清空FreqInfo中数据结构的数据
            /// </summary>
            public void Clear()
            {
                //底层frequency数据集清空
                _frequency.WriteableBars.Clear();
                _frequency.WriteableBars.ClearPartialItem();

                //FreqInfo数据集清空
                this.ClearPendingBars();

                
                //重新生成generator
                this._freqgenerator = _key.Settings.CreateFrequencyGenerator();
                this._freqgenerator.NewBarEvent += new Action<SingleBarEventArgs>(_freqgenerator_NewBarEvent);
                this._freqgenerator.NewTickEvent += new Action<NewTickEventArgs>(_freqgenerator_NewTickEvent);

                this._freqgenerator.Initialize(this.FreqKey.Symbol,BarConstructionType.Trade);
            }

            private NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
            public FreqInfo(FreqKey key, bool synchronizeBars, FrequencyManager manager)
            {
                this._key = key;
                this._manager = manager;

                

                //生成对应的Frequency数据结构
                this._frequency = new Frequency(_key, synchronizeBars);
                this._pendingBarEvents = new List<SingleBarEventArgs>();

                //生成Bar数据生成器 FrequencyPlugin不同,可以按不同的逻辑生成Bar数据
                this._freqgenerator = _key.Settings.CreateFrequencyGenerator();
                //绑定freqgenerator事件 此处Bar生成事件 只是将Bar添加到待发送列表,不实际出发 需要通过SendNewBar进行发送
                this._freqgenerator.NewBarEvent += new Action<SingleBarEventArgs>(_freqgenerator_NewBarEvent);
                this._freqgenerator.NewTickEvent += new Action<NewTickEventArgs>(_freqgenerator_NewTickEvent);
            }

            public override string ToString()
            {
                return string.Format("FreqInfo for Key:{0}", this._key);
            }
            /// <summary>
            /// 更新Frequency的Bar数据
            /// QList<>Add操作会清空PartialItem项
            /// </summary>
            /// <param name="bar"></param>
            /// <param name="barEndTime"></param>
            public void UpdateBarCollection(Bar bar, DateTime barEndTime)
            {
                
                this.Frequency.WriteableBars.Add(bar);
                this.Frequency.CurrentBarEndTime = barEndTime;
#if DEBUG
                //logger.Debug("UpdateBarCollection:" + bar.ToString() + " QTY:" + this.Frequency.WriteableBars.Count);
#endif
            }

            /// <summary>
            /// 清空待发送Bar数据
            /// </summary>
            public void ClearPendingBars()
            {
#if DEBUG
                //logger.Info("ClearPendingBars");
#endif
                this._pendingBarEvents.Clear();
                this._pendingPartialBar = null;
            }

            /// <summary>
            /// 发送Bar数据 通过Frequency的事件对外触发Bar数据
            /// </summary>
            /// <param name="args"></param>
            public void SendNewBar(SingleBarEventArgs args)
            {
#if DEBUG
                //logger.Info("SendNewBar:" + args.Bar.ToString());
#endif
                this.Frequency.OnNewBar(args);
            }


            /// <summary>
            /// 处理Generator的Tick事件
            /// 当处理完毕Tick后 该事件会携带当前的PartialBar 设定PartialBar
            /// </summary>
            /// <param name="obj"></param>
            void _freqgenerator_NewTickEvent(NewTickEventArgs obj)
            {
                if (obj.Symbol.Symbol != this._key.Symbol.Symbol)
                {
                    return;
                }
                this._pendingPartialBar = obj.PartialBar;//设置当前最新的PartialBar
                this._manager.OnFrequencyTick(this, obj.Tick);
            }

            /// <summary>
            /// 处理Generator生成的Bar事件
            /// </summary>
            /// <param name="obj"></param>
            void _freqgenerator_NewBarEvent(SingleBarEventArgs obj)
            {
                //合约不一致直接返回
                if (obj.Symbol.Symbol != this._key.Symbol.Symbol)
                {
                    return;
                }
                bool flag = false;
                //Bar不为空
                if (!obj.Bar.EmptyBar)
                {
                    flag = true;
                }
                //或者SynchronizeBars为True并且Frequnecy的Bar list有数据
                else if (this.Frequency.SynchronizeBars && (this.Frequency.WriteableBars.Count > 0 || this.PendingBarEvents.Count > 0))
                {
                    flag = true;
                }
                if (flag)
                {
                    this.PendingBarEvents.Add(obj);
                }
            }
        }
        #endregion

        #region FreqKey定义
        /// <summary>
        /// 频率数据Key
        /// Symbol-FrequencyPlugin组成了一个唯一键
        /// 用什么样的Bar生成器为合约Symbol生成Bar数据
        /// </summary>
        public class FreqKey
        {
            public FrequencyPlugin Settings { get; set; }

            public Symbol Symbol { get; set; }

            public FreqKey(FrequencyPlugin setting, Symbol symbol)
            {
                this.Settings = setting;
                this.Symbol = symbol;
            }

            public override bool Equals(object obj)
            {
                FreqKey key = obj as FreqKey;
                return key != null && key.Symbol.Symbol == this.Symbol.Symbol && key.Settings.Equals(this.Settings);
            }

            public override int GetHashCode()
            {
                int hashCode = this.Settings.GetHashCode();
                int num = 0;
                if (this.Symbol != null)
                {
                    num = this.Symbol.GetHashCode();
                }
                return hashCode ^ num;
            }

            /// <summary>
            /// 获得FreqKey
            /// Exchange-symbol-interval-intervaltype
            /// </summary>
            /// <returns></returns>
            public string ToFreqKey()
            {
                return string.Format("{0}-{1}-{2}", this.Symbol.SecurityFamily.Exchange.EXCode, this.Symbol.Symbol, this.Settings.BarFrequency.ToUniqueId());
            }
            public override string ToString()
            {
                return string.Format("Symbol:{0} - Freq:{1}", this.Symbol.Symbol, this.Settings);
            }
        }

        #endregion


    }
}
