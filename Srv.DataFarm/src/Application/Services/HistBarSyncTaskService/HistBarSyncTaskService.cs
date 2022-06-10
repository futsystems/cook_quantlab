using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using k8s;
using UniCryptoLab.Common.Data;
using TradingLib.API;
using TradingLib.Common;
using UniCryptoLab.Common;
using UniCryptoLab.Models;


namespace UniCryptoLab.Services
{

    public interface IHistBarSyncTaskService
    {
        Entities.HistBarSyncTaskInfo AddTask(string exchange,string symbol, BarInterval type, int interval, DateTime start,
            DateTime end);

        Entities.HistBarSyncTaskInfo GetTask(string exchange, string symbol, BarInterval type, int interval);
        

        List<Entities.HistBarSyncTaskInfo> GetPendingTask();

        void UpdateSyncedtime(Entities.HistBarSyncTaskInfo task, DateTime syncedTime);

        void ProcessTask(Entities.HistBarSyncTaskInfo task);
        
        void CompleteTask(Entities.HistBarSyncTaskInfo task);

        void CancelTask(Entities.HistBarSyncTaskInfo task,string reason);

        void TerminateTask(Entities.HistBarSyncTaskInfo task,string reason);
        
        Entities.HistBarSyncTaskInfo GetTaskById(string id);
    }

    public class HistBarSyncTaskService : IHistBarSyncTaskService
    {
        private NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

        public Entities.HistBarSyncTaskInfo GetTaskById(string id)
        {
            return DBFactory.Default.Get<Entities.HistBarSyncTaskInfo>().Where(e => e.Id == id).FirstOrDefault();
        }

        public List<Entities.HistBarSyncTaskInfo> GetPendingTask()
        {
            return DBFactory.Default.Get<Entities.HistBarSyncTaskInfo>()
                .Where(e => e.Status == EnumBarSyncTaskStatus.Pending || e.Status == EnumBarSyncTaskStatus.Processing).ToList();
        }

        public Entities.HistBarSyncTaskInfo AddTask(string exchange, string symbol, BarInterval type, int interval,
            DateTime start,
            DateTime end)
        {
            var task = GetTask(exchange, symbol, type, interval);
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
                    Status = EnumBarSyncTaskStatus.Pending,
                    CreateTime = DateTime.UtcNow,
                    UpdateTime = DateTime.UtcNow,
                };
                DBFactory.Default.Add(task);
            }
            else
            {

                if (task.Status == EnumBarSyncTaskStatus.Pending || task.Status == EnumBarSyncTaskStatus.Processing)
                {
                    throw new UniCryptoLabException("task is under processing");
                }
                
                task.StartTime = start;
                task.EndTime = end;
                task.SyncedTime = start;
                task.Status = EnumBarSyncTaskStatus.Pending;
                task.Reason = null;
                task.UpdateTime = DateTime.UtcNow;
                DBFactory.Default.Update(task);
            }

            return task;
        }



        public Entities.HistBarSyncTaskInfo GetTask(string exchange, string symbol, BarInterval type, int interval)
        {
            return DBFactory.Default.Get<Entities.HistBarSyncTaskInfo>()
                .Where(e => e.Exchange == exchange && e.Symbol == symbol)
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
            DBFactory.Default.Set<Entities.HistBarSyncTaskInfo>().Set(e => e.Status, EnumBarSyncTaskStatus.Completed)
                .Where(e => e.Id == task.Id).Execute();
        }


        public void CancelTask(Entities.HistBarSyncTaskInfo task, string reason)
        {
            DBFactory.Default.Set<Entities.HistBarSyncTaskInfo>()
                .Set(e => e.Status, EnumBarSyncTaskStatus.Canceled)
                .Set(e => e.Reason, reason)
                .Where(e => e.Id == task.Id).Execute();
        }
        
        public void ProcessTask(Entities.HistBarSyncTaskInfo task)
        {
            DBFactory.Default.Set<Entities.HistBarSyncTaskInfo>()
                .Set(e => e.Status, EnumBarSyncTaskStatus.Processing)
                .Where(e => e.Id == task.Id).Execute();
        }

        
        public void TerminateTask(Entities.HistBarSyncTaskInfo task, string reason)
        {
            DBFactory.Default.Set<Entities.HistBarSyncTaskInfo>()
                .Set(e => e.Status, EnumBarSyncTaskStatus.Terminated)
                .Set(e => e.Reason, reason)
                .Where(e => e.Id == task.Id).Execute();
        }


    }
}