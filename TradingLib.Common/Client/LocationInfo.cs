using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    public class LocationInfo
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 物理地址
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 本机MAC地址
        /// </summary>
        public string MAC { get; set; }

        public override string ToString()
        {
            return string.Format("IP:{0} Location:{1} MAC:{2}", this.IP, this.Location, this.MAC);
        }

        public static string Serialize(LocationInfo info)
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';

            sb.Append(info.IP);
            sb.Append(d);
            sb.Append(info.Location);
            sb.Append(d);
            sb.Append(info.MAC);
            return sb.ToString();
        }

        public static LocationInfo Deserialize(string content)
        {
            LocationInfo info = new LocationInfo();

            string[] rec = content.Split(',');
            info.IP = rec[0];
            info.Location = rec[1];
            info.MAC = rec[2];
            return info;

        }
    }

    
}
