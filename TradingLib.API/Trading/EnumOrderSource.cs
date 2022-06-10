using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace TradingLib.API
{
    /// <summary>
    /// 委托来源
    /// </summary>
    public enum QSEnumOrderSource
    {
        [Description("未标注")]//由交易客户端产生的原始委托,客户端生成具体的委托数据
        UNKNOWN,
        [Description("交易客户端")]//由交易客户端产生的原始委托,客户端生成具体的委托数据
        CLIENT,
        [Description("交易客户端-快捷指令")]//由客户端产生的快捷指令,系统接受快捷指令后按照服务端的数据生成的委托
        CLIENTQUICK,
        [Description("风控中心")]//由风控中心执行检查后对客户持仓强平产生的委托 具体委托可能交由清算中心生成
        RISKCENTRE,
        [Description("风控中心-帐户规则")]//由风控中心执行检查后对客户持仓强平产生的委托 具体委托可能交由清算中心生成
        RISKCENTREACCOUNTRULE,
        [Description("清算中心尾盘强平")]//由管理员通过QSMoniter对客户持仓进行的操作
        CLEARCENTRE,
        [Description("管理端")]//由管理员通过QSMoniter对客户持仓进行的操作
        QSMONITER,
        [Description("服务端止损止盈")]//由管理员通过QSMoniter对客户持仓进行的操作
        SRVPOSITIONOFFSET,

    }
}
