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
    public interface IHistBarSource
    {
        Task<List<IBarItem>> GetHistBar(SymbolInfo info, DateTime start, DateTime end);
    }
}