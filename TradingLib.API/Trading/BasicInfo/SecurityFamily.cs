using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    /// <summary>
    /// 证券品种簇
    /// 用于定义某一个证券品种
    /// </summary>
    public interface SecurityFamily
    {
        /// <summary>
        /// 品种代号
        /// </summary>
        string Code { get; set; }

        /// <summary>
        /// 品种名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 货币类别
        /// </summary>
        CurrencyType Currency { get; set; }

        /// <summary>
        /// 品种类别
        /// </summary>
        SecurityType Type { get; set; }

        /// <summary>
        /// 所属交易所
        /// </summary>
        Exchange Exchange { get; set; }


        /// <summary>
        /// 乘数
        /// </summary>
        int Multiple { get; set; }


        /// <summary>
        /// 最小价格变动
        /// </summary>
        decimal PriceTick { get; set; }

        /// <summary>
        /// 是否可交易
        /// </summary>
        bool Tradeable { get; set; }

        /// <summary>
        /// 底层证券
        /// 某个衍生品证券会依赖于底层证券
        /// 比如沪深300股指期货依赖于沪深300，沪深300股指期权依赖于沪深300
        /// 沪深300不可交易，而起衍生品证券可以进行交易
        /// </summary>
        SecurityFamily UnderLaying { get; set; }

        /// <summary>
        /// 开仓手续费
        /// </summary>
        decimal EntryCommission { get; set; }

        /// <summary>
        /// 平仓手续费
        /// </summary>
        decimal ExitCommission { get; set; }

        /// <summary>
        /// 平今手续费
        /// </summary>
        decimal ExitCommissionToday { get; set; }

        /// <summary>
        /// 保证金比例
        /// </summary>
        decimal Margin { get; set; }

        /// <summary>
        /// 额外保证金字段
        /// 用于在基本保证金外提供额外质押
        /// </summary>
        decimal ExtraMargin { get; set; }

        /// <summary>
        /// 过夜保证金,如果需要过夜则需要提供Maintance保证金
        /// </summary>
        decimal MaintanceMargin { get; set; }

        /// <summary>
        /// 交易品种的交易时间段
        /// </summary>
        MarketTime MarketTime { get; set; }

        /// <summary>
        /// 行情源
        /// 用于标注给品种对应的行情源
        /// </summary>
        QSEnumDataFeedTypes DataFeed { get; set; }
    }
}
