using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class DomainImpl : Domain
    {
        public DomainImpl()
        {
            this.ID=0;
            this.Name="分区";
            this.LinkMan = string.Empty;
            this.Mobile=string.Empty;
            this.QQ = string.Empty;
            this.Email = string.Empty;
            this.AccLimit = 1;
            this.DateExpired = Util.ToTLDate();
            this.DateCreated = Util.ToTLDate();
            this.Super = false;
            this.Dedicated = false;
            this.RouterGroupLimit = 1;
            this.RouterItemLimit = 1;
            this.VendorLimit = 1;
            this.InterfaceList=string.Empty;
            this.FinSPList = string.Empty;
            this.Module_Agent = false;
            this.Module_FinService = false;
            this.Module_PayOnline = false;
            this.Module_SubAgent = false;
            this.Module_Follow = false;
            
            this.Router_Live = true;
            this.Router_Sim = true;
            this.Switch_Router = true;
            this.Misc_InsertTrade = false;

            this.Cfg_GrossPosition = true;
            this.Cfg_MaxMarginSide = true;
            this.Cfg_FollowStrategyNum = 0;


        }
        /// <summary>
        /// 域ID
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// 域名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkMan { get; set; }


        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }


        /// <summary>
        /// QQ号码
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }


        /// <summary>
        /// 帐户数目限制
        /// </summary>
        public int AccLimit { get; set; }


        /// <summary>
        /// 代理数量限制
        /// </summary>
        public int AgentLimit { get; set; }


        /// <summary>
        /// 过期日
        /// </summary>
        public int DateExpired { get; set; }


        /// <summary>
        /// 创建日
        /// </summary>
        public int DateCreated { get; set; }


        /// <summary>
        /// 是否是超级域
        /// </summary>
        public bool Super { get; set; }

        /// <summary>
        /// 独立安装标识
        /// </summary>
        public bool Dedicated { get; set; }

        /// <summary>
        /// 路由组数量限制
        /// </summary>
        public int RouterGroupLimit { get; set; }

        /// <summary>
        /// 路由组内路由项目数量限制
        /// </summary>
        public int RouterItemLimit { get; set; }

        /// <summary>
        /// 实盘帐户数量限制
        /// </summary>
        public int VendorLimit { get; set; }

        /// <summary>
        /// 接口列表
        /// </summary>
        public string InterfaceList { get; set; }

        /// <summary>
        /// 配资服务计划列表
        /// </summary>
        public string FinSPList { get; set; }

        /// <summary>
        /// 代理模块
        /// </summary>
        public bool Module_Agent { get; set; }

        /// <summary>
        /// 是否支持多级代理
        /// </summary>
        public bool Module_SubAgent { get; set; }

        /// <summary>
        /// 配资模块
        /// </summary>
        public bool Module_FinService { get; set; }

        /// <summary>
        /// 在线出入金模块
        /// </summary>
        public bool Module_PayOnline { get; set; }

        /// <summary>
        /// 跟单模块
        /// </summary>
        public bool Module_Follow { get; set; }

        /// <summary>
        /// 滑点模块
        /// </summary>
        public bool Module_Slip { get; set; }

        /// <summary>
        /// 实盘路由
        /// </summary>
        public bool Router_Live { get; set; }

        /// <summary>
        /// 模拟路由
        /// </summary>
        public bool Router_Sim { get; set; }

        /// <summary>
        /// 切换路由模式
        /// </summary>
        public bool Switch_Router { get; set; }

        /// <summary>
        /// 调试插入成交
        /// </summary>
        public bool Misc_InsertTrade { get; set; }

        /// <summary>
        /// 是否支持大额单边保证金算法
        /// </summary>
        public bool Cfg_MaxMarginSide { get; set; }

        /// <summary>
        /// 是否支持综合持仓
        /// Gross可以同时持有多头与空头持仓
        /// </summary>
        public bool Cfg_GrossPosition { get; set; }

        /// <summary>
        /// 跟单策略数
        /// </summary>
        public int Cfg_FollowStrategyNum { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int CFG_SyncVendor_ID { get; set; }



        /// <summary>
        /// 是否处于运营状态
        /// </summary>
        public bool IsProduction { get; set; }

        /// <summary>
        /// 分帐户个数
        /// </summary>
        public int DiscountNum { get; set; }

    }
}
