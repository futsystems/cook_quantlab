using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{

    /// <summary>
    /// PostionEx用于封装持仓汇总信息
    /// 将Position工作对象的持仓状态 封装成PositionEx用于接收客户端的查询
    /// </summary>
    public  class PositionEx
    {
        public bool IsValid
        {
            get { 
                if(string.IsNullOrEmpty(this.Account) || string.IsNullOrEmpty(this.Symbol))
                    return false;
                return true;
            }
        }
        /// <summary>
        /// 交易日
        /// </summary>
        public int Tradingday { get; set; }

        /// <summary>
        /// 帐户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange { get; set; }
        /// <summary>
        /// 乘数
        /// </summary>
        public int Multiple { get; set; }

        /// <summary>
        /// 持仓均价 点数
        /// </summary>
        public decimal AvgPrice { get; set; }

        /// <summary>
        /// 持仓方向
        /// </summary>
        public bool Side { get; set; }


        /// <summary>
        /// 持仓描述类型
        /// </summary>
        public QSEnumPositionDirectionType DirectionType { get; set; }



        #region 当日开平汇总
        /// <summary>
        /// 开仓量 
        /// </summary>
        public int OpenVolume { get; set; }

        /// <summary>
        /// 开仓金额 SUM（今日开仓数量 * 开仓价 * 合约乘数)
        /// </summary>
        public decimal OpenAmount { get; set; }

        /// <summary>
        /// 平仓量
        /// </summary>
        public int CloseVolume { get; set; }

        /// <summary>
        /// 平仓金额 SUM（平仓数量 * 平仓价 * 合约乘数）
        /// </summary>
        public decimal CloseAmount { get; set; }

        #endregion

        /// <summary>
        /// 平仓盈亏点数
        /// 
        /// 每次有平仓明细产生时，会累加平仓盈亏点数和对应的金额
        /// 平仓明细中的平仓盈亏计算 1.今仓 开仓价- 平价价  2.昨仓 平仓价 - 昨结价
        /// </summary>
        public decimal ClosePL { get; set; }

        /// <summary>
        /// 当日平仓盈亏金额
        /// </summary>
        public decimal CloseProfit { get; set; }

        /// <summary>
        /// 当日浮动盈亏点数
        /// </summary>
        public decimal UnRealizedPL { get; set; }

        /// <summary>
        /// 浮动盈亏金额/持仓盈亏
        /// </summary>
        public decimal UnRealizedProfit { get; set; }



        /// <summary>
        /// 持仓成本
        /// </summary>
        public decimal PositionCost { get; set; }


        /// <summary>
        /// 开仓成本
        /// </summary>
        public decimal OpenCost { get; set; }



        #region 持仓数量

        /// <summary>
        /// 今仓数量 
        /// </summary>
        public int TodayPosition { get; set; }

        /// <summary>
        /// 昨仓数量
        /// </summary>
        public int YdPosition { get; set; }

        /// <summary>
        /// 持仓数量
        /// </summary>
        public int Position { get; set; }

        #endregion


        /// <summary>
        /// 昨日结算价
        /// </summary>
        public decimal LastSettlementPrice { get; set; }

        /// <summary>
        /// 结算价
        /// </summary>
        public decimal SettlementPrice { get; set; }


        /// <summary>
        /// 盯市平仓盈亏
        /// </summary>
        public decimal CloseProfitByDate { get; set; }

        /// <summary>
        /// 逐笔平仓盈亏
        /// </summary>
        public decimal CloseProfitByTrade { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public decimal Margin { get; set; }

        /// <summary>
        /// 手续费
        /// </summary>
        public decimal Commission { get; set; }


        public PositionEx()
        {
            Account = string.Empty;
            Symbol = string.Empty;
            Multiple = 1;
            //ClosedPL = 0;
            Position = 0;
            AvgPrice = 0;
            Side = true;
            //Size = 0;

            OpenAmount = 0;
            //OpenAVGPrice = 0;
            OpenVolume = 0;
            CloseAmount = 0;
            //CloseAVGPrice = 0;
            CloseVolume = 0;
            DirectionType = QSEnumPositionDirectionType.BothSide;
            CloseProfit = 0;
            PositionCost = 0;
            UnRealizedPL = 0;
            UnRealizedProfit = 0;
            OpenCost = 0;
            Margin = 0;
            CloseProfitByDate = 0;
            CloseProfitByTrade = 0;
        }

        public static string Serialize(PositionEx p)
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(p.Account);
            sb.Append(d);
            sb.Append(p.Symbol);
            sb.Append(d);
            sb.Append(p.Multiple.ToString());
            sb.Append(d);
            sb.Append(p.ClosePL);
            sb.Append(d);
            sb.Append(p.Position);
            sb.Append(d);
            sb.Append(p.AvgPrice.ToString());
            sb.Append(d);
            sb.Append(p.Side.ToString());
            sb.Append(d);
            sb.Append("0");
            sb.Append(d);
            sb.Append(p.OpenAmount.ToString());
            sb.Append(d);
            sb.Append("0");
            sb.Append(d);
            sb.Append(p.OpenVolume.ToString());
            sb.Append(d);
            sb.Append(p.CloseAmount.ToString());
            sb.Append(d);
            sb.Append("0");
            sb.Append(d);
            sb.Append(p.CloseVolume.ToString());
            sb.Append(d);
            sb.Append(p.DirectionType.ToString());
            sb.Append(d);
            sb.Append(p.CloseProfit.ToString());
            sb.Append(d);
            sb.Append(p.PositionCost.ToString());
            sb.Append(d);
            sb.Append(p.UnRealizedPL.ToString());
            sb.Append(d);
            sb.Append(p.UnRealizedProfit.ToString());
            sb.Append(d);
            sb.Append(p.Commission);
            sb.Append(d);
            sb.Append(p.TodayPosition);
            sb.Append(d);
            sb.Append(p.YdPosition);
            sb.Append(d);
            sb.Append(p.LastSettlementPrice);
            sb.Append(d);
            sb.Append(p.SettlementPrice);
            sb.Append(d);
            sb.Append(p.CloseProfitByDate);
            sb.Append(d);
            sb.Append(p.Tradingday);
            sb.Append(d);
            sb.Append(p.OpenCost);
            sb.Append(d);
            sb.Append(p.Margin);
            sb.Append(d);
            sb.Append(p.CloseProfitByTrade);
            sb.Append(d);
            sb.Append(p.Exchange);
            return sb.ToString();

        }


        public static PositionEx Deserialize(string msg)
        {
            PositionEx p = new PositionEx();
            string [] rec =  msg.Split(',');
            p.Account = rec[0];
            p.Symbol = rec[1];
            p.Multiple = int.Parse(rec[2]);
            p.ClosePL = decimal.Parse(rec[3]);
            p.Position = int.Parse(rec[4]);
            p.AvgPrice = decimal.Parse(rec[5]);
            p.Side = bool.Parse(rec[6]);
            //p.Size = int.Parse(rec[7]);
            p.OpenAmount = decimal.Parse(rec[8]);
            //p.OpenAVGPrice = decimal.Parse(rec[9]);
            p.OpenVolume = int.Parse(rec[10]);
            p.CloseAmount = decimal.Parse(rec[11]);
            //p.CloseAVGPrice = decimal.Parse(rec[12]);
            p.CloseVolume = int.Parse(rec[13]);
            p.DirectionType = (QSEnumPositionDirectionType)Enum.Parse(typeof(QSEnumPositionDirectionType),rec[14]);
            p.CloseProfit = decimal.Parse(rec[15]);
            p.PositionCost = decimal.Parse(rec[16]);
            p.UnRealizedPL = decimal.Parse(rec[17]);
            p.UnRealizedProfit = decimal.Parse(rec[18]);
            p.Commission = decimal.Parse(rec[19]);
            p.TodayPosition = int.Parse(rec[20]);
            p.YdPosition = int.Parse(rec[21]);
            p.LastSettlementPrice = decimal.Parse(rec[22]);
            p.SettlementPrice = decimal.Parse(rec[23]);
            p.CloseProfitByDate = decimal.Parse(rec[24]);
            p.Tradingday = int.Parse(rec[25]);
            p.OpenCost = decimal.Parse(rec[26]);
            p.Margin = decimal.Parse(rec[27]);
            p.CloseProfitByTrade = decimal.Parse(rec[28]);
            p.Exchange = rec[29];
            return p;
        }
    }
}
