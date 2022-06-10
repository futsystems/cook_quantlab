using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
   

    public class TLNegotiation
    {

        public TLNegotiation()
        { 
            
        }
        /// <summary>
        /// 部署ID
        /// </summary>
        public string DeployID { get; set; }

        /// <summary>
        /// 操作系统ID
        /// </summary>
        public PlatformID PlatformID { get; set; }

        /// <summary>
        /// 版本编号字符串
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 产品类别
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// 协议类别
        /// </summary>
        public EnumTLProtoclType TLProtoclType { get; set; }


        /// <summary>
        /// 密钥
        /// 客户端与服务端建立连接后
        /// 初始化协议信息 服务端返回客户端一个动态口令
        /// 客户端通过该口令与服务端建立通讯
        /// 服务端动态口+客户端MAC地址拼接为一个可用口令
        /// 
        /// </summary>
        public string EncryptKey { get; set; }

        /// <summary>
        /// 通讯应答
        /// </summary>
        public string NegoResponse { get; set; }

        public static string Serialize(TLNegotiation nego)
        {
            char d = ',';
            StringBuilder sb = new StringBuilder();
            sb.Append(nego.DeployID);
            sb.Append(d);
            sb.Append(nego.PlatformID);
            sb.Append(d);
            sb.Append(nego.Version);
            sb.Append(d);
            sb.Append(nego.Product);
            sb.Append(d);
            sb.Append(nego.TLProtoclType);
            sb.Append(d);
            sb.Append(nego.EncryptKey);
            sb.Append(d);
            sb.Append(nego.NegoResponse);
            return sb.ToString();
        }

        public static TLNegotiation Deserialize(string content)
        {
            try
            {
                string[] rec = content.Split(',');
                TLNegotiation nego = new TLNegotiation();
                nego.DeployID = rec[0];
                nego.PlatformID = rec[1].ParseEnum<PlatformID>();
                nego.Version = rec[2];
                nego.Product = rec[3];
                nego.TLProtoclType = rec[4].ParseEnum<EnumTLProtoclType>();
                nego.EncryptKey = rec[5];
                nego.NegoResponse = rec[6];
                return nego;
            }
            catch (Exception ex)
            {

                return null;
            }

        }
    }
}
