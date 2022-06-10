using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public interface IFrequencyGenerator
    {

        // Events
        event Action<NewTickEventArgs> NewTickEvent;
        event Action<SingleBarEventArgs> NewBarEvent;
        /// <summary>
        /// 初始化数据发生器
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="barConstruction"></param>
        void Initialize(Symbol symbol, BarConstructionType barConstruction);

        /// <summary>
        /// 处理Tick数据
        /// </summary>
        /// <param name="k"></param>
        void ProcessTick(Tick k);

        /// <summary>
        /// 处理Bar数据
        /// </summary>
        /// <param name="bar"></param>
        void ProcessBar(Bar bar);

        DateTime NextTimeUpdateNeeded { get; }
    }
}
