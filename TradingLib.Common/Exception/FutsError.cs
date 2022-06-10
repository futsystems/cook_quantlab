using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    /// <summary>
    /// 回报异常 当操作产生异常时候 通过将异常封装到FutsRspError来向外层抛出报错信息
    /// FutsRspError 主要提供ErrorID和ErrorMessage
    /// </summary>
    public class FutsRspError : TLException
    {
        static string UNKNOWERROR = "未知错误";
        public int ErrorID { get; set; }
        public string ErrorMessage { get; set; }

        public FutsRspError()
            : base(UNKNOWERROR)
        {
            ErrorID = 1;
            ErrorMessage = UNKNOWERROR;
        }
        /// <summary>
        /// 从一个异常创建一个错误信息
        /// </summary>
        /// <param name="e"></param>
        public FutsRspError(Exception e)
            :base(e.Message,e)
        {
            ErrorID = 1;
            ErrorMessage = e.Message;
        }

        /// <summary>
        /// 从一个错误信息创建一个FutsRspError
        /// </summary>
        /// <param name="error"></param>
        public FutsRspError(string error)
            :base(error)
        {
            ErrorID = 1;
            ErrorMessage = error;
        }

        

        /// <summary>
        /// 用自定义的XMLRspInfo填充错误信息
        /// </summary>
        /// <param name="error"></param>
        public void FillError(XMLRspInfo error)
        {
            ErrorID = error.Code;
            ErrorMessage = error.Message;
        }

        

    }
}
