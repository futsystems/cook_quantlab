using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.Common
{
    /// <summary>
    /// 数据恢复任务
    /// 系统启动过程中用于将合约对应Bar数据恢复到当前最新状态
    /// </summary>
    public class RestoreTask
    {

        public RestoreTask()
        {
            this.CreatedTime = DateTime.Now;
            this.CompleteTime = DateTime.MaxValue;

            this.oSymbol = null;
            this.Symbol = string.Empty;
            this.Exchange = string.Empty;

            this.Intraday1MinHistBarEnd = DateTime.MinValue;
            this.EodHistBarEndTradingDay = int.MinValue;

            this.DataFeed1MinRoundTime = DateTime.MinValue;
            this.Exchange1MinRoundtime = DateTime.MinValue;

            this.IsTickFilled = false;
            this.IsTickFillSuccess = false;

            this.IsEODRestored = false;
            this.IsEODRestoreSuccess = false;

            this.Complete = false;
        }
        public RestoreTask(Symbol symbol)
        {
            this.CreatedTime = DateTime.Now;
            this.CompleteTime = DateTime.MaxValue;

            this.oSymbol = symbol;
            this.Symbol = this.oSymbol.Symbol;
            this.Exchange = this.oSymbol.Exchange;

            this.Intraday1MinHistBarEnd = DateTime.MinValue;
            this.EodHistBarEndTradingDay = int.MinValue;

            this.DataFeed1MinRoundTime = DateTime.MinValue;
            this.Exchange1MinRoundtime = DateTime.MinValue;

            this.IsTickFilled = false;
            this.IsTickFillSuccess = false;

            this.IsEODRestored = false;
            this.IsEODRestoreSuccess = false;

            this.Complete = false;
        }

        /// <summary>
        /// 合约
        /// </summary>
        //[NoJsonExportAttr()]
        [Newtonsoft.Json.JsonIgnore]
        public Symbol oSymbol { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 任务创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime CompleteTime { get; set; }

        /// <summary>
        /// 数据库加载日内数据后 最近的一个1分钟Bar结束时间
        /// </summary>
        public DateTime Intraday1MinHistBarEnd { get; set; }

        /// <summary>
        /// 数据库加载日线数据后 最近的一个Bar结束交易日
        /// </summary>
        public int EodHistBarEndTradingDay { get; set; }

        /// <summary>
        /// 行情源第一个1分钟Round结束时间
        /// 通过该时刻来作为历史Tick数据加载的结束时间
        /// 无论是开盘还是收盘 主力还是非主力 当时间跨越过1分钟后 则该时刻之前的数据就是完整的,而之后的Bar需要被储存,
        /// 这里原先第一个Bar不储存的方法有漏洞
        /// 如果非主力合约 中间一段时间没有数据 过了1分钟 数据恢复完毕之后 才有Tick数据过来 生成第一个Bar 则该Bar是完整的,可以直接储存
        /// </summary>
        public DateTime Exchange1MinRoundtime { get; set; }

        /// <summary>
        /// 行情源1分钟Round时间
        /// 该时间为启动1分钟之后 截取的数据完备的最小周期结束时间
        /// </summary>
        public DateTime DataFeed1MinRoundTime { get; set; }

        /// <summary>
        /// 历史Tick数据恢复标识
        /// </summary>
        public bool IsTickFilled { get; set; }

        /// <summary>
        /// 历史Tick数据回补成功
        /// </summary>
        public bool IsTickFillSuccess { get; set; }

        /// <summary>
        /// EOD数据恢复标识
        /// </summary>
        public bool IsEODRestored { get; set; }

        /// <summary>
        /// 日级别数据恢复成功
        /// </summary>
        public bool IsEODRestoreSuccess { get; set; }

        /// <summary>
        /// 任务完成
        /// </summary>
        public bool Complete { get; set; }

    }


}
