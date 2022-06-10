using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    /// <summary>
    /// 交易账户当前交易日交易数据
    /// </summary>
    public interface ITradingInfo
    {
        /// <summary>
        /// 是否有任何持仓
        /// </summary>
        bool AnyPosition { get; }

        /// <summary>
        /// 获得所有持仓对象
        /// </summary>
        IEnumerable<Position> Positions { get; }

        /// <summary>
        /// 多头持仓维护器
        /// </summary>
        IEnumerable<Position> PositionsLong { get; }

        /// <summary>
        /// 空头持仓维护器
        /// </summary>
        IEnumerable<Position> PositionsShort { get; }

        /// <summary>
        /// 获得所有委托对象
        /// </summary>
        IEnumerable<Order> Orders { get; }

        /// <summary>
        /// 获得所有成交对象
        /// </summary>
        IEnumerable<Trade> Trades { get; }

        /// <summary>
        /// 获得所有二元委托对象
        /// </summary>
        IEnumerable<BinaryOptionOrder> BinaryOptionOrders { get; }

        /// <summary>
        /// 获得所有隔夜持仓数据
        /// </summary>
        IEnumerable<PositionDetail> YdPositions { get; }

        /// <summary>
        /// 获得某个合约的持仓对象
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        Position GetPosition(string symbol,bool side);
    }
}
