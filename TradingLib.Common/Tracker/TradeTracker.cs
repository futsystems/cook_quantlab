using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class TradeTracker: IEnumerable<Trade>
    {
        GenericTracker<Trade> trades = new GenericTracker<Trade>();//记录委托

        public void Clear()
        {
            trades.Clear();
        }
        public TradeTracker()
        {
            trades.NewTxt += new TextIdxDelegate(trades_NewTxt);
        }

        public event TextIdxDelegate NewTxt;

        void trades_NewTxt(string txt, int idx)
        {
            if (NewTxt != null)
                NewTxt(txt, idx);
        }
        void debug(string msg)
        { 
            
        }
        public void GotFill(Trade f)
        {
            if (string.IsNullOrEmpty(f.TradeID))
            {
                debug("TradeTracker can not tracker trade withoud tradeid");
            }

            int idx = trades.getindex(f.TradeID);
            if (idx < 0)//记录器没有记录该成交
            {
                idx = trades.addindex(f.TradeID, f);
            }
        }

        

        /// <summary>
        /// get orders from tracker
        /// 这里默认对已结算的成交进行过滤
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Trade> GetEnumerator()
        {
            return trades.Where(f => !f.Settled && f.oSymbol!= null).GetEnumerator();

        }

        IEnumerator<Trade> IEnumerable<Trade>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
