using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TradingLib.API
{
    public interface IOrderCheck:IRule
    {
        //检查某个委托是否有效
        bool checkOrder(Order o,out string msg);
    }
}
