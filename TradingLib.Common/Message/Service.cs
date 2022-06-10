using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{

    public enum QSEnumAPIType
    { 
        ERROR=0,

        MD_ZMQ=10,
        /// <summary>
        /// 行情Sockt接口
        /// </summary>
        MD_Socket=11,
        /// <summary>
        /// 行情WebSocket接口
        /// </summary>
        MD_WebSocket=12,
    }
    /// <summary>
    /// 服务查询
    /// </summary>
    public class QryServiceRequest : RequestPacket
    {
        public QryServiceRequest()
        {
            _type = MessageTypes.SERVICEREQUEST;
        }

        /// <summary>
        /// API接口版本
        /// </summary>
        public string APIVersion { get; set; }


        public QSEnumAPIType APIType { get; set; }

        public override string ContentSerialize()
        {
            return string.Format("{0},{1}", (int)this.APIType, this.APIVersion);
        }

        public override void ContentDeserialize(string contentstr)
        {
            try
            {
                string[] rec = contentstr.Split(',');
                
                this.APIType = (QSEnumAPIType)int.Parse(rec[0]);
                this.APIVersion = rec[1];
            }
            catch (Exception ex)
            {
                this.APIVersion = string.Empty;
                this.APIType = QSEnumAPIType.ERROR;
            }
            
        }

    }

    /// <summary>
    /// 服务回报
    /// </summary>
    public class RspQryServiceResponse : RspResponsePacket
    {
        public RspQryServiceResponse()
        {
            _type = MessageTypes.SERVICERESPONSE;
        }

        /// <summary>
        /// 服务端API版本
        /// </summary>
        public string APIVersion { get; set; }

        /// <summary>
        /// 服务端API类别
        /// </summary>
        public QSEnumAPIType APIType { get; set; }

        /// <summary>
        /// 是否可以提供服务
        /// </summary>
        public bool OnService { get; set; }

        public override string ResponseSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append((int)APIType);
            sb.Append(d);
            sb.Append(APIVersion);
            sb.Append(d);
            sb.Append(this.OnService);
            return sb.ToString();

        }

        public override void ResponseDeserialize(string content)
        {
            try
            {
                string[] rec = content.Split(',');
                this.APIType = (QSEnumAPIType)int.Parse(rec[0]);
                this.APIVersion = rec[1];
                this.OnService = bool.Parse(rec[2]);
            }
            catch (Exception ex)
            {
                this.OnService = false;
                this.APIType = QSEnumAPIType.ERROR;
                this.APIVersion = string.Empty;
            }
        }

    }
}
