using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{

    public static class PositionDetailUtil
    {

        /// <summary>
        /// 获得开仓时间
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static long GetDateTime(this PositionDetail pos)
        {
            return Util.ToTLDateTime(pos.OpenDate, pos.OpenTime);
        }


        /// <summary>
        /// 该持仓是否已经被关闭
        /// 如果开仓量等于平仓量则该持仓关闭
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static bool IsClosed(this PositionDetail pos)
        {
            if (pos.Volume==0)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 持仓成本
        /// 国内期货 今仓的持仓成本为 当日开仓价格 昨仓的平仓成本为 结算价格
        /// 昨仓/今仓判断
        /// 1.在交易日内由开仓成交生成的持仓明细为今仓
        /// 2.从数据库加载的持仓明细为昨仓
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static decimal CostPrice(this PositionDetail pos)
        {
            //按交易所结算方式设定来返回成本价
            switch (pos.oSymbol.SecurityFamily.Exchange.SettleType)
            { 
                    //逐日结算 昨仓为昨日结算价 今仓为开仓价
                case QSEnumSettleType.ByDate:
                    return pos.IsHisPosition ? pos.LastSettlementPrice : pos.OpenPrice;
                    //逐笔结算 为开仓价
                case QSEnumSettleType.ByTrade:
                    return pos.OpenPrice;
                default:
                    return pos.OpenPrice;
            }
        }

        /// <summary>
        /// 获得文字输出
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static string GetPositionDetailStr(this PositionDetail pos)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0}-{1} [{2}]",pos.Account,pos.Symbol,pos.Broker));
            sb.Append(" ");
            sb.Append(" T:" + pos.GetDateTime().ToString());
            sb.Append(" S:" + (pos.Side ? "Long" : "Short"));
            sb.Append(string.Format(" {0}@{1}", pos.Volume, pos.OpenPrice));
            sb.Append(" HoldSize:" + pos.Volume +" TotalSize:"+(pos.Volume+pos.CloseVolume).ToString());
            sb.Append(" TradeID:" + pos.TradeID);
            sb.Append(string.Format(" PreS:{0} S:{1}", pos.LastSettlementPrice, pos.SettlementPrice));
            sb.Append(string.Format(" PL:{0} UnPL:{1}", pos.CloseProfitByDate, pos.PositionProfitByDate));
            sb.Append(string.Format(" His:{0}", pos.IsHisPosition ? "YdPos" : "TdPos"));
            return sb.ToString();
        }


        



    }



}
