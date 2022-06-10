using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// A trade or execution of a stock order.  Also called a fill.
    /// </summary>
    public class TradeImpl : Trade
    {
        #region 基础属性
        long _id = 0;//对应的委托编号
        public long id { get { return _id; } set { _id = value; } }

        string _acct = string.Empty;
        /// <summary>
        /// 交易账号
        /// </summary>
        public string Account { get { return _acct; } set { _acct = value; } }

        int _xdate = 0;
        /// <summary>
        /// 成交日期
        /// </summary>
        public int xDate { get { return _xdate; } set { _xdate = value; } }

        int _xtime = 0;
        /// <summary>
        /// 成交时间
        /// </summary>
        public int xTime { get { return _xtime; } set { _xtime = value; } }


        string _sym = string.Empty;
        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get { return _sym; } set { _sym = value; } }

        string _localsymbol = string.Empty;
        /// <summary>
        /// 本地合约编号
        /// </summary>
        public string LocalSymbol { get { return _localsymbol; } set { _localsymbol = value; } }

        Symbol _osymbol = null;
        /// <summary>
        /// 合约对象
        /// </summary>
        public Symbol oSymbol { get { return _osymbol; } set { _osymbol = value; } }


        bool _side = true;
        /// <summary>
        /// 成交方向
        /// </summary>
        public bool Side { get { return _side; } set { _side = value; } }

        int _xsize = 0;
        /// <summary>
        /// 成交数量
        /// </summary>
        public int xSize { get { return _xsize; } set { _xsize = value; } }

        /// <summary>
        /// 数量绝对值
        /// </summary>
        public int UnsignedSize { get { return Math.Abs(_xsize); } }

        decimal _xprice = 0;
        /// <summary>
        /// 成交价格
        /// </summary>
        public decimal xPrice { get { return _xprice; } set { _xprice = value; } }

        QSEnumOffsetFlag _posflag = QSEnumOffsetFlag.UNKNOWN;
        /// <summary>
        /// 开平标识
        /// </summary>
        public QSEnumOffsetFlag OffsetFlag { get { return _posflag; } set { _posflag = value; } }

        QSEnumHedgeFlag _hedgeflag = QSEnumHedgeFlag.Speculation;
        /// <summary>
        /// 投机 套保标识
        /// </summary>
        public QSEnumHedgeFlag HedgeFlag { get { return _hedgeflag; } set { _hedgeflag = value; } }

        decimal _commission = -1;//默认手续费为-1,若为-1表明没有计算过手续费
        /// <summary>
        /// 手续费
        /// </summary>
        public decimal Commission { get { return _commission; } set { _commission = value; } }

        decimal _stamptax = 0;
        /// <summary>
        /// 印花税
        /// </summary>
        public decimal StampTax { get { return _stamptax; } set { _stamptax = value; } }

        decimal _transferfee = 0;
        /// <summary>
        /// 过户费
        /// </summary>
        public decimal TransferFee { get { return _transferfee; } set { _transferfee = value; } }


        decimal _profit = 0;
        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public decimal Profit { get { return _profit; } set { _profit = value; } }

        readonly List<PositionCloseDetail> _closedetails = new List<PositionCloseDetail>();
 	
        public List<PositionCloseDetail> CloseDetails { get { return _closedetails; } }

        #endregion


        #region 其余成交属性
        //如果存在oSymbol则从oSymnbol获得对应需要的数据
        string _ex = string.Empty;
        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange { get { return oSymbol != null ? oSymbol.SecurityFamily.Exchange.EXCode : _ex; } set { _ex = value; } }

        SecurityType type = SecurityType.NIL;
        /// <summary>
        /// 品种
        /// </summary>
        public SecurityType SecurityType    { get { return oSymbol != null ? oSymbol.SecurityFamily.Type : type;} set { type = value; } }

        string _securitycode = string.Empty;
        /// <summary>
        /// 品种code
        /// </summary>
        public string SecurityCode { get { return oSymbol != null ? oSymbol.SecurityFamily.Code : _securitycode; } set { _securitycode = value; } }

        CurrencyType cur = CurrencyType.USD;
        /// <summary>
        /// 货币
        /// </summary>
        public CurrencyType Currency { get { return oSymbol != null ? oSymbol.SecurityFamily.Currency : cur; } set { cur = value; } }

        int _settleday = 0;
        /// <summary>
        /// 结算日
        /// </summary>
        public int SettleDay { get { return _settleday; } set { _settleday = value; } }

        bool _settled = false;
        /// <summary>
        /// 结算标识
        /// </summary>
        public bool Settled { get { return _settled; } set { _settled = value; } }
        #endregion

        #region 判定属性
        /// <summary>
        /// 判断有效成交
        /// </summary>
        public virtual bool isValid { get { return (xSize != 0) && (xPrice != 0) && (xTime + xDate != 0) && (Symbol != null) && (Symbol != ""); } }

        /// <summary>
        /// 判定是否有成交
        /// Order继承自Trade 通过xprice和xsize进行判断是否有成交 xprice xsize均不为0 则表明有有效成交
        /// </summary>
        public bool isFilled { get { return (xPrice * xSize) != 0; } }

        /// <summary>
        /// 是否是开仓
        /// </summary>
        public bool IsEntryPosition
        {
            get
            {
                if (this.OffsetFlag == QSEnumOffsetFlag.OPEN)
                {
                    return true;
                }
                else if (this.OffsetFlag == QSEnumOffsetFlag.UNKNOWN)
                {
                    throw new QSUnknownOffsetException();
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 持仓方向
        /// </summary>
        public bool PositionSide
        {
            get
            {
                bool entrypostion = IsEntryPosition;
                if ((entrypostion && this.Side) || ((!entrypostion) && (!this.Side)))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        #endregion

        QSEnumPosOperation _posop;
        public QSEnumPosOperation PositionOperation { get { return _posop; } set { _posop = value; } }


        #region Broker端属性
        string _broker = string.Empty;
        /// <summary>
        /// Broker标识
        /// </summary>
        public string Broker { get { return _broker; } set { _broker = value; } }

        /// <summary>
        /// Broker端 本地委托编号
        /// 系统有2种方式将成交与委托联系起来
        /// 1.近端委托编号进行关联 自己按一定方式维护唯一编号,成交端发送成交回报时按该编号关联委托
        /// 2.远端委托编号进行关联 由成交端在委托回报中设定远端委托编号,成交端发送成交回报时候按该编号关联委托
        /// </summary>
        string _brokerLocalOrderID = string.Empty;
        public string BrokerLocalOrderID { get { return _brokerLocalOrderID; } set { _brokerLocalOrderID = value; } }

        /// <summary>
        /// Broker端 远端委托编号
        /// </summary>
        string _brokerRemoteOrderID = string.Empty;
        public string BrokerRemoteOrderID { get { return _brokerRemoteOrderID; } set { _brokerRemoteOrderID = value; } }

        /// <summary>
        /// Broker端的成交编号 由Broker按一定规则赋值
        /// </summary>
        string _brokerTradeID = string.Empty;
        public string BrokerTradeID { get { return _brokerTradeID; } set { _brokerTradeID = value; } }
        #endregion

        #region 分帐户端属性

        string _tradeID = string.Empty;
        /// <summary>
        /// 成交编号
        /// </summary>
        public string TradeID { get { return _tradeID; } set { _tradeID = value; } }

        int _orderseq = 0;
        /// <summary>
        /// 委托流水号
        /// </summary>
        public int OrderSeq { get { return _orderseq; } set { _orderseq = value; } }

        string _orderref = "";//对应的委托引用
        /// <summary>
        /// 客户端委托引用
        /// </summary>
        public string OrderRef { get { return _orderref; } set { _orderref = value; } }

        string _orderSysID = "";
        /// <summary>
        /// 委托交易所编号
        /// </summary>
        public string OrderSysID { get { return _orderSysID; } set { _orderSysID = value; } }

        #endregion


        #region 委托分解属性

        QSEnumOrderBreedType? _fatherbreed = null;
        
        /// <summary>
        /// 分解父源
        /// </summary>
        public QSEnumOrderBreedType? FatherBreed { get { return _fatherbreed; } set { _fatherbreed = value; } }
        
        
        long _fatherID = 0;
        /// <summary>
        /// 父委托编号
        /// </summary>
        public long FatherID { get { return _fatherID; } set { _fatherID = value; } }

        QSEnumOrderBreedType _breed= QSEnumOrderBreedType.ACCT;
        /// <summary>
        /// 委托分解源
        /// </summary>
        public QSEnumOrderBreedType Breed { get { return _breed; } set { _breed = value; } }

        /// <summary>
        /// 域ID
        /// </summary>
        //public int Domain_ID { get; set; }
        #endregion

        #region 成交构造函数
        public TradeImpl() { }
        public TradeImpl(string symbol, decimal fillprice, int fillsize) : this(symbol, fillprice, fillsize, DateTime.Now) { }
        public TradeImpl(string sym, decimal fillprice, int fillsize, DateTime tradedate) : this(sym, fillprice, fillsize, Util.ToTLDate(tradedate), Util.DT2FT(tradedate)) { }
        public TradeImpl(string sym, decimal fillprice, int fillsize, int filldate, int filltime)
        {
            if (sym != null) Symbol = sym;
            if ((fillsize == 0) || (fillprice == 0)) throw new Exception("Invalid trade: Zero price or size provided.");
            xTime = filltime;
            xDate = filldate;
            xSize = fillsize;
            xPrice = fillprice;
            Side = (fillsize > 0);
            //Domain_ID = 0;
        }



        public TradeImpl(Trade copytrade)
        {
            // copy constructor, for copying using by-value (rather than by default of by-reference)

            this.id = copytrade.id;
            this.Account = copytrade.Account;
            this.xDate = copytrade.xDate;
            this.xTime = copytrade.xTime;

            this.Symbol = copytrade.Symbol;
            this.LocalSymbol = copytrade.LocalSymbol;
            this.oSymbol = copytrade.oSymbol;//成交复制时 同时复制对应的合约对象 所有委托引用同一份合约对象

            this.Side = copytrade.Side;
            this.xSize = copytrade.xSize;
            this.xPrice = copytrade.xPrice;
            this.OffsetFlag = copytrade.OffsetFlag;
            this.HedgeFlag = copytrade.HedgeFlag;
            this.Commission = copytrade.Commission;
            this.StampTax = copytrade.StampTax;
            this.TransferFee = copytrade.TransferFee;
            this.Profit = copytrade.Profit;
            
            this.Exchange = copytrade.Exchange;
            this.SecurityType = copytrade.SecurityType;
            this.SecurityCode = copytrade.SecurityCode;
            this.Currency = copytrade.Currency;

            this.PositionOperation = copytrade.PositionOperation;

            this.Broker = copytrade.Broker;
            this.BrokerLocalOrderID = copytrade.BrokerLocalOrderID;
            this.BrokerRemoteOrderID = copytrade.BrokerRemoteOrderID;
            this.BrokerTradeID = copytrade.BrokerTradeID;

            this.OrderRef = copytrade.OrderRef;
            this.OrderSeq = copytrade.OrderSeq;
            this.OrderSysID = copytrade.OrderSysID;
            this.TradeID = copytrade.TradeID;

            this.FatherBreed = copytrade.FatherBreed;
            this.FatherID = copytrade.FatherID;
            this.Breed = copytrade.Breed;
            this.Settled = copytrade.Settled;
            this.SettleDay = copytrade.SettleDay;

            //this.Domain_ID = copytrade.Domain_ID;

        }
        #endregion



        /// <summary>
        /// Serialize trade as a string
        /// </summary>
        /// <returns></returns>
        public static string Serialize(Trade t)
        {
            if (t == null) return string.Empty;
            const char d = ',';
            StringBuilder sb = new StringBuilder();
            sb.Append(t.xDate.ToString()); sb.Append(d);
            sb.Append(t.xTime.ToString()); sb.Append(d);
            sb.Append(d);
            sb.Append(t.Symbol); sb.Append(d);
            sb.Append(t.Side.ToString()); sb.Append(d);
            sb.Append(t.xSize.ToString()); sb.Append(d);
            sb.Append(t.xPrice.ToString()); sb.Append(d);
            sb.Append(""); sb.Append(d);

            sb.Append(t.Account); sb.Append(d);//8
            sb.Append(t.SecurityType); sb.Append(d);
            sb.Append(t.Currency); sb.Append(d);
            sb.Append(t.LocalSymbol); sb.Append(d);
            sb.Append(t.id); sb.Append(d); //12
            sb.Append(t.Exchange); sb.Append(d);

            sb.Append(t.Broker); sb.Append(d);//14
            sb.Append(t.TradeID); sb.Append(d);
            sb.Append(t.Commission); sb.Append(d);
            sb.Append(t.PositionOperation.ToString()); sb.Append(d);
            sb.Append(t.Profit.ToString()); sb.Append(d);
            sb.Append(t.OrderRef); sb.Append(d);
            sb.Append(t.HedgeFlag); sb.Append(d);
            sb.Append(t.OrderSeq.ToString()); sb.Append(d);
            sb.Append(t.OrderSysID); sb.Append(d);
            sb.Append(t.OffsetFlag.ToString()); sb.Append(d);
            sb.Append(t.BrokerLocalOrderID); sb.Append(d);
            sb.Append(t.BrokerRemoteOrderID); sb.Append(d);//
            sb.Append(t.BrokerTradeID); sb.Append(d);
            sb.Append(t.StampTax); sb.Append(d);
            sb.Append(t.TransferFee); sb.Append(d);
            sb.Append(t.SecurityCode);


            return sb.ToString();
        }

        public override int GetHashCode()
        {
            int code = string.Format("{0}-{1}-{2}", this.Account, this.xDate, this.TradeID).GetHashCode();
            return code;
        }
        /// <summary>
        /// Deserialize string to Trade
        /// </summary>
        /// <returns></returns>
        public static Trade Deserialize(string message)
        {
            if (string.IsNullOrEmpty(message)) return null;
            Trade t = null;
            string[] rec = message.Split(',');
            if (rec.Length < 18) throw new InvalidTrade();
            bool side = Convert.ToBoolean(rec[(int)TradeField.Side]);
            int size = Convert.ToInt32(rec[(int)TradeField.Size], System.Globalization.CultureInfo.InvariantCulture);
            size = Math.Abs(size) * (side ? 1 : -1);
            decimal xprice = Convert.ToDecimal(rec[(int)TradeField.Price],System.Globalization.CultureInfo.InvariantCulture);
            string sym = rec[(int)TradeField.Symbol];

            t = new TradeImpl(sym, xprice, size);
            t.xDate = Convert.ToInt32(rec[(int)TradeField.xDate], System.Globalization.CultureInfo.InvariantCulture);
            t.xTime = Convert.ToInt32(rec[(int)TradeField.xTime], System.Globalization.CultureInfo.InvariantCulture);
            //t.Comment = rec[(int)TradeField.Comment];
            t.Account = rec[(int)TradeField.Account];
            t.LocalSymbol = rec[(int)TradeField.LocalSymbol];
            t.id = Convert.ToInt64(rec[(int)TradeField.ID], System.Globalization.CultureInfo.InvariantCulture);
            t.Exchange = rec[(int)TradeField.Exch];
            t.Currency = (CurrencyType)Enum.Parse(typeof(CurrencyType), rec[(int)TradeField.Currency]);
            t.SecurityType = (SecurityType)Enum.Parse(typeof(SecurityType), rec[(int)TradeField.Security]);
            t.Broker = rec[(int)TradeField.Broker];
            t.TradeID = rec[(int)TradeField.BrokerKey];
            t.Commission = decimal.Parse(rec[(int)TradeField.Commission]);
            t.PositionOperation = (QSEnumPosOperation)Enum.Parse(typeof(QSEnumPosOperation), rec[(int)TradeField.PositionOperatoin]);
            t.Profit = decimal.Parse(rec[(int)TradeField.Profit]);
            t.OrderRef = rec[(int)TradeField.OrderRef];
            t.HedgeFlag = (QSEnumHedgeFlag)Enum.Parse(typeof(QSEnumHedgeFlag),rec[(int)TradeField.HedgeFlag]);
            t.OrderSeq = int.Parse(rec[(int)TradeField.OrderSeq]);
            t.OrderSysID = rec[(int)TradeField.OrderExchID];
            t.OffsetFlag = (QSEnumOffsetFlag)Enum.Parse(typeof(QSEnumOffsetFlag),rec[(int)TradeField.OffsetFlag]);
            t.BrokerLocalOrderID = rec[(int)TradeField.BrokerLocalOrderID];
            t.BrokerRemoteOrderID = rec[(int)TradeField.BrokerRemoteOrderID];
            t.BrokerTradeID = rec[(int)TradeField.BrokerTradeID];
            t.StampTax = decimal.Parse(rec[(int)TradeField.StampTax]);
            t.TransferFee = decimal.Parse(rec[(int)TradeField.TransferFee]);
            t.SecurityCode = rec[(int)TradeField.SecurityCode];

            return t;
        }



       

    }


}
