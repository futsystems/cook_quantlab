using System;
using System.Collections.Generic;
using TradingLib.API;


namespace UniCryptoLab.Services
{
    public interface IHistDataStore
    {
        List<IBarItem> QueryBar(string symbol, BarInterval type, int interval, DateTime start, DateTime end,
            int startIndex, int maxcount);

        Entities.HistBarInfo AddBar(IBarItem bar);
        
        void DeleteBar(string symbol, BarInterval type, int interval, DateTime start, DateTime end);
        
        void UpdateBar(IBarItem bar);
        
    }
}