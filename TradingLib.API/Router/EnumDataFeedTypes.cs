using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    //DataFeed类型需要与TradeLibFast进行同步(与messagetypes一样进行同步)
    /// <summary>
    /// FastTickSrv中请求启动某个数据通道或者向某个数据通道请求某组合约需要指定对应的通道类型/名称
    /// 与本地exchagne_index -> DataFeed 通过交易所编然后通过 合约的交易所来获得对应的数据通道接口进行操作不同
    /// 在定义SecurityFamily时我们可以预先制定该合约所对应的数据通道/或者按照一定的规则来获得对应的数据通道
    /// </summary>
    public enum QSEnumDataFeedTypes
    {
        DEFAULT=0,//默认
        CTP=1,//国内CTP期货 DataFeed
        CTPOPT=2,//国内CTP期权 DataFeed
        IB=3,//外盘IBDataFeed
        IQFEED=4,//外盘IQFeed行情源
        SHZD=5,//上海直达接口 获取恒生行情
        ESUNNY=6,//易盛
        OKCOIN=7,//OKCOIN
        TONGSHI=8,//股票通视规范接口
        WSDATA=9,//威盛数据
    };


    public enum QSEnumConnectorType
    { 
        /// <summary>
        /// 行情通道
        /// </summary>
        [Description("行情")]
        DataFeed,
        /// <summary>
        /// 成交通道
        /// </summary>
        [Description("交易")]
        Broker,
    }
}
