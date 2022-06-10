using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.API;
using System.IO;

namespace TradingLib.Common
{
    public class XQryMinuteDataRequest : RequestPacket
    {
        public XQryMinuteDataRequest()
        {
            _type = MessageTypes.XQRYMINUTEDATA;
            this.Exchange = string.Empty;
            this.Symbol = string.Empty;
            this.Tradingday = 0;
            this.Start = DateTime.MinValue.ToTLDateTime();
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
        /// 交易日
        /// </summary>
        public int Tradingday { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public long Start { get; set; }

        public override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.Exchange);
            sb.Append(d);
            sb.Append(this.Symbol);
            sb.Append(d);
            sb.Append((int)this.Tradingday);
            sb.Append(d);
            sb.Append(this.Start);
            return sb.ToString();
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');
            this.Exchange = rec[0];
            this.Symbol = rec[1];
            this.Tradingday = int.Parse(rec[2]);
            this.Start = long.Parse(rec[3]);
        }


    }




    public class RspXQryMinuteDataResponse : RspResponsePacket
    {
        public QSEnumPacketType PacketType { get; protected set; }


        public RspXQryMinuteDataResponse()
        {
            _type = MessageTypes.XQRYMINUTEDATARESPONSE;
            this.MinuteDataList = new List<MinuteData>();
        }

        public void Add(MinuteData data)
        {
            this.MinuteDataList.Add(data);
        }


        //public bool IsHist { get; set; }
        public List<MinuteData> MinuteDataList { get; set; }

        /// <summary>
        /// Packet对应的底层传输的二进制数据 用于提供给底层传输传进行传输
        /// </summary>
        public override byte[] Data { get { return this.SerializeBin(); } }



        /// <summary>
        /// 消息内容
        /// 消息内容需要序列化对应的逻辑数据包
        /// </summary>
        public override string Content { get { return "Bin MinuteData Response"; } }


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
                        MinuteData md = MinuteData.Read(reader);
                        this.Add(md);
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
            for (int i = 0; i < this.MinuteDataList.Count; i++)
            {
                MinuteData.Write(b, this.MinuteDataList[i]);
            }
            byte[] zipData = ZlibNet.Compress(ms.ToArray());
            int size = (int)zipData.Length + 8 + 4 + 1;
            byte[] buffer = new byte[size];

            byte[] sizebyte = BitConverter.GetBytes(size);
            byte[] typebyte = BitConverter.GetBytes((int)this.Type);
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
        public override void Deserialize(string reqstr)
        {
            throw new NotImplementedException();
        }





        public static RspXQryMinuteDataResponse CreateResponse(XQryMinuteDataRequest request)
        {
            RspXQryMinuteDataResponse response = new RspXQryMinuteDataResponse();
            response.RequestID = request.RequestID;
            response.FrontID = request.FrontID;
            response.ClientID = request.ClientID;
            response.PacketType = QSEnumPacketType.RSPRESPONSE;

            return response;
        }


    }
}
