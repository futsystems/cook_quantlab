using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.Common
{
    
    /// <summary>
    /// 通道状态
    /// </summary>
    public class ConnectorInfo
    {
        /// <summary>
        /// 通道token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 通道名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否处于工作状态
        /// </summary>
        public bool IsLive { get; set; }

        /// <summary>
        /// 通道类别
        /// </summary>
        public QSEnumConnectorType Type { get; set; }

        public ConnectorInfo()
        {
            this.Token = string.Empty;
            this.IsLive = false;
            this.Name = string.Empty;
            this.Type = QSEnumConnectorType.Broker;
        }

        public ConnectorInfo(IBroker broker)
        {
            this.Token = broker.Token;
            this.Name = broker.Name;
            this.IsLive = broker.IsLive;
            this.Type = QSEnumConnectorType.Broker;
        }


        public ConnectorInfo(IDataFeed df)
        {
            this.Token = df.Token;
            this.Name = df.Name;
            this.IsLive = df.IsLive;
            this.Type = QSEnumConnectorType.DataFeed;
        }
       

        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.Token);
            sb.Append(d);
            sb.Append(this.Name);
            sb.Append(d);
            sb.Append(this.IsLive);
            sb.Append(d);
            sb.Append(this.Type);
            return sb.ToString();
        }

        public void Deserialize(string content)
        {
            string[] rec = content.Split(',');
            this.Token = rec[0];
            this.Name = rec[1];
            this.IsLive = bool.Parse(rec[2]);
            this.Type = (QSEnumConnectorType)Enum.Parse(typeof(QSEnumConnectorType), rec[3]);
        }
    }
}
