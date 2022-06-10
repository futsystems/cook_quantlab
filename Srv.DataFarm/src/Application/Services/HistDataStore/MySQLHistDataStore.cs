using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TradingLib.API;
using TradingLib.Common;
using UniCryptoLab.Common.Data;


namespace UniCryptoLab.Services
{
    public class MySQLHistDataStore:IHistDataStore
    {
        public IMapper Mapper { get; set; }


        public MySQLHistDataStore(IMapper mapper)
        {
            this.Mapper = mapper;
        }
        
        /// <summary>
        /// 查询Bar数据
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="type"></param>
        /// <param name="interval"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="startIndex"></param>
        /// <param name="maxcount"></param>
        /// <returns></returns>
        public List<IBarItem> QueryBar(string symbol, BarInterval type, int interval, DateTime start, DateTime end, int startIndex, int maxcount)
        {
            
            var records = DBFactory.Default.Get<Entities.HistBarInfo>()
                .Where(e=>e.Symbol == symbol)//合约匹配
                .Where(e=>e.Interval == interval && e.IntervalType == type)//频率匹配
                .Where(bar => bar.EndTime >= start && bar.EndTime <= end);//时间匹配
        
            
            if (maxcount <= 0)
            {
                //不限制最大返回数量 
                records = records.Take(Math.Max(0, records.Count() - startIndex));
            }
            else //设定最大数量 返回数据要求 按时间先后排列
            {
                //startIndex 首先从数据序列开头截取对应数量的数据
                //maxcount 然后从数据序列末尾截取最大数量的数据
                records = records.Take(Math.Max(0, records.Count() - startIndex)).Skip(Math.Max(0, (records.Count() - startIndex) - maxcount));//返回序列后段元素
            }
            return records.ToList<IBarItem>();
        }
        

        /// <summary>
        /// 更新某个Bar
        /// </summary>
        /// <param name="bar"></param>
        public void UpdateBar(IBarItem bar)
        {
            var item = DBFactory.Default.Get<Entities.HistBarInfo>()
                .Where(e => e.Symbol == bar.Symbol
                            && e.IntervalType == bar.IntervalType
                            && e.Interval == bar.Interval
                            && e.EndTime == bar.EndTime).FirstOrDefault();
            if (item == null)
            {
                item = this.Mapper.Map<Entities.HistBarInfo>(bar);
                DBFactory.Default.Add(item);
            }
            else
            {
                this.Mapper.Map(bar, item);
                DBFactory.Default.Update(item);
            }
        }

        /// <summary>
        /// 增加一个Bar数据
        /// </summary>
        /// <param name="bar"></param>
        /// <returns></returns>
        public Entities.HistBarInfo AddBar(IBarItem bar)
        {
            var item = this.Mapper.Map<Entities.HistBarInfo>(bar);
            DBFactory.Default.Add(item);
            return item;
        }

        /// <summary>
        /// 删除某个时间段内的Bar
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="type"></param>
        /// <param name="interval"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void DeleteBar(string symbol, BarInterval type, int interval, DateTime start, DateTime end)
        {
            DBFactory.Default.Del<Entities.HistBarInfo>()
                .Where(e => e.Symbol == symbol) //合约匹配
                .Where(e => e.Interval == interval && e.IntervalType == type) //频率匹配
                .Where(bar => bar.EndTime >= start && bar.EndTime <= end)//时间匹配
                .Execute();
        }
        

    }
}