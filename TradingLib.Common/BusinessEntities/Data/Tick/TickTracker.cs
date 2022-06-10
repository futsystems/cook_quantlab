using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// keep track of bid/ask and last data for symbols
    /// 其实TickTracker维护了一个市场行情快照
    /// 当不同的合约有成交数据 报价数据产生时,用于更新本地行情快照 将最新的数据更新到对应的字段
    /// 当使用时 通过symbol进行索引 获得对应的行情快照
    /// 
    /// 这里直接使用S类型的Tick 是否会更理想，在这个数据维护器中，获得一个行情快照 需要从多个数据维护期中查找数据 效率明显比直接获取Tick对象要慢很多
    /// </summary>
    public class TickTracker
    {
        public void GotTick(Tick k)
        {
            this.UpdateTick(k);
        }

        public void Clear()
        {
            tickSnapshotMap.Clear();
        }

        /// <summary>
        /// create ticktracker
        /// </summary>
        public TickTracker()
        {

        }

        public int Count { get { return tickSnapshotMap.Count; } }

        /// <summary>
        /// 返回所有行情Tick
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tick> GetTicks()
        {
            return tickSnapshotMap.Values;
        }

        public IEnumerable<Tick> TickSnapshots
        {
            get
            {
                return tickSnapshotMap.Values;
            }
        }

        ConcurrentDictionary<string, Tick> tickSnapshotMap = new ConcurrentDictionary<string, Tick>();

        //TODO SymbolKey
        /// <summary>
        /// get a tick in tick format
        /// </summary>
        /// <param name="sym"></param>
        /// <returns></returns>
        public Tick this[string exchange,string sym]
        {
            get
            {
                string key = string.Format("{0}-{1}", exchange, sym);
                Tick snapshot = null;
                if (tickSnapshotMap.TryGetValue(key, out snapshot))
                {
                    return snapshot;
                }
                return null;
            }
        }

        /// <summary>
        /// 更新Tick数据到Tick快照维护器
        /// </summary>
        /// <param name="k"></param>
        public void UpdateTick(Tick k)
        {
            if(k == null) return;
            if(string.IsNullOrEmpty(k.Symbol) || string.IsNullOrEmpty(k.Exchange)) return;
            
            string key = k.GetSymbolUniqueKey();

            Tick snapshot = null;
            if (!tickSnapshotMap.TryGetValue(key, out snapshot))
            {
                snapshot = new TickImpl();
                snapshot.UpdateType = "S";
                snapshot.Symbol = k.Symbol;
                snapshot.Exchange = k.Exchange;
                tickSnapshotMap.TryAdd(key, snapshot);
            }
   
            switch (k.UpdateType)
            {
                case "X":
                    {
                        snapshot.Date = k.Date;
                        snapshot.Time = k.Time;
                        snapshot.Price = k.Price;
                        snapshot.Size = k.Size;
                        snapshot.Exchange = k.Exchange;
                        break;
                    }
                case "A":
                    {
                        snapshot.AskPrice = k.AskPrice;
                        snapshot.AskSize = k.AskSize;
                        snapshot.AskExchange = k.AskExchange;
                        snapshot.Exchange = k.Exchange;
                        break;
                    }
                case "B":
                    {
                        snapshot.BidPrice = k.BidPrice;
                        snapshot.BidSize = k.BidSize;
                        snapshot.BidExchange = k.BidExchange;
                        snapshot.Exchange = k.Exchange;
                        break;
                    }
                case "Q":
                    {
                        snapshot.AskPrice = k.AskPrice;
                        snapshot.AskExchange = k.AskExchange;
                        snapshot.AskSize = k.AskSize;
                        snapshot.BidPrice = k.BidPrice;
                        snapshot.BidSize = k.BidSize;
                        snapshot.BidExchange = k.BidExchange;
                        snapshot.Exchange = k.Exchange;
                        break;
                    }
                
                case "2U":
                    {
                        snapshot.Depth = k.Depth;
                        switch (k.Depth)
                        {
                            case 1:
                                {
                                    snapshot.AskPrice = k.AskPrice;
                                    snapshot.AskSize = k.AskSize;
                                    snapshot.BidPrice = k.BidPrice;
                                    snapshot.BidSize = k.BidSize;
                                    break;
                                }
                            case 2:
                                {
                                    snapshot.AskPrice2 = k.AskPrice2;
                                    snapshot.AskSize2 = k.AskSize2;
                                    snapshot.BidPrice2 = k.BidPrice2;
                                    snapshot.BidSize2 = k.BidSize2;
                                    break;
                                }
                            case 3:
                                {
                                    snapshot.AskPrice3 = k.AskPrice3;
                                    snapshot.AskSize3 = k.AskSize3;
                                    snapshot.BidPrice3 = k.BidPrice3;
                                    snapshot.BidSize3 = k.BidSize3;
                                    break;
                                }
                            case 4:
                                {
                                    snapshot.AskPrice4 = k.AskPrice4;
                                    snapshot.AskSize4 = k.AskSize4;
                                    snapshot.BidPrice4 = k.BidPrice4;
                                    snapshot.BidSize4 = k.BidSize4;
                                    break;
                                }
                            case 5:
                                {
                                    snapshot.AskPrice5 = k.AskPrice5;
                                    snapshot.AskSize5 = k.AskSize5;
                                    snapshot.BidPrice5 = k.BidPrice5;
                                    snapshot.BidSize5 = k.BidSize5;
                                    break;
                                }
                            case 6:
                                {
                                    snapshot.AskPrice6 = k.AskPrice6;
                                    snapshot.AskSize6 = k.AskSize6;
                                    snapshot.BidPrice6 = k.BidPrice6;
                                    snapshot.BidSize6 = k.BidSize6;
                                    break;
                                }
                            case 7:
                                {
                                    snapshot.AskPrice7 = k.AskPrice7;
                                    snapshot.AskSize7 = k.AskSize7;
                                    snapshot.BidPrice7 = k.BidPrice7;
                                    snapshot.BidSize7 = k.BidSize7;
                                    break;
                                }
                            case 8:
                                {
                                    snapshot.AskPrice8 = k.AskPrice8;
                                    snapshot.AskSize8 = k.AskSize8;
                                    snapshot.BidPrice8 = k.BidPrice8;
                                    snapshot.BidSize8 = k.BidSize8;
                                    break;
                                }
                            case 9:
                                {
                                    snapshot.AskPrice9 = k.AskPrice9;
                                    snapshot.AskSize9 = k.AskSize9;
                                    snapshot.BidPrice9 = k.BidPrice9;
                                    snapshot.BidSize9 = k.BidSize9;
                                    break;
                                }
                            case 10:
                                {
                                    snapshot.AskPrice10 = k.AskPrice10;
                                    snapshot.AskSize10 = k.AskSize10;
                                    snapshot.BidPrice10 = k.BidPrice10;
                                    snapshot.BidSize10 = k.BidSize10;
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
