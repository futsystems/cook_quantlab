using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Binance.Net.Interfaces;

using Binance.Net;
using Binance.Net.Clients;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using Binance.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net;
using CryptoExchange.Net.Sockets;
using NLog;

namespace TradingLib.DataFeed
{
    
    public class OrderBookItem
    {
        public OrderBookItem(decimal price, decimal quantity)
        {
            this.Price = price;
            this.Quantity = quantity;
        }

        public OrderBookItem(decimal price, decimal quantity, decimal avgFillPrice)
        {
            this.Price = price;
            this.Quantity = quantity;
            this.AvgFilledPrice = avgFillPrice;
        }
        public decimal Price = 0;
        public decimal Quantity = 0;
        public decimal AvgFilledPrice = 0;

        public override string ToString()
        {
            return $"{this.Price} X {this.Quantity}";
        }

        public string Serialize()
        {
            return $"{this.Price} {this.Quantity} {this.AvgFilledPrice}";
        }

        private const string FORMAT = "0.########";
        public string Serialize2()
        {
            return $"{this.Price.ToString(FORMAT)} {this.Quantity.ToString(FORMAT)}";
        }
    }
    
    public class OrderBook
    {
        private NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        
        public bool Synced { get; set; }

        public DateTime CacheTime { get; set; }
        public DateTime OrderBookUpdateTime { get; set; }

        public int Level { get; set; }
        public string FeedSymbol { get; set; }
        SortedList<decimal, OrderBookItem> AskItems { get; set; }
        SortedList<decimal, OrderBookItem> BidItems { get; set; }
        
        private List<IBinanceEventOrderBook> CachedOrderBook { get; set; }
        
        public long LastUpdateId { get; set; }

        public OrderBook(string feedSymbol, int level=500)
        {
            this.FeedSymbol = feedSymbol;
            this.AskItems = new SortedList<decimal, OrderBookItem>();
            this.BidItems = new SortedList<decimal, OrderBookItem>();
            this.CachedOrderBook = new List<IBinanceEventOrderBook>();
            this.LastUpdateId = 0;
            this.Synced = false;
            this.CacheTime = DateTime.UtcNow;
            this.Level = level;
        }
        

        public void CacheOrderUpdate(IBinanceEventOrderBook update)
        {
            this.CachedOrderBook.Add(update);
        }

        public void UseOrderBookSnapshot(BinanceDepth depth)
        {
            this.AskItems.Clear();
            this.BidItems.Clear();
            
            foreach (var ask in depth.Asks)
            {
                this.UpdateAsk(ask);
            }

            foreach (var bid in depth.Bids)
            {
                this.UpdateBid(bid);
            }
                                
            this.LastUpdateId = depth.LastUpdateId;
        }


        private bool depthUpdated = false;
        
        /// <summary>
        /// 使用缓存的update
        /// </summary>
        public void UseCachedOrderBookUpdate()
        {
            foreach (var update in this.CachedOrderBook)
            {
               this.UpdateOrderBook(update);
            }
            this.CachedOrderBook.Clear();
            this.Synced = true;
        }

        public void UpdateOrderBook(IBinanceEventOrderBook depth)
        {
            //如果没有用diff depth更新过 则快照updateId在depth区间之内 就执行更新
            if (this.depthUpdated == false )
            {
                if (depth.LastUpdateId < this.LastUpdateId)
                {
                    logger.Debug($"{this.FeedSymbol}  depth ignore, lastUpdateId:{this.LastUpdateId} depth:{depth.FirstUpdateId} - {depth.LastUpdateId}");
                    return;
                }
                
                if (depth.FirstUpdateId <= this.LastUpdateId &&
                    depth.LastUpdateId >= this.LastUpdateId)
                {
                    logger.Debug(
                        $"{this.FeedSymbol} update order book first with depth, snapshotId:{this.LastUpdateId} depth:{depth.FirstUpdateId} - {depth.LastUpdateId}");
                    this.ProcessUpdate(depth);
                    this.depthUpdated = true;
                    
                }
            }
            else
            {
                //数据衔接正常
                if (depth.FirstUpdateId == this.LastUpdateId + 1)
                {
                    //logger.Debug($"{this.FeedSymbol} update order book with depth, lastUpdateId:{this.LastUpdateId} depth:{depth.FirstUpdateId} - {depth.LastUpdateId}");
                    ProcessUpdate(depth);
                }
                else
                {
                    //数据衔接异常
                    logger.Error($"{this.FeedSymbol}  depth lost, lastUpdateId:{this.LastUpdateId} depth:{depth.FirstUpdateId} - {depth.LastUpdateId}");
                    //设置同步标志
                    this.CacheTime = DateTime.UtcNow;
                    this.Synced = false;
                }
            }
        }

        void ProcessUpdate(IBinanceEventOrderBook depth)
        {
            foreach (var item in depth.Asks)
            {
                this.UpdateAsk(item.Price, item.Quantity);
            }

            foreach (var item in depth.Bids)
            {
                this.UpdateBid(item.Price, item.Quantity);
            }

            this.LastUpdateId = depth.LastUpdateId;
            if (depth.Asks.Count() > 0 || depth.Bids.Count() > 0)
            {
                this.OrderBookUpdateTime = DateTime.UtcNow;
            }
        }
        
        public void UpdateBid(decimal price, decimal quantity)
        {
            if (!this.BidItems.ContainsKey(price))
            {
                this.BidItems.Add(price,new OrderBookItem(price,0));
            }

            this.BidItems[price].Quantity = quantity;
            if (quantity == 0)
            {
                this.BidItems.Remove(price);
            }
        }

        public void UpdateAsk(decimal[] data)
        {
            this.UpdateAsk(data[0],data[1]);
        }
        
        public void UpdateBid(decimal[] data)
        {
            this.UpdateBid(data[0],data[1]);
        }
        
        public void UpdateAsk(decimal price, decimal quantity)
        {
            if (!this.AskItems.ContainsKey(price))
            {
                this.AskItems.Add(price, new OrderBookItem(price, 0));
            }
            //更新某个价格的数量
            this.AskItems[price].Quantity = quantity;
            if (quantity == 0)
            {
                this.AskItems.Remove(price);
            }

        }

        public string SerializeAsk()
        {
            StringBuilder sb = new StringBuilder();
            var items = this.AskItems.Values.ToList();
            int totalCnt = Math.Max(this.Level,items.Count);
            for(int i = 0;i<totalCnt; i++)
            {
                sb.Append(items[i].Serialize2());
                sb.Append("|");
            }

            return sb.ToString();
        }

        public string SerializeBid()
        {
            StringBuilder sb = new StringBuilder();
            var items = this.BidItems.Values.ToList();
            int totalCnt = Math.Max(this.Level,items.Count);
            for(int i = 0;i<totalCnt; i++)
            {
                sb.Append(items[totalCnt -1 - i].Serialize2());
                sb.Append("|");
            }

            return sb.ToString();
        }

        decimal CalcNextLevelPrice(int level,OrderBookItem item)
        {
            decimal result = 0;
            if (level == 1)
            {
                result = item.Price * 1.0001m;
            }
            else if (level == 2)
            {
                result = item.Price * 1.0001m;
            }
            else if (level == 3)
            {
                result = item.Price * 1.0001m;
            }
            else if (level == 4)
            {
                result = item.Price * 1.0001m;
            }
            else if (level == 5)
            {
                result = item.Price * 1.0001m;
            }
            else if (level == 6)
            {
                result = item.Price * 1.0002m;
            }
            else if (level == 7)
            {
                result = item.Price * 1.0002m;
            }
            else if (level == 8)
            {
                result = item.Price * 1.0002m;
            }
            else if (level == 9)
            {
                result = item.Price * 1.0002m;
            }
            else if (level == 10)
            {
                result = item.Price * 1.0002m;
            }
            
            result = item.Price * 1.0005m;
            return Math.Round(result, 2);
        }
        
        public  List<OrderBookItem>  GetAggregatedAskOrderBook(int levelCnt)
        {
            List<OrderBookItem> aggreatedItems = new List<OrderBookItem>();
            
            decimal nextPrice = 0;
            decimal totalQuantity = 0;
            decimal turnOver = 0;
            if (this.AskItems.Count > 0)
            {
                 var askCnt = 0;
                var start = this.AskItems.First();
                nextPrice = CalcNextLevelPrice(askCnt+1, start.Value);
               
                foreach (var item in this.AskItems)
                {
                    if (item.Value.Price <= nextPrice)
                    {
                        totalQuantity += item.Value.Quantity;
                        turnOver += item.Value.Quantity * item.Value.Price;
                    }
                    else
                    {
                        //越过计算的价格 则进入下一档
                        aggreatedItems.Add(new OrderBookItem(nextPrice,totalQuantity,totalQuantity>0 ? Math.Round(turnOver / totalQuantity,2) : 0));
                        askCnt++;
                        if (askCnt >= levelCnt)
                        {
                            goto RTN;
                        }
                        nextPrice = CalcNextLevelPrice(askCnt +1, item.Value);
                        totalQuantity = item.Value.Quantity;
                        turnOver = item.Value.Quantity * item.Value.Price;
                    }
                }
                aggreatedItems.Add(new OrderBookItem(nextPrice,totalQuantity, totalQuantity>0 ? Math.Round(turnOver/totalQuantity,2) : 0));
                askCnt++;
            }
            RTN:
            return aggreatedItems;
        }

    }
}