using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface IAccountCheck:IRule
    {
        //检查某个委托是否有效
        bool CheckAccount(out string msg);
    }
}
