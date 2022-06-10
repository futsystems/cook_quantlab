
namespace TradingLib.API
{
    /// <summary>
    /// Ordering of fields in SENDORDER and ORDERNOTIFY messages
    /// </summary>
    public enum OrderField
    {
        Symbol = 0,//合约编码
        Side,//方向
        TotalSize,//原始委托总数
        Size,//数量
        Price,//limit价格
        Stop,//stop价格
        Comment,//备注
        Exchange,//交易所
        Account,//账户
        Security,//合约
        Currency,//货币
        LocalSymbol, // non-pretty symbol or contract symbol for futures
        OrderID,//委托编号
        OrderTIF,//委托TIF
        oDate,//委托日期
        oTime,//委托时间
        oFilled,//成交数量
        Trail,//trailing 信息
        Broker,//成交接口broker
        BrokerKey,//期货成交所分配的委托编号
        LocalID,//本地记录编号
        Status,//委托状态
        PostFlag,//委托开平标识
        OrderRef,//委托引用
        ForceClose,//强平
        HedgeFlag,//投机标识
        OrderSeq,//委托流水
        OrderExchID,//交易所委托标识
        ForceReason,//强平原因
        FrontID,//前置编号
        SessionID,//回话编号
        RequestID,//请求编号
    }
}