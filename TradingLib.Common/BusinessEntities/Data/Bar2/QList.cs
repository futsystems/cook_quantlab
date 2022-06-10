using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    public  class QList<T>
    {
        /// <summary>
        /// 内部List类
        /// </summary>
        private class InnerListClass : System.Collections.IEnumerable, System.Collections.Generic.IEnumerable<T>, System.Collections.Generic.IList<T>, System.Collections.Generic.ICollection<T>
        {
            private QList<T> _RList;

            private bool _fromSystemStart;

            private Dequeue<T> _queue
            {
                get
                {
                    return this._RList._queue;
                }
            }

            public T this[int index]
            {
                get
                {
                    int num = this.ToQueueIndex(index);
                    if (num < 0)
                    {
                        int num2 = 0;
                        int num3 = this.Count - 1;
                        if (this._fromSystemStart)
                        {
                            num2 = this.ToListIndex(this._queue.Count - 1);
                        }
                        throw new System.IndexOutOfRangeException(string.Concat(new object[]
						{
							"The index must be between ",
							num2,
							" and ",
							num3
						}));
                    }
                    return this._queue[num];
                }
                set
                {
                    throw new System.NotSupportedException();
                }
            }

            public int Count
            {
                get
                {
                    if (this._fromSystemStart)
                    {
                        return this._RList.TotalCount;
                    }
                    return this._queue.Count;
                }
            }

            /// <summary>
            /// 是否是只读QList
            /// </summary>
            public bool IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            public InnerListClass(QList<T> rlist, bool fromSystemStart)
            {
                this._RList = rlist;
                this._fromSystemStart = fromSystemStart;
            }

            private int ToQueueIndex(int listIndex)
            {
                if (listIndex < 0)
                {
                    return -1;
                }
                if (this._fromSystemStart)
                {
                    int num = this._RList.TotalCount - listIndex - 1;
                    if (num < this._queue.Count)
                    {
                        return num;
                    }
                    return -1;
                }
                else
                {
                    if (listIndex < this._queue.Count)
                    {
                        return this._queue.Count - listIndex - 1;
                    }
                    return -1;
                }
            }

            private int ToListIndex(int queueIndex)
            {
                if (queueIndex < 0 || queueIndex >= this._queue.Count)
                {
                    return -1;
                }
                if (this._fromSystemStart)
                {
                    return this._RList.TotalCount - queueIndex - 1;
                }
                return this._queue.Count - queueIndex - 1;
            }

            public int IndexOf(T item)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (object.Equals(item, this[i]))
                    {
                        return i;
                    }
                }
                return -1;
            }

            /// <summary>
            /// 在某个位置插入一个数据项
            /// </summary>
            /// <param name="index"></param>
            /// <param name="item"></param>
            public void Insert(int index, T item)
            {
                throw new System.NotSupportedException();
            }

            /// <summary>
            /// 在某个位置删除一个数据项
            /// </summary>
            /// <param name="index"></param>
            public void RemoveAt(int index)
            {
                throw new System.NotSupportedException();
            }

            /// <summary>
            /// 添加一个数据项
            /// </summary>
            /// <param name="item"></param>
            public void Add(T item)
            {
                throw new System.NotSupportedException();
            }

            /// <summary>
            /// 清空数据项
            /// </summary>
            public void Clear()
            {
                throw new System.NotSupportedException();
            }

            /// <summary>
            /// 删除某个数据项
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public bool Remove(T item)
            {
                throw new System.NotSupportedException();
            }



            /// <summary>
            /// 判断是否包含了某个数据项
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public bool Contains(T item)
            {
                return this.IndexOf(item) >= 0;
            }

            /// <summary>
            /// 将数据复制到目标数组
            /// </summary>
            /// <param name="array"></param>
            /// <param name="arrayIndex"></param>
            public void CopyTo(T[] array, int arrayIndex)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    array[arrayIndex + i] = this[i];
                }
            }



            public System.Collections.Generic.IEnumerator<T> GetEnumerator()
            {
                ulong version = this._queue.Version;
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    if (version != this._queue.Version)
                    {
                        throw new System.InvalidOperationException("Collection was changed.");
                    }
                    yield return this[i];
                }
                yield break;
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private Dequeue<T> _queue;

        private QList<T>.InnerListClass _accessibleItems;

        private int _maxLookBack;

        private int _totalCount;

        private QList<T> _writeableList;

        private bool _hasPartialItem;

        private T _partialItem;

        /// <summary>
        /// Indicates whether the list is read only
        /// 如果WriteableList不为空 则表明QList本身是只读，写操作需要对writeablelist进行
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return this._writeableList != null;
            }
        }

        /// <summary>
        /// The maximum number of items that the list will keep in memory
        /// 设定最大数据项数
        /// </summary>
        public int MaxLookBack
        {
            get
            {
                if (this.IsReadOnly)
                {
                    return this._writeableList.MaxLookBack;
                }
                return this._maxLookBack;
            }
            set
            {
                if (this.IsReadOnly)
                {
                    this._writeableList.MaxLookBack = value;
                    return;
                }
                this._maxLookBack = value;
                this.Trim();
            }
        }

        /// <summary>
        /// The items in the list as an <see cref="T:System.Collections.Generic.IList`1" />.  In the returned list, the items are ordered with the oldest at index 0.
        /// IList<T> 通过InnerList对QList进行接口封装 实现IList访问
        /// </summary>
        public System.Collections.Generic.IList<T> Items
        {
            get
            {
                if (this._accessibleItems == null)
                {
                    this._accessibleItems = new QList<T>.InnerListClass(this, false);
                }
                return this._accessibleItems;
            }
        }

        /// <summary>
        /// Returns the total number of bars.
        /// 返回数据个数
        /// </summary>
        /// <remarks>The <see cref="P:RightEdge.Common.RList`1.TotalCount" /> refers to the total number of items added to the list.  If the
        /// <see cref="P:RightEdge.Common.RList`1.MaxLookBack" /> is enabled, they may not all be accessible.</remarks>
        public int TotalCount
        {
            get
            {
                if (this.IsReadOnly)
                {
                    return this._writeableList.TotalCount;
                }
                return this._totalCount;
            }
        }

        /// <summary>
        /// The number of elements currently in the list.
        /// 返回在List中的数据个数
        /// </summary>
        public int Count
        {
            get
            {
                return this._queue.Count;
            }
        }

        /// <summary>
        /// Gets the actual maximum current lookback, taking into account the size of the list and the MaxLookBack property.
        /// 当前最大回溯数,如果设定了最大回溯数量 则返回最大回溯数与当前个数对应回溯数的最小值
        /// </summary>
        private int MaxCurrentLookBack
        {
            get
            {
                if (this.MaxLookBack > 0)
                {
                    return System.Math.Min(this.MaxLookBack, this.Count - 1);
                }
                return this.Count - 1;
            }
        }

        /// <summary>
        /// The current value of the series.  Using this property is the same as calling LookBack(0).
        /// 当前值 最近的一个数据项
        /// </summary>
        public T Current
        {
            get
            {
                return this.LookBack(0);
            }
            set
            {
                this.EnsureWriteable();
                this._queue[0] = value;
            }
        }

        /// <summary>
        /// If true, then <see cref="P:RightEdge.Common.RList`1.PartialItem" /> is the value of the series for the current partial bar, which may change before the bar closes
        /// PartialItem意为 部分数据 没有被关闭的Bar数据
        /// 设置是否有PartialItem
        /// </summary>
        public bool HasPartialItem
        {
            get
            {
                if (this.IsReadOnly)
                {
                    return this._writeableList.HasPartialItem;
                }
                return this._hasPartialItem;
            }
            private set
            {
                this.EnsureWriteable();
                this._hasPartialItem = value;
                //如果没有PartialItem则将partialItem置空
                if (!this._hasPartialItem)
                {
                    this._partialItem = default(T);
                }
            }
        }

        /// <summary>
        /// If <see cref="P:RightEdge.Common.RList`1.HasPartialItem" /> is true, then this property is the value of the series for the current partial bar, which may change before the bar closes
        /// </summary>
        public T PartialItem
        {
            get
            {
                if (!this.HasPartialItem)
                {
                    return default(T);
                }
                if (this.IsReadOnly)
                {
                    return this._writeableList.PartialItem;
                }
                return this._partialItem;
            }
            set
            {
                //如果设定PartialItem则自动将HasPartialItem设置为True
                this.EnsureWriteable();
                this.HasPartialItem = true;
                this._partialItem = value;
            }
        }

        /// <summary>
        /// Constructs an empty RList.
        /// </summary>
        public QList()
        {
            this._queue = new Dequeue<T>();
        }

        /// <summary>
        /// Constructs an RList with the specified items.
        /// </summary>
        /// <param name="items">A list of items to add to the RList.</param>
        /// <remarks>
        /// The last item in the supplied list will be the current item of the RList.
        /// </remarks>
        public QList(System.Collections.Generic.IList<T> items)
            : this()
        {
            foreach (T current in items)
            {
                this.Add(current);
            }
        }

        /// <summary>
        /// 生成只读QList
        /// </summary>
        /// <param name="writeableList"></param>
        private QList(QList<T> writeableList)
        {
            this._writeableList = writeableList;
            this._queue = writeableList._queue;
        }

        /// <summary>
        /// 写操作前确认List是否可写,只读List抛出异常
        /// </summary>
        private void EnsureWriteable()
        {
            if (this.IsReadOnly)
            {
                throw new System.InvalidOperationException("Cannot modify a read only RList.");
            }
        }

        /// <summary>
        /// Returns the element in the series from the specified number of bars ago.  LookBack(0) is the current value,
        /// LookBack(1) is the previous bar's value, etc.
        /// </summary>
        /// <param name="nBars">The number of bars to look back.</param>
        /// <returns>The element from the specified number of bars ago.</returns>
        public T LookBack(int nBars)
        {
            if (nBars < 0 || nBars > this.MaxCurrentLookBack)
            {
                throw new System.ArgumentOutOfRangeException("nBars", string.Concat(new object[]
				{
					"Value must be between 0 and ",
					this.MaxCurrentLookBack,
					", value was ",
					nBars
				}));
            }
            //queue先进去的数据 索引为大
            return this._queue[nBars];
        }

        

        /// <summary>
        /// Sets a value that has been previously set.
        /// 设定某个数据项
        /// </summary>
        /// <param name="lookBack">number of bars to look back.</param>
        /// <param name="value">The new value to set.</param>
        public void SetValue(int lookBack, T value)
        {
            this.EnsureWriteable();
            if (lookBack < 0 || lookBack > this.MaxCurrentLookBack)
            {
                throw new System.ArgumentOutOfRangeException("lookBack", string.Concat(new object[]
				{
					"Value must be between 0 and ",
					this.MaxCurrentLookBack,
					", value was ",
					lookBack
				}));
            }
            this._queue[lookBack] = value;
        }

        /// <summary>
        /// Sets <see cref="P:RightEdge.Common.RList`1.HasPartialItem" /> to false
        /// 清空PartialItem
        /// </summary>
        public void ClearPartialItem()
        {
            this.HasPartialItem = false;
        }

        /// <summary>
        /// Adds an item to the series.
        /// </summary>
        /// <param name="item">The item to add to the series.</param>
        /// <remarks>
        /// If there is a current partial item (<see cref="P:RightEdge.Common.RList`1.HasPartialItem" /> is true), it will be removed.
        /// 添加某个数据项 添加数据项时会清空PartialItem 添加时默认添加的数据项时最新数据
        /// </remarks>
        public void Add(T item)
        {
            this.EnsureWriteable();
            this._queue.EnqueueHead(item);
            this._totalCount++;
            this.HasPartialItem = false;
            this.Trim();
        }

        /// <summary>
        /// 按设定MaxLookBack将多余的数据清除
        /// </summary>
        private void Trim()
        {
            if (this.MaxLookBack > 0)
            {
                while (this._queue.Count > this.MaxLookBack + 1)
                {
                    this._queue.DequeueTail();
                }
            }
        }

        /// <summary>
        /// Removes all items from the series.
        /// 清空所有数据
        /// </summary>
        public void Clear()
        {
            this.EnsureWriteable();
            this._queue.Clear();
            this._totalCount = 0;
            this.HasPartialItem = false;
        }

        /// <summary>
        /// Returns a readonly version of this RList.
        /// 返回只读队列，如果本身就是只读则获得引用，如果本身是可写队列则生成一个只读队列
        /// </summary>
        /// <returns>A readonly version of this RList.</returns>
        /// <remarks>If this RList is already readonly, this method will return a reference to this RList instead of creating a new one.</remarks>
        public QList<T> AsReadOnly()
        {
            if (this.IsReadOnly)
            {
                return this;
            }
            return new QList<T>(this);
        }
    }
}
