using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 储存与数据库中的一条风控规则
    /// 包含了帐户,风控类别,规则类名,比较方法，参考值，应用品种范围，是否启用等参数
    /// </summary>
    public class RuleItem :IRuleItem
    {

        public RuleItem()
        {
            this.ID = 0;
            this.Account = string.Empty;
            this.RuleName = string.Empty;
            this.Compare = QSEnumCompareType.Equals;
            this.Value = string.Empty;
            this.SymbolSet = string.Empty;
            this.Enable = false;
            this.RuleType = QSEnumRuleType.OrderRule;
            this.RuleDescription = string.Empty;
        }
        /// <summary>
        /// 数据库全局编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 交易帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 风控规则名称
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// 风控比较逻辑
        /// </summary>
        public QSEnumCompareType Compare { get; set; }

        /// <summary>
        /// 风控参考值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 品种集合
        /// </summary>
        public string SymbolSet { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }


        /// <summary>
        /// 风控规则类别
        /// </summary>
        public QSEnumRuleType RuleType { get; set; }

        /// <summary>
        /// 规则描述
        /// </summary>
        public string RuleDescription { get; set; }

        #region 序列化与反序列化
        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.ID.ToString());
            sb.Append(d);
            sb.Append(this.Account);
            sb.Append(d);
            sb.Append(this.RuleName);
            sb.Append(d);
            sb.Append(this.Compare.ToString());
            sb.Append(d);
            sb.Append(this.Value);
            sb.Append(d);
            sb.Append(this.SymbolSet);
            sb.Append(d);
            sb.Append(this.Enable.ToString());
            sb.Append(d);
            sb.Append(this.RuleType.ToString());
            sb.Append(d);
            sb.Append(this.RuleDescription.Replace(',', ' ').Replace('|', ' ').Replace('^', ' '));//将协议保留的分隔符替换为' '

            return sb.ToString();
        }

        public void Deserialize(string content)
        {
            string[] rec = content.Split(',');
            this.ID = int.Parse(rec[0]);
            this.Account = rec[1];
            this.RuleName = rec[2];
            this.Compare = (QSEnumCompareType)Enum.Parse(typeof(QSEnumCompareType), rec[3]);
            this.Value = rec[4];
            this.SymbolSet = rec[5];
            this.Enable = bool.Parse(rec[6]);
            this.RuleType = (QSEnumRuleType)Enum.Parse(typeof(QSEnumRuleType), rec[7]);
            this.RuleDescription = rec[8];
        }
        #endregion

        /// <summary>
        /// 从一条规则IRule生成对应的RuleItem
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static RuleItem IRule2RuleItem(IRule rule)
        {
            RuleItem item = new RuleItem();
            item.Account = rule.Account.ID;
            item.Compare = rule.Compare;
            item.Enable = rule.Enable;
            item.ID = rule.ID;
            item.RuleName = rule.GetType().FullName;

            //判断规则类型 委托检查或帐户检查
            Type t = rule.GetType();
            if(typeof(IOrderCheck).IsAssignableFrom(t))
            {
                item.RuleType = QSEnumRuleType.OrderRule;
            }
            else if (typeof(IAccountCheck).IsAssignableFrom(t))
            {
                item.RuleType = QSEnumRuleType.AccountRule;
            }

            item.SymbolSet = rule.SymbolSet;
            item.Value = rule.Value;
            item.RuleDescription = rule.RuleDescription;//规则描述

            return item;
        }
    }
}
