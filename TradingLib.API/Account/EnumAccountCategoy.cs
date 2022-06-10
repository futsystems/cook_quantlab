using System;
using System.ComponentModel;

namespace TradingLib.API
{
    /// <summary>
    /// 交易帐户类别
    /// </summary>
    public enum QSEnumAccountCategory
    {
        /// <summary>
        /// 子帐户可以用于交易客户端登入
        /// 同时接受交易客户端提交的交易指令
        /// </summary>
        [Description("子帐户")]
        SUBACCOUNT,

        /// <summary>
        /// 信号账户
        /// 信号账户用于绑定对应的交易通道,从交易通道获得交易历史数据和实时交易数据
        /// 1.用作跟单信号
        /// 2.主帐户监控
        /// </summary>
        [Description("信号帐户")]
        SIGACCOUNT,

        /// <summary>
        /// 监控帐户
        /// 监控帐户用于绑定对应的主帐户 实现主帐户交易数据的回复和采集
        /// 与信号类帐户的区别在于，信号类帐户不运行通过对应的通道进行交易
        /// 监控类帐户运行通过通道进行交易
        /// </summary>
        [Description("监控帐户")]
        MONITERACCOUNT,

        /// <summary>
        /// 策略帐户
        /// 策略帐户用于绑定到对应的交易策略(套利策略,跟单策略等)用于提供该策略下单
        /// 某个策略运行所触发的交易指令均通过策略帐户发送
        /// </summary>
        [Description("策略帐户")]
        STRATEGYACCOUNT,


    }
}
