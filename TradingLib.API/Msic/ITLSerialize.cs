using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface ITLSerialize<T>
    {
        string Serialize(T obj);

        T Deserialize(string content);
    }
}
