using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 管理端界面权限
    /// 用于控制管理端的界面元素的现实
    /// </summary>
    public class UIAccess : JsonObjectBase
    {
        /// <summary>
        /// 数据库ID
        /// </summary>
        public int id { get; set; }


        /// <summary>
        /// 域ID
        /// </summary>
        public int domain_id { get; set; }


        /// <summary>
        /// 界面权限名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string desp { get; set; }

        /// <summary>
        /// 添加交易帐户
        /// </summary>
        [PermissionFieldAttr("添加帐户", "是否有权添加分帐户")]
        public bool r_account_add { get; set; }

        /// <summary>
        /// 删除交易帐户权限
        /// </summary>
        [PermissionFieldAttr("删除帐户", "是否有权删除分帐户")]
        public bool r_account_del { get; set; }

        /// <summary>
        /// 是否有权进行交易操作
        /// </summary>
        [PermissionFieldAttr("交易操作","是否有权向分帐户下单或平仓")]
        public bool r_execution { get; set; }

        /// <summary>
        /// 是否有权冻结交易账户
        /// </summary>
        [PermissionFieldAttr("冻结帐户", "是否有权冻结或激活交易帐户")]
        public bool r_block { get; set; }

        /// <summary>
        /// 执行出入金操作
        /// </summary>
        [PermissionFieldAttr("出入金", "是否有权执行出入金操作")]
        public bool r_cashop { get; set; }

        /// <summary>
        /// 是否有权设定风控规则
        /// </summary>
        [PermissionFieldAttr("风控规则", "是否有权设定帐户风控规则")]
        public bool r_riskrule { get; set; }


        /// <summary>
        /// 设置手续费权限
        /// </summary>
        [PermissionFieldAttr("手续费模板", "是否有权设定帐户手续费")]
        public bool r_commission { get; set; }

        /// <summary>
        /// 设置保证金权限
        /// </summary>
        [PermissionFieldAttr("保证金模板", "是否有权设定帐户保证金")]
        public bool r_margin { get; set; }

        /// <summary>
        /// 设置交易参数权限
        /// </summary>
        [PermissionFieldAttr("交易参数", "是否有权设定帐户交易参数")]
        public bool r_exstrategy { get; set; }

        /// <summary>
        /// 修改帐户基本信息
        /// </summary>
        [PermissionFieldAttr("帐户信息", "是否有权修改帐户信息")]
        public bool r_account_info { get; set; }

        /// <summary>
        /// 是否有权修改帐户隔夜设置
        /// </summary>
        [PermissionFieldAttr("隔夜设置", "是否有权修改帐户隔夜设置")]
        public bool r_account_interday { get; set; }
    }
}
