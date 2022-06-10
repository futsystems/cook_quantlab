using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    /// <summary>
    /// 扩展调用参数解析方式
    /// </summary>
    public enum QSEnumArgParseType
    {
        /// <summary>
        /// 将扩展请求部分所有参数以Json字符串的形式提供,被调用函数自行解析该Json字符串
        /// 可以传递对象,通过Json反序列化可以生成对应的对象 如预先有相关对象侧可以使用该方式来进行参数传递
        /// </summary>
        Json,
        /// <summary>
        /// TNetString 用标签字符串传递参数,通过TnetString来进行参数打包
        /// 参数可以带有任何字符串,如果提供的参数本身没有对象进行描述,且含有,等特殊符号则
        /// </summary>
        //TNetString,

        /// <summary>
        /// 逗号分隔的方式传递参数
        /// 调用参数较少，简单拼接就可以进行参数传递
        /// </summary>
        CommaSeparated

    }
}
