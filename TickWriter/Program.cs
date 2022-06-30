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
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var config = new TickWriterConfig(); 
            configuration.Bind("TickWriter", config);


            var path = Path.Combine(config.DataPath, "tick", "trades");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            TickRepository rep = new TickRepository(path);

            ITickFeed feed = new FastTickDataFeed(config.MasterTick, config.SlaveTick, config.DataPort, config.ReqPort);
            feed.TickEvent += (f, t) =>
            {
                //Console.WriteLine(t.ToString());
                rep.NewTick(t);
            };
            feed.Start();

            foreach (var prefix in config.TickPrefix.Split("|"))
            {
                feed.Register(prefix);
            }
            
            
            Console.WriteLine("TickWriter started");
            while (true)
            {
                Thread.Sleep(10000);
            }
            
        }
    }
}
