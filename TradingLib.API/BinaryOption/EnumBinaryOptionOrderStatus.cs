using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace TradingLib.API
{
    public enum EnumBOOrderStatus
    {
        [Description("提交")]
        Placed = 0,

        /// <summary>
        /// 委托已经通过接口正常提交,但是没有获得成交端的任何返回,成交侧可能拒绝也可能接受
        /// 在其他系统中，如果Submited是表明该委托已经处于待成交状态,那么该状态对应于PendingSubmited
        /// </summary>
        [Description("提交至上手")]
        Submited = 1,

        /// <summary>
        /// 表明委托已经被成交侧接受,处于待成交状态
        /// </summary>
        [Description("持权")]
        Entry = 2,

        /// <summary>
        /// 委托被全部成交
        /// </summary>
        [Description("平权")]
        Exit = 3,


        /// <summary>
        /// 委托被拒绝
        /// </summary>
        [Description("拒绝")]
        Reject = 4,

        /// <summary>
        /// 状态未知
        /// </summary>
        [Description("未知")]
        Unknown = 9,
    }
}
