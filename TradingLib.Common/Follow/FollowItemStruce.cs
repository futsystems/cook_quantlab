using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.Common
{
    /* 跟单项数据结构 用于在管理段进行显示
     * 
     * 
     * 
     * */

    public class FollowItemBase
    {
        /// <summary>
        /// 跟单ID
        /// </summary>
        public int StrategyID { get; set; }

        /// <summary>
        /// 最小价格变动 用于格式化价格与滑点小数位数显示
        /// </summary>
        public string PriceFormat { get; set; }



        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// 跟单方向
        /// </summary>
        public bool Side { get; set; }

        /// <summary>
        /// 信号价格
        /// </summary>
        public decimal SigPrice { get; set; }

        /// <summary>
        /// 信号数量
        /// </summary>
        public int SigSize { get; set; }

        /// <summary>
        /// 跟单发送数量
        /// </summary>
        public int FollowSentSize { get; set; }

        /// <summary>
        /// 跟单成交数量
        /// </summary>
        public int FollowFillSize { get; set; }

        /// <summary>
        /// 跟单均价
        /// </summary>
        public decimal FollowAvgPrice { get; set; }


        /// <summary>
        /// 滑点
        /// </summary>
        public decimal FollowSlip { get; set; }

        /// <summary>
        /// 触发类别
        /// </summary>
        public QSEnumFollowItemTriggerType TriggerType { get; set; }

        /// <summary>
        /// 持仓事件类别
        /// </summary>
        //public QSEnumPositionEventType EventType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public QSEnumFollowStage Stage { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }


        /// <summary>
        /// 初始信号 用于过滤
        /// </summary>
        public string Filter_SourceSignal { get; set; }

        /// <summary>
        /// 持仓数量 用于过滤
        /// </summary>
        public int Filter_PositonHold { get; set; }

        /// <summary>
        /// 盈利 用于过滤
        /// </summary>
        public decimal Filter_Profit { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class EntryFollowItemStruct:FollowItemBase
    {

        /// <summary>
        /// 信号数据库编号
        /// </summary>
        public int SignalID { get; set; }

        /// <summary>
        /// 信号Token
        /// </summary>
        public string SignalToken { get; set; }

        /// <summary>
        /// 跟单编号
        /// </summary>
        public string FollowKey { get; set; }

        /// <summary>
        /// 开仓成交编号
        /// </summary>
        public string OpenTradeID { get; set; }

        /// <summary>
        /// 总滑点 包含了平仓滑点
        /// </summary>
        public decimal TotalSlip { get; set; }

        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public decimal TotalRealizedPL { get; set; }


        /// <summary>
        /// 持仓数量
        /// 开仓跟单项形成的跟单持仓数量 - 对应平仓跟单项的成交数量（未平持仓数量）
        /// </summary>
        public int PositionHoldSize { get; set; }

    }

    public class ExitFollowItemStruct : FollowItemBase
    {
        /// <summary>
        /// 开仓跟单编号
        /// 每个平仓跟单项目都和某条开仓跟单项目对应
        /// </summary>
        public string EntryFollowKey { get; set; }

        /// <summary>
        /// 跟单编号
        /// </summary>
        public string FollowKey { get; set; }

        /// <summary>
        /// 平仓成交编号
        /// </summary>
        public string CloseTradeID { get; set; }

        /// <summary>
        /// 跟单平仓盈亏
        /// </summary>
        public decimal FollowProfit { get; set; }


    }
}
