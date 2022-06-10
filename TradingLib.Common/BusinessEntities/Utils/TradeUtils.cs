using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public static class TradeUtil
    {

        public static long GetDateTime(this Trade f)
        {
            return Util.ToTLDateTime(f.xDate, f.xTime);
        }

        /// <summary>
        /// 获得某个成交的手续费
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static decimal GetCommission(this Trade f)
        {
            decimal fee = f.Commission + f.TransferFee + f.StampTax;
            return fee;
        }


        /// <summary>
        /// 返回某个成交的成交金额
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static decimal GetAmount(this Trade f)
        {
            decimal multiple = f.oSymbol == null ? 1 : f.oSymbol.Multiple;
            return  f.xPrice * Math.Abs(f.xSize) * multiple;
        }

        /// <summary>
        /// 获得成交字符串用于保存到文本
        /// </summary>
        /// <param name="f"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string GetTradStr(this Trade f, string delimiter=",")
        {
            string[] trade = new string[] { f.xDate.ToString(), f.xTime.ToString(), f.Symbol, (f.Side ? "BUY" : "SELL"), f.UnsignedSize.ToString(), f.xPrice.ToFormatStr(f.oSymbol)};
            return string.Join(delimiter,trade);
        }

        public static string GetTradeDetail(this Trade f)
        {
            //Util.Debug("TradeDetail~~~~~~~~~~~~ 00 f==null" + (f == null).ToString());
            StringBuilder sb = new StringBuilder();
            sb.Append(f.Account + " " + f.Symbol + " ");
            sb.Append(" T:" + f.GetDateTime().ToString());
            sb.Append(" " + f.OffsetFlag.ToString());
            sb.Append(f.Side ? " BOT" : " SOD");
            sb.Append(" " + Math.Abs(f.xSize).ToString());
            sb.Append("@" + f.xPrice.ToFormatStr(f.oSymbol));
            sb.Append(" C:" + f.Commission.ToString());
            sb.Append(string.Format("Broker:{0} BLocalID:{1} BRemoteID:{2} TradeID:{3} OrderSysID:{4} Breed:{5}", f.Broker, f.BrokerLocalOrderID, f.BrokerRemoteOrderID, f.TradeID, f.OrderSysID, f.Breed));
            //sb.Append(" R:" + f.Broker + "/" + f.TradeID);
            return sb.ToString();
        }

        public static string GetTradeInfo(this Trade f)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(f.Side ? "BOT" : "SOD");
            sb.Append(" "+f.OffsetFlag.ToString());
            sb.Append(" " + Math.Abs(f.xSize).ToString());
            sb.Append(" " + f.Symbol);
            sb.Append("  @" + f.xPrice.ToFormatStr(f.oSymbol));
            sb.Append(" C:"+f.Commission.ToString());
            sb.Append(" R:" + f.Broker+"/"+f.TradeID);

            return sb.ToString();
        }
    }
}
