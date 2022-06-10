using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    public class AccountCreation
    {
        public AccountCreation()
        {
            this.Account = string.Empty;
            this.Password = string.Empty;
            this.Category = QSEnumAccountCategory.SUBACCOUNT;
            this.RouterType = QSEnumOrderTransferType.SIM;

            this.UserID = 0;//交易帐户对应其他系统的userID 用于实现其他系统user与交易系统Account关联
            this.RouterID = 0;//交易帐户路由组
            this.BaseManagerID = 0;//该交易帐户属于哪个manager

            this.Profile = new AccountProfile();
        }

        /// <summary>
        /// 交易帐户对应的基本信息
        /// </summary>
        public AccountProfile Profile { get; set; }

        /// <summary>
        /// 交易帐户编号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 设定的帐户和密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 帐户类型
        /// </summary>
        public QSEnumAccountCategory Category { get; set; }

        /// <summary>
        /// 路由类别
        /// </summary>
        public QSEnumOrderTransferType RouterType { get; set; }

        /// <summary>
        /// 货币类别
        /// </summary>
        public CurrencyType Currency { get; set; }

        /// <summary>
        /// 用户ID预留与web站点帐户系统
        /// </summary>
        public int UserID { get; set; }


        /// <summary>
        /// 该交易帐户主管理员ID
        /// </summary>
        public int BaseManagerID { get; set; }


        /// <summary>
        /// 交易路由ID
        /// </summary>
        public int RouterID { get; set; }
    }
}
