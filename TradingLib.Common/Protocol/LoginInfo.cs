using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

/* Protocol
 * 用于存放服务端与管理端通讯时使用到的对象定义
 * 
 * 目前服务端与管理端通讯协议
 * 1.固定消息类型 原本预定义消息类型，每个消息有对应的request,response
 * 2.扩展消息类型 在ContribMessage基础上，请求与返回通过Args,Result进行打包发送，返回消息内容时通过Json进行转换
 * 
 * 在服务端与管理端均需要使用的对象定义中
 * 1.业务需要的对象 在具体的业务逻辑中定义，通讯过程中使用相关对象比如RouterGroupSetting 等
 * 2.业务不需要的对象 需要单独进行定义方便服务端与管理端的序列化与反序列化，比如查询分区管理员登入信息等
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * **/

namespace TradingLib.Protocol
{
    public class LoginInfo
    {
        /// <summary>
        /// 登入ID
        /// </summary>
        public string LoginID { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pass { get; set; }
    }
}
