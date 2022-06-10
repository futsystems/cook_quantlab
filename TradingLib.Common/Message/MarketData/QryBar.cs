///////////////////////////////////////////////////////////////////////////////////////
// 用于查询历史行情
// 
//
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public enum EnumBarResponseType
    {
        PLAINTEXT,
        BINARY,
    }

    public class MDDemoTickRequest : RequestPacket
    {
        public MDDemoTickRequest()
        {
            _type = MessageTypes.MD_DEMOTICK;
        }

        /// <summary>
        /// 时间
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// 成交价格
        /// </summary>
        public decimal Trade { get; set; }

        public override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.Time);
            sb.Append(d);
            sb.Append(this.Trade);
            return sb.ToString();
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');
            this.Time = int.Parse(rec[0]);
            this.Trade = decimal.Parse(rec[1]);
        }



    }


    public class QryBarRequest:RequestPacket
    {
        public QryBarRequest()
        {
            _type = MessageTypes.BARREQUEST;
            this.Exchange = string.Empty;
            this.Symbol = string.Empty;
            this.IntervalType = BarInterval.CustomTime;
            this.Interval = 30;
            this.MaxCount = 800;
            this.StartIndex = 0;
            this.Start = DateTime.MinValue.ToTLDateTime();
            this.End = DateTime.MaxValue.ToTLDateTime();
            this.FromEnd = true;
            this.BarResponseType = EnumBarResponseType.PLAINTEXT;
            this.HavePartial = true;
        }

        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 间隔类别
        /// </summary>
        public BarInterval IntervalType { get; set; }

        /// <summary>
        /// 间隔数
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 开始时间 时间区间
        /// </summary>
        public long Start { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public long End { get; set; }

        /// <summary>
        /// 最大返回Bar个数
        /// </summary>
        public int MaxCount { get; set; }

        /// <summary>
        /// 返回数据开始位置 默认从0开始
        /// 比如查询2000个Bar 下次补充数据从 2000个开始再查2000个
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// 是否从最新的数据开始
        /// </summary>
        public bool FromEnd { get; set; }

        /// <summary>
        /// 包含Partial
        /// </summary>
        public bool HavePartial { get; set; }


        /// <summary>
        /// 返回方式
        /// </summary>
        public EnumBarResponseType BarResponseType { get; set; }

        public override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d=',';
            sb.Append(this.Exchange);
            sb.Append(d);
            sb.Append(this.Symbol);
            sb.Append(d);
            sb.Append((int)this.IntervalType);
            sb.Append(d);
            sb.Append(this.Interval);
            sb.Append(d);
            sb.Append(this.Start);
            sb.Append(d);
            sb.Append(this.End);
            sb.Append(d);
            sb.Append(this.StartIndex);
            sb.Append(d);
            sb.Append(this.MaxCount);
            sb.Append(d);
            sb.Append(this.FromEnd);
            sb.Append(d);
            sb.Append(this.BarResponseType);
            sb.Append(d);
            sb.Append(this.HavePartial);
            return sb.ToString();
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');
            this.Exchange = rec[0];
            this.Symbol = rec[1];
            this.IntervalType = (BarInterval)int.Parse(rec[2]);
            this.Interval = int.Parse(rec[3]);
            this.Start = long.Parse(rec[4]);
            this.End = long.Parse(rec[5]);
            this.StartIndex = int.Parse(rec[6]);
            this.MaxCount = int.Parse(rec[7]);
            this.FromEnd = bool.Parse(rec[8]);
            this.BarResponseType = (EnumBarResponseType)Enum.Parse(typeof(EnumBarResponseType), rec[9]);
            this.HavePartial = bool.Parse(rec[10]);
        }


    }

    public class RspQryBarResponse : RspResponsePacket
    {
        public RspQryBarResponse()
        {
            _type = MessageTypes.BARRESPONSE;
            this.Bar = null;
        }

        public Bar Bar { get; set; }
        public override string ResponseSerialize()
        {
            if (this.Bar == null)
                return string.Empty;
            return BarImpl.Serialize(this.Bar);
        }

        public override void ResponseDeserialize(string content)
        {
            if (string.IsNullOrEmpty(content))
                this.Bar = null;
            else
                this.Bar = BarImpl.Deserialize(content);
        }
    }



    public class RspQryBarResponseBin:RspResponsePacket
    {
        public QSEnumPacketType PacketType { get; protected set; }

        /// <summary>
        /// 请求数据包前置ID
        /// </summary>
        //public string FrontID { get; protected set; }

        /// <summary>
        /// 请求数据包客户端Client
        /// </summary>
        //public string ClientID { get; protected set; }

        /// <summary>
        /// 逻辑数据包RequestID
        /// </summary>
        //public int RequestID { get; protected set; }


        /// <summary>
        /// Packet对应的底层传输的二进制数据 用于提供给底层传输传进行传输
        /// </summary>
        public override  byte[] Data { get { return this.SerializeBin(); } }

        /// <summary>
        /// 默认消息类型为未知类型
        /// </summary>
        //protected MessageTypes _type = MessageTypes.UNKNOWN_MESSAGE;

        /// <summary>
        /// 消息类型
        /// </summary>
        //public MessageTypes Type { get { return _type; } }

        /// <summary>
        /// 消息内容
        /// 消息内容需要序列化对应的逻辑数据包
        /// </summary>
        public override string Content { get { return "Bin Bar Response"; } }


        /// <summary>
        /// 二进制数据反序列化
        /// </summary>
        /// <param name="data"></param>
        public override void DeserializeBin(byte[] data)
        {
            this.RequestID = BitConverter.ToInt32(data, 0);
            this.IsLast = BitConverter.ToBoolean(data, 4);
            byte[] zipData = new byte[data.Length - 5];
            Array.Copy(data, 5, zipData, 0, data.Length - 5);
            byte[] rawData = ZlibNet.Decompress(zipData);

            using (MemoryStream ms = new MemoryStream(rawData))
            {
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    while (ms.Position < ms.Length)
                    {
                        BarImpl bar = BarImpl.Read(reader);
                        this.Add(bar);
                    }
                }
            }
        }

        
        
        /// <summary>
        /// 序列化成二进制
        /// </summary>
        /// <returns></returns>
        public override byte[] SerializeBin()
        {
            //MemoryStream会根据写入的数据自动扩充底层数组大小
            
            MemoryStream ms = new MemoryStream();
            BinaryWriter b = new BinaryWriter(ms);
            for (int i = 0; i < this.Bars.Count; i++)
            {
                BarImpl.Write(b, this.Bars[i]);
            }

            byte[] zipData = ZlibNet.Compress(ms.ToArray());
            int size = (int)zipData.Length + 8 + 4 + 1;
            byte[] buffer = new byte[size];
            
            byte[] sizebyte = BitConverter.GetBytes(size);
            byte[] typebyte = BitConverter.GetBytes((int)MessageTypes.BIN_BARRESPONSE);
            byte[] requestidbyte = BitConverter.GetBytes(this.RequestID);
            byte[] islastbyte = BitConverter.GetBytes(this.IsLast);

            Array.Copy(sizebyte, 0, buffer, 0, sizebyte.Length);
            Array.Copy(typebyte, 0, buffer, 4, typebyte.Length);
            Array.Copy(requestidbyte, 0, buffer, 8, requestidbyte.Length);
            Array.Copy(islastbyte, 0, buffer, 8 + 4, islastbyte.Length);

            Array.Copy(zipData, 0, buffer, 8 + 4 + 1, zipData.Length);
            return buffer;
        }


        /// <summary>
        /// 序列化成字符串 由子类提供序列化函数
        /// </summary>
        /// <returns></returns>
        public override string Serialize()
        {
            throw new NotImplementedException();
        }

        

        /// <summary>
        /// 反序列化成对象
        /// </summary>
        /// <param name="reqstr"></param>
        /// <returns></returns>
        public override  void Deserialize(string reqstr)
        {
            throw new NotImplementedException();
        }


        public RspQryBarResponseBin()
        {
            _type = MessageTypes.BIN_BARRESPONSE;
            this.Bars = new List<BarImpl>();
        }

        public void Add(BarImpl bar)
        {
            this.Bars.Add(bar);
        }

        
        public List<BarImpl> Bars { get; set; }


        public static RspQryBarResponseBin CreateResponse(QryBarRequest request)
        {
            RspQryBarResponseBin response = new RspQryBarResponseBin();
            response.RequestID = request.RequestID;
            response.FrontID = request.FrontID;
            response.ClientID = request.ClientID;
            response.PacketType = QSEnumPacketType.RSPRESPONSE;

            return response;
        }


    }

}
