using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class CommissionTemplateSetting
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 域ID
        /// </summary>
        public int Domain_ID { get; set; }

        /// <summary>
        /// 管理员主域ID
        /// </summary>
        public int Manager_ID { get; set; }


        /// <summary>
        /// 股票交易手续费率
        /// </summary>
        public decimal STKCommissioinRate { get; set; }


        /// <summary>
        /// 过户费率
        /// </summary>
        public decimal STKTransferFee { get; set; }


        /// <summary>
        /// 印花税率
        /// </summary>
        public decimal STKStampTaxRate { get; set; }



        public override string ToString()
        {
            return this.Name;
        }
    }


    /// <summary>
    /// 手续费模板
    /// </summary>
    public class CommissionTemplate : CommissionTemplateSetting
    {
        [Newtonsoft.Json.JsonIgnore]
        public IEnumerable<CommissionTemplateItem> CommissionItems { get { return _itemamp.Values; } }

        Dictionary<string, CommissionTemplateItem> _itemamp = new Dictionary<string, CommissionTemplateItem>();

        
        /// <summary>
        /// 添加手续费项目
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(CommissionTemplateItem item)
        {
            if (_itemamp.Keys.Contains(item.CommissionItemKey))
            {
                _itemamp[item.CommissionItemKey] = item;
            }
            else
            {
                _itemamp.Add(item.CommissionItemKey, item);
            }
        }

        public CommissionTemplateItem this[string key]
        {
            get
            {
                CommissionTemplateItem item = null;
                if (_itemamp.TryGetValue(key, out item))
                {
                    return item;
                }
                return null;
            }
        }
        /// <summary>
        /// 获得某个合约对应的手续费项
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public CommissionTemplateItem GetCommissionItem(Symbol symbol)
        { 
            //品种 月份 
            return this[symbol.GetCommissionItemKey()];
        }

        /// <summary>
        /// 判断是否存在某个品种的模板项目
        /// </summary>
        /// <param name="code"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public bool HaveTemplateItem(string code, int month = 0)
        {
            if (month == 0)
            {
                return _itemamp.Keys.Contains(string.Format("{0}-1", code));
            }
            else
            {
                return _itemamp.Keys.Contains(string.Format("{0}-{1}", code, month));
            }
        }

        

    }
}
