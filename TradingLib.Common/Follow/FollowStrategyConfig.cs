using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    
    /// <summary>
    /// 跟单策略参数
    /// </summary>
    public class FollowStrategyConfig
    {
        /// <summary>
        /// 数据库全局ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 分区编号
        /// </summary>
        public int Domain_ID { get; set; }

        /// <summary>
        /// 策略名称/编号
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 策略描述
        /// </summary>
        public string Desp { get; set; }

        /// <summary>
        /// 跟单方向
        /// </summary>
        public QSEnumFollowDirection FollowDirection { get; set; }

        /// <summary>
        /// 跟单倍率
        /// </summary>
        public int FollowPower { get; set; }

        /// <summary>
        /// 跟单帐号 绑定该账户 策略所有委托从该账户发出
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 开仓跟单价格类型
        /// </summary>
        public QSEnumFollowPriceType EntryPriceType {get;set;}

        /// <summary>
        /// 开仓偏移价格点数
        /// 正数：买入 以更低的价格买入，卖出 以更高的价格卖出
        /// </summary>
        public int EntryOffsetTicks { get; set; }

         /// <summary>
        /// 开仓挂单阀值类型
        /// </summary>
        public QSEnumPendingThresholdType EntryPendingThresholdType { get; set; }

        /// <summary>
        /// 开仓挂单阀值
        /// </summary>
        public int EntryPendingThresholdValue { get; set; }

        /// <summary>
        /// 开仓挂单延迟处理方式
        /// </summary>
        public QSEnumPendingOperationType EntryPendingOperationType { get; set; }

        /// <summary>
        /// 平仓跟单价格类型
        /// </summary>
        public QSEnumFollowPriceType ExitPriceType {get;set;}

        /// <summary>
        /// 平仓便宜价格点数
        /// </summary>
        public int ExitOffsetTicks { get; set; }

        /// <summary>
        /// 平仓挂单阀值类型
        /// </summary>
        public QSEnumPendingThresholdType ExitPendingThreadsholdType { get; set; }
        
        /// <summary>
        /// 平仓挂单阀值
        /// </summary>
        public int ExitPendingThresholdValue { get; set; }

        /// <summary>
        /// 平仓挂单延迟处理方式
        /// </summary>
        public QSEnumPendingOperationType ExitPendingOperationType { get; set; }

        string _secFilterStr = string.Empty;
        List<string> _secList = new List<string>();
        /// <summary>
        /// 品种过滤列表
        /// </summary>
        public string SecFilter { 
            get
            {
                return _secFilterStr;
            }
            set{
                _secList.Clear();
                _secFilterStr = value;
                if (string.IsNullOrEmpty(_secFilterStr)) return;
                _secList.AddRange(_secFilterStr.Split(','));
            }
        }

        /// <summary>
        /// 是否接受某个品种
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public bool ValidSecFilter(string sec)
        {
            if (_secList.Count == 0) return true;
            return _secList.Contains(sec);
        }


        string _timeFilterStr = string.Empty;
        List<RuleTimeSpan> _tslist = new List<RuleTimeSpan>();
        /// <summary>
        /// 时间段过滤列表
        /// </summary>
        public string TimeFilter {
            get { return _timeFilterStr; }
            set
            {
                _tslist.Clear();
                _timeFilterStr = value;
                if (string.IsNullOrEmpty(_timeFilterStr)) return;
                foreach (var str in _timeFilterStr.Split(','))
                {
                    RuleTimeSpan ts = RuleTimeSpan.Deserialize(str);
                    if (ts != null)
                    {
                        _tslist.Add(ts);
                    }
                }
            }
        }

        /// <summary>
        /// 检查是否在设定的时间区间内
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool ValidTimeFilter(int time)
        {
            if (_tslist.Count == 0) return true;
            foreach (var tmp in _tslist)
            {
                if (tmp.InSpan(time))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 信号最大手数过滤
        /// </summary>
        public int SizeFilter { get; set; }

        /// <summary>
        /// 止损启用
        /// </summary>
        public bool StopEnable { get; set; }

        /// <summary>
        /// 止损值
        /// </summary>
        public decimal StopValue { get; set; }

        /// <summary>
        /// 止损值类型
        /// </summary>
        public QSEnumFollowProtectValueType StopValueType { get; set; }

        /// <summary>
        /// 止盈1启用
        /// </summary>
        public bool Profit1Enable { get; set; }

        /// <summary>
        /// 止盈1值
        /// </summary>
        public decimal Profit1Value { get; set; }

        /// <summary>
        /// 止盈1值类型
        /// </summary>
        public QSEnumFollowProtectValueType Profit1ValueType { get; set; }

        /// <summary>
        /// 止盈2启用
        /// </summary>
        public bool Profit2Enable { get; set; }

        /// <summary>
        /// 止盈2值1
        /// </summary>
        public decimal Profit2Value1 { get; set; }

        /// <summary>
        /// 止盈2跟踪1
        /// </summary>
        public decimal Profit2Trailing1 { get; set; }

        /// <summary>
        /// 止盈2值类型
        /// </summary>
        public QSEnumFollowProtectValueType Profit2Value1Type { get; set; }

        /// <summary>
        /// 止盈2值2
        /// </summary>
        public decimal Profit2Value2 { get; set; }

        /// <summary>
        /// 止盈2跟踪2
        /// </summary>
        public decimal Profit2Trailing2 { get; set; }

        /// <summary>
        /// 止盈2值2类型
        /// </summary>
        public QSEnumFollowProtectValueType Profit2Value2Type { get; set; }


    }

    
}
