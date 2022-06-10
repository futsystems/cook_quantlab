using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TradingLib.API
{
    public interface Exchange
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        string EXCode { get; set; }
        /// <summary>
        /// 交易所名称
        /// </summary>
        string Name {get;set;}
        /// <summary>
        /// 所在国家
        /// </summary>
        Country Country {get;set; }

        /// <summary>
        /// 交易所对应的时区信息
        /// </summary>
        string TimeZoneID { get; set; }

        /// <summary>
        /// 获取交易所当前时间
        /// </summary>
        /// <returns></returns>
        DateTime GetExchangeTime();

        /// <summary>
        /// 将系统时间转换成交易所时间
        /// </summary>
        /// <param name="systime"></param>
        /// <returns></returns>
        DateTime ConvertToExchangeTime(DateTime systime);

        /// <summary>
        /// 将交易所时间转换成系统时间
        /// </summary>
        /// <param name="extime"></param>
        /// <returns></returns>
        DateTime ConvertToSystemTime(DateTime extime);
        
        /// <summary>
        /// 收盘时间
        /// </summary>
        int CloseTime { get; set; }
        /// <summary>
        /// 交易日历
        /// </summary>
        string Calendar { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// 行情源
        /// </summary>
        QSEnumDataFeedTypes DataFeed { get; set; }

        /// <summary>
        /// 结算方式
        /// </summary>
        QSEnumSettleType SettleType { get; set; }
    }
}
