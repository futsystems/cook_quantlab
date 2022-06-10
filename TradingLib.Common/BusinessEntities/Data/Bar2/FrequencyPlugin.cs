using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    public abstract class FrequencyPlugin
    {
        /// <summary>
        /// Bar间隔类型
        /// </summary>
        public abstract BarFrequency BarFrequency { get; }
        /// <summary>
        /// 返回Bar数据生成器
        /// </summary>
        /// <returns></returns>
        public abstract IFrequencyGenerator CreateFrequencyGenerator();

        /// <summary>
        /// 全复制
        /// </summary>
        /// <returns></returns>
        public abstract FrequencyPlugin Clone();

        /// <summary>
        /// 是否是基于时间
        /// </summary>
        public abstract bool IsTimeBased { get; }

        /// <summary>
        /// Determines whether two FrequencyPlugin instances are equal
        /// </summary>
        /// <param name="obj">FrequencyPlugin used for comparison</param>
        /// <returns>true if they are equal, otherwise false.</returns>
        public abstract override bool Equals(object obj);

        /// <summary>
        /// Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public abstract override int GetHashCode();


    }
}
