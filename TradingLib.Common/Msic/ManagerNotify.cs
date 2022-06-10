using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    /// <summary>
    /// 管理端通知对象
    /// </summary>
    public class ManagerNotify
    {

        /// <summary>
        /// 通知类别
        /// </summary>
        public string NotifyType { get; set; }

        /// <summary>
        /// 通知代码
        /// ErrorID为0 表明无错误
        /// </summary>
        public int ErrorID { get; set; }

        /// <summary>
        /// 通知消息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
