using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    /// <summary>
    /// 合约接口
    /// 演示更新
    /// </summary>
    public interface Symbol
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        string Symbol { get; set; }

        /// <summary>
        /// 底层合约用于异化合约的生成
        /// </summary>
        Symbol ULSymbol { get; set; }

        /// <summary>
        /// 合约隶属品种
        /// </summary>
        SecurityFamily SecurityFamily { get; set; }

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
        /// 保证金比例/保证金数额
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
        /// 乘数
        /// </summary>
        int Multiple { get; }

       
        /// <summary>
        /// 全称
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// 品种类别
        /// </summary>
        SecurityType SecurityType { get; }

        /// <summary>
        /// 货币类别
        /// </summary>
        CurrencyType Currency { get; }
       
        /// <summary>
        /// 交易所
        /// </summary>
        string Exchange { get; }

        /// <summary>
        /// 是否可交易
        /// </summary>
        bool Tradeable { get; }

        /// <summary>
        /// 综合判断获得是否可交易标识
        /// </summary>
        bool IsTradeable{get;}


        /// <summary>
        /// 给出交易所日期，判定合约是否过期 通过比较过期日与交易所当前日期
        /// </summary>
        /// <param name="exday"></param>
        /// <returns></returns>
        bool IsExpired(int exday);


        /// <summary>
        /// 合约到期日
        /// </summary>
        int ExpireDate { get; set; }

        /// <summary>
        /// 期权 方向
        /// </summary>
        QSEnumOptionSide OptionSide { get; set; }

        /// <summary>
        /// 期权中的行权价
        /// </summary>
        decimal Strike { get; set; }

        /// <summary>
        /// 底层依赖合约 比如股指期权依赖于股指期货
        /// </summary>
        Symbol UnderlayingSymbol { get; set; }

        /// <summary>
        /// 合约月份
        /// </summary>
        string Month { get; set; }

        /// <summary>
        /// 合约类别
        /// 标准合约，月连续合约，主力连续合约,连1,连2等
        /// </summary>
        QSEnumSymbolType SymbolType { get; set; }


        /// <summary>
        /// 交易所小节
        /// </summary>
        string TradingSession { get; set; }


        /// <summary>
        /// 名称 股票对应的 证券名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 通过组合 交易所-合约 形成唯一键值
        /// 股票中 上海和深圳两个交易所的代码有可能会发生重复
        /// </summary>
        string UniqueKey { get;}
    }
}
