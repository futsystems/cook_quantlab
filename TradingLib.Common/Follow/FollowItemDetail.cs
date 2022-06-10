using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 跟单项委托
    /// </summary>
    public class FollowItemOrderInfo
    {
        /// <summary>
        /// 全局委托编号
        /// </summary>
        public long OrderID { get; set; }

        /// <summary>
        /// 本地委托编号
        /// </summary>
        public string LocalOrderID { get; set; }

        /// <summary>
        /// 远端委托编号
        /// </summary>
        public string RemoteOrderID { get; set; }

        /// <summary>
        /// 委托状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        public bool Side { get; set; }

        /// <summary>
        /// 发送数量
        /// </summary>
        public int SentSize { get; set; }


        /// <summary>
        /// 成交数量
        /// </summary>
        public int FillSize { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }


        public FollowItemTradeInfo[] Trades { get; set; }
    }

    /// <summary>
    /// 跟单项成交
    /// </summary>
    public class FollowItemTradeInfo
    {
        /// <summary>
        /// 本地成交编号
        /// </summary>
        public string LocalTradeID { get; set; }

        /// <summary>
        /// 远端成交编号
        /// </summary>
        public string RemoteTradeID { get; set; }



        /// <summary>
        /// 方向
        /// </summary>
        public bool Side { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 成交数量
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 成交价格
        /// </summary>
        public decimal Price { get; set; }
    }


    public class FollowItemManualTriggerInfo
    {
        /// <summary>
        /// 方向
        /// </summary>
        public bool Side { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 成交数量
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 该手工触发下的对应的委托以及委托下面对应的成交
        /// </summary>
        public FollowItemOrderInfo[] Orders { get; set; }
    }
    /// <summary>
    /// 跟单项信号成交
    /// </summary>
    public class FollowItemSignalTradeInfo : FollowItemTradeInfo
    {
        /// <summary>
        /// 该信号成交所触发的委托
        /// 每个触发的委托下面还包含了对应的成交
        /// </summary>
        public FollowItemOrderInfo[] Orders { get; set; }
    }

    /// <summary>
    /// 跟单项目明细
    /// 记录了一个跟单项目的明细数据
    /// </summary>
    public class FollowItemDetail
    {

        /// <summary>
        /// 跟单编号
        /// </summary>
        public string FollowKey { get; set; }

        /// <summary>
        /// 持仓数量
        /// </summary>
        public int PositionHoldSize { get; set; }

        /// <summary>
        /// 总滑点 包含了平仓滑点
        /// </summary>
        public decimal TotalSlip { get; set; }

        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public decimal TotalRealizedPL { get; set; }


        /// <summary>
        /// 开仓信号成交
        /// </summary>
        public FollowItemSignalTradeInfo EntrySignalTrade { get; set; }


        /// <summary>
        /// 平仓信号成交
        /// </summary>
        public FollowItemSignalTradeInfo[] ExitSignalTrades { get; set; }

        /// <summary>
        /// 手工触发平仓
        /// </summary>
        public FollowItemManualTriggerInfo[] ExitManualTrigger { get; set; }

    }
}
