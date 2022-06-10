using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    /// <summary>
    /// 数据集接口
    /// </summary>
    public interface ISeries
    {
        /// <summary>
        /// The current value of the series.  This property should return the same value as calling LookBack(0).
        /// </summary>
        double Current
        {
            get;
        }

        /// <summary>
        /// The number of values in the series.
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// Indicates whether previous values in the series change when a new value is added.
        /// </summary>
        /// <value>
        /// Indicates whether previous values in the series change when a new value is added.
        /// </value>
        /// <remarks>
        /// Usually, in a series that is related to bar data, a new value will be calculated
        /// and added to the end of the series when a new bar comes in.  The previous values
        /// of the series will not change.  Return true for this property to indicate that
        /// this is not the case.  If this returns true, then the <see cref="P:RightEdge.Common.ISeries.OldestValueChanged" />
        /// property should reflect what values changed.
        /// </remarks>
        bool OldValuesChange
        {
            get;
        }

        /// <summary>
        /// The lookback index of the oldest value that changed.  Should be zero unless <see cref="P:RightEdge.Common.ISeries.OldValuesChange" /> is true.
        /// </summary>
        /// <value>
        /// The lookback index of the oldest value that changed.  Should be zero unless <see cref="P:RightEdge.Common.ISeries.OldValuesChange" /> is true.
        /// </value>
        int OldestValueChanged
        {
            get;
        }

        /// <summary>
        /// Contains settings for how the series should be displayed on a chart.
        /// </summary>
        //SeriesChartSettings ChartSettings
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Looks back within the series for the specified value.  A value of 0 represents the most recent value.
        /// </summary>
        /// <param name="nBars">Number of bars to lookback in the series.</param>
        /// <returns>The value contained at the specified index.</returns>
        /// <remarks>
        /// A series is a collection of values.  The values within series can be referenced using the Lookback method.
        /// Use 0 to get the most recent value in the series.  To reference values further back in time, pass a value
        /// greater than 0.  For example, if the series is created against daily bars, to reference yesterday's value
        /// use Lookback(1).  To reference the day before yesterday, use Lookback(2) and so on.  Use the
        /// <see cref="P:RightEdge.Common.ISeries.Count">Count</see> method to determine how many values are contained
        /// within the series.
        /// </remarks>
        double LookBack(int nBars);

        ///// <summary>
        ///// 数据序列个数
        ///// </summary>
        //int Count { get; }
        ///// <summary>
        ///// 最新的数据
        ///// </summary>
        //double Last { get; }
        ///// <summary>
        ///// 最老的数据
        ///// </summary>
        //double First { get; }
        ///// <summary>
        ///// 回溯多少个单位后的数据
        ///// </summary>
        ///// <param name="n"></param>
        ///// <returns></returns>
        //double LookBack(int n);
        ///// <summary>
        ///// 获得index位置处的数据
        ///// </summary>
        ///// <param name="index"></param>
        ///// <returns></returns>
        //double this[int index] { get; }

        ///// <summary>
        ///// 返回double类型的数组 供计算器计算使用
        ///// </summary>
        //double[] Data { get; }

        //ISeriesChartSettings ChartSettings { get; set; }
    }
}
