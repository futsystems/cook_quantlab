//using System;
//using System.Collections.Generic;
//using TradingLib.API;

//namespace TradingLib.Common
//{
//    /// <summary>
//    /// allows automatic sending of profit targets and stop orders for a set of positions.
//    /// automatically manages partial fills.
//    /// 自动发送止盈 止损委托
//    /// </summary>
//    public class OffsetTracker : GenericTracker<OffsetInfo>, GotTickIndicator, SendOrderIndicator, SendCancelIndicator, GotFillIndicator, GotCancelIndicator, GotPositionIndicator
//    {
//        public void GotTick(Tick k) { newTick(k); }
//        public event DebugDelegate SendDebug;
//        public event HitOffsetDelegate HitOffset;
//        void debug(string msg) { if (SendDebug != null) SendDebug(msg); }
//        OffsetInfo _default = new OffsetInfo();
//        string[] _ignore = new string[0];
//        /// <summary>
//        /// default offset used by the tracker, in the event no custom offset is set. eg ot["IBM"] = new OffsetInfo();
//        /// </summary>
//        public OffsetInfo DefaultOffset { get { return new OffsetInfo(_default); } set { value.ProfitId = 0; value.StopId = 0; _default = value; } }
       
//        bool _ignoredefault = false;
//        /// <summary>
//        /// 如果某个合约没有设定offset,那么默认是不监监控该合约 
//        /// ignore symbols by default.   if true... a symbol has no custom offset defined will be ignored (regardless of ignore list).  the default is false.
//        /// </summary>
//        public bool IgnoreDefault { get { return _ignoredefault; } set { _ignoredefault = value; } }
//        /// <summary>
//        /// always ignore these symbols.   this list is only in affect when IgnoreDefault is false.
//        /// </summary>
//        public string[] IgnoreSyms { get { return _ignore; } set { _ignore = value; } }
//        bool _hasevents = false;

//        //发送委托与取消
//        public event OrderDelegate SendOrderEvent;
//        public event LongDelegate SendCancelEvent;
//        PositionTracker _pt = new PositionTracker();
//        /// <summary>
//        /// a position tracker you can reuse 
//        /// </summary>
//        public PositionTracker PositionTracker { get { return _pt; } set { _pt = value; } }
//        public OffsetTracker() { }
//        IdTracker _ids = new IdTracker();
//        /// <summary>
//        /// id tracker used by offsettracker, you can reuse in other apps you use OT.
//        /// </summary>
//        public IdTracker Ids { get { return _ids; } set { _ids = value; } }
//        public OffsetTracker(long initialid) : this(new IdTracker(initialid)) { }
//        public OffsetTracker(IdTracker tracker) : base("OFFSET")
//        {
//            _ids = tracker;
//        }

//        int _debdecimals = 2;
//        /// <summary>
//        /// number of decimal places in SendDebug events
//        /// (defaults to 2, set to 5 for forex)
//        /// </summary>
//        public int DebugDecimals { get { return _debdecimals; } set { _debdecimals = value; } }

//        /// <summary>
//        /// clear single custom offset
//        /// </summary>
//        /// <param name="sym"></param>
//        public void ClearCustom(string sym) { this[sym] = IgnoreDefault ? OffsetInfo.DISABLEOFFSET() : DefaultOffset; }

//        object _lock = new object();

//        /// <summary>
//        /// 更新某个合约
//        /// </summary>
//        /// <param name="sym"></param>
//        void doupdate(string sym)
//        {
//            // is update ignored? 检查是否忽略
//            if (IgnoreUpdate(sym)) return;
//            // wait till next tick if we send cancels 取消委托后 等待下一个tick
//            bool sentcancel = false;
//            // get our offset values
//            OffsetInfo off = GetOffset(sym);
//            // get position
//            Position p = new PositionImpl(_pt[sym]);
//            // if we're flat, nothign to do
//            if (p.isFlat) return;
//            // see whats current 检测当前是否需要发送了对应的委托,isprofitcurrent代表当前有止盈委托在监控
//            bool cprofit = off.isProfitCurrent(p);
//            bool cstop = off.isStopCurrent(p);

//            // if not current, mark for update 如果没有设置 则标记为需要更新
//            bool updateprofit = !cprofit;
//            bool updatestop = !cstop;

//            // if we're up to date then quit
//            if (!updatestop && !updateprofit) return; //如果均不要更新则我们直接返回
//            // see if we have stop update 如果我们需要发送止损委托 并且  取消原有委托CancelOnce是否在一个tick中只执行取消,等待下一个tick执行其他动作
//            if ((updatestop && off.hasStop && !CancelOnce) 
//                || (updatestop && off.hasStop && CancelOnce && !off.StopcancelPending))
//            {
//                // notify
//                if (!off.StopcancelPending)
//                    debug(string.Format("attempting stop cancel: {0} {1}", sym, off.StopId));
//                // cancel existing stops
//                cancel(off.StopId);
//                // mark cancel pending
//                off.StopcancelPending = true;
//                // mark as sent
//                sentcancel |= true;
//            }
//            // see if we have profit update
//            if ((updateprofit && off.hasProfit && AllowSimulatenousCancels) ||
//                (updateprofit && off.hasProfit && AllowSimulatenousCancels && !sentcancel))
//            {
//                if (!CancelOnce || (CancelOnce && !off.ProfitcancelPending))
//                {
//                    // notify
//                    if (!off.ProfitcancelPending)
//                        debug(string.Format("attempting profit cancel: {0} {1}", sym, off.ProfitId));
//                    // cancel existing profits
//                    cancel(off.ProfitId);
//                    // mark cancel pending
//                    off.ProfitcancelPending = true;
//                    // mark as sent
//                    sentcancel |= true;
//                }
//            }
             
//            // wait till next tick if we sent cancel如果我们已经发送的取消 如果需要等待下一个tick则等待下一个tick
//            if (sentcancel && WaitAfterCancel)
//                return;

//            bool sentorder = false;
//            // send stop first 发送对应的委托
//            if (!off.hasStop)
//            {
//                // since we have no stop, it's cancel can't be pending
//                off.StopcancelPending = false;
//                // get new stop
//                Order stop = Calc.PositionStop(p, off.StopDist, off.StopPercent, off.NormalizeSize, off.MinimumLotSize);
//                // mark size
//                off.SentStopSize = stop.size;
//                // if it's valid, send and track
//                if (stop.isValid)
//                {
//                    stop.id = Ids.AssignId;
//                    off.StopId = stop.id;
//                    SendOrderEvent(stop);
//                    // notify
//                    debug(string.Format("sent new stop: {0} {1}", stop.id,stop.GetOrderInfo()));
//                    sentorder = true;
//                }
//                else if (_verbdebug)
//                {
//                    debug(sym + " invalid stop: " + stop.GetOrderInfo());
//                }

//            }

//            if ((!off.hasProfit && AllowSimulatenousOrders) || (!off.hasProfit && !AllowSimulatenousOrders && !sentorder))
//            {
//                // since we have no stop, it's cancel can't be pending
//                off.ProfitcancelPending = false;
//                // get new profit
//                Order profit = Calc.PositionProfit(p, off.ProfitDist, off.ProfitPercent, off.NormalizeSize, off.MinimumLotSize);
//                // mark size
//                off.SentProfitSize = profit.size;
//                // if it's valid, send and track it
//                if (profit.isValid)
//                {
//                    profit.id = Ids.AssignId;
//                    off.ProfitId = profit.id;
//                    SendOrderEvent(profit);
//                    // notify
//                    debug(string.Format("sent new profit: {0} {1}", profit.id, profit.GetOrderInfo()));
//                    sentorder = true;
//                }
//                else if (_verbdebug)
//                {
//                    debug(sym + " invalid profit: " + profit.GetOrderInfo());
//                }
//            }
//            // make sure new offset info is reflected
//            SetOffset(sym, off);

//        }

//        bool _cancelonce = false;
//        /// <summary>
//        /// only cancel an offset once per update
//        /// </summary>
//        public bool CancelOnce { get { return _cancelonce; } set { _cancelonce = value; } }

//        bool _waitaftercancel = true;
//        /// <summary>
//        /// wait till next tick after sending cancel orders
//        /// </summary>
//        public bool WaitAfterCancel { get { return _waitaftercancel; } set { _waitaftercancel = value; } }

//        bool _sendsametime = true;
//        /// <summary>
//        /// allow stops and profit offsets to be sent at same time
//        /// </summary>
//        public bool AllowSimulatenousOrders { get { return _sendsametime; } set { _sendsametime = value; } }

//        bool _cancelsametime = true;
//        /// <summary>
//        /// allow stop and profit cancels to be sent at same time
//        /// </summary>
//        public bool AllowSimulatenousCancels { get { return _cancelsametime; } set { _cancelsametime = value; } }

//        bool hascustom(string symbol)
//        {
//            return getindex(symbol) >= 0;
//        }

//        void cancel(OffsetInfo offset) 
//        {

//            bool hit = false;
//            string ids = string.Empty;
//            if (offset.hasProfit)
//            {
//                hit |= true;
//                ids += offset.ProfitId + " ";
//                cancel(offset.ProfitId);
//            }
//            if (offset.hasStop)
//            {
//                hit |= true;
//                ids += offset.StopId + " ";
//                cancel(offset.StopId);
//            }
//            if (hit)
//                debug("canceling offsets: " + ids);
            
//        }
//        void cancel(long id) { if (id != 0) SendCancelEvent(id); }
//        /// <summary>
//        /// cancels all offsets (profit+stops) for given side
//        /// </summary>
//        /// <param name="side"></param>
//        public void CancelAll(bool side)
//        {
//            debug("canceling offsets for: " + (side ? "long" : "short"));
//            foreach (Position p in _pt)
//            {
//                // make sure we're not flat
//                if (p.isFlat) continue;
//                // if side matches, cancel all offsets for side
//                if (p.isLong==side)
//                    cancel(GetOffset(p.Symbol));
//            }
//        }
//        /// <summary>
//        /// cancels all offsets for symbol
//        /// </summary>
//        /// <param name="sym"></param>
//        public void CancelAll(string sym)
//        {
            
//            bool hit = false;
//            foreach (Position p in _pt)
//            {
//                // if sym matches, cancel all offsets
//                if (p.Symbol == sym)
//                {
//                    hit |= true;
//                    cancel(GetOffset(sym));
//                }
//            }
//            if (hit)
//                debug("canceling offsets for: " + sym);
            
//        }

//        /// <summary>
//        /// cancels only profit orders for symbol
//        /// 取消某个合约的止盈
//        /// </summary>
//        /// <param name="sym"></param>
//        public void CancelProfit(string sym)
//        {
//            debug("canceling profits for: " + sym);
//            foreach (Position p in _pt)
//            {
//                // if sym matches, cancel all offsets
//                if (p.Symbol == sym)
//                    cancel(GetOffset(sym).ProfitId);
//            }
//        }

//        /// <summary>
//        /// cancels only stops for symbol
//        /// 取消某个合约的止损
//        /// </summary>
//        /// <param name="sym"></param>
//        public void CancelStop(string sym)
//        {
//            debug("canceling stops for: " + sym);
//            foreach (Position p in _pt)
//            {
//                // if sym matches, cancel all offsets
//                if (p.Symbol == sym)
//                    cancel(GetOffset(sym).StopId);
//            }
//        }

//        /// <summary>
//        /// cancel profits for side (long is true, false is short)
//        /// 取消某个方向的止盈
//        /// </summary>
//        /// <param name="side"></param>
//        public void CancelProfit(bool side)
//        {
//            debug("canceling profits for: " + (side ? "long" : "short"));
//            foreach (Position p in _pt)
//            {
//                // make sure we're not flat
//                if (p.isFlat) continue;
//                // if side matches, cancel profits for side
//                if (p.isLong == side)
//                    cancel(GetOffset(p.Symbol).ProfitId);
//            }
//        }

//        /// <summary>
//        /// cancel stops for a side (long is true, false is short)
//        /// 取消某个方向的止损
//        /// </summary>
//        /// <param name="side"></param>
//        public void CancelStop(bool side)
//        {
//            debug("canceling stops for: " + (side ? "long" : "short"));
//            foreach (Position p in _pt)
//            {
//                // make sure we're not flat
//                if (p.isFlat) continue;
//                // if side matches, cancel stops for side
//                if (p.isLong == side)
//                    cancel(GetOffset(p.Symbol).StopId);
//            }
//        }

//        /// <summary>
//        /// cancels all tracked offsets
//        /// 取消所有委托
//        /// </summary>
//        public void CancelAll()
//        {
//            debug("canceling all pending offsets");
//            foreach (OffsetInfo oi in this)
//                cancel(oi);
//        }

//        //检测是否绑定了sendcancel/sendorder事件
//        bool HasEvents()
//        {
//            if (_hasevents) return true;
//            if ((SendCancelEvent == null) || (SendOrderEvent == null))
//                throw new Exception("You must define targets for SendCancel and SendOffset events.");
//            _hasevents = true;
//            return _hasevents;
//        }

//        //是否忽略更新
//        bool IgnoreUpdate(string sym) 
//        {
//            // if updates are ignored by default
//            if (_ignoredefault) // see if we have custom offset
//                return !hascustom(sym);
//            // otherwise see if it's specifically ignored 如果在我们的非监控列表中,则不监控
//            foreach (string isym in _ignore) 
//                if (sym == isym) 
//                    return true; 
//            return false; 
//        }

//        public void ClearCustom()
//        {
//            Clear();
//        }
//        //查询某个合约的止盈与止损委托
//        //止盈委托
//        long ProfitId(string sym)
//        {
//            int idx = getindex(sym);
//            // no offset id
//            if (idx < 0)
//                return 0;
//            // if we have offset, return it's id
//            return base[idx].ProfitId;
//        }
//        //止损委托
//        long StopId(string sym)
//        {
//            int idx = getindex(sym);
//            // no offset id
//            if (idx < 0)
//                return 0;
//            // if we have offset, return it's id
//            return base[idx].StopId;
//        }

//        bool _shutonreinit = true;
//        /// <summary>
//        /// if a position is provided twice in the same session, assume this is bad and cancel/shutdown offsets.
//        /// </summary>
//        public bool ShutdownOnReinit { get { return _shutonreinit; } set { _shutonreinit = value; } }

//        //获得某个持仓或者某个成交
//        public void GotPosition(Position p) { Adjust(p); }
//        public void GotFill(Trade f) { Adjust(f); }

//        /// <summary>
//        /// must send new positions here (eg from GotPosition on Response)
//        /// 获得持仓
//        /// </summary>
//        /// <param name="p"></param>
//        public void Adjust(Position p)
//        {
//            // did position exist? 检查是否存在该合约的持仓,如果存在则是re-initialization
//            bool exists = !_pt[p.Symbol].isFlat;
//            if (exists)
//                debug(p.Symbol + " re-initialization of existing position");
//            //如果存在持仓，并且 我们设定为重置时 重新设置 则重新设置，并取消原有委托
//            if (exists && ShutdownOnReinit)
//            {
//                // get offset
//                OffsetInfo oi = GetOffset(p.Symbol);
//                // disable it percent代表平仓的比例
//                oi.ProfitPercent = 0;
//                oi.StopPercent = 0;
//                // save it
//                SetOffset(p.Symbol, oi);
//                // cancel existing orders
//                CancelAll(p.Symbol);
//                // stop processing
//                return;
//            }
//            // update position
//            //_pt.Adjust(p);
//            _pt.GotPosition(p);
//            // if we're flat, nothing to do
//            if (_pt[p.Symbol].isFlat)
//            {
//                debug(p.Symbol + " initialized to flat.");
//                // cancel pending offsets
//                CancelAll(p.Symbol);
//                // reset offset state but not configuration
//                SetOffset(p.Symbol,new OffsetInfo(this[p.Symbol]));
//                return;
//            }
//            // do we have events? 如果没有委托与取消事件,则我们直接返回
//            if (!HasEvents()) return;
//            // do update
//            doupdate(p.Symbol);
//        }

//        /// <summary>
//        /// must send new fills here (eg call from Response.GotFill)
//        /// 获得新的成交回报
//        /// </summary>
//        /// <param name="t"></param>
//        public void Adjust(Trade t)
//        {
//            // get original size获得原来持仓信息
//            int osize = _pt[t.symbol].Size;
//            // update position 更新当前持仓信息
//            _pt.GotFill(t);
//            // see if it's our order 获得该symbol的offset信息
//            OffsetInfo oi = GetOffset(t.symbol);
//            // see what was hit 查看该trade是否属于我们的profit order/ stop loss order
//            bool hp = (t.id!=0) && (oi.ProfitId == t.id);//hit profit
//            bool hs = (t.id != 0) && (oi.StopId == t.id);//hit stop
//            // if we hit something 如果是我们的offset触发的成交
//            if (hp || hs)
//            {
//                // notify
//                debug(t.symbol + " hit " +(hp ? "profit" : "stop") +": " + t.id);
//                // see if we should clear offset 检查是否全部成交/如果全部成交则 置profitid为0
//                if (hp && (oi.SentProfitSize == t.xsize))
//                {
//                    debug(t.symbol + " profit closed: " + t.id);
//                    oi.ProfitId = 0;
//                }
//                else if (hp) //如果没有全部成交则 更新当前的size
//                    oi.SentProfitSize -= t.xsize;


//                if (hs && (oi.SentStopSize == t.xsize))
//                {
//                    debug(t.symbol + " stop closed: " + t.id);
//                    oi.StopId = 0;
//                }
//                else if (hs)
//                    oi.SentStopSize -= t.xsize;

//                //对外触发 hitoffset事件
//                if (HitOffset != null)
//                    HitOffset(t.symbol, t.id, t.xprice);
//            }
//            // if we're flat, nothing to do (or if we switched sides)
//            Position p = _pt[t.symbol];
//            //如果仓位已经平掉 或者 仓位方向相反,则我们取消所有委托
//            if (p.isFlat || (osize*p.Size<-1))
//            {
//                if (p.isFlat)
//                    debug(t.symbol + " now flat.");
//                else
//                    debug(t.symbol + " reversed: " + osize + " -> " + p.Size);
//                CancelAll(t.symbol);
//                // reset offset state but not configuration 并且重新设置offset
//                SetOffset(t.symbol,new OffsetInfo(this[t.symbol]));
//            }
//            else // save offset 保存当前offset
//                SetOffset(t.symbol, oi);
//            // do we have events?
//            if (!HasEvents()) return;
//            // do update 更新该symbol
//            doupdate(t.symbol);

//        }

//        /// <summary>
//        /// obtain curretn offset information for a symbol.
//        /// if no custom value has been set, use default
//        /// 获得某个symbol 当前offset信息
//        /// </summary>
//        /// <param name="symbol"></param>
//        /// <returns></returns>
//        public new OffsetInfo this[string symbol] { get { return GetOffset(symbol); } set { SetOffset(symbol, value); } }

//        bool _addcust = false;
//        public bool AddCustom { get { return _addcust; } set { _addcust = value; } }

//        //获得某个symbol的offset信息
//        OffsetInfo GetOffset(string sym)
//        {
//            // see if we have a custom offset
//            int idx = getindex(sym);
//            OffsetInfo oi = null;
//            if (idx >= 0)
//                oi = base[idx];
//            // if we don't have a custom but we're adding one, add from default
//            //是否addcustom如果addcustom则新增加一个,如果不是则返回defaultoffset
//            if (AddCustom && (idx < 0))
//            {
//                idx = addindex(sym, DefaultOffset);
//            }
//            else if (oi==null) // otherwise use default
//                return DefaultOffset;
//            // return custom
//            return oi;
            
//        }

//        bool _verbdebug = false;
//        /// <summary>
//        /// enable verbose debugging messages
//        /// </summary>
//        public bool VerboseDebugging { get { return _verbdebug; } set { _verbdebug = value; } }

//        //设定某个symbol的offset信息
//        void SetOffset(string sym, OffsetInfo off)
//        {
//            // check for index
//            int idx = addindex(sym, off);
//            if (_verbdebug)
//                debug(sym + " set offset: " + off.ToString(DebugDecimals));
//        }
//        /// <summary>
//        /// should be called from GotCancel, when cancels arrive from broker.
//        /// 得到取消委托回报
//        /// 遍历所有symbol,核对其stopid与profitid
//        /// </summary>
//        /// <param name="id"></param>
//        public void GotCancel(long id)
//        {
//            // find any matching orders and reflect them as canceled
//            foreach (string sym in ToLabelArray())
//            {
//                if (base[sym].StopId == id)
//                {
//                    debug(string.Format("stop canceled: {0} {1}", sym, id.ToString()));
//                    base[sym].StopId = 0;
//                }
//                else if (base[sym].ProfitId == id)
//                {
//                    debug(string.Format("profit canceled: {0} {1}", sym, id.ToString()));
//                    base[sym].ProfitId = 0;
//                }
//            }

//        }
//        /// <summary>
//        /// should be called from GotTick, when ticks arrive.
//        /// If cancels are not processed on fill updates, they will be resent each tick until they are processed.
//        /// 根据tick来更新
//        /// </summary>
//        /// <param name="k"></param>
//        public void newTick(Tick k)
//        {
//            // otherwise update the offsets for this tick's symbol
//            doupdate(k.symbol);
//        }

//        // track offset ids
//        Dictionary<string, long> _profitids = new Dictionary<string, long>();//止盈委托
//        Dictionary<string, long> _stopids = new Dictionary<string, long>();//止损委托
        

//    }


//}
