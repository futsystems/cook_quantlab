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
using Binance.Net.Objects.Models.Futures.Socket;
using Binance.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net.Sockets;


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
        
        }


        bool _isrunning = false;
        public override bool IsRunning
        {
            get
            {
                return _isrunning;
            }
        }

        private BinanceSocketClient socketClient = null;
        
        public override void Start()
        {
            if (_isrunning)
            {
                logger.Info("DataFeedIQFeed already start");
                return;
            }
            

            if (socketClient == null)
            {
                socketClient = new BinanceSocketClient();
            }
            
            this.OnConnected();
        }
        

        /// <summary>
        /// Trade
        /// </summary>
        /// <param name="evt"></param>
        void HandleEvent(DataEvent<BinanceStreamAggregatedTrade> evt)
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
        void HandleEvent(DataEvent<BinanceFuturesStreamBookPrice> evt)
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
        void HandeEvent(DataEvent<IBinanceFuturesEventOrderBook> evt)
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
            if (info.SymbolType == SymbolInfo.TYPE_PERPETUAL)
            {
                //币本位
                if (info.Quote == "USDT")
                {
                    return $"{info.Base}{info.Quote}_PERP";
                }
            }
            
            else if (info.SymbolType == SymbolInfo.TYPE_FUTURES)
            {
                //币本位
                if (info.Quote == "USDT")
                {
                    return $"{info.Base}{info.Quote}_{info.Expire}";
                }
            }

            //empty feed symbol will not register data
            return string.Empty;
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
            if (socketClient != null)
            {
                //trade stream
                socketClient.UsdFuturesStreams.SubscribeToAggregatedTradeUpdatesAsync(symbol, HandleEvent);
                //quote stream best ask/bid
                socketClient.UsdFuturesStreams.SubscribeToBookTickerUpdatesAsync(symbol, HandleEvent);
                //order book
                socketClient.UsdFuturesStreams.SubscribeToPartialOrderBookUpdatesAsync(symbol, 10, 100, HandeEvent);
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
