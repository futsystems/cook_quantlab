///////////////////////////////////////////////////////////////////////////////////////
// 用于查询最大保单数量
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
    public class QryMaxOrderVolRequest:RequestPacket
    {
        /// <summary>
        /// 交易帐户
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 持仓
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// 方向
        /// </summary>
        public bool Side { get; set; }
        /// <summary>
        /// 开平标识
        /// </summary>
        public QSEnumOffsetFlag OffsetFlag { get; set; }


        public QryMaxOrderVolRequest()
        {
            _type = MessageTypes.QRYMAXORDERVOL;
            Account = string.Empty;
            Symbol = string.Empty;
            Side = false;
            OffsetFlag = QSEnumOffsetFlag.UNKNOWN;
            
        }
        public override bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(Account))
                    return false;
                return true;
            }
        }

        public override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(Account);
            sb.Append(d);
            sb.Append(Symbol);
            sb.Append(d);
            sb.Append(Side.ToString());
            sb.Append(d);
            sb.Append(OffsetFlag.ToString());
            return sb.ToString();
        }

        public override void ContentDeserialize(string reqstr)
        {
            string[] rec = reqstr.Split(',');
            Account = rec[0];
            Symbol = rec[1];
            Side = bool.Parse(rec[2]);
            QSEnumOffsetFlag offset = QSEnumOffsetFlag.OPEN;
            Enum.TryParse<QSEnumOffsetFlag>(rec[3], out offset);//(QSEnumOffsetFlag)Enum.TryParse(typeof(QSEnumOffsetFlag), rec[2]);
            OffsetFlag = offset;
           
        }


    }

    public class RspQryMaxOrderVolResponse : RspResponsePacket
    {

        public RspQryMaxOrderVolResponse()
        {
            Symbol = string.Empty;
            MaxVol = 0;
            Side = true;
            OffsetFlag = QSEnumOffsetFlag.UNKNOWN;
            _type = MessageTypes.MAXORDERVOLRESPONSE;
        }


        public string Symbol { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        public bool Side { get; set; }
        /// <summary>
        /// 开平标识
        /// </summary>
        public QSEnumOffsetFlag OffsetFlag { get; set; }


        public int MaxVol { get; set; }

        public override string ResponseSerialize()
        {
            //Util.Debug("response serialized: side:" + Side.ToString(), QSEnumDebugLevel.ERROR);
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(Symbol);
            sb.Append(d);
            sb.Append(Side.ToString());
            sb.Append(d);
            sb.Append(((int)OffsetFlag).ToString());
            sb.Append(d);
            sb.Append(MaxVol.ToString());
            return sb.ToString();
            
        }

        public override void ResponseDeserialize(string content)
        {
            string[] rec = content.Split(',');
            Symbol = rec[0];
            Side = bool.Parse(rec[1]);
            QSEnumOffsetFlag offset = QSEnumOffsetFlag.OPEN;
            Enum.TryParse<QSEnumOffsetFlag>(rec[2], out offset);//(QSEnumOffsetFlag)Enum.TryParse(typeof(QSEnumOffsetFlag), rec[2]);
            OffsetFlag = offset;
            MaxVol = int.Parse(rec[3]);
        }
    }
}
