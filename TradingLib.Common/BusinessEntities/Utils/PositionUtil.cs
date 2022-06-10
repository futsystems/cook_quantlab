using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    public static class PostionUtil
    {

        ///// <summary>
        ///// 获得posiiton的key值 用于对该positon进行唯一性标识
        ///// </summary>
        ///// <param name="pos"></param>
        ///// <returns></returns>
        public static string GetKey(this Position pos,bool positionside)
        {

            

            StringBuilder sb = new StringBuilder();
            char d = '-';
            sb.Append(pos.Account);
            sb.Append(d);
            sb.Append(pos.Symbol);
            sb.Append(d);
            sb.Append(positionside?QSEnumPositionDirectionType.Long.ToString():QSEnumPositionDirectionType.Short.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// 获得持仓键
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static string GetPositionKey(this Position pos)
        {
            return string.Format("{0}-{1}-{2}", pos.Account, pos.Symbol, pos.DirectionType);
        }

        /// <summary>
        /// 判断持仓是否是按保证金结算
        /// 持仓结算模式
        /// 1.保证金交易制度
        /// 该模式下 建立持仓是以保证金占用的方式进行 以期货为代表，建立持仓时不发生资金与资产的转换
        /// 2.全额交易制度
        /// 该模式下 建立持仓是以资金与资产的转换方式进行 以股票为代表，建立持仓时资金减少 资产增加 结算时不对账面浮动盈亏进行结算
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static bool IsMarginTrading(this Position pos)
        {
            switch (pos.oSymbol.SecurityType)
            { 
                case SecurityType.FUT:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 计算持仓保证金
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static decimal CalcPositionMargin(this Position p)
        {
            //异化合约按照固定金额来计算
            //if (p.oSymbol.SecurityType == SecurityType.INNOV)
            //{
            //    return p.UnsignedSize * (p.oSymbol.Margin + (p.oSymbol.ExtraMargin > 0 ? p.oSymbol.ExtraMargin : 0));//通过固定保证金来计算持仓保证金占用
            //}

            //其余品种保证金按照最新价格计算
            if (p.oSymbol.Margin <= 1)
            {
                //需要判断价格的有效性
                decimal m = p.UnsignedSize * p.LastPrice * p.oSymbol.Multiple * p.oSymbol.Margin;
                return m;
            }
            else
                return p.oSymbol.Margin * p.UnsignedSize;
        }

        /// <summary>
        /// 计算结算保证金
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static decimal CalcPositionSettleMargin(this Position p)
        {
            //异化合约按照固定金额来计算
            //if (p.oSymbol.SecurityType == SecurityType.INNOV)
            //{
            //    return p.UnsignedSize * (p.oSymbol.Margin + (p.oSymbol.ExtraMargin > 0 ? p.oSymbol.ExtraMargin : 0));//通过固定保证金来计算持仓保证金占用
            //}

            //其余品种保证金按照结算价格计算
            if (p.oSymbol.Margin <= 1)
                return p.UnsignedSize * (decimal)p.SettlementPrice * p.oSymbol.Multiple * p.oSymbol.Margin;
            else
                return p.oSymbol.Margin * p.UnsignedSize;
        }


        /// <summary>
        /// 获得某个持仓的计算持仓明细
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static IEnumerable<PositionDetail> GetSettlePositionDetals(this Position pos)
        {
            switch (pos.oSymbol.SecurityType)
            { 
                    //股票持仓结算时进行持仓合并
                case SecurityType.STK:
                    return new List<PositionDetail>() { pos.MergePositionDetail() };
                case SecurityType.FUT:
                    return pos.PositionDetailTotal.Where(pd => !pd.IsClosed());
                default:
                    return pos.PositionDetailTotal.Where(pd => !pd.IsClosed());
            }
        }

        /// <summary>
        /// 将某个持仓的持仓明细进行明细合并
        /// 股票结算时 会将当日交易以及未平持仓进行成本合并
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        static PositionDetail MergePositionDetail(this Position pos)
        {
            if (pos.isFlat) return null;
            PositionDetail oldPd = pos.PositionDetailTotal.FirstOrDefault();
            if (oldPd == null) return null;

            PositionDetail pd = new PositionDetailImpl(pos);
            pd.Account = pos.Account;
            pd.oSymbol = pos.oSymbol;
            pd.IsHisPosition = false;//通过成交生成的开仓明细均为日内持仓

            pd.OpenDate = Util.ToTLDate();
            pd.OpenTime = Util.ToTLTime();

            //pos.LastSettlementPrice = this.LastSettlementPrice != null ? (decimal)this.LastSettlementPrice : f.xPrice;//新开仓设定昨日结算价
            //pd.Settleday = f.SettleDay;//持仓明细对应的结算日与成交记录的结算日一致
            pd.Side = pos.isLong;
            pd.Volume = pos.UnsignedSize;
            pd.OpenPrice = pos.AvgPrice;//持仓合并明细时 将当前持仓均价设为OpenPrice
            pd.TradeID = "settle-merge";
            pd.HedgeFlag = "";

            //成交数据会传递Broker字段,用于记录该成交是哪个成交接口回报的，对应开仓时,我们需要标记该持仓明细数序那个成交接口
            pd.Broker = oldPd.Broker;
            pd.Breed = oldPd.Breed;
            return pd;
        }


        /// <summary>
        /// 计算持仓开仓均价
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static decimal CalcAvgOpenPrice(this Position pos)
        {
            if (pos.isFlat) return 0;
            return pos.PositionDetailTotal.Where(pd => !pd.IsClosed()).Sum(pd => pd.OpenPrice * pd.Volume) / pos.UnsignedSize;
        }


        #region 计算平仓盈亏与浮动盈亏
        /// <summary>
        /// 计算平仓盈亏
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static decimal CalcRealizedPL(this Position p)
        {
            return p.ClosedPL * p.oSymbol.Multiple;
        }

        /// <summary>
        /// 计算浮动盈亏
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static decimal CalcUnRealizedPL(this Position p)
        {
            return p.UnRealizedPL * p.oSymbol.Multiple;
        }
        #endregion


        #region 计算持仓成本/市值
        /// <summary>
        /// 计算持仓成本
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static decimal CalcPositionCostValue(this Position p)
        {
            return p.UnsignedSize * p.AvgPrice * p.oSymbol.Multiple;
        }

        /// <summary>
        /// 计算持仓市值
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static decimal CalcPositionMarketValue(this Position p)
        {
            return p.UnsignedSize * p.LastPrice * p.oSymbol.Multiple;
        }
        #endregion


        /// <summary>
        /// 获得持仓内所有成交手续费
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static decimal CalCommission(this Position pos)
        {
            return pos.Trades.Sum(fill => fill.GetCommission());
        }



        #region 计算盯市/逐笔 平仓盈亏与浮动盈亏
        /// <summary>
        /// 累加所有持仓明细的逐日平仓盈亏
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static decimal CalCloseProfitByDate(this Position pos)
        {
            return pos.PositionDetailTotal.Sum(pd => pd.CloseProfitByDate);
        }

        /// <summary>
        /// 累加所有持仓明细的逐笔平仓盈亏
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static decimal CalCloseProfitByTrade(this Position pos)
        {
            return pos.PositionDetailTotal.Sum(pd => pd.CloseProfitByTrade);
        }

        /// <summary>
        /// 计算盯市浮动盈亏
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static decimal CalPositionProfitByDate(this Position pos)
        {
            return pos.PositionDetailTotal.Sum(pd => pd.PositionProfitByDate);
        }

        /// <summary>
        /// 计算逐笔浮动盈亏
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static decimal CalPositionProfitByTrade(this Position pos)
        {
            return pos.PositionDetailTotal.Sum(pd => pd.PositionProfitByTrade);
        }
        #endregion


        #region 计算持仓买入/卖出金额
        /// <summary>
        /// 计算持仓对应的买入金额
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static decimal CalcBuyAmount(this Position pos)
        {
            return pos.Trades.Where(t => t.Side).Sum(t => t.GetAmount());
        }

        /// <summary>
        /// 计算持仓对应的卖出金额
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static decimal CalcSellAmount(this Position pos)
        {
            return pos.Trades.Where(t => !t.Side).Sum(t => t.GetAmount());
        }
        #endregion

        /// <summary>
        /// 生成PositionEx用于通知客户端
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static PositionEx GenPositionEx(this Position pos)
        {
            PositionEx p = new PositionEx();
            p.Account = pos.Account;
            p.Symbol = pos.Symbol;
            p.Exchange = pos.oSymbol.Exchange;
            p.Multiple = pos.oSymbol.Multiple;
            
            p.AvgPrice = pos.AvgPrice;//持仓均价

            if (pos.DirectionType == QSEnumPositionDirectionType.Long)
            {
                p.Side = true;
            }
            else if(pos.DirectionType == QSEnumPositionDirectionType.Short)
            {
                p.Side = false;
            }

            p.DirectionType = pos.DirectionType;

            /*
             *  开仓金额=今日开仓数量 * 开仓价 * 合约乘数	                        针对当前交易日的所有开仓
             * */
            p.OpenAmount = pos.OpenAmount;
            p.OpenVolume = pos.OpenVolume;
            
            //平仓金额=平仓数量 * 平仓价 * 合约乘数                                针对当前交易日的所有平仓
            p.CloseAmount = pos.CloseAmount;
            p.CloseVolume = pos.CloseVolume;
            

            //持仓成本
            /* 
             持仓成本	上日持仓 * 昨结算价 * 合约乘数 + SUM（今日持仓 * 开仓价 * 合约乘数）
             持仓均价	持仓成本/总持仓/合约乘数 这里价格采用的持仓价，昨仓取的是昨日结算价
             * */
            p.PositionCost = pos.PositionDetailTotal.Where(pd => !pd.IsClosed()).Sum(pd => pd.CostPrice() * pd.Volume * p.Multiple);
            
            //开仓成本
            /*
                开仓成本	（上日持仓 + 今日持仓）* 开仓价 * 合约乘数	等于逐笔持仓成本
                开仓均价	开仓成本/总持仓/合约乘数	
                指当前持仓对应的开仓成本 平掉的持仓不用计算入内 这里价格采用的是开仓价
            **/
            p.OpenCost = pos.PositionDetailTotal.Where(pd=>!pd.IsClosed()).Sum(pd => pd.OpenPrice * pd.Volume * p.Multiple);
            

            /*
                平仓盈亏	 按照不同的算法 计算出当日的平仓盈亏
               "SUM（平昨量 *（平仓价 - 昨结算价）* 合约乘数）+SUM（平今量 *（平仓价 - 开仓价）* 合约乘数） -- 多头
                SUM（平昨量 *（昨结算价 - 平仓价）* 合约乘数）+SUM（平今量 *（开仓价 - 平仓价）* 合约乘数） -- 空头"	切过第二天后，平仓盈亏原值保留
             * */
            p.ClosePL = pos.ClosedPL;//点数
            p.CloseProfit = pos.ClosedPL * p.Multiple;//盈亏金额

            p.UnRealizedPL = pos.UnRealizedPL;
            p.UnRealizedProfit = p.UnRealizedPL * p.Multiple;


            //总持仓数量 总的有效数量
            p.Position = pos.UnsignedSize;
            //今仓 有效数量 当日平仓会改变该数值
            p.TodayPosition = pos.PositionDetailTodayNew.Where(pd => !pd.IsClosed()).Sum(pd => pd.Volume);//当日新开仓 持仓数量
            //昨仓 是初始状态的昨日持仓数量 平仓后 不改变该数值
            p.YdPosition = pos.PositionDetailYdRef.Where(pd => !pd.IsClosed()).Sum(pd => pd.Volume);//昨仓数量


            //保证金
            p.Margin = pos.CalcPositionMargin();

            //持仓成交的手续费 累加所有成交的手续费
            p.Commission = pos.CalCommission();

            
            p.CloseProfitByDate = pos.CalCloseProfitByDate();
            p.CloseProfitByTrade = pos.CalCloseProfitByTrade();

            p.LastSettlementPrice = (pos.LastSettlementPrice != null ? (decimal)pos.LastSettlementPrice : 0);//获得的是持仓对象Position的昨日结算价格 这个价格是从行情产生的
            p.SettlementPrice = pos.LastPrice;// (pos.SettlementPrice != null ? (decimal)pos.SettlementPrice : 0);



            p.Tradingday=0;
            return p;
        }
    }

}
