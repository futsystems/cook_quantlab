using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 时间序列List
    /// 具备QList<T>的功能,同时具备时间过滤的功能
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QTimeList<T>:QList<T>
        where T : ITimeSeries
    {


        /// <summary>
        /// 如何通过时间来筛选?
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public T[] LoadItem(int count, bool loadFromEnd = true)
        {
            //(this.LookBack(0) as ITimeSeries).Time
            if (count > 0)
            {
                if (count > this.Count)
                {
                    return this.Items.ToArray();
                }
                else
                {
                    List<T> list = new List<T>();
                    if (loadFromEnd)
                    {
                        for (int i = 1; i <= count; i++)
                        {
                            list.Add(this.LookBack(count - i));
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= count; i++)
                        {
                            list.Add(this.LookBack(this.Count - i));
                        }
                    }
                    return list.ToArray();
                }
            }
            else//按日期
            {



            }
            return null;

        }
    }
}
