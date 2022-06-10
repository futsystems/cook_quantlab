using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    /// <summary>
    /// A thread-safe IEnumerable implementation
    /// See: http://www.codeproject.com/KB/cs/safe_enumerable.aspx
    /// </summary>
    public class SafeEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _inner;
        private readonly object _lock;

        public SafeEnumerable(IEnumerable<T> inner, object @lock)
        {
            _lock = @lock;
            _inner = inner;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SafeEnumerator<T>(_inner.GetEnumerator(), _lock);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
