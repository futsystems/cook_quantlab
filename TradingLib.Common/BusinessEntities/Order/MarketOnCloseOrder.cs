using System;
using System.Collections.Generic;
using TradingLib.API;
using System.Text;

namespace TradingLib.Common
{
    /// <summary>
    /// market on close order
    /// </summary>
    public class MOCOrder : OrderImpl
    {
        public MOCOrder(string symbol, bool side, int size)
            : base(symbol, side, System.Math.Abs(size))
        {
            this.TimeInForce = QSEnumTimeInForce.MOC;
            this.Exchange = "NYSE";
        }
    }

}
