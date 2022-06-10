using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{

    /// <summary>
    /// 签约银行
    /// </summary>
    public class ContractBank
    {
        /// <summary>
        /// 数据库全局ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 银行编号
        /// </summary>
        public string BrankID { get; set; }

        /// <summary>
        /// 分支机构代码
        /// </summary>
        public string BrankBrchID { get; set; }
    }
}
