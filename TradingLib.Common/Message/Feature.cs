///////////////////////////////////////////////////////////////////////////////////////
// 用于客户端向服务端查询服务端标识,如果服务无需则返回unknown provider
// 功能操作码请求,用于获取当前客户端所允许执行的操作码,用于管理端的授权以及不同服务节点的不同功能部署
//
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    public class FeatureRequest:RequestPacket
    {
        public FeatureRequest()
        {
            _type = API.MessageTypes.FEATUREREQUEST;
        }

        public override string ContentSerialize()
        {
            return "FeatureRequest";
        }


    }

    public class FeatureResponse : RspResponsePacket
    {
        public FeatureResponse()
        {
            _type = API.MessageTypes.FEATURERESPONSE;
        }

        List<MessageTypes> _featurelist = new List<MessageTypes>();

        public MessageTypes[] Features { get { return _featurelist.ToArray(); } }
        public void Add(MessageTypes feature)
        {
            if (!_featurelist.Contains(feature))
                _featurelist.Add(feature);
        }

        public void Add(MessageTypes[] features)
        {
            foreach (MessageTypes type in features)
            {
                if (_featurelist.Contains(type))
                    continue;
                this.Add(type);
            }
        }

        public override string ResponseSerialize()
        {
            List<string> mf = new List<string>();
            foreach (MessageTypes t in _featurelist)
            {
                int ti = (int)t;
                mf.Add(ti.ToString());
            }
            return string.Join(",", mf.ToArray());
        }

        public override void ResponseDeserialize(string reqstr)
        {
            string[] p = reqstr.Split(',');
            foreach (string s in p)
            {
                try
                {
                    MessageTypes mt = (MessageTypes)Convert.ToInt32(s);
                    _featurelist.Add(mt);
                }
                catch (Exception) { }
            }
        }
    }


}
