
namespace TradingLib.API
{
    /// <summary>
    /// Order of fields in a TRADENOTIFY message
    /// </summary>
    public enum TradeField
    {
        xDate = 0,//成交日期
        xTime,//成交时间
        xUNUSED,
        Symbol,//合约代码
        Side,//方向
        Size,//数量
        Price,//价格
        Comment,//说明
        Account,//账户
        Security,//合约
        Currency,//货币
        LocalSymbol,//本地合约
        ID,//委托编号
        Exch,//交易所
        Broker,//成交接口
        BrokerKey,//成交接口编号
        Commission,//手续费
        PositionOperatoin,//成交尺长操作标识
        Profit,//平仓盈亏
        OrderRef,//委托引用
        HedgeFlag,//投机标识
        OrderSeq,//委托流水
        OrderExchID,//交易所委托标识
        OffsetFlag,//开平标识
        BrokerLocalOrderID,//成交侧本地委托编号
        BrokerRemoteOrderID,//成交侧远端委托编号
        BrokerTradeID,//成交侧成交编号
        StampTax,//印花税
        TransferFee,//过户费
        SecurityCode,//品种代码
    }
}