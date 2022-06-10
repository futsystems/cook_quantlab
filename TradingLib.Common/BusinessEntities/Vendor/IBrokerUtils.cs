using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public static class  IBrokerUtils
    {
        #region 对象过滤 返回对象不toarray避免的内存引用copy所有的计算只需要进行一次foreach循环
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Order> FilterOrders(this IBroker broker, SecurityType type)
        {
            return broker.Orders.Where(o => o.SecurityType == type);
        }

        public static IEnumerable<Order> FilterPendingOrders(this IBroker broker, SecurityType type)
        {
            return broker.Orders.Where(o => o.SecurityType == type && o.IsPending());
        }

        public static IEnumerable<Trade> FilterTrades(this IBroker broker, SecurityType type)
        {
            return broker.Trades.Where(f => f.SecurityType == type);
        }

        public static IEnumerable<Position> FilterPositions(this IBroker broker, SecurityType type)
        {
            return broker.Positions.Where(p => p.oSymbol.SecurityType == type);
        }
        #endregion

        #region 计算期货财务数据
        public static decimal CalFutMargin(this IBroker broker)
        {
            return FilterPositions(broker, SecurityType.FUT).Sum(pos => pos.CalcPositionMargin());
        }

        public static decimal CalFutMarginFrozen(this IBroker broker)
        {
            return 0;
            //return FilterPendingOrders(broker, SecurityType.FUT).Where(o => o.IsEntryPosition).Sum(e => account.CalOrderFundRequired(e, 0));
        }

        public static decimal CalFutUnRealizedPL(this IBroker broker)
        {
            return FilterPositions(broker, SecurityType.FUT).Sum(pos => pos.CalcUnRealizedPL());
        }

        //public static decimal CalFutSettleUnRealizedPL(this IBroker broker)
        //{
        //    return FilterPositions(broker, SecurityType.FUT).Sum(pos => pos.CalcSettleUnRealizedPL());
        //}

        public static decimal CalFutRealizedPL(this IBroker broker)
        {
            return FilterPositions(broker, SecurityType.FUT).Sum(pos => pos.CalcRealizedPL());
        }

        public static decimal CalFutCommission(this IBroker broker)
        {
            return FilterTrades(broker, SecurityType.FUT).Sum(fill => fill.GetCommission());
        }

        public static decimal CalFutCash(this IBroker broker)
        {
            return CalFutRealizedPL(broker) + CalFutUnRealizedPL(broker) - CalFutCommission(broker);
        }

        public static decimal CalFutLiquidation(this IBroker broker)
        {
            return CalFutCash(broker);
        }

        public static decimal CalFutMoneyUsed(this IBroker broker)
        {
            return CalFutMargin(broker) + CalFutMarginFrozen(broker);
        }


        #endregion

        /// <summary>
        /// 获得Broker中属于某个交易所的持仓
        /// </summary>
        /// <param name="broker"></param>
        /// <param name="exchange"></param>
        /// <returns></returns>
        public static IEnumerable<Position> GetPositions(this IBroker broker, Exchange exchange)
        {
            return broker.Positions.Where(pos => pos.oSymbol.SecurityFamily.Exchange.EXCode == exchange.EXCode);
        }

        /// <summary>
        /// 获得交易帐户某个交易所的委托
        /// </summary>
        /// <param name="account"></param>
        /// <param name="exchange"></param>
        /// <returns></returns>
        public static IEnumerable<Order> GetOrders(this IBroker broker, Exchange exchange, int settleday)
        {
            return broker.Orders.Where(o => o.oSymbol.SecurityFamily.Exchange.EXCode == exchange.EXCode && o.SettleDay == settleday);
        }

        /// <summary>
        /// 获得交易帐户某个交易所的所有成交
        /// </summary>
        /// <param name="account"></param>
        /// <param name="exchange"></param>
        /// <returns></returns>
        public static IEnumerable<Trade> GetTrades(this IBroker broker, Exchange exchange, int settleday)
        {
            return broker.Trades.Where(f => f.oSymbol.SecurityFamily.Exchange.EXCode == exchange.EXCode && f.SettleDay == settleday);
        }

    }
}
