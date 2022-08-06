using System.Linq;
using System.Collections.Generic;


namespace TradingLib.DataFeed
{
    public class OrderBookItem
    {
        public OrderBookItem(double price, double quantity)
        {
            this.Price = price;
            this.Quantity = quantity;
        }
        
        public double Price = 0;
        public double Quantity = 0;
    }
    
    public class OrderBook
    {
        public bool Synced { get; set; }
        SortedList<double, OrderBookItem> AskItems { get; set; }
        SortedList<double, OrderBookItem> BidItems { get; set; }

        public long LastUpdateId { get; set; }

        public OrderBook()
        {
            this.AskItems = new SortedList<double, OrderBookItem>();
            this.BidItems = new SortedList<double, OrderBookItem>();
            this.LastUpdateId = 0;
            this.Synced = false;
        }

        public void UpdateBid(double price, double quantity)
        {
            if (!this.BidItems.ContainsKey(price))
            {
                this.BidItems.Add(price,new OrderBookItem(price,0));
            }

            this.BidItems[price].Quantity = quantity;
        }
        
        public void UpdateAsk(double price, double quantity)
        {
            if (!this.AskItems.ContainsKey(price))
            {
                this.AskItems.Add(price, new OrderBookItem(price, 0));
            }
            //更新某个价格的数量
            this.AskItems[price].Quantity = quantity;

        }

        double CalcNextLevelPrice(OrderBookItem item)
        {
            return item.Price * 1.0005;
        }
        
        public  List<OrderBookItem>  GetAggregatedAskOrderBook(int levelCnt)
        {
            List<OrderBookItem> aggreatedItems = new List<OrderBookItem>();
            
            double nextPrice = 0;
            double totalQuantity = 0;
            
            if (this.AskItems.Count > 0)
            {
                var start = this.AskItems.First();
                nextPrice = CalcNextLevelPrice(start.Value);
                var askCnt = 0;
                foreach (var item in this.AskItems)
                {
                    if (item.Value.Price <= nextPrice)
                    {
                        totalQuantity += item.Value.Quantity;
                    }
                    else
                    {
                        //越过计算的价格 则进入下一档
                        aggreatedItems.Add(new OrderBookItem(nextPrice,totalQuantity));
                        askCnt++;
                        if (askCnt >= levelCnt)
                        {
                            break;
                        }
                        nextPrice = CalcNextLevelPrice(item.Value);
                        totalQuantity = item.Value.Quantity;
                    }
                }
                aggreatedItems.Add(new OrderBookItem(nextPrice,totalQuantity));
                askCnt++;
            }

            return aggreatedItems;
        }

    }
}