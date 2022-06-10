using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public static class RouterItemUtils
    {
        /// <summary>
        /// 获得路由项目的Broker标识
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetBrokerToken(this RouterItem item)
        {
            if (item.Broker == null)
                return string.Empty;
            return item.Broker.Token;
        }

        /// <summary>
        /// 判断路由项目的底层通道是否正常可用
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool IsBrokerAvabile(this RouterItem item)
        {
            if (item.Broker != null && item.Broker.IsLive)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断某个实盘帐户是否可用
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool AcceptEntryOrder(this RouterItem item, Order o, decimal margintouse)
        {
            //posincrement = o.UnsignedSize;
            if (item.Broker == null) return false;
            //获得通道下完该委托后预期持仓变动
            int increment = item.Broker.GetPositionAdjustment(o);
            //posincrement = increment;

            decimal marginwilluse = margintouse / o.UnsignedSize * increment;//按比例计算实际使用的保证金
            //这里需要考虑到净持仓,如果可以进行净持仓操作,则需要按规则下到净持仓里面,而不受保证金占用
            decimal marginused = item.CalMargin() + item.CalMarginFrozen();//计算当前使用保证金
            
            //Console.WriteLine(string.Format("Broker:{0} MarginUsedNow:{1} OrderMargin:{2} PositionIncrement:{3} MarginWillUse:{4}", item.Broker.Token, marginused, margintouse, increment, marginwilluse), "VendorUtils");
            if (increment <= 0) return true;

            //当前已经使用的保证金 + 即将使用的保证金 需要小于我们设定的保证金限额
            if (item.MarginLimit > 0)
            {
                return marginused + marginwilluse < item.MarginLimit;
            }
            else
            {
                return true;
            }
        }


        public static decimal CalMargin(this RouterItem item)
        {
            if (item.Broker != null)
                return item.Broker.CalFutMargin();
            return 0;
        }
        public static decimal CalMarginFrozen(this RouterItem item)
        {
            if (item.Broker != null)
                return item.Broker.CalFutMarginFrozen();
            return 0;
        }

        public static decimal CalRealizedPL(this RouterItem item)
        {
            if (item.Broker != null)
                return item.Broker.CalFutRealizedPL();
            return 0;
        }

        public static decimal CalUnRealizedPL(this RouterItem item)
        {
            if (item.Broker != null)
                return item.Broker.CalFutUnRealizedPL();
            return 0;
        }
    }
}
