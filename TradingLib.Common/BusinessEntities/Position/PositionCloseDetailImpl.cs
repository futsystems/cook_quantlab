using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    /// <summary>
    /// 平仓明细
    /// </summary>
    public class PositionCloseDetailImpl: PositionCloseDetail
    {

        public PositionCloseDetailImpl(PositionDetail pd,Trade f,int closesize)
        {
            //生成平仓明细数据
            //设定持仓主体信息
            this.Account = pd.Account;
            this.Symbol = pd.Symbol;
            this.oSymbol = pd.oSymbol;
            this.Side = pd.Side;
            this.IsCloseYdPosition = pd.IsHisPosition;//是否是平昨仓
            this.LastSettlementPrice = pd.LastSettlementPrice;//
            //?tradingday settleday

            //设定开仓时间
            this.OpenDate = pd.OpenDate;
            this.OpenTime = pd.OpenTime;
            this.OpenTradeID = pd.TradeID;
            this.OpenPrice = pd.OpenPrice;

            //设定平仓时间
            this.CloseDate = f.xDate;
            this.CloseTime = f.xTime;
            this.CloseTradeID = f.TradeID;
            this.ClosePrice = f.xPrice;
            this.CloseVolume = closesize;
            //this.Domain_ID = pd.Domain_ID;
            //this.Broker=
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PositionCloseDetailImpl()
        {
            this.Account = string.Empty;
            this.Settleday = 0;
            this.Side = true;

            this.OpenDate = 0;
            this.OpenTime = 0;
            this.OpenTradeID = string.Empty;
            this.OpenPrice = 0;

            this.CloseDate = 0;
            this.CloseTime = 0;
            this.CloseTradeID = string.Empty;
            this.ClosePrice = 0;
            this.CloseVolume = 0;

            this.IsCloseYdPosition = false;
            this.LastSettlementPrice = 0;
            this.CloseAmount = 0;
            this.CloseProfitByDate = 0;
            this.CloseProfitByTrade = 0;
            this.ClosePointByDate = 0;

            this.oSymbol = null;
            this.Exchange = string.Empty;
            this.Symbol = string.Empty;
            this.SecCode = string.Empty;

            this.Broker = string.Empty;//默认broker为空
            this.Breed = QSEnumOrderBreedType.ACCT;//默认为分帐户侧
            this.Domain_ID = 0;
        }

        /// <summary>
        /// 交易帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 平仓所在结算日
        /// </summary>
        public int Settleday { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        public bool Side { get; set; }


        /// <summary>
        /// 平昨仓还是平今仓
        /// </summary>
        public bool IsCloseYdPosition { get; set; }

        /// <summary>
        /// 昨结算价
        /// </summary>
        public decimal LastSettlementPrice { get; set; }



        #region 开平属性
        /// <summary>
        /// 开仓日期
        /// </summary>
        public int OpenDate { get; set; }

        /// <summary>
        /// 开仓时间
        /// </summary>
        public int OpenTime { get; set; }

        /// <summary>
        /// 开仓成交编号
        /// </summary>
        public string OpenTradeID { get; set; }

        /// <summary>
        /// 开仓价格
        /// </summary>
        public decimal OpenPrice { get; set; }

        /// <summary>
        /// 平仓日期
        /// </summary>
        public int CloseDate { get; set; }

        /// <summary>
        /// 平仓时间
        /// </summary>
        public int CloseTime { get; set; }

        /// <summary>
        /// 平仓成交编号
        /// </summary>
        public string CloseTradeID { get; set; }

        /// <summary>
        /// 平仓价格
        /// </summary>
        public decimal ClosePrice { get; set; }

        /// <summary>
        /// 平仓量
        /// </summary>
        public int CloseVolume { get; set; }

        #endregion

        

        decimal? _closeProfitByDate;
        /// <summary>
        /// 盯市平仓盈亏金额
        /// 平当日仓 (开仓-平仓)*手数*乘数
        /// </summary>
        public decimal CloseProfitByDate 
        {
            get
            {
                if (_closeProfitByDate != null)
                    return (decimal)_closeProfitByDate;
                else
                {
                    //平今仓
                    decimal profit = 0;
                    if (!IsCloseYdPosition)
                    {
                        //今仓 平仓盈亏为平仓价-平仓价
                        profit = (ClosePrice - OpenPrice) * CloseVolume * oSymbol.Multiple * (Side ? 1 : -1);
                    }
                    else
                    {
                        //昨仓 平仓盈亏为昨结算-平仓价
                        profit = (ClosePrice - LastSettlementPrice) * CloseVolume * oSymbol.Multiple * (Side ? 1 : -1);
                    }
                    return profit;
                }
            }

            set { _closeProfitByDate = value; }
        }

        decimal? _closeProfitByTrade;
        /// <summary>
        /// 逐笔平仓盈亏金额
        /// </summary>
        public decimal CloseProfitByTrade 
        {
            get
            {
                if (_closeProfitByTrade != null)
                    return (decimal)_closeProfitByTrade;
                else
                {
                    //逐笔平仓盈亏 平仓价格-开仓价格
                    return (ClosePrice - OpenPrice) * CloseVolume * oSymbol.Multiple * (Side ? 1 : -1);
                }
            }
            set { _closeProfitByTrade = value; }
        }

        decimal? _closePointByDate;
        /// <summary>
        /// 盯市平仓盈亏点数
        /// </summary>
        public decimal ClosePointByDate 
        {
            get
            {
                //平今仓
                decimal profit = 0;
                if (!IsCloseYdPosition)
                {
                    //今仓 平仓盈亏为平仓价-平仓价
                    profit = (ClosePrice - OpenPrice) * CloseVolume * (Side ? 1 : -1);
                }
                else
                {
                    //昨仓 平仓盈亏为昨结算-平仓价
                    profit = (ClosePrice - LastSettlementPrice) * CloseVolume * (Side ? 1 : -1);
                }
                return profit;
            }
            set { _closePointByDate = value; }
        }

        decimal? _closePointByTrade;

        /// <summary>
        /// 逐笔平仓盈亏点数
        /// </summary>
        public decimal ClosePointByTrade
        {
            get
            {
                if(_closePointByTrade!=null)
                    return (decimal)_closePointByTrade;
                return (ClosePrice - OpenPrice) * CloseVolume * (Side ? 1 : -1);
            }
            set { _closePointByTrade = value; }
        }

        decimal? _closeamount;
        /// <summary>
        /// 平仓金额
        /// </summary>
        public decimal CloseAmount
        {
            get
            {
                if (_closeamount != null)
                    return (decimal)_closeamount;
                else
                { 
                    return this.CloseVolume * this.ClosePrice * oSymbol.Multiple;
                }
            }

            set
            {
                _closeamount = value;
            }
        }

        /// <summary>
        /// 合约信息
        /// </summary>
        //[NoJsonExportAttr()]
        [Newtonsoft.Json.JsonIgnore]
        public Symbol oSymbol { get; set; }
        string _exchange = string.Empty;
        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange
        {
            get
            {

                return oSymbol != null ? oSymbol.SecurityFamily.Exchange.EXCode : _exchange;
            }
            set
            {
                _exchange = value;
            }
        }

        string _symbol = string.Empty;
        public string Symbol
        {
            get
            {
                return oSymbol != null ? oSymbol.Symbol : _symbol;
            }
            set
            {
                _symbol = value;
            }
        }

        string _seccode = string.Empty;
        public string SecCode
        {
            get
            {
                return oSymbol != null ? oSymbol.SecurityFamily.Code : _seccode;
            }
            set
            {
                _seccode = value;
            }
        }


        /// <summary>
        /// 接口Token如果是接口侧的平仓明细则有BrokerToken字段
        /// 分帐户侧没有Broker
        /// </summary>
        public string Broker { get; set; }

        /// <summary>
        /// 数据来源
        /// 1.分帐户侧
        /// 2.接口侧
        /// 3.路由侧
        /// </summary>
        public QSEnumOrderBreedType Breed { get; set; }


        /// <summary>
        /// 域ID
        /// </summary>
        public int Domain_ID { get; set; }


        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string Serialize(PositionCloseDetail close)
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(close.Account);//0
            sb.Append(d);
            sb.Append(close.Settleday);//1
            sb.Append(d);
            sb.Append(close.Side);//2
            sb.Append(d);
            sb.Append(close.OpenDate);//3
            sb.Append(d);
            sb.Append(close.OpenTime);//4
            sb.Append(d);
            sb.Append(close.OpenTradeID);//5
            sb.Append(d);
            sb.Append(close.OpenPrice);//6
            sb.Append(d);
            sb.Append(close.CloseDate);//7
            sb.Append(d);
            sb.Append(close.CloseTime);//8
            sb.Append(d);
            sb.Append(close.CloseTradeID);//9
            sb.Append(d);
            sb.Append(close.ClosePrice);//10
            sb.Append(d);
            sb.Append(close.CloseVolume);//11
            sb.Append(d);
            sb.Append(close.IsCloseYdPosition);//12
            sb.Append(d);
            sb.Append(close.LastSettlementPrice);//13
            sb.Append(d);
            sb.Append(close.CloseAmount);//14
            sb.Append(d);
            sb.Append(close.ClosePointByDate);//15
            sb.Append(d);
            sb.Append(close.CloseProfitByTrade);//16
            sb.Append(d);
            sb.Append(close.ClosePointByDate);//17
            sb.Append(d);
            sb.Append(close.ClosePointByTrade);
            sb.Append(d);
            sb.Append(close.Exchange);//18
            sb.Append(d);
            sb.Append(close.Symbol);//19
            sb.Append(d);
            sb.Append(close.SecCode);//20
            sb.Append(d);
            sb.Append(close.Broker);//21
            sb.Append(d);
            sb.Append(close.Breed);//22
            return sb.ToString();
        }

        public new static PositionCloseDetail Deserialize(string message)
        {
            string[] rec = message.Split(',');
            PositionCloseDetailImpl close = new PositionCloseDetailImpl();
            close.Account = rec[0];
            close.Settleday = int.Parse(rec[1]);
            close.Side = bool.Parse(rec[2]);

            close.OpenDate = int.Parse(rec[3]);
            close.OpenTime = int.Parse(rec[4]);
            close.OpenTradeID = rec[5];
            close.OpenPrice = decimal.Parse(rec[6]);

            close.CloseDate = int.Parse(rec[7]);
            close.CloseTime = int.Parse(rec[8]);
            close.CloseTradeID = rec[9];
            close.ClosePrice = decimal.Parse(rec[10]);
            close.CloseVolume = int.Parse(rec[11]);
            close.IsCloseYdPosition = bool.Parse(rec[12]);
            close.LastSettlementPrice = decimal.Parse(rec[13]);
            close.CloseAmount = decimal.Parse(rec[14]);
            close.CloseProfitByDate = decimal.Parse(rec[15]);
            close.CloseProfitByTrade = decimal.Parse(rec[16]);
            close.ClosePointByDate = decimal.Parse(rec[17]);
            close.ClosePointByTrade = decimal.Parse(rec[18]);
            close.Exchange = rec[19];
            close.Symbol = rec[20];
            close.SecCode = rec[21];
            close.Broker = rec[22];
            close.Breed = (QSEnumOrderBreedType)Enum.Parse(typeof(QSEnumOrderBreedType), rec[23]);
            return close;

        }


        public override string ToString()
        {
            return Util.PrintObj(this);
            //return string.Format("{0} {1} {2} Open:{3} {4} {5} {6} Close:{7} {8} {9} {10} Symbol:{11} ",Account,Settleday,Side,OpenDate,OpenTime,OpenTradeID,OpenPrice,CloseDate,CloseTime,CloseTradeID,ClosePrice)
        }
    }
}
