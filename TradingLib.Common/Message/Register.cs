///////////////////////////////////////////////////////////////////////////////////////
// 客户端注册
// 整个通讯逻辑过程如下
// 1.客户端发送RegisterClient数据包到服务端 服务端接受注册并记录该客户端Session
// 2.客户端发送Version到服务端 与服务端通讯后确认数据通道建立，这里涉及到协议版本与秘钥的交换与传递
// 3.客户端发送FeatureRequest获得服务端支持的操作码，从而建立相应的功能特性
// 4.客户端发送LoginRequest登入数据包 系统授权通过则会在系统内部Session管理系统内进行标注
// 5.客户端发送其他请求数据包 服务端进行响应处理 并返回
// 6.客户端发送UnregisterClient数据包到服务端 服务端将释放为该客户端所分配的相关资源并注销该客户端
// 
//
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    public class RegisterClientRequest:RequestPacket
    {
        public RegisterClientRequest()
        {
            _type = MessageTypes.REGISTERCLIENT;
            this.VersionToken = string.Empty;
            this.FrontType = EnumFrontType.Direct;
        }

        public EnumFrontType FrontType { get; set; }
        /// <summary>
        /// 版本标识
        /// 不同的版本标识之间不能相互登入
        /// </summary>
        public string VersionToken { get; set; }

        public override string ContentSerialize()
        {
            return string.Format("{0},{1}", this.FrontType, this.VersionToken);
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');
            if (rec.Length >= 2)
            {
                this.FrontType = rec[0].ParseEnum<EnumFrontType>();
                this.VersionToken = rec[1];
            }
        }
    }

    public class RspRegisterClientResponse : RspResponsePacket
    {
        public RspRegisterClientResponse()
        {
            _type = MessageTypes.REGISTERCLIENTRESPONSE;
            this.SessionID = string.Empty;
        }
        /// <summary>
        /// 回话编号
        /// 每个客户端连接到服务端均有一个SessionID对应
        /// </summary>
        public string SessionID { get; set; }

        public override string ResponseSerialize()
        {
            return string.Format("{0}", this.SessionID);
        }

        public override void ResponseDeserialize(string content)
        {
            string[] rec = content.Split(',');
            this.SessionID = rec[0];
        }
    }

    public class UnregisterClientRequest : RequestPacket
    {
        public UnregisterClientRequest()
        {
            _type = MessageTypes.CLEARCLIENT;
        }
        public override string ContentSerialize()
        {
            return "UnregisterClient";
        }
        
    }

   


}
