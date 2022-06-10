namespace UniCryptoLab.Events
{
    public class ReqBarSyncTaskCancelEvent:DomainEvent
    {

        public Entities.HistBarSyncTaskInfo Task { get; set; }

        public string Reason { get; set; }

        public ReqBarSyncTaskCancelEvent(Entities.HistBarSyncTaskInfo task,string reason)
            :base("request_bar_sync_task_cancel")
        {
            this.Task = task;
            this.Reason = reason;
        }
        
    }
}