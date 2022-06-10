using System;
using System.Collections.Generic;
using System.Text;
using TradingLib.API;
namespace TradingLib.Common
{
    /// <summary>
    /// bar的生成引擎接口,设定了bar生成的具体方式,是按照时间,tick数,还是成交量或者自定义的其他方式
    /// </summary>
    public interface IntervalData
    {
        int Last();
        int Count();
        void Reset();
        bool isRecentNew();
        List<decimal> open();
        List<decimal> close();
        List<decimal> high();
        List<decimal> low();
        List<long> vol();
        List<int> date();
        List<int> time();
        List<double> oaDateTime();
        event SymBarIntervalDelegate NewBar;
        Bar GetBar(int index, string symbol);
        Bar GetBar(string symbol);
        void newPoint(string symbol, decimal p, int time, int date, int size);
        void newTick(Tick k);
        void addbar(Bar b);
    }
}
