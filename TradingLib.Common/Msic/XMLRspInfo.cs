using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    /// <summary>
    /// 储存于xml文件中的Rsp消息
    /// 包含Key,Code,Message 用于实现与其他业务系统交互
    /// 主要用于错误或异常提示
    /// </summary>
    public class XMLRspInfo
    {
        public XMLRspInfo(string key, int code, string message)
        {
            Key = key;
            Code = code;
            Message = message;
        }
        /// <summary>
        /// xml消息的key用于建立key索引
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// xml消息的code 用于消息处理的地方进行逻辑判断
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// xml消息的具体内容
        /// </summary>
        public string Message { get; set; }
    }
}
