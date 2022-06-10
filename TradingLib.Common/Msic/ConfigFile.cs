using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TradingLib.Common
{
    public class CfgValue
    {
        public string Value { get { return _value; } set { _value = value; } }
        string _value = "";
        public CfgValue(string strValue)
        {
            _value = strValue;
        }
        public string AsString()
        {
            return _value.ToString();
        }
        public int AsInt()
        { 
            int re=0;
            int.TryParse(_value, out re);
            return re;
        }
        public decimal AsDecimal()
        {
            decimal re = 0M;
            decimal.TryParse(_value, out re);
            return re;
        }
        public bool AsBool()
        {
            bool re = false;
            bool.TryParse(_value, out re);
            return re;
        }

        public string AsHelp()
        {
            return _value;
        }

    }
    /// <summary>
    /// 配置文件对象
    /// 用于从文本加载配置,或保存配置文件
    /// 
    /// </summary>
    public class ConfigFile
    {
        public static ConfigFile GetConfigFile(string filename = "srv.cfg")
        {
			return new ConfigFile(Util.GetConfigFile (filename));
        }

        public Dictionary<string, CfgValue> configData;
        string fullFileName;

        public ConfigFile(string _fileName)
        {
            configData = new Dictionary<string, CfgValue>();
            fullFileName =_fileName;

			//bool hasPath = Directory.Exists(_path);
			//if (!hasPath)
			//{
			//    Directory.CreateDirectory(_path);
			//}

            bool hasCfgFile = File.Exists(_fileName);
            if (hasCfgFile == false)
            {
                StreamWriter writer = new StreamWriter(File.Create(_fileName), Encoding.UTF8);
                writer.Close();
            }
            StreamReader reader = new StreamReader(_fileName, Encoding.UTF8);
            string line;

            int indx = 0;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith(";") || string.IsNullOrEmpty(line))
                    configData.Add(";" + indx++, new CfgValue(line));
                else
                {
                    string[] key_value = line.Split('=');
                    if (key_value.Length >= 2)
                        configData.Add(key_value[0], new CfgValue(key_value[1]));
                    else
                        configData.Add(";" + indx++, new CfgValue(line));
                }
            }
            reader.Close();
        }

        /// <summary>
        /// 通过下标来进行读取或者保存配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public CfgValue this[string key]
        {
            get
            {
                return this.Get(key);
            }

            set
            {
                this.Set(key, value.Value);
            }
        }

        /// <summary>
        /// 是否包含参数相
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return configData.ContainsKey(key);
        }

        public CfgValue Get(string key)
        {
            if (configData.Count <= 0)
                return new CfgValue("");
            else if (configData.ContainsKey(key))
                return configData[key];
            else
                return new CfgValue("");
        }

        public void Set(string key, string value)
        {
            if (configData.ContainsKey(key))
                configData[key].Value = value;
            else
                configData.Add(key, new CfgValue(value));
        }

        public void Save()
        {
            StreamWriter writer = new StreamWriter(fullFileName, false, Encoding.UTF8);
            foreach (string key in configData.Keys)
            {
                if (key.StartsWith(";"))
                {
                    writer.WriteLine(configData[key].Value);
                }
                else
                {
                    writer.WriteLine(key + "=" + configData[key].Value);
                }
            }
            writer.Close();
        }

        public Dictionary<string, CfgValue> Dict
        { 
            get{
                return configData;
            }
        }
    }
}
