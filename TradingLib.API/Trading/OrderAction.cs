using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{

    /// <summary>
    /// 委托操作接口
    /// 删除或者修改委托
    /// </summary>
    public interface OrderAction
    {
        /// <summary>
        /// 委托操作标识
        /// </summary>
        QSEnumOrderActionFlag ActionFlag { get; set; }

        /// <summary>
        /// 交易帐号
        /// </summary>
        string Account { get; set; }

        /// <summary>
        /// 服务端委托唯一编号
        /// </summary>
        long OrderID { get; set; }

        /// <summary>
        /// 前置编号
        /// </summary>
        int FrontID { get; set; }
        /// <summary>
        /// 会话编号,每次建立的会话都有一个唯一的SessionID分配给客户端,同时客户端还绑有UUID用于通讯寻址
        /// 通过组合SessionID和OrderRef就可以定位某个委托
        /// 或者通过ExchOrderID来进行唯一定位
        /// </summary>
        int SessionID { get; set; }

        /// <summary>
        /// 客户端委托引用
        /// </summary>
        string OrderRef { get; set; }

        /// <summary>
        /// 交易所编号
        /// </summary>
        string Exchagne { get; set; }

        /// <summary>
        /// 交易所委托编号类似于CTP的OrderSysID
        /// </summary>
        string OrderExchID { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        string Symbol { get; set; }

        /// <summary>
        /// 请求 ID
        /// </summary>
        int RequestID { get; set; }
    }
}
