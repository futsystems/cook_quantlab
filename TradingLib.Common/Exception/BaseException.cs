using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 异常基类
    /// </summary>
    public class TLException:Exception
    {
        public TLException(string message)
            :base(message)
        {
        
        }

        public TLException(string message, Exception innerException)
            :base(message,innerException)
        { 
        
        }

        



    }
}
