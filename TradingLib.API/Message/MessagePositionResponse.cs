
namespace TradingLib.API
{
    /// <summary>
    /// order of fields in position response message
    /// </summary>
    public enum PositionField
    {
        symbol,//合约
        price,//价格
        size,//数量
        closedpl,//平仓盈亏
        account,//帐户
    }
}