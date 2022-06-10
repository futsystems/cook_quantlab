using AutoMapper;
using TradingLib.API;
using UniCryptoLab.Models;


namespace UniCryptoLab.Services.Payment
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<IBarItem, Entities.HistBarInfo>();
            
            CreateMap<Entities.HistBarSyncTaskInfo, SyncBarTaskModel>();

        }



    }
}
