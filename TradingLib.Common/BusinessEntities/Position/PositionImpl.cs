using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using TradingLib.API;


namespace TradingLib.Common
{
    /// <summary>
    /// A position type used to describe the position in a stock or instrument.
    /// 持仓对象
    /// 持仓对象是一个生成对象，通过持仓明细，成交生成当前最新的持仓状态
    /// </summary>
    [Serializable]
    public class PositionImpl : TradingLib.API.Position, IConvertible
    {
        protected static  NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        #region 类型转换
        public decimal ToDecimal(IFormatProvider provider)
        {
            return AvgPrice;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return (double)AvgPrice;
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return (Int16)Size;
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Size;
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Size;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return !isFlat;
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return ToString();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(Size, conversionType);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
        

        /// <summary>
        /// convert from position to decimal (price)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static implicit operator decimal(PositionImpl p)
        {
            return p.AvgPrice;
        }
        /// <summary>
        /// convert from position to integer (size)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static implicit operator int(PositionImpl p)
        {
            return p.Size;
        }
        /// <summary>
        /// convert from
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static implicit operator bool(PositionImpl p)
        {
            return !p.isFlat;
        }
        #endregion

        

        

        public PositionImpl()
            :this("",0,0,0,"",QSEnumPositionDirectionType.BothSide)
        {
            _sym = string.Empty;
            _size = 0;
            _price = 0;
            _closedpl = 0;
            _acct = string.Empty;
            _directiontype = QSEnumPositionDirectionType.BothSide;
            _last = 0;
            _highest = 0;
            _lowest = 0;
            
        }

        public PositionImpl(string account, string symbol, QSEnumPositionDirectionType type)
            :this(symbol,0,0,0,account,type)
        { 
            
        }

        public PositionImpl(string symbol, decimal price, int size, decimal closedpl, string account,QSEnumPositionDirectionType type)
        { 
            _sym = symbol; 
            if (size == 0) price = 0; 
            _price = price; 
            _size = size; 
            _closedpl = closedpl; 
            _acct = account;
            _directiontype = type;
            if (!this.isValid) throw new Exception("Can't construct invalid position!"); 
        }


        /// <summary>
        /// 持仓是否有效
        /// 合约不为空 价格和数量同时为0 或者同时不为0
        /// </summary>
        public bool isValid
        {
            get { return (_sym!="") && (((AvgPrice == 0) && (Size == 0)) || ((AvgPrice != 0) && (Size != 0))); }
        }



        bool _settled = false;
        /// <summary>
        /// 是否已经结算
        /// </summary>
        public bool Settled { get { return _settled; } set { _settled = value; } }

        /// <summary>
        /// 浮动盈亏
        /// </summary>
        public decimal UnRealizedPL{
            get 
            {
                return _size * (LastPrice - AvgPrice); 
            }
        }


        decimal _closedpl = 0;
        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public decimal ClosedPL { get { return _closedpl; } }


        #region 价格信息
        decimal? _settlementprice = null;
        /// <summary>
        /// 结算价
        /// </summary>
        public decimal? SettlementPrice { get { return _settlementprice; } set { _settlementprice = value; } }

        decimal? _lastsettlementprice = null;
        /// <summary>
        /// 昨日结算价
        /// </summary>
        public decimal? LastSettlementPrice { get { return _lastsettlementprice; } set { _lastsettlementprice = value; } }

        private decimal _highest = decimal.MinValue;
        /// <summary>
        /// 最高价
        /// </summary>
        public decimal Highest { get { return _highest; } set { _highest = value; } }

        private decimal _lowest = decimal.MaxValue;
        /// <summary>
        /// 最低价
        /// </summary>
        public decimal Lowest { get { return _lowest; } set { _lowest = value; } }

        decimal _last = 0;
        /// <summary>
        /// 最新价格
        /// 如果没有正常获得tick数据则返回持仓均价
        /// </summary>
        public decimal LastPrice
        {
            get
            {
                if (!_gotTick) return AvgPrice;
                return _last;
            }
        }

        #endregion

        #region 基础信息
        string _acct = "";
        /// <summary>
        /// 交易帐户
        /// </summary>
        public string Account { get { return _acct; } }

        Symbol _osymbol = null;
        /// <summary>
        /// 合约对象
        /// </summary>
        public Symbol oSymbol { get { return _osymbol; } set { _osymbol = value; } }

        string _sym = "";
        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get { return _sym; } }

        QSEnumPositionDirectionType _directiontype = QSEnumPositionDirectionType.BothSide;
        /// <summary>
        /// 持仓方向
        /// </summary>
        public QSEnumPositionDirectionType DirectionType { get { return _directiontype; } set { _directiontype = value; } }


        decimal _price = 0;
        /// <summary>
        /// 持仓均价
        /// </summary>
        public decimal AvgPrice { get { return _price; } }

        int _size = 0;
        /// <summary>
        /// 持仓数量
        /// </summary>
        public int Size { get { return _size; } }

        /// <summary>
        /// 持仓数量绝对值
        /// </summary>
        public int UnsignedSize { get { return Math.Abs(_size); } }

        public bool isLong { get { return _size > 0; } }
        public bool isFlat { get { return _size == 0; } }
        public bool isShort { get { return _size < 0; } }

        /// <summary>
        /// 平仓数量
        /// </summary>
        public int FlatSize { get { return _size * -1; } }

        #endregion



        decimal _openamount = 0;
        /// <summary>
        /// 开仓金额
        /// </summary>
        public decimal OpenAmount { get { return _openamount; } }

        int _openvol = 0;
        /// <summary>
        /// 开仓数量
        /// </summary>
        public int OpenVolume { get { return _openvol; } }

        decimal _closeamount = 0;
        /// <summary>
        /// 平仓金额
        /// </summary>
        public decimal CloseAmount { get { return _closeamount; } }

        int _closevol = 0;
        /// <summary>
        /// 平仓数量
        /// </summary>
        public int CloseVolume { get { return _closevol ; } }


        #region 行情驱动部分
        //TODO SymbolKey
        bool _gotTick = false;

        /// <summary>
        /// 是否使用盘口价格来更新最新价格
        /// </summary>
        public void GotTick(Tick k)
        {
            
        }
        // {
        //     //动态的更新unrealizedpl，这样就不用再委托检查是频繁计算
        //     //更新最新的价格信息
        //     string key = k.GetSymbolUniqueKey();
        //     if (key != this.oSymbol.GetUniqueKey()) return;
        //     //if (k.Symbol != (this.oSymbol != null ? this.oSymbol.TickSymbol : this.Symbol))//合约的行情比对或者模拟成交是按Tick进行的。应为异化合约只是合约代码和保证金手续费不同,异化合约依赖于底层合约
        //     //    return;
        //
        //     //更新最新价
        //     if (k.IsTrade())
        //     {
        //         _gotTick = true;
        //         _last = k.Trade;
        //     }
        //
        //     //更新最高最低价 持仓内的最高 最低价
        //     if (!isFlat)
        //     {
        //         _highest = _highest >= _last ? _highest : _last;
        //         _lowest = _lowest <= _last ? _lowest : _last;
        //     }
        //
        //     //if (k.ex == "demo")//用于测试
        //     //{
        //         //从行情更新昨结算价
        //         if (_lastsettlementprice == null && k.PreSettlement > 0 && (double)k.PreSettlement < double.MaxValue)
        //         {
        //             logger.Debug("tick:" + k.ToString() + " presettlement:" + k.PreSettlement.ToString());
        //             _lastsettlementprice = k.PreSettlement;
        //             //更新所有持仓明细的昨日结算价格
        //             //昨日持仓明细在YdRef保存的不用更新 该数据用于获得隔夜仓的成本即昨天的结算价为成本
        //             //只用更新新开仓的昨日结算价格 从历史持仓明细表中加载的持仓明细 会从结算价中获得上日结算价 如果结算价异常以收盘价结算，但是加载时又根据行情来更新昨日结算价，则会造成持仓价格变动 金额帐户盈亏计算不准确的问题
        //             foreach (PositionDetail p in this.PositionDetailTodayNew)
        //             {
        //                 p.LastSettlementPrice = k.PreSettlement;
        //             }
        //             //更新所有平仓明细的昨日结算价格
        //             foreach (PositionCloseDetail c in this.PositionCloseDetail)
        //             {
        //                 c.LastSettlementPrice = k.PreSettlement;
        //             }
        //             //Util.Info("update presettlementprice for position[" + this.Account + "-" + this.Symbol + "] price:" + _lastsettlementprice.ToString() + " tick presettlement:" + k.PreSettlement.ToString());
        //         }
        //         //检查昨结算价格是否异常 如果获得了昨日结算价格 但是又和行情中的昨结算价格不一致则有异常
        //         //if (_lastsettlementprice != null && k.PreSettlement > 0 && k.PreSettlement != _lastsettlementprice)
        //         //{
        //         //    //Util.Debug("tick:" + k.ToString() +" presettlement:"+k.PreSettlement.ToString());
        //         //    //Util.Debug("PreSettlement price error,it changed during trading,tick presetttle:"+k.PreSettlement.ToString() +" local presettlement:"+_lastsettlementprice.ToString(), QSEnumDebugLevel.ERROR);
        //         //}
        //
        //
        //         //从行情更新结算价格 更新所有持仓明细的行情
        //         //if (_settlementprice == null && k.Settlement > 0 && (double)k.Settlement < double.MaxValue)
        //         //{
        //         //    _settlementprice = k.Settlement;
        //         //    //更新所有持仓明细的当日结算价格
        //         //    foreach (PositionDetail p in this.PositionDetailTotal)
        //         //    {
        //         //        p.SettlementPrice = k.Settlement;
        //         //    }
        //         //    //Util.Info("update settlementprice for position[" + this.Account + "-" + this.Symbol + "] price:" + _settlementprice.ToString());
        //         //}
        //     //}
        // }
        #endregion


        #region 隔夜持仓明细 当日持仓明细 成交明细 平仓明细
        ThreadSafeList<Trade> _tradelist = new ThreadSafeList<Trade>();
        /// <summary>
        /// 返回该持仓当日所有成交列表
        /// </summary>
        public IEnumerable<Trade> Trades
        {
            get
            {
                return _tradelist;
            }
        }

        ThreadSafeList<PositionDetail> _poshisreflist = new ThreadSafeList<PositionDetail>();
        /// <summary>
        /// 返回该持仓当日所有历史持仓明细
        /// 这里的数据不做具体计算,
        /// </summary>
        public IEnumerable<PositionDetail> PositionDetailYdRef
        {
            get
            {
                return _poshisreflist;
            }
        }

        ThreadSafeList<PositionDetail> _postotallist = new ThreadSafeList<PositionDetail>();
        /// <summary>
        /// 所有持仓明细
        /// 包括昨日结算持仓明细和当日新开仓持仓明细
        /// </summary>
        public IEnumerable<PositionDetail> PositionDetailTotal
        {
            get
            {
                return _postotallist;
            }
        }

        ThreadSafeList<PositionDetail> _poshisnewlist = new ThreadSafeList<PositionDetail>();
        /// <summary>
        /// 返回该持仓当日所有历史持仓明细
        /// 这里的数据做具体计算
        /// </summary>
        public IEnumerable<PositionDetail> PositionDetailYdNew
        {
            get
            {
                return _poshisnewlist;
            }
        }

        ThreadSafeList<PositionDetail> _postodaynewlist = new ThreadSafeList<PositionDetail>();
        /// <summary>
        /// 今日新开仓持仓明细列表
        /// </summary>
        public IEnumerable<PositionDetail> PositionDetailTodayNew
        {
            get
            {
                return _postodaynewlist;
            }
        }

        ThreadSafeList<PositionCloseDetail> _posclosedetaillist = new ThreadSafeList<PositionCloseDetail>();
        /// <summary>
        /// 平仓明细
        /// </summary>
        public IEnumerable<PositionCloseDetail> PositionCloseDetail
        {
            get
            {
                return _posclosedetaillist;
            }
        }
        #endregion

        int _domain_id = 0;
        /// <summary>
        /// 域ID
        /// </summary>
        public int Domain_ID { get{return _domain_id;} set{_domain_id=value;} }

        #region 用Positon PositionDetail Trade更新当前持仓
        /// <summary>
        /// Adjusts the position by applying a new trade or fill.
        /// 这里记录了日内所有成交,用成交更新持仓状态
        /// 在Net类型的持仓状态下 平仓明细会发生错误
        /// 持有多头3手，空头1手，买入平仓1手时会报 持仓方向与成交方向不符的异常
        /// 因为Net状态下 多空是混合在一起的,因此在closeposition中需要用方向进行分组
        /// </summary>
        /// <param name="t">The new fill you want this position to reflect.</param>
        /// <returns></returns>
        public decimal Adjust(Trade t,out bool accept) 
        {
            accept = false;
            //如果合约为空 则默认pos的合约
            if ((_sym == "") && t.isValid) _sym = t.Symbol;
            //合约不为空比较 当前持仓合约和adjusted pos的合约
            if ((_sym != t.Symbol)) throw new Exception("Failed because adjustment symbol did not match position symbol");
            //如果osymbol为空则取默认pos的osymbol
            if (_osymbol == null && t.oSymbol != null)
            {
                if (!t.Symbol.Equals(this.Symbol)) throw new Exception("Failed because osymbol and symbol do not match");
                _osymbol = t.oSymbol;
            }
            
            //帐户比较
            if (_acct == "") _acct = t.Account;
            if (_acct != t.Account) throw new Exception("Failed because adjustment account did not match position account.");

            //成交设置成为可接受
            accept = true;

            //1.保存成交数据
            //_tradelist.Add(t);

            decimal cpl = 0;
            //2.处理成交
            if (t.IsEntryPosition)//开仓 由开仓成交生成新的持仓明细 并设定昨日结算价格
            {
                _openamount += t.GetAmount();
                _openvol += t.UnsignedSize;

                if (NeedGenPositionDetails)
                {
                    //根据新开仓成交生成当日新开持仓明细
                    PositionDetail d = this.OpenPosition(t);
                    _postodaynewlist.Add(d);//插入今日新开仓持仓明细
                    _postotallist.Add(d);//插入到Totallist便于访问
                    NewPositionDetail(t, d);//对外触发持仓明细事件
                }
                //保存成交数据
                _tradelist.Add(t);
            }
            else//平仓金额 数量累加
            {
                bool closefail = false;
                if (NeedGenPositionDetails)
                {
                    cpl = ClosePosition(t,out closefail);//执行平仓操作
                    if (closefail)
                    {
                        accept = false;
                        logger.Error(string.Format("Close Position Fail, Pos:{0} Trade:{1}", this.ToString(), t.GetTradeDetail()));
                        return 0;
                    }
                    else
                    {
                        //保存成交数据
                        _tradelist.Add(t);
                    }
                }

                _closeamount += t.GetAmount();
                _closevol += t.UnsignedSize;
            }

            //3.调整持仓汇总的数量和价格
            //更新持仓数量
            this._size += t.xSize;

            //持仓均价 由于平仓按先开先平的规则进行因此这里持仓均价为未平仓持仓明细部分的均价，而不是综合均价
            if (this._size == 0)
            {
                this._price = 0;
            }
            else
            {
                this._price = _postotallist.Where(pos1 => !pos1.IsClosed()).Sum(pos2 => pos2.Volume * pos2.CostPrice()) / Math.Abs(this._size);
            }
            //Util.Debug("runing size:" + this._size.ToString() + " positiondetail size:" + _postotallist.Where(pos1 => !pos1.IsClosed()).Sum(pos2 => pos2.Volume));
            _closedpl += cpl; // update running closed pl 更新平仓盈亏
            return cpl;//返回平仓盈亏
        }

        /// <summary>
        /// 用历史持仓明细数据调整当前持仓数据 用于初始化时从数据库恢复历史持仓数据
        /// 
        /// 期货按先开先平或指定平今/平昨进行持仓处理 当前持仓均价是会按平仓的过程动态发生变化
        /// 
        /// 股票持仓 进行合并不按先后进行平仓 即开仓后持仓成本固定，平仓是平合并掉的持仓 不按先开先平的规则进行平仓
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public decimal Adjust(PositionDetail d)
        {
            //如果合约为空 则默认pos的合约
            if (_sym == "") _sym = d.Symbol;
            //合约不为空比较 当前持仓合约和adjusted pos的合约
            if ((_sym != d.Symbol)) throw new Exception("Failed because adjustment symbol did not match position symbol");
            //如果osymbol为空则取默认pos的osymbol
            if (_osymbol == null && d.oSymbol != null)
            {
                if (!d.Symbol.Equals(this.Symbol)) throw new Exception("Failed because osymbol and symbol do not match");
                _osymbol = d.oSymbol;
            }

            //帐户比较
            if (_acct == "") _acct = d.Account;
            if (_acct != d.Account) throw new Exception("Failed because adjustment account did not match position account.");

            if (NeedGenPositionDetails)
            {
                _poshisreflist.Add(d);
                //1.加载历史持仓
                PositionDetail pd = LoadHisPosition(d);
                _poshisnewlist.Add(pd);
                _postotallist.Add(pd);
            }

            this._size += d.Side ? d.Volume : d.Volume * -1;

            if (this._size == 0)
            {
                this._price = 0;
            }
            else
            {
                decimal v = d.CostPrice();
                int s = d.Volume;

                //通过加权计算获得当前的持仓均价
                this._price = _postotallist.Where(pos1 => !pos1.IsClosed()).Sum(pos2 => pos2.Volume * pos2.CostPrice()) / Math.Abs(this._size);
            }   
            return 0;//开仓时 平仓成本为0
        }
        #endregion


        /// <summary>
        /// 判断是否需要生成持仓明细
        /// </summary>
        bool NeedGenPositionDetails
        {
            get
            {
                switch (this.DirectionType)
                { 
                    case QSEnumPositionDirectionType.Long:
                    case QSEnumPositionDirectionType.Short:
                        return true;
                    default:
                        return false;
                }
            }
        }

        #region 平仓开仓事件
        /// <summary>
        /// 平仓 产生新的平仓明细
        /// </summary>
        /// <param name="closedetail"></param>
        void NewPositionCloseDetail(Trade close,PositionCloseDetail closedetail)
        {
            _posclosedetaillist.Add(closedetail);
            if (NewPositionCloseDetailEvent != null)
                NewPositionCloseDetailEvent(close,closedetail);
        }
        public event Action<Trade,PositionCloseDetail> NewPositionCloseDetailEvent;

        /// <summary>
        /// 开仓 产生新的持仓明细
        /// </summary>
        /// <param name="open"></param>
        /// <param name="detail"></param>
        void NewPositionDetail(Trade open, PositionDetail detail)
        {
            if (NewPositionDetailEvent != null)
                NewPositionDetailEvent(open, detail);
        }
        public event Action<Trade, PositionDetail> NewPositionDetailEvent;

        #endregion


        /// <summary>
        /// 利用平仓成交平掉对应的持仓明细 按照先开先平或者平今平昨的平仓逻辑
        /// 如果是净持仓 可能会导致逻辑异常 这里需要再分析一下
        /// 平仓操作会返回一个平仓盈亏 用于填充到adjust
        /// 
        /// 平仓操作根据交易所结算规 返回逐日或逐笔平仓数据
        /// </summary>
        /// <param name="close"></param>
        decimal  ClosePosition(Trade close,out bool closefail)
        {
            
            closefail = false;

            int remainsize = close.UnsignedSize;
            decimal closeprofit = 0;//平仓盈亏金额 用于设定平仓成交的平仓盈亏金额
            decimal closepoint = 0;//平仓盈亏点数
            bool bydate = close.oSymbol.SecurityFamily.Exchange.SettleType == QSEnumSettleType.ByDate;

            //先平历史持仓或者按照平今 平昨的规则进行
            foreach (PositionDetail p in _poshisnewlist)
            {
                //上期所平今成交 不对历史持仓进行计算
                if (close.oSymbol.SecurityFamily.Exchange.EXCode == "SHFE" && close.OffsetFlag == QSEnumOffsetFlag.CLOSETODAY) continue;

                if (remainsize == 0) break; //剩余平仓数量为0 跳出 当有多余的持仓明细没有被平掉，而当前平仓成交已经使用完毕
                if (p.IsClosed()) continue;//如果当前持仓明细已经关闭 则取下一条持仓明细
                PositionCloseDetail closedetail = null;
                try
                {
                    //平仓明细
                    closedetail = this.ClosePosition(p,close,remainsize);
                    close.CloseDetails.Add(closedetail);
                }
                catch (Exception ex)
                {
                    logger.Fatal("close position error:" + ex.ToString());
                }
                if (closedetail != null)
                {
                    closeprofit += bydate ? closedetail.CloseProfitByDate : closedetail.CloseProfitByTrade;//平仓盈亏金额
                    closepoint += bydate ? closedetail.ClosePointByDate : closedetail.ClosePointByTrade;
                    remainsize -= closedetail.CloseVolume;
                    NewPositionCloseDetail(close,closedetail);
                }
            }

            //再平日内持仓
            foreach (PositionDetail p in _postodaynewlist)
            {
                //剩余数量为0跳出
                if (remainsize == 0) break;//这里假设有多余的持仓明细没有被平掉，而当前平仓成交已经使用完毕
                if (p.IsClosed()) continue;

                //平仓明细
                PositionCloseDetail closedetail = null;
                try
                {
                    //平仓明细
                    closedetail = this.ClosePosition(p, close,remainsize);
                    close.CloseDetails.Add(closedetail);
                }
                catch (Exception ex)
                {
                    logger.Fatal("close position error:" + ex.ToString());
                }
                if (closedetail != null)
                {
                    closeprofit += bydate ? closedetail.CloseProfitByDate : closedetail.CloseProfitByTrade;
                    closepoint += bydate ? closedetail.ClosePointByDate : closedetail.ClosePointByTrade;
                    remainsize -= closedetail.CloseVolume;
                    NewPositionCloseDetail(close,closedetail);
                }
            }

            //这里需要解决刚好平仓完毕的情况，遍历完毕所有持仓明细 并且平仓成交的剩余平仓量为0
            if (remainsize == 0) //这里假设有多余的持仓明细没有被平掉，而当前平仓成交已经使用完毕
            {   //设定平仓成交的盈亏
                close.Profit = closeprofit;
                return closepoint;
            }
            else
            {
                closefail = true;//平仓成交出现异常
                logger.Error("exit trade have not used up,some big error happend");
            }
            return 0;

        }

        /// <summary>
        /// 加载数据库的昨日持仓对象到工作状态
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        PositionDetail LoadHisPosition(PositionDetail p)
        {
            PositionDetail pd = new PositionDetailImpl(this);
            pd.Settleday = p.Settleday;//隔夜仓将 具体的持仓结算日期记录下来
            pd.TradeID = p.TradeID;
            pd.IsHisPosition = true;//如果是从数据库加载的positiondetail生成的持仓明细就是昨仓
            pd.OpenDate = p.OpenDate;
            pd.OpenTime = p.OpenTime;
            pd.OpenPrice = p.OpenPrice;
            pd.Side = p.Side;
            pd.Volume = p.Volume;

            //记录该持仓对应的接口信息和对应的数据源
            pd.Broker = p.Broker; //分帐户侧平历史持仓时 需要根据broker来寻找对应的接口 否则会导致系统不知道从哪个接口平掉该持仓
            pd.Breed = p.Breed;

            //加载到今日持仓明细列表中的昨日持仓明细列表，需要将对应的昨日结算价格设定为昨日持仓明细的结算价格 并且不能被行情更新
            pd.LastSettlementPrice = p.SettlementPrice;
            pd.HedgeFlag = p.HedgeFlag;

            //平仓统计数据归零  历史持仓加载的时候要将历史信息去除 比如平仓量(属于昨天的信息) 开仓量也是数据昨天的信息
            pd.CloseAmount = 0;
            pd.CloseVolume = 0;
            pd.CloseProfitByDate = 0;
            pd.CloseProfitByTrade = 0;
            
            return pd;
        }

        /// <summary>
        /// 执行开仓操作 返回一个新的持仓明细
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        PositionDetail OpenPosition(Trade f)
        {
            PositionDetail pos = new PositionDetailImpl(this);
            pos.Account = f.Account;
            pos.oSymbol = f.oSymbol;
            pos.IsHisPosition = false;//通过成交生成的开仓明细均为日内持仓

            pos.OpenDate = f.xDate;
            pos.OpenTime = f.xTime;
            //pos.LastSettlementPrice = this.LastSettlementPrice != null ? (decimal)this.LastSettlementPrice : f.xPrice;//新开仓设定昨日结算价
            pos.Settleday = f.SettleDay;//持仓明细对应的结算日与成交记录的结算日一致
            pos.Side = f.PositionSide;
            pos.Volume = Math.Abs(f.xSize);
            pos.OpenPrice = f.xPrice;
            pos.TradeID = f.TradeID;//开仓明细中的开仓成交编号
            pos.HedgeFlag = "";

            //成交数据会传递Broker字段,用于记录该成交是哪个成交接口回报的，对应开仓时,我们需要标记该持仓明细数序那个成交接口
            pos.Broker = f.Broker;
            pos.Breed = f.Breed;
            //pos.Domain_ID = f.Domain_ID;

            return pos;
        }



        /// <summary>
        /// 执行平仓操作 返回平仓明细
        /// </summary>
        /// <param name="close"></param>
        /// <returns></returns>
        PositionCloseDetail ClosePosition(PositionDetail pos, Trade f, int remainsize)
        {
            if (pos.IsClosed()) throw new Exception("can not close the closed position");
            if (f.IsEntryPosition) throw new Exception("entry trade can not close postion");
            if (pos.Account != f.Account) throw new Exception("postion's account do not match with trade");
            if (pos.Symbol != f.Symbol) throw new Exception("position's symbol do not math with trade");
            if (pos.Side != f.PositionSide) throw new Exception(string.Format("position's side[{0}] do not math with trade's side[{1}]", pos.Side, f.PositionSide));

            //计算平仓量
            int closesize = pos.Volume >= remainsize ? remainsize : pos.Volume;

            //生成平仓对象
            PositionCloseDetail closedetail = new PositionCloseDetailImpl(pos, f, closesize);
            closedetail.Settleday = f.SettleDay;//由成交平仓的 平仓明细 与成交的结算日一致


            //更新持仓明细的平仓汇总信息
            pos.Volume -= closedetail.CloseVolume;
            pos.CloseVolume += closedetail.CloseVolume;
            pos.CloseAmount += closedetail.CloseAmount;
            pos.CloseProfitByDate += closedetail.CloseProfitByDate;
            pos.CloseProfitByTrade += closedetail.CloseProfitByTrade;
            

            //pos.Domain_ID = pos.Domain_ID;
            return closedetail;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Account);//交易帐号
            sb.Append(" ");
            sb.Append((this.oSymbol != null ? this.oSymbol.FullName : this.Symbol));//合约
            sb.Append(" ");
            sb.Append(this.DirectionType);
            sb.Append(" ");
            sb.Append(string.Format("{0}@{1}", this.Size, AvgPrice));
            sb.Append(" ");
            sb.Append(string.Format("UnPL:{0} RePL:{1}", this.UnRealizedPL.ToString(), this.ClosedPL.ToString()));
            sb.Append(" ");
            sb.Append("Trade Num:" + _tradelist.Count.ToString() +" SettlePrice:"+this.SettlementPrice.ToString());
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }
            


        #region 静态函数

        
        /// <summary>
        /// 从一组持仓明细生成持仓汇总
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        public static IEnumerable<Position> FromPositionDetail(IEnumerable<PositionDetail> details)
        {
            List<Position> list = new List<Position>();

            //分别按多空 形成持仓
            Position longpos = new PositionImpl();
            longpos.DirectionType = QSEnumPositionDirectionType.Long;
            foreach (PositionDetail p in details.Where(pd=>pd.Side))
            {
                longpos.Adjust(p);
            }
            if (longpos.isValid) list.Add(longpos);

            Position shortpos = new PositionImpl();
            shortpos.DirectionType = QSEnumPositionDirectionType.Long;
            foreach (PositionDetail p in details.Where(pd=>!pd.Side))
            {
                shortpos.Adjust(p);
            }
            if (shortpos.isValid) list.Add(shortpos);
            return list;
        }
        #endregion


    }
}
