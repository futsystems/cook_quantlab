///////////////////////////////////////////////////////////////////////////////////////
// 用于客户端向服务端查询服务端标识,如果服务无需则返回unknown provider
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
    public class BrokerNameRequest: RequestPacket
    {

        public BrokerNameRequest()
        {
            _type = API.MessageTypes.BROKERNAMEREQUEST;

        }
        public override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append("BrokerName");

            return sb.ToString();
        }

        public override void ContentDeserialize(string reqstr)
        {
            //string[] rec = reqstr.Split(',');
            //BrokerNameRequest request = new BrokerNameRequest();
            //request
        }
    }

    public class BrokerNameResponse : RspResponsePacket
    {
        public BrokerNameResponse()
            :this("demoserver",Providers.Unknown)
        {
        
        }
        public BrokerNameResponse(string name,Providers provider)
        {
            BrokerName = name;
            Provider = provider;
            _type = API.MessageTypes.BROKERNAMERESPONSE;
        }

        public string BrokerName { get; set; }

        public Providers Provider { get; set; }

        
        public override string ResponseSerialize()
        { 
            StringBuilder sb = new StringBuilder();
            char d =',';
            sb.Append(this.BrokerName);
            sb.Append(d);
            sb.Append(((int)Provider).ToString());

            return sb.ToString();
        }

        public override void ResponseDeserialize(string repstr)
        {
            try
            {
                string[] rec = repstr.Split(',');
                if (rec.Length < 2)
                {
                    this.Provider = Providers.Unknown;
                }
                BrokerNameResponse response = new BrokerNameResponse();
                this.BrokerName = rec[0];
                int pcode = int.Parse(rec[1]);
                this.Provider = (Providers)pcode;
            }
            catch (Exception ex)
            {
                this.Provider = Providers.Unknown;
            }
        }
    
    }
}
