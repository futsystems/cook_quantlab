using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Xml;
using System.IO;
using System.Text;


namespace TradingLib.Common
{
    public class XMLRspInfoTracker
    {
        NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();

        ConcurrentDictionary<string, XMLRspInfo> xmlkeymap = new ConcurrentDictionary<string, XMLRspInfo>();
        ConcurrentDictionary<int, XMLRspInfo> xmlcodemap = new ConcurrentDictionary<int, XMLRspInfo>();

        string _filename = string.Empty;
        string _nodes = string.Empty;
        string _token = string.Empty;
        XMLRspInfo defaulterror = null;

        public XMLRspInfoTracker(string token,string filename,string nodes="errors")
        {
            _token = token;
            _filename = filename;
            _nodes = nodes;
            foreach (XMLRspInfo rsp in LoadXMLRspInfo())
            {
                xmlkeymap[rsp.Key] = rsp;
                xmlcodemap[rsp.Code] = rsp;
            }
            defaulterror = new XMLRspInfo("DEFAULT", 9999, _token +"-未明确错误类型或代码");
        }

        

        /// <summary>
        /// 通过code获得对应的消息对象
        /// 如果消息对象不存在则返回默认的错误消息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public  XMLRspInfo this[int code]
        {
            get
            {
                XMLRspInfo info=null;
                if (xmlcodemap.TryGetValue(code, out info))
                {
                    return info;
                }
                return new XMLRspInfo("UNKNOWN-"+code.ToString(), code, _token + "-未知错误:" + code.ToString());
            }
        }

        /// <summary>
        /// 通过key获得对应的消息对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public  XMLRspInfo this[string key]
        {
            get
            {
                XMLRspInfo info = null;
                if (xmlkeymap.TryGetValue(key, out info))
                {
                    return info;
                }
                return new XMLRspInfo(key, 999, string.Format("{0}-{1}", _token, key));
            }
        }

        public IEnumerable<XMLRspInfo> RspInfos
        {
            get
            {
                return xmlkeymap.Values;
            }
        }


        /// <summary>
        /// 加载xml rspinfo
        /// </summary>
        /// <returns></returns>
        private IEnumerable<XMLRspInfo> LoadXMLRspInfo()
        {
            string xmlfile = Util.GetConfigFile(_filename);
            XmlDocument xmlDoc = null;
            if (File.Exists(xmlfile))
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlfile);
            }

            List<XMLRspInfo> rsplist  = new List<XMLRspInfo>();
            XmlNode xn = xmlDoc.SelectSingleNode(_nodes);
            XmlNodeList errors = xn.ChildNodes;
            //Util.Debug("total errors:" + errors.Count.ToString());
            foreach (XmlNode node in errors)
            {
                try
                {
                    XmlElement error = (XmlElement)node;
                    string key = error.GetAttribute("id");
                    int code = int.Parse(error.GetAttribute("value"));
                    string prompt = error.GetAttribute("prompt");
                    rsplist.Add(new XMLRspInfo(key, code, prompt));
                }
                catch (Exception ex)
                {
                    logger.Error("error:" + ex.ToString());
                }

            }
            return rsplist;
        }
    }
}
