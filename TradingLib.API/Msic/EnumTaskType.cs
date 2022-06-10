using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    public enum QSEnumTaskType
    {
        /// <summary>
        /// 特定时间,比如在几点几分几秒执行某个任务
        /// </summary>
        [Description("定时任务")]
        SPECIALTIME,

        /// <summary>
        /// 循环往复,比如每隔多少时间执行某个任务
        /// </summary>
        [Description("循环任务")]
        CIRCULATE,
    }
}
