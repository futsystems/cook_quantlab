using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface IRuleItem
    {
        /// <summary>
        /// 数据库全局ID
        /// </summary>
        int ID { get; set; }

        /// <summary>
        /// 风控规则名称
        /// </summary>
        string RuleName { get; set; }

        /// <summary>
        /// 风控比较逻辑
        /// </summary>
        QSEnumCompareType Compare { get; set; }

        /// <summary>
        /// 风控参考值
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// 品种集合
        /// </summary>
        string SymbolSet { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        bool Enable { get; set; }

        /// <summary>
        /// 风控规则类别
        /// </summary>
        QSEnumRuleType RuleType { get; set; }
    }
}
