using Autofac;
using Microsoft.Extensions.Configuration;
using UniCryptoLab.Services;

namespace UniCryptoLab.Web.Framework
{

    public class ApplicationModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            //Autofac 注册Grpc客户端
            //builder.RegisterGrpcClient<UniCryptoLab.Grpc.API.Partion.PartionClient>("SrvPartionClient");
            //builder.RegisterGrpcClient<UniCryptoLab.Grpc.API.CMS.CMSClient>("SrvCMSClient");
            //builder.RegisterGrpcClient<UniCryptoLab.Grpc.API.Merchant.MerchantClient>("SrvMerchantClient");
            //builder.RegisterGrpcClient<UniCryptoLab.Grpc.API.Market.MarketClient>("SrvMarketClient");

            
            builder.RegisterType<MySQLHistDataStore>().As<IHistDataStore>().SingleInstance();
            builder.RegisterType<HistBarSyncTaskService>().As<IHistBarSyncTaskService>().SingleInstance();
        }
    }
}
