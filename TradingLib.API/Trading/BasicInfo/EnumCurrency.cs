using System.ComponentModel;

namespace TradingLib.API
{
    public enum CurrencyType
    {
        [Description("人民币")]
        RMB,
        [Description("美元")]
        USD,
        [Description("港币")]
        HKD,
        [Description("欧元")]
        EUR,
    }
}
