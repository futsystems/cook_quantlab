using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    /// <summary>
    /// 数据操作类别
    /// </summary>
    public enum EnumDataRepositoryType
    {
        /// <summary>
        /// 位置
        /// </summary>
        Unknown=0,
        /// <summary>
        /// 插入委托
        /// </summary>
        InsertOrder,
        /// <summary>
        /// 更新委托
        /// </summary>
        UpdateOrder,
        /// <summary>
        /// 插入成交
        /// </summary>
        InsertTrade,
        /// <summary>
        /// 插入平仓明细
        /// </summary>
        InsertOrderAction,

        /// <summary>
        /// 插入平仓明细
        /// </summary>
        InsertPositionCloseDetail,

        /// <summary>
        /// 插入持仓明细
        /// </summary>
        InsertPositionDetail,

        /// <summary>
        /// 插入交易所结算记录
        /// </summary>
        InsertExchangeSettlement,

        /// <summary>
        /// 插入出入金记录
        /// </summary>
        InsertCashTransaction,

        /// <summary>
        /// 结算委托
        /// </summary>
        SettleOrder,
        /// <summary>
        /// 结算成交
        /// </summary>
        SettleTrade,
        /// <summary>
        /// 结算持仓明细
        /// </summary>
        SettlePositionDetail,
        /// <summary>
        /// 结算交易所结算数据
        /// </summary>
        SettleExchangeSettlement,
        /// <summary>
        /// 结算出入金操作
        /// </summary>
        SettleCashTransaction,

    }
    public class DataRepositoryException:TLException
    {
        public EnumDataRepositoryType RepositoryType { get; set; }
        public object RepositoryData { get; set; }
        public DataRepositoryException(EnumDataRepositoryType type, object o,Exception e)
            :base("DataRepository Error",e)
        {
            this.RepositoryType = type;
            this.RepositoryData = o;

        }
    }
}
