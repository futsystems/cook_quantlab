using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{

    /// <summary>
    /// 前后都可以操作的队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Dequeue<T> : IEnumerable, IEnumerable<T>
    {
        internal class DequeueEnumerator : IEnumerator, System.IDisposable, IEnumerator<T>
        {
            private Dequeue<T> x804ea29e9a51f50f;

            private int x12cb12b5d2cad53d;

            private int x9fd888e65466818c;

            private int x42d1ff243acb2246;

            private ulong x77fa6322561797a0;

            private T x3bd62873fafa6252;

            public T Current
            {
                get
                {
                    if (this.x42d1ff243acb2246 < this.x12cb12b5d2cad53d || this.x42d1ff243acb2246 > this.x9fd888e65466818c)
                    {
                        throw new System.InvalidOperationException("Enumerator not valid!");
                    }
                    return this.x3bd62873fafa6252;
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

            public DequeueEnumerator(Dequeue<T> D, int Start, int End)
            {
                this.x804ea29e9a51f50f = D;
                this.x77fa6322561797a0 = this.x804ea29e9a51f50f.Version;
                this.x12cb12b5d2cad53d = Start;
                this.x9fd888e65466818c = End;
                this.x42d1ff243acb2246 = Start - 1;
            }

            public void Reset()
            {
                this.x42d1ff243acb2246 = this.x12cb12b5d2cad53d - 1;
                if (this.x77fa6322561797a0 != this.x804ea29e9a51f50f.Version)
                {
                    throw new System.InvalidOperationException("Collection was changed!");
                }
            }

            public bool MoveNext()
            {
                if (this.x77fa6322561797a0 != this.x804ea29e9a51f50f.Version)
                {
                    throw new System.InvalidOperationException("Collection was changed!");
                }
                this.x42d1ff243acb2246++;
                if (this.x42d1ff243acb2246 > this.x9fd888e65466818c)
                {
                    return false;
                }
                this.x3bd62873fafa6252 = this.x804ea29e9a51f50f[this.x42d1ff243acb2246];
                return true;
            }

            public void Dispose()
            {
            }
        }

        protected T[] InnerList;

        protected double growthFactor;

        protected int Head;

        protected int Tail;

        protected int count;

        protected ulong version;

        /// <summary>
        /// Indexed access to all elements currently in the collection.
        /// Indexing starts at 0 (head) and ends at Count-1 (tail).
        /// </summary>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                {
                    throw new System.ArgumentOutOfRangeException("index");
                }
                return this.InnerList[(this.Head + index) % this.Capacity];
            }
            set
            {
                if (index < 0 || index >= this.Count)
                {
                    throw new System.ArgumentOutOfRangeException("index");
                }
                this.InnerList[(this.Head + index) % this.Capacity] = value;
                this.version += 1uL;
            }
        }

        /// <summary>
        /// The current factor by which to grow the collection in case of expansion
        /// </summary>
        public double GrowthFactor
        {
            get
            {
                return this.growthFactor;
            }
            set
            {
                this.growthFactor = value;
            }
        }

        /// <summary>
        /// The current amount of cells available to the dequeue
        /// </summary>
        public int Capacity
        {
            get
            {
                return this.InnerList.Length;
            }
            set
            {
                if (this.Capacity >= this.Count)
                {
                    this.SetSize(this.Capacity);
                    return;
                }
                throw new System.ArgumentException("Capacity was smaller than Count!");
            }
        }

        /// <summary>
        /// The current version of the dequeue. The version is increased with every changing operation.
        /// The main use is to invalidate all IEnumerators.
        /// </summary>
        public ulong Version
        {
            get
            {
                return this.version;
            }
        }

        /// <summary>
        /// The current number of elements in the queue
        /// </summary>
        public int Count
        {
            get
            {
                return this.count;
            }
        }

        /// <summary>
        /// Create an empty Dequeu with capacity 32 and growth 2
        /// </summary>
        public Dequeue()
            : this(32, 2.0)
        {
        }

        /// <summary>
        /// Create an empty Dequeu with given capacity and growth 2
        /// </summary>
        /// <param name="Capacity">the initial capacity of the collection</param>
        public Dequeue(int Capacity)
            : this(Capacity, 2.0)
        {
        }

        /// <summary>
        /// Create an empty Dequeu with given capacity and given growth
        /// </summary>
        /// <param name="Capacity">the initial capacity of the collection</param>
        /// <param name="GrowthFactor">the factor by which to grow the collection when the capacity is reached</param>
        public Dequeue(int Capacity, double GrowthFactor)
        {
            this.InnerList = new T[Capacity];
            this.GrowthFactor = GrowthFactor;
            this.Head = (this.Tail = (this.count = 0));
            this.version = 0uL;
        }

        /// <summary>
        /// Create a new Dequeu as a copy of the given collection
        /// </summary>
        /// <param name="C">The source collection</param>
        public Dequeue(System.Collections.Generic.ICollection<T> C)
            : this(C, C.Count)
        {
        }

        /// <summary>
        /// Create a new Dequeu as a copy of the given collection and the given capacity
        /// </summary>
        /// <param name="C">The source collection</param>
        /// <param name="Capacity">The capacity of the new Dequeue (must be &gt;= C.Count)</param>
        public Dequeue(System.Collections.Generic.ICollection<T> C, int Capacity)
            : this(Capacity, 2.0)
        {
            this.EnqueueTailRange(C);
        }

        /// <summary>
        /// Add the given object to the collections head
        /// 从头部添加一条数据
        /// </summary>
        /// <param name="value">The object to enqueue</param>
        public void EnqueueHead(T value)
        {
            if (this.Count == this.Capacity)
            {
                this.SetSize((int)((double)this.Capacity * this.GrowthFactor));
            }
            this.Head--;
            if (this.Head < 0)
            {
                this.Head += this.Capacity;
            }
            this.InnerList[this.Head] = value;
            this.count++;
            this.version += 1uL;
        }

        /// <summary>
        /// Add the given object to the collections tail
        /// 从尾部添加一条数据
        /// </summary>
        /// <param name="value">The object to enqueue</param>
        public void EnqueueTail(T value)
        {
            if (this.Count == this.Capacity)
            {
                this.SetSize((int)((double)this.Capacity * this.GrowthFactor));
            }
            this.InnerList[this.Tail] = value;
            this.Tail++;
            this.Tail %= this.Capacity;
            this.count++;
            this.version += 1uL;
        }

        /// <summary>
        /// Retrieve and remove the current head
        /// 从头部删除一条数据
        /// </summary>
        /// <returns>The removed object</returns>
        public T DequeueHead()
        {
            if (this.Count == 0)
            {
                throw new System.Exception("Dequeue was empty!");
            }
            T result = this.InnerList[this.Head];
            this.Head++;
            this.Head %= this.Capacity;
            this.count--;
            this.version += 1uL;
            return result;
        }

        /// <summary>
        /// Retrieve and remove the current tail
        /// 从尾部删除一条数据
        /// </summary>
        /// <returns>The removed object</returns>
        public T DequeueTail()
        {
            if (this.Count == 0)
            {
                throw new System.Exception("Dequeue was empty!");
            }
            T result = this.InnerList[this.Tail];
            this.Tail--;
            if (this.Tail < 0)
            {
                this.Tail += this.Capacity;
            }
            this.count--;
            this.version += 1uL;
            return result;
        }

        /// <summary>
        /// Add the given collection to the dequeues tail
        /// 将数据集添加到尾部
        /// </summary>
        /// <param name="C">The source collection</param>
        public void EnqueueTailRange(System.Collections.Generic.ICollection<T> C)
        {
            int i;
            for (i = this.Capacity; i < C.Count; i = (int)((double)i * this.GrowthFactor))
            {
            }
            if (i > this.Capacity)
            {
                this.SetSize(i);
            }
            foreach (T current in C)
            {
                this.EnqueueTail(current);
            }
        }

        /// <summary>
        /// Add the given collection to the dequeues head.
        /// To preserve the order in the collection, the entries are
        /// added in revers order.
        /// 将数据集添加到头部
        /// </summary>
        /// <param name="C">The source collection</param>
        public void EnqueueHeadRange(System.Collections.Generic.ICollection<T> C)
        {
            int i;
            for (i = this.Capacity; i < C.Count; i = (int)((double)i * this.GrowthFactor))
            {
            }
            if (i > this.Capacity)
            {
                this.SetSize(i);
            }
            System.Collections.Generic.List<T> list = new System.Collections.Generic.List<T>(C);
            list.Reverse();
            foreach (T current in list)
            {
                this.EnqueueHead(current);
            }
        }

        /// <summary>
        /// Deletes all entries from the collection
        /// </summary>
        public void Clear()
        {
            this.Head = (this.Tail = (this.count = 0));
            this.version += 1uL;
        }

        /// <summary>
        /// Sets the capacity to Count.
        /// </summary>
        public void TrimToSize()
        {
            this.SetSize(this.Count);
        }

        /// <summary>
        /// Implementation of the ICollection.CopyTo function.
        /// </summary>
        /// <param name="array">Target array</param>
        /// <param name="index">Start-Index in target array</param>
        public void CopyTo(System.Array array, int index)
        {
            if (array == null)
            {
                throw new System.ArgumentNullException("array");
            }
            if (index < 0)
            {
                throw new System.ArgumentOutOfRangeException("index", index, "Must be at least zero!");
            }
            if (array.Length - index < this.Count)
            {
                throw new System.ArgumentException("Array was to small!");
            }
            if (array.Rank > 1)
            {
                throw new System.ArgumentException("Array was multidimensional!");
            }
            for (int i = 0; i < this.Count; i++)
            {
                array.SetValue(this[i], i + index);
            }
        }

        /// <summary>
        /// Standard implementation.
        /// </summary>
        /// <returns>A DequeueEnumerator on the current dequeue</returns>
        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            return new Dequeue<T>.DequeueEnumerator(this, 0, this.Count - 1);
        }

        /// <summary>
        /// Sets the collections capacity to newSize
        /// </summary>
        /// <param name="newSize">the new collection size (must be &gt;= Count)</param>
        protected void SetSize(int newSize)
        {
            if (newSize < this.Count)
            {
                throw new System.ArgumentException("New Size was smaller than Count!");
            }
            T[] array = new T[newSize];
            for (int i = 0; i < this.Count; i++)
            {
                array[i] = this[i];
            }
            this.Head = 0;
            this.Tail = this.Count;
            this.InnerList = array;
            this.version += 1uL;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
