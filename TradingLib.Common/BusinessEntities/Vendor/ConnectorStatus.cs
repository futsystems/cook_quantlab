using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{

    public class ConnectorStatus
    {
        /// <summary>
        /// 通道设置ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 对应的通道唯一标识
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 通道状态
        /// </summary>
        public QSEnumConnectorStatus Status { get; set; }


    }
}
