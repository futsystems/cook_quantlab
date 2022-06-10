using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public enum QSEnumPacketType
    { 
        /// <summary>
        /// 未知
        /// </summary>
        UNKNOWN=0,

        /// <summary>
        /// 客户端过来提交上来的请求
        /// </summary>
        REQUEST=1,

        /// <summary>
        /// 服务端向某个客户端应答返回
        /// </summary>
        RSPRESPONSE=2,

        /// <summary>
        /// 通知类返回 通知某个交易帐号的状态
        /// 比如交易帐号同时有多个客户端登入,则需要向多个客户端发送通知
        /// 多个具有查看某个交易帐号的管理员登入管理端，则需要同时向这些管理端发送通知
        /// 交易客户端通过Account寻找对应客户端
        /// 管理客户端通过Account寻找有权限的管理客户端
        /// </summary>
        NOTIFYRESPONSE=3,

        /// <summary>
        /// 设定通知类型为定向地址通知
        /// </summary>
        LOCATIONNOTIFYRESPONSE=4,
    }

    public enum EnumTLProtoclType
    {
        /// <summary>
        /// TL字符串文本协议
        /// </summary>
        TL_Character,

        /// <summary>
        /// TL字符串文本协议加密
        /// </summary>
        TL_Encrypted,
    }

    /// <summary>
    /// 通讯消息Message用于系统底层通讯
    /// Package基于Message构成了具体消息的逻辑结构
    /// </summary>
    public interface IPacket
    {
        /// <summary>
        /// 对应的逻辑包类别
        /// </summary>
        QSEnumPacketType PacketType { get; }

        /// <summary>
        /// 前置ID
        /// </summary>
        string FrontID { get;}

        /// <summary>
        /// 请求数据包客户端Client
        /// </summary>
        string ClientID { get;}

        /// <summary>
        /// 获得消息具体内容
        /// </summary>
        byte[] Data { get;}

        /// <summary>
        /// 对应的消息类型
        /// </summary>
        MessageTypes Type { get;}

        /// <summary>
        /// TL协议类型 加密/明文
        /// </summary>
        EnumTLProtoclType TLProtoclType { get; set; }

        /// <summary>
        /// 加密密钥
        /// </summary>
        string EncryptKey { get; set; }

        /// <summary>
        /// 对应的消息内容
        /// </summary>
        string Content { get;}

        /// <summary>
        /// 序列化字符串到对象
        /// </summary>
        /// <param name="packetstr"></param>
        void Deserialize(string packetstr);

        /// <summary>
        /// 将对象内容序列化成字符串
        /// </summary>
        /// <returns></returns>
        string Serialize();

        /// <summary>
        /// 二进制数据反序列化
        /// </summary>
        /// <param name="data"></param>
        void DeserializeBin(byte[] data);

        /// <summary>
        /// 序列化成二进制
        /// </summary>
        /// <returns></returns>
        byte[] SerializeBin();

        /// <summary>
        /// 输出string用于打印
        /// </summary>
        /// <returns></returns>
        string ToString();
        
    }
}
