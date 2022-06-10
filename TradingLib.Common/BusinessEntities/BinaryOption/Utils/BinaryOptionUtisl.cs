using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public static class BinaryOptionUtisl
    {
        /// <summary>
        /// 是否过期
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public static bool IsExpired(this BinaryOption bo,long now)
        {
            return now > bo.ExpireTime;
        }


    }
}
