using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Autofac.Extensions.DependencyInjection;
using NLog.Web;

using UniCryptoLab.Web.Framework;

using SlackNet;
using SlackNet.WebApi;


namespace UniCryptoLab.Srv.Notify
{

    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Split(".").LastOrDefault();
        public static readonly string Version = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion;

        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                //using (var rtmClient = new SlackRtmClient(token))
                // { rtmClient.Connect().ConfigureAwait(false).GetAwaiter().GetResult();
                //     Console.WriteLine("Connected");
                //
                //     rtmClient.Messages
                //         .Where(m => m.Text.Contains("ping"))
                //         .Subscribe(async m =>
                //         {
                //             var user = (await api.Users.Info(m.User).ConfigureAwait(false));
                //             Console.WriteLine($"Received ping from @{user.Name}");
                //
                //             await api.Chat.PostMessage(new Message
                //             {
                //                 Channel = m.Channel,
                //                 Text = "pong",
                //                 Attachments = { new Attachment { Text = $"Count: {++count}" } }
                //             }).ConfigureAwait(false);
                //         });
                //
                //     //await rtmClient.Events;
                // }
                
                var configuration = GetConfiguration(args);
                // if (args.Length > 0 && args[0].Equals("cli", StringComparison.OrdinalIgnoreCase))
                // {
                //     CliExtensions.HandleCli<DBVersionTable>(configuration, args);
                //     return;
                // }
                CreateHostBuilder(configuration, args).Build().Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(IConfiguration configuration, string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel(options =>
                        {
                            var ports = configuration.GetServerPorts();

                            if (ports.grpcPort.HasValue)
                            {
                                options.Listen(IPAddress.Any, ports.grpcPort.Value, o => o.Protocols = HttpProtocols.Http2);
                            }
                            if (ports.httpPort.HasValue)
                            {
                                options.Listen(IPAddress.Any, ports.httpPort.Value, o => o.Protocols = HttpProtocols.Http1AndHttp2);
                            }

                        })
                        .UseKestrel()
                        .UseStartup<Startup>();
                })
                 .ConfigureLogging(logging =>
                 {
                     logging.ClearProviders();
                     logging.SetMinimumLevel(LogLevel.Trace);
                 })
                 .UseNLog();  // NLog: setup NLog for Dependency injection
        }


        private static IConfiguration GetConfiguration(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddCommandLine(args)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            return builder.Build();
        }
    }
}
