using System;

namespace TradingLib.API
{
    /// <summary>
    /// Order
    /// </summary>
    public interface Order
    {
        #region 帐户 时间 合约 指令
        /// <summary>
        /// 系统内部编号long
        /// </summary>
        long id { get; set; }

        /// <summary>
        /// account to place inventory if order is executed
        /// </summary>
        string Account { get; set; }

        /// <summary>
        /// 委托提交日期 date in  date format (2010/03/05 = 20100305)
        /// </summary>
        int Date { get; set; }

        /// <summary>
        /// 委托提交时间 time including seconds 1:35:07PM = 133507
        /// </summary>
        int Time { get; set; }


        /// <summary>
        /// 合约字头
        /// </summary>
        string Symbol { get; set; }

        /// <summary>
        /// 本地合约编号 用于多交易所时 合约字段冲突时 本地合约唯一标识
        /// </summary>
        string LocalSymbol { get; set; }

        /// <summary>
        /// 合约对象
        /// </summary>
        Symbol oSymbol { get; set; }

        /// <summary>
        /// TIF设置 MOC,IOC 等委托扩展指令
        /// </summary>
        QSEnumTimeInForce TimeInForce { get; set; }

        /// <summary>
        /// 开平标志
        /// </summary>
        QSEnumOffsetFlag OffsetFlag { get; set; }

        /// <summary>
        /// 投机/套保标识
        /// </summary>
        QSEnumHedgeFlag HedgeFlag { get; set; }

        #endregion



        #region 数量 方向 价格
        /// <summary>
        /// signed size of order (-100 = sell 100)
        /// 委托数量根据成交情况会发生变化 带方向
        /// </summary>
        int Size { get; set; }

        /// <summary>
        /// 初始委托总数量 带方向
        /// </summary>
        int TotalSize { get; set; }

        /// <summary>
        /// 已成交数量 绝对值
        /// </summary>
        int FilledSize { get; set; }

        /// <summary>
        /// unsigned size of order
        /// </summary>
        int UnsignedSize { get; }


        /// <summary>
        /// true if buy, otherwise sell
        /// </summary>
        bool Side { get; set; }

        /// <summary>
        /// price of order. (0 for market)
        /// </summary>
        decimal LimitPrice { get; set; }

        /// <summary>
        /// stop price if applicable
        /// </summary>
        decimal StopPrice { get; set; }

        /// <summary>
        /// trail amount if applicable
        /// </summary>
        decimal trail { get; set; }

        #endregion

        #region 委托其他属性
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
        bool Settled { get; set; }


        /// <summary>
        /// 品种类别
        /// </summary>
        SecurityType SecurityType { get; set; }

        /// <summary>
        /// 货币类别
        /// </summary>
        CurrencyType Currency { get; set; }

        /// <summary>
        /// 该委托触发来源
        /// </summary>
        QSEnumOrderSource OrderSource { get; set; }

        /// <summary>
        /// 是否强平
        /// </summary>
        bool ForceClose { get; set; }

        /// <summary>
        /// 强平原因
        /// </summary>
        string ForceCloseReason { get; set; }

        /// <summary>
        /// 委托状态
        /// </summary>
        QSEnumOrderStatus Status { get; set; }

        /// <summary>
        /// order comment
        /// </summary>
        string Comment { get; set; }

        #endregion


        #region 委托相关判定操作
        /// <summary>
        /// whether order has been filled
        /// </summary>
        bool isFilled { get; }

        /// <summary>
        /// limit order
        /// </summary>
        bool isLimit { get; }

        /// <summary>
        /// stop order
        /// </summary>
        bool isStop { get; }

        /// <summary>
        /// trail order
        /// </summary>
        bool isTrail { get; }

        /// <summary>
        /// market order
        /// </summary>
        bool isMarket { get; }

        /// <summary>
        /// order is valid
        /// </summary>
        bool isValid { get; }

        #endregion


        #region 委托成交函数
        /// <summary>
        /// 集合竞价成交
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool FillAuction(Tick t);
        /// <summary>
        /// try to fill order against another order
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        bool Fill(Order o);

        /// <summary>
        /// try to fill order against trade
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Fill(Tick t);

        /// <summary>
        /// try to fill order against trade or bid/ask
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Fill(Tick t, bool bidask, bool fillopg,bool fillall,int minFillSize);

        

        /// <summary>
        /// try to fill order as OPG order
        /// </summary>
        /// <param name="t"></param>
        /// <param name="fillOPG"></param>
        /// <returns></returns>
        bool Fill(Tick t, bool fillOPG);
        #endregion


        #region 分帐户端属性 模拟CTP字段
        /// <summary>
        /// 委托流水号 由系统统一分配
        /// </summary>
        int OrderSeq { get; set; }

        /// <summary>
        /// 客户端委托引用
        /// </summary>
        string OrderRef { get; set; }

        /// <summary>
        /// 委托交易所编号 用于实现CTP字段 服务端通过将OrderSeq赋值给OrderExchID实现
        /// 与交易所字段组合使用 形成委托一ID
        /// </summary>
        string OrderSysID { get; set; }

        /// <summary>
        /// 标注该委托来自于哪个前置
        /// </summary>
        int FrontIDi { get; set; }

        /// <summary>
        /// 标注该委托来自于哪个客户端
        /// </summary>
        int SessionIDi { get; set; }

        /// <summary>
        /// 客户端的请求ID 
        /// </summary>
        int RequestID { get; set; }

        #endregion


        
        #region Broker端字段 抽象成近端ID和远端ID
        /// <summary>
        /// 该委托是通过哪个成交接口发出
        /// </summary>
        string Broker { get; set; }

        /// <summary>
        /// Broker端 本地委托编号
        /// </summary>
        string BrokerLocalOrderID { get; set; }

        /// <summary>
        /// Broker端 远端委托编号
        /// </summary>
        string BrokerRemoteOrderID { get; set; }
        #endregion



        #region 委托分解属性

        /// <summary>
        /// 委托分解父源
        /// 表明该委托是分解的那个源的委托
        /// 如果分帐户侧委托在路由出分解则 则成交侧分解的就是路由侧的委托
        /// </summary>
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


        /// <summary>
        /// 本地CopyID
        /// </summary>
        int CopyID { get; set; }





        /// <summary>
        /// 该成交是否是开仓
        /// </summary>
        /// <returns></returns>
        bool IsEntryPosition { get; }

        /// <summary>
        /// 仓位操作方向
        /// 代表是多头操作还是空头操作
        /// </summary>
        bool PositionSide { get; }

        
        
    }

    public class InvalidOrder : Exception { }
}
