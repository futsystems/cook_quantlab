using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{

    public enum QSEnumFollowProtectValueType
    { 
        /// <summary>
        /// 按点数
        /// </summary>
        Point,
        /// <summary>
        /// 按百分比
        /// </summary>
        Percent,
    }
    /// <summary>
    /// 跟单方向
    /// </summary>
    public enum QSEnumFollowDirection
    {
        [Description("正向")]
        Positive,
        [Description("反向")]
        Reverse,
    }

    /// <summary>
    /// 跟单价格类别
    /// </summary>
    public enum QSEnumFollowPriceType
    { 
        /// <summary>
        /// 以市价方式交易
        /// </summary>
        [Description("市价")]
        MarketPrice,
        /// <summary>
        /// 以当时的盘口对手价格交易
        /// 买入以卖价报单 卖出以买价报单
        /// </summary>
        [Description("对手价")]
        OpponentPrice,
        /// <summary>
        /// 以当时的盘口挂单价格交易
        /// 买入以买价报单 卖出以卖价报单
        /// </summary>
        [Description("挂单价")]
        HangingPrice,

        /// <summary>
        /// 以成交信号价格偏移一定点数进行报单
        /// </summary>
        [Description("信号价")]
        SignalPrice,

    }

    /// <summary>
    /// 挂单触发操作阀值
    /// 1.时间，报单一定时间后没有成交 则触发操作
    /// 2.价格，报单后价格达到一定波动超过一定范围 则触发操作
    /// </summary>
    public enum QSEnumPendingThresholdType
    {
        [Description("时间触发")]
        ByTime,

        [Description("价格触发")]
        ByTicks,
    }

    /// <summary>
    /// 挂单处理方式
    /// </summary>
    public enum QSEnumPendingOperationType
    { 
        [Description("撤单")]
        Cancel,
        [Description("市价追单")]
        ByMarket,
    }

    /// <summary>
    /// 持仓事件类型
    /// 开仓/平仓
    /// </summary>
    public enum QSEnumPositionEventType
    {
        /// <summary>
        /// 开仓
        /// </summary>
        [Description("开仓")]
        EntryPosition,

        /// <summary>
        /// 平仓
        /// </summary>
        [Description("平仓")]
        ExitPosition,
    }

    /// <summary>
    /// 跟单操作类别
    /// 某个跟单项目在跟单引擎中进行处理
    /// 根据跟单项与跟单策略参数会产生相应跟单操作
    /// 
    /// </summary>
    public enum QSEnumFollowActionType
    {
        [Description("等待")]
        Wait,
        [Description("提交委托")]
        PlaceOrder,
        [Description("取消委托")]
        CancelOrder,
        [Description("关闭跟单项")]
        CloseItem
    }

    /// <summary>
    /// 跟单项类别
    /// </summary>
    public enum QSEnumFollowItemTriggerType
    { 
        /// <summary>
        /// 信号源成交触发
        /// </summary>
        [Description("成交")]
        SigTradeTrigger,

        /// <summary>
        /// 信号源委托触发
        /// </summary>
        [Description("委托")]
        SigOrderTrigger,

        /// <summary>
        /// 手工平仓触发
        /// </summary>
        [Description("人工")]
        ManualExitTrigger,

        /// <summary>
        /// 策略平仓触发
        /// </summary>
        [Description("策略")]
        StrategyExitTrigger,
    }

    /// <summary>
    /// 跟单项状态
    /// </summary>
    public enum QSEnumFollowStage
    {
        /// <summary>
        /// 根据信号源事件创建跟单项目ItemCreated
        /// </summary>
        [Description("新建")]
        ItemCreated,

        /// <summary>
        /// 按照跟单策略规则生成委托并发送
        /// </summary>
        [Description("已发送")]
        FollowOrderSent,

        /// <summary>
        /// 跟单委托待成交
        /// </summary>
        [Description("待成交")]
        FollowOrderOpened,

        /// <summary>
        /// 跟单委托已经全部成交
        /// </summary>
        [Description("全成")]
        FollowOrderFilled,

        /// <summary>
        /// 跟单委托已经全部成交
        /// </summary>
        [Description("部成")]
        FollowOrderPartFilled,

        /// <summary>
        /// 跟单委托被扯单
        /// </summary>
        [Description("发送撤单")]
        FollowOrderCancelSent,

        /// <summary>
        /// 跟单委托被扯单
        /// </summary>
        [Description("确认撤单")]
        FollowOrderCanceled,

        /// <summary>
        /// 委托被拒绝
        /// </summary>
        [Description("拒绝")]
        FollowOrderReject,

        [Description("关闭")]
        ItemClosed
    }

    /// <summary>
    /// 信号类别
    /// </summary>
    public enum QSEnumSignalType
    {
        [Description("外部通道")]
        Connector,
        [Description("内部帐户")]
        Account,
    }

    /// <summary>
    /// 跟单工作状态
    /// 
    /// </summary>
    public enum QSEnumFollowWorkState
    { 
        /// <summary>
        /// 开始工作 接受信号并处理信号
        /// </summary>
        [Description("开始")]
        Begin,

        /// <summary>
        /// 停止处理开仓信号 当前持仓等待平仓信号处理
        /// </summary>
        [Description("暂停")]
        Suspend,

        /// <summary>
        /// 停止跟单策略 该动作会平掉所有持仓
        /// </summary>
        [Description("停止")]
        Shutdown,

    }
}
