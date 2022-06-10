using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PermissionFieldAttr:Attribute
    {
        public PermissionFieldAttr(string title, string desp)
        {
            this.Title = title;
            this.Desp = desp;
        }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 权限描述
        /// </summary>
        public string Desp { get; set; }
    }
}
