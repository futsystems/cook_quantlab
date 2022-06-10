///////////////////////////////////////////////////////////////////////////////////////
// 服务端消息返回
// 用于返回服务端的相关消息 比如连接失败，提交委托不合法，资金不足等相关通知
// 这里可以考虑建立一个XML错误代码表文件，将错误代码放到对应的文件中，有程序运行时统一加载
// 这样容易实现两端错误代码的统一
//
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{

    /// <summary>
    /// 委托信息
    /// </summary>
    /// <param name="o"></param>
    /// <param name="info"></param>
    public delegate void ErrorOrderRspInoDel(Order o,string error);


    
    /// <summary>
    /// 回报消息
    /// 用于向客户端回报错误提示
    /// 正常查询内也会附带对应的回报消息,逻辑数据包会自行进行解析并形成对应的逻辑包
    /// </summary>
    public class RspInfoImpl:RspInfo
    {
        public RspInfoImpl()
        {
            ErrorID = 0;
            ErrorMessage = string.Empty;
        }

        public int ErrorID { get; set; }

        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return string.Format("ErrID[{0}] ErrMsg[{1}]", this.ErrorID, this.ErrorMessage);
        }
        public static string Serialize(RspInfo info)
        {
            return info.ErrorID.ToString() + "," + info.ErrorMessage.Replace(',', ' ');
        }

        public static RspInfo Deserialize(string str)
        {
            RspInfo info = new RspInfoImpl();
            string[] rec = str.Split(',');
            int errorid = 0;
            if (rec.Length < 2)
            {
                int.TryParse(rec[0], out errorid);
                info.ErrorID = errorid;
                return info;
            }
            int.TryParse(rec[0], out errorid);
            info.ErrorID = errorid;
            info.ErrorMessage = rec[1];
            return info;
        }
       
    }
}
