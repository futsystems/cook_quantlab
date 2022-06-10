//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using TradingLib.API;

//namespace TradingLib.Common
//{
//    public static class VendorUtils
//    {

//         <summary>
//         获得实盘帐户对象的成交通道标识
//         </summary>
//         <param name="v"></param>
//         <returns></returns>
//        public static string GetBrokerToken(this Vendor v)
//        {
//            if (v.Broker == null)
//                return string.Empty;
//            return v.Broker.Token;
//        }

//         <summary>
//         判断某个实盘帐户是否可用
//         </summary>
//         <param name="v"></param>
//         <returns></returns>
//        public static bool AcceptEntryOrder(this Vendor v, Order o, decimal margintouse)
//        {
//            posincrement = o.UnsignedSize;
//            if (v.Broker == null) return false;
//            获得通道下完该委托后预期持仓变动
//            int increment = v.Broker.GetPositionAdjustment(o);
//            posincrement = increment;

//            decimal marginwilluse = margintouse / o.UnsignedSize * increment;//按比例计算实际使用的保证金
//            这里需要考虑到净持仓,如果可以进行净持仓操作,则需要按规则下到净持仓里面,而不受保证金占用
//            decimal marginused = v.CalMargin() + v.CalMarginFrozen();//计算当前使用保证金

//            Util.Info(string.Format("Vendor:{0} MarginUsedNow:{1} OrderMargin:{2} PositionIncrement:{3} MarginWillUse:{4}", v.Name, marginused, margintouse, increment, marginwilluse), "VendorUtils");
//            if (increment <= 0) return true;

//            当前已经使用的保证金 + 即将使用的保证金 需要小于我们设定的保证金限额
//            if (v.MarginLimit > 1)
//            {
//                return marginused + marginwilluse < v.MarginLimit;
//            }
//            else
//            {
//                return true;//marginused + marginwilluse/;
//            }
//        }

//         <summary>
//         判断Vendoer的底层通道是否正常可用
//         </summary>
//         <param name="v"></param>
//         <returns></returns>
//        public static bool IsBrokerAvabile(this Vendor v)
//        {
//            if (v.Broker != null && v.Broker.IsLive)
//            {
//                return true;
//            }
//            return false;
//        }

//        public static decimal CalMargin(this Vendor v)
//        {
//            if (v.Broker != null)
//                return v.Broker.CalFutMargin();
//            return 0;
//        }
//        public static decimal CalMarginFrozen(this Vendor v)
//        {
//            if (v.Broker != null)
//                return v.Broker.CalFutMarginFrozen();
//            return 0;
//        }

//        public static decimal CalRealizedPL(this Vendor v)
//        {
//            if (v.Broker != null)
//                return v.Broker.CalFutRealizedPL();
//            return 0;
//        }

//        public static decimal CalUnRealizedPL(this Vendor v)
//        {
//            if (v.Broker != null)
//                return v.Broker.CalFutUnRealizedPL();
//            return 0;
//        }
//    }
//}
