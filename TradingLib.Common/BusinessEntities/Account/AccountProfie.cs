using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    /// <summary>
    /// Profit信息
    /// 用于记录交易帐户对应的客户信息
    /// 联系信息，银行卡信息等
    /// </summary>
    public class AccountProfile
    {
        /// <summary>
        /// 交易帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }


        /// <summary>
        /// QQ号码
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDCard { get; set; }


        /// <summary>
        /// 银行编号
        /// </summary>
        public int Bank_ID { get; set; }


        /// <summary>
        /// 分支银行
        /// </summary>
        public string Branch { get; set; }


        /// <summary>
        /// 银行帐号
        /// </summary>
        public string BankAC { get; set; }

        /// <summary>
        /// 期货公司
        /// </summary>
        public string Broker { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Memo { get; set; }

    }
}
