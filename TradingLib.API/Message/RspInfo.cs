using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface RspInfo
    {
        int ErrorID { get; set; }

        string ErrorMessage { get; set; }
    }
}
