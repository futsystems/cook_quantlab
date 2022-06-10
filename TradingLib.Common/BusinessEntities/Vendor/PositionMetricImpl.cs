using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    /// <summary>
    /// 净持仓操作结果 
    /// 只针对开仓做处理,平仓的话按照在那里开仓就在哪里平仓的原则,不管保证金变化,均要在该通道进行平仓
    /// </summary>
    public class PositionAdjustmentResult
    {
        public PositionAdjustmentResult()
        {
            this.LongEntry = 0;
            this.LongExit = 0;
            this.ShortEntry = 0;
            this.ShortExit = 0;
        }
        /// <summary>
        /// 多头开仓数量
        /// </summary>
        public int LongEntry { get; set; }
        /// <summary>
        ///多头平仓数量
        /// </summary>
        public int LongExit { get; set; }
        /// <summary>
        /// 空头开仓数量
        /// </summary>
        public int ShortEntry { get; set; }
        /// <summary>
        /// 空头平仓数量
        /// </summary>
        public int ShortExit { get; set; }
    }


    /// <summary>
    /// 经持仓状态度量 用于判断是否可以进行净持仓操作
    /// </summary>
    public class PositionMetricImpl : PositionMetric
    {
        public PositionMetricImpl()
            :this("")
        { 
            
        }
        public PositionMetricImpl(string symbol)
        {
            this.Symbol = symbol;
            this.LongHoldSize = 0;
            this.LongPendingEntrySize = 0;
            this.LongPendingExitSize = 0;

            this.ShortHoldSize = 0;
            this.ShortPendingEntrySize = 0;
            this.ShortPendingExitSize = 0;
        }
        public PositionMetricImpl(PositionMetric copy)
        {
            this.Symbol = copy.Symbol;
            this.LongHoldSize = copy.LongHoldSize;
            this.LongPendingExitSize = copy.LongPendingExitSize;
            this.LongPendingEntrySize = copy.LongPendingEntrySize;

            this.ShortHoldSize = copy.ShortHoldSize;
            this.ShortPendingEntrySize = copy.ShortPendingEntrySize;
            this.ShortPendingExitSize = copy.ShortPendingExitSize;
        }
        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        public string Token { get; set; }
        /// <summary>
        /// 多头持有仓位
        /// </summary>
        public int LongHoldSize { get; set; }

        /// <summary>
        /// 多头待开数量
        /// </summary>
        public int LongPendingEntrySize { get; set; }

        /// <summary>
        /// 多头待平数量
        /// </summary>
        public int LongPendingExitSize { get; set; }

        /// <summary>
        /// 多方可以平掉的数量
        /// </summary>
        //[NoJsonExportAttr()]
        [Newtonsoft.Json.JsonIgnore]
        public int LongCanExitSize { get { return LongHoldSize - LongPendingExitSize; } }

        /// <summary>
        /// 空头持仓仓位
        /// </summary>
        public int ShortHoldSize { get; set; }

        /// <summary>
        /// 空头待开数量
        /// </summary>
        public int ShortPendingEntrySize { get; set; }

        /// <summary>
        /// 空头待平数量
        /// </summary>
        public int ShortPendingExitSize { get; set; }

        /// <summary>
        /// 空头可以平掉的数量
        /// </summary>
        //[NoJsonExportAttr()]
        [Newtonsoft.Json.JsonIgnore]
        public int ShortCanExitSaize { get { return ShortHoldSize - ShortPendingExitSize; } }
       
    }
}
