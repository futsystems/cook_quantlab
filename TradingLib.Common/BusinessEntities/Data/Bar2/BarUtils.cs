using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    /// <summary>
    /// A set of utility functions (typically static) that are aimed to aid in extracting values or prices between sets of bars or values.
    /// </summary>
    public static class BarUtils
    {
        public static BarFrequency GetBarFrequency(this Bar bar)
        {
            return new BarFrequency(bar.IntervalType, bar.Interval);
        }

        public static SymbolFreq GetSymbolFreq(this Bar bar)
        {
            return new SymbolFreq(bar.Symbol, new BarFrequency(bar.IntervalType, bar.Interval));
        }

        private static Func<QSEnumBarElement, string> x31af784cbc72c68d;

        /// <summary>
        /// Gets the <see cref="T:RightEdge.Common.BarConstructionType" /> associated with this symbol.
        /// </summary>
        /// <param name="symbol">The <see cref="T:RightEdge.Common.Symbol" /> object to query.</param>
        /// <param name="barConstruction">Default bar construction type.</param>
        /// <returns><see cref="T:RightEdge.Common.BarConstructionType" /> associated with the input symbol.</returns>
        /// <remarks>
        /// This version finds the desired <see cref="T:RightEdge.Common.BarConstructionType" /> setting based on
        /// the symbol's AssetClass.  If the type is <see cref="F:RightEdge.Common.AssetClass.Forex" /> and 
        /// the <see cref="T:RightEdge.Common.BarConstructionType" /> is <see cref="F:RightEdge.Common.BarConstructionType.Default" />
        /// the default behavior is to assign the type to <see cref="F:RightEdge.Common.BarConstructionType.Mid" />.  In the
        /// case of other asset classes, the default type would be set to <see cref="F:RightEdge.Common.BarConstructionType.Trades" />
        /// 获得某个合约Bar生成类别 依据中间价,或者成交价格
        /// </remarks>
        public static BarConstructionType GetBarConstruction(Symbol symbol, BarConstructionType barConstruction)
        {
            BarConstructionType barConstructionType = barConstruction;
            if (barConstructionType == BarConstructionType.Default)
            {
                if (symbol.SecurityType == SecurityType.FOX)
                {
                    barConstructionType = BarConstructionType.Mid;
                }
                else
                {
                    barConstructionType = BarConstructionType.Trade;
                }
            }
            return barConstructionType;
        }

        /// <summary>
        /// Gets a single value for the specified bar from the specified series.
        /// </summary>
        /// <param name="bar">The bar in which to analyze.</param>
        /// <param name="barElement">The portion of the bar to derive the input data from.</param>
        /// <returns>The value for the specified bar element.</returns>
        public static double GetValueForBarElement(Bar bar, QSEnumBarElement barElement)
        {
            switch (barElement)
            {
                case QSEnumBarElement.Open:
                    return bar.Open;
                case QSEnumBarElement.High:
                    return bar.High;
                case QSEnumBarElement.Low:
                    return bar.Low;
                case QSEnumBarElement.Close:
                    return bar.Close;
                case QSEnumBarElement.Volume:
                    return bar.Volume;
                case QSEnumBarElement.Bid:
                    return bar.Bid;
                case QSEnumBarElement.Ask:
                    return bar.Ask;
                case QSEnumBarElement.BarDate:
                    return bar.EndTime.ToOADate();
                case QSEnumBarElement.OpenInterest:
                    return (double)bar.OpenInterest;
                default:
                    throw new ArgumentException("Invalid bar element: " + barElement);
            }
        }

        /// <summary>
        /// Returns a bar with OHLC values corresponding to the bid price.
        /// </summary>
        /// <param name="bar">A bar where the <see cref="P:RightEdge.Common.BarData.Bid" /> corresponds to the bid price at close.</param>
        /// <returns>A bar with OHLC values corresponding to the bid price.</returns>
        /// <remarks>The difference between <see cref="P:RightEdge.Common.BarData.Bid" /> and <see cref="P:RightEdge.Common.BarData.Close" /> in <paramref name="bar" />
        /// is used to calculate the adjusted OHLC values for the returned bar.
        /// </remarks>
        public static Bar GetBidBar(Bar bar)
        {
            if (bar.Bid == 0.0 || bar.Ask == 0.0 || bar.Bid == bar.Close)
            {
                return bar;
            }
            Bar barData = bar.Clone();
            if (double.IsNaN(barData.Bid))
            {
                barData.Bid = barData.Close;
                return barData;
            }
            double num = bar.Close - bar.Bid;
            barData.Open -= num;
            barData.High -= num;
            barData.Low -= num;
            barData.Close -= num;
            return barData;
        }

        /// <summary>
        /// Returns a bar with OHLC values corresponding to the ask price.
        /// </summary>
        /// <param name="bar">A bar where the <see cref="P:RightEdge.Common.BarData.Ask" /> corresponds to the ask price at close.</param>
        /// <returns>A bar with OHLC values corresponding to the ask price.</returns>
        /// <remarks>The difference between <see cref="P:RightEdge.Common.BarData.Ask" /> and <see cref="P:RightEdge.Common.BarData.Close" /> in <paramref name="bar" />
        /// is used to calculate the adjusted OHLC values for the returned bar.
        /// </remarks>
        public static Bar GetAskBar(Bar bar)
        {
            if (bar.Bid == 0.0 || bar.Ask == 0.0 || bar.Ask == bar.Close)
            {
                return bar;
            }
            Bar barData = bar.Clone();
            if (double.IsNaN(barData.Ask))
            {
                barData.Ask = bar.Close;
                return barData;
            }
            double num = bar.Close - bar.Ask;
            barData.Open -= num;
            barData.High -= num;
            barData.Low -= num;
            barData.Close -= num;
            return barData;
        }

        /// <summary>
        /// Calculates the timestamp for a tick inside a bar.
        /// </summary>
        /// <param name="barStartTime">The bar start time</param>
        /// <param name="barEndTime">The bar end time</param>
        /// <param name="ratio">How far through the bar the returned time should be.  Zero means the start of the bar, One means just before the bar end time.</param>
        /// <returns>A time greater than or equal to the bar start time and strictly less than the bar end time.</returns>
        public static System.DateTime GetBarTime(System.DateTime barStartTime, System.DateTime barEndTime, double ratio)
        {
            double totalSeconds = (barEndTime - barStartTime).TotalSeconds;
            System.DateTime dateTime = barStartTime + System.TimeSpan.FromSeconds(totalSeconds * ratio);
            System.DateTime dateTime2 = barEndTime.Subtract(System.TimeSpan.FromSeconds(1.0));
            if (dateTime2 < barStartTime)
            {
                dateTime2 = barEndTime.Subtract(System.TimeSpan.FromMilliseconds(1.0));
                if (dateTime2 < barStartTime)
                {
                    dateTime2 = barStartTime + System.TimeSpan.FromSeconds(totalSeconds * 5.0 / 6.0);
                }
            }
            if (dateTime > dateTime2)
            {
                dateTime = dateTime2;
            }
            return dateTime;
        }

        /// <summary>
        /// Checks a bar for errors
        /// </summary>
        /// <param name="bar">Bar to check for errors</param>
        /// <param name="error">If the bar has errors, will contain a description of the error</param>
        /// <returns>True if the bar is valid, false if it has errors</returns>
        public static bool IsValidBar(Bar bar, out string error)
        {
            error = null;
            System.Collections.Generic.List<QSEnumBarElement> list = new System.Collections.Generic.List<QSEnumBarElement>();
            QSEnumBarElement[] array = new QSEnumBarElement[]
			{
				QSEnumBarElement.Open,
				QSEnumBarElement.High,
				QSEnumBarElement.Low,
				QSEnumBarElement.Close
			};
            for (int i = 0; i < array.Length; i++)
            {
                QSEnumBarElement barElement = array[i];
                double valueForBarElement = BarUtils.GetValueForBarElement(bar, barElement);
                if (valueForBarElement == 0.0 || double.IsNaN(valueForBarElement) || double.IsPositiveInfinity(valueForBarElement) || double.IsNegativeInfinity(valueForBarElement) || valueForBarElement == 1.7976931348623157E+308 || valueForBarElement == -1.7976931348623157E+308)
                {
                    list.Add(barElement);
                }
            }
            if (list.Count > 0)
            {
                if (list.Count == 1)
                {
                    error = string.Format("The bar's {0} value is invalid.", list[0].ToString());
                }
                else
                {
                    string arg_EF_0 = "The following values are invalid: {0}";
                    string arg_EA_0 = ", ";
                    System.Collections.Generic.IEnumerable<QSEnumBarElement> arg_E0_0 = list;
                    if (BarUtils.x31af784cbc72c68d == null)
                    {
                        BarUtils.x31af784cbc72c68d = new Func<QSEnumBarElement, string>(BarUtils.xaf4a25e8362b945d);
                    }
                    error = string.Format(arg_EF_0, string.Join(arg_EA_0, arg_E0_0.Select(BarUtils.x31af784cbc72c68d).ToArray<string>()));
                }
            }
            else if (bar.High < bar.Low)
            {
                error = "The low value is greater than the high value.";
            }
            else if (bar.Open < bar.Low)
            {
                error = "The open value is less than the low value.";
            }
            else if (bar.Close < bar.Low)
            {
                error = "The close value is less than the low value.";
            }
            else if (bar.Open > bar.High)
            {
                error = "The open value is greater than the high value.";
            }
            else if (bar.Close > bar.High)
            {
                error = "The close value is greater than the high value.";
            }
            return error == null;
        }

        /// <summary>
        /// Returns the highest value among the list of values.
        /// </summary>
        /// <param name="values">The list of double values to analyze.</param>
        /// <param name="start">The index in the list in which to start the analysis.</param>
        /// <param name="length">The length or number of values to analyze before completing.</param>
        /// <returns>The highest double value in the list.</returns>
        public static double HighestValue(System.Collections.Generic.IList<double> values, int start, int length)
        {
            double num = -1.7976931348623157E+308;
            for (int i = start; i < start + length; i++)
            {
                num = System.Math.Max(values[i], num);
            }
            return num;
        }

        /// <summary>
        /// Returns the highest value among the list of values.
        /// </summary>
        /// <param name="values">The list of double values to analyze.</param>
        /// <returns>The highest double value in the list.</returns>
        public static double HighestValue(System.Collections.Generic.IList<double> values)
        {
            return BarUtils.HighestValue(values, 0, values.Count);
        }

        /// <summary>
        /// Returns the lowest value among the list of values.
        /// </summary>
        /// <param name="values">The list of double values to analyze.</param>
        /// <param name="start">The index in the list in which to start the analysis.</param>
        /// <param name="length">The length or number of values to analyze before completing.</param>
        /// <returns>The lowest double value in the list.</returns>
        public static double LowestValue(System.Collections.Generic.IList<double> values, int start, int length)
        {
            double num = 1.7976931348623157E+308;
            for (int i = start; i < start + length; i++)
            {
                num = System.Math.Min(values[i], num);
            }
            return num;
        }

        /// <summary>
        /// Returns the lowest value among the list of values.
        /// </summary>
        /// <param name="values">The list of double values to analyze.</param>
        /// <returns>The lowest double value in the list.</returns>
        public static double LowestValue(System.Collections.Generic.IList<double> values)
        {
            return BarUtils.LowestValue(values, 0, values.Count);
        }

        /// <summary>
        /// Returns the highest high value within the list of bars.
        /// </summary>
        /// <param name="bars">List containing a series of BarData classes.</param>
        /// <param name="start">The index in the list in which to start the analysis.</param>
        /// <param name="length">The length or number of values to analyze before completing.</param>
        /// <returns>Highest high value as a double.</returns>
        public static double HighestHigh(System.Collections.Generic.IList<Bar> bars, int start, int length)
        {
            double num = -1.7976931348623157E+308;
            for (int i = start; i < start + length; i++)
            {
                if (!bars[i].EmptyBar && bars[i].High > num)
                {
                    num = bars[i].High;
                }
            }
            return num;
        }

        /// <summary>
        /// Returns the highest high value within the list of bars.
        /// </summary>
        /// <param name="bars">List containing a series of BarData classes.</param>
        /// <returns>Highest high value as a double.</returns>
        public static double HighestHigh(System.Collections.Generic.IList<Bar> bars)
        {
            return BarUtils.HighestHigh(bars, 0, bars.Count);
        }

        /// <summary>
        /// Returns the highest high for the specified number of recent bars in a bar series.
        /// </summary>
        /// <param name="bars">RList containing a series of bars.</param>
        /// <param name="count">The number of bars to compute the highest high for.</param>
        /// <returns>The highest high in the bar series over the last number of bars specified by <paramref name="count" />.</returns>
        public static double HighestHigh(QList<Bar> bars, int count)
        {
            double num = -1.7976931348623157E+308;
            for (int i = count - 1; i >= 0; i--)
            {
                Bar barData = bars.LookBack(i);
                if (!barData.EmptyBar)
                {
                    num = System.Math.Max(barData.High, num);
                }
            }
            return num;
        }

        /// <summary>
        /// Returns the highest high value in a series of bars.
        /// </summary>
        /// <param name="bars">RList containing a series of bars.</param>
        /// <returns>The highest high in the series of bars.</returns>
        public static double HighestHigh(QList<Bar> bars)
        {
            return BarUtils.HighestHigh(bars, bars.Count);
        }

        /// <summary>
        /// Returns the lowest low value within the list of bars.
        /// </summary>
        /// <param name="bars">List containing a series of BarData classes.</param>
        /// <param name="start">The index in the list in which to start the analysis.</param>
        /// <param name="length">The length or number of values to analyze before completing.</param>
        /// <returns>Lowest low value as a double.</returns>
        public static double LowestLow(System.Collections.Generic.IList<Bar> bars, int start, int length)
        {
            double num = 1.7976931348623157E+308;
            for (int i = start; i < start + length; i++)
            {
                if (!bars[i].EmptyBar && bars[i].Low < num)
                {
                    num = bars[i].Low;
                }
            }
            return num;
        }

        /// <summary>
        /// Returns the lowest low value within the list of bars.
        /// </summary>
        /// <param name="bars">List containing a series of BarData classes.</param>
        /// <returns>Lowest low value as a double.</returns>
        public static double LowestLow(System.Collections.Generic.IList<Bar> bars)
        {
            return BarUtils.LowestLow(bars, 0, bars.Count);
        }

        /// <summary>
        /// Returns the lowest low for the specified number of recent bars in a bar series.
        /// </summary>
        /// <param name="bars">RList containing a series of bars.</param>
        /// <param name="count">The number of bars to compute the lowest low for.</param>
        /// <returns>The lowest low in the bar series over the last number of bars specified by <paramref name="count" />.</returns>
        public static double LowestLow(QList<Bar> bars, int count)
        {
            double num = 1.7976931348623157E+308;
            for (int i = count - 1; i >= 0; i--)
            {
                Bar barData = bars.LookBack(i);
                if (!barData.EmptyBar)
                {
                    num = System.Math.Min(barData.Low, num);
                }
            }
            return num;
        }

        /// <summary>
        /// Returns the lowest low value in a series of bars.
        /// </summary>
        /// <param name="bars">RList containing a series of bars.</param>
        /// <returns>The lowest low in the series of bars.</returns>
        public static double LowestLow(QList<Bar> bars)
        {
            return BarUtils.LowestLow(bars, bars.Count);
        }

        /// <summary>
        /// Find a bar in an RList based on a date.
        /// </summary>
        /// <param name="bars">RList containing a series of BarData</param>
        /// <param name="date">The date to find the bar for.</param>
        /// <returns>Integer containing the lookback at which the bar was found, or -1 if the bar is not found.</returns>
        public static int BarLookBackFromDate(QList<Bar> bars, System.DateTime date)
        {
            int result = -1;
            if (date == System.DateTime.MinValue)
            {
                return -1;
            }
            int num = 0;
            int i = bars.Count - 1;
            while (i >= num)
            {
                int num2 = (num + i) / 2;
                Bar barData = bars.LookBack(num2);
                if (barData.EndTime == date)
                {
                    result = num2;
                    break;
                }
                if (date > barData.EndTime)
                {
                    i = num2 - 1;
                }
                else
                {
                    num = num2 + 1;
                }
            }
            return result;
        }

        /// <summary>
        /// Retrieves the most recent value of a series.
        /// </summary>
        /// <param name="series">A series to retrieve the most recent value from.</param>
        /// <returns>The most recent value of <paramref name="series" />.</returns>
        public static double LastValueOf(ISeries series)
        {
            if (series.Count == 0)
            {
                return double.NaN;
            }
            return series.LookBack(0);
        }

        /// <summary>
        /// Retrieves the last non-empty bar in a list of bars.
        /// </summary>
        /// <param name="bars">A list of bars.</param>
        /// <returns>The last non-empty bar in <paramref name="bars" />.  If there are no non-empty bars in the list, returns null.</returns>
        public static Bar LastValidBar(QList<Bar> bars)
        {
            for (int i = 0; i < bars.Count; i++)
            {
                Bar barData = bars.LookBack(i);
                if (!barData.EmptyBar)
                {
                    return barData;
                }
            }
            return null;
        }

        /// <summary>
        /// Processes an RList of bars for the specified frequency.
        /// </summary>
        /// <param name="generator">Frequency generator used for processing.</param>
        /// <param name="symbol">Symbol object to process for.</param>
        /// <param name="bars">RList of bars as input.</param>
        /// <param name="lastBarEnd">The final date to assign to bars with no end date specified.</param>
        public static void ProcessRListInBarGenerator(IFrequencyGenerator generator, Symbol symbol, QList<Bar> bars, System.DateTime lastBarEnd)
        {
            //for (int i = bars.Count - 1; i >= 0; i--)
            //{
            //    Bar barData = bars.LookBack(i);
            //    System.DateTime barEndTime;
            //    if (i > 0)
            //    {
            //        barEndTime = bars.LookBack(i - 1).BarStartTime;
            //    }
            //    else if (lastBarEnd != System.DateTime.MinValue)
            //    {
            //        barEndTime = lastBarEnd;
            //    }
            //    else
            //    {
            //        barEndTime = barData.BarStartTime;
            //    }
            //    SingleBarEventArgs args = new SingleBarEventArgs(symbol, barData, barEndTime, false);
            //    generator.ProcessBar(args);
            //}
            //generator.ProcessTick(new TickData
            //{
            //    time = System.DateTime.MaxValue.Subtract(System.TimeSpan.FromDays(365000.0)),
            //    tickType = TickType.CurrentTime
            //});
        }

        /// <summary>
        /// Gets the known, default frequencies supported by RightEdge.
        /// </summary>
        /// <returns>Dictionary list of the frequencies and corresponding names.</returns>
        public static System.Collections.Generic.Dictionary<int, string> GetFrequencies()
        {
            System.Collections.Generic.Dictionary<int, string> dictionary = new System.Collections.Generic.Dictionary<int, string>();
            dictionary[1] = "1 Minute";
            dictionary[5] = "5 Minutes";
            dictionary[15] = "15 Minutes";
            dictionary[30] = "30 Minutes";
            dictionary[60] = "1 Hour";
            dictionary[1440] = "Daily";
            dictionary[10080] = "Weekly";
            dictionary[43200] = "Monthly";
            dictionary[525600] = "Yearly";
            return dictionary;
        }

        /// <summary>
        /// Returns a text representation of a frequency.
        /// </summary>
        /// <param name="frequency">integer containing the frequency number.</param>
        /// <returns>string containing the frequency text.</returns>
        public static string GetFrequencyText(int frequency)
        {
            System.Collections.Generic.Dictionary<int, string> frequencies = BarUtils.GetFrequencies();
            if (frequencies.ContainsKey(frequency))
            {
                return frequencies[frequency];
            }
            return BarUtils.xd88d539647f5ce4c(frequency, false);
        }

        /// <summary>
        /// Returns a shortened text representation of a frequency.
        /// </summary>
        /// <param name="frequency">integer containing the frequency number.</param>
        /// <returns>string containing the frequency text.</returns>
        public static string GetShortFrequencyText(int frequency)
        {
            if (frequency == 1)
            {
                return "1M";
            }
            if (frequency == 5)
            {
                return "5M";
            }
            if (frequency == 10)
            {
                return "10M";
            }
            if (frequency == 15)
            {
                return "15M";
            }
            if (frequency == 30)
            {
                return "30M";
            }
            if (frequency == 60)
            {
                return "1H";
            }
            if (frequency == 1440)
            {
                return "1D";
            }
            if (frequency == 10080)
            {
                return "1W";
            }
            if (frequency == 43200)
            {
                return "1Mo";
            }
            if (frequency == 525600)
            {
                return "1Y";
            }
            return BarUtils.xd88d539647f5ce4c(frequency, true);
        }

        private static string xd88d539647f5ce4c(int x227fefe64408b240, bool x51e7d03c24b4580e)
        {
            if (x227fefe64408b240 % 525600 == 0)
            {
                return (x227fefe64408b240 / 525600).ToString() + (x51e7d03c24b4580e ? "Y" : " Years");
            }
            if (x227fefe64408b240 % 43200 == 0)
            {
                return (x227fefe64408b240 / 43200).ToString() + (x51e7d03c24b4580e ? "Mo" : " Months");
            }
            if (x227fefe64408b240 % 10080 == 0)
            {
                return (x227fefe64408b240 / 10080).ToString() + (x51e7d03c24b4580e ? "W" : " Weeks");
            }
            if (x227fefe64408b240 % 1440 == 0)
            {
                return (x227fefe64408b240 / 1440).ToString() + (x51e7d03c24b4580e ? "D" : " Days");
            }
            if (x227fefe64408b240 % 60 == 0)
            {
                return (x227fefe64408b240 / 60).ToString() + (x51e7d03c24b4580e ? "H" : " Hours");
            }
            if (x227fefe64408b240 % 1 == 0)
            {
                return (x227fefe64408b240 / 1).ToString() + (x51e7d03c24b4580e ? "M" : " Minutes");
            }
            return x227fefe64408b240 + " Minutes";
        }

        /// <summary>
        /// Gets a <see cref="T:System.Globalization.CultureInfo" /> object which can be used to format currency.
        /// </summary>
        /// <param name="currencyType">A string representing the currency, such as USD or GBP</param>
        /// <returns>A CultureInfo object which can be used to format currency.</returns>
        public static System.Globalization.CultureInfo GetCurrencyCulture(string currencyType)
        {
            //ReturnValue<CurrencyType> returnValue = EnumUtil<CurrencyType>.Parse(currencyType);
            //if (returnValue.Success)
            //{
            //    return BarUtils.GetCurrencyCulture(returnValue.Value);
            //}
            //return BarUtils.GetCurrencyCulture(CurrencyType.None);
            return System.Globalization.CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Gets a <see cref="T:System.Globalization.CultureInfo" /> object which can be used to format currency.
        /// </summary>
        /// <param name="currencyType">A CurrencyType to get a formatter for.</param>
        /// <returns>A CultureInfo object which can be used to format currency.</returns>
        public static System.Globalization.CultureInfo GetCurrencyCulture(CurrencyType currencyType)
        {
            switch (currencyType)
            {
                case CurrencyType.USD:
                    return new System.Globalization.CultureInfo("en-US");
                //case CurrencyType.GBP:
                //    return new System.Globalization.CultureInfo("en-GB");
                //case CurrencyType.EUR:
                //    return new System.Globalization.CultureInfo("fr-FR");
                //case CurrencyType.AUD:
                //    return new System.Globalization.CultureInfo("en-AU");
                //case CurrencyType.CAD:
                //    return new System.Globalization.CultureInfo("en-CA");
                //case CurrencyType.NZD:
                //    return new System.Globalization.CultureInfo("en-NZ");
                //case CurrencyType.JPY:
                //    return new System.Globalization.CultureInfo("ja-JP");
                //case CurrencyType.CHF:
                //    return new System.Globalization.CultureInfo("fr-CH");
                //case CurrencyType.BRL:
                //    return new System.Globalization.CultureInfo("pt-BR");
                //case CurrencyType.HKD:
                //    return new System.Globalization.CultureInfo("zh-HK");
                //case CurrencyType.SEK:
                //    return new System.Globalization.CultureInfo("sv-SE");
                //case CurrencyType.NOK:
                //    return new System.Globalization.CultureInfo("no");
                //case CurrencyType.KRW:
                //    return new System.Globalization.CultureInfo("ko-KR");
                //case CurrencyType.SGD:
                //    return new System.Globalization.CultureInfo("zh-SG");
                //case CurrencyType.CNY:
                //    return new System.Globalization.CultureInfo("zh-CN");
                //case CurrencyType.MXN:
                //    return new System.Globalization.CultureInfo("es-MX");
                //case CurrencyType.RUB:
                //    return new System.Globalization.CultureInfo("ru-RU");
                //case CurrencyType.INR:
                //    return new System.Globalization.CultureInfo("ta-IN");
                //case CurrencyType.TRY:
                //    return new System.Globalization.CultureInfo("tr-TR");
                //case CurrencyType.CZK:
                //    return new System.Globalization.CultureInfo("cs-CZ");
                //case CurrencyType.DKK:
                //    return new System.Globalization.CultureInfo("da-DK");
                //case CurrencyType.EEK:
                //    return new System.Globalization.CultureInfo("et-EE");
                //case CurrencyType.HRK:
                //    return new System.Globalization.CultureInfo("hr-HR");
                //case CurrencyType.HUF:
                //    return new System.Globalization.CultureInfo("hu-HU");
                //case CurrencyType.LTL:
                //    return new System.Globalization.CultureInfo("lt-LT");
                //case CurrencyType.LVL:
                //    return new System.Globalization.CultureInfo("lv-LV");
                //case CurrencyType.PLN:
                //    return new System.Globalization.CultureInfo("pl-PL");
                //case CurrencyType.RON:
                //    return new System.Globalization.CultureInfo("ro-RO");
                //case CurrencyType.SIT:
                //    return new System.Globalization.CultureInfo("sl-SI");
                //case CurrencyType.SKK:
                //    return new System.Globalization.CultureInfo("sk-SK");
                //case CurrencyType.TWD:
                //    return new System.Globalization.CultureInfo("zh-TW");
                //case CurrencyType.ISK:
                //    return new System.Globalization.CultureInfo("is");
                //case CurrencyType.MYR:
                //    return new System.Globalization.CultureInfo("ms-MY");
                //case CurrencyType.IDR:
                //    return new System.Globalization.CultureInfo("id");
                //case CurrencyType.PHP:
                //    return new System.Globalization.CultureInfo("en-PH");
                //case CurrencyType.MAD:
                //    return new System.Globalization.CultureInfo("ar-MA");
                //case CurrencyType.THB:
                //    return new System.Globalization.CultureInfo("th-TH");
                //case CurrencyType.COB:
                //    return new System.Globalization.CultureInfo("es-CO");
                //case CurrencyType.CLP:
                //    return new System.Globalization.CultureInfo("es-CL");
                //case CurrencyType.EGP:
                //    return new System.Globalization.CultureInfo("ar-EG");
            }
            return System.Globalization.CultureInfo.CurrentCulture;
        }

        [System.Runtime.CompilerServices.CompilerGenerated]
        private static string xaf4a25e8362b945d(QSEnumBarElement xfbf34718e704c6bc)
        {
            return xfbf34718e704c6bc.ToString();
        }
    }
}
