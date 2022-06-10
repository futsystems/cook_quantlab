using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;


namespace TradingLib.Common
{
    /// <summary>
    /// 持仓明细
    /// 结算时通过历史持仓,交易帐户的成交数据分组获得开仓记录，平仓记录(汇总)
    /// 
    /// 持仓明细是以成交为单位的交易历史记录，每条开仓成交会形成一条持仓明细
    /// 有平仓成交时按照先开先平或者上期所的 平今 平昨指令，从对应的持仓明细列表中进行平仓
    /// 
    /// 历史持仓明细(隔夜持仓)
    /// 当日持仓明细 由当日开仓成交形成的持仓明细
    /// 
    /// 当有平仓成交时按照规则从对应的持仓明细中获得对应的持仓进行平仓
    /// 
    /// 
    /// 
    /// </summary>
    public class PositionDetailImpl:PositionDetail
    {
        /// <summary>
        /// 默认构造函数 用于从数据库加载持仓明细对象
        /// </summary>
        public PositionDetailImpl()
        {
            this.Account = string.Empty;
            this.Symbol = string.Empty;
            this.OpenDate = 0;
            this.OpenTime = 0;
            this.Side = true;
            this.Volume = 0;
            this.OpenPrice = 0;
            this.TradeID = string.Empty;
            this.IsHisPosition = false;
            this.LastSettlementPrice = 0M;
            this.SettlementPrice = 0M;
            this.CloseVolume = 0;

            this.HedgeFlag = string.Empty;
            this.oSymbol = null;
            this.Exchange = string.Empty;
            this.SecCode = string.Empty;
            this.Margin = 0M;
            this.PositionProfitByDate = 0;
            this.PositionProfitByTrade = 0;
            this.CloseProfitByDate = 0;
            this.CloseProfitByTrade = 0;

            //默认broker为空,数据来源为分帐户侧
            this.Broker = string.Empty;
            this.Breed = QSEnumOrderBreedType.ACCT;
            this.Domain_ID = 0;
        }

        /// <summary>
        /// 设定Position持仓对象
        /// 开仓成交生成新的持仓明细
        /// </summary>
        /// <param name="pos"></param>
        public PositionDetailImpl(Position pos)
        {
            Position = pos;

            this.Account = pos.Account;
            this.oSymbol = pos.oSymbol;
            this.Side = true;
            this.OpenDate = 0;
            this.OpenTime = 0;
            this.Volume = 0;
            this.OpenPrice = 0;
            this.TradeID = string.Empty;
            this.IsHisPosition = false;
            //this.LastSettlementPrice = 0M; 不能赋值LastSettlementPrice否则为永远为0，无法获得正常的昨日结算价，在持仓对象中产生的持仓明细 都包含有Position对象 通过取Position对象的LastSettlementPrice来获得昨日结算价
            
            this.CloseVolume = 0;
            this.CloseAmount = 0;
            this.CloseProfitByDate = 0;
            this.CloseProfitByTrade = 0;
            //this.Domain_ID = pos.Domain_ID;
        }

        /// <summary>
        /// 结算日 表明该持仓明细记录属于哪个结算日 如果为0 表明是今仓，在结算时保存到数据库时 赋上当交易日信息
        /// </summary>
        public int Settleday { get; set; }

        #region 开仓信息

        /// <summary>
        /// 是否是昨仓
        /// 从数据库加载的持仓就是历史持仓
        /// 由当天开仓成交生成的持仓就是今仓
        /// </summary>
        public  bool IsHisPosition { get; set; }

        /// <summary>
        /// 交易帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 成交编号
        /// </summary>
        public string TradeID { get; set; }

         /// <summary>
        /// 开仓日期
        /// </summary>
        public int OpenDate { get; set; }

        /// <summary>
        /// 开仓时间
        /// </summary>
        public int OpenTime { get; set; }

        /// <summary>
        /// 开仓价格 记录开仓时的开仓价
        /// </summary>
        public decimal OpenPrice { get; set; }


        /// <summary>
        /// 方向 多或空
        /// </summary>
        public bool Side { get; set; }
        #endregion

        /// <summary>
        /// 对应持仓信息
        /// </summary>
        Position Position { get; set; }






        decimal? _lastSelltementPrice;

        /// <summary>
        /// 昨结算价
        /// 如果对象时设定类型的对象则直接取值
        /// 持仓对象中产生的持仓明细 则通过Position对象来取值
        /// </summary>
        public decimal LastSettlementPrice
        {
            get
            {
                if (_lastSelltementPrice != null)
                    return (decimal)_lastSelltementPrice;
                else
                {
                    if (Position == null)
                    {
                        return this.OpenPrice;
                    }
                    else
                    {
                        if (Position.LastSettlementPrice == null)
                        {
                            return this.OpenPrice;
                        }
                        return (decimal)Position.LastSettlementPrice;
                    }
                }
            }
            set
            {
                _lastSelltementPrice = value;
            }
        }

        decimal? _settlementPrice;
        /// <summary>
        /// 今结算价
        /// 通过实现动态结算价 可以在不同情况下获得不同的结算价格
        /// 
        /// 用于计算逐笔盈亏或逐日盈亏
        /// 
        /// 开仓成交生成持仓明细
        /// 持仓未设定结算价 则SettlementPrice为持仓最新价 LastPrice
        /// 持仓设定结算价   则SettlementPrice为持仓结算价
        /// </summary>
        public decimal SettlementPrice 
        {
            get
            {
                if (_settlementPrice != null)
                    return (decimal)_settlementPrice;
                else
                { 
                    //通过Position对价格的引用获得结算价格
                    //如果持仓对象为空 则返回当前的开仓价格为结算价格
                    if (Position == null)
                        return this.OpenPrice;
                    else
                    {
                        //如果持仓对象没有今日结算价格 则返回当前持仓的最新价格 否则返回对应的结算价格
                        if (Position.SettlementPrice == null)
                            return Position.LastPrice;
                        else
                            return (decimal)Position.SettlementPrice;
                    }
                }
            }
            set { _settlementPrice = value; }
        
        }


       

        #region 合约基础类信息

        /// <summary>
        /// 投机套保标识
        /// </summary>
        public string HedgeFlag { get; set; }

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
        #endregion


        #region 由平仓时 进行累加的数据
        /// <summary>
        /// 数量
        /// </summary>
        public int Volume { get; set; }

        /// <summary>
        /// 平仓金额 记录当前交易日的平仓金额
        /// </summary>
        public decimal CloseAmount { get; set; }

        /// <summary>
        /// 平仓量 记录当前交易日的平仓数量 每次平仓产生时 平仓量累加
        /// </summary>
        public int CloseVolume { get; set; }

        /// <summary>
        /// 盯市平仓盈亏
        /// </summary>
        public decimal CloseProfitByDate {get;set;}
        
        /// <summary>
        /// 逐笔平仓盈亏
        /// </summary>
        public decimal CloseProfitByTrade { get; set; }
        #endregion


        #region 实时动态计算的数据

        decimal? _positionProfitByDate;
        /// <summary>
        /// 盯市浮动盈亏
        /// </summary>
        public decimal PositionProfitByDate 
        {
            get
            {
                if (_positionProfitByDate != null)
                {
                    return (decimal)_positionProfitByDate;
                }
                else
                {
                    if (this.oSymbol == null)
                        return 0;
                    else
                    {
                        return (this.SettlementPrice - this.CostPrice()) * this.Volume * this.oSymbol.Multiple * (this.Side ? 1 : -1);
                    }
                }
            }
            set { _positionProfitByDate = value; }
        }


        decimal? _positionProfitByTrade;
        /// <summary>
        /// 逐笔浮动盈亏
        /// </summary>
        public decimal PositionProfitByTrade 
        {
            get
            {
                if (_positionProfitByTrade != null)
                    return (decimal)_positionProfitByTrade;
                else
                {
                    if (this.oSymbol == null)
                        return 0;
                    return (this.SettlementPrice - this.OpenPrice) * this.Volume * this.oSymbol.Multiple * (this.Side ? 1 : -1);
                }
            }

            set { _positionProfitByTrade = value; }
        
        }

        decimal? _margin;
        /// <summary>
        /// 投资者保证金
        /// 
        /// </summary>
        public decimal Margin
        {
            get 
            {
                if (_margin != null)
                    return (decimal)_margin;
                else
                {
                    if (oSymbol == null) return 0;
                    //其余品种保证金按照最新价格计算
                    if (oSymbol.Margin <= 1)
                    {
                        //需要判断价格的有效性
                        return this.Volume * this.SettlementPrice * oSymbol.Multiple * oSymbol.Margin;
                    }
                    else
                        return this.Volume * oSymbol.Margin;
                }
            }

            set { _margin = value; }
        }

        #endregion



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

        public override string ToString()
        {
            return Util.PrintObj(this);
        }

        //public override string ToString()
        //{
        //    return string.Format("{0} {1} ")
        //}

        public static string Serialize(PositionDetail p)
        {
            char d = ',';
            StringBuilder sb = new StringBuilder();
            sb.Append(p.Account);//0
            sb.Append(d);
            sb.Append(p.OpenDate);//1
            sb.Append(d);
            sb.Append(p.OpenTime);//2
            sb.Append(d);
            sb.Append("0");//3
            sb.Append(d);
            sb.Append(p.Settleday);//4
            sb.Append(d);
            sb.Append(p.Side);//5
            sb.Append(d);
            sb.Append(p.Volume);//6
            sb.Append(d);
            sb.Append(p.OpenPrice);//7
            sb.Append(d);
            sb.Append(p.TradeID);//8
            sb.Append(d);
            sb.Append(p.LastSettlementPrice);//9
            sb.Append(d);
            sb.Append(p.SettlementPrice);//10
            sb.Append(d);
            sb.Append(p.CloseVolume);//11
            sb.Append(d);
            sb.Append(p.HedgeFlag);//12
            sb.Append(d);
            sb.Append(p.Exchange);//13
            sb.Append(d);
            sb.Append(p.Symbol);//14
            sb.Append(d);
            sb.Append(p.SecCode);//15
            sb.Append(d);
            sb.Append(p.Margin);//16
            sb.Append(d);
            sb.Append(p.CloseProfitByDate);//17
            sb.Append(d);
            sb.Append(p.PositionProfitByDate);//18
            sb.Append(d);
            sb.Append(p.CloseAmount);
            sb.Append(d);
            sb.Append(p.CloseProfitByTrade);
            sb.Append(d);
            sb.Append(p.PositionProfitByTrade);
            return sb.ToString();
        }

        public static PositionDetail Deserialize(string content)
        {
            string[] rec = content.Split(',');
            PositionDetail p = new PositionDetailImpl();
            p.Account = rec[0];
            p.OpenDate = int.Parse(rec[1]);
            p.OpenTime = int.Parse(rec[2]);
            //p.Tradingday = int.Parse(rec[3]);
            p.Settleday = int.Parse(rec[4]);
            p.Side = bool.Parse(rec[5]);
            p.Volume = int.Parse(rec[6]);
            p.OpenPrice = decimal.Parse(rec[7]);
            p.TradeID = rec[8];
            p.LastSettlementPrice = decimal.Parse(rec[9]);
            p.SettlementPrice = decimal.Parse(rec[10]);
            p.CloseVolume = int.Parse(rec[11]);
            p.HedgeFlag = rec[12];
            p.Exchange = rec[13];
            p.Symbol = rec[14];
            p.SecCode = rec[15];
            p.Margin = decimal.Parse(rec[16]);
            p.CloseProfitByDate = decimal.Parse(rec[17]);
            p.PositionProfitByDate = decimal.Parse(rec[18]);
            p.CloseAmount = decimal.Parse(rec[19]);
            p.CloseProfitByTrade = decimal.Parse(rec[20]);
            p.PositionProfitByTrade = decimal.Parse(rec[21]);
            return p;
        }
    }
}
