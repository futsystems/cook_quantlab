///////////////////////////////////////////////////////////////////////////////////////
// 版本交换与密钥交换
// 
//
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TradingLib.Common
{
//////////////////////////////////////////////////////////////
//用于客户端与服务端交互版本信息,客户端向服务端告知版本信息
//服务端向客户端通知服务端版本信息
//////////////////////////////////////////////////////////////
    /// <summary>
    /// TL协议加密通讯过程
    /// 1.RegisterClient 客户端与服务端建立连接向系统登记终端,系统分配给终端一个UUID作为该终端的唯一标识
    /// 2.VersionRequest 客户端随机产生PassPhrase，待加密字符串以及加密后的UUID提交到服务端
    /// 3.服务端根据客户端提交的随机PassPhrase和加密的UUI还原UUID如果UUID一致则表明该终端 可以使用加密消息处理
    /// 4.同时使用随机PassPhrase加密待加密字符串在VersionResponse中发送给客户端，客户端解密验证后用于验证服务端是否匹配
    /// </summary>
    public class VersionRequest : RequestPacket
    {
        public VersionRequest()
        {
            _type = API.MessageTypes.VERSIONREQUEST;
        }

        public string ClientVersion {get;set;}

        public string DeviceType {get;set;}

        /// <summary>
        /// PassPhrase
        /// </summary>
        public string NegotiationKey { get; set; }

        /// <summary>
        /// 待加密字符串
        /// </summary>
        public string NegotiationString { get; set; }

        /// <summary>
        /// 加密的UUID
        /// </summary>
        public string EncryptUUID { get; set; }

        public override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.ClientVersion);
            sb.Append(d);
            sb.Append(this.DeviceType);
            sb.Append(d);
            sb.Append(this.NegotiationKey);
            sb.Append(d);
            sb.Append(this.NegotiationString);
            sb.Append(d);
            sb.Append(this.EncryptUUID);


            return sb.ToString();
        }

        public override void ContentDeserialize(string reqstr)
        {
            string [] rec = reqstr.Split(',');
            this.ClientVersion = rec[0];
            this.DeviceType = rec[1];
            this.NegotiationKey = rec[2];
            this.NegotiationString = rec[3];
            this.EncryptUUID = rec[4];
            
        }
    }


    public class VersionResponse : RspResponsePacket
    {
        public VersionResponse()
        {
            _type = API.MessageTypes.VERSIONRESPONSE;
            this.Negotiation = null;
        }

        //public TLVersion Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TLNegotiation Negotiation { get; set; }
        public override string ResponseSerialize()
        {
            if (this.Negotiation == null)
                return string.Empty;
            return TLNegotiation.Serialize(this.Negotiation);
        }


        public override void ResponseDeserialize(string repstr)
        {
            if (string.IsNullOrEmpty(repstr))
                this.Negotiation = null;
            this.Negotiation = TLNegotiation.Deserialize(repstr);
        }
        
    }
}
