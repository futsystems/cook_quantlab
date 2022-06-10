using System;
using System.Collections.Generic;

namespace TradingLib.API
{
    public interface Trade
    {
        #region 基础属性
        /// <summary>
        /// id of trade
        /// </summary>
        long id { get; set; }

        /// <summary>
        /// 成交所属交易帐号
        /// </summary>
        string Account { get; set; }

        /// <summary>
        /// 成交日期
        /// </summary>
        int xDate { get; set; }

        /// <summary>
        /// 成交时间
        /// </summary>
        int xTime { get; set; }


        /// <summary>
        /// 合约
        /// </summary>
        string Symbol { get; set; }

        /// <summary>
        /// 本地合约编号 用于多交易所时 合约字段冲突时 本地合约唯一标识
        /// </summary>
        string LocalSymbol { get; set; }

        /// <summary>
        /// symbol assocated with this order,
        /// </summary>
        Symbol oSymbol { get; set; }

        /// <summary>
        /// 成交方向
        /// </summary>
        bool Side { get; set; }

        /// <summary>
        /// 成交数量
        /// </summary>
        int xSize { get; set; }

        /// <summary>
        /// 交易数量绝对值
        /// </summary>
        int UnsignedSize { get; }

        /// <summary>
        /// 成交价格
        /// </summary>
        decimal xPrice { get; set; }

        /// <summary>
        /// 开平标志
        /// </summary>
        QSEnumOffsetFlag OffsetFlag { get; set; }

        /// <summary>
        /// 投机 套保标识
        /// </summary>
        QSEnumHedgeFlag HedgeFlag { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        decimal Commission { get; set; }

        /// <summary>
        /// 印花税
        /// </summary>
        decimal StampTax { get; set; }

        /// <summary>
        /// 过户费
        /// </summary>
        decimal TransferFee { get; set; }

        /// <summary>
        /// 平仓盈亏
        /// </summary>
        decimal Profit { get; set; }

        

        #endregion



        #region 成交其他属性
        /// <summary>
        /// 交易所
        /// </summary>
        string Exchange { get; set; }

        /// <summary>
        /// 结算日 标注属于哪个结算日
        /// </summary>
        int SettleDay { get; set; }

        /// <summary>
        /// 结算标认识
        /// </summary>
        bool Settled { get;set; }

        /// <summary>
        /// 品种类型
        /// </summary>
        SecurityType SecurityType { get; set; }

        /// <summary>
        /// 品种 以字符串形式给出 当有oSymbol时候统一从oSymbol对应的字段进行取值
        /// </summary>
        string SecurityCode { get; set; }

        /// <summary>
        /// 货币
        /// </summary>
        CurrencyType Currency { get; set; }

        /// <summary>
        /// 如果是平仓成交则获得对应所有平仓明细
        /// </summary>
        List<PositionCloseDetail> CloseDetails { get; }
        #endregion


        #region 判定属性
        /// <summary>
        /// 是否有效
        /// </summary>
        bool isValid { get; }

        /// <summary>
        /// 是否已经被成交过
        /// </summary>
        bool isFilled { get; }

        /// <summary>
        /// 该成交是否是开仓
        /// 开仓为True
        /// 平仓为False
        /// </summary>
        /// <returns></returns>
        bool IsEntryPosition { get; }

        /// <summary>
        /// 判定的持仓方向
        /// 正向开仓或者反向平仓则仓位方向为true代表longposition
        /// 反向开仓或者正向平仓则仓位方向为false代表shortpostion
        /// </summary>
        bool PositionSide { get; }
        #endregion

        /// <summary>
        /// 标记该成交的性质 开仓 加仓 平仓 减仓
        /// </summary>
        QSEnumPosOperation PositionOperation { get; set; }


        #region Broker端属性
        /// <summary>
        /// 获得该委托通过哪个交易通道发出
        /// </summary>
        string Broker { get; set; }

        /// <summary>
        /// Broker端 本地委托编号
        /// 系统有2种方式将成交与委托联系起来
        /// 1.近端委托编号进行关联 自己按一定方式维护唯一编号,成交端发送成交回报时按该编号关联委托
        /// 2.远端委托编号进行关联 由成交端在委托回报中设定远端委托编号,成交端发送成交回报时候按该编号关联委托
        /// </summary>
        string BrokerLocalOrderID { get; set; }

        /// <summary>
        /// Broker端 远端委托编号
        /// </summary>
        string BrokerRemoteOrderID { get; set; }

        /// <summary>
        /// Broker端的成交编号 由Broker按一定规则赋值
        /// </summary>
        string BrokerTradeID { get; set; }
        #endregion

        #region 分帐户端属性
        /// <summary>
        /// 成交编号 由系统内TradeID生成器 统一赋值 保证日内唯一
        /// </summary>
        string TradeID { get; set; }

        /// <summary>
        /// 委托流水号
        /// </summary>
        int OrderSeq { get; set; }

        /// <summary>
        /// 客户端委托引用
        /// </summary>
        string OrderRef { get; set; }

        /// <summary>
        /// 委托交易所编号
        /// </summary>
        string OrderSysID { get; set; }
        #endregion


        #region 委托分解属性

        QSEnumOrderBreedType ?FatherBreed { get; set; }
        /// <summary>
        /// 父委托编号
        /// </summary>
        long FatherID { get; set; }

        /// <summary>
        /// 委托分解源
        /// </summary>
        QSEnumOrderBreedType Breed { get; set; }

        #endregion

       

    }

    public class InvalidTrade : Exception { }
}