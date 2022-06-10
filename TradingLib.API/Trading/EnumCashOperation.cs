using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    public enum QSEnumCashInOutStatus
    {
        [Description("待处理")]
        PENDING,
        [Description("已确认")]
        CONFIRMED,
        [Description("已拒绝")]
        REFUSED,
        [Description("已取消")]
        CANCELED,
    }

    public enum QSEnumCashOperation//:byte
    {
        [Description("充值")]
        Deposit,

        [Description("提现")]
        WithDraw,
    }
}
