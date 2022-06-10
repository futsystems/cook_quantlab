using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    /// <summary>
    /// Constants that define a price type within a list of bars.
    /// </summary>
    public enum QSEnumBarElement
    {
        /// <summary>
        /// Open price for this bar
        /// </summary>
        Open,
        /// <summary>
        /// High price for this bar
        /// </summary>
        High,
        /// <summary>
        /// Low price for this bar
        /// </summary>
        Low,
        /// <summary>
        /// Close price for this bar
        /// </summary>
        Close,
        /// <summary>
        /// Volume figure for this bar
        /// </summary>
        Volume,
        /// <summary>
        /// Last Bid value for this bar
        /// </summary>
        Bid,
        /// <summary>
        /// Last Ask value for this bar
        /// </summary>
        Ask,
        /// <summary>
        /// Retrieves the date for this bar.
        /// </summary>
        /// <remarks>
        /// The date is represented as a decimal and
        /// can be converted into a DateTime structure
        /// using DateTime.FromOADate()
        /// </remarks>
        BarDate,
        /// <summary>
        /// Retrieves the open interest value for this bar.
        /// </summary>
        OpenInterest
    }
}
