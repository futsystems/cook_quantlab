using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public abstract class RuleBase
    {
        /// <summary>
        /// 对应交易帐号
        /// </summary>
        public IAccount Account { get; set; }

        /// <summary>
        /// 数据库全局ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 逻辑关系
        /// </summary>
        public QSEnumCompareType Compare { get; set; }

        List<string> _symbolset=new List<string>();
        /// <summary>
        /// 检查品种列表
        /// </summary>
        public string SymbolSet
        {
            get
            {
                if (_symbolset == null)
                    return string.Empty;
                else
                    return string.Join("_", _symbolset);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    foreach (string s in value.Split('_'))
                    {
                        _symbolset.Add(s);
                    }
                }
            }
        }

        public virtual string Value { get; set; }


        /// <summary>
        /// 是否需要检查该合约
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        protected bool NeedCheckSymbol(Symbol symbol)
        {
            //如果有合约过滤集,并且过滤集不包含当前品种,则不用进行风控项检查
            if (_symbolset != null && !_symbolset.Contains(symbol.SecurityFamily.Code))
                return false;//如果当前Order的symbol不在我们检查行列，我们直接返回Ture
            return true;
        }

        /// <summary>
        /// 检查合约是否在品种集合中
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        protected bool IsInSymbolSet(Symbol symbol)
        {
            if (_symbolset != null && _symbolset.Contains(symbol.SecurityFamily.Code))
                return true;
            return false;
        }

        /// <summary>
        /// 初始化风控项
        /// </summary>
        /// <param name="item"></param>
        public void FromRuleItem(IRuleItem item)
        {
            ID = item.ID;
            Enable = item.Enable;
            if (CanSetCompare)
            {
                Compare = item.Compare;
            }
            if (CanSetValue)
            {
                Value = item.Value;
            }
            if (CanSetSymbols)
            {
                SymbolSet = item.SymbolSet;
            }
        }


        /// <summary>
        /// 该规则内容
        /// </summary>
        public virtual string RuleDescription
        {
            get
            {
                return "规则描述";
            }
        }

        #region 规则静态类属性,用于获得规则的名称,描述,以及参数的可设置性 从而可以在设置窗口进行展示
        /// <summary>
        /// 规则名称
        /// </summary>
        public static string Title
        {
            get { return "风控规则"; }
        }

        /// <summary>
        /// 规则描述
        /// </summary>
        public static string Description
        {
            get { return "规则描述"; }
        }




        /// <summary>
        /// 是否允许设置参数值
        /// </summary>
        public static bool CanSetValue { get { return true; } }

        /// <summary>
        /// 默认值
        /// </summary>
        public static string DefaultValue { get { return ""; } }

        /// <summary>
        /// 是否需要设置参数关系
        /// </summary>
        public static bool CanSetCompare { get { return true; } }

        /// <summary>
        /// 默认比较关系
        /// </summary>
        public static QSEnumCompareType DefaultCompare { get { return QSEnumCompareType.GreaterEqual; } }

        /// <summary>
        /// 是否需要设置合约列表
        /// </summary>
        public static bool CanSetSymbols { get { return true; } }


        /// <summary>
        /// 参数名称
        /// </summary>
        public static string ValueName { get { return "检查变量"; } }

        /// <summary>
        /// 验证UI输入item是否有效
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool ValidSetting(RuleItem item, out string msg)
        {
            msg = "";
            return true;
        }
        #endregion

    }
}
