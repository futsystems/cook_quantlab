using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface PositionCloseDetail
    {
        /// <summary>
        /// 交易帐号
        /// </summary>
        string Account { get; set; }

        /// <summary>
        /// 结算日，平仓时所在结算日
        /// 通过交易日与结算日可以判断是今仓还是昨仓
        /// </summary>
        int Settleday { get; set; }


        /// <summary>
        /// 方向
        /// </summary>
        bool Side { get; set; }

        /// <summary>
        /// 平昨仓还是平今仓
        /// </summary>
        bool IsCloseYdPosition { get; set; }

        /// <summary>
        /// 开仓日期
        /// </summary>
        int OpenDate { get; set; }

        /// <summary>
        /// 开仓时间
        /// </summary>
        int OpenTime { get; set; }

        /// <summary>
        /// 开仓成交编号
        /// </summary>
        string OpenTradeID { get; set; }

        /// <summary>
        /// 平仓日期
        /// </summary>
        int CloseDate { get; set; }

        /// <summary>
        /// 平仓时间
        /// </summary>
        int CloseTime { get; set; }

        /// <summary>
        /// 平仓成交编号
        /// </summary>
        string CloseTradeID { get; set; }

        /// <summary>
        /// 开仓价格
        /// </summary>
        decimal OpenPrice { get; set; }

        /// <summary>
        /// 昨结算价
        /// </summary>
        decimal LastSettlementPrice { get; set; }

        /// <summary>
        /// 平仓价格
        /// </summary>
        decimal ClosePrice { get; set; }


        /// <summary>
        /// 平仓量
        /// </summary>
        int CloseVolume { get; set; }

        /// <summary>
        /// 平仓金额
        /// </summary>
        decimal CloseAmount { get; set; }


        /// <summary>
        /// 盯市平仓盈亏金额
        /// 平当日仓 (开仓-平仓)*手数*乘数
        /// </summary>
        decimal CloseProfitByDate { get; set; }

        /// <summary>
        /// 逐笔平仓盈亏金额
        /// </summary>
        decimal CloseProfitByTrade { get; set; }

        /// <summary>
        /// 盯市平仓盈亏点数
        /// </summary>
        decimal ClosePointByDate { get; set; }

        /// <summary>
        /// 逐笔平仓盈亏点数
        /// </summary>
        decimal ClosePointByTrade { get; set; }
        /// <summary>
        /// 合约信息
        /// </summary>
        Symbol oSymbol { get; set; }

        
        /// <summary>
        /// 交易所
        /// </summary>
        string Exchange { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        string Symbol { get; set; }

        /// <summary>
        /// 品种代码
        /// </summary>
        string SecCode {get;set;}

        /// <summary>
        /// 接口Token如果是接口侧的平仓明细则有BrokerToken字段
        /// 分帐户侧没有Broker
        /// </summary>
        string Broker { get; set; }

        /// <summary>
        /// 数据来源
        /// 1.分帐户侧
        /// 2.接口侧
        /// 3.路由侧
        /// </summary>
        QSEnumOrderBreedType Breed { get; set; }
    }
}
