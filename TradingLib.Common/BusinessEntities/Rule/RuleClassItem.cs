using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 规则集条目 用于从反射加载的type中获得相关信息 比如名称，描述，参数的可设置性等
    /// 通过序列化后可以在管理形成对应的风控规则条目,并启动对应的设置界面进行设置
    /// 具体的设置通过RuleItem进行传递,然后调用对应的验证过程
    /// </summary>
    public class RuleClassItem
    {
        /// <summary>
        /// 类名全称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 检查变量名称
        /// </summary>
        public string ValueName { get; set; }

        /// <summary>
        /// 规则类别 委托风控规则/帐户风控规则
        /// </summary>
        public QSEnumRuleType Type { get; set; }

        /// <summary>
        /// 风控规则名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否允许设置参数值
        /// </summary>
        public bool CanSetValue { get; set; }

        /// <summary>
        /// 是否需要设置参数关系
        /// </summary>
        public bool CanSetCompare { get; set; }

        /// <summary>
        /// 是否需要设置合约列表
        /// </summary>
        public bool CanSetSymbols { get; set; }

       

        /// <summary>
        /// 默认关系
        /// </summary>
        
        public QSEnumCompareType DefaultCompare { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }


        /// <summary>
        /// 风控规则type
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        //[NoJsonExportAttr()]
        public Type RuleClassType { get; set; }


        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.ClassName);
            sb.Append(d);
            sb.Append(this.Title.Replace(',',' '));
            sb.Append(d);
            sb.Append(this.Description.Replace(',', ' '));
            sb.Append(d);
            sb.Append(this.CanSetValue.ToString());
            sb.Append(d);
            sb.Append(this.CanSetCompare.ToString());
            sb.Append(d);
            sb.Append(this.CanSetSymbols.ToString());
            sb.Append(d);
            sb.Append(this.ValueName);
            sb.Append(d);
            sb.Append(this.Type.ToString());
            sb.Append(d);
            sb.Append(this.DefaultCompare.ToString());
            sb.Append(d);
            sb.Append(this.DefaultValue);

            return sb.ToString();
        }

        public void Deserialize(string content)
        {
            string[] rec = content.Split(',');
            this.ClassName = rec[0];
            this.Title = rec[1];
            this.Description = rec[2];
            this.CanSetValue=bool.Parse(rec[3]);
            this.CanSetCompare = bool.Parse(rec[4]);
            this.CanSetSymbols = bool.Parse(rec[5]);
            this.ValueName = rec[6];
            this.Type = (QSEnumRuleType)Enum.Parse(typeof(QSEnumRuleType), rec[7]);
            this.DefaultCompare = (QSEnumCompareType)Enum.Parse(typeof(QSEnumCompareType), rec[8]);
            this.DefaultValue = rec[9];
        }

        /// <summary>
        /// 生成风控规则实例对象
        /// 传入一条RuleItem记录 生成对应的IRule规则对象
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IRule GenerateRuleInstance(RuleItem item)
        {
            //通过类型RuleClassType生成对象
            IRule t = (IRule)Activator.CreateInstance(this.RuleClassType);
            //然后将参数item设置到IRule
            t.FromRuleItem(item);
            return t;
        }

        /// <summary>
        /// 验证设定
        /// </summary>
        /// <param name="item"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool ValidSetting(RuleItem item, out string msg)
        {
            //RuleBase rb = (RuleBase)Activator.CreateInstance(this.RuleClassType);

            //object [] list = new object[]{item,msg};
            //bool re = (bool)this.RuleClassType.InvokeMember("ValidSetting",BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static |BindingFlags.InvokeMethod, null, null, list)

            msg = "";
            return true;
        }



        /// <summary>
        /// 由反射的type生成ruleclasstype 
        /// 将相关属性进行封装方便生成风控规则实例
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static RuleClassItem Type2RuleClassItem(Type t)
        {
            RuleClassItem item = new RuleClassItem();
            //判断规则类型
            if (typeof(IOrderCheck).IsAssignableFrom(t))
            {
                item.Type = QSEnumRuleType.OrderRule;
            }
            else if (typeof(IAccountCheck).IsAssignableFrom(t))
            {
                item.Type = QSEnumRuleType.AccountRule;
            }
            else
            {
                throw new Exception("风控规则加载出错");
            }

            item.ClassName = t.FullName;

            item.Title = (string)t.InvokeMember("Title",BindingFlags.FlattenHierarchy |BindingFlags.Public| BindingFlags.Static | BindingFlags.GetProperty, null, null, null);
            item.Description = (string)t.InvokeMember("Description", BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty, null, null, null);
            item.CanSetValue = (bool)t.InvokeMember("CanSetValue", BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty, null, null, null);
            item.CanSetCompare = (bool)t.InvokeMember("CanSetCompare", BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty, null, null, null);
            item.CanSetSymbols = (bool)t.InvokeMember("CanSetSymbols", BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty, null, null, null);
            item.ValueName = (string)t.InvokeMember("ValueName", BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty, null, null, null);
            item.DefaultValue = (string)t.InvokeMember("DefaultValue", BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty, null, null, null);
            item.DefaultCompare = (QSEnumCompareType)t.InvokeMember("DefaultCompare", BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty, null, null, null);
            
            
            item.RuleClassType = t;
            return item;
        }
    }
}
