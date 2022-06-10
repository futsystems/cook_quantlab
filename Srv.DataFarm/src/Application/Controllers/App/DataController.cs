using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingLib.API;
using UniCryptoLab.Common;
using UniCryptoLab.Common.Translations;
using UniCryptoLab.Web.Framework;

using UniCryptoLab.Models;
using UniCryptoLab.Services;
using TradingLib.Common;
using UniCryptoLab.Events;

namespace UniCryptoLab.Srv.Portal.API.App
{

    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class DataController : AppController
    {
        private IMapper Mapper { get; set; }

        private IHistBarSyncTaskService TaskService { get; set; }

        private IHistDataStore DataStore { get; set; }

        private IDomainEventPublisher DomainEventPublisher { get; set; }

        public DataController(IMapper mapper, IHistBarSyncTaskService taskService,
            IHistDataStore store,
            IDomainEventPublisher publisher)
        {
            this.Mapper = mapper;
            this.TaskService = taskService;
            this.DataStore = store;
            this.DomainEventPublisher = publisher;
        }
        
        /// <summary>
        /// 添加bar历史数据同步任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("bar/task/create")]
        public IActionResult CreateTask(ReqAddSyncBarTask req)
        {
            var task = this.TaskService.AddTask(req.Exchange, req.Symbol, BarInterval.CustomTime, 60, req.Start, req.End);
            return Json(SuccessResult(this.Mapper.Map<SyncBarTaskModel>(task)));
        }
        
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("bar/task/cancel")]
        public IActionResult CancelTask(ReqById req)
        {
            var task = this.TaskService.GetTaskById(req.Id);
            this.DomainEventPublisher.Publish(new ReqBarSyncTaskCancelEvent(task,"manual cancel"));
            return Json(SuccessResult("request is submitted"));
        }
        
        /// <summary>
        /// 查看某个任务的详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("bar/task/detail")]
        public IActionResult GetTask(ReqById req)
        {
            var task = this.TaskService.GetTaskById(req.Id);
            return Json(SuccessResult(this.Mapper.Map<SyncBarTaskModel>(task)));
        }
        

        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("bar/query")]
        public IActionResult QueryBar(QueryBar query)
        {
            var barSymbol = $"{query.Exchange}-{query.Symbol}";
            var data = this.DataStore.QueryBar(barSymbol, BarInterval.CustomTime, 60, query.Start, query.End,
                query.StartIndex,query.MaxCount);
            return Json(SuccessResult(data));
        }
    }
}