using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    /// <summary>
    /// 账户交易数据财务统计
    /// 用于管理段与服务端之间的交易帐户实时财务数据更新
    /// </summary>
    public class AccountStatistic
    {
        public string Account { get; set; }
        public decimal NowEquity { get; set; }//当前动态权益
        public decimal Credit { get; set; }//信用额度
        public decimal Margin { get; set; }//占用保证金
        public decimal ForzenMargin { get; set; }//冻结保证金
        public decimal RealizedPL { get; set; }//平仓盈亏
        public decimal UnRealizedPL { get; set; }//浮动盈亏
        public decimal Commission { get; set; }//手续费
        public decimal Profit { get; set; }//净利
        public int TotalPositionSize { get; set; }//所有持仓手数量


        public decimal StkBuyAmount { get; set; }//证券买入额
        public decimal StkSellAmount { get; set; }//证券卖出额
        public decimal StkCommission { get; set; }//证券手续费
        public decimal StkMoneyFronzen { get; set; }//冻结资金
        public decimal StkAvabileFunds { get; set; }//证券可用资金
        public decimal StkPositoinValue { get; set; }//证券市值
        public decimal StkPositionCost { get; set; }//证券成本
        public decimal StkRealizedPL { get; set; }//证券平仓盈亏


        public static string Serialize(AccountStatistic info)
        {
            const char d = ',';
            StringBuilder sb = new StringBuilder();
            sb.Append(info.NowEquity);
            sb.Append(d);
            sb.Append(info.Margin);
            sb.Append(d);
            sb.Append(info.ForzenMargin);
            sb.Append(d);
            sb.Append(d);
            sb.Append(info.RealizedPL);
            sb.Append(d);
            sb.Append(info.UnRealizedPL);
            sb.Append(d);
            sb.Append(info.Commission);
            sb.Append(d);
            sb.Append(info.Profit);
            sb.Append(d);
            sb.Append(info.Account);
            sb.Append(d);
            sb.Append(info.TotalPositionSize);
            sb.Append(d);
            sb.Append(info.Credit);

            sb.Append(d);
            sb.Append(info.StkBuyAmount);
            sb.Append(d);
            sb.Append(info.StkSellAmount);
            sb.Append(d);
            sb.Append(info.StkCommission);
            sb.Append(d);
            sb.Append(info.StkMoneyFronzen);
            sb.Append(d);
            sb.Append(info.StkAvabileFunds);
            sb.Append(d);
            sb.Append(info.StkPositoinValue);
            sb.Append(d);
            sb.Append(info.StkPositionCost);
            sb.Append(d);
            sb.Append(info.StkRealizedPL);

            return sb.ToString();

        }

        public static AccountStatistic Deserialize(string msg)
        {
            string[] r = msg.Split(',');
            AccountStatistic a = new AccountStatistic();
            a.NowEquity = Decimal.Parse(r[0]);
            a.Margin = Decimal.Parse(r[1]);
            a.ForzenMargin = Decimal.Parse(r[2]);
            a.RealizedPL = Decimal.Parse(r[4]);
            a.UnRealizedPL = Decimal.Parse(r[5]);
            a.Commission = Decimal.Parse(r[6]);
            a.Profit = Decimal.Parse(r[7]);
            a.Account = r[8];
            a.TotalPositionSize = int.Parse(r[9]);
            a.Credit = decimal.Parse(r[10]);


            a.StkBuyAmount = decimal.Parse(r[11]);
            a.StkSellAmount = decimal.Parse(r[12]);
            a.StkCommission = decimal.Parse(r[13]);
            a.StkMoneyFronzen = decimal.Parse(r[14]);
            a.StkAvabileFunds = decimal.Parse(r[15]);
            a.StkPositoinValue = decimal.Parse(r[16]);
            a.StkPositionCost = decimal.Parse(r[17]);
            a.StkRealizedPL = decimal.Parse(r[18]);

            return a;
        }
    }
}
