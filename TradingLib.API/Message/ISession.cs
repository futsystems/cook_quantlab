using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TradingLib.API
{
    public delegate void ISessionDel(ISession session);
    public delegate void IPacketSessionDelegate(IPacket packet,ISession session);

    public enum QSEnumSessionType
    { 
        /// <summary>
        /// 交易客户端session
        /// </summary>
        CLIENT,
        /// <summary>
        /// 管理端session
        /// </summary>
        MANAGER,

    }


    /// <summary>
    /// 在扩展模块相应网络端消息时,函数调用需要提供一个支持ISession接口的对象,用于标注客户端位置并向该客户端端发送消息
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// 获得该会话对端地址
        /// </summary>
        ILocation Location { get; }

        /// <summary>
        /// 回话类型
        /// </summary>
        QSEnumSessionType SessionType { get; }

        /// <summary>
        /// 是否已经认证授权
        /// </summary>
        bool Authorized { get; }

        /// <summary>
        /// 授权ID
        /// 交易帐户授权ID为交易帐号
        /// 管理员帐户授权ID为管理员登入ID
        /// </summary>
        string AuthorizedID { get; }

        /// <summary>
        /// 前置编号 整数
        /// </summary>
        int FrontIDi { get; }

        /// <summary>
        /// 客户连接编号 整数
        /// </summary>
        int SessionIDi { get; }

        /// <summary>
        /// 对应的扩展模块编号
        /// </summary>
        string ContirbID { get; set; }

        /// <summary>
        /// 对应的扩展模块命令
        /// </summary>
        string CMDStr { get; set; }

        /// <summary>
        /// 对端请求编号
        /// </summary>
        int RequestID { get; set; }


    }
}
