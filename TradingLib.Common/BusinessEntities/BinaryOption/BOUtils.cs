using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public static class BOUtils
    {

        /// <summary>
        /// 是否已平权
        /// </summary>
        public static bool IsExit(this BinaryOptionOrder order)
        {
            return order.Status == EnumBOOrderStatus.Exit;
        }

        public static bool IsEntry(this BinaryOptionOrder order)
        {
            return order.Status == EnumBOOrderStatus.Entry;
        }

        /// <summary>
        /// 判定二元期权委托是否处于Open状态
        /// 只有正常退出Exit或Reject为关闭状态其余均为Open状态
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static bool IsOpen(this BinaryOptionOrder order)
        {
            switch (order.Status)
            {
                case EnumBOOrderStatus.Exit:
                case EnumBOOrderStatus.Reject:
                    return false;
                default:
                    return true;
            }
        }

        public static bool IsClose(this BinaryOptionOrder order)
        {
            switch (order.Status)
            { 
                case EnumBOOrderStatus.Exit:
                case EnumBOOrderStatus.Reject:
                    return true;
                default:
                    return true;
            }
        }
        /// <summary>
        /// 判定二元期权胜负结果
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        //public static bool AdjustBinaryOptionOrder(BinaryOptionOrder o)
        //{ 
            
        //}
    }
}
