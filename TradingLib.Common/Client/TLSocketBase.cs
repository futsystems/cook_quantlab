using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// TLScoketBase
    /// 用于封装底层Socket通讯
    /// </summary>
    public abstract class TLSocketBase
    {
        /// <summary>
        /// Socket消息事件
        /// </summary>
        public event Action<Message> MessageEvent;

        /// <summary>
        /// 处于Socket收到的消息
        /// </summary>
        /// <param name="message"></param>
        protected void HandleMessage(Message message)
        {
            
            if (MessageEvent != null)
            {
                MessageEvent(message);
            }
        }

        IPEndPoint _server;
        public IPEndPoint Server { get { return _server; } set { _server=value; } }


        /// <summary>
        /// Socket是否处于连接状态
        /// </summary>
        public abstract bool IsConnected { get; }

        /// <summary>
        /// 查询服务
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public abstract RspQryServiceResponse QryService(QSEnumAPIType apiType,string version);

        /// <summary>
        /// 连接
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// 断开连接
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public abstract void Send(byte[] msg);

    }
}
