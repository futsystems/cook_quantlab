using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TradingLib.API
{

    /// <summary>
    /// 底层帐户接口
    /// 集成了财务数据,交易信息,风控检查等几大功能接口
    /// </summary>
    public interface IAccount : IFinanceTotal, ITradingInfo, IGeneralCheck, IRiskRule,IAccountIndicator
    {

        /// <summary>
        /// 交易帐号对应的数据库全局ID
        /// </summary>
        string ID { get; }

        /// <summary>
        /// 密码
        /// </summary>
        string Pass { get; set; }

        /// <summary>
        /// 是否可以进行交易
        /// </summary>
        bool Execute { get; set; }

        /// <summary>
        /// 是否处于警告状态
        /// </summary>
        bool IsWarn { get; set; }



        /// <summary>
        /// 是否日内交易
        /// </summary>
        bool IntraDay { get; set; }

        /// <summary>
        /// 账户委托转发通道类型
        /// </summary>
        QSEnumOrderTransferType OrderRouteType { get; set; }

        /// <summary>
        /// 账户类型
        /// </summary>
        QSEnumAccountCategory Category { get; set; }

        /// <summary>
        /// 货币类别
        /// </summary>
        CurrencyType Currency { get; set; }

        /// <summary>
        /// 账户建立时间
        /// </summary>
        DateTime CreatedTime { get; set; }


        #region 模板编号
        /// <summary>
        /// 手续费模板ID
        /// </summary>
        int Commission_ID { get; set; }

        /// <summary>
        /// 保证金模板ID
        /// </summary>
        int Margin_ID { get; set; }

        /// <summary>
        /// 交易参数模板ID
        /// </summary>
        int ExStrategy_ID { get; set; }

        #endregion

        #region 结算与结算数据加载
        /// <summary>
        /// 交易所结算
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="settleday"></param>
        void SettleExchange(Exchange exchange, int settleday);

        /// <summary>
        /// 交易账户结算
        /// </summary>
        void SettleAccount(int settleday);

        /// <summary>
        /// 加载交易所结算记录
        /// </summary>
        /// <param name="settle"></param>
        void LoadExchangeSettlement(ExchangeSettlement settle);

        /// <summary>
        /// 加载出入金操作
        /// </summary>
        /// <param name="txn"></param>
        void LoadCashTrans(CashTransaction txn);

        /// <summary>
        /// 上次结算日
        /// </summary>
        DateTime SettleDateTime { get; set; }

        /// <summary>
        /// 确认结算日期
        /// </summary>
        long SettlementConfirmTimeStamp { get; set; }

        #endregion

        #region 对象绑定
        /// <summary>
        /// 账户绑定路由组
        /// </summary>
        RouterGroup RouteGroup { get;}

        /// <summary>
        /// 账户所在域
        /// </summary>
        Domain Domain { get;}

        /// <summary>
        /// 账户User绑定 用于与其他系统用户进行关联
        /// </summary>
        int UserID { get; set; }

        /// <summary>
        /// 账户所属管理员
        /// </summary>
        int Mgr_fk { get; set; }
        #endregion


        #region 交易回话类

        /// <summary>
        /// 是否登入
        /// </summary>
        bool IsLogin { get; set; }

        /// <summary>
        /// 回话信息 
        /// </summary>
        string SessionInfo { get; set; }

        #endregion


        /// <summary>
        /// 重置账户状态,用于每日造成开盘时,重置数据 
        /// </summary>
        void Reset();

        /// <summary>
        /// 删除标志，如果已经删除 则管理端不显示，下次启动不会加载
        /// </summary>
        bool Deleted { get; set; }


        /// <summary>
        /// 获得帐户交易通知
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetNotice();
        

    }


}
