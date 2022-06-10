using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 启动DataFeed
    /// </summary>
    public class MDReqStartDataFeedRequest : RequestPacket
    {
        public MDReqStartDataFeedRequest()
        {
            _type = MessageTypes.MGR_MD_STARTDATAFEED;
            this.DataFeed = QSEnumDataFeedTypes.CTP;
        }

        public QSEnumDataFeedTypes DataFeed { get; set; }

        public override string ContentSerialize()
        {
            return ((int)this.DataFeed).ToString();
        }

        public override void ContentDeserialize(string contentstr)
        {
            this.DataFeed = (QSEnumDataFeedTypes)Enum.Parse(typeof(QSEnumDataFeedTypes), contentstr);
        }
    }

    /// <summary>
    /// 停止DataFeed
    /// </summary>
    public class MDReqStopDataFeedRequest : RequestPacket
    {
        public MDReqStopDataFeedRequest()
        {
            _type = MessageTypes.MGR_MD_STOPDATAFEED;
        }

        public QSEnumDataFeedTypes DataFeed { get; set; }

        public override string ContentSerialize()
        {
            return ((int)this.DataFeed).ToString();
        }

        public override void ContentDeserialize(string contentstr)
        {
            this.DataFeed = (QSEnumDataFeedTypes)Enum.Parse(typeof(QSEnumDataFeedTypes), contentstr);
        }
    }


    /// <summary>
    /// 注册合约数据
    /// 
    /// </summary>
    public class MDRegisterSymbolsRequest : RequestPacket
    {
        public MDRegisterSymbolsRequest()
        {
            _type = MessageTypes.MGR_MD_REGISTERSYMBOLS;
            this.SymbolList = new List<string>();
            this.Exchange = string.Empty;
        }

        /// <summary>
        /// 合约
        /// </summary>
        public List<string> SymbolList { get; set; }


        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange { get; set; }


        public override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.Exchange);
            sb.Append(d);
            string str = string.Empty;
            if(this.SymbolList!= null && this.SymbolList.Count>0)
            {
                str = string.Join(" ",this.SymbolList.ToArray());
            }
            sb.Append(str);

            return sb.ToString();
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');
            this.Exchange = rec[0];
            string[] syms = rec[1].Split(' ');
            this.SymbolList.Clear();
            foreach(var symbol in syms)
            {
                this.SymbolList.Add(symbol);
            }
        }

    }


    public class MDSetSymbolFilter : RequestPacket
    {
        public MDSetSymbolFilter()
        {
            _type = MessageTypes.MGR_MD_SETSYMBOLFILTER;
            this.DataFeed = QSEnumDataFeedTypes.DEFAULT;
            this.SymbolList = new List<string>();
        }

        /// <summary>
        /// 合约
        /// </summary>
        public List<string> SymbolList { get; set; }
        /// <summary>
        /// 行情源
        /// </summary>
        public QSEnumDataFeedTypes DataFeed { get; set; }

        public override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append((int)this.DataFeed);
            sb.Append(d);
            string str = string.Empty;
            if (this.SymbolList != null && this.SymbolList.Count > 0)
            {
                str = string.Join(" ", this.SymbolList.ToArray());
            }
            sb.Append(str);

            return sb.ToString();
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');
            this.DataFeed = (QSEnumDataFeedTypes)Enum.Parse(typeof(QSEnumDataFeedTypes), rec[0]);
            string[] syms = rec[1].Split(' ');
            this.SymbolList.Clear();
            foreach (var symbol in syms)
            {
                this.SymbolList.Add(symbol);
            }
        }
    }

    /// <summary>
    /// 请求查询已注册合约
    /// </summary>
    public class MDQrySymbolsRegistedRequest : RequestPacket
    {
        public MDQrySymbolsRegistedRequest()
        {
            _type = MessageTypes.MGR_MD_QRYSYMBOLSREGISTED;
            this.Exchange = "";
        }

        /// <summary>
        /// 行情源
        /// </summary>
        public string Exchange { get; set; }

        public override string ContentSerialize()
        {
            return this.Exchange;
        }

        public override void ContentDeserialize(string contentstr)
        {
            this.Exchange = contentstr;
        }
    }

    public class RspMDQrySymbolsRegistedResponse : RspResponsePacket
    {
        public RspMDQrySymbolsRegistedResponse()
        {
            _type = MessageTypes.MGR_MD_QRYSYMBOLSREGISTEDRESPONSE;
            this.SymbolList = new List<string>();
            this.Exchange = string.Empty;

        }
        

        /// <summary>
        /// 合约
        /// </summary>
        public List<string> SymbolList { get; set; }


        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange { get; set; }

        public override string  ResponseSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.Exchange);
            sb.Append(d);
            string str = string.Empty;
            if (this.SymbolList != null && this.SymbolList.Count > 0)
            {
                str = string.Join(" ", this.SymbolList.ToArray());
            }
            sb.Append(str);

            return sb.ToString();
        }


        public override void ContentDeserialize(string content)
        {
            string[] rec = content.Split(',');
            this.Exchange = rec[0];
            if (!string.IsNullOrEmpty(rec[1]))
            {
                string[] syms = rec[1].Split(' ');
                this.SymbolList.Clear();
                
                foreach (var symbol in syms)
                {
                    this.SymbolList.Add(symbol);
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct UploadBarDataRequestHeader
    {
        /// <summary>
        /// 请求ID
        /// </summary>
        public int RequestID;

        /// <summary>
        /// Bar数量
        /// </summary>
        public int BarCount;

        /// <summary>
        /// 交易所
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string Exchange;

        /// <summary>
        /// 合约
        /// </summary>

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string Symbol;

        /// <summary>
        /// 周期数量
        /// </summary>
        public int Interval;

        /// <summary>
        /// 周期类别
        /// </summary>
        public BarInterval IntervalType;

    }
    /// <summary>
    /// 上传Bar历史数据
    /// </summary>
    public class UploadBarDataRequest : RequestPacket
    {
        public UploadBarDataRequest()
        {
            _type = MessageTypes.MGR_MD_UPLOADBARDATA;
            this.Header = new UploadBarDataRequestHeader();
            this.Bars = new List<BarImpl>();
        }

        public void Add(BarImpl bar)
        {
            this.Bars.Add(bar);
        }

        /// <summary>
        /// 请求头部
        /// </summary>
        public UploadBarDataRequestHeader Header;

        /// <summary>
        /// Bar数据
        /// </summary>
        public List<BarImpl> Bars { get; set; }

        

        /// <summary>
        /// Packet对应的底层传输的二进制数据 用于提供给底层传输传进行传输
        /// </summary>
        public override byte[] Data { get { return this.SerializeBin(); } }


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
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    byte[] headerbyte = reader.ReadBytes(Marshal.SizeOf(typeof(UploadBarDataRequestHeader)));
                    this.Header = UtilStruct.BytesToStruct<UploadBarDataRequestHeader>(headerbyte);
                    List<BarImpl> barlsit = new List<BarImpl>();
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
            
            byte[] headerbyte = UtilStruct.StructToBytes(this.Header);

            int size = 4 + 4 + headerbyte.Length + (int)ms.Length;
            byte[] sizebyte = BitConverter.GetBytes(size);
            byte[] typebyte = BitConverter.GetBytes((int)this.Type);

            byte[] buffer = new byte[size];

            Array.Copy(sizebyte, 0, buffer, 0, sizebyte.Length);
            Array.Copy(typebyte, 0, buffer, 4, typebyte.Length);

            Array.Copy(headerbyte, 0, buffer, 8, headerbyte.Length);


            Array.Copy(ms.GetBuffer(), 0, buffer, 8 + headerbyte.Length, ms.Length);
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

        //public static RspQryBarResponseBin CreateResponse(QryBarRequest request)
        //{
        //    RspQryBarResponseBin response = new RspQryBarResponseBin();
        //    response.RequestID = request.RequestID;
        //    response.FrontID = request.FrontID;
        //    response.ClientID = request.ClientID;
        //    response.PacketType = QSEnumPacketType.RSPRESPONSE;

        //    return response;
        //}
    }
}
