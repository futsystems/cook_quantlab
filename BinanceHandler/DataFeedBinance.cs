using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.DataFeed;

using Binance.Net;
using Binance.Net.Clients;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using Binance.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net.Sockets;
using System.Threading;

//doc
/*
 * https://jkorf.github.io/Binance.Net/
 * https://binance-docs.github.io/apidocs/spot/en/#diff-depth-stream
 * 
 */
namespace BinanceHander
{
    /// <summary>
    /// Binance DataFeed
    /// </summary>
    public class DataFeedBinance:DataFeedBase
    {
        private const string EXCHANGE = "BINANCE";
        

        public DataFeedBinance(TickPot tickpot, string exchange, string address, int qryport)
            : base(tickpot, exchange, address, qryport)
        { 
        
            InitTimer();
            timer.Start();
            
        }

        System.Timers.Timer timer;
        void InitTimer()
        {
            int interval = 1000 * 60 * 5;//5分钟发送一次快照
            timer = new System.Timers.Timer(interval);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(SendTickSnapshot);
        }
        


        private void SendTickSnapshot(object sender, System.Timers.ElapsedEventArgs e)
        {
            logger.Info("send tick snapshot");
            foreach (var item in feedSym2Orderbook)
            {
                if (feedsym2tickSnapshotMap.TryGetValue(item.Key, out var k))
                {
                    lock (item.Value)
                    {
                        k.HostTime = DateTime.UtcNow.ToTimeStamp();
                        k.TickTime = k.HostTime;
                        k.TickContent1 = item.Value.SerializeAsk();
                        k.TickContent2 = item.Value.SerializeBid();
                        k.TickContent3 = item.Value.LastUpdateId.ToString();
                        k.UpdateType = "OS";
                        this.NewTick(k);
                    }
                }
            }
        }


        bool _isrunning = false;
        public override bool IsRunning
        {
            get
            {
                return _isrunning;
            }
        }

        private BinanceSocketClient spotClient = null;
        
        public override void Start()
        {
            if (_isrunning)
            {
                logger.Info("DataFeedIQFeed already start");
                return;
            }
            

            if (spotClient == null)
            {
                spotClient = new BinanceSocketClient();
            }

            var client = new BinanceClient();
            
            this.StartSync();
            
            this.OnConnected();
        }
        

        /// <summary>
        /// Trade
        /// </summary>
        /// <param name="evt"></param>
        void HandleEvent(DataEvent<BinanceStreamTrade> evt)
        {
            if (feedsym2tickSnapshotMap.TryGetValue(evt.Data.Symbol.ToUpper(), out var k))
            {
                //更新Trade
                k.Price = (double)evt.Data.Price;
                k.Size = (double)evt.Data.Quantity;
                k.TradeId = evt.Data.Id.ToString();
                k.TradeFlag = evt.Data.BuyerIsMaker ? 1 : 0;
                k.HostTime = DateTime.UtcNow.ToTimeStamp();
                k.TickTime = evt.Data.TradeTime.ToTimeStamp();
                
                k.UpdateType = "X";
                this.NewTick(k);
            }
        }

        /// <summary>
        /// Quote
        /// </summary>
        /// <param name="evt"></param>
        void HandleEvent(DataEvent<BinanceStreamBookPrice> evt)
        {
            if (feedsym2tickSnapshotMap.TryGetValue(evt.Data.Symbol.ToUpper(), out var k))
            {
                //更新Trade
                bool changed = false;

                if ((double) evt.Data.BestAskPrice != k.AskPrice || (double) evt.Data.BestAskQuantity != k.AskSize)
                {
                    k.AskPrice = (double) evt.Data.BestAskPrice;
                    k.AskSize = (double) evt.Data.BestAskQuantity;
                    changed = true;
                }

                if ((double) evt.Data.BestBidPrice != k.BidPrice || (double) evt.Data.BestBidQuantity != k.BidSize)
                {
                    k.BidPrice = (double) evt.Data.BestBidPrice;
                    k.BidSize = (double) evt.Data.BestBidQuantity;
                    changed = true;
                }

                if (changed)
                {
                    k.HostTime = DateTime.UtcNow.ToTimeStamp();
                    k.TickTime = evt.Timestamp.ToTimeStamp();
                    k.UpdateType = "Q";
                    this.NewTick(k);
                }
            }
        }

        /// <summary>
        /// Order Book
        /// </summary>
        /// <param name="evt"></param>
        void HandleEvent(DataEvent<IBinanceOrderBook> evt)
        {
            if (feedsym2tickSnapshotMap.TryGetValue(evt.Data.Symbol.ToUpper(), out var k))
            {
               
                //更新Quote
                var item = evt.Data.Asks.ElementAt(0);
                k.AskPrice = (double)item.Price;
                k.AskSize = (double)item.Quantity;
                
                item =  evt.Data.Asks.ElementAt(1);
                k.AskPrice2 =  (double)item.Price;
                k.AskSize2 =   (double)item.Quantity;
                
                item =  evt.Data.Asks.ElementAt(2);
                k.AskPrice3 =  (double)item.Price;
                k.AskSize3 =   (double)item.Quantity;
                
                item = evt.Data.Asks.ElementAt(3);
                k.AskPrice4 =  (double)item.Price;
                k.AskSize4 =   (double)item.Quantity;
                
                item = evt.Data.Asks.ElementAt(4);
                k.AskPrice5 =  (double)item.Price;
                k.AskSize5 =   (double)item.Quantity;
                
                item =  evt.Data.Asks.ElementAt(5);
                k.AskPrice6 =  (double)item.Price;
                k.AskSize6 =   (double)item.Quantity;
                
                item =  evt.Data.Asks.ElementAt(6);
                k.AskPrice7 =  (double)item.Price;
                k.AskSize7 =   (double)item.Quantity;
                
                item =  evt.Data.Asks.ElementAt(7);
                k.AskPrice8 =  (double)item.Price;
                k.AskSize8 =   (double)item.Quantity;
                
                item =  evt.Data.Asks.ElementAt(8);
                k.AskPrice9 =  (double)item.Price;
                k.AskSize9 =   (double)item.Quantity;
                
                item =  evt.Data.Asks.ElementAt(9);
                k.AskPrice10 =  (double)item.Price;
                k.AskSize10 =   (double)item.Quantity;

                item = evt.Data.Bids.ElementAt(0);
                k.BidPrice =  (double)item.Price;
                k.BidSize =  (double) item.Quantity;
                
                item = evt.Data.Bids.ElementAt(1);
                k.BidPrice2 =  (double)item.Price;
                k.BidSize2 =   (double)item.Quantity;
                
                item = evt.Data.Bids.ElementAt(2);
                k.BidPrice3 =  (double)item.Price;
                k.BidSize3 =   (double)item.Quantity;
                
                item = evt.Data.Bids.ElementAt(3);
                k.BidPrice4 =  (double)item.Price;
                k.BidSize4 =   (double)item.Quantity;
                
                item = evt.Data.Bids.ElementAt(4);
                k.BidPrice5 =  (double)item.Price;
                k.BidSize5 =   (double)item.Quantity;
                
                item = evt.Data.Bids.ElementAt(5);
                k.BidPrice6 =  (double)item.Price;
                k.BidSize6 =   (double)item.Quantity;
                
                item = evt.Data.Bids.ElementAt(6);
                k.BidPrice7 =  (double)item.Price;
                k.BidSize7 =  (double) item.Quantity;
                
                item = evt.Data.Bids.ElementAt(7);
                k.BidPrice8 =  (double)item.Price;
                k.BidSize8 =   (double)item.Quantity;
                
                item = evt.Data.Bids.ElementAt(8);
                k.BidPrice9 =  (double)item.Price;
                k.BidSize9 =   (double)item.Quantity;
                
                item = evt.Data.Bids.ElementAt(9);
                k.BidPrice10 = (double) item.Price;
                k.BidSize10 =   (double)item.Quantity;

                k.HostTime = DateTime.UtcNow.ToTimeStamp();
                k.TickTime = evt.Timestamp.ToTimeStamp();
                
                k.UpdateType = "O";
                this.NewTick(k);
            }
        }

        BinanceDepth GetOrderBookDepth(string symbol)
        {
            var url = $"https://api.binance.com/api/v3/depth?symbol={symbol}&limit=1000";
            var result = HttpHelper.Get<BinanceDepth>(url);
            return result;
        }

        void StartSync()
        {
            if (_syncGo == false)
            {
                _syncGo = true;
                _syncThread = new Thread(ProcessOrderBookSyncTask);
                _syncThread.IsBackground = true;
                _syncThread.Start();
                
            }
        }


        private Thread _syncThread = null;
        private bool _syncGo = false;
        ManualResetEvent _syncwaiting = new ManualResetEvent(false);
        void ProcessOrderBookSyncTask()
        {
            while (_syncGo)
            {
                try
                {
                    foreach (var item in feedSym2Orderbook.Values.Where(e=>e.Synced == false).ToList())
                    {
                        //延迟5秒查询depth snapshot，可以获得updateId包含在实时数据中
                        if (DateTime.UtcNow.Subtract(item.CacheTime).TotalSeconds > 5)
                        {
                            logger.Info($"sync order book depth data for:{item.FeedSymbol}");
                            var result = this.GetOrderBookDepth(item.FeedSymbol);
                            logger.Info($"snapshot last update id:{result.LastUpdateId}");
                            
                            lock (item)
                            {
                                item.UseOrderBookSnapshot(result);
                                item.UseCachedOrderBookUpdate();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                // clear current flag signal
                _syncwaiting.Reset();

                // wait for a new signal to continue reading
                _syncwaiting.WaitOne(100);
            }
            
          
        }

        private ConcurrentDictionary<string, OrderBook> feedSym2Orderbook =
            new ConcurrentDictionary<string, OrderBook>();


        private long? lastUpdateId = 0;
        void HandleEvent(DataEvent<IBinanceEventOrderBook> evt)
        {
            if (feedsym2tickSnapshotMap.TryGetValue(evt.Data.Symbol.ToUpper(), out var k) && evt.Data.Symbol.ToUpper() == "BTCUSDT")
            {
                //logger.Info($"depth order update id:{evt.Data.LastUpdateId}");
                if (!feedSym2Orderbook.TryGetValue(evt.Data.Symbol.ToUpper(), out var orderBook))
                {
                    orderBook = new OrderBook(evt.Data.Symbol.ToUpper());
                    feedSym2Orderbook.TryAdd(evt.Data.Symbol.ToUpper(), orderBook);//在其他线程内执行数据同步
                }
                
                lock (orderBook)
                {
                    if (lastUpdateId.HasValue && evt.Data.FirstUpdateId > lastUpdateId)//确保lastUpdateId有值 然后 顺序
                    {
                        if (lastUpdateId + 1 != evt.Data.FirstUpdateId)
                        {
                            logger.Info(
                                $"--> data lost, lastupdateId:{lastUpdateId} current data first:{evt.Data.FirstUpdateId} last:{evt.Data.LastUpdateId}");
                        }
                    }
                    else
                    {
                        lastUpdateId = evt.Data.LastUpdateId;
                    }
                    
                    if (orderBook.Synced == true)
                    {
                       orderBook.UpdateOrderBook(evt.Data);
                    }
                    else
                    {
                        orderBook.CacheOrderUpdate(evt.Data);
                    }

                    if (evt.Data.FirstUpdateId.HasValue)
                    {
                        if (evt.Data.FirstUpdateId.Value > orderBook.LastDataUpdateId)
                        {
                            orderBook.LastDataUpdateId = evt.Data.LastUpdateId;//将depth的最近更新Id记录到orderBook 用于排除数据源有乱序数据
                            //发送depth tick
                            k.HostTime = DateTime.UtcNow.ToTimeStamp();
                            k.TickTime = evt.Timestamp.ToTimeStamp();
                            k.TickContent1 = Serialize(evt.Data.Asks);
                            k.TickContent2 = Serialize(evt.Data.Bids);
                            k.TickContent3 = evt.Data.FirstUpdateId.HasValue ? evt.Data.FirstUpdateId.ToString() : "0";
                            k.TickContent4 = evt.Data.LastUpdateId.ToString();
                            k.UpdateType = "OD";
                            this.NewTick(k);
                        }
                        else
                        {
                            logger.Info(
                                $"got error detph update, {evt.Data.FirstUpdateId} - {evt.Data.LastUpdateId} order book last data update id: {orderBook.LastDataUpdateId}");
                        }
                    }
                    else
                    {
                        orderBook.LastDataUpdateId = evt.Data.LastUpdateId;//将depth的最近更新Id记录到orderBook 用于排除数据源有乱序数据
                        //发送depth tick
                        k.HostTime = DateTime.UtcNow.ToTimeStamp();
                        k.TickTime = evt.Timestamp.ToTimeStamp();
                        k.TickContent1 = Serialize(evt.Data.Asks);
                        k.TickContent2 = Serialize(evt.Data.Bids);
                        k.TickContent3 = evt.Data.FirstUpdateId.HasValue ? evt.Data.FirstUpdateId.ToString() : "0";
                        k.TickContent4 = evt.Data.LastUpdateId.ToString();
                        k.UpdateType = "OD";
                        this.NewTick(k);
                        
                    }
                    
                }
                
                
            }
        }

        private const string FORMAT = "0.########";
        
        string Serialize(IEnumerable<Binance.Net.Objects.Models.BinanceOrderBookEntry> entries)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in entries)
            {
                sb.Append($"{item.Price.ToString(FORMAT)} {item.Quantity.ToString(FORMAT)}");
                sb.Append('|');
            }
            return sb.ToString();
        }


        public override void Stop()
        {
            if (!_isrunning)
            {
                logger.Info("BinanceFeed not started");
                return;
            }
            
            logger.Info("BinanceFeed stopped");
        }
        
        protected override void OnConnected()
        {
            logger.Info("BinanceFeed Connected");
            base.OnConnected();
        }

        protected override void OnDisconnected()
        {
            logger.Info("BinanceFeed Disconnected");
            base.OnDisconnected();
        }

        

        protected override string ConvertExchange2FeedFormat(SymbolInfo info)
        {
            return info.Exchange;
        }
        
        protected override string ConvertSymbol2FeedFormat(SymbolInfo info)
        {
            if (info.SymbolType == SymbolInfo.TYPE_SPOT)
            {
                return $"{info.Base}{info.Quote}";
            }
            else
            {
                return string.Empty;
            }
        }

        

        ConcurrentDictionary<string,Tick> feedsym2tickSnapshotMap = new ConcurrentDictionary<string,Tick>();


        public override void SubMarketData(SymbolInfo info, string exchange, string symbol)
        {
            if (!feedsym2tickSnapshotMap.Keys.Contains(symbol))
            {
                Tick tick = new TickImpl();
                tick.Exchange = EXCHANGE;
                tick.Symbol = symbol;

                feedsym2tickSnapshotMap.TryAdd(symbol, tick);
            }

            //注册数据
            if (spotClient != null)
            {
                //trade stream
                spotClient.SpotStreams.SubscribeToTradeUpdatesAsync(symbol, HandleEvent);
                //quote stream best ask/bid
                spotClient.SpotStreams.SubscribeToBookTickerUpdatesAsync(symbol, HandleEvent);
                //order book
                spotClient.SpotStreams.SubscribeToPartialOrderBookUpdatesAsync(symbol, 20, 100, HandleEvent);
                //Diff. Depth Stream
                spotClient.SpotStreams.SubscribeToOrderBookUpdatesAsync(symbol, 100, HandleEvent);
            }
            
        }


        /// <summary>
        /// IQFeed为实时市场行情数据，盘口的每次变化都会触发一个tick数据
        /// 这里我们定时发送行情快照与成交发送结合的方式
        /// 1.产生每笔成交发送行情数据
        /// 2.定时发送快照数据
        /// </summary>
        void SendTickSnapshot()
        {
            foreach (var tick in feedsym2tickSnapshotMap.Values)
            {
                this.NewTick(TickImpl.Copy(tick));
            }
        }
        
    }
}
