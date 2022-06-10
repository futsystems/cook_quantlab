using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using UniCryptoLab.Models;
using TradingLib.API;
using TradingLib.Common;

namespace UniCryptoLab.Services
{
    public class BinanceBarSource:IHistBarSource
    {
        private NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

        private BinanceClient client = new BinanceClient();
        
        // public async Task<List<IBarItem>> GetBar(string symbol,DateTime start, DateTime end)
        // {
        //     List<IBarItem> data = new List<IBarItem>();
        //     var span = TimeSpan.FromMinutes(1);
        //     var qryEnd = TimeFrequency.BarEndTime(end, span);
        //     
        //     var info = SymbolInfo.ParseSymbol(symbol);
        //     if (info.SymbolType == "SPOT")
        //     {
        //         var binanceSymbol = $"{info.Base}{info.Quote}";
        //         
        //
        //         DateTime currentStart = start;
        //
        //         while (currentStart < end)
        //         {
        //             var result =
        //                 await client.SpotApi.ExchangeData.GetKlinesAsync(binanceSymbol, KlineInterval.OneMinute, currentStart,
        //                     qryEnd,
        //                     1000);
        //             if (result.Success)
        //             {
        //                 
        //                 var items = result.Data.Select(e => ConvertToLocalBar(symbol, e, span)).ToList();
        //                 data.AddRange(items);
        //                 if (items.Count > 0)
        //                 {
        //                     currentStart = items.Last().EndTime;
        //                 }
        //             }
        //         }
        //     }
        //     return new List<IBarItem>();
        // }

        public async Task<List<IBarItem>> GetHistBar(SymbolInfo info, DateTime start, DateTime end)
        {
            var symbol =  $"{info.Base}{info.Quote}";
            var barSymbol =  $"{info.Exchange}-{info.Symbol}";
            var span = TimeSpan.FromMinutes(1);
            var result = await client.SpotApi.ExchangeData.GetKlinesAsync(symbol, KlineInterval.OneMinute, start, end, 1000);
            if (result.Success)
            {
                var items = result.Data.Select(e => ConvertToLocalBar(barSymbol, e, span)).ToList();
                return items;
            }
            else
            {
                throw new Exception("Get hist bar data error");
            }
        }


        IBarItem ConvertToLocalBar(string symbol,IBinanceKline bar, TimeSpan span)
        {
            return new BarItem()
            {
                EndTime = TimeFrequency.BarEndTime(bar.OpenTime, span),
                Symbol = symbol,
                Interval = 60,
                IntervalType = BarInterval.CustomTime,
                Open = (double)bar.OpenPrice,
                High = (double)bar.HighPrice,
                Low = (double)bar.LowPrice,
                Close = (double)bar.ClosePrice,
                Volume = (double)bar.Volume,
                TradeCount = bar.TradeCount,
            };
        }
    }
}