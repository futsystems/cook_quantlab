using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using Microsoft.Extensions.Configuration;
using TradingLib.DataFeed;

namespace BinanceHander
{
    class Program
    {
        static void Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var config = new TickPubSrvConfig(); 
            configuration.Bind("TickPubSrv", config);

            TickPot tickpot = new TickPot(config.Host, config.DataSubPort);
            tickpot.Start();

            DataFeedBase datafeed = new DataFeedBinance(tickpot, config.ExchangeName, config.Host, config.MgrQryPort, config.Level);
            datafeed.Start();

            TickPortMgr tickpotmgr = new TickPortMgr(config.Host, config.MgrPubPort, config.ExchangeName);

            //注册TickPot和DataFeed
            tickpotmgr.RegisterTickPort(tickpot);
            tickpotmgr.RegisterDataFeed(datafeed);
            

            tickpotmgr.Join();

        }

        public class TickPubSrvConfig
        {
            public string ExchangeName { get; set; }
            
            public string Host { get; set; }

            
            public int DataSubPort { get; set; }

            public int MgrQryPort { get; set; }

            public int MgrPubPort { get; set; }


            public int Level { get; set; }
        }
    }
}
