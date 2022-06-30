using System;
using System.IO;
using System.Threading;
using TradingLib.Common;
using TradingLib.DataFeed;
using Microsoft.Extensions.Configuration;

namespace TickWriter
{
    class Program
    {
        private static NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        
        static void Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var config = new TickWriterConfig(); 
            configuration.Bind("TickWriter", config);
            
            logger.Info($"Data path:{config.DataPath}");
            if (!Directory.Exists(config.DataPath))
            {
                Directory.CreateDirectory(config.DataPath);
            }
            
            TickRepository rep = new TickRepository(config.DataPath);

            ITickFeed feed = new FastTickDataFeed(config.MasterTick, config.SlaveTick, config.DataPort, config.ReqPort);
            feed.TickEvent += (f, t) =>
            {
                rep.NewTick(t);
            };
            feed.Start();

            foreach (var prefix in config.TickPrefix.Split("|"))
            {
                logger.Info($"Register preifx: {prefix}");
                feed.Register(prefix);
            }
            
            
            logger.Info("TickWriter started");
            while (true)
            {
                Thread.Sleep(10000);
            }
            
        }
    }
}
