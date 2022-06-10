using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 用于保存客户端的位置信息
    /// 记录前置编号,客户端UUID,以及全局分配的SessionID
    /// </summary>
    public class Location : ILocation
    {
        public Location()
            :this("","")
        {
        }
        public Location(string frontid, string clientid)
        {
            FrontID = frontid;
            ClientID = clientid;
            //SessionID = sessionid;
        }
        /// <summary>
        /// 前置地址
        /// </summary>
        public string FrontID { get; set; }

        /// <summary>
        /// 客户端UUID
        /// </summary>
        public string ClientID { get; set; }

        /// <summary>
        /// 客户端SessionID
        /// </summary>
        //public int SessionID { get; set; }
    }
}
