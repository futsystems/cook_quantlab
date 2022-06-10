using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;

namespace TradingLib.Common
{
    public class ThreadTracker
    {
        static ConcurrentDictionary<int, Thread> _threadmap = new ConcurrentDictionary<int, Thread>();
        public static void Register(Thread t)
        {
            if (!_threadmap.Keys.Contains(t.ManagedThreadId))
            {
                _threadmap[t.ManagedThreadId] = t;
            }
        }

        public static Thread[] GetAllThreads()
        {
            return _threadmap.Values.ToArray();
        }

        public static void Unregister(Thread t)
        { 
            Thread remove = null;
            _threadmap.TryRemove(t.ManagedThreadId, out remove);
        }
        public static string PrintThreads()
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append("---------Thread List-------------" + "\r\n");

            foreach (Thread t in _threadmap.Values)
            {

                sb.Append("Thread#" + i.ToString() + " name:" + t.Name + " live:" + t.IsAlive.ToString() + " background:" + t.IsBackground.ToString() + " id:" + t.ManagedThreadId + " priority:" + t.Priority + " status:" + t.ThreadState.ToString()+"\r\n");
                i++;
            }

            return sb.ToString();
        }
    }

}
