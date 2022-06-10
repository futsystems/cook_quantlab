using System;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// Specify an order to buy or sell a quantity of a security.
    /// 由委托产生成交
    /// 1.调用order.fill(tick) order.fill(order)的方式来触发成交,
    /// 2.当有成交产生时会填写order内部字段 xDate xTime xPrice xSize
    /// 其余属性Trade和Order均相同
    /// 3.交由其他组件填充其他书信字段比如BrokerSide,AccountSide相关属性
    /// </summary>
    public class OrderImpl : TradeImpl, Order
    {
        

        int _date = 0;
        /// <summary>
        /// 日期
        /// </summary>
        public int Date { get { return _date; } set { _date = value; } }

        int _time = 0;
        /// <summary>
        /// 时间
        /// </summary>
        public int Time { get { return _time; } set { _time = value; } }


        QSEnumTimeInForce _tif = QSEnumTimeInForce.DAY;
        /// <summary>
        /// TIF指令
        /// </summary>
        public QSEnumTimeInForce TimeInForce { get { return _tif; } set { _tif = value; } }

        int _size = 0;
        /// <summary>
        /// 剩余当前数量
        /// </summary>
        public int Size { get { return _size; } set { _size = value; } }

        int _totalsize;
        /// <summary>
        /// 所有数量
        /// </summary>
        public int TotalSize { get { return _totalsize; } set { _totalsize = value; } }

        int _filled = 0;
        /// <summary>
        /// 成交手数
        /// </summary>
        public int FilledSize { get { return _filled; } set { _filled = value; } }

        /// <summary>
        /// 当前委托数量 无符号
        /// </summary>
        public new int UnsignedSize { get { return Math.Abs(_size); } }

        decimal _price=0;
        /// <summary>
        /// Limit价格
        /// </summary>
        public decimal LimitPrice { get { return _price; } set { _price = value; } }


        decimal _stopp=0;
        /// <summary>
        /// Stop价格
        /// </summary>
        public decimal StopPrice { get { return _stopp; } set { _stopp = value; } }

        decimal _trail=0;
        /// <summary>
        /// trail
        /// </summary>
        public decimal trail { get { return _trail; } set { _trail = value; } }
        

        QSEnumOrderSource _ordersource = QSEnumOrderSource.UNKNOWN;
        /// <summary>
        /// 委托来源
        /// </summary>
        public QSEnumOrderSource OrderSource { get { return _ordersource; } set { _ordersource = value; } }


        //委托状态 记录了委托过程
        QSEnumOrderStatus _status=QSEnumOrderStatus.Unknown;
        /// <summary>
        /// 委托状态
        /// </summary>
        public QSEnumOrderStatus Status { get { return _status; } set { _status = value; } }


        bool _isforceclose = false;
        /// <summary>
        /// 是否强平
        /// </summary>
        public bool ForceClose { get { return _isforceclose; } set { _isforceclose = value; } }


        string _forceclosereason = string.Empty;
        /// <summary>
        /// 强平原因
        /// </summary>
        public string ForceCloseReason { get { return _forceclosereason; } set { _forceclosereason = value.Replace(',', ' ').Replace('|', ' ').Replace('^', ' '); } }

        string _comment = string.Empty;
        /// <summary>
        /// 备注/状态信息
        /// </summary>
        public string Comment
        {
            get { return _comment; }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _comment = string.Empty;
                }
                else
                {
                    //替换comment中出现的协议保留字段, | ^(|分割请求编号 ^ 分割内容 islast rspinfo ,分割具体的内容)
                    string tmp = value.Replace(",", " ").Replace("|", " ").Replace("^", " ");//替换特殊符号 , ^ |
                    _comment = tmp;
                }

            }
        }

        



        #region 判定函数
        public new bool isValid
        {
            get
            {
                if (isFilled) return base.isValid;//如果已经成交 则返回Trade的valid判定
                return (Symbol != null) && (TotalSize != 0);
            }
        }
        public bool isMarket { get { return (LimitPrice == 0) && (StopPrice == 0); } }
        public bool isLimit { get { return (LimitPrice != 0); } }
        public bool isStop { get { return (StopPrice != 0); } }
        public bool isTrail { get { return trail != 0; } }
        #endregion


        #region 分帐户端属性

        int _frontidi = 0;
        /// <summary>
        /// 标注该委托来自于哪个前置
        /// </summary>
        public int FrontIDi { get { return _frontidi; } set { _frontidi = value; } }

        int _sessionIDi = 0;
        /// <summary>
        /// 回话编号
        /// </summary>
        public int SessionIDi { get { return _sessionIDi; } set { _sessionIDi = value; } }

        int _nRequest = 0;
        /// <summary>
        /// 客户端请求编号
        /// </summary>
        public int RequestID { get { return _nRequest; } set { _nRequest = value; } }

        int _copyid = 0;
        /// <summary>
        /// 标识Copy引用
        /// </summary>
        public int CopyID { get { return _copyid; } set { _copyid = value; } }
        #endregion

        #region 构造函数
        public OrderImpl() : base() { }
        public OrderImpl(bool side) : base() { this.Side = side; } 
        /// <summary>
        /// 复制一个Order得到一个全新的副本,对其中一个副本的数据操作不会影响到另外一个副本的数据
        /// 系统内部复制委托需要同时复制对应的合约对象引用,用于保持对底层基本信息的快速引用
        /// </summary>
        /// <param name="copythis"></param>
        public OrderImpl(Order copythis)
        {
            //Util.Debug("Order Copyed:" + copythis.GetOrderInfo(), QSEnumDebugLevel.WARNING);
            this.id = copythis.id;
            this.Account = copythis.Account;
            this.Date = copythis.Date;
            this.Time = copythis.Time;

            this.Symbol = copythis.Symbol;
            this.LocalSymbol = copythis.LocalSymbol;
            this.oSymbol = copythis.oSymbol;

            this.TimeInForce = copythis.TimeInForce;

            this.OffsetFlag = copythis.OffsetFlag;
            this.HedgeFlag = copythis.HedgeFlag;

            this.Size = copythis.Size;
            this.TotalSize = copythis.TotalSize;
            this.FilledSize = copythis.FilledSize;
            this.Side = copythis.Side;
            this.LimitPrice = copythis.LimitPrice;
            this.StopPrice = copythis.StopPrice;
            this.trail = copythis.trail;

            this.Exchange= copythis.Exchange;
            this.SecurityType = copythis.SecurityType;
            this.Currency = copythis.Currency;
            this.OrderSource = copythis.OrderSource;
            this.ForceClose = copythis.ForceClose;
            this.ForceCloseReason = copythis.ForceCloseReason;

            this.Status = copythis.Status;
            this.Comment = copythis.Comment;

            this.Broker = copythis.Broker;
            this.BrokerLocalOrderID = copythis.BrokerLocalOrderID;
            this.BrokerRemoteOrderID = copythis.BrokerRemoteOrderID;

            this.OrderSeq = copythis.OrderSeq;
            this.OrderRef = copythis.OrderRef;
            this.OrderSysID = copythis.OrderSysID;
            this.SessionIDi = copythis.SessionIDi;
            this.FrontIDi = copythis.FrontIDi;
            this.RequestID = copythis.RequestID;

            this.FatherBreed = copythis.FatherBreed;
            this.FatherID = copythis.FatherID;
            this.Breed = copythis.Breed;
            this.Settled = copythis.Settled;
            this.SettleDay = copythis.SettleDay;

            this.CopyID = copythis.CopyID + 1;
        }

        public OrderImpl(string sym, bool side, int size, decimal p, decimal s, string c, int time, int date)
        {
            this.Symbol = sym;
            this.Side = side;
            this.Size = System.Math.Abs(size) * (side ? 1 : -1);
            this.LimitPrice = p;
            this.StopPrice = s;
            this.Comment = c;
            this.Time = time;
            this.Date = date;
            this.TotalSize = this.Size;
        }
        public OrderImpl(string sym, bool side, int size, decimal p, decimal s, string c, int time, int date, long orderid)
        {
            this.Symbol = sym;
            this.Side = side;
            this.Size = System.Math.Abs(size) * (side ? 1 : -1);
            this.LimitPrice = p;
            this.StopPrice = s;
            this.Comment = c;
            this.Time = time;
            this.Date = date;
            this.id = orderid;
            this.TotalSize = this.Size;
        }
        public OrderImpl(string sym, bool side, int size)
        {
            this.Symbol = sym;
            this.Side = side;
            this.LimitPrice = 0;
            this.StopPrice = 0;
            this.Comment = "";
            this.Time = 0;
            this.Date = 0;
            this.Size = System.Math.Abs(size) * (side ? 1 : -1);
            this.TotalSize = this.Size;
        }
        public OrderImpl(string sym, bool side, int size, string c)
        {
            this.Symbol = sym;
            this.Side = side;
            this.LimitPrice = 0;
            this.StopPrice = 0;
            this.Comment = c;
            this.Time = 0;
            this.Date = 0;
            this.Size = System.Math.Abs(size) * (side ? 1 : -1);
            this.TotalSize = this.Size;
        }
        
        public OrderImpl(string sym, int size)
        {
            this.Symbol = sym;
            this.Side = size > 0;
            this.LimitPrice = 0;
            this.StopPrice = 0;
            this.Comment = "";
            this.Time = 0;
            this.Date = 0;
            this.Size = System.Math.Abs(size) * (Side ? 1 : -1);
            this.TotalSize = this.Size;
        }
        #endregion


        #region Fill section

        /// <summary>
        /// 集合竞价方式成交该委托
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public bool FillAuction(Tick t)
        {
            // //合约不一致直接返回
            // if (t.Symbol != oSymbol.Symbol) return false;
            // //买入 委托价格大于等于开盘价 或者 卖出 委托价格小于等于开盘价
            // if ((isLimit && Side && (t.Open <= LimitPrice)) // buy limit
            //     || (isLimit && !Side && (t.Open >= LimitPrice))// sell limit
            //     )
            // {
            //     this.xPrice = t.Open;//开盘价成交
            //     this.xSize = UnsignedSize;//所有委托数量
            //     this.xSize *= Side ? 1 : -1;
            //     this.xTime = t.Time != 0 ? t.Time : Util.ToTLTime();
            //     this.xDate = t.Date != 0 ? t.Date : Util.ToTLDate();
            //     return true;
            // }
            return false;
        }

        /// <summary>
        /// Fills this order with a tick(trade price)
        /// 用最新成交价去成交一个委托
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Fill(Tick t) { return Fill(t, false); }
        public bool Fill(Tick t, bool fillOPG)
        {
            // if (!t.IsTrade()) return false;//fill with trade 
            // if (t.Symbol != oSymbol.Symbol) return false;
            // if (!fillOPG && TimeInForce == QSEnumTimeInForce.OPG) return false;
            // if ((isLimit && Side && (t.Trade <= LimitPrice)) // buy limit
            //     || (isLimit && !Side && (t.Trade >= LimitPrice))// sell limit
            //     || (isStop && Side && (t.Trade >= StopPrice)) // buy stop
            //     || (isStop && !Side && (t.Trade <= StopPrice)) // sell stop
            //     || isMarket)
            // {
            //     this.xPrice = t.Trade;
            //     this.xSize = t.Size >= UnsignedSize ? UnsignedSize : t.Size;
            //     this.xSize *= Side ? 1 : -1;
            //     this.xTime = t.Time != 0 ? t.Time : Util.ToTLTime();
            //     this.xDate = t.Date != 0 ? t.Date : Util.ToTLDate();
            //     return true;
            // }
            return false;
        }


        /// <summary>
        /// fill against bid and ask rather than trade
        /// 
        /// </summary>
        /// <param name="k"></param>
        /// <param name="smart"></param>
        /// <param name="fillOPG"></param>
        /// <returns></returns>
        public bool Fill(Tick k, bool bidask, bool fillOPG,bool fillall=false,int minFillSize=0)
        {
            // //如果不使用askbid来fill trade我们就使用成交价格来fill
            // if (!bidask)
            //     return Fill(k, fillOPG);
            // // buyer has to match with seller and vice verca利用ask,bid来成交Order
            // bool ok = Side ? k.HasAsk() : k.HasBid();
            // if (!ok) return false;
            // //debug("got here 1");
            // decimal p = Side ? k.AskPrice : k.BidPrice;
            // //获得对应的ask bid size大小用于fill
            // int s=0;
            // if(this.SecurityType ==SecurityType.STK)
            //     s = Side ? k.StockAskSize : k.StockBidSize;
            // else
            //     s = Side ? k.AskSize : k.BidSize;
            // if (k.Symbol != oSymbol.Symbol) return false;
            // if (!fillOPG && TimeInForce == QSEnumTimeInForce.OPG) return false;
            // if ((isLimit && Side && (p <= LimitPrice)) // buy limit
            //     || (isLimit && !Side && (p >= LimitPrice))// sell limit
            //     || (isStop && Side && (p >= StopPrice)) // buy stop
            //     || (isStop && !Side && (p <= StopPrice)) // sell stop
            //     || isMarket)
            // {
            //     this.xPrice = p;
            //     if (fillall)//成交所有
            //     {
            //         this.xSize = UnsignedSize * (Side ? 1 : -1);
            //     }
            //     else//根据盘口数量生成成交数量
            //     {
            //         s = s > minFillSize ? s : minFillSize;
            //         this.xSize = (s >= UnsignedSize ? UnsignedSize : s) * (Side ? 1 : -1);
            //     }
            //     this.xTime = k.Time != 0 ? k.Time : Util.ToTLTime();
            //     this.xDate = k.Date != 0 ? k.Date : Util.ToTLDate();
            //     return true;
            // }
            return false;
        }
        /// <summary>
        /// fill against bid and ask rather than trade
        /// </summary>
        /// <param name="k"></param>
        /// <param name="OPG"></param>
        /// <returns></returns>
        public bool FillBidAsk(Tick k, bool OPG)
        {
            return Fill(k, true, OPG);
        }
        /// <summary>
        /// fill against bid and ask rather than trade
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public bool FillBidAsk(Tick k)
        {
            return Fill(k, true, false);
        }

        /// <summary>
        /// Try to fill incoming order against this order.  If orders match.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>order can be cast to valid Trade and function returns true.  Otherwise, false</returns>
        public bool Fill(Order o)
        {
            // sides must match
            if (Side == o.Side) return false;
            // orders must be valid
            if (!o.isValid || !this.isValid) return false;
            // acounts must be different
            if (o.Account == Account) return false;
            if ((isLimit && Side && (o.LimitPrice <= LimitPrice)) // buy limit cross
                || (isLimit && !Side && (o.LimitPrice >= LimitPrice))// sell limit cross
                || (isStop && Side && (o.LimitPrice >= StopPrice)) // buy stop
                || (isStop && !Side && (o.LimitPrice <= StopPrice)) // sell stop
                || isMarket)
            {
                this.xPrice = o.isLimit ? o.LimitPrice : o.StopPrice;
                if (xPrice == 0) xPrice = isLimit ? LimitPrice : StopPrice;
                this.xSize = o.UnsignedSize >= UnsignedSize ? UnsignedSize : o.UnsignedSize;
                this.xTime = o.Time;
                this.xDate = o.Date;
                return isFilled;
            }
            return false;
        }

        #endregion


        /// <summary>
        /// Serialize order as a string
        /// </summary>
        /// <returns></returns>
        public static string Serialize(Order o)
        {
            if (o == null) return string.Empty;
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(o.Symbol);
            sb.Append(d);
            sb.Append(o.Side ? "true" : "false");
            sb.Append(d);
            sb.Append(o.TotalSize.ToString());
            sb.Append(d);
            sb.Append((o.UnsignedSize * (o.Side ? 1 : -1)).ToString());
            sb.Append(d);
            sb.Append(o.LimitPrice.ToString());
            sb.Append(d);
            sb.Append(o.StopPrice.ToString());
            sb.Append(d);
            sb.Append(o.Comment);
            sb.Append(d);
            sb.Append(o.Exchange);
            sb.Append(d);
            sb.Append(o.Account);
            sb.Append(d);
            sb.Append(o.SecurityType.ToString());
            sb.Append(d);
            sb.Append(o.Currency.ToString());
            sb.Append(d);
            sb.Append("unused");
            sb.Append(d);
            sb.Append(o.id.ToString());
            sb.Append(d);
            sb.Append(o.TimeInForce);
            sb.Append(d);
            sb.Append(o.Date.ToString());
            sb.Append(d);
            sb.Append(o.Time.ToString());
            sb.Append(d);
            sb.Append(o.FilledSize.ToString());
            sb.Append(d);
            sb.Append(o.trail.ToString());
            sb.Append(d);
            sb.Append(o.Broker);
            sb.Append(d);
            sb.Append(o.BrokerRemoteOrderID);
            sb.Append(d);
            sb.Append(o.BrokerLocalOrderID);
            sb.Append(d);
            sb.Append(o.Status.ToString());
            sb.Append(d);
            sb.Append(o.OffsetFlag.ToString());
            sb.Append(d);
            sb.Append(o.OrderRef);
            sb.Append(d);
            sb.Append(o.ForceClose.ToString());
            sb.Append(d);
            sb.Append(o.HedgeFlag);
            sb.Append(d);
            sb.Append(o.OrderSeq.ToString());
            sb.Append(d);
            sb.Append(o.OrderSysID);
            sb.Append(d);
            sb.Append(o.ForceCloseReason);
            sb.Append(d);
            sb.Append(o.FrontIDi);
            sb.Append(d);
            sb.Append(o.SessionIDi);
            sb.Append(d);
            sb.Append(o.RequestID);
            return sb.ToString();
        }

        /// <summary>
        /// Deserialize string to Order
        /// 委托解析不包含oSymbol定义,只能获得基本的相关信息
        /// </summary>
        /// <returns></returns>
        public new static Order Deserialize(string message)
        {
            if (string.IsNullOrEmpty(message)) return null;
            Order o = null;
            string[] rec = message.Split(','); // get the record
            if (rec.Length < 17) throw new InvalidOrder();

            bool side = Convert.ToBoolean(rec[(int)OrderField.Side]);
            int size = Math.Abs(Convert.ToInt32(rec[(int)OrderField.Size])) * (side ? 1 : -1);
            decimal oprice = Convert.ToDecimal(rec[(int)OrderField.Price], System.Globalization.CultureInfo.InvariantCulture);
            decimal ostop = Convert.ToDecimal(rec[(int)OrderField.Stop], System.Globalization.CultureInfo.InvariantCulture);
            string sym = rec[(int)OrderField.Symbol];
            int totalsize = int.Parse(rec[(int)OrderField.TotalSize]);
            o = new OrderImpl(sym, side, size);

            o.TotalSize = totalsize;

            o.LimitPrice = oprice;
            o.StopPrice = ostop;
            o.Comment = rec[(int)OrderField.Comment];
            o.Account = rec[(int)OrderField.Account];
            o.Exchange = rec[(int)OrderField.Exchange];
            //o.LocalSymbol = rec[(int)OrderField.LocalSymbol];
            o.Currency = (CurrencyType)Enum.Parse(typeof(CurrencyType), rec[(int)OrderField.Currency]);
            o.SecurityType = (SecurityType)Enum.Parse(typeof(SecurityType), rec[(int)OrderField.Security]);
            o.id = Convert.ToInt64(rec[(int)OrderField.OrderID]);
            o.TimeInForce = (QSEnumTimeInForce)Enum.Parse(typeof(QSEnumTimeInForce), rec[(int)OrderField.OrderTIF]);
            decimal trail = 0;
            if (decimal.TryParse(rec[(int)OrderField.Trail], System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out trail))
                o.trail = trail;
            o.Date = Convert.ToInt32(rec[(int)OrderField.oDate]);
            o.Time = Convert.ToInt32(rec[(int)OrderField.oTime]);
            o.Broker = rec[(int)OrderField.Broker];
            o.BrokerRemoteOrderID = rec[(int)OrderField.BrokerKey];
            o.BrokerLocalOrderID = rec[(int)OrderField.LocalID];
            o.Status = (QSEnumOrderStatus)Enum.Parse(typeof(QSEnumOrderStatus), rec[(int)OrderField.Status]);
            int f=0;
            int.TryParse(rec[(int)OrderField.oFilled],out f);
            o.FilledSize = f;

            o.OffsetFlag = (QSEnumOffsetFlag)Enum.Parse(typeof(QSEnumOffsetFlag), rec[(int)OrderField.PostFlag]);

            o.OrderRef = rec[(int)OrderField.OrderRef];
            o.ForceClose = bool.Parse(rec[(int)OrderField.ForceClose]);
            o.HedgeFlag = (QSEnumHedgeFlag)Enum.Parse(typeof(QSEnumHedgeFlag),rec[(int)OrderField.HedgeFlag]);
            o.OrderSeq = int.Parse(rec[(int)OrderField.OrderSeq]);
            o.OrderSysID = rec[(int)OrderField.OrderExchID];
            o.ForceCloseReason = rec[(int)OrderField.ForceReason];
            if (rec.Length > 29)
            {
                o.FrontIDi = int.Parse(rec[(int)OrderField.FrontID]);
                o.SessionIDi = int.Parse(rec[(int)OrderField.SessionID]);
                o.RequestID = int.Parse(rec[(int)OrderField.RequestID]);
            }
            return o;
        }

        public static long Unique { get { return DateTime.Now.Ticks; } }
    }



}
