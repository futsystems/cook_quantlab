using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    public class BOOrderTracker:GenericTrackerI, IEnumerable<BinaryOptionOrder>
    {
        GenericTracker<BinaryOptionOrder> orders = new GenericTracker<BinaryOptionOrder>();
        GenericTracker<decimal> sent = new GenericTracker<decimal>();//记录订单金额

        public void Clear()
        {
            orders.Clear();
            sent.Clear();
            
        }
        public string Display(string txt) { return string.Empty; }
        public string Display(int idx) { return this[idx].ToString(); }
        public string getlabel(int idx) { return orders.getlabel(idx); }
        public int Count { get { return orders.Count; } }
        public decimal ValueDecimal(string txt) { int idx = getindex(txt); if (idx < 0) return 0; return this[idx].Amount; }
        public decimal ValueDecimal(int idx) { return this[idx].Amount; }
        public object Value(string txt) { return ValueDecimal(txt); }
        public object Value(int idx) { return ValueDecimal(idx); }
        public int getindex(string txt) { return orders.getindex(txt); }
        public Type TrackedType { get { return typeof(BinaryOptionOrder); } }

        string _name = string.Empty;
        public string Name { get { return _name; } set { _name = value; } }
        public int addindex(string txt)
        {
            return orders.addindex(txt);
        }


        /// <summary>
        /// 返回某个Order未成交数量(记录序号)
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public BinaryOptionOrder this[int idx]
        {
            get
            {
                return orders[idx];
            }
        }


        public BOOrderTracker()
        {
            orders.NewTxt += new TextIdxDelegate(orders_NewTxt);
        }

        public event TextIdxDelegate NewTxt;

        void orders_NewTxt(string txt, int idx)
        {
            decimal amount = orders[idx].Amount;
            sent.addindex(txt, amount);
            if (NewTxt != null)
                NewTxt(txt, idx);
        }


        public BinaryOptionOrder SentOrder(long id)
        {
            if (orders.getindex(id.ToString()) != GenericTracker.UNKNOWN)
                return orders[id.ToString()];
            else
                return null;
        }


        public void GotOrder(BinaryOptionOrder o)
        {
            if (o.ID == 0)
            {
                return;
            }
            int idx = sent.getindex(o.ID.ToString());
            if (idx < 0)//记录器没有记录该委托
            {
                idx = orders.addindex(o.ID.ToString(), o);
            }
            else
            {
                
                BinaryOptionOrder to = orders[idx];
                BinaryOptionOrderImpl.Copy(o, to);               
            }
        }


        /// <summary>
        /// 返回所有委托数组
        /// </summary>
        /// <returns></returns>
        public BinaryOptionOrder[] ToArray()
        {
            return orders.ToArray();
        }


        /// <summary>
        /// 查看某个Order是否被维护跟踪
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsTracked(long id)
        {
            int idx = sent.getindex(id.ToString());
            return idx != GenericTracker.UNKNOWN;
        }


        /// <summary>
        /// 开权状态委托
        /// </summary>
        public IEnumerable<BinaryOptionOrder> EentyOrders
        {
            get
            {
                return this.Where(o => o.IsEntry());
            }
        }

        /// <summary>
        /// 平权状态委托
        /// </summary>
        public IEnumerable<BinaryOptionOrder> ExitOrders
        {
            get
            {
                return this.Where(o => o.IsExit());
            }
        }


        /// <summary>
        /// get orders from tracker
        /// 这里默认对已结算的委托进行过滤
        /// </summary>
        /// <returns></returns>
        public IEnumerator<BinaryOptionOrder> GetEnumerator()
        {
            return orders.Where(o => !o.Settled && (!string.IsNullOrEmpty(o.Account)) && o.oSymbol != null).GetEnumerator();

        }

        IEnumerator<BinaryOptionOrder> IEnumerable<BinaryOptionOrder>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
