using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace  TradingLib.Common
{
    /// <summary>
    /// 信号配置
    /// </summary>
    public class SignalConfig
    {
        /// <summary>
        /// 信号全局ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 信号类别
        /// </summary>
        public QSEnumSignalType SignalType { get; set; }

        /// <summary>
        /// 信号标识
        /// 1.CTP为通道标识
        /// 2.帐户为交易帐号
        /// </summary>
        public string SignalToken { get; set; }

        /// <summary>
        /// 分区编号
        /// </summary>
        public int Domain_ID { get; set; }
    }
}
