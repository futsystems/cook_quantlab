﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    /// <summary>
    /// 浮动盈亏算法
    /// </summary>
    public enum QSEnumAlgorithm
    {
        /// <summary>
        /// 浮盈浮亏都计
        /// </summary>
        [Description("浮盈和浮亏")]
        AG_All = 1,

        /// <summary>
        /// 只计算亏损
        /// </summary>
        [Description("浮亏")]
        AG_OnlyLost = 1,

        /// <summary>
        /// 只计算盈利
        /// </summary>
        [Description("浮盈")]
        AG_OnlyGain = 2,

        /// <summary>
        /// 都不计算
        /// </summary>
        [Description("不计算")]
        AG_None = 3,


    }
}