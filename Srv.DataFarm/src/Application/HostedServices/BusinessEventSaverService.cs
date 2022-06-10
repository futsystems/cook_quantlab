//
// using System.Threading;
// using System.Threading.Tasks;
// using UniCryptoLab.Events;
// using UniCryptoLab.Models;
//
// namespace UniCryptoLab.Services
// {
//     /// <summary>
//     /// 业务日志记录
//     /// </summary>
//     public class BusinessEventSaverService : EventHostedServiceBase
//     {
//
//         IEventLogSaver EventLogSaver { get; set; }
//
//         public BusinessEventSaverService(IDomainEventSubscriber domainEventSubscriber,
//             IEventLogSaver eventLogSaver)
//             : base(domainEventSubscriber)
//         {
//             this.EventLogSaver = eventLogSaver;
//         }
//
//         protected override void SubscribeToEvents()
//         {
//             Subscribe<IPNEvent>();//IPN消息推送事件
//         }
//
//         protected override async Task ProcessEvent(object evt, CancellationToken cancellationToken)
//         {
//             await Task.Run(() =>
//             {
//                 if (evt is IEventLog eventLog)
//                 {
//                     this.EventLogSaver.SaveEventLog(eventLog);
//                 }
//             });
//         }
//     }
// }
//
