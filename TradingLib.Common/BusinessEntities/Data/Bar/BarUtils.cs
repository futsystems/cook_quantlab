using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{

    public static class Bar_Util
    {
        /// <summary>
        /// 获得时间Key
        /// </summary>
        /// <param name="bar"></param>
        /// <returns></returns>
        public static long GetTimeKey(this Bar bar)
        { 
                switch (bar.IntervalType)
                { 
                    case BarInterval.CustomTime:
                        return bar.EndTime.ToTLDateTime();//日内数据以对应的Bar结束时间为Key
                    case BarInterval.Day:
                        return Util.ToTLDateTime(bar.TradingDay, 0);//日线数据以对应的交易日时间为Key
                    default:
                        return bar.EndTime.ToTLDateTime();
                }
        }
            
        
        
        
    }
}
