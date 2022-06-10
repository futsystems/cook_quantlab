
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Autofac;
using Autofac.Extensions.DependencyInjection;


using UniCryptoLab.API;
using UniCryptoLab.Common;
using UniCryptoLab.Models;
using UniCryptoLab.Services;
using UniCryptoLab.Web.Framework;



namespace UniCryptoLab.Srv.Notify
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ILifetimeScope AutofacContainer { get; private set; }

        public IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // ConfigureServices is where you register dependencies. This gets
        // called by the runtime before the ConfigureContainer method, below.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCommon(Configuration);

            //添加健康检查
            services.AddHealth(Configuration);

            //添加grpc服务
            services.AddGrpc(options => { options.EnableDetailedErrors = true; });

            //添加认证和授权
            services.AddXHeaderAuthentication(Configuration);
            
            services.AddCustomAuthorization<Permissions>(Configuration);

            //Hangfire后台任务组件
            services.AddCustomHangfire(Configuration);
            
            //添加web api
            services.AddCustomMVC();

            //添加swager文档支持
            services.AddCustomSwagger(Configuration);
            //分区识别
            //services.AddPartionDetector();

            //用户识别
            services.AddUserDetector();

            //添加mysql数据库支持
            services.AddMySql(Configuration);
            
            //添加业务事件记录服务
            //services.AddHostedService<BusinessEventSaverService>();
            //services.AddHostedService<BarSyncTaskWatcher>();
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var assemblies = new Assembly[] { Assembly.GetExecutingAssembly() };
            //通过Autofac.Module注册服务
            builder.RegisterModule(new AutoMapperModule(assemblies));

            //注册中介者
            builder.RegisterModule(new MediatorModule(assemblies));

            //注册IObjectResolver 用于Common中的对象容器启动
            builder.RegisterModule(new ObjectResolverModule());

            //注册服务发现
            //builder.RegisterModule(new ServiceDiscoveryModule());

            //注册事件总线
            //builder.RegisterModule(new EventBusModule(Configuration));//typeof(EventLogPublisher), typeof(EventLogProcessor)););

            //注册程序所需接口
            builder.RegisterModule(new ApplicationModule());

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Configure is where you add middleware. This is called after
        // ConfigureContainer. You can use IApplicationBuilder.ApplicationServices
        // here if you need to resolve things from the container.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SystemInfo systemInfo)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            //数据库升级
            // FluentMigratorHelper.UpdateDatabase<DBVersionTable>(Configuration);
            //
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            //app.UsePartionDetector();

            app.UseCustomeSwagger();

            app.UseUserDetector();

            //app.UseApm(Configuration);

            app.UseEndpoints(endpoints =>
            {
                //添加grpc服务
                //endpoints.MapGrpcService<Grpc.API.ProductServiceImpl>();
                //endpoints.MapGrpcService<Grpc.API.HealthServiceImpl>();
                endpoints.MapControllers();
                //endpoints.MapGet("/_proto/", async ctx => await ctx.RenderProto("common"));
                endpoints.MapGet("/", async ctx => await ctx.Response.WriteAsync($"Crypto {Program.AppName} Service,Version:{Program.Version}\r\nProduct:{systemInfo.Product} Env:{systemInfo.Env}"));
                endpoints.MapGet("/info", async ctx => await ctx.Response.SuccessAsync("", ServiceNodeInfo.GetNodeInfo()));
            });

            app.UseHealth(Configuration);

            //注册grpc服务
            //app.UseConsul(Configuration);

            //注册Mysql数据库
            //app.UseMySql(Configuration);

            //注册异步事件
            //app.UseEventBus(Configuration);

            //向CMC执行事件上报
            //app.UseCMC(Configuration);

            //
            app.UseCustomHangfire(Configuration, app.ApplicationServices);
            
            //初始化ObjectContainer用于支持模块中通过ObjectContainer来获取对象
            ObjectContainer.Init(app.ApplicationServices.GetRequiredService<IObjectResolver>());
        }
    }
}