using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    internal class PrioritySortedQueue<T, C> : SortedQueue<PriorityItemPair<T, C>> where C : System.IComparable
    {
        //[System.Runtime.CompilerServices.CompilerGenerated]
        //private static System.Comparison<PriorityItemPair<T, C>> x31af784cbc72c68d;

        //public PrioritySortedQueue()
        //{
        //    if (x80a4d735376284c8<T, C>.x31af784cbc72c68d == null)
        //    {
        //        x80a4d735376284c8<T, C>.x31af784cbc72c68d = new System.Comparison<PriorityItemPair<T, C>>(x80a4d735376284c8<T, C>.PriorityComparer);
        //    }
        //    base..ctor(x80a4d735376284c8<T, C>.x31af784cbc72c68d);
        //}
        public PrioritySortedQueue():
            base(new Comparison<PriorityItemPair<T,C>>(PrioritySortedQueue<T, C>.PriorityComparer))
        { 
            
        }

        /// <summary>
        /// 入队列
        /// </summary>
        /// <param name="item"></param>
        /// <param name="priority"></param>
        public void Enqueue(T item, C priority)
        {
            base.Enqueue(new PriorityItemPair<T, C>(item, priority));
        }

        /// <summary>
        /// 出队列
        /// </summary>
        /// <returns></returns>
        public new T Dequeue()
        {
            return base.Dequeue().Item;
        }

        private static int PriorityComparer(PriorityItemPair<T, C> item1, PriorityItemPair<T, C> item2)
        {
            C priority = item1.Priority;
            return priority.CompareTo(item2.Priority);
        }
    }

    public class SortedQueue<T>
    {
        private int _count;

        private int _capacity;

        private int _x272bc993a9d89cb6;

        private T[] _items;

        private System.Comparison<T> _comparer;

        public int Count
        {
            get
            {
                return this._count;
            }
        }

        public SortedQueue(System.Comparison<T> comparer)
        {
            this._comparer = comparer;
            this._capacity = 15;
            this._items = new T[this._capacity];
        }

        public T[] Items
        {
            get
            {
                return this._items;
            }
        }

        public T Peek()
        {
            if (this._count == 0)
            {
                throw new System.InvalidOperationException();
            }
            return this._items[0];
        }

        /// <summary>
        /// 出队列
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            if (this._count == 0)
            {
                throw new System.InvalidOperationException();
            }
            T result = this._items[0];
            this._count--;
            this.Delete(0, this._items[this._count]);
            this._items[this._count] = default(T);
            this._x272bc993a9d89cb6++;
            return result;
        }

        /// <summary>
        /// 压入队列
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {
            //如果当前对象数量等于总容量则对数据结构进行扩容
            if (this._count == this._capacity)
            {
                this.ExpandCapacity();
            }
            this._count++;
            this.Add(this._count - 1, item);
            this._x272bc993a9d89cb6++;
        }
        /// <summary>
        /// 在某个位置插入数据对象
        /// 会按照从小到大进行排列,如果插入的数据最大，则放在最末尾
        /// </summary>
        /// <param name="xc0c4c459c6ccbd00"></param>
        /// <param name="item"></param>
        private void Add(int index, T item)
        {
            int num = this.CenterIndex(index);//获得当前位置的二分位
            //如果位置大于0 并且 比较位的数据项大于当前数据项 则将比较位的数据放到当前插入位
            while (index > 0 && this._comparer(this._items[num], item) > 0)
            {
                this._items[index] = this._items[num];//当前位置的数据设置为num出的数据对象
                index = num;//index为当前num
                num = this.CenterIndex(index);
            }
            this._items[index] = item;
        }

        private int DoubleIndex(int xc0c4c459c6ccbd00)
        {
            return xc0c4c459c6ccbd00 * 2 + 1;
        }

        private int CenterIndex(int xc0c4c459c6ccbd00)
        {
            return (xc0c4c459c6ccbd00 - 1) / 2;
        }

        /// <summary>
        /// 数组扩容
        /// </summary>
        private void ExpandCapacity()
        {
            this._capacity = this._capacity * 2 + 1;
            T[] array = new T[this._capacity];
            System.Array.Copy(this._items, 0, array, 0, this._count);
            this._items = array;
        }

        /// <summary>
        /// 删除某个数据项
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        private void Delete(int index, T item)
        {
            for (int i = this.DoubleIndex(index); i < this._count; i = this.DoubleIndex(index))
            {
                //如果i位置大于下一位数据 则i递增
                if (i + 1 < this._count && this._comparer(this._items[i], this._items[i + 1]) > 0)
                {
                    i++;
                }
                //index处的数据项 设置为i处的数据项
                this._items[index] = this._items[i];
                index = i;
            }
            this.Add(index, item);
        }
    }

    internal struct PriorityItemPair<T, C> where C : System.IComparable
    {
        private T _item;

        private C _priority;

        public T Item
        {
            get
            {
                return this._item;
            }
        }

        public C Priority
        {
            get
            {
                return this._priority;
            }
        }

        public PriorityItemPair(T item, C priority)
        {
            this._item = item;
            this._priority = priority;
        }

        public void Clear()
        {
            this._item = default(T);
            this._priority = default(C);
        }
    }
}
