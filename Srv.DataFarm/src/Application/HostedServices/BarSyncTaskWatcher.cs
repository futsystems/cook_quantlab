using System;
using System.Linq;
using System.Threading.Tasks;
using TradingLib.API;
using TradingLib.Common;
using UniCryptoLab.Grpc.API;


namespace UniCryptoLab.Services
{

    public class BarSyncTaskWatcher :  BaseAsyncService
    {
        private static NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        IHistBarSyncTaskService TaskService { get; set; }

        private IHistDataStore HistDataStore { get; set; }

        public BarSyncTaskWatcher( IHistBarSyncTaskService taskService, IHistDataStore dataStore)
        {
            this.TaskService = taskService;
            this.HistDataStore = dataStore;
        }

       
        protected override Task[] InitializeTasks()
        {
            return new Task[]
            {
                CreateLoopTask(ProcessUnCompletedTask)
            };
        }
        

        async Task ProcessUnCompletedTask()
        {
            var tasks = this.TaskService.GetUnCompletedTask();
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
                    
                    while (task.SyncedTime < task.EndTime && task.Completed == false)
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
                                task.Completed = true;
                                this.TaskService.CompleteTask(task);
                            }
                        }
                        catch (Exception e)
                        {
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
