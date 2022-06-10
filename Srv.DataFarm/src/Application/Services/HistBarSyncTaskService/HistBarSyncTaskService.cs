using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using k8s;
using UniCryptoLab.Common.Data;
using TradingLib.API;
using TradingLib.Common;
using UniCryptoLab.Common;
using UniCryptoLab.Grpc.API;


namespace UniCryptoLab.Services
{

    public interface IHistBarSyncTaskService
    {
        Entities.HistBarSyncTaskInfo AddTask(string exchange,string symbol, BarInterval type, int interval, DateTime start,
            DateTime end);

        Entities.HistBarSyncTaskInfo GetTask(string exchange, string symbol, BarInterval type, int interval);
        

        List<Entities.HistBarSyncTaskInfo> GetUnCompletedTask();

        void UpdateSyncedtime(Entities.HistBarSyncTaskInfo task, DateTime syncedTime);

        void CompleteTask(Entities.HistBarSyncTaskInfo task);

        Entities.HistBarSyncTaskInfo GetTaskById(string id);
    }
    
    public class HistBarSyncTaskService:IHistBarSyncTaskService
    {
        private NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

        public Entities.HistBarSyncTaskInfo GetTaskById(string id)
        {
            return DBFactory.Default.Get<Entities.HistBarSyncTaskInfo>().Where(e => e.Id == id).FirstOrDefault();
        }

        public List<Entities.HistBarSyncTaskInfo> GetUnCompletedTask()
        {
            return DBFactory.Default.Get<Entities.HistBarSyncTaskInfo>().Where(e => e.Completed == false).ToList();
        }
        
        public Entities.HistBarSyncTaskInfo AddTask(string exchange,string symbol, BarInterval type, int interval, DateTime start,
            DateTime end)
        {
            var task = GetTask(exchange,symbol, type, interval);
            if (task == null)
            {
                task = new Entities.HistBarSyncTaskInfo()
                {
                    Id = IDHelper.GetID(),
                    Exchange = exchange,
                    Symbol = symbol,
                    IntervalType = type,
                    Interval = interval,
                    StartTime = start,
                    EndTime = end,
                    SyncedTime = start,
                    Completed = false,
                    CreateTime = DateTime.UtcNow,
                    UpdateTime = DateTime.UtcNow,
                };
                DBFactory.Default.Add(task);
            }
            else
            {
                task.StartTime = start;
                task.EndTime = end;
                task.SyncedTime = start;
                task.Completed = false;
                task.UpdateTime = DateTime.UtcNow;
            }
            return task;
        }

        public Entities.HistBarSyncTaskInfo GetTask(string exchange,string symbol, BarInterval type, int interval)
        {
            return DBFactory.Default.Get<Entities.HistBarSyncTaskInfo>()
                .Where(e => e.Exchange==exchange && e.Symbol == symbol)
                .Where(e => e.IntervalType == type && e.Interval == interval)
                .FirstOrDefault();
        }


        public void UpdateSyncedtime(Entities.HistBarSyncTaskInfo task, DateTime syncedTime)
        {
            DBFactory.Default.Set<Entities.HistBarSyncTaskInfo>().Set(e => e.SyncedTime, syncedTime)
                .Where(e => e.Id == task.Id).Execute();
        }

        public void CompleteTask(Entities.HistBarSyncTaskInfo task)
        {
            DBFactory.Default.Set<Entities.HistBarSyncTaskInfo>().Set(e => e.Completed,true
                )
                .Where(e => e.Id == task.Id).Execute();
        }

 
    }
}