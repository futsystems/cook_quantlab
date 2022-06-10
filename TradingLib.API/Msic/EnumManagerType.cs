using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingLib.API
{
    public enum QSEnumManagerType
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        [Description("管理员")]
        ROOT = 0,//系统总权限 可以进行所有操作

        /// <summary>
        /// 代理商
        /// </summary>
        [Description("代理商")]
        AGENT = 1,//代理人员,代理人员对其开设的帐户有绝对权限,

        /// <summary>
        /// 财务人员角色
        /// </summary>
        [Description("财务员")]
        ACCOUNTENTER = 2,//财务人员 可以处理财务操作出入金，调整手续费等

        /// <summary>
        /// 风控管理员角色
        /// </summary>
        [Description("风控员")]
        RISKER = 3,//风控人员 可以进行强平,设置帐户相关全新或风控规则

        /// <summary>
        /// 观察员
        /// </summary>
        [Description("观察员")]
        MONITER = 4,//观察员,无法进行帐户设置,只能查看帐户当前状态
    }
}
