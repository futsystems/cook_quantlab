///////////////////////////////////////////////////////////////////////////////////////
// 实现了逻辑包的抽象与统一
// PacketBase是所有逻辑数据包的父类
// RequestPacket,ResponsePacket继承了PacketBase，分别实现了请求包和回复包的相关特性
// ResponsePacket同时再次分化成两类
// RspResponsePacket用于查询类通讯，比如查询委托，查询成交，查询持仓等
// NotifyResponsePacket用于操作类通许，比如提交委托后系统将委托，成交，持仓等回报按序返回给客户端
// 
//
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

/*
 * 消息内容的解析
 * 所有的消息包均包含 RequestID
 * 普通逻辑包          RequestID|Content
 * 查询回报逻辑包      RequestID|ErrorID^ErrorMessage|Content
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * */

namespace TradingLib.Common
{


    /// <summary>
    /// 错误的汇报消息类型
    /// 比如插入委托或者委托操作失败 会附上具体的错误内容
    /// OnErrRtnOrderInsert
    /// OnErrRtnOrderAction
    /// </summary>
    public class ErrorNotifyResponsePacket : NotifyResponsePacket
    {
        public RspInfo RspInfo { get; set; }

        public ErrorNotifyResponsePacket()
        {
            RspInfo = new RspInfoImpl();
        }

        public sealed override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = '^';
            sb.Append(this.NotifySerialize());
            sb.Append(d);
            sb.Append(RspInfoImpl.Serialize(this.RspInfo));
            return sb.ToString();
        }

        char[] d = new char[] { '^' };
        public sealed override void ContentDeserialize(string contentstr)
        {
            try
            {
                string[] rec = contentstr.Split(d, 2);

                //解析查询回报信息
                this.RspInfo = RspInfoImpl.Deserialize(rec[1]);

                //解析具体的消息
                this.NotifyDeserialize(rec[0]);
            }
            catch (Exception ex)
            {
                RspInfo.ErrorID = 999;
                RspInfo.ErrorMessage = "协议解析错误";
            }
        }


        public virtual string NotifySerialize()
        {
            return "";    
        }

        public virtual void NotifyDeserialize(string notify)
        { 
            
        }
            
        
    }

    /// <summary>
    /// 通知类型的逻辑包,用于生成成交回报,委托回报,持仓更新回报等数据包 通知类的数据包不包含RspInfo
    /// 这些包调用OnRtnOrder,OnRtnTrade
    /// 
    /// </summary>
    public class NotifyResponsePacket : ResponsePacket
    { 

        
    }
    

    /// <summary>
    /// 查询回报逻辑数据包
    /// 用于回报客户端的查询,包含IsLast,RspIno信息
    /// 该数据包区别于交易过程中的Notify数据包
    /// </summary>
    public class RspResponsePacket : ResponsePacket
    {
        public RspResponsePacket()
        {
            IsLast = true;
            RspInfo = new RspInfoImpl();
        }

        /// <summary>
        /// 是否是最后一条回复
        /// </summary>
        public bool IsLast { get; set; }

        /// <summary>
        /// 查询回报信息 用于传递错误信息或内容
        /// </summary>
        public RspInfo RspInfo { get; set; }

        /// <summary>
        /// 将IsLast,RspIno附加到消息体的第一第二区域,具体内容放在第三区域
        /// </summary>
        /// <returns></returns>
        public sealed override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = '^';
            sb.Append(this.ResponseSerialize());
            sb.Append(d);
            sb.Append(IsLast.ToString());
            sb.Append(d);
            sb.Append(RspInfoImpl.Serialize(this.RspInfo));

            return sb.ToString();
        }

        char[] d = new char[] { '^' };
        /// <summary>
        /// 将回报消息进行解析,首先解析第一第二区域到IsLast,RspIno字段,然后调用具体内容的解析函数解析具体的内容到子类对象
        /// </summary>
        /// <param name="reqstr"></param>
        public  override void ContentDeserialize(string reqstr)
        {
            try
            {
                string[] rec = reqstr.Split(d, 3);

                //解析是否是最后数据标识
                bool islast = false;
                bool.TryParse(rec[1], out islast);
                IsLast = islast;

                //解析查询回报信息
                this.RspInfo = RspInfoImpl.Deserialize(rec[2]);

                //解析具体的消息
                this.ResponseDeserialize(rec[0]);
            }
            catch (Exception ex)
            {
                //Util.Debug(ex.ToString());
                RspInfo.ErrorID = 999;
                RspInfo.ErrorMessage = "协议解析错误";
            }
        }


        /// <summary>
        /// 具体内容序列化
        /// </summary>
        /// <returns></returns>
        public virtual string ResponseSerialize()
        {
            return string.Empty;
        }

        /// <summary>
        /// 具体内容反序列化
        /// </summary>
        /// <param name="content"></param>
        public virtual void ResponseDeserialize(string content)
        {

        }



    }


    /// <summary>
    /// 请求逻辑包父类
    /// </summary>
    public class RequestPacket : PacketBase
    {
        /// <summary>
        /// 设定请求数据包地址信息
        /// 用于服务端接收到数据后,创建对应的packet,并设定对应的地址信息
        /// 用于服务端解析到请求数据包时填入
        /// </summary>
        /// <param name="frontid"></param>
        /// <param name="clientid"></param>
        internal void SetSource(string frontid, string clientid)
        {
            this.FrontID = frontid;
            this.ClientID = clientid;
        }

        /// <summary>
        /// 设定请求ID,用于客户端发起请求生成请求逻辑包时候填充请求ID
        /// </summary>
        /// <param name="requestid"></param>
        /// <returns></returns>
        public void SetRequestID(int requestid)
        {
            this.RequestID = requestid;
        }


    }


    /// <summary>
    /// 回复逻辑包父类
    /// 系统内部存在不同种类的响应
    /// </summary>
    public class ResponsePacket:PacketBase
    {
        public ResponsePacket()
        {
            
        }

        /// <summary>
        /// 1.一问一答的响应，响应包用于应答某个请求,响应可以是是1个或者多个，响应包从Request获得请求者的地址信息 前置ID，客户端ID，以及对应的RequestID
        /// 并且将请求的FrontID RequestID数据传递给Response,从而系统可以准确的将该Response发送到对应的客户端
        /// 通过FrontID,ClientID进行寻址
        /// </summary>
        /// <param name="request"></param>
        public void BindRequest(RequestPacket request)
        {
            this.RequestID = request.RequestID;
            this.FrontID = request.FrontID;
            this.ClientID = request.ClientID;

            this.PacketType = QSEnumPacketType.RSPRESPONSE;
        }

        /// <summary>
        /// 该逻辑数据包底层对应帐户Account
        /// 2.绑定Account通过Account进行通知寻址
        /// 在交易消息交换中，绑定了某个account的通知包需要遍历该account的所有客户端连接并发送通知。某个account可以有多个交易客户端等同，需要同步通知到每个交易客户端
        /// 在管理消息交换中，绑定了某个account的通知包需要按照权限逻辑找到有权限查看该帐户的管理端进行通知
        /// </summary>
        public string Account { get; protected set; }

        /// <summary>
        /// 将该response绑定到对应的交易帐号,通过交易帐号寻址
        /// 交易客户端通过Account寻找对应的登入客户端
        /// </summary>
        /// <param name="account"></param>
        public void BindAccount(string account)
        {
            this.Account = account;
            this.PacketType = QSEnumPacketType.NOTIFYRESPONSE;
        }

        List<ILocation> _locatoins = new List<ILocation>();
        public List<ILocation> Locatioins { get { return _locatoins; } }
        /// <summary>
        /// 设定该通知所指定的地址
        /// 3.绑定通知地址，在某些通讯逻辑中直接绑定了该响应包的发送地址，按照设定的发送地址列表进行通知
        /// </summary>
        /// <param name="location"></param>
        public void BindLocation(IEnumerable<ILocation> locations)
        {
            Locatioins.Clear();
            Locatioins.AddRange(locations);

            this.PacketType = QSEnumPacketType.LOCATIONNOTIFYRESPONSE;
        }

        /// <summary>
        /// 扩展模块的回复消息通过ISession进行客户端定位
        /// 第一种类型的变种，用于从ISession获得地址信息 进行发送
        /// </summary>
        /// <param name="session"></param>
        public void BindSession(ISession session)
        {
            this.FrontID = session.Location.FrontID;
            this.ClientID = session.Location.ClientID;
            this.RequestID = session.RequestID;
            this.PacketType = QSEnumPacketType.RSPRESPONSE;
        }

        public void BindSession(string front, string clientid, int reqId)
        {
            this.FrontID = front;
            this.ClientID = clientid;
            this.RequestID = reqId;
            this.PacketType = QSEnumPacketType.RSPRESPONSE;
        }
    }

    /// <summary>
    /// Message用于包结构的打包与解包
    /// Packket将具体的Message解析成对应的逻辑数据
    /// 在系统内寻址有2种方案
    /// 1.通过frontid,uuid进行寻址
    /// 2.通过Account进行寻址,某个交易帐号可能有多个登入,因此通知类的消息就要通过Account寻址然后进行广播
    /// </summary>
    public class PacketBase : IPacket
    {
        public PacketBase()
        {
            FrontID = string.Empty;
            ClientID = string.Empty;
            RequestID = 0;
            PacketType = QSEnumPacketType.UNKNOWN;
            _type = MessageTypes.UNKNOWN_MESSAGE;
            TLProtoclType = EnumTLProtoclType.TL_Character;
            EncryptKey = "123456";

        }

        public QSEnumPacketType PacketType { get; protected set; }

        /// <summary>
        /// 请求数据包前置ID
        /// </summary>
        public string FrontID { get; protected set; }
        
        /// <summary>
        /// 请求数据包客户端Client
        /// </summary>
        public string ClientID { get; protected set; }

        /// <summary>
        /// 逻辑数据包RequestID
        /// </summary>
        public int RequestID { get; protected set; }

        /// <summary>
        /// 默认消息类型为未知类型
        /// </summary>
        protected MessageTypes _type = MessageTypes.UNKNOWN_MESSAGE;
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageTypes Type { get { return _type; } }


        public EnumTLProtoclType TLProtoclType { get; set; }

        public string EncryptKey { get; set; }

        /// <summary>
        /// Packet对应的底层传输的二进制数据 用于提供给底层传输传进行传输
        /// </summary>
        public virtual byte[] Data
        {
            get
            {
                //if (this.TLProtoclType == EnumTLProtoclType.TL_Encrypted)
                //{
                //    switch (this.Type)
                //    {
                //        case MessageTypes.XQRYMARKETTIME:
                //        //case MessageTypes.XMARKETTIMERESPONSE:
                //            {
                //                string encContent = StringCipher.Encrypt(Content, this.EncryptKey);
                //                return Message.sendmessage(Type, encContent);
                //            }
                //        default:
                //            break;
                //    }
                    
                //}
                //switch (this.Type)
                //{ 
                //    case MessageTypes.XQRYMARKETTIME:
                //        string encContent = StringCipher.Encrypt(Content, this.EncryptKey);
                //        return Message.sendmessage(Type, encContent);
                //    default:
                //        break;

                //}
                return Message.sendmessage(Type, Content);

            }
        
        }


        /// <summary>
        /// 消息内容
        /// 消息内容需要序列化对应的逻辑数据包
        /// </summary>
        public virtual string Content { get { return this.Serialize(); } }


        /// <summary>
        /// 二进制数据反序列化
        /// </summary>
        /// <param name="data"></param>
        public virtual void DeserializeBin(byte[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 序列化成二进制
        /// </summary>
        /// <returns></returns>
        public virtual byte[] SerializeBin()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 消息逻辑包是否有效 做一个消息的初步检验
        /// </summary>
        public virtual bool IsValid
        {

            get
            {
                return true;
            }
        }

        char[] d = new char[] { '|' };

        /// <summary>
        /// 序列化成字符串 由子类提供序列化函数
        /// </summary>
        /// <returns></returns>
        public  virtual string Serialize()
        {
            // StringBuilder sb = new StringBuilder();
            // sb.Append(this.ContentSerialize());
            // sb.Append("|");
            // sb.Append(RequestID.ToString());
            // return sb.ToString();
            return this.ContentSerialize();
        }


        /// <summary>
        /// 反序列化成对象
        /// </summary>
        /// <param name="reqstr"></param>
        /// <returns></returns>
        public virtual void Deserialize(string reqstr)
        {
            //LibUtil.Debug("deserialize -> " + reqstr);
            // //1.将消息内容按'|'进行第一次解析
            // string[] rec = reqstr.Split('|');
            // int reqid = 0;
            // int.TryParse(rec[1], out reqid);
            // this.RequestID = reqid;
            
            //2.将具体的内容进行解析
            //this.ContentDeserialize(rec[0]);
            this.ContentDeserialize(reqstr);
        }

        /// <summary>
        /// 逻辑包内容序列化
        /// </summary>
        /// <returns></returns>
        public virtual string ContentSerialize()
        {
            return string.Empty;
        }


        /// <summary>
        /// 逻辑包内容反序列化
        /// </summary>
        /// <param name="contentstr"></param>
        public virtual void ContentDeserialize(string contentstr)
        { 
            
        }


        public sealed override string ToString()
        {
            return "Packet Type:" + Type.ToString() + " Content:" + Content + " FrontID:"+FrontID +" Client:"+ClientID+" Size:" + this.Data.Length;
        }
    }
}
