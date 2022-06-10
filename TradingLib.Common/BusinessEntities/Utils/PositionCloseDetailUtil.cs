using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    public static class PositionCloseDetailUtil
    {


        /// <summary>
        /// 获得文字输出
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string GetPositionCloseStr(this PositionCloseDetail d)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(d.Account + " ");
            sb.Append(d.Symbol + " ");
            sb.Append("Open:" + d.OpenDate.ToString() + " " + d.OpenPrice.ToString() + " ID:" + d.OpenTradeID + " ");
            sb.Append("Close:" + d.CloseDate.ToString() + " " + d.ClosePrice.ToString() + " ID:" + d.CloseTradeID + " ");
            sb.Append(string.Format("{0} {1}手@{2} PreS:{3}", d.Side ? "买平" : "卖平", d.CloseVolume, d.ClosePrice, d.LastSettlementPrice));
            sb.Append(string.Format(" CloseProfit:{0}", d.CloseProfitByDate));
            return sb.ToString();
        }
    }
}
