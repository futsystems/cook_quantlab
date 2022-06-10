using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using TradingLib.API;
using TradingLib.Common;
using UniCryptoLab.Entities;
using UniCryptoLab.Events;
using UniCryptoLab.Grpc.API;
using UniCryptoLab.Models;


namespace UniCryptoLab.Services
{

    public class BarSyncTaskWatcher :  BaseAsyncService
    {
        private static NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        IHistBarSyncTaskService TaskService { get; set; }

        private IHistDataStore HistDataStore { get; set; }

        public BarSyncTaskWatcher( IHistBarSyncTaskService taskService, IHistDataStore dataStore, IDomainEventSubscriber domainEventSubscriber)
        {
            this.TaskService = taskService;
            this.HistDataStore = dataStore;
            
            domainEventSubscriber.Subscribe<ReqBarSyncTaskCancelEvent>(async evt =>
            {
                taskToCancel.TryAdd(evt.Task.Id, evt);
            });
        }

       
        protected override Task[] InitializeTasks()
        {
            return new Task[]
            {
                CreateLoopTask(ProcessUnCompletedTask)
            };
        }


        private ConcurrentDictionary<string, ReqBarSyncTaskCancelEvent> taskToCancel =
            new ConcurrentDictionary<string, ReqBarSyncTaskCancelEvent>();

        async Task ProcessUnCompletedTask()
        {
            var tasks = this.TaskService.GetPendingTask();
            if (tasks.Count == 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), Cancellation);
                return;
            }

            //遍历所有已确认的Deposit 执行入账操作
            foreach (var task in tasks)
            {
                try
                {
                    var source = new BinanceBarSource();
                    var barSymbol = $"{task.Exchange}-{task.Symbol}";//BINANCE SPOT_BTC_USDT
                    
                    var info = SymbolInfo.ParseSymbol(task.Symbol);
                    info.Exchange = task.Exchange;

                    if (task.Status == EnumBarSyncTaskStatus.Pending)
                    {
                        task.Status = EnumBarSyncTaskStatus.Processing;
                        this.TaskService.ProcessTask(task);
                    }

                    //任务处于Pending 或者 Processing 状态 则执行处理
                    while (task.SyncedTime < task.EndTime && task.Status == EnumBarSyncTaskStatus.Processing)
                    {
                        try
                        {
                            var result = await source.GetHistBar(info, task.SyncedTime, task.EndTime);
                            this.HistDataStore.DeleteBar(barSymbol, task.IntervalType, task.Interval, task.SyncedTime,
                                task.EndTime);
                            foreach (var bar in result)
                            {
                                this.HistDataStore.AddBar(bar);
                            }
                            if (result.Count > 0)
                            {
                                task.SyncedTime = result.Last().EndTime;
                                this.TaskService.UpdateSyncedtime(task,task.SyncedTime);
                            }
                            else
                            {
                                //如果查询不到任何数据则 同步时间更新为结束时间 查询今天到后天的数据，则明天和后天的数据还没有产生
                                task.SyncedTime = task.EndTime;
                                this.TaskService.UpdateSyncedtime(task,task.EndTime);
                            }
                            
                            //如果已经越过了设定的结束时间则设置为任务完成
                            if (task.SyncedTime >= task.EndTime)
                            {
                                task.Status = EnumBarSyncTaskStatus.Completed;
                                this.TaskService.CompleteTask(task);
                                
                            }

                            if (taskToCancel.TryRemove(task.Id,out var evt))
                            {
                                task.Status = EnumBarSyncTaskStatus.Canceled;
                                this.TaskService.CancelTask(task, evt.Reason);
                                
                            }
                        }
                        catch (Exception e)
                        {
                            this.TaskService.TerminateTask(task,e.Message);
                            logger.Error("restore bar data error", e);
                        }
                        
                        
                
                    }
                }
                catch(Exception ex)
                {
                    logger.Error($"task :{task.Id} error:{ex}");
                   
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(5), Cancellation);
        }
    }
}
