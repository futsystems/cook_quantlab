using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;


namespace TradingLib.Protocol
{
    /* 在管理端进行相关设置,需要将对应的设置序列化后发送到管理端
     * 管理端反序列化后生成对应的对象进行操作。
     * 这里会存在一些不一致性，比如更新单个手续费模板项目时，可以通用
     * CommissionTemplateItemSetting，但是如果是设置到所有月份，则该设置无法使用
     * CommissionTemplateItemSetting进行传递，因此需要生成一个对象用于传递设置信息
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * **/
    /// <summary>
    /// 设置手续费模板项目
    /// </summary>
    public class MGRCommissionTemplateItemSetting: CommissionTemplateItemSetting
    {
        public MGRCommissionTemplateItemSetting()
            :base()
        { 
        
        }

        public MGRCommissionTemplateItemSetting(CommissionTemplateItemSetting item)
        {
            this.ChargeType = item.ChargeType;
            this.CloseByMoney = item.CloseByMoney;
            this.CloseByVolume = item.CloseByVolume;
            this.CloseTodayByMoney = item.CloseTodayByMoney;
            this.CloseTodayByVolume = item.CloseTodayByVolume;
            this.Code = item.Code;
            this.ID = item.ID;
            this.Month = item.Month;
            this.OpenByMoney = item.OpenByMoney;
            this.OpenByVolume = item.OpenByVolume;
            this.Percent = item.Percent;
            this.SetAllMonth = false;
            this.SetAllCodeMonth = false;
            this.Template_ID = item.Template_ID;
            this.SecurityType = item.SecurityType;
            
        }
        /// <summary>
        /// 是否设置到该品种所有月份
        /// </summary>
        public bool SetAllMonth { get; set; }

        /// <summary>
        /// 是否设置到所有品种所有月份
        /// </summary>
        public bool SetAllCodeMonth { get; set; }
    }

    public class MGRMarginTemplateItemSetting : MarginTemplateItemSetting
    {
        public MGRMarginTemplateItemSetting()
            :base()
        { 
        
        }

        public MGRMarginTemplateItemSetting(MarginTemplateItemSetting item)
        {
            this.ChargeType = item.ChargeType;
            this.Code = item.Code;
            this.ID = item.ID;
            this.MarginByMoney = item.MarginByMoney;
            this.MarginByVolume = item.MarginByVolume;
            this.Month = item.Month;
            this.Percent = item.Percent;
            this.SetAllCodeMonth = false;
            this.SetAllMonth = false;
            this.Template_ID = item.Template_ID;
        }
        /// <summary>
        /// 是否设置到该品种所有月份
        /// </summary>
        public bool SetAllMonth { get; set; }

        /// <summary>
        /// 是否设置到所有品种所有月份
        /// </summary>
        public bool SetAllCodeMonth { get; set; }
    }
}
