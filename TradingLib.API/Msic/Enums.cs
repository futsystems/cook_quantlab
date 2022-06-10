using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    public enum QSEnumSettleMode
    {
        /// <summary>
        /// 历史结算模式
        /// 手工回滚到某个交易日 处于手工结算模式
        /// </summary>
        HistSettleMode,

        /// <summary>
        /// 结算状态
        /// 系统按定时任务执行结算任务
        /// </summary>
        SettleMode,

        /// <summary>
        /// 待机状态
        /// 结算中心未执行任何结算任务 此时系统处于正常工作状态 只要清算中心开启 即可接受客户委托
        /// </summary>
        StandbyMode,
    }

    /// <summary>
    /// 数据恢复状态
    /// </summary>
    public enum QSEnumResumeStatus
    { 
        BEGIN,//开始恢复数据
        END,//恢复数据结束
    }

    /// <summary>
    /// 规则集所使用的比较规则
    /// </summary>
    public enum QSEnumCompareType
    {
        [Description("大于")]
        Greater,
        [Description("大于等于")]
        GreaterEqual,
        [Description("小于")]
        Less,
        [Description("小于等于")]
        LessEqual,
        [Description("等于")]
        Equals,
        [Description("之内")]
        In,
        [Description("除外")]
        Out,
    }


    /// <summary>
    /// 止损 止盈方式
    /// </summary>
    public enum StopOffsetType
    {
        [Description("固定点数")]
        POINTS,
        [Description("固定价格")]
        PRICE,
        [Description("百分比")]
        PERCENT,
    }

    /// <summary>
    /// 止损 止盈方式
    /// </summary>
    public enum ProfitOffsetType
    {
        [Description("固定点数")]
        POINTS,
        [Description("固定价格")]
        PRICE,
        [Description("百分比")]
        PERCENT,
        [Description("跟踪")]
        TRAILING,
    }

    /// <summary>
    /// 止盈与止损方式
    /// </summary>
    public enum QSEnumPositionOffsetType
    {
        [Description("固定价格")]
        PRICE,
        [Description("固定点数")]
        POINTS,
        [Description("百分比")]
        PERCENT,
        [Description("跟踪")]
        TRAILING,
    }

    public enum QSEnumPositionOffsetDirection
    {
        [Description("止盈参数")]
        PROFIT,
        [Description("止损参数")]
        LOSS,
    }

    //加载账户类型
    public enum QSEnumAccountLoadMode
    {
        [Description("模拟与实盘")]
        ALL,//加载所有账户
        [Description("模拟")]
        SIM,//模拟账户
        [Description("实盘")]
        REAL,//实盘账户
    }
}
