using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 账户信息对象
    /// 
    /// test
    /// </summary>
    public class AccountInfo
    {
        public string Account { get; set; }
        public string Name { get; set; }//交易者姓名
        public decimal LastEquity { get; set; }//昨日权益
        public decimal NowEquity { get; set; }//当前动态权益

        public decimal RealizedPL { get; set; }//平仓盈亏
        public decimal UnRealizedPL { get; set; }//浮动盈亏
        public decimal Commission { get; set; }//手续费
        public decimal Profit { get; set; }//净利
        public decimal CashIn { get; set; }//入金
        public decimal CashOut { get; set; }//出金

        public decimal LastCredit { get; set; }//昨日优先资金
        public decimal CreditCashIn { get; set; }//优先入金
        public decimal CreditCashOut {get;set;}//优先出金
        public decimal MoneyUsed { get; set; } //总资金占用
        public decimal TotalLiquidation { get; set; }//帐户总净值
        public decimal AvabileFunds { get; set; }//帐户总可用资金  12


        public QSEnumAccountCategory Category { get; set; }//账户类别
        public QSEnumOrderTransferType OrderRouteType { get; set; }//路由类别
        public bool Execute { get; set; }//冻结
        public bool IntraDay { get; set; }//日内

        public decimal CommissionFrozen { get; set; }//冻结手续费

        //补充 增加总帐户的资金占用 资金冻结 | 资金占用+资金冻结 = moneyused
        /// <summary>
        /// 保证金占用
        /// </summary>
        public decimal Margin { get; set; }

        /// <summary>
        /// 保证金冻结
        /// </summary>
        public decimal MarginFrozen { get; set; }

        #region 多品种交易 账户财务数据
        public decimal FutMarginUsed { get; set; }//期货占用保证金
        public decimal FutMarginFrozen { get; set; }//期货冻结保证金
        public decimal FutRealizedPL { get; set; }//期货平仓盈亏
        public decimal FutUnRealizedPL { get; set; }//期货浮动盈亏
        public decimal FutCommission { get; set; }//期货交易手续费
        public decimal FutCash { get; set; }//期货交易现金
        public decimal FutLiquidation { get; set; }//期货总净值
        public decimal FutMoneyUsed { get; set; }//期货资金占用
        public decimal FutAvabileFunds { get; set; }


        public decimal OptPositionCost { get; set; }//期权持仓成本
        public decimal OptPositionValue { get; set; }//期权持仓市值
        public decimal OptRealizedPL { get; set; }//期权平仓盈亏
        public decimal OptCommission { get; set; }//期权交易手续费
        public decimal OptMoneyFrozen { get; set; }//期权资金冻结
        public decimal OptCash { get; set; }//期权交易现金
        public decimal OptMarketValue { get; set; }//期权总市值
        public decimal OptLiquidation { get; set; }//期权总净值
        public decimal OptMoneyUsed { get; set; }//期权资金占用
        public decimal OptAvabileFunds { get; set; }


        public decimal StkPositionCost { get; set; }//股票持仓成本
        public decimal StkPositionValue { get; set; }//股票欧持仓市值
        public decimal StkCommission { get; set; }//股票交易手续费
        public decimal StkBuyAmount { get; set; }//股票买入金额
        public decimal StkSellAmount { get; set; }//股票卖出金额
        public decimal StkMoneyFronzen { get; set; }//股票冻结资金
        public decimal StkRealizedPL { get; set; }//股票平仓盈亏
        public decimal StkAvabileFunds { get; set; }//异化合约可用资金

        

        #endregion

        public decimal Credit { get; set; }//信用额度


        /// <summary>
        /// 将IAccountInfo 序列化成字符串
        /// </summary>
        /// <returns></returns>
        public static string Serialize(AccountInfo info)
        {
            StringBuilder sb = new StringBuilder();
            char d=',';
            sb.Append(info.Account);
            sb.Append(d);
            sb.Append(info.LastEquity.ToString());//昨日权益
            sb.Append(d);
            sb.Append(info.NowEquity.ToString());
            sb.Append(d);
            sb.Append(info.RealizedPL.ToString());
            sb.Append(d);
            sb.Append(info.UnRealizedPL.ToString());
            sb.Append(d);
            sb.Append(info.Commission.ToString());
            sb.Append(d);
            sb.Append(info.Profit.ToString());
            sb.Append(d);
            sb.Append(info.CashIn.ToString());
            sb.Append(d);
            sb.Append(info.CashOut.ToString());
            sb.Append(d);
            sb.Append(info.MoneyUsed.ToString());
            sb.Append(d);
            sb.Append(info.TotalLiquidation.ToString());//10
            sb.Append(d);
            sb.Append(info.AvabileFunds); //
            sb.Append(d);
            sb.Append(info.Category.ToString());
            sb.Append(d);
            sb.Append(info.OrderRouteType.ToString());
            sb.Append(d);
            sb.Append(info.Execute.ToString());
            sb.Append(d);
            sb.Append(info.IntraDay.ToString());//15

            sb.Append(d);
            sb.Append(info.FutMarginUsed.ToString());
            sb.Append(d);
            sb.Append(info.FutMarginFrozen.ToString());
            sb.Append(d);
            sb.Append(info.FutRealizedPL.ToString());
            sb.Append(d);
            sb.Append(info.FutUnRealizedPL.ToString());
            sb.Append(d);
            sb.Append(info.FutCommission.ToString());
            sb.Append(d);
            sb.Append(info.FutCash.ToString());
            sb.Append(d);
            sb.Append(info.FutLiquidation.ToString());
            sb.Append(d);
            sb.Append(info.FutMoneyUsed.ToString());
            sb.Append(d);
            sb.Append(info.FutAvabileFunds.ToString());//24

            sb.Append(d);
            sb.Append(info.OptPositionCost.ToString());
            sb.Append(d);
            sb.Append(info.OptPositionValue.ToString());
            sb.Append(d);
            sb.Append(info.OptRealizedPL.ToString());
            sb.Append(d);
            sb.Append(info.OptCommission.ToString());
            sb.Append(d);
            sb.Append(info.OptMoneyFrozen.ToString());
            sb.Append(d);
            sb.Append(info.OptCash.ToString());
            sb.Append(d);
            sb.Append(info.OptMarketValue.ToString());
            sb.Append(d);
            sb.Append(info.OptLiquidation.ToString());
            sb.Append(d);
            sb.Append(info.OptMoneyUsed.ToString());
            sb.Append(d);
            sb.Append(info.OptAvabileFunds.ToString());//34

            sb.Append(d);
            sb.Append(info.StkPositionCost);
            sb.Append(d);
            sb.Append(info.StkPositionValue);
            sb.Append(d);
            sb.Append(info.StkCommission);
            sb.Append(d);
            sb.Append(info.StkBuyAmount);
            sb.Append(d);
            sb.Append(info.StkSellAmount);
            sb.Append(d);
            sb.Append(info.StkMoneyFronzen);
            sb.Append(d);
            sb.Append(info.StkRealizedPL);
            sb.Append(d);
            sb.Append(0);
            sb.Append(d);
            sb.Append(0);
            sb.Append(d);
            sb.Append(0);
            sb.Append(d);
            sb.Append(info.StkAvabileFunds);//45

            //
            sb.Append(d);
            sb.Append(info.Margin.ToString());
            sb.Append(d);
            sb.Append(info.MarginFrozen.ToString());
            sb.Append(d);
            sb.Append(info.Credit);
            sb.Append(d);
            sb.Append(info.LastCredit);
            sb.Append(d);
            sb.Append(info.CreditCashIn);
            sb.Append(d);
            sb.Append(info.CreditCashOut);
            sb.Append(d);
            sb.Append(info.CommissionFrozen);
            sb.Append(d);
            sb.Append(info.Name);
            return sb.ToString();
        }


        /// <summary>
        /// 反序列化字符串生成AccountInfo对象
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static AccountInfo Deserialize(string msg)
        {
            string[] r = msg.Split(',');
            AccountInfo a = new AccountInfo();
            if (r.Length >= 10)
            {
                a.Account = r[0];
                a.LastEquity = decimal.Parse(r[1]);
                a.NowEquity = decimal.Parse(r[2]);
                a.RealizedPL = decimal.Parse(r[3]);
                a.UnRealizedPL = decimal.Parse(r[4]);
                a.Commission = decimal.Parse(r[5]);
                a.Profit = decimal.Parse(r[6]);
                a.CashIn = decimal.Parse(r[7]);
                a.CashOut = decimal.Parse(r[8]);
                a.MoneyUsed = decimal.Parse(r[9]);
                a.TotalLiquidation = decimal.Parse(r[10]);
                a.AvabileFunds = decimal.Parse(r[11]);
                a.Category = (QSEnumAccountCategory)Enum.Parse(typeof(QSEnumAccountCategory),r[12]);
                a.OrderRouteType = (QSEnumOrderTransferType)Enum.Parse(typeof(QSEnumOrderTransferType),r[13]);
                a.Execute = bool.Parse(r[14]);
                a.IntraDay = bool.Parse(r[15]);
                a.FutMarginUsed = decimal.Parse(r[16]);
                a.FutMarginFrozen = decimal.Parse(r[17]);
                a.FutRealizedPL = decimal.Parse(r[18]);
                a.FutUnRealizedPL = decimal.Parse(r[19]);
                a.FutCommission = decimal.Parse(r[20]);
                a.FutCash = decimal.Parse(r[21]);
                a.FutLiquidation = decimal.Parse(r[22]);
                a.FutMoneyUsed = decimal.Parse(r[23]);
                a.FutAvabileFunds = decimal.Parse(r[24]);

                a.OptPositionCost = decimal.Parse(r[25]);
                a.OptPositionValue = decimal.Parse(r[26]);
                a.OptRealizedPL = decimal.Parse(r[27]);
                a.OptCommission = decimal.Parse(r[28]);
                a.OptMoneyFrozen = decimal.Parse(r[29]);
                a.OptCash = decimal.Parse(r[30]);
                a.OptMarketValue = decimal.Parse(r[31]);
                a.OptLiquidation = decimal.Parse(r[32]);
                a.OptMoneyUsed = decimal.Parse(r[33]);
                a.OptAvabileFunds = decimal.Parse(r[34]);

                a.StkPositionCost = decimal.Parse(r[35]);
                a.StkPositionValue = decimal.Parse(r[36]);
                a.StkCommission = decimal.Parse(r[37]);
                a.StkBuyAmount = decimal.Parse(r[38]);
                a.StkSellAmount = decimal.Parse(r[39]);
                a.StkMoneyFronzen = decimal.Parse(r[40]);
                a.StkRealizedPL = decimal.Parse(r[41]);
                //a.StkRealizedPL = decimal.Parse(r[42]);
                //a.StkLiquidation = decimal.Parse(r[43]);
                //a.StkMoneyUsed = decimal.Parse(r[44]);
                a.StkAvabileFunds = decimal.Parse(r[45]);

                a.Margin = decimal.Parse(r[46]);
                a.MarginFrozen = decimal.Parse(r[47]);
                a.Credit = decimal.Parse(r[48]);
                a.LastCredit = decimal.Parse(r[49]);
                a.CreditCashIn = decimal.Parse(r[50]);
                a.CreditCashOut = decimal.Parse(r[51]);
                a.CommissionFrozen = decimal.Parse(r[52]);
                a.Name = r[53];
            }
            return a;
        }
    }
}
