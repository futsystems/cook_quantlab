using System.ComponentModel;
namespace TradingLib.API
{
    /// <summary>
    /// Stock, Option, Future, Currency Forward, Forward, FOP, Warrant, ForEx, Index, Bond
    /// </summary>
    public enum SecurityType //: byte
    {

        NIL,
        [Description("股票")] 
        STK,//股票
        [Description("期货")]
        FUT,//期货
        [Description("期权")] 
        OPT,//期权
        //CFD,
        //FOR,
        FOP,
        //WAR,
        FOX,
        [Description("指数")]
        IDX,
        //BND,
        [Description("货币")]
        CASH,
        //BAG,
        //[Description("异化证券")]
        //INNOV,
    }
}