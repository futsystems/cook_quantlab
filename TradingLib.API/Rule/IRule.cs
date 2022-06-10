using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TradingLib.API
{
    public enum QSEnumRuleType
    { 
        /// <summary>
        /// 委托风控规则
        /// </summary>
        OrderRule,
        /// <summary>
        /// 帐户风控规则
        /// </summary>
        AccountRule,
    }

    public interface IRule
    {
        /// <summary>
        /// 全局ID
        /// </summary>
        int ID { get; set; }

        //该规则附着的账户
        IAccount Account { get; set; }

        /// <summary>
        /// 标记规则是否有效
        /// </summary>
        bool Enable { get; set; }

        /// <summary>
        /// 变量参考值
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// 逻辑关系 大于,小于,等于
        /// </summary>
        QSEnumCompareType Compare { get; set; }

        /// <summary>
        /// 该规则所应用品种集合字符串 以','分开
        /// </summary>
        string SymbolSet { get; set; }
        
        /// <summary>
        /// 生成规则的描述,人可以阅读并理解
        /// </summary>
        string RuleDescription { get; }

        /// <summary>
        /// 从RuleItem进行初始化设置
        /// </summary>
        /// <param name="item"></param>
        void FromRuleItem(IRuleItem item);



    }
}
