using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{




    /// <summary>
    /// 域接口
    /// </summary>
    public interface Domain
    {
        /// <summary>
        /// 域ID
        /// </summary>
        int ID { get; set; }


        /// <summary>
        /// 域名称
        /// </summary>
        string Name { get; set; }


        /// <summary>
        /// 联系人
        /// </summary>
        string LinkMan { get; set; }


        /// <summary>
        /// 手机号码
        /// </summary>
        string Mobile { get; set; }


        /// <summary>
        /// QQ号码
        /// </summary>
        string QQ { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        string Email { get; set; }


        


        /// <summary>
        /// 过期日
        /// </summary>
        int DateExpired { get; set; }


        /// <summary>
        /// 创建日
        /// </summary>
        int DateCreated { get; set; }

        /// <summary>
        /// 是否是超级域
        /// </summary>
        bool Super { get; set; }

        /// <summary>
        /// 独立安装标识
        /// </summary>
        bool Dedicated { get; set; }

        #region limit
        /// <summary>
        /// 帐户数目限制
        /// </summary>
        int AccLimit { get; set; }

        /// <summary>
        /// 代理数量限制
        /// </summary>
        int AgentLimit { get; set; }

        /// <summary>
        /// 路由组数量限制
        /// </summary>
        int RouterGroupLimit { get; set; }

        /// <summary>
        /// 路由组内路由项目数量限制
        /// </summary>
        int RouterItemLimit { get; set; }

        /// <summary>
        /// 实盘帐户数量限制
        /// </summary>
        int VendorLimit { get; set; }

        /// <summary>
        /// 接口列表
        /// </summary>
        string InterfaceList { get; set; }

        /// <summary>
        /// 配资服务计划列表
        /// </summary>
        string FinSPList { get; set; }

        /// <summary>
        /// 代理模块
        /// </summary>
        bool Module_Agent { get; set; }

        /// <summary>
        /// 是否支持多级代理
        /// </summary>
        bool Module_SubAgent{get;set;}

        /// <summary>
        /// 配资模块
        /// </summary>
        bool Module_FinService { get; set; }

        /// <summary>
        /// 在线出入金模块
        /// </summary>
        bool Module_PayOnline { get; set; }

        /// <summary>
        /// 跟单模块
        /// </summary>
        bool Module_Follow { get; set; }

        /// <summary>
        /// 滑点模块
        /// </summary>
        bool Module_Slip { get; set; }

        /// <summary>
        /// 实盘路由
        /// </summary>
        bool Router_Live { get; set; }

        /// <summary>
        /// 模拟路由
        /// </summary>
        bool Router_Sim { get; set; }


        /// <summary>
        /// 切换路由模式
        /// </summary>
        bool Switch_Router { get; set; }

        /// <summary>
        /// 调试插入成交
        /// </summary>
        bool Misc_InsertTrade { get; set; }
        #endregion


        #region 域设置
        /// <summary>
        /// 同步实盘帐户ID
        /// </summary>
        int CFG_SyncVendor_ID { get; set; }

        /// <summary>
        /// 是否处于运营状态
        /// </summary>
        bool IsProduction { get; set; }

        /// <summary>
        /// 分帐户个数
        /// </summary>
        int DiscountNum { get; set; }


        /// <summary>
        /// 是否支持大额单边保证金算法
        /// </summary>
        bool Cfg_MaxMarginSide { get; set; }

        /// <summary>
        /// 是否支持综合持仓
        /// Gross可以同时持有多头与空头持仓
        /// </summary>
        bool Cfg_GrossPosition { get; set; }

        /// <summary>
        /// 跟单策略数
        /// </summary>
        int Cfg_FollowStrategyNum { get; set; }

        #endregion
    }
}
