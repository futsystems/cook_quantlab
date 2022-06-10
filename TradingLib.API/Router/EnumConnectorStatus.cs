using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    public enum QSEnumConnectorStatus
    {
        [Description("运行")]
        Start,//启动
        [Description("停止")]
        Stop,//停止
        [Description("接口异常")]
        InterfaceError,//接口异常
        [Description("加载异常")]
        LoadError,//加载异常
    }
}
