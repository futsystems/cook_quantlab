using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    /// <summary>
    /// 持仓明细
    /// </summary>
    public interface PositionDetail
    {
        /// <summary>
        /// 交易帐号
        /// </summary>
        string Account { get; set; }

        /// <summary>
        /// 开仓日期
        /// </summary>
        int OpenDate { get; set; }


        /// <summary>
        /// 开仓时间
        /// </summary>
        int OpenTime { get; set; }

        /// <summary>
        /// 结算日 表明该持仓明细记录属于哪个结算日
        /// </summary>
        int Settleday { get; set; }

        /// <summary>
        /// 方向 多或空
        /// </summary>
        bool Side { get; set; }

        /// <summary>
        /// 是否是历史持仓
        /// </summary>
        bool IsHisPosition { get;set;}

        /// <summary>
        /// 数量
        /// </summary>
        int Volume { get; set; }


        /// <summary>
        /// 开仓价格
        /// </summary>
        decimal OpenPrice { get; set; }


        /// <summary>
        /// 成交编号
        /// </summary>
        string TradeID { get; set; }

        /// <summary>
        /// 昨结算价
        /// </summary>
        decimal LastSettlementPrice { get; set; }

        /// <summary>
        /// 今结算价
        /// </summary>
        decimal SettlementPrice { get; set; }

        /// <summary>
        /// 平仓金额
        /// </summary>
        decimal CloseAmount { get; set; }

        /// <summary>
        /// 平仓量
        /// </summary>
        int CloseVolume { get; set; }

        /// <summary>
        /// 投机套保标识
        /// </summary>
        string HedgeFlag { get; set; }

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
        string Symbol {get;set;}

        /// <summary>
        /// 品种信息
        /// </summary>
        string SecCode { get; set; }
        


        /// <summary>
        /// 投资者保证金
        /// </summary>
        decimal Margin { get; set; }


        /// <summary>
        /// 盯市平仓盈亏
        ///  SUM（平昨量 *（平仓价 - 昨结算价）* 合约乘数）+SUM（平今量 *（平仓价 - 开仓价）* 合约乘数） -- 多头
        ///  SUM（平昨量 *（昨结算价 - 平仓价）* 合约乘数）+SUM（平今量 *（开仓价 - 平仓价）* 合约乘数） -- 空头
        /// </summary>
        decimal CloseProfitByDate { get; set; }

        /// <summary>
        /// 逐笔平仓盈亏 
        /// SUM（平仓量 * （平仓价 - 开仓价）* 合约乘数） -- 多头
        /// SUM（平仓量 * （开仓价 - 平仓价）* 合约乘数） -- 空头
        /// </summary>
        decimal CloseProfitByTrade { get; set; }

        /// <summary>
        /// 浮动盈亏 盯市
        /// 今仓 (开仓价格-结算价)*手数*乘数
        /// 
        /// 在结算过程中 盯市浮动盈亏会计入结算单并反映在帐户权益上
        /// </summary>
        decimal PositionProfitByDate { get; set; }

        /// <summary>
        /// 浮动盈亏 逐笔
        /// </summary>
        decimal PositionProfitByTrade { get; set; }


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
