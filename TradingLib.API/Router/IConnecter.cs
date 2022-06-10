using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TradingLib.API
{
    public interface IConnecter
    {
        /// <summary>
        /// 连接标识
        /// </summary>
        string Token { get; }

        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 当前服务状态是否有效
        /// </summary>
        bool IsLive { get; }

        /// <summary>
        /// 启动服务
        /// </summary>
        void Start();

        /// <summary>
        /// 停止服务
        /// </summary>
        void Stop();

        /// <summary>
        /// 连接成功事件
        /// </summary>
        event IConnecterParamDel Connected;
        /// <summary>
        /// 连接断开事件
        /// </summary>
        event IConnecterParamDel Disconnected;

        
    }
}
