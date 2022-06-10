using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 将Frequency适配成ISeries接口用于绘图
    /// </summary>
    internal sealed class FrequencyBarElementSeries : ISeries
    {
        private readonly Frequency _frequency;

        private readonly QSEnumBarElement _barElement;

        internal Frequency Frequency
        {
            get
            {
                return this._frequency;
            }
        }

        public double Current
        {
            get
            {
                return this.LookBack(0);
            }
        }

        public int Count
        {
            get
            {
                return this._frequency.Bars.Count;
            }
        }

        public bool OldValuesChange
        {
            get
            {
                return false;
            }
        }

        public int OldestValueChanged
        {
            get
            {
                return 0;
            }
        }

        //public SeriesChartSettings ChartSettings
        //{
        //    get
        //    {
        //        throw new System.NotSupportedException();
        //    }
        //    set
        //    {
        //        throw new System.NotSupportedException();
        //    }
        //}

        public FrequencyBarElementSeries(Frequency frequency, QSEnumBarElement barElement)
        {
            this._frequency = frequency;
            this._barElement = barElement;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nBars"></param>
        /// <returns></returns>
        public double LookBack(int nBars)
        {
            return BarUtils.GetValueForBarElement(this._frequency.Bars.LookBack(nBars), this._barElement);
        }
    }
}
