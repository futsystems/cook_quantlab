using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    /// <summary>
    /// 委托状态
    /// 1.客户端向服务端发送委托 Placed,表明已经经过风控检查,并且被清算中心记录
    /// 2.服务端将委托转发到对应的成交接口,如果对应的接口接收了委托则成交接口返回gotorder到服务端,则表明委托已经Open等待成交
    /// 委托经过风控检验 则该委托状态变为placed 表明已提交到系统
    /// 交易系统将经过风控的委托 通过BrokerRouter进行提交到对应成交接口,此时如果仍然为placed,同时broker需要有一定的时间来回报该委托, 如果同时有其他委托进入系统,则风控规则就会漏过该委托
    /// 因此当系统提交委托到Broker时,委托状态就发生变化,用于标识该委托需要纳入风控保证金计算,如果Broker拒绝则释放该保证金,如果Broker接受则状态改变继续计算保证金
    /// </summary>
    public enum QSEnumOrderStatus : byte
    {
        /// <summary>
        /// 通过风控检查 被清算中心记录
        /// </summary>
        [Description("提交至清算中心")]Placed=0,

        /// <summary>
        /// 委托已经通过接口正常提交,但是没有获得成交端的任何返回,成交侧可能拒绝也可能接受
        /// 在其他系统中，如果Submited是表明该委托已经处于待成交状态,那么该状态对应于PendingSubmited
        /// </summary>
        [Description("提交至Broker")]Submited=1,

        /// <summary>
        /// 表明委托已经被成交侧接受,处于待成交状态
        /// </summary>
        [Description("等待成交")]Opened=2,

        /// <summary>
        /// 委托被全部成交
        /// </summary>
        [Description("成交")]Filled=3,

        /// <summary>
        /// 委托被部分成交
        /// </summary>
        [Description("部分成交")]PartFilled=4,

        /// <summary>
        /// 委托被取消
        /// </summary>
        [Description("取消")]Canceled=5,

        /// <summary>
        /// 委托被拒绝
        /// </summary>
        [Description("拒绝")]Reject=6,

        /// <summary>
        /// 如果是系统模拟的委托,则返回预提交状态，当模拟的委托条件触发,提交到成交侧时,状态更新为Submited
        /// </summary>
        [Description("预提交状态")]PreSubmited = 10,

        /// <summary>
        /// 状态未知
        /// </summary>
        [Description("未知")]Unknown=9,
    }
}
