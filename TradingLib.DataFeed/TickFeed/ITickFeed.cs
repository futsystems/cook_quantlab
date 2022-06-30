using System;
using System.Collections.Generic;
using TradingLib.API;

namespace TradingLib.DataFeed
{
    /// <summary>
    /// 实时行情Feed接口
    /// </summary>
    public interface ITickFeed
    {
        string Name { get; }

        /// <summary>
        /// 当前所连实时行情服务器
        /// </summary>
        string CurrentServer { get; }

        /// <summary>
        /// 注册前缀订阅
        /// 定于成交
        /// X,
        /// 订阅CLX6的成交数据
        /// X,CLX6
        /// 只需要将对应的实时行情前置注册进去 就可以实现行情订阅
        /// </summary>
        /// <param name="prefix"></param>
        void Register(string prefix);

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="prefix"></param>
        void Unregister(string prefix);

        /// <summary>
        /// 注册合约
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="exchange"></param>
        /// <param name="symbols"></param>
        void RegisterSymbols(string exchange, List<string> symbols);

        /// <summary>
        /// 行情到达事件
        /// </summary>
        event Action<ITickFeed,Tick> TickEvent;

        /// <summary>
        /// 连接建立事件
        /// </summary>
        event Action<ITickFeed> ConnectEvent;

        /// <summary>
        /// 连接断开事件
        /// </summary>
        event Action<ITickFeed> DisconnectEvent;

        /// <summary>
        /// 切换行情源服务器
        /// </summary>
        void SwitchTickSrv();

        /// <summary>
        /// 启动
        /// </summary>
        void Start();

        /// <summary>
        /// 停止
        /// </summary>
        void Stop();

        /// <summary>
        /// 是否处于运行中
        /// </summary>
        bool IsLive { get; }
    }
}